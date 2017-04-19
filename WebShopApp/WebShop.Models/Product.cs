namespace WebShop.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    public class Product
    {
        public Product()
        {
            this.Comments = new HashSet<Comment>();
            this.Orders = new HashSet<Order>();
        }
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public DateTime AddedOn { get; set; }

        public decimal Price { get; set; }

        public string Review { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public string ImageUrl { get; set; }
    }
}
