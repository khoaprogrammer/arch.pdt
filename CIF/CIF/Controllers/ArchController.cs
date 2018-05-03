using CIF.DB;
using CIF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CIF.Controllers
{
    public class ArchController : Controller
    {
        // GET: Arch
        public ActionResult Index(int id)
        {
            ArchModelView a = UIHelper.getArchModelView(id);
            if (a.Type == ModelType.SKETCHUP)
                ViewBag.SketChup = true;
            return View(a);
        }
        
    }
}