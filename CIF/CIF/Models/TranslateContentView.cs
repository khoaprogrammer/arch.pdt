using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIF.Models
{
    public class TranslateContentView
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string Data { get; set; }
        public string UserId { get; set; }
        public string Code { get; set; }
        public int Location { get; set; }
    }
}