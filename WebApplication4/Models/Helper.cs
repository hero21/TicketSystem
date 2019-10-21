using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using WebApplication4.DBModel;

namespace TicketSystem.Models
{
    public class Helper
    {
        private SmtpClient Smtp { get; set; } = new SmtpClient();
        private Entities db;
        public Helper()
        {
            db = new Entities();
            var Config=db.SMTP.FirstOrDefault();
            NetworkCredential NetworkCred = new NetworkCredential
            {
                UserName = Config.UserName,
                Password = Config.Password
            };
            Smtp.Host = Config.Server;
            Smtp.Port = Config.Port;
            Smtp.EnableSsl = Config.EnableSsl;
            Smtp.UseDefaultCredentials = false;
            Smtp.Credentials =  NetworkCred;
            Smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
        }
        public void SendMail(string From, string To, string Subject, string Body)
        {
            using (MailMessage mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress(From);
                mailMessage.To.Add(To);
                mailMessage.Subject = Subject;
                mailMessage.Body = Body;
                mailMessage.IsBodyHtml = true;
                Smtp.Send(mailMessage);
            }
        }

        public void SendMailToAllMemberOfRequest(int CurrentUserId, int RequestId,string Message, bool IsNew = true)
        {
            string Subject = "";
            string ToBody = "";
            string FromBody = "";
            string[] MessageInfo = new string[] { "created a new Request", "recived a new request","send a new meessage", "recived a new message" };
            string MessageTemplate = "<h4>Hi {UserName}</h4><p>You have just {MessageInfo}, Ticket No. <b>{RequestId} </b> for Issue Type <b> {IssueType} </b> at Location <b> {Location} </b> with Message <b> {Message} </b></p><p>Thank</p><p>TS</p>";
            User toUser = new User();
            User fromUser = db.Users.Where(u => u.Id == CurrentUserId).FirstOrDefault();
            Request request = db.Requests.Where(r => r.Id == RequestId).FirstOrDefault();
            Flow flow = db.Flows.Where(f => f.Id == request.Flow_Id).FirstOrDefault();
            IssueType issueType = db.IssueTypes.Where(i => i.Id == flow.IssueType_Id).FirstOrDefault();
            Location location = db.Locations.Where(l => l.Id == flow.Location_Id).FirstOrDefault();
            string TempMessage = MessageTemplate.Replace("{RequestId}", RequestId.ToString())
                                                .Replace("{IssueType}", issueType.Name)
                                                .Replace("{Location}", location.Name)
                                                .Replace("{Message}", Message);
            FromBody = TempMessage.Replace("{UserName}", fromUser.FirstName + " " + fromUser.LastName);
            ToBody = TempMessage.Replace("{UserName}", toUser.FirstName + " " + toUser.LastName);
            FromBody = IsNew ? FromBody.Replace("{MessageInfo}", MessageInfo[0]) : FromBody.Replace("{MessageInfo}", MessageInfo[2]);
            ToBody = IsNew ? ToBody.Replace("{MessageInfo}", MessageInfo[1]) : ToBody.Replace("{MessageInfo}", MessageInfo[3]);
            Subject=IsNew? Subject = "New Ticket No." + RequestId:Subject = "New Reply Ticket No." + RequestId;
            if (CurrentUserId == flow.User_Id) //Action  send Create
            {
                toUser = db.Users.Where(u => u.Id == request.User_Id).FirstOrDefault();
            }
            else //Request
            {
                toUser = db.Users.Where(u => u.Id == flow.User_Id).FirstOrDefault();
            }
            SendMail(fromUser.Email,fromUser.Email,Subject, FromBody);
            SendMail(fromUser.Email, toUser.Email, Subject, ToBody);
        }
    }
}