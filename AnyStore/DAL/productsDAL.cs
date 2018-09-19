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
    class productsDAL
    {
        //Static String Method for Database Connection String
        static DataClasses1DataContext db = new DataClasses1DataContext();

        #region Select method for Product Module
        public List<tbl_products> Select()
        {
            //To hold the data from database 
            List<tbl_products> prod = new List<tbl_products>();

            try
            {
                //Query to Get Data From Database
                prod = db.tbl_products.ToList<tbl_products>();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return prod;
        }
        #endregion
        #region Method to Insert Product in database
        public bool Insert(productsBLL p)
        {
            //Creating Boolean Variable and set its default value to false
            bool isSuccess = false;

            try
            {
                tbl_products prod = new tbl_products();
                //Passign the values through parameters
                prod.name = p.name;
                prod.category = p.category;
                prod.description = p.description;
                prod.rate = p.rate;
                prod.qty = p.qty;
                prod.added_date = p.added_date;
                prod.added_by = p.added_by;
                db.tbl_products.InsertOnSubmit(prod);
                db.SubmitChanges();

                //If the query is executed Successfully then the value to rows will be greater than 0 else it will be less than 0
                if (prod.Id > 0)
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
        #region Method to Update Product in Database
        public bool Update(productsBLL p)
        {
            //create a boolean variable and set its initial value to false
            bool isSuccess = false;

            try
            {
                var erg = from prods in db.tbl_products
                          where prods.Id == p.id
                          select prods;

                tbl_products myProd = erg.FirstOrDefault();
                if (myProd != null)
                {
                    //Passing the values using parameters and cmd
                    myProd.name = p.name;
                    myProd.category = p.category;
                    myProd.description = p.description;
                    myProd.rate = p.rate;
                    myProd.qty = p.qty;
                    myProd.added_date = p.added_date;
                    myProd.added_by = p.added_by;
                    //myProd.Id = p.id;
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
        #region Method to Delete Product from Database
        public bool Delete(productsBLL p)
        {
            //Create Boolean Variable and Set its default value to false
            bool isSuccess = false;

            try
            {
                var erg = from prods in db.tbl_products
                          where prods.Id == p.id
                          select prods;

                db.tbl_products.DeleteOnSubmit(erg.FirstOrDefault());
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
        #region SEARCH Method for Product Module
        public List<tbl_products> Search (string keywords)
        {
            //To hold the data from database 
            List<tbl_products> prod = new List<tbl_products>();

            try
            {
                var erg = from prods in db.tbl_products
                          where SqlMethods.Like(prods.name, "%" + keywords + "%")
                          select prods;

                prod = erg.ToList<tbl_products>();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return prod;
        }
        #endregion
        #region METHOD TO SEARCH PRODUCT IN TRANSACTION MODULE
        public productsBLL GetProductsForTransaction(string keyword)
        {
            //Create an object of productsBLL and return it
            productsBLL p = new productsBLL();

            try
            {
                var erg = from prods in db.tbl_products
                          where SqlMethods.Like(prods.name, "%" + keyword + "%")
                          select prods;

                tbl_products myProd = erg.FirstOrDefault();
                if (myProd != null)
                {
                    p.id = myProd.Id;
                    p.name = myProd.name;
                    p.rate = decimal.Parse(myProd.rate.ToString());
                    p.qty = decimal.Parse(myProd.qty.ToString());
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return p;
        }
        #endregion
        #region METHOD TO GET PRODUCT ID BASED ON PRODUCT NAME
        public productsBLL GetProductIDFromName(string ProductName)
        {
            //First Create an Object of DeaCust BLL and REturn it
            productsBLL p = new productsBLL();

            try
            {
                var erg = from prods in db.tbl_products
                          where SqlMethods.Like(prods.name, "%" + ProductName + "%")
                          select prods;

                tbl_products myProd = erg.FirstOrDefault();
                if (myProd != null)
                {
                    p.id = myProd.Id;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return p;
        }
        #endregion
        #region METHOD TO GET CURRENT QUantity from the Database based on Product ID
        public decimal GetProductQty(int ProductID)
        {
            //Create a Decimal Variable and set its default value to 0
            decimal qty = 0;

            try
            {
                var erg = from prods in db.tbl_products
                          where prods.Id == ProductID
                          select prods;

                tbl_products myProd = erg.FirstOrDefault();
                //Lets check if the datatable has value or not
                if (myProd != null)
                {
                    myProd.Id = myProd.Id;
                    myProd.qty = decimal.Parse(myProd.qty.ToString());
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return qty;
        }
        #endregion
        #region METHOD TO UPDATE QUANTITY
        public bool UpdateQuantity(int ProductID, decimal Qty)
        {
            //Create a Boolean Variable and Set its value to false
            bool success = false;

            try
            {
                var erg = from prods in db.tbl_products
                          where prods.Id == ProductID
                          select prods;

                tbl_products myProd = erg.FirstOrDefault();
                if (myProd != null)
                {
                    //Passing the VAlue trhough parameters
                    myProd.qty = Qty;
                    //myProd.Id = p.id;
                    db.SubmitChanges();
                }

                success = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                success = false;
            }

            return success;
        }
        #endregion
        #region METHOD TO INCREASE PRODUCT
        public bool IncreaseProduct(int ProductID, decimal IncreaseQty)
        {
            //Create a Boolean Variable and SEt its value to False
            bool success = false;

            try
            {
                //Get the Current Qty From dAtabase based on id
                decimal currentQty = GetProductQty(ProductID);

                //Increase the Current Quantity by the qty purchased from Dealer
                decimal NewQty = currentQty + IncreaseQty;

                //Update the Prudcty Quantity Now
                success = UpdateQuantity(ProductID, NewQty);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return success;
        }
        #endregion
        #region METHOD TO DECREASE PRODUCT
        public bool DecreaseProduct(int ProductID, decimal Qty)
        {
            //Create Boolean Variable and SEt its Value to false
            bool success = false;

            try
            {
                //Get the Current product Quantity
                decimal currentQty = GetProductQty(ProductID);

                //Decrease the Product Quantity based on product sales
                decimal NewQty = currentQty - Qty;

                //Update Product in Database
                success = UpdateQuantity(ProductID, NewQty);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return success;
        }
        #endregion
        #region DISPLAY PRODUCTS BASED ON CATEGORIES
        public List<tbl_products> DisplayProductsByCategory(int category)
        {
            //To hold the data from database 
            List<tbl_products> prod = new List<tbl_products>();

            try
            {
                var erg = from prods in db.tbl_products
                          where prods.category == category
                          select prods;

                prod = erg.ToList<tbl_products>();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return prod;
        }
        #endregion
    }
}
