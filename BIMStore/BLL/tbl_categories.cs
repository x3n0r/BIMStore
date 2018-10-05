namespace BIMStore.BLL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_categories
    {
        public int Id { get; set; }

        [StringLength(64)]
        public string title { get; set; }

        public string description { get; set; }

        public DateTime added_date { get; set; }

        public int? added_by { get; set; }

        public decimal tax { get; set; }
    }
}
