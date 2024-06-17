using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Description;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using PassionProjectN01661067.Models;

namespace PassionProjectN01661067.Controllers
{
    public class ProductController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static ProductController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44365/api/productdata/");
        }
        /// <summary>
        /// Displays a list of Products.
        /// </summary>
        /// <returns>
        /// The view containing the list of Products.
        /// </returns>
        /// <example>
        /// GET: Product/List
        /// </example>
        public ActionResult List()
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;


            string url = "listproducts";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<ProductDto> Products = response.Content.ReadAsAsync<IEnumerable<ProductDto>>().Result;
            return View(Products);
        }

        // GET: Product/Show{id}System.AggregateException: 'One or more errors occurred.'
        /// <summary>
        /// Displays details of a specific product.
        /// </summary>
        /// <param name="id">The ID of the product to be displayed.</param>
        /// <returns>
        /// The view containing details of the specified product.
        /// </returns>
        /// <example>
        /// GET: Product/Show/9
        /// </example>
        public ActionResult Show(int id)

        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            string url = "findproduct/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            ProductDto selectedproduct = response.Content.ReadAsAsync<ProductDto>().Result;
            Debug.WriteLine("product received : ");
            Debug.WriteLine(selectedproduct.ProductName);


            return View(selectedproduct);
        }

        public ActionResult Error()
        {

            return View();
        }

        /// <summary>
        /// Displays a form for creating a new Product.
        /// </summary>
        /// <returns>
        /// The view containing the form for creating a new Product.
        /// </returns>
        /// <example>
        /// GET: Product/New
        /// </example>
        public ActionResult New()
        {
            return View();
        }

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="product">The product to be created.</param>
        /// <returns>
        /// Redirects to the list of products upon successful creation, otherwise redirects to an error page.
        /// </returns>
        /// <example>
        /// POST: Product/Create
        /// </example>

        [HttpPost]
        public ActionResult Create(Product product)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(product.ProductName);
            //objective: add a new Product into our system using the API
            //curl -H "Content-Type:application/json" -d @product.json https://localhost:44324/api/productdata/addproduct
            string url = "addproduct";


            string jsonpayload = jss.Serialize(product);

            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                Debug.WriteLine("Error: " + response.StatusCode);
                Debug.WriteLine("Response: " + response.Content.ReadAsStringAsync().Result);
                return RedirectToAction("Error");
            }


        }

        /// <summary>
        /// Displays a form for editing a product.
        /// </summary>
        /// <param name="id">The ID of the product to be edited.</param>
        /// <returns>
        /// The view containing the form for editing the specified product.
        /// </returns>
        /// <example>
        /// GET: product/Edit/5
        /// </example>
        public ActionResult Edit(int id)
        {

            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            //objective: communicate with our product data api to retrieve one product
            /*//curl https://localhost:44324/api/productdata/findproduct/{id}*/

            string url = "findproduct/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            ProductDto selectedproduct = response.Content.ReadAsAsync<ProductDto>().Result;
            //Debug.WriteLine("product received : ");
            //Debug.WriteLine(selectedproduct.ProductName);

            return View(selectedproduct);
        }

        /// <summary>
        /// Updates a product.
        /// </summary>
        /// <param name="id">The ID of the product to be updated.</param>
        /// <param name="product">The updated product data.</param>
        /// <returns>
        /// Redirects to the details page of the updated product upon successful update, otherwise returns to the edit page.
        /// </returns>
        /// <example>
        /// POST: product/Update/5
        /// </example>

        // POST: Product/Update/5
        [HttpPost]
        public ActionResult Update(int id, Product product)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            try
            {
                Debug.WriteLine("The new product info is:");
                /*Debug.WriteLine(product.ProductId);
                Debug.WriteLine(product.ProductName);*/
                /*Debug.WriteLine(product.ProductPrice);*/

                //serialize into JSON
                //Send the request to the API

                string url = "UpdateProduct/" + id;


                string jsonpayload = jss.Serialize(product);
                Debug.WriteLine(jsonpayload);

                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";

                //POST: api/ProductData/UpdateProduct/{id}
                //Header : Content-Type: application/json
                HttpResponseMessage response = client.PostAsync(url, content).Result;




                return RedirectToAction("Show/" + id);
            }
            catch
            {
                return View();
            }

        }

        /// <summary>
        /// Displays a confirmation page for deleting a product.
        /// </summary>
        /// <param name="id">The ID of the product to be deleted.</param>
        /// <returns>
        /// The view containing the confirmation message for deleting the specified product.
        /// </returns>
        /// <example>
        /// GET: Product/Delete/5
        /// </example>
        // GET: Product/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            string url = "findproduct/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ProductDto selectedproduct = response.Content.ReadAsAsync<ProductDto>().Result;
            return View(selectedproduct);
        }
        /// <summary>
        /// Deletes a product.
        /// </summary>
        /// <param name="id">The ID of the product to be deleted.</param>
        /// <returns>
        /// Redirects to the list of products upon successful deletion, otherwise redirects to an error page.
        /// </returns>
        /// <example>
        /// POST: product/Delete/5
        /// </example>
        // POST: Product/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            string url = "deleteproduct/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
    }

