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
using static AnyStore.BLL.PDFBLL;

namespace AnyStore.UI
{
    public partial class frmTest : Form
    {
        public frmTest()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /* PDF */
            // Hier wird der Aufruf aus der GUI-Simuliert mit allen übergebenen Variablen/Parameter
            PrintDialog printDialog1 = new PrintDialog();
            DialogResult result = printDialog1.ShowDialog();
            if (result == DialogResult.OK)
            { //Prüfen, ob auf Abbrechen im Druckdialog gedrückt wurde
                PDF pdfengine = new PDF();
                PDFBLL pdf = new PDFBLL();
                //start pdf struct filling
                Dictionary<string, decimal> tmptaxes = new Dictionary<string, decimal>();
                
                //COMPANY ITEMS
                pdf.companyname = "ANyStore";
                pdf.slogan = "fucking slogan";
                //companyaddress
                company comp = new company();
                comp.address_city = "Mönchhof";
                comp.address_country = "österreich";
                comp.address_postcode = "1111";
                comp.address_street = "Baumschulgasse 1/1" ;
                comp.contact_mail = "asdf@asdf.at";
                comp.contact_phone = "+43 676 77777777";

                pdf.companyaddress = comp;

                pdf.IBAN = "at 123412341234";
                pdf.BIC = "2134";

                //customeradrress
                customer cust = new customer();
                cust.first_name = "Christopher";
                cust.last_name = "Kozlicek";
                cust.address_city = "Wien";
                cust.address_country = "österreich";
                cust.address_postcode = "1210";
                cust.address_street = "fultonstraße 26/23/3";
                cust.form_of_address = "Herr";
                pdf.customeraddress = cust;


 
                List<items> lit = new List<items>();
                //fill product listitems
                //listitems
                items it = new items();
                it.amount = 2;
                it.productnumber = 2;
                it.name = "prod1";
                it.price = 1;
                it.total = 2;
                it.tax = 10.00M;
                lit.Add(it);

                it = new items();
                it.amount = 2;
                it.productnumber = 3;
                it.name = "prod2";
                it.price = 1;
                it.total = 2;
                it.tax = 20.00M;
                lit.Add(it);



                pdf.sum = 4.00M;

                tmptaxes.Add("Mwst10%", 0.40M);
                tmptaxes.Add("Mwst20%", 0.80M);

                pdf.taxes = tmptaxes;
                pdf.total = 2.60M;
                pdf.discount = (pdf.sum + 0.40M + 0.80M) - pdf.total;

                //fill product listitems
                pdf.listitems = lit;

                pdf.invoicenumber = 1;
                //TODO
                pdf.invoicedate =
                pdf.invoicedate = new DateTime(2018, 09, 11);



                companyuser usr = new companyuser();
                usr.first_name = "veri";
                usr.last_name = "bader";
                pdf.user = usr;
                //Generate PDF
                pdfengine.generate(pdf, printDialog1);
                //Transaction Failed
            }
            else
            {
                //Transaction Failed
                MessageBox.Show("Transaction Failed");
            }
        }
    }
}
