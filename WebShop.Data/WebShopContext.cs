namespace WebShop.Data
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using WebShop.Models;

    public class WebShopContext : IdentityDbContext<ApplicationUser>
    {
        public WebShopContext()
            : base("WebShopContext", throwIfV1Schema: false)
        {
        }

        public static WebShopContext Create()
        {
            return new WebShopContext();
        }
    }
}
