using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIF.DB
{
    public class BookRequest
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public BookRequestStatus Status { get; set; }
        public string Requester { get; set; }
        public string Email { get; set; }
    }
}