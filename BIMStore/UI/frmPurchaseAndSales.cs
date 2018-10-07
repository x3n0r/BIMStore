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
using System.Transactions;
using System.Windows.Forms;
using static BIMStore.BLL.PDFBLL;

namespace BIMStore.UI
{
    public partial class frmPurchaseAndSales : Form
    {
        public frmPurchaseAndSales()
        {
            InitializeComponent();
        }

        private int count_prod = 0;

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        DeaCustDAL dcDAL = new DeaCustDAL();
        productsDAL pDAL = new productsDAL();
        categoriesDAL cDAL = new categoriesDAL();
        userDAL uDAL = new userDAL();
        transactionDAL tDAL = new transactionDAL();
        transactionDetailDAL tdDAL = new transactionDetailDAL();

        DataTable transactionDT = new DataTable();
        Dictionary<string, decimal> tmptaxes = new Dictionary<string, decimal>();
        savebtnhelper sbtnh = new savebtnhelper();
        public string transactionType {get; set;}

        private struct savebtnhelper
        {
            public tbl_transactions transaction;
            public tbl_dea_cust dea_cust;
            public List<items> lit;
        }

        public void LoadBooking()
        {
            btnBook.Visible = true;
            btnSave.Visible = false;
            btnAdd.Enabled = false;
            txtSearch.Enabled = true;
            txtSearchProduct.Enabled = false;
            txtDiscount.Enabled = false;
            txtPaidAmount.Enabled = true;
            txtQty.Enabled = false;
        }

        public void LoadBooking(int transaction_id)
        {
            LoadBooking();
            tbl_transactions trans = tDAL.SearchByID(transaction_id);
            txtPaidAmount.Text = trans.grandTotal.ToString();
            tbl_dea_cust deacust = dcDAL.GetDeaCustIDFromID((int)trans.dea_cust_id);
            lblTop.Text = "Booking";
            FillDeaCust(deacust);
        }

        public void LoadTransaction(int transaction_id)
        {
            btnBook.Visible = false;
            btnSave.Visible = true;
            btnAdd.Enabled = false;
            txtSearch.Enabled = false;
            txtSearchProduct.Enabled = false;
            txtDiscount.Enabled = false;
            txtPaidAmount.Enabled = false;
            dtpBillDate.Enabled = false;
            txtQty.Enabled = false;
            tbl_transactions trans = tDAL.SearchByID(transaction_id);

            lblTop.Text = trans.type;
            txtDiscount.Text = trans.discount.ToString();
            txtGrandTotal.Text = trans.grandTotal.ToString();
            dtpBillDate.Value = trans.transaction_date;

            tbl_dea_cust deacust = dcDAL.GetDeaCustIDFromID((int)trans.dea_cust_id);
            FillDeaCust(deacust);

            //gettransactiondetail list
            List<tbl_transaction_detail> transdets = tdDAL.SearchByID(transaction_id);
            //foreach transactdet 
            List<items> lit = new List<items>();
            foreach ( var transdet in transdets)
            {

                if (count_prod >= 26)
                {
                    //Display error MEssage
                    MessageBox.Show("Not more than 26 Products allowed.");
                    return;
                }

                tbl_products p = pDAL.GetProductFromID((int)transdet.product_id);
                tbl_categories c = cDAL.Search((int)transdet.product_id);
                
                items it = new items();
                it.amount = Convert.ToInt32(transdet.qty);
                it.productnumber = (int)transdet.product_id;
                it.name = p.name;
                it.price = transdet.price;
                it.total = it.amount * it.price;
                it.tax = c.tax;
                lit.Add(it);

                bool cpas = false;
                cpas = CalculateProductAndShow(p.name, transdet.price, transdet.qty, c.tax);
            }

            sbtnh.dea_cust = deacust;
            sbtnh.transaction = trans;
            sbtnh.lit = lit;

        }

