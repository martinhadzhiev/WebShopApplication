namespace WebShop.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public DateTime AddedOn { get; set; }

        public decimal Price { get; set; }

        public string Review { get; set; }
    }
}
