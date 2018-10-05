using BIMStore.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BIMStore.DAL
{
    class transactionDetailDAL
    {
        //Static String Method for Database Connection String
        static Context db = new Context();

        #region Insert Method for Transaction Detail
        public bool InsertTransactionDetail(tbl_transaction_detail td)
        {
            //Create a boolean value and set its default value to false
            bool isSuccess = false;

            try
            {
                db.tbl_transaction_detail.Add(td);
                db.SaveChanges();

                //If the query is executed Successfully then the value to rows will be greater than 0 else it will be less than 0
                if (td.Id > 0)
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
        #region Search BY TRANSACTION-ID 
        public List<tbl_transaction_detail> SearchByID(int transID)
        {
            //To hold the data from database 
            List<tbl_transaction_detail> transd = new List<tbl_transaction_detail>();

            try
            {
                //Write SQL Query
                var erg = from trand in db.tbl_transaction_detail
                          where trand.dea_cust_id == transID
                          select trand;

                transd = erg.ToList<tbl_transaction_detail>();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return transd;
        }
        #endregion
    }
}
