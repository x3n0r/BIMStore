using BIMStore.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace BIMStore.DAL
{
    class userDAL
    {
        //Static String Method for Database Connection String
        static Context db = new Context();

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
        public bool Insert(tbl_users u)
        {
            bool isSuccess = false;
            try
            {

                db.tbl_users.Add(u);
                db.SaveChanges();

                //If the query is executed Successfully then the value to rows will be greater than 0 else it will be less than 0
                if(u.Id>0)
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
        public bool Update(tbl_users u)
        {
            bool isSuccess = false;
            try
            {
                var erg = from user in db.tbl_users
                          where user.Id == u.Id
                          select user;

                tbl_users myUser = erg.FirstOrDefault();
                if (myUser != null)
                {
                    myUser.first_name = u.first_name;
                    myUser.last_name = u.last_name;
                    myUser.contact_email = u.contact_email;
                    myUser.username = u.username;
                    myUser.password = u.password;
                    myUser.contact_phone = u.contact_phone;
                    myUser.address = u.address;
                    myUser.gender = u.gender;
                    myUser.user_type = u.user_type;
                    myUser.added_date = u.added_date;
                    myUser.added_by = u.added_by;
                    //myUser.Id = u.id;
                    db.SaveChanges();
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
        public bool Delete(tbl_users u)
        {
            //Create a Boolean Variable and set its default value to false
            bool isSuccess = false;

            try
            {
                var erg = from user in db.tbl_users
                          where user.Id == u.Id
                          select user;

                db.tbl_users.Remove(erg.FirstOrDefault());
                db.SaveChanges();

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
                          where user.first_name.Contains(keywords) ||
                                user.last_name.Contains(keywords) ||
                                user.username.Contains(keywords)
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
        public tbl_users GetIDFromUsername (string username)
        {
            tbl_users u = new tbl_users();

            try
            {
                var erg = from user in db.tbl_users
                          where user.username == username
                          select user;


                tbl_users myUser = erg.FirstOrDefault();
                if (myUser != null)
                {
                    u.Id = myUser.Id;
                    u.first_name = myUser.first_name;
                    u.last_name = myUser.last_name;
                    u.contact_email = myUser.contact_email;
                    u.username = myUser.username;
                    u.contact_phone = myUser.contact_phone;
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
