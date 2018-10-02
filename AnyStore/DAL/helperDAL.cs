using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnyStore.DAL
{
    class helperDAL
    {

        public static void check_buttons(TextBox txtid, Button btnadd, Button btndelete, Button btnupdate)
        {
            if (txtid.Text == "")
            {
                btnadd.Visible = true;
                btndelete.Visible = false;
                btnupdate.Visible = false;
            }
            else
            {
                btnadd.Visible = false;
                btndelete.Visible = true;
                btnupdate.Visible = true;
            }
        }

        public static Dictionary<string, string> DeaCustToPurchaseSale = new Dictionary<string, string>()
            {
                { "Dealer", "Purchase" },
                { "Customer", "Sale" }
            };
    }
}
