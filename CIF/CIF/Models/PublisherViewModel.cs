﻿using CIF.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CIF.Models
{
    public class PublisherViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [AllowHtml]
        public string Introduction { get; set; }
        public virtual ICollection<Book> Books { get; set; }
    }
}