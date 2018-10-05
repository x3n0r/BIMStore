using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMStore.BLL
{
    class PDFBLL
    {
        public struct items
        {
            public int amount;
            public int productnumber;
            public string name;
            public decimal price;
            public decimal tax; // percent
            public decimal total; // price* amount
        }

        public struct customer
        {
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string form_of_address { get; set; }
            public string address_street { get; set; }
            public string address_postcode { get; set; }
            public string address_city { get; set; }
            public string address_country { get; set; }
        }

        public struct company
        {
            public string address_street { get; set; }
            public string address_postcode { get; set; }
            public string address_city { get; set; }
            public string address_country { get; set; }
            public string contact_phone { get; set; }
            public string contact_mail { get; set; }
        }

        public struct companyuser
        {
            public string first_name { get; set; }
            public string last_name { get; set; }
        }

        public string companyname { get; set; }
        public string slogan { get; set; }
        public string logo { get; set; }
        public customer customeraddress { get; set; }
        public company companyaddress { get; set; }
        public companyuser user { get; set; }
        public int invoicenumber { get; set; }
        public DateTime invoicedate { get; set; }
        public List<items> listitems { get; set; }
        public decimal sum { get; set; }
        public decimal discount { get; set; }
        public decimal total { get; set; }
        public string IBAN { get; set; }
        public string BIC { get; set; }
        public Dictionary<string, decimal> taxes { get; set; }
    }
}
