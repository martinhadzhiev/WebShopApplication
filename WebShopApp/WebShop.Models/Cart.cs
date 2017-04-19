using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebShop.Models
{
    public class Cart
    {
        public Cart()
        {
            this.Products = new HashSet<Product>();
        }
        public int Id { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public decimal? TotalPrice { get; set; }

    }
}
