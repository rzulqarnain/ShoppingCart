using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShoppingCart.Models;

namespace ShoppingCart.Controllers
{
    public class ProductController : Controller
    {
        private ShoppingCartEntities db = new ShoppingCartEntities();

        // GET: /Product/
        public ActionResult Index(string catergorySearch, string searchString)
        {
            var categoryProducts = from c in db.Categories
                                   orderby c.Type
                                   select c.Type;

            var catergoryList = new List<string>();
            catergoryList.AddRange(categoryProducts.Distinct());
            ViewBag.catergorySearch = new SelectList(catergoryList);

            var products = from p in db.Products
                           select p;

            if (!String.IsNullOrEmpty(catergorySearch))
            {
                products = products.Where(c => c.Category.Type == catergorySearch);
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(c => c.Name.Contains(searchString));
            }
            return View(products);
            //var products = db.Products.Include(p => p.Category);
            //return View(products.ToList());
        }

        // GET: /Product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: /Product/Create
         [Authorize(Users = "admin")]
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "Id", "Type");
            return View();
        }

        // POST: /Product/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Users = "admin")]
        public ActionResult Create([Bind(Include="Id,Name,Price,image,CategoryID")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "Id", "Type", product.CategoryID);
            return View(product);
        }

        // GET: /Product/Edit/5
         [Authorize(Users = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
           
            if (product.CartItems.Count != 0)
            {
                TempData["Error"] = "Product Cannot be edited";
                return RedirectToAction("Index");
            }
          
            ViewBag.CategoryID = new SelectList(db.Categories, "Id", "Type", product.CategoryID);
            return View(product);
        }

        // POST: /Product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Users = "admin")]
        public ActionResult Edit([Bind(Include="Id,Name,Price,image,CategoryID")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "Id", "Type", product.CategoryID);
            return View(product);
        }

        // GET: /Product/Delete/5
         [Authorize(Users = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: /Product/Delete/5
         [Authorize(Users = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            if (product.CartItems.Count != 0)
            {
                TempData["Error message"] = "Item cannot be deleted : already used in the records";
                return View(product);
            }
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
