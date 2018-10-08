using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMStore.BLL
{
    public partial class tbl_products_view
    {
        public int Id { get; set; }

        public string name { get; set; }

        public string category { get; set; }

        public string description { get; set; }

        public decimal rate { get; set; }

        public decimal qty { get; set; }

        public DateTime? added_date { get; set; }

        public int? added_by { get; set; }

        public bool hasqty { get; set; }

        public int warningqty { get; set; }

        public static explicit operator tbl_products_view(tbl_products v)
        {
            tbl_products_view a = new tbl_products_view
            {
                Id = v.Id,
                added_by = v.added_by,
                added_date = v.added_date,
                warningqty = v.warningqty,
                description = v.description,
                rate = v.rate,
                hasqty = v.hasqty,
                name = v.name,
                qty = v.qty
            };

            DAL.categoriesDAL dcDAL = new DAL.categoriesDAL();
            tbl_categories b = dcDAL.Search((int)v.category);
            a.category = b.title;

            return a;
        }
    }
}
