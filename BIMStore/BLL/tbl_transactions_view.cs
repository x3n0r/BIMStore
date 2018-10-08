using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMStore.BLL
{
    public partial class tbl_transactions_view
    {
        public int Id { get; set; }

        public string type { get; set; }

        public string dea_cust_id { get; set; }

        public decimal grandTotal { get; set; }

        public DateTime transaction_date { get; set; }

        public decimal tax { get; set; }

        public decimal discount { get; set; }

        public int? added_by { get; set; }

        public string kontobez { get; set; }

        public static explicit operator tbl_transactions_view(tbl_transactions v)
        {
            tbl_transactions_view a = new tbl_transactions_view
            {
                Id = v.Id,
                added_by = v.added_by,
                discount = v.discount,
                grandTotal = v.grandTotal,
                kontobez = v.kontobez,
                tax = v.tax,
                transaction_date = v.transaction_date,
                type = v.type
            };

            DAL.DeaCustDAL dcDAL = new DAL.DeaCustDAL();
            tbl_dea_cust b = dcDAL.GetDeaCustIDFromID((int)v.dea_cust_id);
            a.dea_cust_id = b.first_name + " " + b.last_name;

            return a;
        }
    }
}
