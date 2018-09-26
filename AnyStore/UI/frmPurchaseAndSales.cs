using AnyStore.BLL;
using AnyStore.DAL;
using DGVPrinterHelper;
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
using static AnyStore.BLL.PDFBLL;

namespace AnyStore.UI
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
        userDAL uDAL = new userDAL();
        transactionDAL tDAL = new transactionDAL();
        transactionDetailDAL tdDAL = new transactionDetailDAL();

        DataTable transactionDT = new DataTable();
        private void frmPurchaseAndSales_Load(object sender, EventArgs e)
        {
            //Get the transactionType value from frmUserDashboard
            string type = frmUserDashboard.transactionType;
            if (type == "" || type == null)
            {
                type = frmAdminDashboard.transactionType;
            }
            //Set the value on lblTop
            lblTop.Text = type;

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
                txtName.Text = "";
                txtEmail.Text = "";
                txtContact.Text = "";
                txtAddress.Text = "";
                return;
            }
            DeaCustBLL dc = new DeaCustBLL();
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
            txtName.Text = dc.first_name + " " + dc.last_name;
            txtEmail.Text = dc.contact_mail;
            txtContact.Text = dc.contact_phone;
            txtAddress.Text = dc.address_street + Environment.NewLine + dc.address_postcode + " " + dc.address_city + Environment.NewLine  + dc.address_country;
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
                TxtQty.Text = "";
                return;
            }

            //Search the product and display on respective textboxes
            productsBLL p = pDAL.GetProductsForTransaction(keyword);

            //Set the values on textboxes based on p object
            txtProductName.Text = p.name;
            txtInventory.Text = p.qty.ToString();
            txtRate.Text = p.rate.ToString();
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
            if (TxtQty.Text == "" || txtRate.Text == "" || txtProductName.Text == "")
            {
                return;
            }
            string productName = txtProductName.Text;
            decimal Rate = decimal.Parse(txtRate.Text);
            decimal Qty = decimal.Parse(TxtQty.Text);
            decimal Total = Rate * Qty; //Total=RatexQty

            //Display the Subtotal in textbox
            //Get the subtotal value from textbox
            decimal subTotal = decimal.Parse(txtSubTotal.Text);
            subTotal = subTotal + Total;

            //Check whether the product is selected or not
            if(productName=="")
            {
                //Display error MEssage
                MessageBox.Show("Select the product first. Try Again.");
            }
            else
            {
                //Add product to the dAta Grid View
                transactionDT.Rows.Add(productName,Rate,Qty,Total);

                //Show in DAta Grid View
                dgvAddedProducts.DataSource = transactionDT;
                //Display the Subtotal in textbox
                txtSubTotal.Text = subTotal.ToString();

                //Clear the Textboxes
                txtSearchProduct.Text = "";
                txtProductName.Text = "";
                txtInventory.Text = "0.00";
                txtRate.Text = "0.00";
                TxtQty.Text = "0.00";
                count_prod += 1;
            }
        }

        private void txtDiscount_TextChanged(object sender, EventArgs e)
        {
            //Get the value fro discount textbox
            string value = txtDiscount.Text;

            if(value=="")
            {
                //Display Error Message
                MessageBox.Show("Please Add Discount First");
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

        private void txtVat_TextChanged(object sender, EventArgs e)
        {
            //Check if the grandTotal has value or not if it has not value then calculate the discount first
            string check = txtGrandTotal.Text;
            if(check=="")
            {
                //Deisplay the error message to calcuate discount
                MessageBox.Show("Calculate the discount and set the Grand Total First.");
            }
            else
            {
                //Calculate VAT
                //Getting the VAT Percent first
                decimal previousGT = decimal.Parse(txtGrandTotal.Text);
                decimal vat = decimal.Parse(txtVat.Text);
                decimal grandTotalWithVAT=((100+vat)/100)*previousGT;

                //Displaying new grand total with vat
                txtGrandTotal.Text = grandTotalWithVAT.ToString();
            }
        }

        private void txtPaidAmount_TextChanged(object sender, EventArgs e)
        {
            //Get the paid amount and grand total
            decimal grandTotal = decimal.Parse(txtGrandTotal.Text);
            decimal paidAmount = decimal.Parse(txtPaidAmount.Text);

            decimal returnAmount = paidAmount - grandTotal;

            //Display the return amount as well
            txtReturnAmount.Text = returnAmount.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //Get the Values from PurchaseSales Form First
            transactionsBLL transaction = new transactionsBLL();

            transaction.type = lblTop.Text;

            //Get the ID of Dealer or Customer Here
            //Lets get name of the dealer or customer first
            string deaCustName = txtName.Text;
            DeaCustBLL dc = dcDAL.GetDeaCustIDFromName(deaCustName);

            transaction.dea_cust_id = dc.Id;
            transaction.grandTotal = Math.Round(decimal.Parse(txtGrandTotal.Text),2);
            transaction.transaction_date = DateTime.Now;
            if (txtVat.Text != "")
            {
                transaction.tax = decimal.Parse(txtVat.Text);
            } else
            {
                transaction.tax = 0;
            }
            transaction.discount = decimal.Parse(txtDiscount.Text);

            //Get the Username of Logged in user
            string username = frmLogin.loggedIn;
            userBLL u = uDAL.GetIDFromUsername(username);

            transaction.added_by = u.id;
            transaction.transactionDetails = transactionDT;

            //Lets Create a Boolean Variable and set its value to false
            bool success = false;

            //Actual Code to Insert Transaction And Transaction Details
            using (TransactionScope scope = new TransactionScope())
            {
                int transactionID = -1;
                //Create aboolean value and insert transaction 
                bool w = tDAL.Insert_Transaction(transaction, out transactionID);

                List<items> lit = new List<items>();

                //Use for loop to insert Transaction Details
                for (int i=0;i<transactionDT.Rows.Count;i++)
                {
                    //Get all the details of the product
                    transactionDetailBLL transactionDetail = new transactionDetailBLL();
                    //Get the Product name and convert it to id
                    string ProductName = transactionDT.Rows[i][0].ToString();
                    productsBLL p = pDAL.GetProductIDFromName(ProductName);

                    transactionDetail.product_id = p.id;
                    transactionDetail.rate = decimal.Parse(transactionDT.Rows[i][1].ToString());
                    transactionDetail.qty = decimal.Parse(transactionDT.Rows[i][2].ToString());
                    transactionDetail.total = Math.Round(decimal.Parse(transactionDT.Rows[i][3].ToString()),2);
                    transactionDetail.dea_cust_id = dc.Id;
                    transactionDetail.added_date = DateTime.Now;
                    transactionDetail.added_by = u.id;

                    //listitems

                    items it = new items();
                    it.amount = Convert.ToInt32(transactionDetail.qty);
                    it.productnumber = transactionDetail.product_id;
                    it.name = transactionDT.Rows[i][0].ToString();
                    it.price = transactionDetail.rate;
                    it.total = 0;
                    lit.Add(it);

                    //Here Increase or Decrease Product Quantity based on Purchase or sales
                    string transactionType = lblTop.Text;

                    //Lets check whether we are on Purchase or Sales
                    bool x=false;
                    if(transactionType=="Purchase")
                    {
                        //Increase the Product
                        x = pDAL.IncreaseProduct(transactionDetail.product_id, transactionDetail.qty);
                    }
                    else if(transactionType=="Sales")
                    {
                        //Decrease the Product Quntiyt
                        x = pDAL.DecreaseProduct(transactionDetail.product_id, transactionDetail.qty);
                    }

                    //Insert Transaction Details inside the database
                    bool y = tdDAL.InsertTransactionDetail(transactionDetail);
                    success = w && x && y;
                }

                if (success == true)
                {

                    CompanyDataDAL cdDAL = new CompanyDataDAL();
                    List<tbl_companydata> cd = cdDAL.Select();

                    /* DGVPrinter*/
                    //Transaction Complete
                    scope.Complete();

                    //Code to Print Bill
                    /*
                    DGVPrinter printer = new DGVPrinter();

                    printer.Title = "\r\n\r\n\r\n ANYSTORE PVT. LTD. \r\n\r\n";
                    printer.SubTitle = "Kathmandu, Nepal \r\n Phone: 01-045XXXX \r\n\r\n";
                    printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
                    printer.PageNumbers = true;
                    printer.PageNumberInHeader = false;
                    printer.PorportionalColumns = true;
                    printer.HeaderCellAlignment = StringAlignment.Near;
                    printer.Footer = "Discount: "+ txtDiscount.Text +"% \r\n" + "VAT: " + txtVat.Text + "% \r\n" + "Grand Total: "+ txtGrandTotal.Text + "\r\n\r\n" +"Thank you for doing business with us.";
                    printer.FooterSpacing = 15;
                    printer.PrintDataGridView(dgvAddedProducts);

                    MessageBox.Show("Transaction Completed Sucessfully");
                    //Celar the Data Grid View and Clear all the TExtboxes
                    dgvAddedProducts.DataSource = null;
                    dgvAddedProducts.Rows.Clear();

                    txtSearch.Text = "";
                    txtName.Text = "";
                    txtEmail.Text = "";
                    txtContact.Text = "";
                    txtAddress.Text = "";
                    txtSearchProduct.Text = "";
                    txtProductName.Text = "";
                    txtInventory.Text = "0";
                    txtRate.Text = "0";
                    TxtQty.Text = "0";
                    txtSubTotal.Text = "0";
                    txtDiscount.Text = "0";
                    txtVat.Text = "0";
                    txtGrandTotal.Text = "0";
                    txtPaidAmount.Text = "0";
                    txtReturnAmount.Text = "0";
                    */

                    /* PDF */
                    // Hier wird der Aufruf aus der GUI-Simuliert mit allen übergebenen Variablen/Parameter
                    PrintDialog printDialog1 = new PrintDialog();
                    DialogResult result = printDialog1.ShowDialog();
                    if (result == DialogResult.OK)
                    { //Prüfen, ob auf Abbrechen im Druckdialog gedrückt wurde
                        PDF pdfengine = new PDF();
                        PDFBLL pdf = new PDFBLL();

                        //start pdf struct filling

                        //COMPANY ITEMS
                        pdf.companyname = cd[0].name;
                        pdf.slogan = cd[0].slogan;
                        //companyaddress
                        company comp = new company();
                        comp.address_city = cd[0].address_city;
                        comp.address_country = cd[0].address_country;
                        comp.address_postcode = cd[0].address_postcode;
                        comp.address_street = cd[0].address_street;
                        comp.contact_mail = cd[0].contact_email;
                        comp.contact_phone = cd[0].contact_phone;
                        
                        pdf.companyaddress = comp;

                        pdf.IBAN = cd[0].IBAN;
                        pdf.BIC = cd[0].BIC;

                        //customeradrress
                        customer cust = new customer();
                        cust.first_name = dc.first_name;
                        cust.last_name = dc.last_name;
                        cust.address_city = dc.address_city;
                        cust.address_country = dc.address_country;
                        cust.address_postcode = dc.address_postcode;
                        cust.address_street = dc.address_street;
                        pdf.customeraddress = cust;

                        //fill product listitems
                        pdf.listitems = lit;

                        pdf.invoicenumber = transactionID;
                        //TODO
                        pdf.invoicedate = 
                        pdf.invoicedate = new DateTime(transaction.transaction_date.Year, transaction.transaction_date.Month, transaction.transaction_date.Day); 

                        pdf.sum = Convert.ToDecimal(txtSubTotal.Text);
                        pdf.total = Convert.ToDecimal(txtGrandTotal.Text);
                        pdf.discount = Convert.ToDecimal(Convert.ToDecimal(txtGrandTotal.Text) - Convert.ToDecimal(txtSubTotal.Text));

                        //Generate PDF
                        pdfengine.generate(pdf, printDialog1);
                    }

                    //Transaction Failed
                    MessageBox.Show("Saved");
                }
                else
                {
                    //Transaction Failed
                    MessageBox.Show("Transaction Failed");
                }
            }
        }
    }
}
