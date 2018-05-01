using CIF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CIF.Controllers
{
    public class DocumentController : Controller
    {
        // GET: Document
        public ActionResult Index(int id)
        {
            var doc = UIHelper.GetDocument(id);
            return View(doc);
        }
    }
}