using CIF.Filters;
using CIF.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CIF.Controllers
{
    public class BlogController : Controller
    {

        [UserToViewBag]
        [ShowStats]
        public ActionResult Index()
        {
            List<BlogEntryView> entries = UIHelper.GetAllBlogEntries();
            return View(entries);
        }

        [UserToViewBag]
        [ShowStats]
        public ActionResult Entry(int id)
        {
            DBHelper.AddBlogEntryRead(id);
            BlogEntryView entry = UIHelper.GetBlogEntry(id);
            ViewBag.Title = entry.Title + " - " + "Coding is FUN";
            return View(entry);
        }
    }
}