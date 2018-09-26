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
    public partial class frmInventory : Form
    {
        public frmInventory()
        {
            InitializeComponent();
        }
        categoriesDAL cdal = new categoriesDAL();
        productsDAL pdal = new productsDAL();
        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            //Addd Functionality to Close this form
            this.Close();
        }

        private void frmInventory_Load(object sender, EventArgs e)
        {
            //Display the CAtegories in Combobox
            List<tbl_categories> cDt = cdal.Select();

            cmbCategories.DataSource = cDt;

            //Give the Value member and display member for Combobox
            cmbCategories.DisplayMember = "title";
            cmbCategories.ValueMember = "Id";

            //Display all the products in Datagrid view when the form is loaded
            List<tbl_products> prod = pdal.Select();
            dgvProducts.DataSource = prod;
        }
        
        private void cmbCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Display all the Products Based on Selected CAtegory
            /*
            MessageBox.Show("asdf: "+ cmbCategories);
            int category = Convert.ToInt32(cmbCategories.SelectedValue);


            List<tbl_products> prod = pdal.DisplayProductsByCategory(category);
            dgvProducts.DataSource = prod;
            */
        }
        
        private void btnAll_Click(object sender, EventArgs e)
        {
            //Display all the productswhen this button is clicked
            List<tbl_products> prod = pdal.Select();
            dgvProducts.DataSource = prod;
        }
    }
}
