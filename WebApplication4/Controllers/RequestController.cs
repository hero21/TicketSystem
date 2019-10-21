using System;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicketSystem.Models;
using WebApplication4.DBModel;

namespace TicketSystem.Controllers
{
    [Authorize]
    public class RequestController : Controller
    {
        private string AttachmentSplit = "~";
        private string prefixPath = "";
        private Entities db = new Entities();

        public ActionResult MyRequest()
        {
            var requests = db.Requests.Where(r => r.User_Id == UserDetails.UserId)
                           .Include(r => r.Flow).Include(r => r.User).Include(r => r.Status)
                           .OrderByDescending(r=>r.Date).ToList();
            return View(requests);
        }

        public ActionResult MyAction()
        {
            var requests = (from r in db.Requests
                            join f in db.Flows on r.Flow_Id equals f.Id
                            where f.User_Id == UserDetails.UserId
                            select r).OrderByDescending(r => r.Date).ToList();
            return View(requests);
        }

        public ActionResult MyRequestDetails(int id)
        {
            var requests = db.Requests.Where(r => r.Id == id)
                           .Include(r => r.Flow).Include(r => r.User).Include(r => r.Status).Include(r=>r.Messages).FirstOrDefault();
            Dropdown dropdown = new Dropdown(db);
            ViewBag.StatusId = dropdown.GetStatus();
            requests.Details = "";
            return View(requests);
        }

        public ActionResult MyActionDetails(int id)
        {
            var requests = db.Requests.Where(r => r.Id == id)
                          .Include(r => r.Flow).Include(r => r.User).Include(r => r.Status).FirstOrDefault();
            Dropdown dropdown = new Dropdown(db);
            ViewBag.StatusId = dropdown.GetStatus();
            requests.Details = "";
            return View("MyRequestDetails", requests);
        }

        public ActionResult Create()
        {
            RequestForm requestForm = new RequestForm();
            Dropdown dropdown = new Dropdown(db, true);
            ViewBag.Location_Id = dropdown.Location;
            ViewBag.User_Id = dropdown.User;
            ViewBag.Status_Id = dropdown.Status;
            ViewBag.IssueType_Id = dropdown.IssueType;
            return View(requestForm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RequestForm requestForm)
        {
            string Paths = "";
            Request request = new Request();
            if (ModelState.IsValid)
            {
                var flow = db.Flows.Where(f => f.IssueType_Id == requestForm.IssueType_Id && f.Location_Id == requestForm.Location_Id).FirstOrDefault();
                if (flow!=null)
                {
                    Helper helper = new Helper();
                    request.Flow_Id = flow.Id;
                    request.Status_Id = 6;
                    request.User_Id = UserDetails.UserId;
                    request.Date = DateTime.Now;
                    request.Details = requestForm.Details;
                    Message Messages = new Message
                    {
                        CreatedBy = UserDetails.Name,
                        CreatedById = UserDetails.UserId,
                        CreatedOn = DateTime.Now,
                        Text = requestForm.Details,
                        Request_Id = request.Id,
                        Attachment = Paths
                    };
                    db.Requests.Add(request);
                    db.SaveChanges();
                    prefixPath = request.Id + "_" + DateTime.Now.ToString()+"_";
                    try
                    {
                        foreach (var Attachment in requestForm.Attachments)
                        {
                            if (Attachment != null && Attachment.ContentLength > 0)
                            {
                                string _FileName = prefixPath + Path.GetFileName(Attachment.FileName).Replace('~', '_');
                                string _path = Path.Combine(Server.MapPath("~/Attachments"), _FileName);
                                Attachment.SaveAs(_path);
                                Paths = Paths + _path + AttachmentSplit;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Paths = e.Message + "----"+ Paths;
                        var smpt=db.SMTP.FirstOrDefault();
                        helper.SendMail(smpt.UserName, smpt.UserName,"Exception in Code", e.Message);
                    }
                    Messages.Request_Id = request.Id;
                    Messages.Attachment = Paths;
                    db.Messages.Add(Messages);
                    db.SaveChanges();
                    helper.SendMailToAllMemberOfRequest(UserDetails.UserId, request.Id, requestForm.Details,true);
                    return RedirectToAction("MyRequest");
                }
                else
                {
                    requestForm.ErrorMessage="Flow did not exist";
                    requestForm.IsError = false;
                }
            }
            Dropdown dropdown = new Dropdown(db, true);
            ViewBag.Location_Id = dropdown.Location;
            ViewBag.User_Id = dropdown.User;
            ViewBag.Status_Id = dropdown.Status;
            ViewBag.IssueType_Id = dropdown.IssueType;
            return View(requestForm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Replay(Message message,int StatusId, HttpPostedFileBase[] Attachments)
        {

            prefixPath = message.Request_Id+ "_" + DateTime.Now.ToString() + "_";
            string Paths = "";
            Helper helper = new Helper();
            try
            {
                foreach (var Attachment in Attachments)
                {
                    if (Attachment != null && Attachment.ContentLength > 0)
                    {
                        string _FileName = prefixPath + Path.GetFileName(Attachment.FileName).Replace('~', '_');
                        string _path = Path.Combine(Server.MapPath("~/Attachments"), _FileName);
                        Attachment.SaveAs(_path);
                        Paths = Paths + _path + AttachmentSplit;
                    }
                }
            }
            catch (Exception e)
            {
                Paths = e.Message + "----" + Paths;
                var smpt = db.SMTP.FirstOrDefault();
                helper.SendMail(smpt.UserName, smpt.UserName, "Exception in Code", e.Message);
            }
            helper.SendMailToAllMemberOfRequest(UserDetails.UserId, (int)message.Request_Id, message.Text, false);
            db.Requests.Where(r => r.Id == message.Request_Id).FirstOrDefault().Status_Id= StatusId;
            db.SaveChanges();
            message.CreatedBy = UserDetails.Name;
            message.CreatedById = UserDetails.UserId;
            message.Attachment = Paths;
            db.Messages.Add(message);
            db.SaveChanges();
            return RedirectToAction("MyRequestDetails", new { id = message.Request_Id });
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
