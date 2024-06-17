using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PassionProjectN01661067.Models
{
    public class Product
    {
        [Key]
        public int ProductId {  get; set; }
        public string ProductName { get; set; }

        public decimal ProductPrice { get; set; }

        public int StockQuantity { get; set; }    

        public int NoOfTransactions { get; set; }

        [ForeignKey("Supplier")]
        public int SupplierId { get; set; }
        public virtual Supplier Supplier { get; set; }

    }
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        public decimal ProductPrice { get; set; }

        public int StockQuantity { get; set; }

        public int NoOfTransactions { get; set; }

        public int SupplierId { get; set; }
    }
}