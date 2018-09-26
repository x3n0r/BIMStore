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
using System.Windows.Forms;

namespace AnyStore.UI
{
    public partial class frmCompanyData : Form
    {
        public frmCompanyData()
        {
            InitializeComponent();
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        CompanyDataBLL cd = new CompanyDataBLL();
        CompanyDataDAL dal = new CompanyDataDAL();

        private void frmCategories_Load(object sender, EventArgs e)
        {
            //Here write the code to display all the categries in DAta Grid View
            List<tbl_companydata> dt = dal.Select();
            //dgvCategories.DataSource = dt;
            txtCompanyDataID.Text = dt[0].Id.ToString();
            txtName.Text = dt[0].name;
            txtSlogan.Text = dt[0].slogan;
            txtStreet.Text = dt[0].address_street;
            txtCity.Text = dt[0].address_city;
            txtPostcode.Text = dt[0].address_postcode;
            txtCountry.Text = dt[0].address_country;
            txtTelno.Text = dt[0].contact_phone;
            txteMail.Text = dt[0].contact_email;
            txtIBAN.Text = dt[0].IBAN;
            txtBIC.Text = dt[0].BIC;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //Get the Values from the CAtegory form
            cd.Id = int.Parse(txtCompanyDataID.Text);
            cd.name = txtName.Text;
            cd.slogan = txtSlogan.Text;
            cd.address_street = txtStreet.Text;
            cd.address_country = txtCountry.Text;
            cd.address_city = txtCity.Text;
            cd.address_postcode = txtPostcode.Text;
            cd.contact_phone = txtTelno.Text;
            cd.contact_mail = txteMail.Text;
            cd.IBAN = txtIBAN.Text;
            cd.BIC = txtBIC.Text;

            //Creating Boolean variable to update categories and check 
            bool success = dal.Update(cd);
            //If the cateory is updated successfully then the value of success will be true else it will be false
            if(success==true)
            {
                //CAtegory updated Successfully 
                MessageBox.Show("Company Data Updated Successfully");
            }
            else
            {
                //FAiled to Update Category
                MessageBox.Show("Failed to Update Company Data");
            }
        }
    }
}
