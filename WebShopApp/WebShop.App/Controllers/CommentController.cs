using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebShop.App.ViewModels.Comment;
using WebShop.Data;
using WebShop.Models;

namespace WebShop.App.Controllers
{
    public class CommentController : Controller
    {
        // GET: Comment
        public ActionResult Index()
        {
            return View();
        }

        //
        //Get:Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        //
        //Post: Comment/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(Comment comment, int? id)
        {
            if (ModelState.IsValid)
            {
                //insert in DB
                using (var db = new WebShopContext())
                {
                    var authorId = db.Users.Where(u => u.UserName == this.User.Identity.Name).First().Id;
                    var productId = db.Products.FirstOrDefault(p => p.Id == id).Id;
                    var addedOn = DateTime.Now;
                    var email = db.Users.FirstOrDefault(e => e.Email == this.User.Identity.Name).Email;

                    comment.Email = email;
                    comment.AddedOn = addedOn;
                    comment.AuthorId = authorId;
                    comment.ProductId = productId;

                    db.Comments.Add(comment);
                    db.SaveChanges();

                    return RedirectToAction("Details", "Product", new { @id = productId });
                }
            }

            return View(comment);
        }

        //
        //Get: Comment/Delete
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var db = new WebShopContext())
            {
                //get comment
                var comment = db.Comments
                    .Where(c => c.Id == id)
                    .First();

                if (!IsAuthor(comment))
                {
                    if (!IsAdmin(comment))
                    {
                        return RedirectToAction("List", "Product");
                    }
                }

                //check if exists
                if (comment == null)
                {
                    return HttpNotFound();
                }
                //pass to the view
                return View(comment);
            }
        }

        //
        //Post: Comment/Delete
        [HttpPost]
        [Authorize]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmation(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var db = new WebShopContext())
            {
                //get comment
                var comment = db.Comments
                    .Where(c => c.Id == id)
                    .First();

                var productId = comment.ProductId;

                //check if exists
                if (comment == null)
                {
                    return HttpNotFound();
                }

                //remove product
                db.Comments.Remove(comment);
                db.SaveChanges();

                //redirect to /product/list
                return RedirectToAction("Details", "Product", new { id = productId });
            }
        }

        //
        //Get: Product/Edit
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var db = new WebShopContext())
            {
                //get products
                var comment = db.Comments
                    .Where(p => p.Id == id)
                    .First();

                if (!IsAuthor(comment))
                {
                    if (!IsAdmin(comment))
                    {
                        return RedirectToAction("List", "Product");
                    }
                }


                //check if exists
                if (comment == null)
                {
                    return HttpNotFound();
                }

                //create view model
                var model = new CommentViewModel();

                model.ProductId = comment.ProductId;
                model.Id = comment.Id;
                model.Content = comment.Content;

                //pass to view
                return View(model);
            }
        }

        //
        //Post: Product/Edit
        [HttpPost]
        [Authorize]
        public ActionResult Edit(CommentViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new WebShopContext())
                {
                    //get comment
                    var comment = db.Comments
                        .FirstOrDefault(p => p.Id == model.Id);

                    var productId = comment.ProductId;

                    //set the same props
                    comment.Id = model.Id;
                    comment.Content = model.Content;

                    //save in db
                    db.Entry(comment).State = EntityState.Modified;
                    db.SaveChanges();

                    //redirect to /product/details/productID
                    return RedirectToAction("Details", "Product", new { id = productId });
                }
            }

            return View(model);
        }

        private bool IsAdmin(Comment comment)
        {
            bool isAdmin = this.User.IsInRole("Admin");

            return isAdmin;
        }

        private bool IsAuthor(Comment comment)
        {
            using (var db = new WebShopContext())
            {
                var authorId = db.Users.FirstOrDefault(u => u.Email == this.User.Identity.Name).Id;

                bool isAuthor = comment.AuthorId == authorId;

                return isAuthor;
            }
        }
    }
}