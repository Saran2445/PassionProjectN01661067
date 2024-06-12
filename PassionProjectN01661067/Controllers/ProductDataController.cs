using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PassionProjectN01661067.Models;

namespace PassionProjectN01661067.Controllers
{

    public class ProductDataController : ApiController
    {
        //GET: /api/ProductData/ListProducts
        private ApplicationDbContext db = new ApplicationDbContext();
        [HttpGet]
        [Route("api/ProductData/ListProducts")]
        public List<ProductDto> ListProducts()
        {
            List<Product> Products = db.Products.ToList();
            List<ProductDto> ProductDtos = new List<ProductDto>();

            foreach (Product item in Products)
            {
                ProductDto Item = new ProductDto();
                Item.ProductId = item.ProductId;
                Item.ProductName = item.ProductName;
                Item.ProductPrice = item.ProductPrice;

                ProductDtos.Add(Item);
            }
            return ProductDtos;
        }


        [HttpGet]
        [Route("api/ProductData/FindProduct/{id}")]
        public ProductDto FindProduct(int id)
        {
            Product Item = db.Products.Find(id);

            
            ProductDto ProductDto = new ProductDto();
            ProductDto.ProductId = Item.ProductId;
            ProductDto.ProductName = Item.ProductName;
            ProductDto.ProductPrice = Item.ProductPrice;
            return ProductDto;
        }
    }
}
