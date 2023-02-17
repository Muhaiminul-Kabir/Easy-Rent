using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using projectsd.Models;
using System.Web.Script.Serialization;

namespace projectsd.Controllers
{
    public class UserController : Controller
    {
        private efdbEntitiesNirjon db = new efdbEntitiesNirjon();
     
         Models.View.User uservm = new Models.View.User();
        // GET: /User/
        public ActionResult Index()
        {
            return View();
        }

        // GET: /User/Details/5
        public ActionResult Details(int? id)
        {

            Models.View.User user = new Models.View.User();
            if (id == null)
            {
                return Content("Invalid request");
            }
            //User user = db.Users.Find(id);
            if (user == null)
            {
                return Content("Data not avaliable");
            }

            var u = (from x in db.Users
                     where x.id == id
                     select x 
                     
                     ).FirstOrDefault();

            if (u.OwnerId != null)
            {
                user.isOwner = true;
            }
            if (u.Tenantid != null)
            {
                user.isTenant = true;
            }
            user.id = id;
            user.name = u.Name;
            user.email = u.Email;
            user.cell = u.Cell;
            user.address = u.Address;
            user.rating = u.Rating;
            user.gender = u.Gender;
            user.pic = u.pic;
            return View(user);
        }

        // GET: /User/Create
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create( string name,string cell, string password, string gender, string email,string address, string propic,string start)
        {
            
            if (email != "" && password != "")
            {

                uservm.email = email;
                uservm.cell = cell;
                uservm.gender = gender;
                uservm.pass = password;
                uservm.name = name;
                uservm.address = address;
                uservm.pic = propic;


                var tw = new Auth
                {
                    email = uservm.email,
                    pass = uservm.pass
                };

                db.Auths.Add(tw);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewBag.error = "xyz";
                }


                var t = new User //Make sure you have a table called test in DB
                {
                    Name = uservm.name,
                    Rating = 0,
                    Address = uservm.address,
                    Cell = uservm.cell,
                    Gender = uservm.gender,
                    Email = uservm.email,
                    pic = uservm.pic

                };

                db.Users.Add(t);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewBag.error = "xyz";
                    return View();
                }



                return RedirectToAction("Login", "User");
            }
            else
            {
                ViewBag.error = "zyx";
            }

            //IFERROR
            return View(); 
        }




        public ActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Login(string email,string pass)
        {
            if (email == null)
            {
                return Content("Invalid request");
            }

            var result = (from item in db.Auths
                          where item.email == email
                         
                          select item).Count();
            if (result !=1)
            {
                ViewBag.error = "Invalid";
            }


            if (email != "" && pass != "")
            {
               
                   var x = (from p in db.Users
                          join e in db.Auths
                          on p.Email equals e.email
                          where e.email == email
                          select new{
                              id = p.id,
                              pass = e.pass,
                              pic = p.pic
                          }).FirstOrDefault();

                if(x != null){
                    
                   if(x.pass == pass){
                       Session["log"] = "in";
                       Session["pic"] = x.pic;
                        
                       Session["user"] = x.id;
                      // return Content(ViewBag.userid.ToString());
                       if (Session["need"] != "1")
                       {
                           return RedirectToAction("Index", "Rents"); ;
                       }
                       else
                       {
                           return RedirectToAction("Details", "Rents", new { id = (int?)Session["rentid"]}); ;
                       }
                   }
                   else
                   {
                       ViewBag.error = "Password";
                   }
                }
            }
            else
            {
                ViewBag.error = "empty";
            }

            return View();
        }




        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login","User");
        }



       
    }
}
