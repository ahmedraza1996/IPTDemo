using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Test2.Models;

namespace Test2.Controllers.apiController
{
    public class OProductController : ApiController
    {

        public HttpResponseMessage GetProductList()
        {
            List<Product> lstProduct = new List<Product>();
            string whereclause = "";
            lstProduct = ProductManager.GetProduct(whereclause, null);

            return Request.CreateResponse(HttpStatusCode.OK, lstProduct);


        }
        public HttpResponseMessage GetProductDetail([FromBody]string id)
        {
            List<Product> lstProduct = new List<Product>();
            string whereclause = "";
            if (!string.IsNullOrEmpty(id))
            {
                whereclause = "ProductID='" + id + "'";
            }

            lstProduct = ProductManager.GetProduct(whereclause, null);

            return Request.CreateResponse(HttpStatusCode.OK, lstProduct[0]);


        }
        [HttpPost]
        public HttpResponseMessage SaveProduct([FromBody]Product model)
        {

            string ret = ProductManager.SaveProduct(model);
            if (ret.Equals(Shared.Constants.MSG_OK_DBSAVE.Text))
            {
                return Request.CreateResponse(HttpStatusCode.OK, Shared.Constants.MSG_OK_DBSAVE.Text);

            }

            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }
    }
}
