using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BIMStore.DAL
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

        private static bool alreadyExist(string _text, ref char KeyChar)
        {
            if (_text.IndexOf('.') > -1)
            {
                KeyChar = '.';
                return true;
            }
            if (_text.IndexOf(',') > -1)
            {
                KeyChar = ',';
                return true;
            }
            return false;
        }

        public static void txtBoxCheckNumber(KeyPressEventArgs e, TextBox txtBox)
        {
            if (!char.IsControl(e.KeyChar)
             && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        public static void txtBoxCheckDecimal(KeyPressEventArgs e,TextBox txtBox)
        {
            if (!char.IsControl(e.KeyChar)
    && !char.IsDigit(e.KeyChar)
    //&& e.KeyChar != '.' && e.KeyChar != ',')
    && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            //check if '.' , ',' pressed
            char sepratorChar = 's';
            //if (e.KeyChar == '.' || e.KeyChar == ',')
            if (e.KeyChar == ',')
            {
                // check if it's in the beginning of text not accept
                if (txtBox.Text.Length == 0) e.Handled = true;
                // check if it's in the beginning of text not accept
                if (txtBox.SelectionStart == 0) e.Handled = true;
                // check if there is already exist a '.' , ','
                if (alreadyExist(txtBox.Text, ref sepratorChar)) e.Handled = true;
                //check if '.' or ',' is in middle of a number and after it is not a number greater than 99
                if (txtBox.SelectionStart != txtBox.Text.Length && e.Handled == false)
                {
                    // '.' or ',' is in the middle
                    string AfterDotString = txtBox.Text.Substring(txtBox.SelectionStart);

                    if (AfterDotString.Length > 2)
                    {
                        e.Handled = true;
                    }
                }
            }
            //check if a number pressed

            if (Char.IsDigit(e.KeyChar))
            {
                //check if a coma or dot exist
                if (alreadyExist(txtBox.Text, ref sepratorChar))
                {
                    int sepratorPosition = txtBox.Text.IndexOf(sepratorChar);
                    string afterSepratorString = txtBox.Text.Substring(sepratorPosition + 1);
                    if (txtBox.SelectionStart > sepratorPosition && afterSepratorString.Length > 1)
                    {
                        e.Handled = true;
                    }

                }
            }
        }
    }
}
