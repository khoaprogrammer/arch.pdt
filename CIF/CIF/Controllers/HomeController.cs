using CIF.DB;
using CIF.Filters;
using CIF.Models;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CIF.Controllers
{
    public class HomeController : Controller
    {
        [UserToViewBag]
        [ShowStats]
        public ActionResult Index()
        {
            /* List<BookViewModel> lastestBooks = UIHelper.GetLastestBooks(10);
             ViewBag.LastestBooks = lastestBooks;
             List<BlogEntryView> lastestEntries = UIHelper.GetLastestBlogEntries(Globals.NumOfLastestBlogEntryShow);
             ViewBag.LastestBlogEntries = lastestEntries;
             if (HttpContext.Request.Browser.IsMobileDevice)
             {
                 ViewBag.IsMobile = true;
             }
             ViewBag.ShowTopSearch = true;*/
            List<ArchModelView> Max3D = UIHelper.GetAll3DMax();
            List<ArchModelView> SketChup = UIHelper.GetAllSketChup();
            ViewBag.Max3D = Max3D;
            ViewBag.SketChup = SketChup;
            if (HttpContext.Request.Browser.IsMobileDevice)
            {
                ViewBag.IsMobile = true;
            }

            return View();
        }

        [UserToViewBag]
        [ShowStats]
        public ActionResult Books(int[] topicIds = null, int authorId = -1, int publisherId = -1, int page = 1, string search = "*")
        {
            var books = UIHelper.GetAllBooksView(topicIds, authorId, publisherId, page, search);
            ViewBag.Search = search;
            ViewBag.TotalCount = books.Item2;
            ViewBag.Categories = UIHelper.GetTopicTree();
            ViewBag.Authors = UIHelper.GetAllAuthorsView();
            ViewBag.Authors.Insert(0, new AuthorViewModel { Id = -1, Name = "All" });
            ViewBag.Publishers = UIHelper.GetAllPublishersView();
            ViewBag.Publishers.Insert(0, new PublisherViewModel { Id = -1, Name = "All" });
            ViewBag.Page = page;
            ViewBag.HideTopSearch = true;
            return View(books.Item1);
        }

        [UserToViewBag]
        [ShowStats]
        public ActionResult Shop()
        {
            ViewBag.Message = TempData["Message"];
            return View();
        }

        [UserToViewBag]
        [ShowStats]
        public ActionResult Donate()
        {
            var bankAccounts = DBHelper.GetBankAccountsData();
            ViewBag.BankAccounts = bankAccounts;
            ViewBag.Message = TempData["Message"];
            return View();
        }

        [UserToViewBag]
        public ActionResult Documents(int[] topicIds, int page = 1, string search = "*")
        {
            var docs = UIHelper.GetAllDocuments(topicIds, page, search);
            ViewBag.Page = page;
            ViewBag.Categories = UIHelper.GetTopicTree();
            ViewBag.TotalCount = docs.Item2;
            return View(docs.Item1);
        }

        public ActionResult Test()
        {
            ApplicationUserManager manager = new ApplicationUserManager(new UserStore<ApplicationUser>(ApplicationDbContext.Create()));
            //manager.Create(new ApplicationUser
            //{
            //    UserName = "arch.support@phongdaotao.com",
            //    Email = "arch.support@phongdaotao.com"
            //}, "Abc123456789*");
            var context = ApplicationDbContext.Create();
            context.Roles.Add(new IdentityRole
            {
                Id = "ADMIN",
                Name = "ADMIN"
            });
            context.SaveChanges();
            manager.AddToRole(User.Identity.GetUserId(), "ADMIN");

            return Content("OK");
        }
        public ActionResult Max3D(int page = 1, string search = "*")
        {

            //var books = UIHelper.GetAllBooksView(topicIds, authorId, publisherId, page, search);
            ViewBag.Search = search;
            ViewBag.Page = page;
            Tuple<List<ArchModelView>, int> list = UIHelper.GetAll_Max3D(search, page);
            ViewBag.TotalCount = list.Item2;
            return View(list.Item1);
        }
        public ActionResult SketChup(int page = 1, string search = "*")
        {

            //var books = UIHelper.GetAllBooksView(topicIds, authorId, publisherId, page, search);
            ViewBag.Search = search;
            ViewBag.Page = page;
            Tuple<List<ArchModelView>, int> list = UIHelper.GetAll_SketChup(search, page);
            ViewBag.TotalCount = list.Item2;
            return View(list.Item1);
        }

       
     /*   public ActionResult Admin()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            db.Roles.Add(new IdentityRole
            {
                Id = "ADMIN",
                Name = "ADMIN"
            });
            db.SaveChanges();
            ApplicationUserManager manager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
            ApplicationUser user = new ApplicationUser
            {
                Email = "arch.support@phongdaotao.com",
                UserName = "arch.support@phongdaotao.com",
                SecurityStamp = Guid.NewGuid().ToString()
            };
            manager.Create(user, "Abc123546789*");
            manager.AddToRole(user.Id, "ADMIN");
            db.SaveChanges();
            return Content("OK");
        }*/

    }
}