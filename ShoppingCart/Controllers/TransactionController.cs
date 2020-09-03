using ShoppingCart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCart.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private ShoppingCartEntities db = new ShoppingCartEntities();

        //
        // GET: /Transaction/
        public ActionResult Index()
        {
            return View();
        }

        private Cart GetUsersCart()
        {

            string username = User.Identity.Name;
            try
            {

                db.Customers.First(c => c.UserName.Equals(username));
            }
            catch (Exception)
            {
                db.Customers.Add(new Customer { UserName = username });
                db.SaveChanges();
            }
            Cart cart = null;
            try
            {

                cart = db.Carts.First(c => c.CustomerID.Equals(username) &&
               c.Status.Equals("unpaid"));
            }
            catch (Exception)
            {

                cart = new Cart {Date= DateTime.Now , CustomerID = username, Status = "unpaid" };
                
                db.Carts.Add(cart);
                db.SaveChanges();
            }
            return cart;
        }


        public ActionResult Buy(int? id)
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

            Cart cart = GetUsersCart();

            try
            {

                CartItem cartitem = db.CartItems.First(c => (c.CartId == cart.Id &&
               c.ProductId == product.Id));
                cartitem.Quantity++;
            }
            catch (Exception)
            {

                CartItem cartitem = new CartItem
                {
                    ProductId = product.Id,
                    CartId = cart.Id,
                    Quantity = 1
                };
                db.CartItems.Add(cartitem);
            }
            db.SaveChanges();

            return RedirectToAction("Details", "CartUser", new { id = cart.Id });
        }


        public ActionResult MyCart()
        {
            // get user logged-in id + cart
            Cart cart = GetUsersCart();
            // go to that user's shopping cart page
            return RedirectToAction("Details", "CartUser", new { id = cart.Id });
        }

        public ActionResult Checkout(int? id)
        {
            //if id is null
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            if (cart.CartItems.Count == 0)
            {
                @ViewBag.message = "The Cart is Empty";
            }
            else
            {
                cart.Status = "paid";
                db.SaveChanges();
            }
            return RedirectToAction("Details", "CartUser", new { id = cart.Id });
        }
    }
}