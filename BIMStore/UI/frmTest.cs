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
    public partial class frmTest : Form
    {
        public frmTest()
        {
            InitializeComponent();
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            //Write the code to close this form
            this.Close();
        }

        tbl_products a = new tbl_products();
        productsDAL aDal = new productsDAL();
        private void frmTest_Load(object sender, EventArgs e)
        {

            //change appearance datagridview
            changeDgvAppearance();
            //Load all the Products in Data Grid View
            List<tbl_products> prod = aDal.Select();
            dgvTest.DataSource = helperDAL.convertProductsTableToView(prod);
        }

        private void dgvDeaCust_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //int variable to get the identityof row clicked
            int rowIndex = e.RowIndex;

            MessageBox.Show("RowIndex:" + rowIndex);

        }

        private void changeDgvAppearance()
        {

        }

        private void frmTest_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.X)
            {
                this.Close();
            }
        }
    }
}
