﻿using BIMStore.BLL;
using BIMStore.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BIMStore.UI
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

        tbl_companydata cd = new tbl_companydata();
        CompanyDataDAL dal = new CompanyDataDAL();

        private void frmCompanyData_Load(object sender, EventArgs e)
        {
            //Here write the code to display all the categries in DAta Grid View
            tbl_companydata dt = dal.Select();
            if (dt != null)
            {
                //dgvCategories.DataSource = dt;
                txtCompanyDataID.Text = dt.Id.ToString();
                txtName.Text = dt.name;
                txtSlogan.Text = dt.slogan;
                txtStreet.Text = dt.address_street;
                txtCity.Text = dt.address_city;
                txtPostcode.Text = dt.address_postcode;
                txtCountry.Text = dt.address_country;
                txtTelno.Text = dt.contact_phone;
                txteMail.Text = dt.contact_email;
                txtIBAN.Text = dt.IBAN;
                txtLogo.Text = dt.logo;
                if (txtLogo.Text != "")
                {
                    LoadImage(txtLogo.Text);
                }
                txtBIC.Text = dt.BIC;
            } else
            {
                btnAdd.Visible = true;
                btnUpdate.Visible = false;
            }
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
            cd.contact_email = txteMail.Text;
            cd.IBAN = txtIBAN.Text;
            cd.logo = txtLogo.Text;
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

        private void btnLoadLogo_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Open Image";
            dlg.Filter = "Image Files (*.bmp;*.jpg;*.jpeg,*.png)|*.BMP;*.JPG;*.JPEG;*.PNG";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                LoadImage(dlg.FileName);
                txtLogo.Text = dlg.FileName;
            }
            dlg.Dispose();
        }

        private void LoadImage(String Filename)
        {
            FileInfo file = new FileInfo(Filename);
            if (file.Exists)
            {
                pbLogo.Image = Image.FromFile(Filename);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            txtLogo.Text = "";
            pbLogo.Image = null;
        }

        private void txtPostcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            helperDAL.txtBoxCheckNumber(e, txtPostcode);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            tbl_companydata dt = new tbl_companydata();
            dt.address_city = txtCity.Text;
            dt.address_country = txtCountry.Text;
            dt.address_postcode = txtPostcode.Text;
            dt.address_street = txtStreet.Text;
            dt.BIC = txtBIC.Text;
            dt.contact_email = txteMail.Text;
            dt.contact_phone = txtTelno.Text;
            dt.IBAN= txtIBAN.Text;
            dt.logo = txtLogo.Text;
            dt.name = txtName.Text;
            dt.slogan = txtSlogan.Text;
            bool success = dal.Insert(dt);
            if (success == true)
            {
                //CAtegory updated Successfully 
                MessageBox.Show("Company Data Added Successfully");
                btnAdd.Visible = false;
                btnUpdate.Visible = true;
            }
            else
            {
                //FAiled to Update Category
                MessageBox.Show("Failed to Add Company Data");
            }
        }

        private void frmCompanyData_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.X)
            {
                this.Close();
            }
        }
    }
}
