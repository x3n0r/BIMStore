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
    class animalsDAL
    {
        //Static String Method for Database Connection String
        static DataClasses1DataContext db = new DataClasses1DataContext();

        #region Select Method
        public List<tbl_animal> Select()
        {
            //To hold the data from database 
            List<tbl_animal> animals = new List<tbl_animal>();
            try
            {
                //Query to Get Data From Database
                animals = db.tbl_animal.ToList<tbl_animal>();

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return animals;
        }
        #endregion
        #region Insert New Category
        public bool Insert(animalsBLL a)
        {
            //Creating A Boolean VAriable and set its default value to false
            bool isSuccess = false;

            try
            {
                tbl_animal anm = new tbl_animal();
                //Passing Values through parameter
                anm.cust_id = a.cust_id;
                anm.date_of_birth = a.date_of_birth;
                //anm.Id = a.Id;
                anm.name = a.name;
                anm.notes = a.notes;
                anm.race = a.race;
                anm.species = a.species;
                db.tbl_animal.InsertOnSubmit(anm);
                db.SubmitChanges();

                //If the query is executed Successfully then the value to rows will be greater than 0 else it will be less than 0
                if (anm.Id > 0)
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
        public bool Update(animalsBLL a)
        {
            //Creating Boolean variable and set its default value to false
            bool isSuccess = false;

            try
            {
                var erg = from anm in db.tbl_animal
                          where anm.Id == a.Id
                          select anm;

                tbl_animal myAnm = erg.FirstOrDefault();
                if (myAnm != null)
                {
                    //Passing Value using cmd
                    myAnm.date_of_birth = a.date_of_birth;
                    myAnm.name = a.name;
                    myAnm.notes = a.notes;
                    myAnm.race = a.race;
                    myAnm.species = a.species;
                    myAnm.cust_id = a.cust_id;
                    //myAnm.Id = c.Id;
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
        public bool Delete(animalsBLL a)
        {
            //Create a Boolean variable and set its value to false
            bool isSuccess = false;

            try
            {

                var erg = from anm in db.tbl_animal
                          where anm.Id == a.Id
                          select anm;

                db.tbl_animal.DeleteOnSubmit(erg.FirstOrDefault());
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
        public List<tbl_animal> Search(int custid, string keywords)
        {
            //To hold the data from database 
            List<tbl_animal> anms = new List<tbl_animal>();
            try
            {
                var erg = from anm in db.tbl_animal
                          where anm.cust_id == custid && 
                                ( SqlMethods.Like(anm.name, "%" + keywords + "%") ||
                                SqlMethods.Like(anm.notes, "%" + keywords + "%") )
                          select anm;

                anms = erg.ToList<tbl_animal>();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return anms;
        }
        #endregion
        #region Select method for Product Module with cust_id
        public List<tbl_animal> SelectCustId(int cust_id)
        {
            //To hold the data from database 
            List<tbl_animal> prod = new List<tbl_animal>();

            try
            {
                var erg = from anms in db.tbl_animal
                          where anms.cust_id == cust_id
                          select anms;

                prod = erg.ToList<tbl_animal>();
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
