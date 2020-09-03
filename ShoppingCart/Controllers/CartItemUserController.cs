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
    public class CartItemUserController : Controller
    {
        private ShoppingCartEntities db = new ShoppingCartEntities();

        // GET: /CartItemUser/
        public ActionResult Index()
        {
            string username = User.Identity.Name;
            var cartitems = db.CartItems.Include(c => c.Cart).Include(c => c.Product).Where(e => e.Cart.Customer.UserName.Equals(username));
            //var cartitems = db.CartItems.Include(c => c.Cart).Include(c => c.Product);
            return View(cartitems.ToList());
        }

        // GET: /CartItemUser/Details/5
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

        

        // GET: /CartItemUser/Edit/5
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
            //ViewBag.CartId = new SelectList(db.Carts, "Id", "CustomerID", cartitem.CartId);
            //ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", cartitem.ProductId);
            return View(cartitem);
        }

        // POST: /CartItemUser/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Edit([Bind(Include="Id,ProductId,CartId,Quantity")] CartItem cartitem)
        {
            if (ModelState.IsValid)
            {
                if (cartitem.Quantity <= 0)
                {
                    @TempData["Error message"] = "Quantity is less than 0";
                }
                else if (cartitem.Quantity > 0)
                {
                    
                    db.Entry(cartitem).State = EntityState.Modified;

                }
                  db.SaveChanges();
                return RedirectToAction("Details", "CartUser", new { id = cartitem.CartId });
            }

            ViewBag.CartId = new SelectList(db.Carts, "Id", "CustomerID", cartitem.CartId);
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", cartitem.ProductId);

            return View(cartitem);
        }

        // GET: /CartItemUser/Delete/5
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

        // POST: /CartItemUser/Delete/5
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
