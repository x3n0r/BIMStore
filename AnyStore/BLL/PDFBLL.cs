using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyStore.BLL
{
    class PDFBLL
    {
        public struct items
        {
            public int amount;
            public int productnumber;
            public string name;
            public decimal price;
            public decimal total;
        }

        public struct customer
        {
            public string name;
            public string address;
            public string country;
        }

        public struct company
        {
            public string address;
            public string country;
            public string telnb;
            public string email;
        }

        public string companyname { get; set; }
        public string slogan { get; set; }
        public customer customeraddress { get; set; }
        public company companyaddress { get; set; }
        public int invoicenumber { get; set; }
        public string invoicedate { get; set; }
        public List<items> listitems { get; set; }
        public decimal sum { get; set; }
        public decimal discount { get; set; }
        public decimal total { get; set; }
        public string IBAN { get; set; }
        public string BIC { get; set; }
    }
}
