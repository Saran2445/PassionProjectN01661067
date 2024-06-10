using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PassionProjectN01661067.Models
{
    public class Supplier
    {
        [Key]
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        
        public string ContactPerson { get; set;}

        public string EmailAddress { get; set;} 

        public string SupplierAddress { get; set;}  
 
    }
}