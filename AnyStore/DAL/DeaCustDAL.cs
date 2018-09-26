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
    class DeaCustDAL
    {
        //Static String Method for Database Connection String
        static DataClasses1DataContext db = new DataClasses1DataContext();

        #region SELECT Method for Dealer and Customer
        public List<tbl_dea_cust> Select()
        {
            //To hold the data from database 
            List<tbl_dea_cust> deacust = new List<tbl_dea_cust>();

            try
            {
                //Query to Get Data From Database
                deacust = db.tbl_dea_cust.ToList<tbl_dea_cust>();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return deacust;
        }
        #endregion
        #region INSERT Method to Add details fo Dealer or Customer
        public bool Insert(DeaCustBLL dc)
        {
            //Create and Boolean value and set its default value to false
            bool isSuccess = false;
            try
            {
                tbl_dea_cust deacust = new tbl_dea_cust();
                //Passing the values using Parameters
                deacust.type = dc.type;
                deacust.first_name = dc.first_name;
                deacust.last_name = dc.last_name;
                deacust.form_of_address = dc.form_of_address;
                deacust.contact_mail = dc.contact_mail;
                deacust.contact_phone = dc.contact_phone;
                deacust.address_street = dc.address_street;
                deacust.address_postcode = dc.address_postcode;
                deacust.address_city = dc.address_city;
                deacust.address_country = dc.address_country;
                db.tbl_dea_cust.InsertOnSubmit(deacust);
                db.SubmitChanges();

                //If the query is executed Successfully then the value to rows will be greater than 0 else it will be less than 0
                if (deacust.Id > 0)
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
        #region UPDATE method for Dealer and Customer Module
        public bool Update(DeaCustBLL dc)
        {
            bool isSuccess = false;
            try
            {
                var erg = from deacust in db.tbl_dea_cust
                          where deacust.Id == dc.Id
                          select deacust;

                tbl_dea_cust myDeaCust = erg.FirstOrDefault();
                if (myDeaCust != null)
                {
                    //Passing the values through parameters
                    myDeaCust.type = dc.type;
                    myDeaCust.first_name = dc.first_name;
                    myDeaCust.last_name = dc.last_name;
                    myDeaCust.form_of_address = dc.form_of_address;
                    myDeaCust.contact_mail = dc.contact_mail;
                    myDeaCust.contact_phone = dc.contact_phone;
                    myDeaCust.address_street = dc.address_street;
                    myDeaCust.address_postcode = dc.address_postcode;
                    myDeaCust.address_city = dc.address_city;
                    myDeaCust.address_country = dc.address_country;
                    //myDeaCust.Id = dc.id;
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
        #region DELETE Method for Dealer and Customer Module
        public bool Delete(DeaCustBLL dc)
        {
            //Create a Boolean Variable and set its default value to false
            bool isSuccess = false;

            try
            {
                var erg = from deacust in db.tbl_dea_cust
                          where deacust.Id == dc.Id
                          select deacust;

                db.tbl_dea_cust.DeleteOnSubmit(erg.FirstOrDefault());
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
        #region SEARCH METHOD for Dealer and Customer Module
        public List<tbl_dea_cust> Search(string keyword)
        {
            //To hold the data from database 
            List<tbl_dea_cust> deacust = new List<tbl_dea_cust>();
            try
            {
                var erg = from deacusts in db.tbl_dea_cust
                          where SqlMethods.Like(deacusts.type, "%" + keyword + "%") ||
                                SqlMethods.Like(deacusts.first_name, "%" + keyword + "%") ||
                                SqlMethods.Like(deacusts.last_name, "%" + keyword + "%")
                          select deacusts;

                deacust = erg.ToList<tbl_dea_cust>();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return deacust;
        }
        #endregion
        #region METHOD TO SAERCH DEALER Or CUSTOMER FOR TRANSACTON MODULE
        public DeaCustBLL SearchDealerCustomerForTransaction(string keyword,string DeaorCust)
        {
            int i = 0;
            if (DeaorCust == "Dealer")
            {
                i = 1;
            } else
            {
                i = 2;
            }
            return SearchDealerCustomerForTransaction(keyword, i);
        }
        #endregion
        #region METHOD TO SAERCH DEALER Or CUSTOMER FOR TRANSACTON MODULE
        public DeaCustBLL SearchDealerCustomerForTransaction(string keyword)
        {
            return SearchDealerCustomerForTransaction(keyword, 0);
        }
        #endregion
        #region METHOD TO SAERCH DEALER Or CUSTOMER FOR TRANSACTON MODULE
        /* 0 nothing 1 dealer 2 customer*/
        public DeaCustBLL SearchDealerCustomerForTransaction(string keyword,int deacustnothing)
        {
            
            Dictionary<int, string> dict= new Dictionary<int, string>();
            dict.Add(0, "");
            dict.Add(1, "Dealer");
            dict.Add(2, "Customer");
            //Create an object for DeaCustBLL class
            DeaCustBLL dc = new DeaCustBLL();

            try
            {
                var erg = from deacusts in db.tbl_dea_cust
                          where ( SqlMethods.Like(deacusts.first_name, "%" + keyword + "%") ||
                          SqlMethods.Like(deacusts.last_name, "%" + keyword + "%") ) &&
                          SqlMethods.Like(deacusts.type,"%"+ dict[deacustnothing] + "%")                           
                          select deacusts;

                tbl_dea_cust myDeaCust = erg.FirstOrDefault();
                //If we have values on myDeaCust we need to save it in dealerCustomer BLL
                if (myDeaCust != null)
                {
                    dc.Id = myDeaCust.Id;
                    dc.type = myDeaCust.type;
                    dc.first_name = myDeaCust.first_name;
                    dc.last_name = myDeaCust.last_name;
                    dc.form_of_address = myDeaCust.form_of_address;
                    dc.contact_mail = myDeaCust.contact_mail;
                    dc.contact_phone = myDeaCust.contact_phone;
                    dc.address_street = myDeaCust.address_street;
                    dc.address_postcode = myDeaCust.address_postcode;
                    dc.address_city = myDeaCust.address_city;
                    dc.address_country = myDeaCust.address_country;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return dc;
        }
        #endregion
        #region METHOD TO GET ID OF THE DEALER OR CUSTOMER BASED ON NAME
        public DeaCustBLL GetDeaCustIDFromName(string FirstName, string LastName)
        {
            //First Create an Object of DeaCust BLL and REturn it
            DeaCustBLL dc = new DeaCustBLL();

            try
            {
                var erg = from deacusts in db.tbl_dea_cust
                          where deacusts.first_name == FirstName && deacusts.last_name == LastName
                          select deacusts;


                tbl_dea_cust myDeaCust = erg.FirstOrDefault();
                if (myDeaCust != null)
                {
                    dc.Id = myDeaCust.Id;
                    dc.type = myDeaCust.type;
                    dc.first_name = myDeaCust.first_name;
                    dc.last_name = myDeaCust.last_name;
                    dc.form_of_address = myDeaCust.form_of_address;
                    dc.contact_mail = myDeaCust.contact_mail;
                    dc.contact_phone = myDeaCust.contact_phone;
                    dc.address_street = myDeaCust.address_street;
                    dc.address_postcode = myDeaCust.address_postcode;
                    dc.address_city = myDeaCust.address_city;
                    dc.address_country = myDeaCust.address_country;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return dc;
        }
        #endregion

    }
}
