using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Test2.Models;

namespace Test2.Controllers
{
    public class OProductController : Controller
    {
        // GET: OProduct
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetProductList()
        {
            List<Product> lstProduct = new List<Product>();
            string whereclause = "";
            lstProduct = ProductManager.GetProduct(whereclause, null);

            return View(lstProduct);


        }
        public ActionResult GetProductDetail(string id)
        {
            List<Product> lstProduct = new List<Product>();
            string whereclause = "";
            if (!string.IsNullOrEmpty(id))
            {
                whereclause = "ProductID='" + id + "'";
            }
            
            lstProduct = ProductManager.GetProduct(whereclause, null);

            return View(lstProduct[0]);


        }
        public ActionResult SaveProduct(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                List<Product> lstProduct = ProductManager.GetProduct(" ProductID ='" + id + "'");
                if (lstProduct.Count > 0)
                {
                    return View("EditProduct",lstProduct.First());
                }

            }
            return View("AddProduct");
        }
        [HttpPost]
        public ActionResult SaveProduct(Product model)
        {
           
            string ret = ProductManager.SaveProduct(model);
            if (ret.Equals(Shared.Constants.MSG_OK_DBSAVE.Text))
            {
                return RedirectToAction("GetProductList");
            }

            return new HttpUnauthorizedResult();
        }
        public ActionResult DeleteProduct(string id) 
        {
            string result = "";
            if (!string.IsNullOrEmpty(id))
            {
                result = ProductManager.DeleteProduct(id, null);
                if (result.Equals(Shared.Constants.MSG_OK_DBSAVE.Text))
                {
                    return RedirectToAction("GetProductList");
                }
             
            }
            return new HttpUnauthorizedResult();
        }

    }
}