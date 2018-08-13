using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StoreApp.Models
{
    public class StorageProducts
    {
        private int iD;

        public StorageProducts() {  }

     

        public StorageProducts(int prodId, string productName)
        {
            this.ProductID = prodId;
            this.ProductName = productName;
           
            this.Amount = 100;
            this.LastOrder = DateTime.Now;
        }
        [Key]
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public DateTime LastOrder { get; set; }
    
        public int Amount { get; set; }
    }
}
