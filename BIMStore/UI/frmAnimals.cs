﻿using BIMStore.BLL;
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

        tbl_animals a = new tbl_animals();
        animalsDAL aDal = new animalsDAL();
        private void frmAnimals_Load(object sender, EventArgs e)
        {
            check_btns();
            //Get the transactionType value from frmUserDashboard
            int custid = frmDeaCust.cust_id;
            //Set the value on lblTop
            txtCustId.Text = custid.ToString();
            List<tbl_animals> anms = aDal.SelectCustId(Convert.ToInt32(txtCustId.Text));
            dgvDeaCust.DataSource = anms;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //Get the Values from Form
            a.date_of_birth = dtpDateOfBirth.Value;
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
                List<tbl_animals> anms = aDal.SelectCustId(Convert.ToInt32(txtCustId.Text));
                dgvDeaCust.DataSource = anms;
            }
            else
            {
                //failed to insert Pet
            }
        }
        public void Clear()
        {
            txtAnimalID.Text = "";
            txtSpecies.Text = "";
            txtRace.Text = "";
            txtNotes.Text = "";
            txtSearch.Text = "";
            txtName.Text = "";
            dtpDateOfBirth.Value = DateTime.Now;
        }
        /*
        private void frmDeaCust_Load(object sender, EventArgs e)
        {
            //Refresh Data Grid View
            List<tbl_animal> anms = aDal.SelectCustId(Convert.ToInt32(txtCustId.Text));
            dgvDeaCust.DataSource = anms;
        }
        */
        private void dgvDeaCust_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //int variable to get the identityof row clicked
            int rowIndex = e.RowIndex;

            txtAnimalID.Text = dgvDeaCust.Rows[rowIndex].Cells[0].Value.ToString();
            txtName.Text = dgvDeaCust.Rows[rowIndex].Cells[1].Value.ToString();
            txtSpecies.Text = dgvDeaCust.Rows[rowIndex].Cells[2].Value.ToString();
            txtRace.Text = dgvDeaCust.Rows[rowIndex].Cells[3].Value.ToString();
            if (dgvDeaCust.Rows[rowIndex].Cells[4].Value != null)
            {
                dtpDateOfBirth.Value = Convert.ToDateTime(dgvDeaCust.Rows[rowIndex].Cells[4].Value);
            } else
            {
                dtpDateOfBirth.Value = DateTime.Now;
            }
            txtNotes.Text = dgvDeaCust.Rows[rowIndex].Cells[5].Value.ToString();

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //Get the values from Form
            a.Id = int.Parse(txtAnimalID.Text);
            a.cust_id = int.Parse(txtCustId.Text);
            a.species = txtSpecies.Text;
            a.race = txtRace.Text;
            a.notes = txtNotes.Text;
            a.name = txtName.Text;
            a.date_of_birth = dtpDateOfBirth.Value;

            //create boolean variable to check whether the Pet is updated or not
            bool success = aDal.Update(a);
            
            if(success==true)
            {
                //Dealer and Customer update Successfully
                MessageBox.Show("Pet updated Successfully");
                Clear();
                //Refresh the Data Grid View
                List<tbl_animals> anms = aDal.SelectCustId(Convert.ToInt32(txtCustId.Text));
                dgvDeaCust.DataSource = anms;
            }
            else
            {
                //Failed to udate Pet
                MessageBox.Show("Failed to Udpate Pet");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //Get the id of the user to be deleted from form
            a.Id = int.Parse(txtAnimalID.Text);

            //Create boolean variable to check wheteher the Pet is deleted or not
            bool success = aDal.Delete(a);

            if(success==true)
            {
                //Pet Deleted Successfully
                MessageBox.Show("Pet Deleted Successfully");
                Clear();
                //Refresh the Data Grid View
                List<tbl_animals> deacust = aDal.SelectCustId(Convert.ToInt32(txtCustId.Text));
                dgvDeaCust.DataSource = deacust;
            }
            else
            {
                //Pet Failed to Delete
                MessageBox.Show("Failed to Delete Pet");
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            //Get the keyowrd from text box
            string keyword = txtSearch.Text;

            if(keyword!=null)
            {
                //Search the Pet
                List<tbl_animals> deacust = aDal.Search(Convert.ToInt32(txtCustId.Text),keyword);
                dgvDeaCust.DataSource = deacust;
            }
            else
            {
                //Show all the Pet
                List<tbl_animals> deacust = aDal.SelectCustId(Convert.ToInt32(txtCustId.Text));
                dgvDeaCust.DataSource = deacust;
            }
        }

        private void txtAnimalID_TextChanged(object sender, EventArgs e)
        {
            check_btns();
        }

        public void check_btns()
        {
            helperDAL.check_buttons(txtAnimalID, btnAdd, btnDelete, btnUpdate);
        }

        private void frmAnimals_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.X)
            {
                this.Close();
            }
        }

        private void txtAge_KeyPress(object sender, KeyPressEventArgs e)
        {
            helperDAL.txtBoxCheckNumber(e, txtAge);
        }

        private void txtAge_TextChanged(object sender, EventArgs e)
        {
            int age = Convert.ToInt32(txtAge.Text);
            DateTime myDate = DateTime.Now;
            DateTime newDate = myDate.AddYears(-age);
            if (dtpDateOfBirth.Value != newDate)
            {
                dtpDateOfBirth.Value = newDate;
            }
        }

        private void dtpDateOfBirth_ValueChanged(object sender, EventArgs e)
        {
            int years = DateTime.Now.Year - dtpDateOfBirth.Value.Year;
            if (dtpDateOfBirth.Value.AddYears(years) > DateTime.Now) years--;
            //int age = CalculateYourAge(dtpDateOfBirth.Value);
            if (txtAge.Text != years.ToString())
            {
                txtAge.Text = years.ToString();
            }
        }

        public static int CalculateYourAge(DateTime Dob)
        {
            DateTime Now = DateTime.Now;
            int _Years = new DateTime(DateTime.Now.Subtract(Dob).Ticks).Year - 1;
            DateTime _DOBDateNow = Dob.AddYears(_Years);
            int _Months = 0;
            for (int i = 1; i <= 12; i++)
            {
                if (_DOBDateNow.AddMonths(i) == Now)
                {
                    _Months = i;
                    break;
                }
                else if (_DOBDateNow.AddMonths(i) >= Now)
                {
                    _Months = i - 1;
                    break;
                }
            }
            int Days = Now.Subtract(_DOBDateNow.AddMonths(_Months)).Days;

            return _Years;
        }
    }
}
