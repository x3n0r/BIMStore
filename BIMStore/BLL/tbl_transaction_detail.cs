namespace BIMStore.BLL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_transaction_detail
    {
        public int Id { get; set; }

        public int? product_id { get; set; }

        public decimal price { get; set; }

        public decimal qty { get; set; }

        public decimal total { get; set; }

        public int? dea_cust_id { get; set; }

        public DateTime added_date { get; set; }

        public int? added_by { get; set; }
    }
}
