using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebApplication4.DBModel;

namespace TicketSystem.Controllers
{
    [Authorize]
    public class FlowController : Controller
    {
        private Entities db = new Entities();

        public ActionResult Index()
        {
            var flows = db.Flows.Include(f => f.IssueType).Include(f => f.Location).Include(f => f.User);
            return View(flows.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Flow flow = db.Flows.Find(id);
            if (flow == null)
            {
                return HttpNotFound();
            }
            return View(flow);
        }

        public ActionResult Create()
        {
            ViewBag.IssueType_Id = new SelectList(db.IssueTypes, "Id", "Name");
            ViewBag.Location_Id = new SelectList(db.Locations, "Id", "Name");
            ViewBag.User_Id = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Flow flow)
        {
            if (ModelState.IsValid)
            {
                db.Flows.Add(flow);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IssueType_Id = new SelectList(db.IssueTypes, "Id", "Name", flow.IssueType_Id);
            ViewBag.Location_Id = new SelectList(db.Locations, "Id", "Name", flow.Location_Id);
            ViewBag.User_Id = new SelectList(db.Users, "Id", "FirstName", flow.User_Id);
            return View(flow);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Flow flow = db.Flows.Find(id);
            if (flow == null)
            {
                return HttpNotFound();
            }
            ViewBag.IssueType_Id = new SelectList(db.IssueTypes, "Id", "Name", flow.IssueType_Id);
            ViewBag.Location_Id = new SelectList(db.Locations, "Id", "Name", flow.Location_Id);
            ViewBag.User_Id = new SelectList(db.Users, "Id", "FirstName", flow.User_Id);
            return View(flow);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IssueType_Id,Location_Id,User_Id")] Flow flow)
        {
            if (ModelState.IsValid)
            {
                db.Entry(flow).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IssueType_Id = new SelectList(db.IssueTypes, "Id", "Name", flow.IssueType_Id);
            ViewBag.Location_Id = new SelectList(db.Locations, "Id", "Name", flow.Location_Id);
            ViewBag.User_Id = new SelectList(db.Users, "Id", "FirstName", flow.User_Id);
            return View(flow);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Flow flow = db.Flows.Find(id);
            if (flow == null)
            {
                return HttpNotFound();
            }
            return View(flow);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Flow flow = db.Flows.Find(id);
            db.Flows.Remove(flow);
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
