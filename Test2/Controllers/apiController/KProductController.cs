using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using SqlKata.Compilers;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Test2.Models;

namespace Test2.Controllers.apiController
{
    public class KProductController : ApiController
    {
        public static string connstring = "server=localhost;user id = root; password=gsmgms12; database=fos";
        public HttpResponseMessage GetProductList()
        {
            
            //IList<Product> lstProduct = new List<Product>();
            MySqlConnection connection = new MySqlConnection(connstring);
            var compiler = new MySqlCompiler();
            var db = new QueryFactory(connection, compiler);
            db.Connection.Open();
            //IEnumerable<dynamic> response = db.Query("product").Get();
            IEnumerable<IDictionary<string, object>> response;
            // sql = select * from product;
            response = db.Query("product")
                .Get()
                .Cast<IDictionary<string, object>>();

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
        public HttpResponseMessage GetProductDetail([FromBody]object obj)
        {
            var test = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert.ToString(obj));
            object _ProductID;
            test.TryGetValue("id", out _ProductID);
            MySqlConnection connection = new MySqlConnection(connstring);
            var compiler = new MySqlCompiler();
            var db = new QueryFactory(connection, compiler);
            db.Connection.Open();
            
            IEnumerable<IDictionary<string, object>> response;

            response = db.Query("product")
                .Where("ProductID", _ProductID)
                .Get()
                .Cast<IDictionary<string, object>>();

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
    }
}

