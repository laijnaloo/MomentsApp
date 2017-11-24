using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Moments.Models;
using System.IO;

namespace Moments.Controllers
{
    public class MomentsController : Controller
    {
        private MomentsEntities1 db = new MomentsEntities1();

        // GET: Moments
        public ActionResult Index()
        {
            ViewBag.MetadataControll = db.Metadata.ToList();
            if (System.Web.HttpContext.Current.Session["UserName"] == null){
                return Redirect("~/Logins/Index");
            } else {
                var moments = db.Moments.Include(m => m.Login).Include(m => m.Place1);
                return View(moments.ToList());
            }
            
        }




        // GET: Moments/Details/5
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

        // GET: Moments/Create
        public ActionResult Create()
        {
            ViewBag.MetadataControll = db.Metadata.ToList();
            if (System.Web.HttpContext.Current.Session["UserName"] == null)
            {
                return Redirect("~/Logins/Index");
            }
            else
            {
                ViewBag.Username = new SelectList(db.Login, "Id", "Username");
                ViewBag.Place = new SelectList(db.Place, "Id", "Country");
                return View();
            }
        }

        // POST: Moments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase file, [Bind(Include = "Id,Username,Titel,Description,Image,Date,Place")] Moments.Models.Moments moments)
        {
            ViewBag.MetadataControll = db.Metadata.ToList();
            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                moments.Image = "/Images/" + fileName;
                file.SaveAs(path);
            } else
            {
                return Redirect("/Moments/Create"); //If there is no file you will be redirected to the same page
            }

            if (ModelState.IsValid)
            {
                db.Moments.Add(moments);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Username = new SelectList(db.Login, "Id", "Username", moments.Username);
            ViewBag.Place = new SelectList(db.Place, "Id", "Country", moments.Place);
            return Redirect("/Moments/");
        }

        // GET: Moments/Edit/5
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

        // POST: Moments/Edit/5
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

        // GET: Moments/Delete/5
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

        // POST: Moments/Delete/5
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
