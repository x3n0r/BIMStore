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
    public partial class frmDeaCust : Form
    {
        public frmDeaCust()
        {
            InitializeComponent();
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            //Write the code to close this form
            this.Close();
        }

        DeaCustBLL dc = new DeaCustBLL();
        DeaCustDAL dcDal = new DeaCustDAL();

        public bool cameoverpets = false;
        userDAL uDal = new userDAL();
        public static int cust_id;
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtDeaCustID.Text != "") { return; }
            //Get the Values from Form
            dc.type = cmbDeaCust.Text;
            dc.first_name = txtFirstname.Text;
            dc.last_name = txtLastname.Text;
            dc.contact_mail = txtEmail.Text;
            dc.contact_phone = txtContact.Text;
            dc.address_street = txtStreet.Text;
            dc.address_city = txtCity.Text;
            dc.address_country = txtCountry.Text;
            dc.address_postcode = txtPostcode.Text;
            dc.form_of_address = txtFormofaddress.Text;
            //Getting the ID to Logged in user and passign its value in dealer or cutomer module
            string loggedUsr = frmLogin.loggedIn;
            userBLL usr = uDal.GetIDFromUsername(loggedUsr);

            //Creating boolean variable to check whether the dealer or cutomer is added or not
            int tmpdeacustid = -1;
            bool success = dcDal.Insert(dc,out tmpdeacustid);

            if(success==true)
            {
                //Dealer or Cutomer inserted successfully 
                if (cameoverpets == false)
                {
                    MessageBox.Show("Dealer or Customer Added Successfully");
                    Clear();
                } else
                {
                    txtDeaCustID.Text = tmpdeacustid.ToString();
                }
                //Refresh Data Grid View
                List<tbl_dea_cust> deacust = dcDal.Select();
                dgvDeaCust.DataSource = deacust;
            }
            else
            {
                //failed to insert dealer or customer
                MessageBox.Show("Failed to Add Dealer or Customer");
            }
            cameoverpets = false;
        }
        public void Clear()
        {
            txtDeaCustID.Text = "";
            txtFirstname.Text = "";
            txtLastname.Text = "";
            txtEmail.Text = "";
            txtContact.Text = "";
            txtStreet.Text = "";
            txtCity.Text = "";
            txtPostcode.Text = "";
            txtCountry.Text = "";
            txtSearch.Text = "";
            txtFormofaddress.Text = "";
        }

        private void frmDeaCust_Load(object sender, EventArgs e)
        {
            check_btns();
            //Refresh Data Grid View
            List<tbl_dea_cust> deacust = dcDal.Select();
            dgvDeaCust.DataSource = deacust;
            check_btnPets();
        }

        private void dgvDeaCust_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //int variable to get the identityof row clicked
            int rowIndex = e.RowIndex;

            txtDeaCustID.Text = dgvDeaCust.Rows[rowIndex].Cells[0].Value.ToString();
            cmbDeaCust.Text = dgvDeaCust.Rows[rowIndex].Cells[1].Value.ToString();
            txtFirstname.Text = dgvDeaCust.Rows[rowIndex].Cells[2].Value.ToString();
            txtLastname.Text = dgvDeaCust.Rows[rowIndex].Cells[3].Value.ToString();
            txtFormofaddress.Text = dgvDeaCust.Rows[rowIndex].Cells[4].Value.ToString();
            txtEmail.Text = dgvDeaCust.Rows[rowIndex].Cells[10].Value.ToString();
            txtContact.Text = dgvDeaCust.Rows[rowIndex].Cells[9].Value.ToString();
            txtStreet.Text = dgvDeaCust.Rows[rowIndex].Cells[5].Value.ToString();
            txtPostcode.Text = dgvDeaCust.Rows[rowIndex].Cells[6].Value.ToString();
            txtCity.Text = dgvDeaCust.Rows[rowIndex].Cells[7].Value.ToString();
            txtCountry.Text = dgvDeaCust.Rows[rowIndex].Cells[8].Value.ToString();

            check_btnPets();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //Get the values from Form
            dc.Id = int.Parse(txtDeaCustID.Text);
            dc.type = cmbDeaCust.Text;
            dc.first_name = txtFirstname.Text;
            dc.last_name = txtLastname.Text;
            dc.contact_mail = txtEmail.Text;
            dc.contact_phone = txtContact.Text;
            dc.address_street = txtStreet.Text;
            dc.address_city = txtCity.Text;
            dc.address_country = txtCountry.Text;
            dc.address_postcode = txtPostcode.Text;
            dc.form_of_address = txtFormofaddress.Text;

            //create boolean variable to check whether the dealer or customer is updated or not
            bool success = dcDal.Update(dc);
            
            if(success==true)
            {
                //Dealer and Customer update Successfully
                MessageBox.Show("Dealer or Customer updated Successfully");
                Clear();
                //Refresh the Data Grid View
                List<tbl_dea_cust> deacust = dcDal.Select();
                dgvDeaCust.DataSource = deacust;
            }
            else
            {
                //Failed to udate Dealer or Customer
                MessageBox.Show("Failed to Udpate Dealer or Customer");
            }
            check_btnPets();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //Get the id of the user to be deleted from form
            dc.Id = int.Parse(txtDeaCustID.Text);

            //Create boolean variable to check wheteher the dealer or customer is deleted or not
            bool success = dcDal.Delete(dc);

            if(success==true)
            {
                //Dealer or Customer Deleted Successfully
                MessageBox.Show("Dealer or Customer Deleted Successfully");
                Clear();
                //Refresh the Data Grid View
                List<tbl_dea_cust> deacust = dcDal.Select();
                dgvDeaCust.DataSource = deacust;
            }
            else
            {
                //Dealer or Customer Failed to Delete
                MessageBox.Show("Failed to Delete Dealer or Customer");
            }
            check_btnPets();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            //Get the keyowrd from text box
            string keyword = txtSearch.Text;

            if(keyword!=null)
            {
                //Search the Dealer or Customer
                List<tbl_dea_cust> deacust = dcDal.Search(keyword);
                dgvDeaCust.DataSource = deacust;
            }
            else
            {
                //Show all the Dealer or Customer
                List<tbl_dea_cust> deacust = dcDal.Select();
                dgvDeaCust.DataSource = deacust;
            }
        }

        private void btnPets_Click(object sender, EventArgs e)
        {
            if ( cmbDeaCust.SelectedItem.ToString() != "Customer" && (txtFirstname.Text != "" || txtLastname.Text != "") ) { return; }
            cameoverpets = true;
            btnAdd.PerformClick();
            cust_id = int.Parse(txtDeaCustID.Text);
            frmAnimals animals = new frmAnimals();
            //this.Hide();
            animals.Show();
        }

        private void cmbDeaCust_SelectedIndexChanged(object sender, EventArgs e)
        {
            check_btnPets();
        }

        private void check_btnPets()
        {
            if (cmbDeaCust.SelectedItem != null && ( cmbDeaCust.SelectedItem.ToString() == "Customer" && (txtFirstname.Text != "" || txtLastname.Text != "") ))
            {
                btnPets.Enabled = true;
            }
            else
            {
                btnPets.Enabled = false;
            }
        }

        private void txtFirstname_Leave(object sender, EventArgs e)
        {
            check_btnPets();
        }

        private void txtLastname_Leave(object sender, EventArgs e)
        {
            check_btnPets();
        }

        private void txtDeaCustID_TextChanged(object sender, EventArgs e)
        {
            check_btns();
        }

        public void check_btns()
        {
            helperDAL.check_buttons(txtDeaCustID, btnAdd, btnDelete, btnUpdate);
        }

        private void txtPostcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            helperDAL.txtBoxCheckNumber(e, txtPostcode);
        }
    }
}
