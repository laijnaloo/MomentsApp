using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Moments.Models;

namespace Moments.Controllers
{
    public class MetadatasController : Controller
    {
        private MomentsEntities1 db = new MomentsEntities1();

        // GET: Metadatas
        public ActionResult Index()
        {
            return View(db.Metadata.ToList());
        }

        // GET: Metadatas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Metadata metadata = db.Metadata.Find(id);
            if (metadata == null)
            {
                return HttpNotFound();
            }
            return View(metadata);
        }

        // GET: Metadatas/Create
        public ActionResult Create()
        {
            ViewBag.DC_Creator = new SelectList(db.Login, "Id", "Username");
            return View();
        }

        // POST: Metadatas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DC_Title,DC_Description,DC_Creator,DC_Publisher,DC_Keywords,DC_Type")] Metadata metadata)
        {
            if (ModelState.IsValid)
            {
                db.Metadata.Add(metadata);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DC_Creator = new SelectList(db.Login, "Id", "Username", metadata.DC_Creator);
            return View(metadata);
        }

        // GET: Metadatas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Metadata metadata = db.Metadata.Find(id);
            if (metadata == null)
            {
                return HttpNotFound();
            }
            ViewBag.DC_Creator = new SelectList(db.Login, "Id", "Username", metadata.DC_Creator);
            return View(metadata);
        }

        // POST: Metadatas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DC_Title,DC_Description,DC_Creator,DC_Publisher,DC_Keywords,DC_Type")] Metadata metadata)
        {
            if (ModelState.IsValid)
            {
                db.Entry(metadata).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DC_Creator = new SelectList(db.Login, "Id", "Username", metadata.DC_Creator);
            return View(metadata);
        }

        // GET: Metadatas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Metadata metadata = db.Metadata.Find(id);
            if (metadata == null)
            {
                return HttpNotFound();
            }
            return View(metadata);
        }

        // POST: Metadatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Metadata metadata = db.Metadata.Find(id);
            db.Metadata.Remove(metadata);
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
