using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CIF.Models
{
    public class SystemDataView
    {
        public int Id { get; set; }
        public string Code { get; set; }
        [AllowHtml]
        public string Data { get; set; }
    }
}