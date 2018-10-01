using AnyStore.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnyStore.UI
{
    public partial class frmTransactions : Form
    {
        public frmTransactions()
        {
            InitializeComponent();
        }

        transactionDAL tdal = new transactionDAL();
        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmTransactions_Load(object sender, EventArgs e)
        {
            //Dispay all the transactions
            List<tbl_transactions> trans = tdal.DisplayAllTransactions();
            dgvTransactions.DataSource = trans;
        }

        private void cmbTransactionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Get the Value from Combobox
            string type = cmbTransactionType.Text;

            List<tbl_transactions> trans = tdal.DisplayTransactionByType(type);
            dgvTransactions.DataSource = trans;
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            //Dispay all the transactions
            List<tbl_transactions> trans = tdal.DisplayAllTransactions();
            dgvTransactions.DataSource = trans;
        }

        private void dgvTransactions_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //int variable to get the identityof row clicked
            int rowIndex = e.RowIndex;
            //0=id 1=type 2=
            string kontobez = dgvTransactions.Rows[rowIndex].Cells[8].Value.ToString();
            if ( kontobez != "H" ) { return; }
            frmPurchaseAndSale purchase = new frmPurchaseAndSale();
            purchase.transactionType = dgvTransactions.Rows[rowIndex].Cells[1].Value.ToString();
            purchase.Show();
            purchase.LoadTransaction(Convert.ToInt32(dgvTransactions.Rows[rowIndex].Cells[0].Value));
        }
    }
}
