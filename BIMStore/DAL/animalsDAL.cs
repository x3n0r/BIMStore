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
    class animalsDAL
    {
        //Static String Method for Database Connection String
        static Context db = new Context();

        #region Select Method
        public List<tbl_animals> Select()
        {
            //To hold the data from database 
            List<tbl_animals> animals = new List<tbl_animals>();
            try
            {
                //Query to Get Data From Database
                animals = db.tbl_animals.ToList<tbl_animals>();

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return animals;
        }
        #endregion
        #region Insert New Category
        public bool Insert(tbl_animals a)
        {
            //Creating A Boolean VAriable and set its default value to false
            bool isSuccess = false;

            try
            {
                db.tbl_animals.Add(a);
                db.SaveChanges();

                //If the query is executed Successfully then the value to rows will be greater than 0 else it will be less than 0
                if (a.Id > 0)
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
        public bool Update(tbl_animals a)
        {
            //Creating Boolean variable and set its default value to false
            bool isSuccess = false;

            try
            {
                var erg = from anm in db.tbl_animals
                          where anm.Id == a.Id
                          select anm;

                tbl_animals myAnm = erg.SingleOrDefault();

                if (myAnm != null)
                {
                    //Passing Value using cmd
                    //myAnm.Id = c.Id;
                    myAnm.date_of_birth = a.date_of_birth;
                    myAnm.name = a.name;
                    myAnm.notes = a.notes;
                    myAnm.race = a.race;
                    myAnm.species = a.species;
                    myAnm.cust_id = a.cust_id;
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
        public bool Delete(tbl_animals a)
        {
            //Create a Boolean variable and set its value to false
            bool isSuccess = false;

            try
            {

                var erg = from anm in db.tbl_animals
                          where anm.Id == a.Id
                          select anm;

                db.tbl_animals.Remove(erg.SingleOrDefault());
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
        public List<tbl_animals> Search(int custid, string keywords)
        {
            //To hold the data from database 
            List<tbl_animals> anms = new List<tbl_animals>();
            try
            {
                var erg = from anm in db.tbl_animals
                          where anm.cust_id == custid && 
                                ( anm.name.Contains(keywords) || anm.notes.Contains(keywords) )
                          select anm;

                anms = erg.ToList<tbl_animals>();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return anms;
        }
        #endregion
        #region Select method for Product Module with cust_id
        public List<tbl_animals> SelectCustId(int cust_id)
        {
            //To hold the data from database 
            List<tbl_animals> prod = new List<tbl_animals>();

            try
            {
                var erg = from anms in db.tbl_animals
                          where anms.cust_id == cust_id
                          select anms;

                prod = erg.ToList<tbl_animals>();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return prod;
        }
        #endregion
    }
}
