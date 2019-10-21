using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebApplication4.DBModel;

namespace TicketSystem.Controllers
{
    [Authorize]
    public class IssueTypeController : Controller
    {
        private Entities db = new Entities();

        public ActionResult Index()
        {
            return View(db.IssueTypes.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IssueType issueType = db.IssueTypes.Find(id);
            if (issueType == null)
            {
                return HttpNotFound();
            }
            return View(issueType);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] IssueType issueType)
        {
            if (ModelState.IsValid)
            {
                db.IssueTypes.Add(issueType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(issueType);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IssueType issueType = db.IssueTypes.Find(id);
            if (issueType == null)
            {
                return HttpNotFound();
            }
            return View(issueType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] IssueType issueType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(issueType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(issueType);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IssueType issueType = db.IssueTypes.Find(id);
            if (issueType == null)
            {
                return HttpNotFound();
            }
            return View(issueType);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            IssueType issueType = db.IssueTypes.Find(id);
            db.IssueTypes.Remove(issueType);
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
