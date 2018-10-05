namespace BIMStore.BLL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_transactions
    {
        public int Id { get; set; }

        [StringLength(64)]
        public string type { get; set; }

        public int? dea_cust_id { get; set; }

        public decimal grandTotal { get; set; }

        public DateTime transaction_date { get; set; }

        public decimal tax { get; set; }

        public decimal discount { get; set; }

        public int? added_by { get; set; }

        [StringLength(1)]
        public string kontobez { get; set; }
    }
}
