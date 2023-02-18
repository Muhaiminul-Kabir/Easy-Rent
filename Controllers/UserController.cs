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
        private dbf db = new dbf();
     
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
  Session["visit"] = id;
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


            //find reviews
            var revs = (from i in db.userrevs
                        where i.userid == id
                        select i).ToList();


            foreach (var item in revs)
            {
                Models.View.Review v = new Models.View.Review();

                v.reviewerid = (int?)item.id;
                v.reviewtext = item.review;
                v.reviewerName = (from i in db.Users
                                  where i.id == item.reveiewerid
                                  select i.Name).FirstOrDefault();
                v.reviewerpic = (from i in db.Users
                                 where i.id == item.reveiewerid
                                 select i.pic).FirstOrDefault();

                user.revs.Add(v);

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
        public ActionResult Create( string name,string cell,string vid, string password, string gender, string email,string address, string propic,string start)
        {
            
            if (email != "" && password != "" && vid != "")
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
                    pic = uservm.pic,
                    VID = vid
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
            else if (vid == "")
            {
                ViewBag.error = "zyxi";
            }
            else
            {
                ViewBag.error = "zyx";
            }



            //IFERROR
            return View(); 
        }





        [HttpPost]
        public ActionResult Details(int? rating,string review)
        {

            var cc = (int?)Session["visit"];

            var up = (from i in db.Users
                      where i.id == cc
                      select i).First();
            int? pr = up.Rating;

            up.Rating = (int?)(pr + rating) / 2;
            db.SaveChanges();

            var newRev = new userrev
            {
                
                reveiewerid = (int?)Session["user"],
                review = review,
                userid = (int?)Session["visit"]
                
            };

            db.userrevs.Add(newRev);
            db.SaveChanges();


            return RedirectToAction("Details", "User", new { id = (int?)Session["visit"] });
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
                       Session["email"] = email;
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
