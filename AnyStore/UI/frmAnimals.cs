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
    public partial class frmAnimals : Form
    {
        public frmAnimals()
        {
            InitializeComponent();
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            //Write the code to close this form
            this.Close();
        }

        animalsBLL a = new animalsBLL();
        animalsDAL aDal = new animalsDAL();
        private void frmAnimals_Load(object sender, EventArgs e)
        {
            //Get the transactionType value from frmUserDashboard
            int custid = frmDeaCust.cust_id;
            //Set the value on lblTop
            txtCustId.Text = custid.ToString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //Get the Values from Form
            a.date_of_birth = dtpDateOfBirth.Value.ToShortDateString();
            a.cust_id = Convert.ToInt32(txtCustId.Text);
            a.name = txtName.Text;
            a.notes = txtNotes.Text;
            a.race = txtRace.Text;
            a.species = txtSpecies.Text;

            //Getting the ID to Logged in user and passign its value in dealer or cutomer module
            string loggedUsr = frmLogin.loggedIn;

            //Creating boolean variable to check whether the dealer or cutomer is added or not
            bool success = aDal.Insert(a);

            if(success==true)
            {
                //Dealer or Cutomer inserted successfully 
                MessageBox.Show("Pet Added Successfully");
                Clear();
                //Refresh Data Grid View
                List<tbl_animal> anms = aDal.SelectCustId(Convert.ToInt32(txtCustId.Text));
                dgvDeaCust.DataSource = anms;
            }
            else
            {
                //failed to insert dealer or customer
            }
        }
        public void Clear()
        {
            txtDeaCustID.Text = "";
            txtSpecies.Text = "";
            txtRace.Text = "";
            txtNotes.Text = "";
            txtSearch.Text = "";
            txtName.Text = "";
        }

        private void frmDeaCust_Load(object sender, EventArgs e)
        {
            //Refresh Data Grid View
            List<tbl_animal> anms = aDal.SelectCustId(Convert.ToInt32(txtCustId.Text));
            dgvDeaCust.DataSource = anms;
        }

        private void dgvDeaCust_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //int variable to get the identityof row clicked
            int rowIndex = e.RowIndex;

            txtDeaCustID.Text = dgvDeaCust.Rows[rowIndex].Cells[0].Value.ToString();
            txtName.Text = dgvDeaCust.Rows[rowIndex].Cells[1].Value.ToString();
            txtSpecies.Text = dgvDeaCust.Rows[rowIndex].Cells[2].Value.ToString();
            txtRace.Text = dgvDeaCust.Rows[rowIndex].Cells[3].Value.ToString();
            dtpDateOfBirth
            txtNotes.Text = dgvDeaCust.Rows[rowIndex].Cells[5].Value.ToString();

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //Get the values from Form
            dc.Id = int.Parse(txtDeaCustID.Text);
            dc.type = cmbDeaCust.Text;
            dc.first_name = txtSpecies.Text;
            dc.last_name = txtRace.Text;
            dc.contact_mail = txtNotes.Text;
            dc.contact_phone = txtContact.Text;
            dc.address_street = txtStreet.Text;
            dc.address_city = txtCity.Text;
            dc.address_country = txtCountry.Text;
            dc.address_postcode = txtPostcode.Text;
            dc.form_of_address = txtName.Text;

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

    }
}
