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
    class categoriesDAL
    {
        //Static String Method for Database Connection String
        static Context db = new Context();

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
        public bool Insert(tbl_categories c)
        {
            //Creating A Boolean VAriable and set its default value to false
            bool isSuccess = false;

            try
            {
                db.tbl_categories.Add(c);
                db.SaveChanges();

                //If the query is executed Successfully then the value to rows will be greater than 0 else it will be less than 0
                if (c.Id > 0)
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
        public bool Update(tbl_categories c)
        {
            //Creating Boolean variable and set its default value to false
            bool isSuccess = false;

            try
            {
                var erg = from cat in db.tbl_categories
                          where cat.Id == c.Id
                          select cat;

                tbl_categories myCat = erg.SingleOrDefault();
                if (myCat != null)
                {
                    //Passing Value using cmd
                    //myCat.Id = c.Id;
                    myCat.title = c.title;
                    myCat.description = c.description;
                    myCat.added_date = c.added_date;
                    myCat.added_by = c.added_by;
                    myCat.tax = c.tax;

                    db.SaveChanges();
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
        public bool Delete(tbl_categories c)
        {
            //Create a Boolean variable and set its value to false
            bool isSuccess = false;

            try
            {

                var erg = from cat in db.tbl_categories
                          where cat.Id == c.Id
                          select cat;

                db.tbl_categories.Remove(erg.SingleOrDefault());
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
        #region Method for Search Functionality
        public List<tbl_categories> Search(string keywords)
        {
            //To hold the data from database 
            List<tbl_categories> cats = new List<tbl_categories>();
            try
            {
                var erg = from cat in db.tbl_categories
                          where cat.title.Contains(keywords) ||
                                cat.description.Contains(keywords)
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
        #region Method for Search Functionality
        public tbl_categories Search(int catid)
        {
            //To hold the data from database 
            tbl_categories category = new tbl_categories();
            try
            {
                var erg = from cat in db.tbl_categories
                          where cat.Id == catid
                          select cat;
                tbl_categories cats = erg.SingleOrDefault();
                //If we have values on myDeaCust we need to save it
                if (cats != null)
                {
                    category.Id = cats.Id;
                    category.tax = cats.tax;
                    category.description = cats.description;
                    category.title = cats.title;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return category;
        }
        #endregion
    }
}
