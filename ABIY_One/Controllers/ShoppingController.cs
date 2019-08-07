using ABIY_One.Models;
using ABIY_One.Models.Data_Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ABIY_One.Controllers
{
    public class ShoppingController : Controller
    {
        // GET: Shopping
        private ApplicationDbContext db = new ApplicationDbContext();
        public string shoppingCartID { get; set; }
        public const string CartSessionKey = "CartId";

        private bool Pymnt;
        public ActionResult Index()
        {
            return View(db.Products.ToList());
        }
        public ActionResult add_to_cart(int id)
        {
            var item = db.Products.Find(id);
            if (item != null)
            {
                add_item_to_cart(id);
                return RedirectToAction("Index");
            }
            else
                return RedirectToAction("Not_Found", "Error");
        }
        public ActionResult remove_from_cart(string id)
        {
            var item = db.Cart_Items.Find(id);
            if (item != null)
            {
                remove_item_from_cart(id: id);
                return RedirectToAction("ShoppingCart");
            }
            else
                return RedirectToAction("Not_Found", "Error");
        }
        public ActionResult ShoppingCart()
        {
            shoppingCartID = GetCartID();
            ViewBag.Total = get_cart_total(id: shoppingCartID);
            ViewBag.TotalQTY = get_Cart_Items().FindAll(x => x.cart_id == shoppingCartID).Sum(q => q.quantity);
            return View(db.Cart_Items.ToList().FindAll(x => x.cart_id == shoppingCartID));
        }
        [HttpPost]
        public ActionResult ShoppingCart(List<Cart_Items> items)
        {
            shoppingCartID = GetCartID();

            foreach (var i in items)
            {
                updateCart(i.cart_item_id, i.quantity);
            }
            ViewBag.Total = get_cart_total(shoppingCartID);
            ViewBag.TotalQTY = get_Cart_Items().FindAll(x => x.cart_id == shoppingCartID).Sum(q => q.quantity);
            return View(db.Cart_Items.ToList().FindAll(x => x.cart_id == shoppingCartID));
        }
        public ActionResult countCartItems()
        {
            int qty = get_Cart_Items().Count();
            return Content(qty.ToString());
        }
        public ActionResult Checkout()
        {
            if (get_Cart_Items().Count == 0)
            {
                ViewBag.Err = "Opps... you should have at least one cart item, please shop a few items";
                return RedirectToAction("Index");
            }
            else
                return RedirectToAction("DeliveryOption");
        }
        [Authorize]
        public ActionResult DeliveryOption()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DeliveryOption(string colorRadio, string Street, string City, string PostalCode)
        {
            if (!String.IsNullOrEmpty(colorRadio))
            {
                if (colorRadio.Equals("StandardDelivery"))
                {
                    Session["Street"] = Street;
                    Session["City"] = City;
                    Session["PostalCode"] = PostalCode;
                    return RedirectToAction("PlaceOrder", new { id = "deliver" });
                }
            }
            return View();
        }
        public ActionResult PlaceOrder(string id)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            ApplicationUser customer = UserManager.FindById(User.Identity.GetUserId());

            db.Orders.Add(new Order
            {


                Email = customer.Email,
                date_created = DateTime.Now,
                shipped = false,
                status = "Payment Complete"


            });
            db.SaveChanges();
            var order = db.Orders.ToList()
                .FindAll(x => x.Email == customer.Email)
                .LastOrDefault();

            if (id == "deliver")
            {
                db.Order_Addresses.Add(new Order_Address()
                {
                    Order_ID = order.Order_ID,
                    street = Session["Street"].ToString(),
                    city = Session["City"].ToString(),
                    zipcode = Session["PostalCode"].ToString()
                });
                db.SaveChanges();
            }

            var items = get_Cart_Items();

            foreach (var item in items)
            {
                var x = new Order_Item()
                {
                    Order_id = order.Order_ID,
                    item_id = item.item_id,
                    quantity = item.quantity,
                    price = item.price
                };
                db.Order_Items.Add(x);
                db.SaveChanges();
            }
            empty_Cart();
            //order tracking
            //db.Order_Trackings.Add(new Order_Tracking()
            //{
            //    order_ID = order.Order_ID,
            //    date = DateTime.Now,
            //    status = "Awaiting Payment",
            //    Recipient = ""
            //});
            db.SaveChanges();

            //Redirect to payment
            return RedirectToAction("Payment", new { id = order.Order_ID });
        }
        public ActionResult Payment(int? id)
        {
            var order = db.Orders.Find(id);
            ViewBag.Order = order;
            //here
            ViewBag.Account = db.Users.Find(order.Email);
            ViewBag.Address = db.Order_Addresses.ToList().Find(x => x.Order_ID == order.Order_ID);
            ViewBag.Items = db.Order_Items.ToList().FindAll(x => x.Order_id == order.Order_ID);
            ViewBag.Total = get_order_total(order.Order_ID);


            try
            {
                string url = "<a href=" + "https://www.payfast.co.za/" + id + " >  here" + "</a>";
                string table = "<br/>" +
                               "Items in this order<br/>" +
                               "<table>";
                table += "<tr>" +
                         "<th>Item</th>"
                         +
                         "<th>Quantity</th>"
                         +
                         "<th>Price</th>" +
                         "</tr>";
                foreach (var item in (List<Order_Item>)ViewBag.Items)
                {
                    string itemsinoder = "<tr> " +
                                         "<td>" + item.Item.Name + " </td>" +
                                         "<td>" + item.quantity + " </td>" +
                                         "<td>R " + item.price + " </td>" +
                                         "<tr/>";
                    table += itemsinoder;
                }

                table += "<tr>" +
                         "<th></th>"
                         +
                         "<th></th>"
                         +
                         "<th>" + get_order_total(order.Order_ID).ToString("R0.00") + "</th>" +
                         "</tr>";
                table += "</table>";

                var client = new SendGridClient("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");
                var from = new EmailAddress("no-reply@shopifyhere.com", "Shopify Here");
                var subject = "Order " + id + " | Awaiting Payment";
                var to = new EmailAddress(((Customer)ViewBag.Account).Email, ((Customer)ViewBag.Account).FirstName + " " + ((Customer)ViewBag.Account).LastName);
                var htmlContent = "Hi " + order.Customer.FirstName + ", Your order was placed, you can securely pay your order from " + url + table;
                var msg = MailHelper.CreateSingleEmail(from, to, subject, null, htmlContent);
                var response = client.SendEmailAsync(msg);

                Pymnt = true;
            }
            catch (Exception ex)
            {

            }
            return View();
        }
        public ActionResult Secure_Payment(int? id)
        {
            var order = db.Orders.Find(id);
            ViewBag.Order = order;
            ViewBag.Account = db.Customers.Find(order.Email);
            ViewBag.Address = db.Order_Addresses.ToList().Find(x => x.Order_ID == order.Order_ID);
            ViewBag.Items = db.Order_Items.ToList().FindAll(x => x.Order_id == order.Order_ID);

            //foreach (var item in db.Order_Items.Where(x => x.Order_id == order.Order_ID))
            //{
            //    Item product = db.Items.Find(item.item_id);
            //    product.QuantityInStock -= item.quantity;
            //}
            //db.SaveChanges();

            ViewBag.Total = get_order_total(order.Order_ID);
            Pymnt = true;

            return Redirect(PaymentLink(get_order_total(order.Order_ID).ToString(), "Order Payment | Order No: " + order.Order_ID, order.Order_ID));
        }
        public ActionResult Payment_Cancelled(int? id)
        {
            var order = db.Orders.Find(id);
            ViewBag.Order = order;
            ViewBag.Account = db.Customers.Find(order.Email);
            ViewBag.Address = db.Order_Addresses.ToList().Find(x => x.Order_ID == order.Order_ID);
            ViewBag.Items = db.Order_Items.ToList().FindAll(x => x.Order_id == order.Order_ID);

            ViewBag.Total = get_order_total(order.Order_ID);
            try
            {
                string url = "<a href=" + "https://www.payfast.co.za/" + id + " >  here" + "</a>";
                string table = "<br/>" +
                               "Items in this order<br/>" +
                               "<table>";
                table += "<tr>" +
                         "<th>Item</th>"
                         +
                         "<th>Quantity</th>"
                         +
                         "<th>Price</th>" +
                         "</tr>";
                foreach (var item in (List<Order_Item>)ViewBag.Items)
                {
                    string items = "<tr> " +
                                   "<td>" + item.Item.Name + " </td>" +
                                   "<td>" + item.quantity + " </td>" +
                                   "<td>R " + item.price + " </td>" +
                                   "<tr/>";
                    table += items;
                }

                table += "<tr>" +
                         "<th></th>"
                         +
                         "<th></th>"
                         +
                         "<th>" + get_order_total(order.Order_ID).ToString("R0.00") + "</th>" +
                         "</tr>";
                table += "</table>";

                var client = new SendGridClient("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");
                var from = new EmailAddress("no-reply@shopifyhere.com", "Shopify Here");
                var subject = "Order " + id + " | Awaiting Payment";
                var to = new EmailAddress(order.Customer.Email, order.Customer.FirstName + " " + order.Customer.LastName);
                var htmlContent = "Hi " + order.Customer.FirstName + ", Your order payment process was cancelled, you can still securely pay your order from " + url + table;
                var msg = MailHelper.CreateSingleEmail(from, to, subject, null, htmlContent);
                var response = client.SendEmailAsync(msg);
            }
            catch (Exception ex)
            {

            }
            return View();
        }
        public ActionResult Payment_Successfull(int? id)
        {
            var order = db.Orders.Find(id);
            try
            {
                order.status = "At warehouse";

                //order tracking
                //db.Order_Trackings.Add(new Order_Tracking()
                //{
                //    order_ID = order.Order_ID,
                //    date = DateTime.Now,
                //    status = "Payment Recieved | Order still at warehouse",
                //    Recipient = ""
                //});
                db.SaveChanges();
                db.Payments.Add(new Payment()
                {
                    Date = DateTime.Now,
                    Email = db.Customers.FirstOrDefault(p => p.Email == User.Identity.Name).Email,
                    AmountPaid = get_order_total(order.Order_ID),
                    PaymentFor = "Order " + id + " Payment",
                    PaymentMethod = "PayFast Online"
                });
                db.SaveChanges();
                if (db.Order_Addresses.Where(p => p.Order_ID == id) != null)
                {
                    var expected_Date = DateTime.Now.AddDays(2);
                    do
                    {
                        expected_Date = expected_Date.AddDays(1);
                    } while (expected_Date.DayOfWeek.ToString().ToLower() == "sunday" ||
                        expected_Date.DayOfWeek.ToString().ToLower() == "saturday");

                    //Delivery
                }
                db.SaveChanges();
                ViewBag.Items = db.Order_Items.ToList().FindAll(x => x.Order_id == order.Order_ID);

                update_Stock((int)id);

                string table = "<br/>" +
                               "Items in this order<br/>" +
                               "<table>";
                table += "<tr>" +
                         "<th>Item</th>"
                         +
                         "<th>Quantity</th>"
                         +
                         "<th>Price</th>" +
                         "</tr>";
                foreach (var item in (List<Order_Item>)ViewBag.Items)
                {
                    string items = "<tr> " +
                                   "<td>" + item.Item.Name + " </td>" +
                                   "<td>" + item.quantity + " </td>" +
                                   "<td>R " + item.price + " </td>" +
                                   "<tr/>";
                    table += items;
                }

                table += "<tr>" +
                         "<th></th>"
                         +
                         "<th></th>"
                         +
                         "<th>" + get_order_total(order.Order_ID).ToString("R 0.00") + "</th>" +
                         "</tr>";
                table += "</table>";

                var client = new SendGridClient("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");
                var from = new EmailAddress("no-reply@shopifyhere.com", "Shopify Here");
                var subject = "Order " + id + " | Payment Recieved";
                var to = new EmailAddress(order.Customer.Email, order.Customer.FirstName + " " + order.Customer.LastName);
                var htmlContent = "Hi " + order.Customer.FirstName + ", We recieved your payment, you will have your goodies any time from now " + table;
                var msg = MailHelper.CreateSingleEmail(from, to, subject, null, htmlContent);
                var response = client.SendEmailAsync(msg);
            }
            catch (Exception ex) { }

            ViewBag.Order = order;
            ViewBag.Account = db.Customers.Find(order.Email);
            ViewBag.Address = db.Order_Addresses.ToList().Find(x => x.Order_ID == order.Order_ID);
            ViewBag.Total = get_order_total(order.Order_ID);
            Pymnt = true;
            return View();
        }

        #region Cart Methods
        public void add_item_to_cart(int id)
        {
            shoppingCartID = GetCartID();

            var item = db.Products.Find(id);
            if (item != null)
            {
                var cartItem =
                    db.Cart_Items.FirstOrDefault(x => x.cart_id == shoppingCartID && x.item_id == item.ItemCode);
                if (cartItem == null)
                {
                    var cart = db.Carts.Find(shoppingCartID);
                    if (cart == null)
                    {
                        db.Carts.Add(entity: new Cart()
                        {
                            cart_id = shoppingCartID,
                            date_created = DateTime.Now
                        });
                        db.SaveChanges();
                    }

                    db.Cart_Items.Add(entity: new Cart_Items()
                    {
                        cart_item_id = Guid.NewGuid().ToString(),
                        cart_id = shoppingCartID,
                        item_id = item.ItemCode,
                        quantity = 1,
                        price = item.Price
                    }
                        );
                }
                else
                {
                    cartItem.quantity++;
                }
                db.SaveChanges();
            }
        }
        public void remove_item_from_cart(string id)
        {
            shoppingCartID = GetCartID();

            var item = db.Cart_Items.Find(id);
            if (item != null)
            {
                var cartItem =
                    db.Cart_Items.FirstOrDefault(predicate: x => x.cart_id == shoppingCartID && x.item_id == item.item_id);
                if (cartItem != null)
                {
                    db.Cart_Items.Remove(entity: cartItem);
                }
                db.SaveChanges();
            }
        }
        public List<Cart_Items> get_Cart_Items()
        {
            shoppingCartID = GetCartID();
            return db.Cart_Items.ToList().FindAll(match: x => x.cart_id == shoppingCartID);
        }
        public void updateCart(string id, int qty)
        {
            var item = db.Cart_Items.Find(id);
            if (qty < 0)
                item.quantity = qty / -1;
            else if (qty == 0)
                remove_item_from_cart(item.cart_item_id);
            else
                item.quantity = qty;
            db.SaveChanges();
        }
        public double get_cart_total(string id)
        {
            double amount = 0;
            foreach (var item in db.Cart_Items.ToList().FindAll(match: x => x.cart_id == id))
            {
                amount += (item.price * item.quantity);
            }
            return amount;
        }
        public void empty_Cart()
        {
            shoppingCartID = GetCartID();
            foreach (var item in db.Cart_Items.ToList().FindAll(match: x => x.cart_id == shoppingCartID))
            {
                db.Cart_Items.Remove(item);
            }
            try
            {
                db.Carts.Remove(db.Carts.Find(shoppingCartID));
                db.SaveChanges();
            }
            catch (Exception ex) { }
        }
        public string GetCartID()
        {
            if (System.Web.HttpContext.Current.Session[name: CartSessionKey] == null)
            {
                if (!String.IsNullOrWhiteSpace(value: System.Web.HttpContext.Current.User.Identity.Name))
                {
                    System.Web.HttpContext.Current.Session[name: CartSessionKey] = System.Web.HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    Guid temp = Guid.NewGuid();
                    System.Web.HttpContext.Current.Session[name: CartSessionKey] = temp.ToString();
                }
            }
            return System.Web.HttpContext.Current.Session[name: CartSessionKey].ToString();
        }
        #endregion

        #region Customer Order Methods
        public double get_order_total(int id)
        {
            double amount = 0;
            foreach (var item in db.Order_Items.ToList().FindAll(match: x => x.Order_id == id))
            {
                amount += (item.price * item.quantity);
            }
            return amount;
        }
        public string PaymentLink(string totalCost, string paymentSubjetc, int order_id)
        {

            string paymentMode = ConfigurationManager.AppSettings["PaymentMode"], site, merchantId, merchantKey, returnUrl;

            if (paymentMode == "test")
            {
                site = "https://sandbox.payfast.co.za/eng/process?";
                merchantId = "10002201";
                merchantKey = "25lbpwmazv8rn";
            }
            else if (paymentMode == "live")
            {
                site = "https://www.payfast.co.za/eng/process?";
                merchantId = ConfigurationManager.AppSettings["PF_MerchantID"];
                merchantKey = ConfigurationManager.AppSettings["PF_MerchantKey"];
            }
            else
            {
                throw new InvalidOperationException("Payment method unknown.");
            }
            var stringBuilder = new StringBuilder();
            //string url = Url.Action("Quotes", "Order",
            //    new System.Web.Routing.RouteValueDictionary(new { id = orderid }),
            //    "http", Request.Url.Host);

            stringBuilder.Append("&merchant_id=" + HttpUtility.HtmlEncode(merchantId));
            stringBuilder.Append("&merchant_key=" + HttpUtility.HtmlEncode(merchantKey));
            stringBuilder.Append("&return_url=" + HttpUtility.HtmlEncode("https://abiytshirt.azurewebsites.net/Shopping/Payment_Successfull?id=" + order_id));
            stringBuilder.Append("&cancel_url=" + HttpUtility.HtmlEncode("https://abiytshirt.azurewebsites.net/Shopping/Payment_Cancelled?id=" + order_id));
            stringBuilder.Append("&notify_url=" + HttpUtility.HtmlEncode(ConfigurationManager.AppSettings["PF_NotifyURL"]));

            string amt = totalCost;
            amt = amt.Replace(",", ".");

            stringBuilder.Append("&amount=" + HttpUtility.HtmlEncode(amt));
            stringBuilder.Append("&item_name=" + HttpUtility.HtmlEncode(paymentSubjetc));
            stringBuilder.Append("&email_confirmation=" + HttpUtility.HtmlEncode("1"));
            stringBuilder.Append("&confirmation_address=" + HttpUtility.HtmlEncode(ConfigurationManager.AppSettings["PF_ConfirmationAddress"]));

            return (site + stringBuilder);
        }
        public void update_Stock(int id)
        {
            var order = db.Orders.Find(id);
            List<Order_Item> items = db.Order_Items.ToList().FindAll(x => x.Order_id == id);
            foreach (var item in items)
            {
                var product = db.Products.Find(item.item_id);
                if (product != null)
                {
                    if ((product.QuantityInStock -= item.quantity) >= 0)
                    {
                        product.QuantityInStock -= item.quantity;
                    }
                    else
                    {
                        item.quantity = product.QuantityInStock;
                        product.QuantityInStock = 0;
                    }
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception ex) { }
                }
            }
        }
        #endregion
    }
}