        private void frmPurchaseAndSale_Load(object sender, EventArgs e)
        {

            //Set the value on lblTop
            lblTop.Text = transactionType;

            //Specify Columns for our TransactionDataTable
            transactionDT.Columns.Add("Product Name");
            transactionDT.Columns.Add("Rate");
            transactionDT.Columns.Add("Quantity");
            transactionDT.Columns.Add("Total");
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            //Get the keyword fro the text box
            string keyword = txtSearch.Text;

            if (keyword == "")
            {
                //Clear all the textboxes
                txtFirstname.Text = "";
                txtLastname.Text = "";
                txtEmail.Text = "";
                txtContact.Text = "";
                txtAddress.Text = "";
                return;
            }
            tbl_dea_cust dc = new tbl_dea_cust();
            //Write the code to get the details and set the value on text boxes
            string transactionType = lblTop.Text;
            if (transactionType == "Purchase") { 
                //search dealer
                dc = dcDAL.SearchDealerCustomerForTransaction(keyword,"Dealer");
            } else { 
                //serach customer
                dc = dcDAL.SearchDealerCustomerForTransaction(keyword,"Customer");
            }

            //Now transfer or set the value from DeCustBLL to textboxes
            FillDeaCust(dc);
        }

        private void FillDeaCust(tbl_dea_cust dc)
        {
            txtFirstname.Text = dc.first_name;
            txtLastname.Text = dc.last_name;
            txtEmail.Text = dc.contact_mail;
            txtContact.Text = dc.contact_phone;
            txtAddress.Text = dc.address_street + Environment.NewLine + dc.address_postcode + " " + dc.address_city + Environment.NewLine + dc.address_country;

        }

        private void txtSearchProduct_TextChanged(object sender, EventArgs e)
        {
            //Get the keyword from productsearch textbox
            string keyword = txtSearchProduct.Text;

            //Check if we have value to txtSearchProduct or not
            if(keyword=="")
            {
                txtProductName.Text = "";
                txtInventory.Text = "";
                txtRate.Text = "";
                txtTax.Text = "";
                txtQty.Text = "";
                return;
            }

            //Search the product and display on respective textboxes
            tbl_products p = pDAL.GetProductIDFromName(keyword);

            //Set the values on textboxes based on p object
            txtProductName.Text = p.name;
            txtInventory.Text = p.qty.ToString();
            txtRate.Text = p.rate.ToString();
            tbl_categories c = cDAL.Search(p.Id);
            txtTax.Text = c.tax.ToString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (count_prod >= 26)
            {
                //Display error MEssage
                MessageBox.Show("Not more than 26 Products allowed.");
                return;
            }
            //Get Product Name, Rate and Qty customer wants to buy
            if (txtQty.Text == "" || txtRate.Text == "" || txtProductName.Text == "")
            {
                return;
            }

            bool cpas = false;
            cpas = CalculateProductAndShow(txtProductName.Text, decimal.Parse(txtRate.Text),
                decimal.Parse(txtQty.Text), Convert.ToDecimal(txtTax.Text));
            if ( cpas == true )
            {
                ClearProduct();
            }
            CalculateGrandTotal();
        }

        private bool CalculateProductAndShow(string productName, decimal Rate, decimal Qty, 
            decimal taxpercent)
        {

            bool success = false;

            decimal Total = Rate * Qty; //Total=RatexQty
            decimal subTotal = decimal.Parse(txtSubTotal.Text);
            //Display the Subtotal in textbox
            //Get the subtotal value from textbox

            subTotal = subTotal + Total;
            decimal taxcalc = 0;
            decimal.TryParse(txtTaxCalc.Text, out taxcalc);
            decimal ntax = (Total / 100) * taxpercent;
            taxcalc += ntax;
            tbl_products p = pDAL.GetProductIDFromName(productName);
            tbl_categories c = cDAL.Search((int)p.category);

            if (tmptaxes.ContainsKey(c.title))
            {
                tmptaxes[c.title] = tmptaxes[c.title] + ntax;
            }
            else
            {
                tmptaxes.Add(c.title, ntax);
            }

            //Check whether the product is selected or not
            if (productName == "")
            {
                //Display error MEssage
                MessageBox.Show("Select the product first. Try Again.");
            }
            else
            {
                //Add product to the dAta Grid View
                transactionDT.Rows.Add(productName, Rate, Qty, Total);

                //Show in DAta Grid View
                dgvAddedProducts.DataSource = transactionDT;
                //Display the Subtotal in textbox
                txtSubTotal.Text = subTotal.ToString();
                txtTaxCalc.Text = taxcalc.ToString();

                count_prod += 1;
                success = true;
            }

            return success;
        }

