using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMStore.BLL
{
    class DeaCustBLL
    {
        public int Id { get; set; }
        public string type { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string form_of_address { get; set; }
        public string address_street;
        public string address_postcode;
        public string address_city;
        public string address_country;
        public string contact_phone { get; set; }
        public string contact_mail { get; set; }
    }
}
