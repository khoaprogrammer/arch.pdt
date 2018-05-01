using CIF.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CIF.Models
{
    public class SupportTicketView
    {
        public int Id { get; set; }
        public string title { get; set; }
        public string Email { get; set; }
        [AllowHtml]
        public string Detail { get; set; }
        public SupportTickerType Type { get; set; }
        public DateTime Time { get; set; }
        public List<TicketMessage> Messages { get; set; }
    }
}