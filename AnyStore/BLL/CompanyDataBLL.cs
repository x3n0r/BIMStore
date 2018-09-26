using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyStore.BLL
{
    class CompanyDataBLL
    {
        /*
        public struct company
        {
            public string address;
            public string country;
            public string telnb;
            public string email;
        }
        */
        public int Id { get; set; }
        public string name { get; set; }
        public string slogan { get; set; }
        public string contact_phone;
        public string contact_mail;
        public string IBAN { get; set; }
        public string BIC { get; set; }
        // public company companyaddress { get; set; }
        public string address_street;
        public string address_postcode;
        public string address_city;
        public string address_country;

    }
}
