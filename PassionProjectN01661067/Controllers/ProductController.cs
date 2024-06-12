using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using PassionProjectN01661067.Models;

namespace PassionProjectN01661067.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product/list
        public ActionResult List()
        {

            HttpClient client = new HttpClient();
            string url = "https://localhost:44365/api/productdata/listproducts";
            HttpResponseMessage response=client.GetAsync(url).Result;
            IEnumerable<ProductDto> Products=response.Content.ReadAsAsync<IEnumerable<ProductDto>>().Result;
            return View(Products);
        }

        // GET: Product/Show{id}
        

        public ActionResult Show(int id) 
        
        {
            return View();
        }
    }
}