        private void ClearProduct()
        {

            //Clear product Textboxes
            txtSearchProduct.Text = "";
            txtProductName.Text = "";
            txtInventory.Text = "0";
            txtRate.Text = "0.00";
            txtQty.Text = "0";
            txtTax.Text = "0";
        }

        private void txtDiscount_TextChanged(object sender, EventArgs e)
        {
            CalculateGrandTotal();
        }

        private void CalculateGrandTotal ()
        {
            //Get the value fro discount textbox
            string value = txtDiscount.Text;

            if (value == "")
            {
                txtDiscount.Text = "0";
            }
            else
            {
                //Get the discount in decimal value
                decimal subTotal = decimal.Parse(txtSubTotal.Text);
                decimal discount = decimal.Parse(txtDiscount.Text);

                //Calculate the grandtotal based on discount
                decimal grandTotal = ((100 - discount) / 100) * subTotal;

                //Display the GrandTotla in TextBox
                txtGrandTotal.Text = grandTotal.ToString();
            }
        }

        private void txtPaidAmount_TextChanged(object sender, EventArgs e)
        {
            decimal paidAmount = 0;
            if (txtGrandTotal.Text == "")
            {
                return;
            }
            decimal grandTotal = decimal.Parse(txtGrandTotal.Text);
            //Get the paid amount and grand total
            if ( txtPaidAmount.Text != "")
            {
               paidAmount = decimal.Parse(txtPaidAmount.Text);
            }

            decimal returnAmount = paidAmount - grandTotal;

            //Display the return amount as well
            txtReturnAmount.Text = returnAmount.ToString();
        }


        private void btnBook_Click(object sender, EventArgs e)
        {
            string transactionType = lblTop.Text;
            if (transactionType == "Booking")
            {
                bool s = S_booking();
                if ( s )
                {
                    //Transaction Failed
                    MessageBox.Show("Transaction Saved");
                } else
                {
                    //Transaction Failed
                    MessageBox.Show("Transaction Failed");
                }
            }
            else
            {
                bool h = H_booking();
                if (h == true && txtPaidAmount.Text != "" && decimal.Parse(txtPaidAmount.Text) > 0)
                {
                    bool s = S_booking();
                }
                if (h == true)
                {
                    //Transaction Failed
                    MessageBox.Show("Transaction Saved");
                }
                else
                {
                    //Transaction Failed
                    MessageBox.Show("Transaction Failed");
                }
            }
        }

        /*ausgleichsbuchung*/
        private bool S_booking()
        {
            //Get the Values from PurchaseSale Form First
            tbl_transactions transaction = new tbl_transactions();

            //Get the ID of Dealer or Customer Here
            //Lets get name of the dealer or customer first
            string deaCustFirstName = txtFirstname.Text;
            string deaCustLastName = txtLastname.Text;
            tbl_dea_cust dc = dcDAL.GetDeaCustIDFromName(deaCustFirstName, deaCustLastName);

            transaction.type = helperDAL.DeaCustToPurchaseSale[dc.type];

            transaction.dea_cust_id = dc.Id;
            transaction.grandTotal = Math.Round(decimal.Parse(txtPaidAmount.Text), 2);
            transaction.transaction_date = DateTime.Now;
            transaction.discount = 0;

            //Get the Username of Logged in user
            string username = frmLogin.loggedIn;
            tbl_users u = uDAL.GetIDFromUsername(username);

            transaction.kontobez = "S";
            transaction.added_by = u.Id;

            bool w;
            //Actual Code to Insert Transaction And Transaction Details
            using (TransactionScope scope = new TransactionScope())
            {
                int transactionID = -1;
                //Create aboolean value and insert transaction 
                w = tDAL.Insert_Transaction(transaction, out transactionID);
                scope.Complete();
            }
            return w;
        }

