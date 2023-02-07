using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using projectsd.Models;

namespace projectsd.Controllers
{
    public class RentsController : Controller
    {
        private efdbEntities2 db = new efdbEntities2();
        List<Models.View.Rent> rents = new List<Models.View.Rent>();
        Models.View.Rent room = new Models.View.Rent();
        // GET: /Rents/

        public ActionResult Index()
        {

            var fac = (from x in db.Facilities
                       select x).ToList();
            List<Models.View.Facilities> facobjl = new List<Models.View.Facilities>();

            foreach (var item in fac)
            {
                Models.View.Facilities facobj = new Models.View.Facilities();
                facobj.id = item.id;
                facobj.icon = item.icon;
                facobj.type = item.type;
                facobjl.Add(facobj);
            }

            ViewBag.fac = facobjl;



            var loc = (from c in db.Districts

                       select c.DistrictName

                      ).ToList();

            ViewBag.uiDist = loc;


            ViewBag.userid = Session["user"];
            var lrents = (from p in db.Rentealseats
                          join x in db.Rooms on p.RoomId equals x.id
                          select new
                          {
                              cover = p.pic,
                              ID = p.id,
                              roomno = p.RoomId,
                              price = p.price,
                              location = x.address,
                              start = p.startdate
                          }
                          ).ToList();




            foreach (var item in lrents)
            {

                Models.View.Rent objcvm = new Models.View.Rent(); // ViewModel

                objcvm.ID = item.ID;
                objcvm.pic = item.cover;

                objcvm.posted = new DateTime();
                objcvm.startDate = item.start;
                objcvm.Location = item.location;

                objcvm.price = item.price;

                rents.Add(objcvm);

            }


            return View(rents);
        }

        // GET: /Rents/Details/5


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rentealseat rentdb = db.Rentealseats.Find(id);
            if (rentdb == null)
            {
                return HttpNotFound();//TO BE DONE: will show a error view
            }


            var coverpic = (from d in db.Rentealseats
                              where d.id == id
                            select  d.pic).FirstOrDefault();



            var roomDetail = (from d in db.Rentealseats
                              join c in db.Rooms on d.RoomId equals c.id
                              where d.id == id
                              select new
                              {
                                  rentid = d.id,
                                  start = d.startdate,
                                  roomno = c.id,
                                  noOfrooms = c.noofrooms,
                                  maxmember = c.maxmembers,
                                  price = d.price,
                                  size = c.sqft,
                                  at = c.address,
                                  oner = (from s in db.Users
                                         where s.OwnerId == c.ownerid
                                         select s).FirstOrDefault()

                              }).FirstOrDefault();

            int? roomno = roomDetail.roomno;



            var result = (from item in db.Rentealseats
                          where item.id == id
                          where item.TenantId != null
                          select item).Count();


            var fac = (from d in db.Rooms
                       join c in db.FacilitiesForRooms on d.id equals c.RoomId
                       join f in db.Facilities on c.id equals f.id
                       where d.id == roomno
                       select f
                      ).ToList();



            //hierarchy

            if (roomDetail == null)
            {
                return Content("No data avaliable");
            }
            else
            {

                room.ID = roomDetail.rentid;
                room.startDate = roomDetail.start;
                room.maxmembers = roomDetail.maxmember;
                room.noofrooms = (int?)roomDetail.noOfrooms;
                room.needmembers = result;
                room.Location = roomDetail.at;
                room.size = roomDetail.size;
                room.pic = coverpic;
                room.price = roomDetail.price;
                room.oname = roomDetail.oner.Name;
                if ((int?)Session["user"] == roomDetail.oner.id)
                {
                    ViewBag.userType = "owner";
                }
            }

            foreach (var items in fac)
            {
                Models.View.Facilities x = new Models.View.Facilities();
                x.icon = items.icon;
                x.type = items.type;
                room.facilities.Add(x);
            }



            return View(room);
        }


        [HttpPost]
        public ActionResult Index(int? minp,int? maxp,DateTime? start,int? minroom,int? maxroom,
                                  int? minmem, int? maxmem, int? minsize, int? maxsize)
        {

            ViewBag.Search = 0;
            var fac = (from x in db.Facilities
                       select x).ToList();
            List<Models.View.Facilities> facobjl = new List<Models.View.Facilities>();

            foreach (var item in fac)
            {
                Models.View.Facilities facobj = new Models.View.Facilities();
                facobj.id = item.id;
                facobj.icon = item.icon;
                facobj.type = item.type;
                facobjl.Add(facobj);
            }

            ViewBag.fac = facobjl;



            var loc = (from c in db.Districts

                       select c.DistrictName

                      ).ToList();

            ViewBag.uiDist = loc;


            ViewBag.userid = Session["user"];
            

            if(minp != null && maxp != null && minmem != null && maxmem != null && maxroom != null && minroom != null && minsize != null && maxroom != null ){
               var lrents = (from p in db.Rentealseats
                          join x in db.Rooms on p.RoomId equals x.id
                          where (p.price >= minp && p.price <= maxp) && p.startdate >= start && (x.sqft <= maxsize && x.sqft >= minsize) && (x.maxmembers <= maxmem && x.maxmembers >= minmem) && (x.noofrooms >= minroom && x.noofrooms <= maxroom)
                         
                          select new
                          {
                              cover = p.pic,
                              ID = p.id,
                              roomno = p.RoomId,
                              price = p.price,
                              location = x.address,
                              start = p.startdate
                          }
                          ).ToList();
                ViewBag.Search = lrents.Count();
               foreach (var item in lrents)
               {

                   Models.View.Rent objcvm = new Models.View.Rent(); // ViewModel

                   objcvm.ID = item.ID;
                   objcvm.pic = item.cover;

                   objcvm.startDate = item.start;

                   objcvm.Location = item.location;

                   objcvm.price = item.price;

                   rents.Add(objcvm);

               }

               return View(rents);
               

            }else{
               ViewBag.searchError = "yes";
                
                var lrents = (from p in db.Rentealseats
                          join x in db.Rooms on p.RoomId equals x.id
                          select new
                          {
                              cover = p.pic,
                              ID = p.id,
                              roomno = p.RoomId,
                              price = p.price,
                              location = x.address,
                              start = p.startdate
                          }
                          ).ToList();
                foreach (var item in lrents)
                {

                    Models.View.Rent objcvm = new Models.View.Rent(); // ViewModel

                    objcvm.ID = item.ID;
                    objcvm.pic = item.cover;

                    objcvm.startDate = item.start;

                    objcvm.Location = item.location;

                    objcvm.price = item.price;

                    rents.Add(objcvm);

                }


                return View(rents);


            }

            
            
        }



        // GET: /Rents/Create
        public ActionResult Create()
        {
            return View();
        }
        
        public ActionResult Requests(int? rentid){

            
            Session["rentid"] = rentid;
            int? user = (int?)Session["user"];
            if ((int?)Session["user"] == null)
            {
                
                ViewBag.error = "no";
                Session["need"] = "1";
               
                return RedirectToAction("Login", "User");

            }

             var t = new Tenant
             {
                 
             };
             db.Tenants.Add(t);

             db.SaveChanges();


            var req = new Request
            {
                rentid = rentid,
                tenantid = t.id,
                date = DateTime.Today

            };
           
                
            db.Requests.Add(req);
            db.SaveChanges();

            var u = (from i in db.Users
                     where i.id == user
                     select i
                         ).First();
            u.Tenantid = t.id;
            db.SaveChanges();

            
           
            


            return RedirectToAction("Details","Rents", new { id = rentid});

        }


    }

    

}
