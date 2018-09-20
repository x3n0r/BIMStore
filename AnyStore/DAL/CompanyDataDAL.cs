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
    class CompanyDataDAL
    {
        //Static String Method for Database Connection String
        static DataClasses1DataContext db = new DataClasses1DataContext();

        #region Select Method
        public List<tbl_companydata> Select()
        {
            //To hold the data from database 
            List<tbl_companydata> compdata = new List<tbl_companydata>();
            try
            {
                //Query to Get Data From Database
                compdata = db.tbl_companydata.ToList<tbl_companydata>();

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return compdata;
        }
        #endregion
        #region Update Method
        public bool Update(CompanyDataBLL cd)
        {
            //Creating Boolean variable and set its default value to false
            bool isSuccess = false;

            try
            {
                var erg = from compdata in db.tbl_companydata
                          where compdata.Id == cd.Id
                          select compdata;

                tbl_companydata myCompData = erg.FirstOrDefault();
                if (myCompData != null)
                {
                    //Passing Value using cmd
                    myCompData.name = cd.name;
                    myCompData.slogan = cd.slogan;
                    myCompData.address = cd.address;
                    myCompData.country = cd.country;
                    myCompData.telnb = cd.telnb;
                    myCompData.email = cd.email;
                    myCompData.IBAN = cd.IBAN;
                    myCompData.BIC = cd.BIC;
                    //myCompData.Id = c.id;
                    db.SubmitChanges();
                }

                isSuccess = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                //Failed to Execute Query
                isSuccess = false;
            }

            return isSuccess;
        }
        #endregion
    }
}
