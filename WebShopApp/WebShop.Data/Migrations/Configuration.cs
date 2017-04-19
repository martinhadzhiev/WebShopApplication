namespace WebShop.Data.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<WebShop.Data.WebShopContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "WebShop.Data.WebShopContext";
        }

        protected override void Seed(WebShop.Data.WebShopContext context)
        {

            if (!context.Roles.Any())
            {
                this.CreateRole(context, "Admin");
                this.CreateRole(context, "User");
            }

            if (!context.Users.Any(u => u.Email == "admin@admin.com"))
            {
                this.CreateUser(context, "admin@admin.com", "123");
                this.SetRoleUser(context, "admin@admin.com", "Admin");
            }

        }

        private void CreateRole(WebShopContext context, string roleName)
        {
            var roleManager = new Microsoft.AspNet.Identity.RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));

            var result = roleManager.Create(new IdentityRole(roleName));

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(";", result.Errors));
            }
        }

        private void CreateUser(WebShopContext context, string email, string password)
        {
            //user manager
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            //set user manager pass validator
            userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 1,
                RequireDigit = false,
                RequireLowercase = false,
                RequireNonLetterOrDigit = false,
                RequireUppercase = false
            };

            //create user object
            var admin = new ApplicationUser
            {
                UserName = email,
                Email = email
            };

            //create user
            var result = userManager.Create(admin, password);

            //validate result
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(";", result.Errors));
            }
        }

        private void SetRoleUser(WebShopContext context, string email, string role)
        {
            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));

            var user = context.Users.Where(u => u.Email == email).First();

            var result = userManager.AddToRole(user.Id, role);

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(";", result.Errors));
            }
        }
    }
}
