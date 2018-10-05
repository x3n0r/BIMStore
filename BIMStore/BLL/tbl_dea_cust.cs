namespace BIMStore.BLL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_dea_cust
    {
        public int Id { get; set; }

        [StringLength(64)]
        public string type { get; set; }

        [StringLength(128)]
        public string first_name { get; set; }

        [StringLength(128)]
        public string last_name { get; set; }

        [StringLength(64)]
        public string form_of_address { get; set; }

        [StringLength(64)]
        public string address_street { get; set; }

        [StringLength(64)]
        public string address_postcode { get; set; }

        [StringLength(64)]
        public string address_city { get; set; }

        [StringLength(64)]
        public string address_country { get; set; }

        [StringLength(64)]
        public string contact_phone { get; set; }

        [StringLength(64)]
        public string contact_mail { get; set; }
    }
}
