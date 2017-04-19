namespace WebShop.App.Controllers
{
    using Data;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using ViewModels.Product;

    public class ProductController : Controller
    {

        //Get:Product
        public ActionResult Index()
        {
            return View("List");
        }

        public ActionResult List()
        {
            using (var db = new WebShopContext())
            {
                var products = db.Products.ToList();

                return View(products);
            }
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var db = new WebShopContext())
            {
                var product = db.Products
                    .Where(a => a.Id == id)
                    .Include(a => a.Comments)
                    .First();

                if (product == null)
                {
                    return HttpNotFound();
                }

                return View(product);
            }
        }

        //
        //Get: Product/Create
        [Authorize]
        public ActionResult Create()
        {
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }


            return View();
        }

        //
        //Post: Product/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(Product product, HttpPostedFileBase file)
        {
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                //insert in DB
                using (var db = new WebShopContext())
                {
                    if (file == null)
                    {
                        ModelState.AddModelError("", "You have to upload an image");
                        return View(product);
                    }

                    string imageName = System.IO.Path.GetFileName(file.FileName);
                    string physicalPath = Server.MapPath("~/UploadedImages/" + imageName);

                    file.SaveAs(physicalPath);

                    DateTime date = DateTime.Now;
                    product.AddedOn = date;
                    product.ImageUrl = imageName;

                    //save
                    db.Products.Add(product);
                    db.SaveChanges();

                    return RedirectToAction("List");
                }
            }

            return View(product);
        }


        //
        //Get: Product/Delete
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var db = new WebShopContext())
            {
                //get products
                var product = db.Products
                    .Where(p => p.Id == id)
                    .First();

                if (!IsUserAuthorizedToEdit(product))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                //check if exists
                if (product == null)
                {
                    return HttpNotFound();
                }
                //pass to the view
                return View(product);
            }

        }

        //
        //Post: Product/Delete
        [HttpPost]
        [ActionName("Delete")]
        [Authorize]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var db = new WebShopContext())
            {
                //get products
                var product = db.Products
                    .Where(p => p.Id == id)
                    .First();

                if (!IsUserAuthorizedToEdit(product))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                //check if exists
                if (product == null)
                {
                    return HttpNotFound();
                }

                //remove product
                db.Products.Remove(product);
                db.SaveChanges();

                //redirect to /product/list
                return RedirectToAction("List");
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
                var product = db.Products
                    .Where(p => p.Id == id)
                    .First();

                if (!IsUserAuthorizedToEdit(product))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
                //check if exists
                if (product == null)
                {
                    return HttpNotFound();
                }

                //create view model
                var model = new ProductViewModel();
                model.Id = product.Id;
                model.Name = product.Name;
                model.Price = product.Price;
                model.Review = product.Review;


                //pass to view
                return View(model);
            }
        }


        //
        //Post: Product/Edit
        [HttpPost]
        [Authorize]
        public ActionResult Edit(ProductViewModel model, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                using (var db = new WebShopContext())
                {


                    //get products
                    var product = db.Products
                        .FirstOrDefault(p => p.Id == model.Id);

                    if (!IsUserAuthorizedToEdit(product))
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                    }

                    if (file != null)
                    {
                        string ImageName = System.IO.Path.GetFileName(file.FileName);
                        string physicalPath = Server.MapPath("~/UploadedImages/" + ImageName);

                        file.SaveAs(physicalPath);

                        product.ImageUrl = ImageName;
                    }

                    //set the same props
                    product.Name = model.Name;
                    product.Price = model.Price;
                    product.Review = model.Review;

                    //save in db
                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();

                    //redirect to /product/list
                    return RedirectToAction($"Details/{product.Id}");
                }
            }

            return View(model);
        }

        private bool IsUserAuthorizedToEdit(Product product)
        {
            bool isAdmin = this.User.IsInRole("Admin");

            return isAdmin;
        }
    }
}