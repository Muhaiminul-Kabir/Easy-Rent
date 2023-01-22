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
        private efdbEntities11 db = new efdbEntities11();
        List<Models.View.Rent> rents = new List<Models.View.Rent>();
        Models.View.Rooms room = new Models.View.Rooms();
        // GET: /Rents/
        public ActionResult Index()
        {

            
            var lrents = (from p in db.Rents
                          join e in db.RentInfoes
                          on p.InfoId equals e.id
                          select new
                          {
                              cover = e.pic, 
                              ID = p.id,
                              roomno = p.RoomId,
                              price = e.price,
                              location = "dkjf",
                              posted = e.posted,
                          }
                          ).ToList();




            foreach (var item in lrents)
            {

                Models.View.Rent objcvm = new Models.View.Rent(); // ViewModel

                objcvm.ID = item.ID;
                objcvm.pic = item.cover;

                objcvm.posted = item.posted;

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
            Rent rentdb = db.Rents.Find(id);
            if (rentdb == null)
            {
                return HttpNotFound();//TO BE DONE: will show a error view
            }


            var coverpic = (from d in db.Rents
                            join c in db.RentInfoes on d.RoomId equals c.id
                            where d.id == id
                            select new { cp = c.pic}).FirstOrDefault();
                             


            var roomDetail = (from d in db.Rents
                             join c in db.Rooms on d.RoomId equals c.id
                             join s in db.roominfoes on c.infoid equals s.id
                             where d.id == id
                             select new
                             {
                                 
                                 roomno = c.id, 
                                 noOfrooms= s.nofrooms ,
                                 maxmember = s.maxmembers,
                                 size = s.sqft,
                                 at = s.address
                                 
                             }).FirstOrDefault();

            int? roomno = roomDetail.roomno;

            

            var result = (from item in db.Rents  
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

                room.pic = coverpic.cp;
                room.maxmembers = roomDetail.maxmember;
                room.noofrooms = (int?)roomDetail.noOfrooms;
                room.needmembers = result ;
                room.Location = roomDetail.at;
                room.size = roomDetail.size;
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

      



        // GET: /Rents/Create
        public ActionResult Create()
        {
            ViewBag.InfoId = new SelectList(db.RentInfoes, "id", "id");
            ViewBag.RoomId = new SelectList(db.Rooms, "id", "id");
            ViewBag.TenantId = new SelectList(db.Tenants, "id", "id");
            return View();
        }

        // POST: /Rents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="id,InfoId,RoomId,TenantId")] Rent rent)
        {
            if (ModelState.IsValid)
            {
                db.Rents.Add(rent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.InfoId = new SelectList(db.RentInfoes, "id", "id", rent.InfoId);
            ViewBag.RoomId = new SelectList(db.Rooms, "id", "id", rent.RoomId);
            ViewBag.TenantId = new SelectList(db.Tenants, "id", "id", rent.TenantId);
            return View(rent);
        }

        // GET: /Rents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rent rent = db.Rents.Find(id);
            if (rent == null)
            {
                return HttpNotFound();
            }
            ViewBag.InfoId = new SelectList(db.RentInfoes, "id", "id", rent.InfoId);
            ViewBag.RoomId = new SelectList(db.Rooms, "id", "id", rent.RoomId);
            ViewBag.TenantId = new SelectList(db.Tenants, "id", "id", rent.TenantId);
            return View(rent);
        }

        // POST: /Rents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="id,InfoId,RoomId,TenantId")] Rent rent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.InfoId = new SelectList(db.RentInfoes, "id", "id", rent.InfoId);
            ViewBag.RoomId = new SelectList(db.Rooms, "id", "id", rent.RoomId);
            ViewBag.TenantId = new SelectList(db.Tenants, "id", "id", rent.TenantId);
            return View(rent);
        }

        // GET: /Rents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rent rent = db.Rents.Find(id);
            if (rent == null)
            {
                return HttpNotFound();
            }
            return View(rent);
        }

        // POST: /Rents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Rent rent = db.Rents.Find(id);
            db.Rents.Remove(rent);
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
