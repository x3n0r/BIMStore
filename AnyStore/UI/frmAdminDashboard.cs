using AnyStore.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnyStore
{
    public partial class frmAdminDashboard : Form
    {
        public frmAdminDashboard()
        {
            InitializeComponent();
        }

        //Set a public static method to specify whether the form is purchase or sale
        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUsers user = new frmUsers();
            user.Show();
        }

        private void frmAdminDashboard_FormClosed(object sender, FormClosedEventArgs e)
        {
            frmLogin login = new frmLogin();
            login.Show();
            this.Hide();
        }

        private void frmAdminDashboard_Load(object sender, EventArgs e)
        {
            lblLoggedInUser.Text = frmLogin.loggedIn;
        }

        private void categoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCategories category = new frmCategories();
            category.Show();
        }

        private void productsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmProducts product = new frmProducts();
            product.Show();
        }

        private void deealerAndCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDeaCust DeaCust = new frmDeaCust();
            DeaCust.Show();
        }

        private void transactionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmTransactions transaction = new frmTransactions();
            transaction.Show();
        }

        private void inventoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmInventory inventory = new frmInventory();
            inventory.Show();
        }

        private void companydataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCompanyData companydata = new frmCompanyData();
            companydata.Show();
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmTest test = new frmTest();
            test.Show();
        }

        private void rectangleShape1_Click(object sender, EventArgs e)
        {
            frmCompanyData companydata = new frmCompanyData();
            companydata.Show();
        }


        private void lblbtnCompanyData_Click(object sender, EventArgs e)
        {
            frmCompanyData companydata = new frmCompanyData();
            companydata.Show();
        }

        private void purchaseToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //set value on transactionType static method
            frmPurchaseAndSales purchase = new frmPurchaseAndSales();
            purchase.transactionType = "Purchase";
            purchase.Show();
        }

        private void saleToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //Set the value to transacionType method to sale
            frmPurchaseAndSales sale = new frmPurchaseAndSales();
            sale.transactionType = "Sale";
            sale.Show();
        }

        private void bookingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Set the value to transacionType method to sale
            frmPurchaseAndSales sale = new frmPurchaseAndSales();
            sale.transactionType = "Booking";
            sale.LoadBooking();
            sale.Show();
        }

        private void salesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSales sales = new frmSales();
            sales.Show();
        }
    }
}
