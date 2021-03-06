﻿using BIMStore.BLL;
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
    class CompanyDataDAL
    {
        //Static String Method for Database Connection String
        static Context db = new Context();

        #region Select Method
        public tbl_companydata Select()
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

            if (compdata.Count == 0)
            {
                return null;
            } else {
                return compdata[0];
            }
        }
        #endregion
        #region Insert Data in Database
        public bool Insert(tbl_companydata cd)
        {
            bool isSuccess = false;
            try
            {

                db.tbl_companydata.Add(cd);
                db.SaveChanges();

                //If the query is executed Successfully then the value to rows will be greater than 0 else it will be less than 0
                if (cd.Id > 0)
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return isSuccess;
        }
        #endregion
        #region Update Method
        public bool Update(tbl_companydata cd)
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
                    myCompData.address_street = cd.address_street;
                    myCompData.address_postcode = cd.address_postcode;
                    myCompData.address_country = cd.address_country;
                    myCompData.address_city = cd.address_city;
                    myCompData.contact_phone = cd.contact_phone;
                    myCompData.contact_email = cd.contact_email;
                    myCompData.IBAN = cd.IBAN;
                    myCompData.BIC = cd.BIC;
                    myCompData.logo = cd.logo;
                    //myCompData.Id = c.id;
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
    }
}
