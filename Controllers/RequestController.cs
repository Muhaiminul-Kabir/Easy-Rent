using projectsd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Net;
using System.Net.Mail;






namespace projectsd.Controllers
{
    public class RequestController : Controller
    {
        private efdbEntitiesNirjon db = new efdbEntitiesNirjon();
        List<Models.View.Requests> reqs = new List<Models.View.Requests>();





        public ActionResult Accept(int? reqid)
        {
            //if no id than show this
            if (reqid == null)
            {
                return Content("Invalid request");
            }

            // get data using request id
            var req = (from i in db.Requests
                       where i.id == reqid
                       select i).FirstOrDefault();


            //store data in variables
            int? rent = req.rentid;
            int? sender = req.senderid;

            //get tenant id from user
            var getTenant = (from i in db.Users
                             where i.id == sender
                             select i.Tenantid).FirstOrDefault();


            //update tenantid in  rentalseats 
            var upRent = (from i in db.Rentealseats
                          where i.id == rent
                          select i).First();

            upRent.TenantId = (int?)getTenant;

            db.SaveChanges();

            // Remove the request
            req.stat = "accepted";
            db.SaveChanges();




            // return to rent details page
            return RedirectToAction("Details", "Rents", new { id = Session["rent"] });
        }



        public ActionResult Reject(int? reqid)
        {
            return View();
        }

        public ActionResult Remove(int? reqid)
        {
            var i = (from c in db.Requests
                     where c.id == reqid
                     select c).First();
            db.Requests.Remove(i);
            db.SaveChanges();

            return RedirectToAction("Index");
        }



        
        //
        // GET: /Request/
        public ActionResult Index()
        {
            int? uid = (int?)Session["user"];

            
            var x = (
                    from r in db.Requests
                    join i in db.Rentealseats on r.rentid equals i.id
                    where r.senderid == uid
                    select new
                    {
                        reqid = r.id,
                        rentid = i.id,
                        sent = r.date,
                        stat = r.stat,
                        userid = r.senderid
                    }
                ).ToList();


            ViewBag.rqcnt = x.Count();
            foreach (var item in x)
            {

                
                Models.View.Requests objcvm = new Models.View.Requests(); // ViewModel


                objcvm.reqid = item.reqid;
                objcvm.ID = item.rentid;
                objcvm.date = item.sent;
                objcvm.status = item.stat;

                reqs.Add(objcvm);

            }



            return View(reqs);
        }





	}
}