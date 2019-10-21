using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicketSystem.Models
{
    public class ErrorModel
    {
        public string ErrorMessage { get; set; } = "";
        public bool IsError { get; set; } = true;
    }
}