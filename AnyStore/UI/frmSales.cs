using AnyStore.BLL;
using AnyStore.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;
using static AnyStore.BLL.PDFBLL;

namespace AnyStore.UI
{
    public partial class frmSales : Form
    {
        public frmSales()
        {
            InitializeComponent();
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        transactionDAL tDAL = new transactionDAL();

        private void frmSales_Load(object sender, EventArgs e)
        {
            DateTime Today = DateTime.Now;
            dtpDateFrom.Value = new DateTime(Today.Year, Today.Month, 01);
            dtpDateTo.Value = new DateTime(Today.Year, Today.Month, DateTime.DaysInMonth(Today.Year, Today.Month));
            //LoadStatistics();
        }

        private void dtpDateFrom_ValueChanged(object sender, EventArgs e)
        {
            LoadStatistics();
        }

        private void dtpDateTo_ValueChanged(object sender, EventArgs e)
        {
            LoadStatistics();
        }

        private void LoadStatistics()
        {
            decimal? Gewinn = 0;
            decimal? offeneCustRechnungen = 0;
            decimal? offeneDeaRechnungen = 0;
            int offeneCustRechnungenAnz = 0;
            int offeneDeaRechnungenAnz = 0;
            decimal? Einnahmen = 0;
            decimal? Ausgaben = 0;
            decimal? bezahlteCustRechnungen = 0;
            decimal? bezahlteDeaRechnungen = 0;
            int bezhalteCustRechnungenAnz = 0;
            int bezhalteDeaRechnungenAnz = 0;

            DateTime DateFrom = dtpDateFrom.Value.Date;
            DateTime DateTo = dtpDateTo.Value.Date;
            TimeSpan ts = new TimeSpan(23, 59, 59);
            DateTo = DateTo.Date + ts;
            List<tbl_transactions> trans =  tDAL.SearchByDate(DateFrom,DateTo);
            dgvTransactions.DataSource = trans;

            foreach ( var tran in trans)
            {
                if ( tran.kontobez == "H" )
                {
                    if ( tran.type == "Sale" )
                    {
                        Einnahmen += tran.grandTotal;
                        Gewinn += tran.grandTotal;
                        tbl_transactions foundtrans = tDAL.SearchByGrandTotal((int)tran.dea_cust_id, (decimal)tran.grandTotal);
                        if ( foundtrans != null )
                        {
                            bezhalteCustRechnungenAnz += 1;
                            bezahlteCustRechnungen += foundtrans.grandTotal;
                        }
                        else
                        {
                            offeneCustRechnungenAnz += 1;
                            offeneCustRechnungen += tran.grandTotal;
                        }
                    } else
                    {
                        Ausgaben += tran.grandTotal;
                        Gewinn -= tran.grandTotal;
                        tbl_transactions foundtrans = tDAL.SearchByGrandTotal((int)tran.dea_cust_id, (decimal)tran.grandTotal);
                        if (foundtrans != null)
                        {
                            bezhalteDeaRechnungenAnz += 1;
                            bezahlteDeaRechnungen += foundtrans.grandTotal;
                        }
                        else
                        {
                            offeneDeaRechnungenAnz += 1;
                            offeneDeaRechnungen += tran.grandTotal;
                        }
                    }
                }
            }

            txtOutput.Text =
                "  Offene Cust Rechnungen: " + offeneCustRechnungen + " (" + offeneCustRechnungenAnz + ")" + Environment.NewLine +
                "  Offene Dea  Rechnungen: " + offeneDeaRechnungen + " (" + offeneDeaRechnungenAnz + ")" + Environment.NewLine +
                "Bezahlte Cust Rechnungen: " + bezahlteCustRechnungen + " (" + bezhalteCustRechnungenAnz + ")" + Environment.NewLine +
                " Bezahlte Dea Rechnungen: " + bezahlteDeaRechnungen + " (" + bezhalteDeaRechnungenAnz + ")" + Environment.NewLine +
                "                Einnahen: " + Einnahmen + Environment.NewLine +
                "                Ausgaben: " + Ausgaben + Environment.NewLine +
                "                  Gewinn: " + Gewinn + Environment.NewLine;


        }

    }
}
