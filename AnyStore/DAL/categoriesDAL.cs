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
    class categoriesDAL
    {
        //Static String Method for Database Connection String
        static DataClasses1DataContext db = new DataClasses1DataContext();

        #region Select Method
        public List<tbl_categories> Select()
        {
            //To hold the data from database 
            List<tbl_categories> categories = new List<tbl_categories>();
            try
            {
                //Query to Get Data From Database
                categories = db.tbl_categories.ToList<tbl_categories>();

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return categories;
        }
        #endregion
        #region Insert New Category
        public bool Insert(categoriesBLL c)
        {
            //Creating A Boolean VAriable and set its default value to false
            bool isSuccess = false;

            try
            {
                tbl_categories cat = new tbl_categories();
                //Passing Values through parameter
                cat.title = c.title;
                cat.description = c.description;
                cat.added_date = c.added_date;
                cat.added_by = c.added_by;
                db.tbl_categories.InsertOnSubmit(cat);
                db.SubmitChanges();

                //If the query is executed Successfully then the value to rows will be greater than 0 else it will be less than 0
                if (cat.Id > 0)
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
        #region Update Method
        public bool Update(categoriesBLL c)
        {
            //Creating Boolean variable and set its default value to false
            bool isSuccess = false;

            try
            {
                var erg = from cat in db.tbl_categories
                          where cat.Id == c.id
                          select cat;

                tbl_categories myCat = erg.FirstOrDefault();
                if (myCat != null)
                {
                    //Passing Value using cmd
                    myCat.title = c.title;
                    myCat.description = c.description;
                    myCat.added_date = c.added_date;
                    myCat.added_by = c.added_by;
                    //myCat.Id = c.id;
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
        #region Delete Category Method
        public bool Delete(categoriesBLL c)
        {
            //Create a Boolean variable and set its value to false
            bool isSuccess = false;

            try
            {

                var erg = from cat in db.tbl_categories
                          where cat.Id == c.id
                          select cat;

                db.tbl_categories.DeleteOnSubmit(erg.FirstOrDefault());
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
        #region Method for Search Functionality
        public List<tbl_categories> Search(string keywords)
        {
            //To hold the data from database 
            List<tbl_categories> cats = new List<tbl_categories>();
            try
            {
                var erg = from cat in db.tbl_categories
                          where SqlMethods.Like(cat.title, "%" + keywords + "%") ||
                                SqlMethods.Like(cat.description, "%" + keywords + "%")
                          select cat;

                cats = erg.ToList<tbl_categories>();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return cats;
        }
        #endregion
    }
}
