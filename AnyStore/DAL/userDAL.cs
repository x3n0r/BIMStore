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
using System.Data.Linq.SqlClient;

namespace AnyStore.DAL
{
    class userDAL
    {
        //Static String Method for Database Connection String
        static DataClasses1DataContext db = new DataClasses1DataContext();

        #region Select Data from Database
        public List<tbl_users> Select()
        {
            //To hold the data from database 
            List<tbl_users> users = new List<tbl_users>();

            try
            {
                //Query to Get Data From Database
                users = db.tbl_users.ToList<tbl_users>();
            }
            catch (Exception ex)
            {
                //Throw Message if any error occurs
                MessageBox.Show(ex.Message);
            }
            //Return the value in DataTable
            return users;
        }
        #endregion
        #region Insert Data in Database
        public bool Insert(userBLL u)
        {
            bool isSuccess = false;
            try
            {

                tbl_users user = new tbl_users();
                //Passing Values through parameter
                user.first_name = u.first_name;
                user.last_name = u.last_name;
                user.email = u.email;
                user.username = u.username;
                user.password = u.password;
                user.contact = u.contact;
                user.address = u.address;
                user.gender = u.gender;
                user.user_type = u.user_type;
                user.added_date = u.added_date;
                //zeit.ti_duration = (float)Math.Round(duration, 2);
                user.added_by = u.added_by;
                db.tbl_users.InsertOnSubmit(user);
                db.SubmitChanges();

                //If the query is executed Successfully then the value to rows will be greater than 0 else it will be less than 0
                if(user.Id>0)
                {
                    //Query Sucessfull
                    isSuccess = true;
                }
                else
                {
                    //Query Failed
                    isSuccess = false;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return isSuccess;
        }
        #endregion
        #region Update data in Database
        public bool Update(userBLL u)
        {
            bool isSuccess = false;
            try
            {
                var erg = from user in db.tbl_users
                          where user.Id == u.id
                          select user;

                tbl_users myUser = erg.FirstOrDefault();
                if (myUser != null)
                {
                    myUser.first_name = u.first_name;
                    myUser.last_name = u.last_name;
                    myUser.email = u.email;
                    myUser.username = u.username;
                    myUser.password = u.password;
                    myUser.contact = u.contact;
                    myUser.address = u.address;
                    myUser.gender = u.gender;
                    myUser.user_type = u.user_type;
                    myUser.added_date = u.added_date;
                    myUser.added_by = u.added_by;
                    //myUser.Id = u.id;
                    db.SubmitChanges();
                }

                isSuccess = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                isSuccess = false;
            }
            return isSuccess;
        }
        #endregion
        #region Delete Data from Database
        public bool Delete(userBLL u)
        {
            //Create a Boolean Variable and set its default value to false
            bool isSuccess = false;

            try
            {
                var erg = from user in db.tbl_users
                          where user.Id == u.id
                          select user;

                db.tbl_users.DeleteOnSubmit(erg.FirstOrDefault());
                db.SubmitChanges();

                isSuccess = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                isSuccess = false;
            }
            return isSuccess;
        }
        #endregion
        #region Search User on Database usingKeywords
        public List<tbl_users> Search(string keywords)
        {
            //To hold the data from database 
            List<tbl_users> users = new List<tbl_users>();
            try
            {
                var erg = from user in db.tbl_users
                          where SqlMethods.Like(user.first_name, "%" + keywords + "%") ||
                                SqlMethods.Like(user.last_name, "%" + keywords + "%")  ||
                                SqlMethods.Like(user.username, "%" + keywords + "%")
                          select user;

                users = erg.ToList<tbl_users>();
            }
            catch (Exception ex)
            {
                //Throw Message if any error occurs
                MessageBox.Show(ex.Message);
            }
            //Return the value in DataTable
            return users;
        }
        #endregion
        #region Getting User ID from Username
        public userBLL GetIDFromUsername (string username)
        {
            userBLL u = new userBLL();

            try
            {
                var erg = from user in db.tbl_users
                          where user.username == username
                          select user;


                tbl_users myUser = erg.FirstOrDefault();
                if (myUser != null)
                {
                    u.id = myUser.Id;
                    u.first_name = myUser.first_name;
                    u.last_name = myUser.last_name;
                    u.email = myUser.email;
                    u.username = myUser.username;
                    u.contact = myUser.contact;
                    u.address = myUser.address;
                    u.gender = myUser.gender;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return u;
        }
        #endregion
    }
}
