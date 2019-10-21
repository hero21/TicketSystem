using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication4.DBModel;

namespace TicketSystem.Models
{
    public class ViewModel
    {
        public ViewModel()
        {
            Flow = new Flow();
            IssueType = new IssueType();
            Location = new Location();
            Message = new Message();
            Request = new Request();
            Status = new Status();
            User = new User();
        }
        public Dropdown Dropdown { get; set; }
        public Flow Flow { get; set; }
        public IssueType IssueType { get; set; }
        public Location Location { get; set; }
        public Message Message { get; set; }
        public Request Request { get; set; }
        public Status Status { get; set; }
        public User User { get; set; }
    }
    public class Dashboard
    {
        public int Users { get; set; }
        public int Flows { get; set; }
        public int Locations { get; set; }
        public int IssueTypes { get; set; }
        public int TotalRequests { get; set; }
        public int Requests { get; set; }
        public int Actions { get; set; }
    }
}