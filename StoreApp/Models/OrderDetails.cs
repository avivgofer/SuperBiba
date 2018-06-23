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
        public string UserID { get; set; }
        public DateTime OrderTime { get; set; }
        public double Total { get; set; }
        public virtual ICollection<Product> Cart { get; set; }
    }
}
