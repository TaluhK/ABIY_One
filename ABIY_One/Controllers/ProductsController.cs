using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ABIY_One.ABIY_Business_Logic;
using ABIY_One.Models;
using ABIY_One.Models.Data_Models;

namespace ABIY_One.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private Product_Business pb = new Product_Business();
        Category_Business cb = new Category_Business();
        public string shoppingCartID { get; set; }
        public const string CartSessionKey = "CartId";

        public ProductsController() { }
        public ActionResult Index()
        {
            return View(pb.all());
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
                return RedirectToAction("Bad_Request", "Error");
            if (pb.find_by_id(id) != null)
                return View(pb.find_by_id(id));
            else
                return RedirectToAction("Not_Found", "Error");
        }

        public ActionResult Create()
        {
            ViewBag.Category_ID = new SelectList(cb.all(), "Category_ID", "Category_Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product model, HttpPostedFileBase img_upload)
        {
            ViewBag.Category_ID = new SelectList(cb.all(), "Category_ID", "Category_Name");
            byte[] data = null;
            data = new byte[img_upload.ContentLength];
            img_upload.InputStream.Read(data, 0, img_upload.ContentLength);
            model.Picture = data;
            model.Price = model.CalcSellingPrice();
            model.ExpectedProfit = model.CalcExpctdProft();
            if (ModelState.IsValid)
            {
                pb.add(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }
        public ActionResult Edit(int? id)
        {
            ViewBag.Category_ID = new SelectList(cb.all(), "Category_ID", "Category_Name");
            if (id == null)
                return RedirectToAction("Bad_Request", "Error");
            if (pb.find_by_id(id) != null)
                return View(pb.find_by_id(id));
            else
                return RedirectToAction("Not_Found", "Error");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product model, HttpPostedFileBase img_upload)
        {
            byte[] data = null;
            data = new byte[img_upload.ContentLength];
            img_upload.InputStream.Read(data, 0, img_upload.ContentLength);
            model.Picture = data;
            model.Price = model.CalcSellingPrice();
            if (ModelState.IsValid)
            {
                pb.edit(model);
                return RedirectToAction("Index");
            }
            ViewBag.Category_ID = new SelectList(cb.all(), "Category_ID", "Category_Name");
            return View(model);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return RedirectToAction("Bad_Request", "Error");
            if (pb.find_by_id(id) != null)
                return View(pb.find_by_id(id));
            else
                return RedirectToAction("Not_Found", "Error");
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            pb.delete(pb.find_by_id(id));
            return RedirectToAction("Index");
        }


        public ActionResult Fall_catalog()
        {
            return View(pb.all());
        }

        public string GetCartID()
        {
            if (System.Web.HttpContext.Current.Session[CartSessionKey] == null)
            {
                if (!String.IsNullOrWhiteSpace(System.Web.HttpContext.Current.User.Identity.Name))
                {
                    System.Web.HttpContext.Current.Session[CartSessionKey] = System.Web.HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    Guid temp_cart_ID = Guid.NewGuid();
                    System.Web.HttpContext.Current.Session[CartSessionKey] = temp_cart_ID.ToString();
                }
            }
            return System.Web.HttpContext.Current.Session[CartSessionKey].ToString();
        }
    }
}
