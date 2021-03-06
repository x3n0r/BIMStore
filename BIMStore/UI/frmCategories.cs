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
    public partial class frmCategories : Form
    {
        public frmCategories()
        {
            InitializeComponent();
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        tbl_categories c = new tbl_categories();
        categoriesDAL dal = new categoriesDAL();
        userDAL udal = new userDAL();

        private void btnADD_Click(object sender, EventArgs e)
        {
            if (txtDescription.Text == "" && txtTitle.Text == "")
            {
                return;
            }
            //Get the values from Categroy Form
            c.title = txtTitle.Text;
            c.description = txtDescription.Text;
            c.added_date = DateTime.Now;
            c.tax = Convert.ToDecimal(txtTax.Text);

            //Getting ID in Added by field
            string loggedUser = frmLogin.loggedIn;
            tbl_users usr = udal.GetIDFromUsername(loggedUser);
            //Passign the id of Logged in User in added by field
            c.added_by = usr.Id;

            //Creating Boolean Method To insert data into database
            bool success = dal.Insert(c);

            //If the category is inserted successfully then the value of the success will be true else it will be false
            if(success==true)
            {
                //NewCAtegory Inserted Successfully
                MessageBox.Show("New Category Inserted Successfully.");
                Clear();
                //Refresh Data Grid View
                List<tbl_categories> cats = dal.Select();
                dgvCategories.DataSource = cats;
            }
            else
            {
                //FAiled to Insert New Category
                MessageBox.Show("Failed to Insert New CAtegory.");
            }
        }
        public void Clear()
        {
            txtCategoryID.Text = "";
            txtTitle.Text = "";
            txtDescription.Text = "";
            txtSearch.Text = "";
            txtTax.Text = "";
        }

        private void frmCategories_Load(object sender, EventArgs e)
        {
            check_btns();
            //Here write the code to display all the categries in DAta Grid View
            List<tbl_categories> dt = dal.Select();
            dgvCategories.DataSource = dt;
        }

        private void dgvCategories_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //Finding the Row Index of the Row Clicked on Data Grid View
            int RowIndex = e.RowIndex;
            txtCategoryID.Text = dgvCategories.Rows[RowIndex].Cells[0].Value.ToString();
            txtTitle.Text = dgvCategories.Rows[RowIndex].Cells[1].Value.ToString();
            txtDescription.Text = dgvCategories.Rows[RowIndex].Cells[2].Value.ToString();
            txtTax.Text = dgvCategories.Rows[RowIndex].Cells[5].Value.ToString();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtCategoryID.Text == "")
            {
                return;
            }
            //Get the Values from the CAtegory form
            c.Id = int.Parse(txtCategoryID.Text);
            c.title = txtTitle.Text;
            c.description = txtDescription.Text;
            c.added_date = DateTime.Now;
            c.tax = Convert.ToDecimal(txtTax.Text);
            //Getting ID in Added by field
            string loggedUser = frmLogin.loggedIn;
            tbl_users usr = udal.GetIDFromUsername(loggedUser);
            //Passign the id of Logged in User in added by field
            c.added_by = usr.Id;

            //Creating Boolean variable to update categories and check 
            bool success = dal.Update(c);
            //If the cateory is updated successfully then the value of success will be true else it will be false
            if(success==true)
            {
                //CAtegory updated Successfully 
                MessageBox.Show("Category Updated Successfully");
                Clear();
                //Refresh Data Gid View
                List<tbl_categories> cats = dal.Select();
                dgvCategories.DataSource = cats;
            }
            else
            {
                //FAiled to Update Category
                MessageBox.Show("Failed to Update Category");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtCategoryID.Text == "")
            {
                return;
            }
            //Get te ID of the Category Which we want to Delete
            c.Id = int.Parse(txtCategoryID.Text);

            //Creating Boolean Variable to Delete The CAtegory
            bool success = dal.Delete(c);

            //If the CAtegory id Deleted Successfully then the vaue of success will be true else it will be false
            if(success==true)
            {
                //Category Deleted Successfully
                MessageBox.Show("Category Deleted Successfully");
                Clear();
                //REfreshing DAta Grid View
                List<tbl_categories> cats = dal.Select();
                dgvCategories.DataSource = cats;
            }
            else
            {
                //FAiled to Delete CAtegory 
                MessageBox.Show("Failed to Delete CAtegory");
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            //Get the Keywords
            string keywords = txtSearch.Text;

            //Filte the categories based on keywords
            if(keywords!=null)
            {
                //Use Searh Method To Display Categoreis
                List<tbl_categories> cats = dal.Search(keywords);
                dgvCategories.DataSource = cats;
            }
            else
            {
                //Use Select Method to Display All Categories
                List<tbl_categories> cats = dal.Select();
                dgvCategories.DataSource = cats;
            }
        }

        private void txtCategoryID_TextChanged(object sender, EventArgs e)
        {
            check_btns();
        }

        public void check_btns()
        {
            helperDAL.check_buttons(txtCategoryID, btnAdd, btnDelete, btnUpdate);
        }

        private void txtTax_KeyPress(object sender, KeyPressEventArgs e)
        {
            helperDAL.txtBoxCheckDecimal(e, txtTax);
        }

        private void frmCategories_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.X)
            {
                this.Close();
            }
        }
    }
}
