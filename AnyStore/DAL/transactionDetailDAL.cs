using AnyStore.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnyStore.DAL
{
    class transactionDetailDAL
    {
        //Static String Method for Database Connection String
        static DataClasses1DataContext db = new DataClasses1DataContext();

        #region Insert Method for Transaction Detail
        public bool InsertTransactionDetail(transactionDetailBLL td)
        {
            //Create a boolean value and set its default value to false
            bool isSuccess = false;

            try
            {
                
                tbl_transaction_detail transdet = new tbl_transaction_detail();
                //Passing the values using cmd
                transdet.product_id = td.product_id;
                transdet.price = td.price;
                transdet.qty = td.qty;
                transdet.total = td.total;
                transdet.dea_cust_id = td.dea_cust_id;
                transdet.added_date = td.added_date;
                transdet.added_by = td.added_by;
                db.tbl_transaction_detail.InsertOnSubmit(transdet);
                db.SubmitChanges();

                //If the query is executed Successfully then the value to rows will be greater than 0 else it will be less than 0
                if (transdet.Id > 0)
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
    }
}
