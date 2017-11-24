using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Moments.Models;
using System.Web.Security;

using System.Security.Cryptography;
using System.Net;
using System.Net.Mail;
using System.Drawing;
using System.Configuration;
using System.Data.SqlClient;

namespace Moments.Controllers
{
    public class LoginsController : Controller
    {
        private MomentsEntities1 db = new MomentsEntities1();

        // GET: Logins
        public ActionResult Index()
        {
            ViewBag.MetadataControll = db.Metadata.ToList();
            var loginList = db.Login.ToList();

            return View(loginList);
        }


        // GET: Logins/Details/5


            // GET: Logins/Create
       
        public ActionResult Create()
        {
            return View();
        }

        // POST: Logins/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Username,Password,Email")] Login login)
        {
            if (ModelState.IsValid)
            {
                db.Login.Add(login);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(login);
        }

        // GET: Logins/Edit/5
        public ActionResult PasswordSent()
        {
            ViewBag.MetadataControll = db.Metadata.ToList();
            int id = 1;

            Login login = db.Login.Find(id);
            if (login == null)
            {
                return HttpNotFound();
            }
            return View(login);
        }

        // POST: Logins/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PasswordSent([Bind(Include = "Id,Username,Password,Email")] Login login)
        {
            ViewBag.MetadataControll = db.Metadata.ToList();
            if (ModelState.IsValid)
            {
                db.Entry(login).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(login);
        }

        // GET: Logins/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Login login = db.Login.Find(id);
            if (login == null)
            {
                return HttpNotFound();
            }
            return View(login);
        }

        // POST: Logins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Login login = db.Login.Find(id);
            db.Login.Remove(login);
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

        [HttpPost]
        public ActionResult PasswordCheck()
        {
            ViewBag.MetadataControll = db.Metadata.ToList();
            if (Request.Params["user"] == null || Request.Params["user"] == "" || Request.Params["pass"] == "" || Request.Params["pass"] == null)
            {
                return Redirect("/?missInfo=true");
            } else if (Encryption(Request.Params["pass"], Request.Params["user"]) == true)
            {
                System.Web.HttpContext.Current.Session["UserName"] = Request.Params["user"];

                ViewBag.UserNameVisible = System.Web.HttpContext.Current.Session["UserName"];
                return Redirect("/Moments");
            }
            else 
            {
                return Redirect("/?falseLogin=true");
            }
        }

        public bool Encryption(string password, string username)
        {
            Login login = db.Login.SqlQuery("SELECT * FROM Login WHERE username='" + username + "'").FirstOrDefault();

            if (login == null) { 
               
                return false;
            };

            password = FormsAuthentication.HashPasswordForStoringInConfigFile(password.Trim(), "md5"); //kryptering till md5

            

            return login.Password.ToUpper() == password;
        }

        public ActionResult NewPassword()
        {
            ViewBag.MetadataControll = db.Metadata.ToList();
            int id = 1;

                Login login = db.Login.Find(id);
                if (login == null)
                {
                    return HttpNotFound();
                }
                return View(login);
            
        }

        public ActionResult ChangePassword()
        {
            ViewBag.MetadataControll = db.Metadata.ToList();
            Login emailAdress = db.Login.SqlQuery("SELECT * FROM Login WHERE Email='" + Request.Params["email"] + "'").FirstOrDefault();

            if (Request.Params["email"] == null || Request.Params["email"] == "") //If the user doesnt write anything and just push the button
            {
                return Redirect("NewPassword/?falseEmail=true");
            }
            else if (checkEmail(emailAdress) == true) //if the user writes an email adress that is in the database
            {
                var loginList = db.Login.ToList();
                foreach (Login log in loginList)
                {
                    if(Request.Params["email"] == log.Email) {
                        sendEmail(log.Email);
                    }
                }

                return Redirect("PasswordSent");
            }
            else
                {
                return Redirect("NewPassword/?missingEmail=true"); //if the user writes in an adress that isnt in the database
            }
        }


        //Checks if the user has written a valid email adress and if so, returns true
        public bool checkEmail(Login email)
        {
            if (email == null)
            {
                return false;
            };

            return true;
        }

        private void sendEmail(string sendTo)
        {
            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("yourmomentsapp@gmail.com", "momentsapp");
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            MailMessage mail = new MailMessage();

            //Setting From , To and CC
            mail.From = new MailAddress("yourmomentsapp@gmail.com", "Moments Application");
            mail.To.Add(new MailAddress(sendTo));
            mail.Subject = "Reset Password";
            string newPass = createNewPassword();
            mail.Body = "Hey, we heard that you lost your password, but no worries! We have a new one for you right here: " + newPass;

            //kryptering av det nya lösenordet till md5-lösenord för att göra applikationen lite säkrare
            var loginList = db.Login.ToList();
            foreach (Login log in loginList)
            {
                string password = FormsAuthentication.HashPasswordForStoringInConfigFile(newPass.Trim(), "md5"); //kryptering

                //change password in database
                var user = new Login() { Id = log.Id, Password = password };
                using (var db = new MomentsEntities1())
                {
                    db.Login.Attach(user);
                    db.Entry(user).Property(x => x.Password).IsModified = true;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                }
            }
                client.Send(mail);
        }


        private static Random random = new Random();
        public static string createNewPassword()
        {
            //generates a new randomstring that the user can use as a password
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var sChars = new char[8];
            var random = new Random();

            for (int i = 0; i < sChars.Length; i++)
            {
                sChars[i] = chars[random.Next(chars.Length)];
            }

            var password = new String(sChars);

            return password;
        }

    }


}
