using CIF.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIF.Models
{
    public class ContentViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string HTML { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
        public int Order { get; set; }
        public List<TOCLine> TOCLines { get; set; }
        public string Next { get; set; }
        public string Prev { get; set; }
    }
}