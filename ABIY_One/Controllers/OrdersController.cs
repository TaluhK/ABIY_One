using ABIY_One.ABIY_Business_Logic;
using ABIY_One.Models;
using ABIY_One.Models.Data_Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABIY_One.Controllers
{
    public class OrdersController : Controller
    {
        // GET: Orders
        private ApplicationDbContext db = new ApplicationDbContext();

        Order_Business ob = new Order_Business();

        //Customer orders
        public ActionResult Customer_Orders(string id)
        {
            if (String.IsNullOrEmpty(id) || id == "all")
            {
                ViewBag.Status = "All";
                return View(ob.cust_all());
            }
            else
            {
                ViewBag.Status = id;
                return View(ob.cust_find_by_status(id));
            }
        }
        public ActionResult New_Orders(string id)
        {
            if (String.IsNullOrEmpty(id) || id == "all")
            {
                ViewBag.Status = "All";
                return View(ob.cust_all());
            }
            else
            {
                ViewBag.Status = id;
                return View(ob.cust_find_by_status(id));
            }
        }

        public ActionResult Order_Details(int? id)
        {
            if (id == null)
                return RedirectToAction("Bad_Request", "Error");
            if (ob.cust_find_by_id(id) != null)
            {
                ViewBag.Order_Items = ob.cust_Order_items(id);
                ViewBag.Address = db.Order_Addresses.ToList().Find(x => x.Order_ID == id);
                ViewBag.Total = get_order_total((int)id);

                return View(ob.cust_find_by_id(id));
            }
            else
                return RedirectToAction("Not_Found", "Error");
        }
        //public ActionResult Order_Tracking(int? id)
        //{
        //    if (id == null)
        //        return RedirectToAction("Bad_Request", "Error");
        //    if (ob.cust_find_by_id(id) != null)
        //    {
        //        ViewBag.Order = ob.cust_find_by_id(id);
        //        return View(ob.get_tracking_report(id));
        //    }
        //    else
        //        return RedirectToAction("Not_Found", "Error");
        //}

        public ActionResult Mark_As_Packed(int? id)
        {
            if (id == null)
                return RedirectToAction("Bad_Request", "Error");
            if (ob.cust_find_by_id(id) != null)
            {
                ob.mark_as_packed(id);
                return RedirectToAction("Order_Details", new { id = id });
            }
            else
                return RedirectToAction("Not_Found", "Error");
        }
        public double get_order_total(int id)
        {
            double amount = 0;
            foreach (var item in db.Order_Items.ToList().FindAll(match: x => x.Order_id == id))
            {
                amount += (item.price * item.quantity);
            }
            return amount;
        }

        //account orders
        public ActionResult Order_History()
        {
            return View(ob.cust_all().Where(x => x.Customer.Email == User.Identity.Name));
        }


















        public ActionResult MyOrders(int? page)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());

            // string UserId = user.Id;

            List<Order> orders = db.Orders.Where(x => x.Email == user.Email).ToList();

            foreach (var item in orders)
            {
                List<Order_Item> orderItems = db.Order_Items.Where(x => x.Order_id == item.Order_ID).ToList();
                double total = 0;
                item.Order_Items = orderItems;
                foreach (var ting in orderItems)
                {
                    ting.Item = db.Products.Find(ting.item_id);
                    ting.Item.Category = db.Categories.Find(ting.Item.Category_ID);

                    total += ting.price;
                    ViewData["i" + ting.Order_Item_id] = ting.Item.Name + " " + ting.Item.Description + " (" + ting.Item.Category.Category_Name + ")";
                }
                ViewData["p" + item.Order_ID] = total;
            }





            //var pageNumber = page ?? 1;

            //var onePageOfOrders = orders.ToPagedList(pageNumber, 10);
            //ViewBag.OnePageOfOrders = onePageOfOrders;

            return View(orders);

        }

    }
}