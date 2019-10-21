using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication4.DBModel;

namespace TicketSystem.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private Entities db = new Entities();

        // GET: Report
        public ActionResult Index()
        {
            return View(db.Requests.ToList());
        }
        public ActionResult RequestDetails(int id)
        {
            var requests = db.Requests.Where(r => r.Id == id)
                           .Include(r => r.Flow).Include(r => r.User).Include(r => r.Status).Include(r => r.Messages).FirstOrDefault();
            return View(requests);
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
