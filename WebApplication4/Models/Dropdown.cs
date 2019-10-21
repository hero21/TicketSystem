using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication4.DBModel;

namespace TicketSystem.Models
{
    public class Dropdown
    {
        private Entities _db;
        public SelectList Flow { get; set; }
        public SelectList IssueType { get; }
        public SelectList Location { get; }
        public SelectList Request { get; set; }
        public SelectList Status { get; set; }
        public SelectList User { get; set; }

        public Dropdown(Entities db,bool FillData=false)
        {
            _db = db;
            if (FillData)
            {

                Flow = new SelectList(db.Flows, "Id", "Name");
                IssueType = new SelectList(db.IssueTypes, "Id", "Name");
                Location = new SelectList(db.Locations, "Id", "Name");
                Request = new SelectList(db.Requests, "Id", "Name");
                Status = new SelectList(db.Status, "Id", "Name");
                User = new SelectList(db.Users, "Id", "FirstName");
            }
        }

        public SelectList GetIssueTypes()
        {
            return new SelectList(_db.IssueTypes, "Id", "Name");
        }
        public SelectList GetLocations()
        {
            return new SelectList(_db.Locations, "Id", "Name");
        }
        public SelectList GetUsers()
        {
            return new SelectList(_db.Users, "Id", "FirstName");
        }
        public SelectList GetStatus()
        {
            return new SelectList(_db.Status, "Id", "Name");
        }
    }
}