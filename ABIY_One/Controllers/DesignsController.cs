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
    public class DesignsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Designs
        public ActionResult Index()
        {
            var designs = db.Designs.Include(d => d.DesignType);
            return View(designs.ToList());
        }

        // GET: Designs/Details/5
        public ActionResult ViewDesignShop()
        {
            return View(db.Designs.ToList());
        }
        public ActionResult ViewDesigns()
        {
            return View(db.Designs.ToList());
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Design design = db.Designs.Find(id);
            if (design == null)
            {
                return HttpNotFound();
            }
            return View(design);
        }

        // GET: Designs/Create
        public ActionResult Create()
        {
            ViewBag.DesignTypeId = new SelectList(db.DesignTypes, "DesignTypeId", "DesignTypeName");
            return View();
        }

        // POST: Designs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DesignId,DesignTypeId,DesignName,DesignImage,DesignPrice")] Design design, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    int fileLength = upload.ContentLength;
                    Byte[] array = new Byte[fileLength];
                    upload.InputStream.Read(array, 0, fileLength);
                    design.DesignImage = array;
                    db.Designs.Add(design);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
              }

            ViewBag.DesignTypeId = new SelectList(db.DesignTypes, "DesignTypeId", "DesignTypeName", design.DesignTypeId);
            return View(design);
        }

        // GET: Designs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Design design = db.Designs.Find(id);
            if (design == null)
            {
                return HttpNotFound();
            }
            ViewBag.DesignTypeId = new SelectList(db.DesignTypes, "DesignTypeId", "DesignTypeName", design.DesignTypeId);
            return View(design);
        }

        // POST: Designs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DesignId,DesignTypeId,DesignName,DesignImage,DesignPrice")] Design design, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    int fileLength = upload.ContentLength;
                    Byte[] array = new Byte[fileLength];
                    upload.InputStream.Read(array, 0, fileLength);
                    design.DesignImage = array;
                    db.Entry(design).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                }
            ViewBag.DesignTypeId = new SelectList(db.DesignTypes, "DesignTypeId", "DesignTypeName", design.DesignTypeId);
            return View(design);
        }

        // GET: Designs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Design design = db.Designs.Find(id);
            if (design == null)
            {
                return HttpNotFound();
            }
            return View(design);
        }

        // POST: Designs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Design design = db.Designs.Find(id);
            db.Designs.Remove(design);
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
