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
    public partial class frmUsers : Form
    {
        public frmUsers()
        {
            InitializeComponent();
        }

        tbl_users u = new tbl_users();
        userDAL dal = new userDAL();
        bool firstlogin = false;

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void FirstLogin()
        {
            cmbUserType.Text = "Admin";
            MessageBox.Show("This is your first Login, please add an Admin-User first."+ Environment.NewLine +
                            "After adding Program will be closed");
            firstlogin = true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            

            //Gettting Data FRom UI
            u.first_name = txtFirstName.Text;
            u.last_name = txtLastName.Text;
            u.contact_email = txtEmail.Text;
            u.username = txtUsername.Text;
            u.password = txtPassword.Text;
            u.contact_phone = txtContact.Text;
            u.address = txtAddress.Text;
            u.gender = cmbGender.Text;
            u.user_type = cmbUserType.Text;
            u.added_date = DateTime.Now;

            //Getting Username of the logged in user
            string loggedUser = frmLogin.loggedIn;
            tbl_users usr = dal.GetIDFromUsername(loggedUser);

            u.added_by = usr.Id;

            //Inserting Data into DAtabase
            bool success = dal.Insert(u);
            //If the data is successfully inserted then the value of success will be true else it will be false
            if(success==true)
            {
                //Data Successfully Inserted
                MessageBox.Show("User successfully created.");
                clear();
            }
            else
            {
                //Failed to insert data
                MessageBox.Show("Failed to add new user");
            }
            //Refreshing Data Grid View
            List<tbl_users> users = dal.Select();
            dgvUsers.DataSource = users;

            if (firstlogin)
            {
                this.Close();
                Environment.Exit(0);
            }
        }

        private void frmUsers_Load(object sender, EventArgs e)
        {
            check_btns();
            List<tbl_users> users = dal.Select();
            dgvUsers.DataSource = users;
        }
        private void clear()
        {
            txtUserID.Text = "";
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtEmail.Text = "";
            txtUsername.Text = "";
            txtPassword.Text = "";
            txtContact.Text = "";
            txtAddress.Text = "";
            cmbGender.Text = "";
            cmbUserType.Text = "";
        }

        private void dgvUsers_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //Get the index of particular row
            int rowIndex = e.RowIndex;
            txtUserID.Text = dgvUsers.Rows[rowIndex].Cells[0].Value.ToString();
            txtFirstName.Text = dgvUsers.Rows[rowIndex].Cells[1].Value.ToString();
            txtLastName.Text = dgvUsers.Rows[rowIndex].Cells[2].Value.ToString();
            txtEmail.Text = dgvUsers.Rows[rowIndex].Cells[3].Value.ToString();
            txtUsername.Text = dgvUsers.Rows[rowIndex].Cells[4].Value.ToString();
            txtPassword.Text = dgvUsers.Rows[rowIndex].Cells[5].Value.ToString();
            txtContact.Text = dgvUsers.Rows[rowIndex].Cells[6].Value.ToString();
            txtAddress.Text = dgvUsers.Rows[rowIndex].Cells[7].Value.ToString();
            cmbGender.Text = dgvUsers.Rows[rowIndex].Cells[8].Value.ToString();
            cmbUserType.Text = dgvUsers.Rows[rowIndex].Cells[9].Value.ToString();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //Get the values from User UI
            //u.id = Convert.ToInt32(txtUserID.Text);
            u.first_name = txtFirstName.Text;
            u.last_name = txtLastName.Text;
            u.contact_email = txtEmail.Text;
            u.username = txtUsername.Text;
            u.password = txtPassword.Text;
            u.contact_phone = txtContact.Text;
            u.address = txtAddress.Text;
            u.gender = cmbGender.Text;
            u.user_type = cmbUserType.Text;
            u.added_date = DateTime.Now;
            u.added_by = 1;

            //Updating Data into database
            bool success = dal.Update(u);
            //if data is updated successfully then the value of success will be true else it will be false
            if(success==true)
            {
                //Data Updated Successfully
                MessageBox.Show("User successfully updated");
                clear();
            }
            else
            {
                //failed to update user
                MessageBox.Show("Failed to update user");
            }
            //Refreshing Data Grid View
            List<tbl_users> users = dal.Select();
            dgvUsers.DataSource = users;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //Getting User ID from Form
            u.Id = Convert.ToInt32(txtUserID.Text);

            bool success = dal.Delete(u);
            //if data is deleted then the value of success will be true else it will be false
            if(success==true)
            {
                //User Deleted Successfully 
                MessageBox.Show("User deleted successfully");
                clear();
            }
            else
            {
                //Failed to Delete User
                MessageBox.Show("Failed to delete user");

            }
            //refreshing Datagrid view
            List<tbl_users> users = dal.Select();
            dgvUsers.DataSource = users;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            //Get Keyword from Text box
            string keywords = txtSearch.Text;

            //Chec if the keywords has value or not
            if(keywords!=null)
            {
                //Show user based on keywords
                List<tbl_users> users = dal.Search(keywords);
                dgvUsers.DataSource = users;
            }
            else
            {
                //show all users from the database
                List<tbl_users> users = dal.Select();
                dgvUsers.DataSource = users;
            }
        }

        private void txtUserID_TextChanged(object sender, EventArgs e)
        {
            check_btns();
        }

        public void check_btns()
        {
            helperDAL.check_buttons(txtUserID, btnAdd, btnDelete, btnUpdate);
        }

        private void frmUsers_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.X)
            {
                this.Close();
            }
        }
    }
}
