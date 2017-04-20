using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebShop.Data;
using WebShop.Models;

namespace WebShop.App.Controllers
{
    public class OrderController : Controller
    {

        public ActionResult List()
        {
            using (var db = new WebShopContext())
            {
                var orders = db.Orders.Include(c => c.Products).Include(u => u.User).ToList();
                return View(orders);
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult ConfirmOrder(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var db = new WebShopContext())
            {
                var cart = db.Carts.Include(c => c.Products).FirstOrDefault(c => c.Id == id);

                return View(cart);
            }
        }

        [HttpPost]
        [Authorize]
        [ActionName("ConfirmOrder")]
        public ActionResult ConfirmOrderConf(int? id)
        {
            using (var db = new WebShopContext())
            {

                var cart = db.Carts.FirstOrDefault(c => c.Id == id);

                if (cart == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                Order order = new Order
                {
                    Cart = cart,
                    DeliveryDate = DateTime.Now.AddDays(10),
                };

                foreach (var product in cart.Products)
                {
                    order.Products.Add(product);
                }

                order.User = cart.User;

                db.Orders.Add(order);
                cart.Products.Clear();
                db.SaveChanges();

                return RedirectToAction("List", "Product");
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult Complete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var db = new WebShopContext())
            {
                var order = db.Orders.Include(o => o.Products).FirstOrDefault(o => o.Id == id);

                return View(order);
            }
        }

        [HttpPost]
        [Authorize]
        [ActionName("Complete")]
        public ActionResult CompleteConfirm(int? id)
        {
            using (var db = new WebShopContext())
            {
                var order = db.Orders.FirstOrDefault(o => o.Id == id);

                if (order == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }


                order.IsCompleted = true;
                db.SaveChanges();

                return RedirectToAction("List");
            }

        }
    }
}