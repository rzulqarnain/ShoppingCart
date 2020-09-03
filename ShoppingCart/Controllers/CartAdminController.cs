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
     [Authorize]
    public class CartAdminController : Controller
    {
        private ShoppingCartEntities db = new ShoppingCartEntities();

        // GET: /CartAdmin/
        public ActionResult Index()
        {
            var carts = db.Carts.Include(c => c.Customer);
            return View(carts.ToList());
        }

        // GET: /CartAdmin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // GET: /CartAdmin/Create
        public ActionResult Create()
        {
            ViewBag.CustomerID = new SelectList(db.Customers, "UserName", "Name");
            return View();
        }

        // POST: /CartAdmin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,CustomerID,Date,Status")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                db.Carts.Add(cart);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerID = new SelectList(db.Customers, "UserName", "Name", cart.CustomerID);
            return View(cart);
        }

        // GET: /CartAdmin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "UserName", "Name", cart.CustomerID);
            return View(cart);
        }

        // POST: /CartAdmin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,CustomerID,Date,Status")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "UserName", "Name", cart.CustomerID);
            return View(cart);
        }

        // GET: /CartAdmin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // POST: /CartAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            
            Cart cart = db.Carts.Find(id);
            
                if (cart.Status == "unpaid" && cart.CartItems == null)
                {
                  
                    db.Carts.Remove(cart);
                    db.SaveChanges();
                    @TempData["Error message"] = "Cannot Delete the Cart";
                }

                else
                {
                    @TempData["Error message"] = "Cannot Delete the Cart - Item purchased in the Cart";
                }
           
           
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
