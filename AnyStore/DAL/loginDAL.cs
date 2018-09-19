using AnyStore.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnyStore.DAL
{
    class loginDAL
    {
        private List<tbl_users> users = new List<tbl_users>();
        DataClasses1DataContext db = new DataClasses1DataContext();

        #region check login with username password
        public userBLL loginCheck(loginBLL l)
        {
            userBLL u = new userBLL();

            //Connecting To DAtabase
            users = db.tbl_users.ToList<tbl_users>();
            try
            {

                //Query to check login
                var erg = from z in db.tbl_users
                          where z.username == l.username &&
                                z.password == l.password
                                select z;
                //E.g.
                //foreach (tbl_users zz in erg)
                //{
                //    ListViewItem item = new ListViewItem("" + zz.ToDos.td_name);
                //    item.SubItems.Add("" + Math.Round(zz.ti_duration, 2));
                //    item.SubItems.Add("" + zz.ti_id);
                //    today_listview.Items.Add(item);
                //}

                tbl_users myUser = erg.FirstOrDefault();
                if (myUser != null)
                {
                    u.user_type = myUser.user_type;
                    return u;
                }
                //Checking The rows in DataTable 
                //if (erg.Count()>0)
                //{
                    //Login Sucessful
                //    return u;
                //}
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return null;
        }
        #endregion
    }
}
