using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIF.DB
{
    public class Document
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ViewCount { get; set; }
        public int DownloadCount { get; set; }
        public int ReadCount { get; set; }
        public string DriveCode { get; set; }
        public DateTime AddDate { get; set; }
        public virtual List<Topic> Topics { get; set; }
        public int PageCount { get; set; }
    }
}