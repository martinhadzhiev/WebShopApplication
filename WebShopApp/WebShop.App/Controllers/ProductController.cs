namespace WebShop.App.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class ProductController
    {

        //Get:Product

        public ActionResult Index()
        {
            return View("List");
        }
    }
}