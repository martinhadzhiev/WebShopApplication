using WebShop.Models;

namespace WebShop.App.ViewModels.Comment
{
    using WebShop.Models;
    public class CommentViewModel
    {

        public int Id { get; set; }

        public string Content { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }

    }
}