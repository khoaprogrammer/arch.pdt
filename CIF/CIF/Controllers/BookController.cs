using CIF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CIF.DB;
using System.Text.RegularExpressions;
using System.Net;
using System.Text;
using System.IO;
using OpenQA.Selenium.PhantomJS;
using Microsoft.AspNet.Identity;
using HtmlAgilityPack;

namespace CIF.Controllers
{
    public class BookController : Controller
    {
        // GET: Book
        public ActionResult Index(int Id)
        {
            BookViewModel book = UIHelper.GetBook(Id);
            DBHelper.AddBookVisit(Id);
            if (book == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (book.DriveCode == null)
            {
                var firstContent = book.Contents.OrderBy(x => x.Order).First();
                ViewBag.FirstContent = firstContent.Code;
            }
            ViewBag.Title = book.Name + " - " + "Coding is Fun";
            /* STATS */
            var stats = DBHelper.GetStats();
            ViewBag.AllBookCount = stats.Item1;
            ViewBag.AllCourseCount = stats.Item2;
            ViewBag.AllUserCount = stats.Item3;
            return View(book);
        }

        [Authorize]
        public ActionResult Read(int Id, string code)
        {
            ContentViewModel c = UIHelper.GetContentView(Id, code);
            ContentViewModel prev = UIHelper.GetPrevContent(c);
            ContentViewModel next = UIHelper.GetNextContent(c);
            ViewBag.NextContent = next;
            ViewBag.PrevContent = prev;
            ViewBag.BookCSS = c.Book.CSSURL;
            ViewBag.BookCustomCss = c.Book.CustomCSS;
            ViewBag.BookId = c.BookId;
            c.HTML = CompressHelper.DecompressString(c.HTML);

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(c.HTML);
            var ps = doc.DocumentNode.Descendants("p").Where(x => x.Id.Contains("|"));
            foreach (var p in ps.ToList())
            {
                string pCode = p.Id.Split('|')[0];
                int pPos = int.Parse(p.Id.Split('|')[1]);
                TranslatedContent translated = DBHelper.GetTranslatedContent(c.BookId, pCode, pPos);
                if (translated != null)
                {
                    p.ParentNode.ReplaceChild(HtmlNode.CreateNode(translated.Data), p);
                }
            }

            c.HTML = Regex.Replace(doc.DocumentNode.OuterHtml, "href=\"(.+?)\\.(xhtml|html|htm)(#.+?)?\"", "href=\"" + Id + "?code=$1$3\"");
            return View(c);
        }

        [HttpPost]
        public ActionResult AddBookRead(int Id)
        {
            DBHelper.AddBookRead(Id);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PDFRead(int Id, int page = 1)
        {
            ViewBag.Page = page;
            return View(Id);
        }

        [HttpPost]
        public ActionResult GetPDFData(int Id)
        {
            BookViewModel b = UIHelper.GetBook(Id);

            string url = "https://drive.google.com/uc?export=download&id=" + b.DriveCode;

            WebClient web = new WebClient();

            var bytes = web.DownloadData(url);

            return new FileContentResult(bytes, "application/pdf");
        }
        
        [HttpPost]
        public ActionResult Translate(string text)
        {
            PhantomJSDriver driver = new PhantomJSDriver();
            driver.Navigate().GoToUrl("https://translate.google.com/#en/vi/" + text);
            string result = driver.FindElementById("result_box").Text;
            return Content(result); 
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult TranslateBook(string translateContent, string locationCode, int bookId)
        {
            string userId = null;
            if (User.Identity.IsAuthenticated)
            {
                userId = User.Identity.GetUserId();
            }
            string code = locationCode.Split('|')[0];
            int location = int.Parse(locationCode.Split('|')[1]);
            DBHelper.AddPendingTranslate(userId, translateContent, bookId, code, location);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}