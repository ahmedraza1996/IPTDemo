using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using Test2.Shared;

namespace Test2.Models
{
    public class Product
    {
        public int ProductID { get; set; }

        [Required]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        [Required]
        public int Price { get; set; }
    }
    public class ProductManager: BaseManager
    {
        public static List<Product> GetProduct(string whereclause, MySqlConnection conn = null)
        {
            Product objProduct = null;
            List<Product> lstProduct = new List<Product>();
            try
            {
                bool isConnArgNull = (conn != null) ? false : true;
                MySqlConnection connection = (conn != null) ? conn : PrimaryConnection();
                tryOpenConnection(connection);
                string sql = "select * from Product";
                if (!string.IsNullOrEmpty(whereclause))
                    sql += " where " + whereclause;
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = sql;
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                objProduct = ReaderDataProduct(reader);
                                lstProduct.Add(objProduct);
                            }
                        }
                        else
                        {
                        }
                    }
                    if (isConnArgNull == true)
                    {
                        connection.Dispose();
                    }


                }
            }
            //endtry
            catch (Exception ex)
            {

            }
            return lstProduct;
        }
        private static Product ReaderDataProduct(MySqlDataReader reader)
        {

            Product objProduct = new Product();
            objProduct.ProductID = DbCheck.IsValidInt(reader["ProductID"]);
            objProduct.ProductName = DbCheck.IsValidString(reader["ProductName"]);
            objProduct.Price = DbCheck.IsValidInt(reader["Price"]);
            return objProduct;

        }
        public static string SaveProduct(Product objProduct, MySqlConnection conn = null, MySqlTransaction trans = null)
        {
            string returnMessage = "";
            string sProductID = "";
            sProductID = objProduct.ProductID.ToString();
            var templstProduct = GetProduct("ProductID = '" + sProductID + "'", conn);
            try
            {
                bool isConnArgNull = (conn != null) ? false : true;
                MySqlConnection connection = (conn != null) ? conn : PrimaryConnection();
                tryOpenConnection(connection);
                using (MySqlCommand command = new MySqlCommand())
                {
                    string sql;
                    bool isEdit = true;
                    if (templstProduct.Count <= 0)
                    {
                        isEdit = false;
                        sql = @"INSERT INTO Product(
                                                    ProductName,
                                                    Price
                                                    )
                                                    VALUES(
                                                     @ProductName,
                                                     @Price
                                                    )";
                    }
                    else
                    {
                        sql = @"Update Product set
                                                    ProductID=@ProductID,                                               
                                                    ProductName=@ProductName,
                                                    Price=@Price

                                                    Where ProductID=@ProductID";
                    }
                    if (trans != null)
                    {
                        command.Transaction = trans;
                    }
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = sql;
                    if (isEdit)
                    {
                        command.Parameters.AddWithValue("@ProductID", objProduct.ProductID);
                    }

                    command.Parameters.AddWithValue("@ProductName", objProduct.ProductName);
                    command.Parameters.AddWithValue("@Price", objProduct.Price);
                    

                    int affectedRows = command.ExecuteNonQuery();
                    var lastInsertID = command.LastInsertedId;
                    if (affectedRows > 0)
                    {
                        //    if (!isEdit)
                        //    {
                        //        returnMessage = lastInsertID.ToString();
                        //    }
                        //    else
                        {
                            returnMessage = Shared.Constants.MSG_OK_DBSAVE.Text;
                        }

                    }
                    else
                    {
                        returnMessage = Shared.Constants.MSG_ERR_DBSAVE.Text;
                    }
                }

                if (isConnArgNull == true)
                {
                    connection.Dispose();
                }
            }
            catch (Exception ex)
            {

            }

            return returnMessage;
        }

        public static string DeleteProduct(string ProductID, MySqlConnection conn = null)
        {
            string returnMessage = "";
            try
            {
                bool isConnArgNull = (conn != null) ? false : true;
                MySqlConnection connection = (conn != null) ? conn : PrimaryConnection();
                tryOpenConnection(connection);
                using (MySqlCommand command = new MySqlCommand())
                {
                    string sql;
                    sql = @"DELETE from Product Where ProductID = @ProductID";
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = sql;
                    command.Parameters.AddWithValue("@ProductID", ProductID);
                    int affectedRows = command.ExecuteNonQuery();
                    if (affectedRows > 0)
                    {
                        returnMessage = Shared.Constants.MSG_OK_DBSAVE.Text;
                    }
                    else
                    {
                        returnMessage = Shared.Constants.MSG_ERR_DBSAVE.Text;
                    }
                }

                if (isConnArgNull == true)
                {
                    connection.Dispose();
                }
            }
            catch (Exception ex)
            {

            }

            return returnMessage;
        }






    }
}