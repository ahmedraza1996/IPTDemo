using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test2.Models;

namespace Test2.Controllers
{



    public class ProductController : Controller
    {
        public static string connstring = "server=localhost;user id = root; password=gsmgms12; database=fos";
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetProductList(string id)
        {

            IList<Product> lstProduct = new List<Product>();
            MySqlConnection connection = new MySqlConnection(connstring);
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }
            string sql = "select * from product";
            bool isdetail = false;
            if (!string.IsNullOrEmpty(id))
            {
                sql += " where productid= " + id;
                isdetail = true;
            }

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
                            Product objProduct = new Product();
                            objProduct.ProductID = (reader["productid"] == null || reader["productid"] is DBNull) ? 0 : Convert.ToInt32(reader["productid"]);
                            objProduct.ProductName = (reader["productname"] == null || reader["productname"] is DBNull) ? "" : Convert.ToString(reader["productname"]);
                            objProduct.Price = (reader["price"] == null || reader["price"] is DBNull) ? 0 : Convert.ToInt32(reader["price"]);


                            lstProduct.Add(objProduct);

                        }
                    }
                    else
                    {
                    }
                }

            }

            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
                connection.Dispose();
            }
            if (isdetail)
            {
                return View("GetProductDetail", lstProduct[0]);
            }
            return View(lstProduct);


        }
        public ActionResult GetProductbyId(string id)
        {
            MySqlConnection connection = new MySqlConnection(connstring);
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }
            Product objProduct = new Product();
            string sql = "select * from product";
            sql += " where ProductID=" + id;
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

                            objProduct.ProductID = (reader["productid"] == null || reader["productid"] is DBNull) ? 0 : Convert.ToInt32(reader["productid"]);
                            objProduct.ProductName = (reader["productname"] == null || reader["productname"] is DBNull) ? "" : Convert.ToString(reader["productname"]);
                            objProduct.Price = (reader["price"] == null || reader["price"] is DBNull) ? 0 : Convert.ToInt32(reader["price"]);

                        }
                    }
                    else
                    {
                    }
                }

            }

            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Dispose();
            }
            return View(objProduct);
        }

        public ActionResult AddProduct()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddProduct(Product product)
        {
            MySqlConnection connection = new MySqlConnection(connstring);
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }
            string sql = "Insert into Product (ProductName, Price) values(@ProductName, @Price)";
            var transaction = connection.BeginTransaction();
            using (MySqlCommand command = new MySqlCommand())
            {
                command.CommandText = sql;
                command.Connection = connection;
                command.Transaction = transaction;

                command.Parameters.AddWithValue("@ProductName", product.ProductName);
                command.Parameters.AddWithValue("@Price", product.Price);
                int affectedRows = command.ExecuteNonQuery();
                var lastInsertedId = command.LastInsertedId;
                if (affectedRows > 0)
                {
                    transaction.Commit();
                    connection.Close();
                    connection.Dispose();

                    return RedirectToAction("GetProductList");
                }
                else
                {
                    transaction.Rollback();
                    connection.Close();
                    connection.Dispose();
                }
            }

            return new HttpUnauthorizedResult();


        }

        public ActionResult EditProduct(string id)
        {
            MySqlConnection connection = new MySqlConnection(connstring);
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }
            Product objProduct = new Product();
            string sql = "select * from product where productid= " + id;
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

                            objProduct.ProductID = (reader["productid"] == null || reader["productid"] is DBNull) ? 0 : Convert.ToInt32(reader["productid"]);
                            objProduct.ProductName = (reader["productname"] == null || reader["productname"] is DBNull) ? "" : Convert.ToString(reader["productname"]);
                            objProduct.Price = (reader["price"] == null || reader["price"] is DBNull) ? 0 : Convert.ToInt32(reader["price"]);


                        }
                    }
                    else
                    {
                    }
                }

            }

            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
                connection.Dispose();
            }


            return View(objProduct);
        }
        [HttpPost]
        public ActionResult EditProduct(Product product)
        {
            MySqlConnection connection = new MySqlConnection(connstring);
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }
            string sql = "update product set ProductID=@ProductID, ProductName=@ProductName, Price=@Price where productid=@ProductID";

            var transaction = connection.BeginTransaction();
            using (MySqlCommand command = new MySqlCommand())
            {
                command.CommandText = sql;
                command.Connection = connection;
                command.Transaction = transaction;
                command.Parameters.AddWithValue("@ProductID", product.ProductID);
                command.Parameters.AddWithValue("@ProductName", product.ProductName);
                command.Parameters.AddWithValue("@Price", product.Price);
                int affectedRows = command.ExecuteNonQuery();

                if (affectedRows > 0)
                {
                    transaction.Commit();
                    connection.Close();
                    connection.Dispose();

                    return RedirectToAction("GetProductList", new { id = product.ProductID });
                }
                else
                {
                    transaction.Rollback();
                    connection.Close();
                    connection.Dispose();
                }
            }

            return new HttpUnauthorizedResult();

        }


        public ActionResult DeleteProduct(string id)
        {
            MySqlConnection connection = new MySqlConnection(connstring);
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }
            string sql = " delete from product where productID= " + id;
            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandText = sql;
                int affectedRows = command.ExecuteNonQuery();
                if (affectedRows > 0)
                {
                    connection.Close();
                    connection.Dispose();
                    return RedirectToAction("GetProductList");
                }
            }
            connection.Close();
            connection.Dispose();
            return new HttpUnauthorizedResult();
        }
    }
}
