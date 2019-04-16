using BIMStore.BLL;
using BIMStore.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BIMStore.UI
{

    public partial class frmTransactions : Form
    {
        public frmTransactions()
        {
            InitializeComponent();
        }

        transactionDAL tdal = new transactionDAL();
        DeaCustDAL dcDAL = new DeaCustDAL();
        int currentMouseOverRow = 0;

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmTransactions_Load(object sender, EventArgs e)
        {
            //Dispay all the transactions
            List<tbl_transactions> trans = tdal.DisplayAllTransactions();
            dgvTransactions.DataSource = helperDAL.convertTransactionsTableToView(trans);

        }

        private void cmbTransactionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetTransactionByType();
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            //Dispay all the transactions
            List<tbl_transactions> trans = tdal.DisplayAllTransactions();
            dgvTransactions.DataSource = trans;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            //Get the keyword fro the text box
            string keyword = txtSearch.Text;

            if (keyword == "")
            {
                GetTransactionByType();
                ClearDeaCust();
                return;
            }
            List<tbl_dea_cust> dc = new List<tbl_dea_cust>();
            //Write the code to get the details and set the value on text boxes
            dc = dcDAL.Search(keyword);

            //Now transfer or set the value from DeCustBLL to textboxes
            if (dc.Count() > 0)
            {
                FillDeaCust(dc[0]);
            }
        }

        private void ClearDeaCust()
        {
            //Clear all the textboxes
            txtFirstname.Text = "";
            txtLastname.Text = "";
            txtEmail.Text = "";
            txtContact.Text = "";
            txtAddress.Text = "";
        }

        private void FillDeaCust(tbl_dea_cust dc)
        {
            txtFirstname.Text = dc.first_name;
            txtLastname.Text = dc.last_name;
            txtEmail.Text = dc.contact_mail;
            txtContact.Text = dc.contact_phone;
            txtAddress.Text = dc.address_street + Environment.NewLine + dc.address_postcode + " " + dc.address_city + Environment.NewLine + dc.address_country;

            //set combobox to value
            cmbTransactionType.SelectedItem = helperDAL.DeaCustToPurchaseSale[dc.type];

            List<tbl_transactions> trans = tdal.SearchByDeaCust(dc.Id);
            dgvTransactions.DataSource = trans;
        }

        private void GetTransactionByType ()
        {
            //Get the Value from Combobox
            string type = cmbTransactionType.Text;

            List<tbl_transactions> trans = tdal.DisplayTransactionByType(type);
            dgvTransactions.DataSource = trans;
        }

        private void dgvTransactions_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //int variable to get the identityof row clicked
            int rowIndex = e.RowIndex;
            //0=id 1=type 2=
            string kontobez = dgvTransactions.Rows[rowIndex].Cells[8].Value.ToString();
            if (kontobez != "H") { return; }
            LoadBill(rowIndex);

        }

        private void LoadBill(int rowIndex)
        {
            frmPurchaseAndSales purchase = new frmPurchaseAndSales();
            purchase.transactionType = dgvTransactions.Rows[rowIndex].Cells[1].Value.ToString();
            purchase.Show();
            purchase.LoadTransaction(Convert.ToInt32(dgvTransactions.Rows[rowIndex].Cells[0].Value));
        }

        private void dgvTransactions_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                currentMouseOverRow = dgvTransactions.HitTest(e.X, e.Y).RowIndex;
                String kontobez = dgvTransactions.Rows[currentMouseOverRow].Cells[8].Value.ToString();
                if (kontobez == "H")
                {
                    ContextMenu m = new ContextMenu();
                    m.MenuItems.Add(new MenuItem("Pay Booking", PayBooking_Click)); // muss H sein
                    m.MenuItems.Add(new MenuItem("Show Bill", ShowBill_Click)); // muss H sein

                    /*
                    if (currentMouseOverRow >= 0)
                    {
                        m.MenuItems.Add(new MenuItem(string.Format("Do something to row {0}", currentMouseOverRow.ToString())));
                    }*/

                    m.Show(dgvTransactions, new Point(e.X, e.Y));
                } else
                {
                    ContextMenu m = new ContextMenu();
                    m.MenuItems.Add(new MenuItem("Delete Payment", DeletePayment_Click)); // muss S sein

                    /*
                    if (currentMouseOverRow >= 0)
                    {
                        m.MenuItems.Add(new MenuItem(string.Format("Do something to row {0}", currentMouseOverRow.ToString())));
                    }*/

                    m.Show(dgvTransactions, new Point(e.X, e.Y));
                }
            }
        }

        private void PayBooking_Click(Object sender, System.EventArgs e)
        {
            LoadPayBooking(currentMouseOverRow);
        }
        private void DeletePayment_Click(Object sender, System.EventArgs e)
        {
            int rowIndex = currentMouseOverRow;
            int transID = Convert.ToInt32(dgvTransactions.Rows[rowIndex].Cells[0].Value);
            bool w = tdal.Delete(transID);
            if (w)
            {
                //Transaction Failed
                MessageBox.Show("Deleted");
            }
            else
            {
                //Transaction Failed
                MessageBox.Show("Transaction Failed");
            }
            if (txtSearch.Text != "")
            {
                txtSearch_TextChanged(txtSearch,EventArgs.Empty);
            } else
            {
                if (cmbTransactionType.SelectedItem != null)
                {
                    cmbTransactionType_SelectedIndexChanged(cmbTransactionType, EventArgs.Empty);
                } else
                {
                    btnAll.PerformClick();
                }
            }
        }
        private void ShowBill_Click(Object sender, System.EventArgs e)
        {
            //int currentMouseOverRow = dgvTransactions.HitTest(e.X, e.Y).RowIndex;
            LoadBill(currentMouseOverRow);
        }

        private void LoadPayBooking(int rowIndex)
        {
            frmPurchaseAndSales purchase = new frmPurchaseAndSales();
            purchase.transactionType = dgvTransactions.Rows[rowIndex].Cells[1].Value.ToString();
            purchase.Show();
            purchase.LoadBooking(Convert.ToInt32(dgvTransactions.Rows[rowIndex].Cells[0].Value));
        }

        private void frmTransactions_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.X)
            {
                this.Close();
            }
        }
    }
}
