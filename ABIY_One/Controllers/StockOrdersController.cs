using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ABIY_One.Models;

namespace ABIY_One.Controllers
{
    public class StockOrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: StockOrders
        public ActionResult Index()
        {
            var stockOrders = db.StockOrders.Include(s => s.Supplier);
            return View(stockOrders.ToList());
        }

        // GET: StockOrders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StockOrder stockOrder = db.StockOrders.Find(id);
            if (stockOrder == null)
            {
                return HttpNotFound();
            }
            return View(stockOrder);
        }

        // GET: StockOrders/Create
        public ActionResult Create()
        {
            ViewBag.SupplierId = new SelectList(db.Suppliers, "SupplierId", "FirstName");
            return View();
        }

        // POST: StockOrders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderID,OrderDate,DateCreated,Description,SupplierId")] StockOrder stockOrder)
        {
            if (ModelState.IsValid)
            {
                db.StockOrders.Add(stockOrder);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SupplierId = new SelectList(db.Suppliers, "SupplierId", "FirstName", stockOrder.SupplierId);
            return View(stockOrder);
        }

        // GET: StockOrders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StockOrder stockOrder = db.StockOrders.Find(id);
            if (stockOrder == null)
            {
                return HttpNotFound();
            }
            ViewBag.SupplierId = new SelectList(db.Suppliers, "SupplierId", "FirstName", stockOrder.SupplierId);
            return View(stockOrder);
        }

        // POST: StockOrders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderID,OrderDate,DateCreated,Description,SupplierId")] StockOrder stockOrder)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stockOrder).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SupplierId = new SelectList(db.Suppliers, "SupplierId", "FirstName", stockOrder.SupplierId);
            return View(stockOrder);
        }

        // GET: StockOrders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StockOrder stockOrder = db.StockOrders.Find(id);
            if (stockOrder == null)
            {
                return HttpNotFound();
            }
            return View(stockOrder);
        }

        // POST: StockOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StockOrder stockOrder = db.StockOrders.Find(id);
            db.StockOrders.Remove(stockOrder);
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
