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
    public partial class frmProducts : Form
    {
        public frmProducts()
        {
            InitializeComponent();
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            //Add code to hide this form
            this.Close();
        }

        categoriesDAL cdal = new categoriesDAL();
        tbl_products p = new tbl_products();
        productsDAL pdal = new productsDAL();
        userDAL udal = new userDAL();
        private void frmProducts_Load(object sender, EventArgs e)
        {
            check_btns();
            //Creating DAta Table to hold the categories from Database
            List<tbl_categories> categoriesDT = cdal.Select();
            //Specify DataSource for Category ComboBox
            cmbCategory.DataSource = categoriesDT;
            //Specify Display Member and Value Member for Combobox
            cmbCategory.DisplayMember = "title";
            cmbCategory.ValueMember = "Id";

            //Load all the Products in Data Grid View
            List<tbl_products> prod = pdal.Select();
            dgvProducts.DataSource = helperDAL.convertProductsTableToView(prod);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //Get All the Values from Product Form
            p.name = txtName.Text;
            p.category = Convert.ToInt32(cmbCategory.SelectedValue);
            p.description = txtDescription.Text;
            p.rate = decimal.Parse(txtRate.Text);
            p.qty = 0;
            p.hasqty = chbHasQTY.Checked;
            if (chbHasQTY.Checked && txtwarningqty.Text != "")
            {
                p.warningqty = int.Parse(txtwarningqty.Text);
            } else
            {
                p.warningqty = 0;
            }
            
            p.added_date = DateTime.Now;
            //Getting username of logged in user
            String loggedUsr = frmLogin.loggedIn;
            tbl_users usr = udal.GetIDFromUsername(loggedUsr);

            p.added_by = usr.Id;

            //Create Boolean variable to check if the product is added successfully or not
            bool success = pdal.Insert(p);
            //if the product is added successfully then the value of success will be true else it will be false
            if(success==true)
            {
                //Product Inserted Successfully
                MessageBox.Show("Product Added Successfully");
                //Calling the Clear Method
                Clear();
                //Refreshing DAta Grid View
                List<tbl_products> prod = pdal.Select();
                dgvProducts.DataSource = helperDAL.convertProductsTableToView(prod);
            }
            else
            {
                //Failed to Add New product
                MessageBox.Show("Failed to Add New Product");
            }
        }
        public void Clear()
        {
            txtProductID.Text = "";
            txtName.Text = "";
            txtDescription.Text = "";
            txtRate.Text = "";
            txtSearch.Text = "";
            chbHasQTY.Checked = true;
            txtwarningqty.Text = "";
        }

        private void dgvProducts_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //Create integer variable to know which product was clicked
            int rowIndex = e.RowIndex;
            //Display the Value on Respective TextBoxes
            txtProductID.Text = dgvProducts.Rows[rowIndex].Cells[0].Value.ToString();
            txtName.Text = dgvProducts.Rows[rowIndex].Cells[1].Value.ToString();
            cmbCategory.Text = dgvProducts.Rows[rowIndex].Cells[2].Value.ToString();
            txtDescription.Text = dgvProducts.Rows[rowIndex].Cells[3].Value.ToString();
            txtRate.Text = dgvProducts.Rows[rowIndex].Cells[4].Value.ToString();
            if (dgvProducts.Rows[rowIndex].Cells[8].Value.ToString() == "True" )
            {
                chbHasQTY.Checked = true;
                txtwarningqty.Text = dgvProducts.Rows[rowIndex].Cells[9].Value.ToString();
            } else
            {
                chbHasQTY.Checked = false;
                txtwarningqty.Text = "";
            }

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //Get the Values from UI or Product Form
            p.Id = int.Parse(txtProductID.Text);
            p.name = txtName.Text;
            p.category = Convert.ToInt32(cmbCategory.SelectedValue);
            p.description = txtDescription.Text;
            p.rate = decimal.Parse(txtRate.Text);
            p.added_date = DateTime.Now;
            p.hasqty = chbHasQTY.Checked;
            if (chbHasQTY.Checked && txtwarningqty.Text != "")
            {
                p.warningqty = int.Parse(txtwarningqty.Text);
            }
            else
            {
                p.warningqty = 0;
            }
            //Getting Username of logged in user for added by
            String loggedUsr = frmLogin.loggedIn;
            tbl_users usr = udal.GetIDFromUsername(loggedUsr);

            p.added_by = usr.Id;

            //Create a boolean variable to check if the product is updated or not
            bool success = pdal.Update(p);
            //If the prouct is updated successfully then the value of success will be true else it will be false
            if(success==true)
            {
                //Product updated Successfully
                MessageBox.Show("Product Successfully Updated");
                Clear();
                //REfresh the Data Grid View
                List<tbl_products> prod = pdal.Select();
                dgvProducts.DataSource = helperDAL.convertProductsTableToView(prod);
            }
            else
            {
                //Failed to Update Product
                MessageBox.Show("Failed to Update Product");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //GEt the ID of the product to be deleted
            p.Id = int.Parse(txtProductID.Text);

            //Create Bool VAriable to Check if the product is deleted or not
            bool success = pdal.Delete(p);

            //If prouct is deleted successfully then the value of success will true else it will be false
            if(success==true)
            {
                //Product Successfuly Deleted
                MessageBox.Show("Product successfully deleted.");
                Clear();
                //Refresh DAta Grid View
                List<tbl_products> prod = pdal.Select();
                dgvProducts.DataSource = helperDAL.convertProductsTableToView(prod);
            }
            else
            {
                //Failed to Delete Product
                MessageBox.Show("Failed to Delete Product.");
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            //Get the Keywordss from Form
            string keywords = txtSearch.Text;

            if(keywords!=null)
            {
                //Search the products
                List<tbl_products> prod = pdal.Search(keywords);
                dgvProducts.DataSource = helperDAL.convertProductsTableToView(prod);
            }
            else
            {
                //Display All the products
                List<tbl_products> prod = pdal.Select();
                dgvProducts.DataSource = helperDAL.convertProductsTableToView(prod);
            }
        }

        private void txtProductID_TextChanged(object sender, EventArgs e)
        {
            check_btns();
        }

        public void check_btns()
        {
            helperDAL.check_buttons(txtProductID, btnAdd, btnDelete, btnUpdate);
        }

        private void txtRate_KeyPress(object sender, KeyPressEventArgs e)
        {
            helperDAL.txtBoxCheckDecimal(e, txtRate);
        }

        private void frmProducts_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.X)
            {
                this.Close();
            }
        }

        private void chbHasQTY_CheckedChanged(object sender, EventArgs e)
        {
            txtwarningqty.Visible = !txtwarningqty.Visible;
            lblwarningqty.Visible = !lblwarningqty.Visible;
        }
    }
}
