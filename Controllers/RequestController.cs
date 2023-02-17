using projectsd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace projectsd.Controllers
{
    public class RequestController : Controller
    {
        private efdbEntitiesNirjon db = new efdbEntitiesNirjon();
        List<Models.View.Requests> reqs = new List<Models.View.Requests>();
        
        
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
                        rentid = i.id,
                        sent = r.date,
                        userid = r.senderid
                    }
                ).ToList();


            ViewBag.rqcnt = x.Count();
            foreach (var item in x)
            {

                Models.View.Requests objcvm = new Models.View.Requests(); // ViewModel

                objcvm.ID = item.rentid;
                objcvm.date = item.sent;
                

                reqs.Add(objcvm);

            }



            return View(reqs);
        }
	}
}