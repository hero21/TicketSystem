using System.Linq;
using System.Web.Mvc;
using WebApplication4.DBModel;

namespace TicketSystem.Controllers
{
    public class ConfigController : Controller
    {
        Entities db = new Entities();
        public ActionResult Update()
        {
            var SMPT = db.SMTP.FirstOrDefault();
            return View(SMPT);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(SMTP sMTP)
        {
            db.Entry(sMTP).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return View(sMTP);
        }
    }
}