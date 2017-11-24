using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Moments.Models;

namespace Moments.Views
{
    public class MomentsPageController : Controller
    {
        private MomentsEntities1 db = new MomentsEntities1();

        // GET: MomentsPage
        public ActionResult Index()
        {
            var moments = db.Moments.Include(m => m.Login).Include(m => m.Place1);
            return View(moments.ToList());
        }

        // GET: MomentsPage/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Moments.Models.Moments moments = db.Moments.Find(id);
            if (moments == null)
            {
                return HttpNotFound();
            }
            return View(moments);
        }

        // GET: MomentsPage/Create
        public ActionResult Create()
        {
            ViewBag.Username = new SelectList(db.Login, "Id", "Username");
            ViewBag.Place = new SelectList(db.Place, "Id", "Country");
            return View();
        }

        // POST: MomentsPage/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Username,Titel,Description,Image,Date,Place")] Moments.Models.Moments moments)
        {
            if (ModelState.IsValid)
            {
                db.Moments.Add(moments);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Username = new SelectList(db.Login, "Id", "Username", moments.Username);
            ViewBag.Place = new SelectList(db.Place, "Id", "Country", moments.Place);
            return View(moments);
        }

        // GET: MomentsPage/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Moments.Models.Moments moments = db.Moments.Find(id);
            if (moments == null)
            {
                return HttpNotFound();
            }
            ViewBag.Username = new SelectList(db.Login, "Id", "Username", moments.Username);
            ViewBag.Place = new SelectList(db.Place, "Id", "Country", moments.Place);
            return View(moments);
        }

        // POST: MomentsPage/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Username,Titel,Description,Image,Date,Place")] Moments.Models.Moments moments)
        {
            if (ModelState.IsValid)
            {
                db.Entry(moments).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Username = new SelectList(db.Login, "Id", "Username", moments.Username);
            ViewBag.Place = new SelectList(db.Place, "Id", "Country", moments.Place);
            return View(moments);
        }

        // GET: MomentsPage/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Moments.Models.Moments moments = db.Moments.Find(id);
            if (moments == null)
            {
                return HttpNotFound();
            }
            return View(moments);
        }

        // POST: MomentsPage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Moments.Models.Moments moments = db.Moments.Find(id);
            db.Moments.Remove(moments);
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
