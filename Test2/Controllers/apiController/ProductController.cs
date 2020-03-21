using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Test2.Models;

namespace Test2.Controllers.apiController
{
    public class ProductController : ApiController
    {
        public static string connstring = "server=localhost;user id = root; password=gsmgms12; database=fos";
        [HttpGet]
        public Product GetProduct()
        {
            Product objProduct = new Product();
            objProduct.ProductID = 1;
            objProduct.ProductName = "ABC";
            objProduct.Price = 100;
            return objProduct;
        }

        public HttpResponseMessage GetProductList()
        {
            IList<Product> lstProduct = new List<Product>();
            MySqlConnection connection = new MySqlConnection(connstring);
            HttpStatusCode statusCode = HttpStatusCode.BadRequest;

            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }
            string sql = "select * from product";
          
            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandText = sql;
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        statusCode = HttpStatusCode.OK;
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

            return Request.CreateResponse(statusCode, lstProduct);
        }
        public HttpResponseMessage GetProductDetail([FromBody]string id)
        {
            IList<Product> lstProduct = new List<Product>();
            MySqlConnection connection = new MySqlConnection(connstring);
            HttpStatusCode statusCode = HttpStatusCode.BadRequest;

            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }
            string sql = "select * from product";
            if (!string.IsNullOrEmpty(id))
            {
                sql += " where productid='" + id + "'";
            }
            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandText = sql;
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        statusCode = HttpStatusCode.OK;
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

            return Request.CreateResponse(statusCode, lstProduct[0]);
        }
        [HttpPost]
        public HttpResponseMessage AddProduct([FromBody] Product product)
        {
            MySqlConnection connection = new MySqlConnection(connstring);
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }
            string sql = "Insert into Product (ProductName, Price) values(@ProductName, @Price)";
            HttpStatusCode statusCode = HttpStatusCode.Unauthorized;

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
                    statusCode = HttpStatusCode.OK;
                    return Request.CreateResponse(statusCode,"OK");
                }
                else
                {
                    transaction.Rollback();
                    connection.Close();
                    connection.Dispose();
                }
            }


            return Request.CreateResponse(HttpStatusCode.BadRequest);


        }
        [HttpPost]

        public HttpResponseMessage EditProduct([FromBody]Product product)
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

                    //return RedirectToAction("GetProductList", new { id = product.ProductID });
                    return Request.CreateResponse(HttpStatusCode.OK,"Record Updated");
                }
                else
                {
                    transaction.Rollback();
                    connection.Close();
                    connection.Dispose();
                }
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest);

        }
    }
}
