namespace BIMStore.BLL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_products
    {
        public int Id { get; set; }

        [StringLength(64)]
        public string name { get; set; }

        public int? category { get; set; }

        public string description { get; set; }

        public decimal rate { get; set; }

        public decimal qty { get; set; }

        public DateTime? added_date { get; set; }

        public int? added_by { get; set; }

        public bool hasqty { get; set; }
    }
}
