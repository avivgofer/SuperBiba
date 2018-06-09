using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StoreApp.Models
{
    public class OrderDetails
    {
        [Key]
        public int OrderID { get; set; }
        public string userName { get; set; }
        public DateTime orderTime { get; set; }
        public double total { get; set; }
        public virtual ICollection<Product> Cart { get; set; }
    }
}
