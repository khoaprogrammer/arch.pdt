using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIF.Models
{
    public class TicketMessage
    {
        public string Detail { get; set; }
        public DateTime Date { get; set; }
        public string Sender { get; set; }
    }
}