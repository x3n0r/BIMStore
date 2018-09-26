//Diese Klasse wurde von P Lehr (https://github.com/plehr) im Rahmen des Projektes "stnr" erstellt.
//Alle Eigentumsrechte bleiben bei P Lehr (https://github.com/plehr). 

// Information: Diese Klasse funktioniert soweit.
// Im Moment können nicht mehr als 20 Posten gedruckt werden.
// Das könnte durch eine weitere Methode weitereSeite(string seitenZahl);  behoben werden, sodass unbegrenzte Posten verfügbar sind.

using AnyStore.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnyStore.DAL
{
    class PDF
    {
        //Wichtige Variablen definieren
        private bool debug = false;
        private String Firmenname;
        private String Firmenslogan;
        private String Firmenstr;
        //private String Firmenhnr;
        private int Firmenplz;
        private String Firmenort;
        private String Anrede;
        private String Vorname;
        private String Nachname;
        private String Straße;
        //private int Hausnummer;
        private int Postleitzahl;
        private String Ort;
        private PrintDialog dialog;
        private int Rechnungsnr;
        private DateTime Rechnungsdatum;
        private DataTable tabelle = new DataTable();
        private String Mailaddr;
        private String Sachbearbeiter;
        private String Telefonnr;
        //private String Faxnr;
        private String IBAN;
        private String BIC;
        //private Int64 Steuernr;
        private int tabellentiefe;
        private int anzahlDatensätze;

        private Dictionary<string, decimal> taxes;

        /*
     private double Versandkosten;
     private double mwst19;
     private double mwst7;
     */
        private double Zwischensumme;
        private double rabatt;

        private double sum;
        private double discount;
        private double total;


        public void generate(PDFBLL pdf, PrintDialog dialog)
        {
                
            //Diese Methode wird von der GUI aus angesteuert und übergibt das Ergebnis des Printdialogs und die Rechnungsnummer.
            Rechnungsnr = pdf.invoicenumber; // Die Variable Rechnungsnr wird mit der mitgegebenen Rechnungsnummer gefüllt.
            this.dialog = dialog; //Der lokale Printdialog wird mit den mitgelieferten Ergebnissen des Druckdialogs gefüllt.
            DBIO(pdf);//Hier wird die Methode DBIO aufgerufen, welche die Variablen mit den passenden Datenbankeinträgen füllt.
            ausdruck(pdf);//Dieser Methodenaufruf startet den Druckvorgang der 1.Seite
            if (debug)
            { // Für Debugzwecke wird am Ende jeder Verarbeitung eine Meldung ausgegeben.
                MessageBox.Show("Ihr Druck wurde erfolgreich auf dem Drucker: " + dialog.PrinterSettings.PrinterName.ToString() + " gedruckt!");
            }
         }
       
        private void DBIO(PDFBLL pdf)
        {
            //Datenbankzugriff um Kundendaten zu holen!
            //Aus Krankheitsgründen wird der Datenbankzugriff erst am Wochenende fertiggestellt.
            //Wir haben uns folgende Interaktion mit der zukünftigen Datenbankklasse überlegt: (Beispiel)
           //  

            //MOCK-Daten füllen
            //Bis die Interaktion mit der Datenbank funktioniert, wird hier der Datenbankzugriff simuliert, also Beispielwerte gesetzt.
            try { // Das "try" hat den Hintergund, das im falle einer fehlerhaften Datenbankinteraktion eine Fehlermeldung erscheint.
                Firmenname = pdf.companyname;
                Firmenslogan = pdf.slogan;
                Firmenstr = pdf.companyaddress.address_street;
                Firmenplz = Convert.ToInt32(pdf.companyaddress.address_postcode);
                Firmenort = pdf.companyaddress.address_city;// "Mustershausen";


                Anrede = pdf.customeraddress.form_of_address;
                Vorname = pdf.customeraddress.first_name;// "Maximilian";
                Nachname = pdf.customeraddress.last_name;
                Straße = pdf.customeraddress.address_street;// "Musterstraße";
                Postleitzahl = Convert.ToInt32(pdf.customeraddress.address_postcode);
                Ort = pdf.customeraddress.address_city;// "Musterdorf";

                //Versandkosten = 10.30;
                //Hausnummer = 123;
                taxes = pdf.taxes;
                Rechnungsnr = pdf.invoicenumber;// 1234567890;
                Rechnungsdatum = pdf.invoicedate;
                Mailaddr = pdf.companyaddress.contact_mail;//"erika@musterfrau.tld";
                Sachbearbeiter = pdf.user.first_name + " " + pdf.user.last_name;// "Erika Musterfrau";
                Telefonnr = pdf.companyaddress.contact_phone;// "+49 123 3215 - 10";
                //Faxnr = "+49 123 3215 - 20";
                IBAN = pdf.IBAN;// "DE42 4242 0420 0000 0424 29";
                //Steuernr = 04242424200;
                BIC = pdf.BIC; //"BERLADE42XXX";

                discount = Convert.ToDouble(pdf.discount); //10;
                sum = Convert.ToDouble(pdf.sum);
                total = Convert.ToDouble(pdf.total);

                //Tabelle füllen
                tabelle.Columns.Add("Menge");
                tabelle.Columns.Add("Artikelnummer");
                tabelle.Columns.Add("Beschreibung");
                tabelle.Columns.Add("Einzelpreis Netto");
                tabelle.Columns.Add("MwSt");

                foreach ( var value in pdf.listitems)
                {
                    tabelle.Rows.Add(value.amount, value.productnumber, value.name, value.price, value.tax);

                }

                anzahlDatensätze = tabelle.Rows.Count; //Diese Zuweisung zählt die Datensätze und schreibt die Anzahl in eine Variable.
            }
                catch (Exception Fehler) {
                    MessageBox.Show("Kommunikation mit der Datenbank nicht möglich/fehlerhaft " + Fehler.Message);
            }
                        
        }

        private void ausdruck(PDFBLL pdf)
        {
            PrintDocument PrintDoc = new PrintDocument();

            // Hier wird der Druckdialog angebunden.
            // Der passende Drucker wird von dem Druckdialog ausgelesen und dementsprechend benutzt.
            PrintDoc.PrinterSettings.PrinterName = dialog.PrinterSettings.PrinterName;
            PrintDoc.PrinterSettings.Copies = dialog.PrinterSettings.Copies;
            PrintDoc.PrintPage += new PrintPageEventHandler(ersteSeite); //Aufruf der ersten Seite
            PrintDoc.Print();
        }

        void ersteSeite(object sender, PrintPageEventArgs e)
        {
            //!! Maximal 19 Einträge können gedruckt werden

            //Logo einfügen
            /*
            Image newImage = AnyStore.Properties.Resources.samplecustomer;
            Bitmap logo = new Bitmap(newImage);
            logo.SetResolution(400, 400);
            */
            // Header
            e.Graphics.DrawString(Firmenname, new Font("Courier", 25), new SolidBrush(Color.Black), new Point(453, 25));
            e.Graphics.DrawString(Firmenslogan, new Font("Courier", 12), new SolidBrush(Color.Black), new Point(490, 65));
            //e.Graphics.DrawImageUnscaled(logo, new Point(592, 105));
            e.Graphics.DrawString(Firmenname + "  -  " + Firmenstr + " " + " " + Firmenort, new Font("Courier", 7, FontStyle.Underline), new SolidBrush(Color.DarkGreen), new Point(87, 170)); //30 - 225
           
            // Empfängeradresse
            e.Graphics.DrawString(Anrede, new Font("Courier", 10), new SolidBrush(Color.Black), new Point(87, 195));
            e.Graphics.DrawString(Vorname + " " +Nachname, new Font("Courier", 10), new SolidBrush(Color.Black), new Point(87, 210)); //+15 
            e.Graphics.DrawString(Straße, new Font("Courier", 10), new SolidBrush(Color.Black), new Point(87, 225));//+15
            e.Graphics.DrawString(Postleitzahl + " " + Ort , new Font("Courier", 10), new SolidBrush(Color.Black), new Point(87, 240));//+15
           
            //Absenderadresse
            e.Graphics.DrawString(Firmenstr , new Font("Courier", 10), new SolidBrush(Color.Black), new Point(590, 195));
            e.Graphics.DrawString(Firmenort, new Font("Courier", 10), new SolidBrush(Color.Black), new Point(590, 210));
            e.Graphics.DrawString("Es schreibt Ihnen:", new Font("Courier", 10, FontStyle.Bold), new SolidBrush(Color.Black), new Point(470, 225));
            e.Graphics.DrawString(Sachbearbeiter, new Font("Courier", 10, FontStyle.Bold), new SolidBrush(Color.Black), new Point(590, 225));
            e.Graphics.DrawString("Tel:  " + Telefonnr, new Font("Courier", 10), new SolidBrush(Color.Black), new Point(590, 240));
            //e.Graphics.DrawString("Fax: " + Faxnr, new Font("Courier", 10), new SolidBrush(Color.Black), new Point(590, 255));
            e.Graphics.DrawString(Mailaddr, new Font("Courier", 10), new SolidBrush(Color.Black), new Point(590, 255)); //270
           
            // Faltlinen - an ein standartisiertes Schema angepasst
            e.Graphics.DrawString("__", new Font("Courier", 10), new SolidBrush(Color.Black), new Point(16, 383));
            e.Graphics.DrawString("____", new Font("Courier", 10), new SolidBrush(Color.Black), new Point(16, 550));
            e.Graphics.DrawString("__", new Font("Courier", 10), new SolidBrush(Color.Black), new Point(16, 779));

           // Body der Rechnung
            e.Graphics.DrawString("Rechnungsnummer: " + Rechnungsnr, new Font("Courier", 10, FontStyle.Bold), new SolidBrush(Color.Black), new Point(87, 330));
            e.Graphics.DrawString("Rechnungsdatum: " + Rechnungsdatum.ToString("dd.MM.yyyy"), new Font("Courier", 10, FontStyle.Bold), new SolidBrush(Color.Black), new Point(542, 330));
           
             //Anschreiben
            String Anschreiben;
            Anschreiben = "Sehr geehrte(r) "+ Anrede + " " + Nachname + ",||anbei erhalten Sie die Rechnung nummer " + Rechnungsnr + ".|Nachfolgend eine Detailauftellung Ihrer Bestellung:";
            Anschreiben = Anschreiben.Replace("|", Environment.NewLine);
            e.Graphics.DrawString(Anschreiben, new Font("Courier", 10), new SolidBrush(Color.Black), new Point(87, 400));

            //Start der Tabelle
            //Tabellenhöhe
            int Zeichenhöhe;
            int Zeichenhöhe_orig;
            Zeichenhöhe = 500;
            Zeichenhöhe_orig = Zeichenhöhe;

            //Obere Leiste der Tabelle
            e.Graphics.DrawString("Menge", new Font("Courier", 10), new SolidBrush(Color.Black), new Point(87, Zeichenhöhe - 20));
            e.Graphics.DrawString("Artikelnummer", new Font("Courier", 10), new SolidBrush(Color.Black), new Point(145, Zeichenhöhe - 20));
            e.Graphics.DrawString("Name", new Font("Courier", 10), new SolidBrush(Color.Black), new Point(265, Zeichenhöhe - 20));
            e.Graphics.DrawString("Einzelpreis", new Font("Courier", 10), new SolidBrush(Color.Black), new Point(430, Zeichenhöhe - 20));
            //e.Graphics.DrawString("Einzelpreis", new Font("Courier", 10), new SolidBrush(Color.Black), new Point(490, Zeichenhöhe - 20));
            e.Graphics.DrawString("Mwst", new Font("Courier", 10), new SolidBrush(Color.Black), new Point(523, Zeichenhöhe - 20));
            e.Graphics.DrawString("Gesamtpreis", new Font("Courier", 10), new SolidBrush(Color.Black), new Point(620, Zeichenhöhe - 20));
            //e.Graphics.DrawString("Gesamtpreis", new Font("Courier", 10), new SolidBrush(Color.Black), new Point(585, Zeichenhöhe - 20));
            e.Graphics.DrawLine(new Pen(Color.Black, 2), new Point(87, Zeichenhöhe), new Point(735, Zeichenhöhe)); // Trennung der Tabelle waagerecht
            //Folgende Reihenfolge wird in der Tabelle benutzt (vom Kunde vorgegeben): Menge, Artikelnummer, Name, Einzelpreis Netto, Mwst, Gesamtpreis netto

            //Abspaltungen der Tabelle waagerecht
            //Die Tabelle ist dynamisch nach anzahl der Bestellungen generiert
            tabellentiefe = Convert.ToInt32(Zeichenhöhe + (15.5f * anzahlDatensätze));
            
            e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(140, Zeichenhöhe), new Point(140, tabellentiefe));//nach Menge
            e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(260, Zeichenhöhe), new Point(260, tabellentiefe));//nach Artikelnummer
            e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(420, Zeichenhöhe), new Point(420, tabellentiefe));//nach Name
            //e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(390, Zeichenhöhe), new Point(390, tabellentiefe));//nach Name
            e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(510, Zeichenhöhe), new Point(510, tabellentiefe));//nach Einzelpris Netto
            e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(570, Zeichenhöhe), new Point(570, tabellentiefe));//nach MWS
            e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(87, tabellentiefe), new Point(735, tabellentiefe));//Endlinie

            //Tabelle füllen
            foreach (DataRow row in tabelle.Rows)
            {
               Object item = row.ItemArray[0]; //Menge einfügen
               e.Graphics.DrawString(item.ToString(), new Font("Cournier", 10), new SolidBrush(Color.Black), new Point(92, Zeichenhöhe));
               Zeichenhöhe = Zeichenhöhe + 15;
            }


            Zeichenhöhe = Zeichenhöhe_orig; //Dieser Eintrag setzt den Cursor zum ausfüllen der Tabelle zurück
            foreach (DataRow row in tabelle.Rows)
            {
                Object item = row.ItemArray[1]; //Spalte Artikelnummer einfügen
                e.Graphics.DrawString(item.ToString(), new Font("Cournier", 10), new SolidBrush(Color.Black), new Point(145, Zeichenhöhe));
                Zeichenhöhe = Zeichenhöhe + 15;
            }


            Zeichenhöhe = Zeichenhöhe_orig; //Dieser Eintrag setzt den Cursor zum ausfüllen der Tabelle zurück
            foreach (DataRow row in tabelle.Rows)
            {
                Object item = row.ItemArray[2]; //Spalte Name einfügen
                e.Graphics.DrawString(item.ToString(), new Font("Cournier", 10), new SolidBrush(Color.Black), new Point(265, Zeichenhöhe));
                Zeichenhöhe = Zeichenhöhe + 15;
            }


            Zeichenhöhe = Zeichenhöhe_orig; //Dieser Eintrag setzt den Cursor zum ausfüllen der Tabelle zurück
            foreach (DataRow row in tabelle.Rows)
            {
                Object item = row.ItemArray[3]; //Spalte Einzelpreis Netto einfügen - rechtsbündig             
                string Eintrag = item.ToString() + " €"; //String zusammenfassen
                SizeF stringSize = new SizeF(); // Größe des Strings als Variable definieren
                stringSize = e.Graphics.MeasureString(Eintrag, new Font("Courier", 10)); //Größe des Strings in Variable schreiben
                //e.Graphics.DrawString(Eintrag, new Font("Courier", 10), Brushes.Black, new PointF(550 - stringSize.Width, Zeichenhöhe)); //String schreiben minus die abgemessene Breite um eine künstliche Rechtsbündigkeit zu erzeugen
                e.Graphics.DrawString(Eintrag, new Font("Courier", 10), Brushes.Black, new PointF(506 - stringSize.Width, Zeichenhöhe)); //String schreiben minus die abgemessene Breite um eine künstliche Rechtsbündigkeit zu erzeugen
                Zeichenhöhe = Zeichenhöhe + 15; //Nächster Eintrag muss tiefer sein als dieser Eintrag
            }

            
            Zeichenhöhe = Zeichenhöhe_orig; //Dieser Eintrag setzt den Cursor zum ausfüllen der Tabelle zurück
            foreach (DataRow row in tabelle.Rows)
            {
                Object item = row.ItemArray[4]; //Spalte MwST einfügen
                string Eintrag = item.ToString() + " %";
                SizeF stringSize = new SizeF();
                stringSize = e.Graphics.MeasureString(Eintrag, new Font("Courier", 10));
                e.Graphics.DrawString(Eintrag, new Font("Courier", 10), Brushes.Black, new PointF(562 - stringSize.Width, Zeichenhöhe));
                Zeichenhöhe = Zeichenhöhe + 15;
                
            }


            Zeichenhöhe = Zeichenhöhe_orig; //Dieser Eintrag setzt den Cursor zum ausfüllen der Tabelle zurück
            foreach (DataRow row in tabelle.Rows)
            {
                //Hier wird die Produktsumme ermittelt
                    double menge = Convert.ToDouble(row.ItemArray[0]);
                    double preis = Convert.ToDouble(row.ItemArray[3]);
                    double produktsumme = menge * preis; fehler
                
                string Eintrag = produktsumme.ToString("n2") + " €"; // Hier wird eine spezielle Geldformatierung ausgewählt. (n2)
                SizeF stringSize = new SizeF();
                stringSize = e.Graphics.MeasureString(Eintrag, new Font("Courier", 10));
                e.Graphics.DrawString(Eintrag, new Font("Courier", 10), Brushes.Black, new PointF(706 - stringSize.Width, Zeichenhöhe));
                Zeichenhöhe = Zeichenhöhe + 15;
                Zwischensumme = Zwischensumme + produktsumme; //Wir berechnen in dieser Schleife die Summe aller Artikel

                // - Berechnung MWST - Wir müssen in dieser Schleife die MWST berechnen, da nur hier die Produktsumme in Verbindung mit den verschiedenen MWST-Sätzen steht.
                /*
                Object mwstcheck = row.ItemArray[4]; //Auslesen der MWST des derzeitigen Produkts
                if (mwstcheck.ToString() == "19") //Abfrage, welcher Mehrwertsteuersatz genutzt wird. Kundenvorgabe sind 2 verschiedene Sätze.
                {
                    //mwst19 = mwst19 + (produktsumme * 0.19d); //Hier wird die Mehrwertsteuer 19% des derzeitigen Produkts zu den anderen MWST-Sätzen addiert, wenn das Produkt 19% enthält.
                }
                if (mwstcheck.ToString() == "7")
                {

                   // mwst7 = mwst7 + (produktsumme * 0.07d);//Hier wird die Mehrwertsteuer 7% des derzeitigen Produkts zu den anderen MWST-Sätzen addiert, wenn das Produkt 7% enthält.
                }
                */
             }

            //Rechnungszusammenfassung

            // - Zwischensumme (unterKathegorie von Rechnungszusammenfassung)
            //string Zwischensumme_string = Zwischensumme.ToString("n2") + " €"; //Hier wird die Zwischensumme als String formatiert
            string Zwischensumme_string = sum.ToString("n2") + " €";
            SizeF Zwischensumme_setting = new SizeF(); //Eine größenangabe in der Variable
            Zwischensumme_setting = e.Graphics.MeasureString(Zwischensumme_string, new Font("Courier", 10));//Testen wie groß der String sein wird
            e.Graphics.DrawString(Zwischensumme_string, new Font("Courier", 10), Brushes.Black, new PointF(706 - Zwischensumme_setting.Width, tabellentiefe +3));//An entsprechnder Tabelle unterhalb der Tabelle schreiben
            e.Graphics.DrawLine(new Pen(Color.Gray, 1), new Point(575, tabellentiefe + 20), new Point(735, tabellentiefe + 20));//Zwischensumme Linie
            e.Graphics.DrawString("Zwischensumme: ", new Font("Courier", 10), Brushes.Black, new Point(265, tabellentiefe + 3)); //Beschreibung der Zahlen


            /*
            // - Versandkosten
            string Versandkosten_string = Versandkosten.ToString("n2") + " €";
            SizeF Versandkosten_setting = new SizeF();
            Versandkosten_setting = e.Graphics.MeasureString(Versandkosten_string, new Font("Courier", 10));
            e.Graphics.DrawString(Versandkosten_string, new Font("Courier", 10), Brushes.Black, new PointF(706 - Versandkosten_setting.Width, tabellentiefe + 20));
            e.Graphics.DrawLine(new Pen(Color.Gray, 1), new Point(575, tabellentiefe + 35), new Point(735, tabellentiefe + 35));//Versandkosten
            e.Graphics.DrawString("Versandkosten: ", new Font("Courier", 10), Brushes.Black, new Point(265, tabellentiefe + 20));
            */

            /*
            // - MwSt gesamt ermitteln
            double mwstges = mwst7 + mwst19;
            string mwstges_string = mwstges.ToString("n2") + " €";
            SizeF mwstges_setting = new SizeF();
            mwstges_setting = e.Graphics.MeasureString(mwstges_string, new Font("Courier", 10));
            e.Graphics.DrawString(mwstges_string, new Font("Courier", 10), Brushes.Black, new PointF(706 - mwstges_setting.Width, tabellentiefe + 65));
            e.Graphics.DrawString("MwSt. gesamt: ", new Font("Courier", 10), Brushes.Black, new Point(265, tabellentiefe + 65));
            e.Graphics.DrawLine(new Pen(Color.Gray, 1), new Point(575, tabellentiefe + 80), new Point(735, tabellentiefe + 80));//Linie über Summe
            

            // - Summe ermitteln
            double summe = Zwischensumme; // + Versandkosten + mwstges;
            string summe_string = summe.ToString("n2") + " €";
            SizeF summe_setting = new SizeF();
            summe_setting = e.Graphics.MeasureString(summe_string, new Font("Courier", 10));
            e.Graphics.DrawString(summe_string, new Font("Courier", 10, FontStyle.Bold), Brushes.Black, new PointF(706 - summe_setting.Width, tabellentiefe + 80));
            e.Graphics.DrawString("Summe: ", new Font("Courier", 10, FontStyle.Bold), Brushes.Black, new Point(265, tabellentiefe + 80));
            */

            // - Rabatt ausrechnen & schreiben
            //string rabatt_string = rabatt.ToString("n2") + " €";
            int mytabellentiefe = 0;
            mytabellentiefe += 20;
            string rabatt_string = "-"+discount.ToString("n2") + " €";
            SizeF rabatt_setting = new SizeF();
            rabatt_setting = e.Graphics.MeasureString(rabatt_string, new Font("Courier", 10));
            e.Graphics.DrawString(rabatt_string, new Font("Courier", 10), Brushes.Black, new PointF(706 - rabatt_setting.Width, tabellentiefe + mytabellentiefe));
            e.Graphics.DrawString("Rabatt: ", new Font("Courier", 10), Brushes.Black, new Point(265, tabellentiefe + mytabellentiefe));


            foreach (var value in taxes)
            {
                mytabellentiefe += 20;
                string mwst_string = value.Value.ToString("n2") + " €";
                SizeF mwst_setting = new SizeF();
                mwst_setting = e.Graphics.MeasureString(mwst_string, new Font("Courier", 10));
                e.Graphics.DrawString(mwst_string, new Font("Courier", 10), Brushes.Black, new PointF(706 - mwst_setting.Width, tabellentiefe + mytabellentiefe));
                e.Graphics.DrawString(value.Key, new Font("Courier", 10), Brushes.Black, new Point(265, tabellentiefe + mytabellentiefe));

            }

            // - Gesamt
            mytabellentiefe += 20;
            e.Graphics.DrawLine(new Pen(Color.Black, 1), new Point(575, tabellentiefe + mytabellentiefe), new Point(735, tabellentiefe + mytabellentiefe));//Linie über gesamt
            double gesamt = Zwischensumme - rabatt; //Gesamtbetrag ausrechnen
            //string gesamt_string = gesamt.ToString("n2") + " €";
            string gesamt_string = total.ToString("n2") + " €"; 
            SizeF gesamt_setting = new SizeF();
            gesamt_setting = e.Graphics.MeasureString(gesamt_string, new Font("Courier", 10));
            e.Graphics.DrawString(gesamt_string, new Font("Courier", 10, FontStyle.Bold), Brushes.Black, new PointF(706 - gesamt_setting.Width, tabellentiefe + mytabellentiefe));
            e.Graphics.DrawString("Gesamt: ", new Font("Courier", 10,FontStyle.Bold), Brushes.Black, new Point(265, tabellentiefe + mytabellentiefe));

             //Überweisungsaufforderung unter der Tabelle
            String überweisungsaufforderung = "Bitte überweisen Sie den Gesamtbetrag von " + gesamt_string +" innerhalb 7 Tagen |unter Angabe der Rechnungsnummer auf mein Konto. ||Vielen Dank!||" +Sachbearbeiter;
            überweisungsaufforderung = überweisungsaufforderung.Replace("|", Environment.NewLine);
            //e.Graphics.DrawString(überweisungsaufforderung, new Font("Courier", 10), Brushes.Black, new Point(87, tabellentiefe + 190)); //Dynamisch gehalten, das der Ort dieses Textes je nach Anzahl der Produkte passend geschrieben wird.
            e.Graphics.DrawString(überweisungsaufforderung, new Font("Courier", 10), Brushes.Black, new Point(87, 980)); //Dynamisch gehalten, das der Ort dieses Textes je nach Anzahl der Produkte passend geschrieben wird.

            //Fußzeile
            String Fußzeile = "IBAN: " + IBAN +"       BIC: "+  BIC ;
            Fußzeile = Fußzeile.Replace("|", Environment.NewLine);
            e.Graphics.DrawLine(new Pen(Color.Gray, 1), new Point(87, 1100), new Point(735, 1100)); // Trennung der Fußzeile
            e.Graphics.DrawString(Fußzeile, new Font("Courier", 10), new SolidBrush(Color.Gray), new Point(87, 1110));
            }


    
    }
}
