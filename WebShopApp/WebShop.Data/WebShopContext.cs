namespace WebShop.Data
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Data.Entity;
    using Models;

    public class WebShopContext : IdentityDbContext<ApplicationUser>
    {
        public WebShopContext()
            : base("WebShopContext", throwIfV1Schema: false)
        {
        }

        public DbSet<Product> Products { get; set; }

        public static WebShopContext Create()
        {
            return new WebShopContext();
        }
    }
}
