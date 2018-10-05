using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMStore.BLL
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
        public string address_street { get; set; }
        public string address_postcode { get; set; }
        public string address_city { get; set; }
        public string address_country { get; set; }
        public string logo { get; set; }

    }
}
