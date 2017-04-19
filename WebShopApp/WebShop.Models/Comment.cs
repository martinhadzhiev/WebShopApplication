using System;

namespace WebShop.Models
{
    public class Comment
    {

        public int Id { get; set; }

        public string Content { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public string Email { get; set; }

        public DateTime AddedOn { get; set; }

    }
}
