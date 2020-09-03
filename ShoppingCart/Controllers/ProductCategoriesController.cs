using ShoppingCart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCart.Controllers
{
    public class ProductCategoriesController : Controller
    {
        private ShoppingCartEntities db = new ShoppingCartEntities();
        //
        // GET: /ProductCategories/
        public ActionResult Accessories()
        {
            var cat = db.Products.Where(x => x.Category.Type == "Accessories");
            return View(cat);
        }

        public ActionResult Skirts()
        {
            var cat = db.Products.Where(x => x.Category.Type == "Skirts");
            return View(cat);
        }

        public ActionResult JumpSuit()
        {
            var cat = db.Products.Where(x => x.Category.Type == "JumpSuit");
            return View(cat);
        }

        public ActionResult Shoes()
        {
            var cat = db.Products.Where(x => x.Category.Type == "Shoes");
            return View(cat);
        }

        public ActionResult Dress()
        {
            var cat = db.Products.Where(x => x.Category.Type == "Dress");
            return View(cat);
        }
	}
}