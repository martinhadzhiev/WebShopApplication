using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebShop.Data;
using WebShop.Models;

namespace WebShop.App.Controllers
{
    public class CartController : Controller
    {
        public ActionResult List()
        {
            using (var db = new WebShopContext())
            {
                ApplicationUser user = CurrentUser();
                db.Users.Attach(user);

                var cart = db.Carts.Include(c => c.Products).Include(u => u.User).FirstOrDefault(c => c.UserId == user.Id);

                if (cart == null)
                {
                    return RedirectToAction("List", "Product");
                }

                return View(cart);
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult AddProduct(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var db = new WebShopContext())
            {
                var product = db.Products
                    .Where(a => a.Id == id)
                    .First();

                if (product == null)
                {
                    return HttpNotFound();
                }

                return View(product);
            }
        }

        [Authorize]
        [ActionName("AddProduct")]
        [HttpPost]
        public ActionResult AddProductConfirmed(int? id)
        {
            using (var db = new WebShopContext())
            {
                ApplicationUser user = CurrentUser();

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var product = db.Products.FirstOrDefault(p => p.Id == id);

                if (product == null)
                {
                    return HttpNotFound();
                }

                db.Users.Attach(user);
                if (user.Cart == null)
                {
                    Cart cart = new Cart();
                    db.Carts.Add(cart);
                    db.SaveChanges();
                    user.Cart = cart;
                    cart.User = user;
                    cart.UserId = user.Id;
                }
                user.Cart.Products.Add(product);
                db.SaveChanges();

                return RedirectToAction("List");
            }
        }

        public ApplicationUser CurrentUser()
        {
            using (var db = new WebShopContext())
            {

                return db.Users.FirstOrDefault(u => u.Email == this.User.Identity.Name);

            }
        }
    }
}