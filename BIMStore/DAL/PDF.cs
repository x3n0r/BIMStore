//Diese Klasse wurde von P Lehr (https://github.com/plehr) im Rahmen des Projektes "stnr" erstellt.
// no licence for PDF.cs in project found therefore used

// 39 = exactly 1 page full
// 33 = sum block on first page // 34 = sum block on second page
// 26 = TransferRequest on first page // 27 = transfer request on second page

using BIMStore.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BIMStore.DAL
{
    class PDF
    {
        private String Firmenname;
        private String Firmenslogan;
        private String Firmenstr;
        private int Firmenplz;
        private String Firmenort;
        private String Anrede;
        private String Vorname;
        private String Nachname;
        private String Straße;
        private String Postleitzahl;
        private String Ort;
        private PrintDialog dialog;
        private int Rechnungsnr;
        private DateTime Rechnungsdatum;
        private DataTable tabelle = new DataTable();
        private String Mailaddr;
        private String Sachbearbeiter;
        private String Telefonnr;
        private String IBAN;
        private String BIC;
        //private int tabellentiefe;
        //private int anzahlDatensätze;
        private string Documentname;
        private Dictionary<string, decimal> taxes;
        private string filename;

        private double sum;
        private double discount;
        private double total;

        int pageCount = 0;
        int pageEnd = 1100 - 15;
        int actualDataRowIndex = 0;

        //15 = 1 line   //times 5 cause 5 Newlines  // + 30 2 lines space between table and TransferRequest
        int TransferRequestHeight = 0;
        int InvoiceSummaryHeight = 0;

        bool printInvoiceSummaryFlag = false;
        bool printTransferRequestFlag = false;

        public void generate(PDFBLL pdf, PrintDialog dialog)
        {

            //Diese Methode wird von der GUI aus angesteuert und übergibt das Ergebnis des Printdialogs und die Rechnungsnummer.
            this.dialog = dialog; //Der lokale Printdialog wird mit den mitgelieferten Ergebnissen des Druckdialogs gefüllt.
            DBIO(pdf);//Hier wird die Methode DBIO aufgerufen, welche die Variablen mit den passenden Daten füllt.
            print(pdf);//Dieser Methodenaufruf startet den Druckvorgang
        }

        private void DBIO(PDFBLL pdf)
        {
            try
            {
                Rechnungsnr = pdf.invoicenumber;
                Firmenname = pdf.company_name;
                Firmenslogan = pdf.company_slogan;
                Firmenstr = pdf.company_address.address_street;
                Firmenplz = Convert.ToInt32(pdf.company_address.address_postcode);
                Firmenort = pdf.company_address.address_city;


                Anrede = pdf.customer_address.form_of_address;
                Vorname = pdf.customer_address.first_name;
                Nachname = pdf.customer_address.last_name;
                Straße = pdf.customer_address.address_street;
                Postleitzahl = pdf.customer_address.address_postcode;
                Ort = pdf.customer_address.address_city;
                filename = pdf.company_logo;
                taxes = pdf.taxes;
                Rechnungsnr = pdf.invoicenumber;
                Rechnungsdatum = pdf.invoicedate;
                Mailaddr = pdf.company_address.contact_mail;
                Sachbearbeiter = pdf.user.first_name + " " + pdf.user.last_name;
                Telefonnr = pdf.company_address.contact_phone;
                IBAN = pdf.IBAN;
                BIC = pdf.BIC;

                discount = Convert.ToDouble(pdf.discount);
                sum = Convert.ToDouble(pdf.sum);
                total = Convert.ToDouble(pdf.total);

                Documentname = Rechnungsnr + "_" + Nachname + "_" + Vorname;

                //Fill Table
                tabelle.Columns.Add("Menge");
                tabelle.Columns.Add("Artikelnr");
                tabelle.Columns.Add("Name");
                tabelle.Columns.Add("Einzelpreis");
                tabelle.Columns.Add("MwSt");

                foreach (var value in pdf.items_details)
                {
                    tabelle.Rows.Add(value.amount, value.productnumber, value.name, value.price, value.tax);
                }


                //15 = 1 line   //times 5 cause 5 Newlines  // + 30 2 lines space between table and TransferRequest
                TransferRequestHeight = 15 * 5 + 30;
                InvoiceSummaryHeight = 20 + taxes.Count * 20 + 20 + 15;

                //TODO 
                //anzahlDatensätze = tabelle.Rows.Count; //Count Records write to variable.
            }
            catch (Exception e)
            {
                MessageBox.Show("Error filling Data for PDF " + e.Message);
            }

        }

        private void print(PDFBLL pdf)
        {
            PrintDocument PrintDoc = new PrintDocument();

            // Hier wird der Druckdialog angebunden.
            // Der passende Drucker wird von dem Druckdialog ausgelesen und dementsprechend benutzt.

            PrintDoc.PrinterSettings.PrinterName = dialog.PrinterSettings.PrinterName;
            PrintDoc.PrinterSettings.Copies = dialog.PrinterSettings.Copies;
            //Add DocumentName to PrintDialog
            PrintDoc.DocumentName = Documentname;
            PrintDoc.PrintPage += new PrintPageEventHandler(printPage); //Aufruf der ersten Seite
            PrintDoc.Print();
        }

        private Bitmap ImageFilenameToResizedImage(float width, float height, string filename)
        {
            var image = new Bitmap(filename);
            float scale = Math.Min(width / image.Width, height / image.Height);

            var bmp = new Bitmap((int)width, (int)height);
            var graph = Graphics.FromImage(bmp);

            // uncomment for higher quality output
            graph.InterpolationMode = InterpolationMode.High;
            graph.CompositingQuality = CompositingQuality.HighQuality;
            graph.SmoothingMode = SmoothingMode.AntiAlias;

            var scaleWidth = (int)(image.Width * scale);
            var scaleHeight = (int)(image.Height * scale);
            graph.DrawImage(image, ((int)width - scaleWidth) / 2, ((int)height - scaleHeight) / 2, scaleWidth, scaleHeight);
            return bmp;
        }

        void printPage(object sender, PrintPageEventArgs e)
        {

            pageCount += 1;

            // Header
            if (!string.IsNullOrEmpty(filename) && File.Exists(filename))
            {
                Bitmap logo = ImageFilenameToResizedImage(250f, 180f, filename);
                e.Graphics.DrawImage(logo, new Point(453, 5));
            }
            else
            {
                e.Graphics.DrawString(Firmenname, new Font("Courier", 25), new SolidBrush(Color.Black), new Point(453, 25));
                e.Graphics.DrawString(Firmenslogan, new Font("Courier", 12), new SolidBrush(Color.Black), new Point(490, 65));
            }
            e.Graphics.DrawString(Firmenname + "  -  " + Firmenstr + " " + " " + Firmenort, new Font("Courier", 7, FontStyle.Underline), new SolidBrush(Color.DarkGreen), new Point(87, 170)); //30 - 225

            // recipient address
            e.Graphics.DrawString(Anrede, new Font("Courier", 10), new SolidBrush(Color.Black), new Point(87, 195));
            e.Graphics.DrawString(Vorname + " " + Nachname, new Font("Courier", 10), new SolidBrush(Color.Black), new Point(87, 210)); //+15 
            e.Graphics.DrawString(Straße, new Font("Courier", 10), new SolidBrush(Color.Black), new Point(87, 225));//+15
            e.Graphics.DrawString(Postleitzahl + " " + Ort, new Font("Courier", 10), new SolidBrush(Color.Black), new Point(87, 240));//+15

            //return address
            e.Graphics.DrawString(Firmenstr, new Font("Courier", 10), new SolidBrush(Color.Black), new Point(590, 195));
            e.Graphics.DrawString(Firmenort, new Font("Courier", 10), new SolidBrush(Color.Black), new Point(590, 210));
            e.Graphics.DrawString("Es schreibt Ihnen:", new Font("Courier", 10, FontStyle.Bold), new SolidBrush(Color.Black), new Point(470, 225));
            e.Graphics.DrawString(Sachbearbeiter, new Font("Courier", 10, FontStyle.Bold), new SolidBrush(Color.Black), new Point(590, 225));
            e.Graphics.DrawString("Tel:  " + Telefonnr, new Font("Courier", 10), new SolidBrush(Color.Black), new Point(590, 240));
            //e.Graphics.DrawString("Fax: " + Faxnr, new Font("Courier", 10), new SolidBrush(Color.Black), new Point(590, 255));
            e.Graphics.DrawString(Mailaddr, new Font("Courier", 10), new SolidBrush(Color.Black), new Point(590, 255)); //270

            // fold lines - an ein standartisiertes Schema angepasst
            e.Graphics.DrawString("__", new Font("Courier", 10), new SolidBrush(Color.Black), new Point(16, 383));
            e.Graphics.DrawString("____", new Font("Courier", 10), new SolidBrush(Color.Black), new Point(16, 550));
            e.Graphics.DrawString("__", new Font("Courier", 10), new SolidBrush(Color.Black), new Point(16, 779));

            if (pageCount == 1)
            {
                // Body of Bill
                e.Graphics.DrawString("Rechnungsnummer: " + Rechnungsnr, new Font("Courier", 10, FontStyle.Bold), new SolidBrush(Color.Black), new Point(87, 330));
                e.Graphics.DrawString("Rechnungsdatum: " + Rechnungsdatum.ToString("dd.MM.yyyy"), new Font("Courier", 10, FontStyle.Bold), new SolidBrush(Color.Black), new Point(542, 330));

                //Anschreiben
                String Anschreiben;
                Anschreiben = "Sehr geehrte/r " + Anrede + " " + Nachname + ",||gerne übermittle ich Ihnen Ihre Rechnung Nr. " + Rechnungsnr + ".|Nachfolgend eine Detailaufstellung der in Anspruch genommenen Leistungen:";
                Anschreiben = Anschreiben.Replace("|", Environment.NewLine);
                e.Graphics.DrawString(Anschreiben, new Font("Courier", 10), new SolidBrush(Color.Black), new Point(87, 400));
            }
            //Start der Tabelle
            //Tabellenhöhe
            int Zeichenhöhe = 0;
            if (pageCount == 1)
            {
                Zeichenhöhe = 500;
            }
            else
            {
                Zeichenhöhe = 330;
            }
            int Zeichenhöhe_orig = Zeichenhöhe;

            if (actualDataRowIndex < tabelle.Rows.Count)
            {
                //Obere Leiste der Tabelle
                e.Graphics.DrawString("Menge", new Font("Courier", 10), new SolidBrush(Color.Black), new Point(87, Zeichenhöhe - 20));
                e.Graphics.DrawString("Artikelnr.", new Font("Courier", 10), new SolidBrush(Color.Black), new Point(145, Zeichenhöhe - 20));
                e.Graphics.DrawString("Name", new Font("Courier", 10), new SolidBrush(Color.Black), new Point(215, Zeichenhöhe - 20));
                e.Graphics.DrawString("Einzelpreis", new Font("Courier", 10), new SolidBrush(Color.Black), new Point(430, Zeichenhöhe - 20));
                e.Graphics.DrawString("Mwst", new Font("Courier", 10), new SolidBrush(Color.Black), new Point(523, Zeichenhöhe - 20));
                e.Graphics.DrawString("Gesamtpreis", new Font("Courier", 10), new SolidBrush(Color.Black), new Point(620, Zeichenhöhe - 20));
                e.Graphics.DrawLine(new Pen(Color.Black, 2), new Point(87, Zeichenhöhe), new Point(735, Zeichenhöhe)); // Trennung der Tabelle waagerecht
                                                                                                                       //Folgende Reihenfolge wird in der Tabelle benutzt (vom Kunde vorgegeben): Menge, Artikelnr., Name, Einzelpreis, Mwst, Gesamtpreis
            }

            for (int i = actualDataRowIndex; i <= tabelle.Rows.Count - 1; i++)
            {
                DataRow row = tabelle.Rows[i];
                ++actualDataRowIndex;
                int RowDrawHeightStart = Zeichenhöhe;
                int RowDrawHeightEnd = Zeichenhöhe + 15;
                e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(140, RowDrawHeightStart), new Point(140, RowDrawHeightEnd));//nach Menge
                e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(210, RowDrawHeightStart), new Point(210, RowDrawHeightEnd));//nach Artikelnummer
                e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(420, RowDrawHeightStart), new Point(420, RowDrawHeightEnd));//nach Name
                e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(510, RowDrawHeightStart), new Point(510, RowDrawHeightEnd));//nach Einzelpris Netto
                e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(570, RowDrawHeightStart), new Point(570, RowDrawHeightEnd));//nach MWS
                                                                                                                                   //e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(87, tabellentiefe), new Point(735, tabellentiefe));//Endlinie
                Object item = new Object();
                SizeF stringSize = new SizeF();
                string Eintrag = "";

                item = row.ItemArray[0]; //Menge einfügen
                e.Graphics.DrawString(item.ToString(), new Font("Cournier", 10), new SolidBrush(Color.Black), new Point(92, RowDrawHeightStart));

                item = row.ItemArray[1]; //Spalte Artikelnummer einfügen
                e.Graphics.DrawString(item.ToString(), new Font("Cournier", 10), new SolidBrush(Color.Black), new Point(145, RowDrawHeightStart));

                item = row.ItemArray[2]; //Spalte Name einfügen
                e.Graphics.DrawString(item.ToString(), new Font("Cournier", 10), new SolidBrush(Color.Black), new Point(215, RowDrawHeightStart));

                item = row.ItemArray[3]; //Spalte Einzelpreis Netto einfügen - rechtsbündig             
                Eintrag = item.ToString() + " €"; //String zusammenfassen
                stringSize = new SizeF(); // Größe des Strings als Variable definieren
                stringSize = e.Graphics.MeasureString(Eintrag, new Font("Courier", 10)); //Größe des Strings in Variable schreiben
                //e.Graphics.DrawString(Eintrag, new Font("Courier", 10), Brushes.Black, new PointF(550 - stringSize.Width, Zeichenhöhe)); //String schreiben minus die abgemessene Breite um eine künstliche Rechtsbündigkeit zu erzeugen
                e.Graphics.DrawString(Eintrag, new Font("Courier", 10), Brushes.Black, new PointF(506 - stringSize.Width, RowDrawHeightStart)); //String schreiben minus die abgemessene Breite um eine künstliche Rechtsbündigkeit zu erzeugen

                item = row.ItemArray[4]; //Spalte MwST einfügen
                Eintrag = item.ToString() + " %";
                stringSize = new SizeF();
                stringSize = e.Graphics.MeasureString(Eintrag, new Font("Courier", 10));
                e.Graphics.DrawString(Eintrag, new Font("Courier", 10), Brushes.Black, new PointF(570 - stringSize.Width, RowDrawHeightStart));

                //Hier wird die Produktsumme ermittelt
                double menge = Convert.ToDouble(row.ItemArray[0]);
                double preis = Convert.ToDouble(row.ItemArray[3]);
                double produktsumme = menge * preis;

                Eintrag = produktsumme.ToString("n2") + " €"; // Hier wird eine spezielle Geldformatierung ausgewählt. (n2)
                stringSize = new SizeF();
                stringSize = e.Graphics.MeasureString(Eintrag, new Font("Courier", 10));
                e.Graphics.DrawString(Eintrag, new Font("Courier", 10), Brushes.Black, new PointF(706 - stringSize.Width, RowDrawHeightStart));
                // Zwischensumme = Zwischensumme + produktsumme; //Wir berechnen in dieser Schleife die Summe aller Artikel

                Zeichenhöhe += 15;
                if (Zeichenhöhe >= pageEnd) break;
                //if (++actualDataRowIndex == 3) break;
            }

            if (actualDataRowIndex < tabelle.Rows.Count)
            {
                e.HasMorePages = true;
            }
            else
            {
                e.HasMorePages = false;
            }

            if (!printInvoiceSummaryFlag)
            {
                if (Zeichenhöhe < pageEnd && pageEnd > Zeichenhöhe + InvoiceSummaryHeight)
                {
                    Zeichenhöhe = printInvoiceSummary(Zeichenhöhe, e);
                }
                else
                {
                    e.HasMorePages = true;
                }
            }

            if (!printTransferRequestFlag)
            {
                if (Zeichenhöhe < pageEnd && pageEnd > Zeichenhöhe + TransferRequestHeight)
                {
                    Zeichenhöhe = printTransferRequest(Zeichenhöhe, e);
                }
                else
                {
                    e.HasMorePages = true;
                }
            }

            //Footer
            String Fußzeile = "Seite: " + pageCount + "|     IBAN: " + IBAN + "|     BIC: " + BIC;
            String Fußzeile2 = "FB 501049m|"+ "UID: ATU 66441656|" + "SteuerNr.: 313/5589";
            Fußzeile = Fußzeile.Replace("|", Environment.NewLine);
            Fußzeile2 = Fußzeile2.Replace("|", Environment.NewLine);
            e.Graphics.DrawLine(new Pen(Color.Gray, 1), new Point(87, 1100), new Point(735, 1100)); // Trennung der Fußzeile
            e.Graphics.DrawString(Fußzeile, new Font("Courier", 10), new SolidBrush(Color.Gray), new Point(100, 1110));
            e.Graphics.DrawString(Fußzeile2, new Font("Courier", 10), new SolidBrush(Color.Gray), new Point(500, 1110));
        }

        private int printInvoiceSummary(int Zeichenhöhe, PrintPageEventArgs e)
        {
            //Rechnungszusammenfassung
            e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(87, Zeichenhöhe), new Point(735, Zeichenhöhe));//Endlinie

            // - Zwischensumme (unterKathegorie von Rechnungszusammenfassung)
            //string Zwischensumme_string = Zwischensumme.ToString("n2") + " €"; //Hier wird die Zwischensumme als String formatiert
            string Zwischensumme_string = sum.ToString("n2") + " €";
            SizeF Zwischensumme_setting = new SizeF(); //Eine größenangabe in der Variable
            Zwischensumme_setting = e.Graphics.MeasureString(Zwischensumme_string, new Font("Courier", 10));//Testen wie groß der String sein wird
            Zeichenhöhe += 3;
            e.Graphics.DrawString(Zwischensumme_string, new Font("Courier", 10), Brushes.Black, new PointF(706 - Zwischensumme_setting.Width, Zeichenhöhe));//An entsprechnder Tabelle unterhalb der Tabelle schreiben
            e.Graphics.DrawString("Zwischensumme: ", new Font("Courier", 10), Brushes.Black, new Point(265, Zeichenhöhe)); //Beschreibung der Zahlen
            Zeichenhöhe += 17;
            e.Graphics.DrawLine(new Pen(Color.Gray, 1), new Point(575, Zeichenhöhe), new Point(735, Zeichenhöhe));//Zwischensumme Linie


            // - Rabatt ausrechnen & schreiben
            //string rabatt_string = rabatt.ToString("n2") + " €";
            //Zeichenhöhe += 20;
            string rabatt_string = "-" + discount.ToString("n2") + " €";
            SizeF rabatt_setting = new SizeF();
            rabatt_setting = e.Graphics.MeasureString(rabatt_string, new Font("Courier", 10));
            e.Graphics.DrawString(rabatt_string, new Font("Courier", 10), Brushes.Black, new PointF(706 - rabatt_setting.Width, Zeichenhöhe));
            e.Graphics.DrawString("Rabatt: ", new Font("Courier", 10), Brushes.Black, new Point(265, Zeichenhöhe));


            foreach (var value in taxes)
            {
                Zeichenhöhe += 20;
                string mwst_string = value.Value.ToString("n2") + " €";
                SizeF mwst_setting = new SizeF();
                mwst_setting = e.Graphics.MeasureString(mwst_string, new Font("Courier", 10));
                e.Graphics.DrawString(mwst_string, new Font("Courier", 10), Brushes.Black, new PointF(706 - mwst_setting.Width, Zeichenhöhe));
                e.Graphics.DrawString(value.Key, new Font("Courier", 10), Brushes.Black, new Point(265, Zeichenhöhe));

            }

            // - Gesamt
            Zeichenhöhe += 18;
            e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(575, Zeichenhöhe), new Point(735, Zeichenhöhe));//Linie über gesamt
            Zeichenhöhe += 2;
            //double gesamt = Zwischensumme - rabatt; //Gesamtbetrag ausrechnen
            //string gesamt_string = gesamt.ToString("n2") + " €";
            string gesamt_string = total.ToString("n2") + " €";
            SizeF gesamt_setting = new SizeF();
            gesamt_setting = e.Graphics.MeasureString(gesamt_string, new Font("Courier", 10));
            e.Graphics.DrawString(gesamt_string, new Font("Courier", 10, FontStyle.Bold), Brushes.Black, new PointF(706 - gesamt_setting.Width, Zeichenhöhe));
            e.Graphics.DrawString("Gesamt: ", new Font("Courier", 10, FontStyle.Bold), Brushes.Black, new Point(265, Zeichenhöhe));

            printInvoiceSummaryFlag = true;

            //Set to End of Height
            Zeichenhöhe += 15;
            return Zeichenhöhe;
        }

        private int printTransferRequest(int Zeichenhöhe, PrintPageEventArgs e)
        {
            // Newline
            Zeichenhöhe += 30;
            //Transfer request below table
            string gesamt_string = total.ToString("n2") + " €";
            String überweisungsaufforderung = "Bitte überweisen Sie den Gesamtbetrag von " + gesamt_string + " innerhalb 7 Tagen |unter Angabe des Namen Ihres Tieres auf mein Konto. ||Vielen Dank für Ihr Vertrauen!||Mit freundlichen Grüßen||" + Sachbearbeiter;
            überweisungsaufforderung = überweisungsaufforderung.Replace("|", Environment.NewLine);
            //e.Graphics.DrawString(überweisungsaufforderung, new Font("Courier", 10), Brushes.Black, new Point(87, tabellentiefe + 190)); //Dynamisch gehalten, das der Ort dieses Textes je nach Anzahl der Produkte passend geschrieben wird.
            e.Graphics.DrawString(überweisungsaufforderung, new Font("Courier", 10), Brushes.Black, new Point(87, Zeichenhöhe)); //Dynamisch gehalten, das der Ort dieses Textes je nach Anzahl der Produkte passend geschrieben wird.

            printTransferRequestFlag = true;

            return Zeichenhöhe;
        }
    }
}
