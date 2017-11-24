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
    public class MomentPageController : Controller
    {
        private MomentsEntities1 db = new MomentsEntities1();
        private MomentsEntities1 dbPlace = new MomentsEntities1();
        // GET: MomentPage
        // GET: Moments

        public ActionResult Index()
        {
            ViewBag.MetadataControll = db.Metadata.ToList();
            if (System.Web.HttpContext.Current.Session["UserName"] == null)
            {
                return Redirect("~/Logins/Index");
            }
            else
            {
                //Save the item the user chose to view more about to be able to show it in a separate page
                int key = int.Parse(Request.Params["item.Id"]);
                Moments.Models.Moments chosenMoment = db.Moments.Find(key);

                Moments.Models.Place country = dbPlace.Place.Find(chosenMoment.Place);
                ViewBag.Country = country.Country;
                return View(chosenMoment);
            }
        }

        public ActionResult StreamLink()
        {
            return Redirect("~/Moments/Index");
        }

        //Download image from page here
        public FileResult Download(string ImageName)
        {
            string newImageName = ImageName.Replace("/Images/", "");
            return File("~/" + ImageName, System.Net.Mime.MediaTypeNames.Application.Octet, newImageName);
        }

            // GET: Moments/Details/5
            public ActionResult Details(int? id)
        {
            ViewBag.MetadataControll = db.Metadata.ToList();
            if (System.Web.HttpContext.Current.Session["UserName"] == null)
            {
                return Redirect("~/Logins/Index");
            }
            else
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
        }

        // GET: Moments/Create
        public ActionResult Create()
        {
            ViewBag.Username = new SelectList(db.Login, "Id", "Username");
            ViewBag.Place = new SelectList(db.Place, "Id", "Country");
            return View();
        }

        // POST: Moments/Create
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

        // GET: Moments/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.MetadataControll = db.Metadata.ToList();
            if (System.Web.HttpContext.Current.Session["UserName"] == null)
            {
                return Redirect("~/Logins/Index");
            }
            else
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
        }

        // POST: Moments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Username,Titel,Description,Image,Date,Place")] Moments.Models.Moments moments)
        {
            int UserID = moments.Id;
            if (ModelState.IsValid)
            {
                db.Entry(moments).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new System.Web.Routing.RouteValueDictionary { { "item.Id", UserID } });
            }
            ViewBag.Username = new SelectList(db.Login, "Id", "Username", moments.Username);
            ViewBag.Place = new SelectList(db.Place, "Id", "Country", moments.Place);
            return View(moments);
        }

        // GET: Moments/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewBag.MetadataControll = db.Metadata.ToList();
            if (System.Web.HttpContext.Current.Session["UserName"] == null)
            {
                return Redirect("~/Logins/Index");
            }
            else
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
        }

        // POST: Moments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Moments.Models.Moments moments = db.Moments.Find(id);
            db.Moments.Remove(moments);
            db.SaveChanges();
            return Redirect("~/Moments/Index"); ;
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