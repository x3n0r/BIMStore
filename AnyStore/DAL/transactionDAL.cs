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
    class transactionDAL
    {
        //Static String Method for Database Connection String
        static DataClasses1DataContext db = new DataClasses1DataContext();

        #region Insert Transaction Method
        public bool Insert_Transaction(transactionsBLL t, out int transactionID)
        {
            //Create a boolean value and set its default value to false
            bool isSuccess = false;
            //Set the out transactionID value to negative 1 i.e. -1
            transactionID = -1;

            try
            {
                tbl_transactions trans = new tbl_transactions();
                //Passing the value to sql query using cmd
                trans.type = t.type;
                trans.dea_cust_id = t.dea_cust_id;
                trans.grandTotal = t.grandTotal;
                trans.transaction_date = t.transaction_date;
                trans.discount = t.discount;
                trans.added_by = t.added_by;
                db.tbl_transactions.InsertOnSubmit(trans);
                db.SubmitChanges();

                //If the query is executed Successfully then the value to rows will be greater than 0 else it will be less than 0
                if (trans.Id > 0)
                {
                    //Query Sucessfull
                    transactionID = trans.Id;
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
        #region METHOD TO DISPLAY ALL THE TRANSACTION
        public List<tbl_transactions> DisplayAllTransactions()
        {
            //To hold the data from database 
            List<tbl_transactions> trans = new List<tbl_transactions>();

            try
            {
                //Query to Get Data From Database
                trans = db.tbl_transactions.ToList<tbl_transactions>();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return trans;
        }
        #endregion
        #region METHOD TO DISPLAY TRANSACTION BASED ON TRANSACTION TYPE
        public List<tbl_transactions> DisplayTransactionByType(string type)
        {
            //To hold the data from database 
            List<tbl_transactions> trans = new List<tbl_transactions>();

            try
            {
                //Write SQL Query
                var erg = from tran in db.tbl_transactions
                          where tran.type == type
                          select tran;

                trans = erg.ToList<tbl_transactions>();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return trans;
        }
        #endregion
    }
}
