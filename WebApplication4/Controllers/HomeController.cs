using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication4.DBModel;
using TicketSystem.Models;

namespace TicketSystem.Controllers
{
    [AllowAnonymous]
    [Authorize]
    public class HomeController : Controller
    {
        private Entities db = new Entities();

        [Authorize]
        public ActionResult Index()
        {
            Dashboard dashboard = new Dashboard();
            dashboard.Users = db.Users.Count();
            dashboard.Flows = db.Flows.Count();
            dashboard.Locations = db.Locations.Count();
            dashboard.IssueTypes = db.IssueTypes.Count();
            dashboard.TotalRequests = db.Requests.Count();
            dashboard.Requests = db.Requests.Where(r => r.User_Id == UserDetails.UserId).Count();
            dashboard.Actions = (from r in db.Requests
                          join f in db.Flows on r.Flow_Id equals f.Id
                          where f.User_Id == UserDetails.UserId
                          select r).Count();

            return View(dashboard);
        }

        public ActionResult Login()
        {
            UserVM userVM = new UserVM();
            return View(userVM);
        }

        [HttpPost]
        public ActionResult Login(UserVM userVM, string ReturnUrl = "/Home/Index")
        {
            var user = db.Users.Where(u => u.UserName == userVM.UserName && u.Password == userVM.Password).FirstOrDefault();
            if (user != null)
            {
                string User = user.Id + "~" + user.FirstName + "~" + user.Role_Id;
                FormsAuthentication.Initialize();
                FormsAuthentication.SetAuthCookie(User, false);
                UserDetails.UserId = user.Id;
                UserDetails.RoleId = (int)user.Role_Id;
                UserDetails.Name = user.FirstName;
                return Redirect(ReturnUrl);
            }
            userVM.IsError = false;
            userVM.ErrorMessage = "Invalid UserName or Password";
            return View(userVM);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            ChangesPassword request = new ChangesPassword();
            return View(request);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangesPassword changespassword)
        {
            if (changespassword.Password == changespassword.ConfirmPassword)
            {
                db.Users.Where(u => u.Id == UserDetails.UserId).FirstOrDefault().Password = changespassword.ConfirmPassword;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                changespassword.ErrorMessage = "Unmatch Password";
                changespassword.IsError = false;
            }
            return View(changespassword);
        }

        public ActionResult ForgetPassword()
        {
            ForgetPassword ForgetPassword = new ForgetPassword();
            return View(ForgetPassword);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgetPassword(ForgetPassword forgetPassword)
        {
            User user = db.Users.Where(e => e.Email == forgetPassword.EmailID).FirstOrDefault();
            if (user != null)
            {
                Helper helper = new Helper();
                helper.SendMail(db.SMTP.FirstOrDefault().UserName, user.Email, "Your Password", "your current UserName <b>" + user.UserName + "</b> Password is <b>" + user.Password + "</b> please change your password after login <br> thanks <br> TS");
                forgetPassword.ErrorMessage = "Please check your email";
                forgetPassword.IsError = true;
            }
            else
            {
                forgetPassword.ErrorMessage = "Invalid Email Id";
                forgetPassword.IsError = false;
            }
            return View(forgetPassword);
        }
    }
}