        /* With transaction details */
        private bool H_booking()
        {
            //Get the Values from PurchaseSale Form First
            tbl_transactions transaction = new tbl_transactions();

            transaction.type = lblTop.Text;

            //Get the ID of Dealer or Customer Here
            //Lets get name of the dealer or customer first
            string deaCustFirstName = txtFirstname.Text;
            string deaCustLastName = txtLastname.Text;
            tbl_dea_cust dc = dcDAL.GetDeaCustIDFromName(deaCustFirstName, deaCustLastName);

            transaction.dea_cust_id = dc.Id;
            transaction.grandTotal = Math.Round(decimal.Parse(txtGrandTotal.Text), 2);
            DateTime now = DateTime.Now;
            transaction.transaction_date = new DateTime(dtpBillDate.Value.Year,dtpBillDate.Value.Month,dtpBillDate.Value.Day,now.Hour,now.Minute,now.Second);
            transaction.discount = decimal.Parse(txtDiscount.Text);

            //Get the Username of Logged in user
            string username = frmLogin.loggedIn;
            tbl_users u = uDAL.GetIDFromUsername(username);

            string transactionType = lblTop.Text;

            transaction.kontobez = "H";
            transaction.added_by = u.Id;
            //transaction.transactionDetails = transactionDT;

            //Lets Create a Boolean Variable and set its value to false
            bool success = false;


            int transactionID = -1;
            //Create aboolean value and insert transaction 
            bool w = tDAL.Insert_Transaction(transaction, out transactionID);
            transaction.Id = transactionID;

            CompanyDataDAL cdDAL = new CompanyDataDAL();
            tbl_companydata cd = cdDAL.Select();

            List<items> lit = new List<items>();

            //Use for loop to insert Transaction Details
            for (int i = 0; i < transactionDT.Rows.Count; i++)
            {
                //Get all the details of the product
                tbl_transaction_detail transactionDetail = new tbl_transaction_detail();
                //Get the Product name and convert it to id
                string ProductName = transactionDT.Rows[i][0].ToString();
                tbl_products p = pDAL.GetProductIDFromName(ProductName);
                tbl_categories c = cDAL.Search((int)p.category);

                bool producthasqty = p.hasqty;

                transactionDetail.product_id = p.Id;
                transactionDetail.price = decimal.Parse(transactionDT.Rows[i][1].ToString());
                transactionDetail.qty = decimal.Parse(transactionDT.Rows[i][2].ToString());
                transactionDetail.total = Math.Round(decimal.Parse(transactionDT.Rows[i][3].ToString()), 2);
                transactionDetail.dea_cust_id = transactionID;
                transactionDetail.added_date = DateTime.Now;
                transactionDetail.added_by = u.Id;

                //listitems
                items it = new items();
                it.amount = Convert.ToInt32(transactionDetail.qty);
                it.productnumber = (int)transactionDetail.product_id;
                it.name = transactionDT.Rows[i][0].ToString();
                it.price = transactionDetail.price;
                it.total = it.amount * it.price;
                it.tax = c.tax;
                lit.Add(it);

                //Here Increase or Decrease Product Quantity based on Purchase or sale
                //Lets check whether we are on Purchase or Sale
                bool x = false;
                if (producthasqty)
                {
                    if (transactionType == "Purchase")
                    {
                        //Increase the Product
                        x = pDAL.IncreaseProduct((int)transactionDetail.product_id, transactionDetail.qty);
                    }
                    else if (transactionType == "Sale")
                    {
                        //Decrease the Product Quntiyt
                        x = pDAL.DecreaseProduct((int)transactionDetail.product_id, transactionDetail.qty);
                    }
                }
                else
                {
                    x = true;
                }
                //Insert Transaction Details inside the database
                bool y = tdDAL.InsertTransactionDetail(transactionDetail);
                success = w && x && y;
            }

            if (success == true && transaction.type == "Sale" )
            {
                bool pdfsuccess = false;
                pdfsuccess = FillPDF(cd,transaction,dc,lit);
                if (pdfsuccess == true)
                {
                    //Transaction Failed
                    MessageBox.Show("PDF Saved");
                }
                else
                {
                    //Transaction Failed
                    MessageBox.Show("PDF Failed");
                }
            }
            return success;
        }

