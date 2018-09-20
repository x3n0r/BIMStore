using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyStore.BLL
{
    class CompanyDataBLL
    {

        public struct company
        {
            public string address;
            public string country;
            public string telnb;
            public string email;
        }

        public int Id { get; set; }
        public string name { get; set; }
        public string slogan { get; set; }
        // public company companyaddress { get; set; }
        public string address;
        public string country;
        public string telnb;
        public string email;
        public string IBAN { get; set; }
        public string BIC { get; set; }
    }
}
