using PassionProjectN01661067.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace PassionProjectN01661067.Controllers
{
    public class SupplierController : Controller
    {
        // GET: Supplier
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static SupplierController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                //cookies are manually set in RequestHeader
                UseCookies = false
            };
            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44365/api/supplierdata/");
        }

        /// <summary>
        /// Displays a list of Suppliers.
        /// </summary>
        /// <returns>
        /// The view containing the list of Suppliers.
        /// </returns>
        /// <example>
        /// GET: Supplier/List
        /// </example>
        /// 

        [Authorize]
        public ActionResult List()
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;


            string url = "listsuppliers";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<SupplierDto> Suppliers = response.Content.ReadAsAsync<IEnumerable<SupplierDto>>().Result;
            return View(Suppliers);
        }

        // GET: Supplier/Show{id}System.AggregateException: 'One or more errors occurred.'
        /// <summary>
        /// Displays details of a specific Supplier.
        /// </summary>
        /// <param name="id">The ID of the Supplier to be displayed.</param>
        /// <returns>
        /// The view containing details of the specified Supplier.
        /// </returns>
        /// <example>
        /// GET: Supplier/Show/9
        /// </example>
        public ActionResult Show(int id)

        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            string url = "findsupplier/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            SupplierDto selectedsupplier = response.Content.ReadAsAsync<SupplierDto>().Result;
            Debug.WriteLine("supplier received : ");
        


            return View(selectedsupplier);
        }

        public ActionResult Error()
        {

            return View();
        }
        /// <summary>
        /// Grabs the authentication cookie sent to this controller.
        /// For proper WebAPI authentication, you can send a post request with login credentials to the WebAPI and log the access token from the response. The controller already knows this token, so we're just passing it up the chain.
        ///
        /// </summary>
        private void GetApplicationCookie()
        {
            string token = "";
            //HTTP client is set up to be reused, otherwise it will exhaust server resources.
            //This is a bit dangerous because a previously authenticated cookie could be cached for
            //a follow-up request from someone else. Reset cookies in HTTP client before grabbing a new one.
            client.DefaultRequestHeaders.Remove("Cookie");
            if (!User.Identity.IsAuthenticated) return;

            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies.Get(".AspNet.ApplicationCookie");
            if (cookie != null) token = cookie.Value;

            //collect token as it is submitted to the controller
            //use it to pass along to the WebAPI.
            Debug.WriteLine("Token Submitted is : " + token);
            if (token != "") client.DefaultRequestHeaders.Add("Cookie", ".AspNet.ApplicationCookie=" + token);

            return;
        }
        /// <summary>
        /// Displays a form for creating a new Supplier.
        /// </summary>
        /// <returns>
        /// The view containing the form for creating a new Supplier.
        /// </returns>
        /// <example>
        /// GET: Supplier/New
        /// </example>
        public ActionResult New()
        {
            GetApplicationCookie();
            return View();
        }

        /// <summary>
        /// Creates a new Supplier.
        /// </summary>
        /// <param name="supplier">The Supplier to be created.</param>
        /// <returns>
        /// Redirects to the list of Suppliers upon successful creation, otherwise redirects to an error page.
        /// </returns>
        /// <example>
        /// POST: Supplier/Create
        /// </example>

        [HttpPost]
        public ActionResult Create(Supplier supplier)
        {
            GetApplicationCookie();
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            Debug.WriteLine("the json payload is :");
           
            //curl -H "Content-Type:application/json" -d @supplier.json https://localhost:44324/api/supplierdata/addsupplier
            string url = "addsupplier";


            string jsonpayload = jss.Serialize(supplier);

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
        /// Displays a form for editing a Supplier.
        /// </summary>
        /// <param name="id">The ID of the Supplier to be edited.</param>
        /// <returns>
        /// The view containing the form for editing the specified Supplier.
        /// </returns>
        /// <example>
        /// GET: supplier/Edit/5
        /// </example>
        public ActionResult Edit(int id)
        {
            GetApplicationCookie();
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            //objective: communicate with our Supplier data api to retrieve one Supplier
            /*//curl https://localhost:44324/api/supplierdata/findsupplier/{id}*/

            string url = "findsupplier/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            SupplierDto selectedsupplier = response.Content.ReadAsAsync<SupplierDto>().Result;
         

            return View(selectedsupplier);
        }

        /// <summary>
        /// Updates a Supplier.
        /// </summary>
        /// <param name="id">The ID of the Supplier to be updated.</param>
        /// <param name="supplier">The updated supplier data.</param>
        /// <returns>
        /// Redirects to the details page of the updated Supplier upon successful update, otherwise returns to the edit page.
        /// </returns>
        /// <example>
        /// POST: Supplier/Update/5
        /// </example>

        // POST: Supplier/Update/5
        [HttpPost]
        public ActionResult Update(int id, Supplier supplier)
        {
            GetApplicationCookie();
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            try
            {
              

                //serialize into JSON
                //Send the request to the API

                string url = "UpdateSupplier/" + id;


                string jsonpayload = jss.Serialize(supplier);
                Debug.WriteLine(jsonpayload);

                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";

                //POST: api/SupplierData/UpdateSupplier/{id}
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
        /// Displays a confirmation page for deleting a Supplier.
        /// </summary>
        /// <param name="id">The ID of the Supplier to be deleted.</param>
        /// <returns>
        /// The view containing the confirmation message for deleting the specified Supplier.
        /// </returns>
        /// <example>
        /// GET: Supplier/Delete/5
        /// </example>
        // GET: Supplier/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            GetApplicationCookie();
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            string url = "findsupplier/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            SupplierDto selectedsupplier = response.Content.ReadAsAsync<SupplierDto>().Result;
            return View(selectedsupplier);
        }
        /// <summary>
        /// Deletes a Supplier.
        /// </summary>
        /// <param name="id">The ID of the Supplier to be deleted.</param>
        /// <returns>
        /// Redirects to the list of Suppliers upon successful deletion, otherwise redirects to an error page.
        /// </returns>
        /// <example>
        /// POST: supplier/Delete/5
        /// </example>
        // POST: Supplier/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            GetApplicationCookie();
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            string url = "deletesupplier/" + id;
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


