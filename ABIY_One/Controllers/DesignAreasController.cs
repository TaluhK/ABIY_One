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
    public class DesignAreasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DesignAreas
        public ActionResult Index()
        {
            return View(db.DesignAreas.ToList());
        }

        // GET: DesignAreas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DesignArea designArea = db.DesignAreas.Find(id);
            if (designArea == null)
            {
                return HttpNotFound();
            }
            return View(designArea);
        }

        // GET: DesignAreas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DesignAreas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DesignAreaId,AreaName")] DesignArea designArea)
        {
            if (ModelState.IsValid)
            {
                db.DesignAreas.Add(designArea);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(designArea);
        }

        // GET: DesignAreas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DesignArea designArea = db.DesignAreas.Find(id);
            if (designArea == null)
            {
                return HttpNotFound();
            }
            return View(designArea);
        }

        // POST: DesignAreas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DesignAreaId,AreaName")] DesignArea designArea)
        {
            if (ModelState.IsValid)
            {
                db.Entry(designArea).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(designArea);
        }

        // GET: DesignAreas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DesignArea designArea = db.DesignAreas.Find(id);
            if (designArea == null)
            {
                return HttpNotFound();
            }
            return View(designArea);
        }

        // POST: DesignAreas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DesignArea designArea = db.DesignAreas.Find(id);
            db.DesignAreas.Remove(designArea);
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
