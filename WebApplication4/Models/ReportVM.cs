
using WebApplication4.DBModel;
namespace TicketSystem.Models
{
    public class ReportVM
    {
        public int Id { get; set; }
        public int Flow_Id { get; set; }
        public int IssueType_Id { get; set; }
        public int Location_Id { get; set; }
        public int Request_Id { get; set; }
        public int Status_Id { get; set; }
        public int User_Id { get; set; }
        public virtual Flow Flow { get; set; }
        public virtual IssueType IssueType  { get; set; }
        public virtual Location Location { get; set; }
        public virtual Request Request { get; set; }
        public virtual Status Status { get; set; }
        public virtual User User{ get; set; }
    }
}