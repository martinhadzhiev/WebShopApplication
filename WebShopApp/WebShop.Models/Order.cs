using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Models
{
    public class Order
    {
        public Order()
        {
            this.Products = new HashSet<Product>();
        }
        public int Id { get; set; }

        public DateTime DeliveryDate { get; set; }

        public string Address { get; set; }

        public int CartId { get; set; }

        public virtual Cart Cart { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public bool IsCompleted { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
