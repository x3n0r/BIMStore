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
    class productsDAL
    {
        //Static String Method for Database Connection String
        static Context db = new Context();

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
        #region Select method for Product Module with hasqty true
        public List<tbl_products> SelectHasQTY()
        {
            //To hold the data from database 
            List<tbl_products> prod = new List<tbl_products>();

            try
            {
                var erg = from prods in db.tbl_products
                          where prods.hasqty == true
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
        #region Method to Insert Product in database
        public bool Insert(tbl_products p)
        {
            //Creating Boolean Variable and set its default value to false
            bool isSuccess = false;

            try
            {
                db.tbl_products.Add(p);
                db.SaveChanges();

                //If the query is executed Successfully then the value to rows will be greater than 0 else it will be less than 0
                if (p.Id > 0)
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
        public bool Update(tbl_products p)
        {
            //create a boolean variable and set its initial value to false
            bool isSuccess = false;

            try
            {
                var erg = from prods in db.tbl_products
                          where prods.Id == p.Id
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
                    myProd.hasqty = p.hasqty;
                    myProd.warningqty = p.warningqty;
                    //myProd.Id = p.id;
                    db.SaveChanges();
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
        public bool Delete(tbl_products p)
        {
            //Create Boolean Variable and Set its default value to false
            bool isSuccess = false;

            try
            {
                var erg = from prods in db.tbl_products
                          where prods.Id == p.Id
                          select prods;

                db.tbl_products.Remove(erg.FirstOrDefault());
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
        #region SEARCH Method for Product Module
        public List<tbl_products> Search (string keywords)
        {
            //To hold the data from database 
            List<tbl_products> prod = new List<tbl_products>();

            try
            {
                var erg = from prods in db.tbl_products
                          where prods.name.Contains(keywords)
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
        #region METHOD TO GET PRODUCT ID BASED ON PRODUCT NAME
        public tbl_products GetProductIDFromName(string ProductName)
        {
            //First Create an Object of DeaCust BLL and REturn it
            tbl_products p = new tbl_products();

            try
            {
                var erg = from prods in db.tbl_products
                          where prods.name.Contains(ProductName)
                          select prods;

                tbl_products myProd = erg.FirstOrDefault();
                if (myProd != null)
                {
                    p.Id = myProd.Id;
                    p.name = myProd.name;
                    p.rate = decimal.Parse(myProd.rate.ToString());
                    p.qty = decimal.Parse(myProd.qty.ToString());
                    p.hasqty = (bool)myProd.hasqty;
                    p.category = (int)myProd.category;
                    p.warningqty = myProd.warningqty;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return p;
        }
        #endregion
        #region METHOD TO GET PRODUCT BASED ON PRODUCT ID
        public tbl_products GetProductFromID(int productID)
        {
            //First Create an Object of DeaCust BLL and REturn it
            tbl_products p = new tbl_products();

            try
            {
                var erg = from prods in db.tbl_products
                          where prods.Id == productID
                          select prods;

                tbl_products myProd = erg.FirstOrDefault();
                if (myProd != null)
                {
                    p.Id = myProd.Id;
                    p.added_by = (int)myProd.added_by;
                    p.description = myProd.description;
                    p.hasqty = (bool)myProd.hasqty;
                    p.category = (int)myProd.category;
                    p.rate = myProd.rate;
                    p.qty = myProd.qty;
                    p.name = myProd.name;
                    p.warningqty = myProd.warningqty;
                        
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
                    qty = myProd.qty;
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
                    db.SaveChanges();
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

                //Decrease the Product Quantity based on product sale
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
        #region DISPLAY PRODUCTS BASED ON Warning
        public List<tbl_products> GetProductsByWarning()
        {
            //To hold the data from database 
            List<tbl_products> prod = new List<tbl_products>();
            try
            {
                var erg = from prods in db.tbl_products
                          where (prods.qty <= prods.warningqty && prods.warningqty != 0 ) &&
                          prods.hasqty == true
                          select prods;

                prod = erg.ToList<tbl_products>();
    
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
