namespace BIMStore.BLL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_users
    {
        public int Id { get; set; }

        [StringLength(64)]
        public string first_name { get; set; }

        [StringLength(64)]
        public string last_name { get; set; }


        [StringLength(64)]
        public string username { get; set; }

        [StringLength(64)]
        public string password { get; set; }

        [StringLength(64)]
        public string contact_phone { get; set; }

        [StringLength(64)]
        public string contact_email { get; set; }

        [StringLength(64)]
        public string address { get; set; }

        [StringLength(32)]
        public string gender { get; set; }

        [StringLength(32)]
        public string user_type { get; set; }

        public DateTime added_date { get; set; }

        public int? added_by { get; set; }
    }
}
