using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using PassionProjectN01661067.Models;

namespace PassionProjectN01661067.Controllers
{

    public class ProductDataController : ApiController
    {
        //GET: /api/ProductData/ListProducts
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all products in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all products in the database.
        /// </returns>
        /// <example>
        /// GET: api/ProductData/ListProducts
        /// </example>

        [HttpGet]
        [Route("api/ProductData/ListProducts")]
        public IEnumerable<ProductDto> ListProducts()
        {
            List<Product> Products = db.Products.ToList();
            List<ProductDto> ProductDtos = new List<ProductDto>();

            Products.ForEach(a => ProductDtos.Add(new ProductDto()
            {
                //ProductDto Item = new ProductDto();
                ProductId = a.ProductId,
                ProductName = a.ProductName,
                ProductPrice = a.ProductPrice,
                StockQuantity =a.StockQuantity,
                NoOfTransactions =a.NoOfTransactions,
                SupplierId = a.Supplier.SupplierId

            }));
     
            return ProductDtos;
        }

        /// <summary>
        /// Finds a specific product by ID.
        /// </summary>
        /// <param name="id">The ID of the product.</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: A product matching the provided ID.
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// GET: api/ProductData/FindProduct/5
        /// </example>
        //[HttpGet]
        //[Route("api/ProductData/FindProduct/{id}")]
        [ResponseType(typeof(Product))]
        [HttpGet]
        [Route("api/ProductData/FindProduct/{id}")]
        public IHttpActionResult FindProduct(int id)
        {
            //Product Item = db.Products.Find(id);


            //ProductDto ProductDto = new ProductDto();
            //ProductDto.ProductId = Item.ProductId;
            //ProductDto.ProductName = Item.ProductName;
            //ProductDto.ProductPrice = Item.ProductPrice;
            //return ProductDto;

            Product Product = db.Products.Find(id);
            
            ProductDto ProductDto = new ProductDto()
            {
                ProductId = Product.ProductId,
                ProductName = Product.ProductName,
                ProductPrice = Product.ProductPrice,
                StockQuantity = Product.StockQuantity,
                NoOfTransactions = Product.NoOfTransactions,
                SupplierId = Product.Supplier.SupplierId

            };

            if (Product == null)
            {
                return NotFound();
            }


            return Ok(ProductDto);
        }


        /// <summary>
        /// Updates a product in the system with POST Data input.
        /// </summary>
        /// <param name="id">The ID of the product to be updated.</param>
        /// <param name="product">JSON form data of the product.</param>
        /// <returns>
        /// HEADER: 204 (No Content)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/ProductData/UpdateProduct/5
        /// </example>
        // POST: api/ProductData/UpdateProduct/5
        [ResponseType(typeof(void))]
        [Route("api/ProductData/UpdateProduct/{id}")]
        [HttpPost]
        public IHttpActionResult UpdateProduct(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.ProductId)
            {

                return BadRequest();
            }

            db.Entry(product).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Adds a new product to the system.
        /// </summary>
        /// <param name="product">JSON form data of the product.</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: The created product data.
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/ProductData/AddProduct
        /// </example>

        // POST: api/ProductData/AddProduct
        [ResponseType(typeof(Product))]
        [Route("api/ProductData/AddProduct")]
        [HttpPost]
        public IHttpActionResult AddProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Products.Add(product);
            db.SaveChanges();

            return Ok();
        }


        /// <summary>
        /// Deletes a product from the system by ID.
        /// </summary>
        /// <param name="id">The ID of the product to be deleted.</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/ProductData/DeleteProduct/5
        /// </example>
        // POST: api/ProductData/DeleteProduct/5
        [ResponseType(typeof(Product))]
        [Route("api/ProductData/DeleteProduct/{id}")]
        [HttpPost]
        public IHttpActionResult DeleteProduct(int id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            db.Products.Remove(product);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id)
        {
            return db.Products.Count(e => e.ProductId == id) > 0;
        }
    }
}
