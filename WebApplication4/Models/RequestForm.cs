using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication4.DBModel;

namespace TicketSystem.Models
{
    public class RequestForm : ErrorModel
    {
        public int Id { get; set; }
        public System.DateTime Date { get; set; }
        public string Details { get; set; }
        public Nullable<int> Location_Id { get; set; }
        public Nullable<int> User_Id { get; set; }
        public Nullable<int> Status_Id { get; set; }
        public Nullable<int> IssueType_Id { get; set; }
        public HttpPostedFileBase[] Attachments  { get; set; }
    }
    public class ChangesPassword : ErrorModel
    {
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
    public class ForgetPassword : ErrorModel
    {
        public string EmailID { get; set; }

    }
}