        private bool FillPDF(tbl_companydata cd, tbl_transactions transaction, tbl_dea_cust dc, List<items> lit)
        {

            bool pdfsuccess = false;

            /* PDF */
            // Hier wird der Aufruf aus der GUI-Simuliert mit allen übergebenen Variablen/Parameter
            PrintDialog printDialog1 = new PrintDialog();
            DialogResult result = printDialog1.ShowDialog();
            if (result == DialogResult.OK)
            { //Prüfen, ob auf Abbrechen im Druckdialog gedrückt wurde
                PDF pdfengine = new PDF();
                PDFBLL pdf = new PDFBLL();
                //start pdf struct filling

                //fill taxes struct
                pdf.taxes = tmptaxes;
                //COMPANY ITEMS
                pdf.company_name = cd.name;
                pdf.company_slogan = cd.slogan;
                pdf.company_logo = cd.logo;
                //companyaddress
                company comp = new company();
                comp.address_city = cd.address_city;
                comp.address_country = cd.address_country;
                comp.address_postcode = cd.address_postcode;
                comp.address_street = cd.address_street;
                comp.contact_mail = cd.contact_email;
                comp.contact_phone = cd.contact_phone;

                pdf.company_address = comp;

                pdf.IBAN = cd.IBAN;
                pdf.BIC = cd.BIC;

                //customeradrress
                customer cust = new customer();
                cust.first_name = dc.first_name;
                cust.last_name = dc.last_name;
                cust.address_city = dc.address_city;
                cust.address_country = dc.address_country;
                cust.address_postcode = dc.address_postcode;
                cust.address_street = dc.address_street;
                cust.form_of_address = dc.form_of_address;
                pdf.customer_address = cust;

                //fill product listitems
                pdf.items_details = lit;

                pdf.invoicenumber = transaction.Id;
                //TODO
                pdf.invoicedate = new DateTime(transaction.transaction_date.Year, transaction.transaction_date.Month, transaction.transaction_date.Day);

                pdf.sum = Convert.ToDecimal(txtSubTotal.Text);
                pdf.total = Convert.ToDecimal(txtGrandTotal.Text);
                foreach (var value in pdf.taxes)
                {
                    pdf.total += value.Value;
                }
                pdf.discount = Convert.ToDecimal(txtSubTotal.Text) - Convert.ToDecimal(txtGrandTotal.Text);

                //Get the Username of Logged in user
                string username = frmLogin.loggedIn;
                tbl_users u = uDAL.GetIDFromUsername(username);

                companyuser usr = new companyuser();
                usr.first_name = u.first_name;
                usr.last_name = u.last_name;
                pdf.user = usr;
                //Generate PDF
                pdfengine.generate(pdf, printDialog1);
                pdfsuccess = true;
            }
            return pdfsuccess;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            CompanyDataDAL cdDAL = new CompanyDataDAL();
            tbl_companydata cd = cdDAL.Select();

            bool pdfsuccess = false;
            pdfsuccess = FillPDF(cd,sbtnh.transaction, sbtnh.dea_cust, sbtnh.lit);
            if (pdfsuccess == true)
            {
                //Transaction Failed
                MessageBox.Show("Saved");
            }
            else
            {
                //Transaction Failed
                MessageBox.Show("Transaction Failed");
            }
        }

        private void txtQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            helperDAL.txtBoxCheckNumber(e, txtQty);
        }

        private void txtDiscount_KeyPress(object sender, KeyPressEventArgs e)
        {
            helperDAL.txtBoxCheckDecimal(e, txtDiscount);
        }

        private void txtPaidAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            helperDAL.txtBoxCheckDecimal(e, txtPaidAmount);
        }

        private void frmPurchaseAndSales_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.X)
            {
                this.Close();
            }
        }
    }
}
