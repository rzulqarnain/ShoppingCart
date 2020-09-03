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
    public class CartItemController : Controller
    {
        private ShoppingCartEntities db = new ShoppingCartEntities();

        // GET: /CartItem/
        public ActionResult Index()
        {
            var cartitems = db.CartItems.Include(c => c.Cart).Include(c => c.Product);
            return View(cartitems.ToList());
        }

        // GET: /CartItem/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CartItem cartitem = db.CartItems.Find(id);
            if (cartitem == null)
            {
                return HttpNotFound();
            }
            return View(cartitem);
        }

        // GET: /CartItem/Create
        public ActionResult Create()
        {
            ViewBag.CartId = new SelectList(db.Carts, "Id", "CustomerID");
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name");
            return View();
        }

        // POST: /CartItem/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,ProductId,CartId,Quantity")] CartItem cartitem)
        {
            if (ModelState.IsValid)
            {
                db.CartItems.Add(cartitem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CartId = new SelectList(db.Carts, "Id", "CustomerID", cartitem.CartId);
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", cartitem.ProductId);
            return View(cartitem);
        }

        // GET: /CartItem/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CartItem cartitem = db.CartItems.Find(id);
            if (cartitem == null)
            {
                return HttpNotFound();
            }
            ViewBag.CartId = new SelectList(db.Carts, "Id", "CustomerID", cartitem.CartId);
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", cartitem.ProductId);
            return View(cartitem);
        }

        // POST: /CartItem/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,ProductId,CartId,Quantity")] CartItem cartitem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cartitem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CartId = new SelectList(db.Carts, "Id", "CustomerID", cartitem.CartId);
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", cartitem.ProductId);
            return View(cartitem);
        }

        // GET: /CartItem/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CartItem cartitem = db.CartItems.Find(id);
            if (cartitem == null)
            {
                return HttpNotFound();
            }
            return View(cartitem);
        }

        // POST: /CartItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CartItem cartitem = db.CartItems.Find(id);
            db.CartItems.Remove(cartitem);
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
