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
    public class CartUserController : Controller
    {
        private ShoppingCartEntities db = new ShoppingCartEntities();

        // GET: /CartUser/
        public ActionResult Index()
        {
            string username = User.Identity.Name;
            var carts = db.Carts.Where(c => c.CustomerID == username);
            return View(carts.ToList());

        }

        // GET: /CartUser/Details/5
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
            @ViewBag.amount = cart.CartItems.Sum(p => (int)p.Quantity * (int)p.Product.Price);
            return View(cart);
        }

        // GET: /CartUser/Create
         [Authorize(Users = "admin")]
        public ActionResult Create()
        {
            ViewBag.CustomerID = new SelectList(db.Customers, "UserName", "Name");
            return View();
        }

        // POST: /CartUser/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Users = "admin")]
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

        // GET: /CartUser/Edit/5
         [Authorize(Users = "admin")]
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

        // POST: /CartUser/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Users = "admin")]
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

        // GET: /CartUser/Delete/5
         [Authorize]
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

        // POST: /CartUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]

         public ActionResult DeleteConfirmed(int id)
        {
          Cart cart = db.Carts.Find(id);
          if (cart.Status != "unpaid") { 
                TempData["error"] = "Cannot delete ";
                return RedirectToAction("Index");
        }
               
                db.Carts.Remove(cart);
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
