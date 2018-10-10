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
using static BIMStore.BLL.PDFBLL;

namespace BIMStore.UI
{
    public partial class frmTest : Form
    {
        public frmTest()
        {
            InitializeComponent();
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            //Write the code to close this form
            this.Close();
        }
        Dictionary<string, decimal> tmptaxes = new Dictionary<string, decimal>();
        tbl_products a = new tbl_products();
        productsDAL aDal = new productsDAL();
        decimal sum = 0;
        private void frmTest_Load(object sender, EventArgs e)
        {

            //change appearance datagridview
            changeDgvAppearance();
            //Load all the Products in Data Grid View
            List<tbl_products> prod = aDal.Select();
            dgvTest.DataSource = helperDAL.convertProductsTableToView(prod);
        }

        private void dgvDeaCust_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //int variable to get the identityof row clicked
            int rowIndex = e.RowIndex;

            MessageBox.Show("RowIndex:" + rowIndex);

        }

        private void changeDgvAppearance()
        {

        }

        private void frmTest_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.X)
            {
                this.Close();
            }
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
                PDF pdftest = new PDF();
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

                pdf.sum = Convert.ToDecimal(sum);
                pdf.total = Convert.ToDecimal(transaction.grandTotal);
                foreach (var value in pdf.taxes)
                {
                    pdf.total += value.Value;
                }
                pdf.discount = sum - transaction.grandTotal;

                companyuser usr = new companyuser();
                usr.first_name = "Jane";
                usr.last_name = "Doe";
                pdf.user = usr;
                //Generate PDF
                pdftest.generate(pdf, printDialog1);
                pdfsuccess = true;
            }
            return pdfsuccess;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            decimal sumTotal = 0;
            items it = new items
            {
                amount = 1,
                name = "prod 1",
                productnumber = 0,
                tax = 20,
                price = 0.5M,
                total = 0.5M
            };
            List<items> lit = new List<items>();

            for (int i = 1; i <= 50; i++)
            {
                sumTotal += it.total;
                lit.Add(it);
            }
            sum = sumTotal;
            decimal ntax = (sumTotal / 100) * 20;
            string title = "20 % Mwst";
            if (tmptaxes.ContainsKey(title))
            {
                tmptaxes[title] = tmptaxes[title] + ntax;
            }
            else
            {
                tmptaxes.Add(title, ntax);
            }

            FillPDF(new tbl_companydata
            {
                name = "CompanyName",
                slogan = "CompanySlogan",
                Id = 0,
                logo = "",
                BIC = "BYLADEM1001",
                IBAN = "DE02120300000000202051",
                contact_email = "company.mail@companymail.at",
                contact_phone = "0676 8888 12345",
                address_city = "Vienna",
                address_country = "Austria",
                address_postcode = "1210",
                address_street = "Teststraße 91/2/40"
            }, new tbl_transactions
            {
                Id = 0,
                dea_cust_id = 0,
                added_by = 0,
                transaction_date = DateTime.Now,
                type = "Sale",
                kontobez = "H",
                discount = 2,
                grandTotal = 28,
                tax = 0
            }, new tbl_dea_cust
            {
                Id = 0,
                type = "Customer",
                first_name = "John",
                last_name = "Doe",
                form_of_address = "Herr",
                contact_mail = "John.Doe@gmx.at",
                contact_phone = "0676 129 38 38",
                address_city = "Vienna",
                address_country = "Austria",
                address_postcode = "1210",
                address_street = "Customerstraße 123"
            }, lit);
        }
    }
}
