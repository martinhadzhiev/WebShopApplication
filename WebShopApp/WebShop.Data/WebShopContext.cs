namespace WebShop.Data
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Data.Entity;
    using Models;
    using Migrations;

    public class WebShopContext : IdentityDbContext<ApplicationUser>
    {
        public WebShopContext()
            : base("WebShopContext", throwIfV1Schema: false)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<WebShopContext, Configuration>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>().HasOptional(u => u.Cart).WithOptionalDependent(c => c.User);

            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<Comment> Comments { get; set; }

        public virtual DbSet<Cart> Carts { get; set; }

        public virtual DbSet<Order> Orders { get; set; }


        public static WebShopContext Create()
        {
            return new WebShopContext();
        }
    }
}
