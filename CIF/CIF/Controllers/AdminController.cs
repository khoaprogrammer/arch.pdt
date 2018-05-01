using CIF.DB;
using CIF.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExcelDataReader.Core;
using ExcelDataReader;
using System.Data;
using System.IO;
using System.Globalization;
using Newtonsoft.Json;
using Google.Apis.Auth.OAuth2;
using System.Threading;
using HtmlAgilityPack;

namespace CIF.Controllers
{
    public class AdminController : Controller
    {
        // Helper Methods
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        // GET: Admin
        public ActionResult Stats()
        {
            string credPath = Server.MapPath("~/Content/PhongDaoTao-Book-1818d16b5e2f.json");

            var json = System.IO.File.ReadAllText(credPath);
            Newtonsoft.Json.Linq.JObject cr = (Newtonsoft.Json.Linq.JObject)JsonConvert.DeserializeObject(json); // "personal" service account credential

            // Create an explicit ServiceAccountCredential credential
            var xCred = new ServiceAccountCredential(new ServiceAccountCredential.Initializer((string)cr.GetValue("client_email"))
            {
                Scopes = new[] { "https://www.googleapis.com/auth/analytics.readonly" }
            }.FromPrivateKey((string)cr.GetValue("private_key")));
            bool status = xCred.RequestAccessTokenAsync(CancellationToken.None).Result;
            ViewBag.GAAccessToken = xCred.Token.AccessToken;
            return View();
        }

        [RuleCheckFilter(RuleName = "ADMIN_INDEX")]
        public ActionResult Index()
        {
            return View();
        }

        [RuleCheckFilter(RuleName = "ADMIN_ACCOUNTS")]
        public ActionResult Accounts()
        {
            var context = new ApplicationDbContext();
            return View(context.Users);
        }

        [RuleCheckFilter(RuleName = "ADMIN_ADD_ACCOUNT")]
        public ActionResult AddAccount()
        {
            return View(new RegisterViewModel());
        }

        [RuleCheckFilter(RuleName = "ADMIN_ADD_ACCOUNT")]
        [HttpPost]
        public ActionResult AddAccount(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                UserName = model.Email
            };
            var context = new ApplicationDbContext();
            var uManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            var result = uManager.Create(user, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Accounts", "Admin");
            }
            AddErrors(result);

            return View(model);
        }

        [RuleCheckFilter(RuleName = "ADMIN_BOOKS")]
        public ActionResult Books(string search = "*", string cat = null, int page = 1)
        {
            Tuple<List<BookViewModel>, int> result;
            if (cat != null)
            {
                int nocatId = ApplicationDbContext.Create().Topics.First(x => x.Name == "Chưa phân loại").Id;
                result = UIHelper.GetAllBooksView(new int[] { nocatId }, -1, -1, 1, search);
            } else
            {
                result = UIHelper.GetAllBooksView(null, -1, -1, 1, search);
            }
            ViewBag.TotalCount = result.Item2;
            ViewBag.Page = page;
            ViewBag.CompleteMessage = TempData["CompleteMessage"];
            return View(result.Item1);
        }

        [RuleCheckFilter(RuleName = "ADMIN_ADD_BOOK")]
        public ActionResult AddBook()
        {
            return View();
        }

        [RuleCheckFilter(RuleName = "ADMIN_ADD_BOOK")]
        [HttpPost]
        public ActionResult AddBook(HttpPostedFileBase file)
        {
            Book b = new Book();
            List<TOCLine> tocs = new List<TOCLine>();
            string css;
            string name;
            string shortCode;
            string isbn;
            string author;
            string publisher;
            string description;
            DateTime published;
            using (StreamReader reader = new StreamReader(file.InputStream))
            {
                shortCode = reader.ReadLine().Trim().Split('/')[5];
                name = reader.ReadLine().Trim();
                published = DateTime.ParseExact(reader.ReadLine(), "MMMM yyyy", CultureInfo.InvariantCulture);
                isbn = reader.ReadLine().Trim();
                author = reader.ReadLine().Trim();
                publisher = reader.ReadLine().Trim();
                description = reader.ReadLine().Trim();
                css = reader.ReadLine().Trim();
                if (reader.ReadLine().Trim() == "")
                {
                    reader.ReadLine();
                }
                int section = 0;
                int contentOrder = 1;
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine().Trim();
                    if (line == "-----")
                    {
                        section += 1;
                        contentOrder = 1;
                        continue;
                    }
                    if (section == 0)
                    {
                        var parts = line.Split(new string[] { "<KHOA>" }, StringSplitOptions.None);
                        string code = parts[0].Split('.')[0];
                        if (code.Contains("ch02"))
                        {
                            code = code;
                        }
                        string html = parts.Last().Replace("<PHONGDAOTAO>", Environment.NewLine);
                        var document = new HtmlDocument();
                        document.LoadHtml(html);
                        var ps = document.DocumentNode.Descendants("p");
                        int id = 1;
                        foreach (var p in ps)
                        {
                            p.Attributes.Add("id", code + "|" + id);
                            id++;
                        }
                        string test = document.DocumentNode.OuterHtml;
                        b.Contents.Add(new Content
                        {
                            Code = code,
                            HTML = CompressHelper.CompressString(document.DocumentNode.OuterHtml),
                            Order = contentOrder
                        });
                    }
                    else
                    {
                        var parts = line.Split('|');
                        string title = parts[0];
                        string code = null;
                        string anchor = null;
                        int level = int.Parse(parts[2]);

                        if (parts[1].Contains("#"))
                        {
                            parts = parts[1].Split('#');
                            code = parts[0].Replace(".html", "").Replace(".htm", "");
                            anchor = parts[1];
                        }
                        else
                        {
                            code = parts[1].Replace(".html", "").Replace(".htm", "");
                        }

                        b.TOCLines.Add(new TOCLine
                        {
                            Anchor = anchor,
                            Level = level,
                            Title = title,
                            Order = contentOrder,
                            Content = b.Contents.FirstOrDefault(x => x.Code == code)
                        });
                    }
                    contentOrder += 1;
                }
            }
            b.ShortCode = shortCode;
            b.Name = name;
            b.Description = description;
            b.ISBN = isbn;
            b.PublishDate = published;
            b.CustomCSS = css;
            Publisher pub = DBHelper.GetPublisher(publisher);
            if (pub == null)
            {
                pub = new Publisher
                {
                    Name = publisher
                };
                b.PublisherId = DBHelper.AddPublisher(pub);
            }
            else
            {
                b.PublisherId = pub.Id;
            }
            var authors = author.Split(',');
            List<int> autIds = new List<int>();
            foreach (string au in authors)
            {
                Author aut = DBHelper.GetAuthor(au);
                if (aut == null)
                {
                    aut = new Author
                    {
                        Name = au
                    };
                    autIds.Add(DBHelper.AddAuthor(aut));
                } else
                {
                    autIds.Add(aut.Id);
                }
            }
            b.Authors = autIds.Select(x => new Author { Id = x }).ToList();
            TempData["addingBook"] = b;
            return RedirectToAction("AddBookDetail");
        }

        [RuleCheckFilter(RuleName = "ADMIN_ADD_PDF_BOOK")]
        public ActionResult AddPDFBook()
        {
            List<TopicViewModel> topics = UIHelper.GetTopicTree();
            List<PublisherViewModel> publishers = UIHelper.GetAllPublishersView();
            List<AuthorViewModel> authors = UIHelper.GetAllAuthorsView();
            ViewBag.TopicList = topics;
            ViewBag.AuthorList = authors;
            ViewBag.PublisherList = publishers;
            return View("~/Views/Admin/AddBookDetail.cshtml", new BookViewModel());
        }

        [RuleCheckFilter(RuleName = "ADMIN_ADD_BOOK_DETAIL")]
        public ActionResult AddBookDetail()
        {
            Book addingBook = (Book)TempData["addingBook"];
            List<TopicViewModel> topics = UIHelper.GetTopicTree();
            List<PublisherViewModel> publishers = UIHelper.GetAllPublishersView();
            List<AuthorViewModel> authors = UIHelper.GetAllAuthorsView();
            ViewBag.TopicList = topics;
            ViewBag.AuthorList = authors;
            ViewBag.PublisherList = publishers;
            ViewBag.FileName = addingBook.Name;
            TempData["AddingBook"] = addingBook;
            return View(ModelConverter.BookToView(addingBook));
        }

        [RuleCheckFilter(RuleName = "ADMIN_ADD_BOOK_DETAIL")]
        [HttpPost]
        public ActionResult AddBookDetail(BookViewModel model, HttpPostedFileBase coverFile)
        {
            if (model.DriveCode != null)
            {
                model.Contents = new List<DB.Content>();
                model.TOCLines = new List<TOCLine>();
                DBHelper.AddBook(model, coverFile, null, null, null, null);
            } else
            {
                Book b = (Book)TempData["AddingBook"];
                model.Contents = b.Contents.ToList();
                var toc = b.TOCLines.ToList();
                DBHelper.AddBook(model, coverFile, toc, b.CSSURL, b.ShortCode, b.CustomCSS);
            }
            TempData["CompleteMethod"] = "<b>" + model.Name + "</b> was added successfully!";
            return RedirectToAction("Books");
        }

        [RuleCheckFilter(RuleName = "ADMIN_EDIT_BOOK")]
        public ActionResult EditBook(int Id)
        {
            BookViewModel b = UIHelper.GetBook(Id);
            List<TopicViewModel> topics = UIHelper.GetTopicTree();
            List<PublisherViewModel> publishers = UIHelper.GetAllPublishersView();
            List<AuthorViewModel> authors = UIHelper.GetAllAuthorsView();
            ViewBag.TopicList = topics;
            ViewBag.AuthorList = authors;
            ViewBag.PublisherList = publishers;
            return View(b);
        }

        [RuleCheckFilter(RuleName = "ADMIN_EDIT_BOOK")]
        [HttpPost]
        public ActionResult EditBook(BookViewModel model, HttpPostedFileBase coverFile)
        {
            DBHelper.UpdateBook(model);
            if (coverFile != null)
            {
                coverFile.SaveAs(Path.Combine(Url.Content(Globals.BookCoverPath), model.Id + ".png"));
            }
            TempData["CompleteMessage"] = "<b>" + model.Name + "</b> was updated successfully!";

            return RedirectToAction("Books");
        }

        [RuleCheckFilter(RuleName = "ADMIN_TOPICS")]
        public ActionResult Topics()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            List<TopicViewModel> models = UIHelper.GetTopicTree();
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            ViewBag.CompleteMessage = TempData["CompleteMessage"];
            return View(models);
        }

        [RuleCheckFilter(RuleName = "ADMIN_ADD_TOPIC")]
        public ActionResult AddTopic()
        {
            List<TopicViewModel> topics = UIHelper.GetTopicTree();
            topics.Insert(0, new TopicViewModel
            {
                Level = 1,
                Name = "ROOT",
                Id = -1
            });
            SelectList parentList = new SelectList(topics, "Id", "DisplayName");
            ViewBag.ParentList = parentList;
            return View();
        }

        [RuleCheckFilter(RuleName = "ADMIN_ADD_TOPIC")]
        [HttpPost]
        public ActionResult AddTopic(TopicViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            Topic add = ModelConverter.ViewToTopic(model);
            ApplicationDbContext context = new ApplicationDbContext();
            add.ParentId = model.ParentId == -1 ? null : model.ParentId;
            if (add.ParentId == null)
            {
                add.Level = 0;
            } else
            {
                add.Level = context.Topics.First(x => x.Id == add.ParentId).Level + 1;
            }
            context.Topics.Add(add);
            context.SaveChanges();
            return RedirectToAction("Topics");
        }

        [RuleCheckFilter(RuleName = "ADMIN_DELETE_TOPIC")]
        public ActionResult DeleteTopic(int Id)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            Topic delete = context.Topics.First(x => x.Id == Id);
            if (delete.Books.Count > 0)
            {
                TempData["ErrorMessage"] = "You're trying to delete a topic that contains books. Please move books to another topic and try again!";
                return RedirectToAction("Topics");
            }
            if (delete.Childs.Count > 0)
            {
                TempData["ErrorMessage"] = "The topic that you're trying to delete contains child topics. Please delete them first and then try again!";
                return RedirectToAction("Topics");
            }
            context.Topics.Remove(delete);
            context.SaveChanges();
            TempData["CompleteMessage"] = "Topic <b>" + delete.Name + "</b> was deleted successfully";
            return RedirectToAction("Topics");
        }

        [RuleCheckFilter(RuleName = "ADMIN_EDIT_TOPIC")]
        public ActionResult EditTopic(int Id)
        {
            TopicViewModel model = UIHelper.GetTopicView(Id);
            List<TopicViewModel> topics = UIHelper.GetTopicTree().Where(x => x.Id != Id).ToList();
            topics.Insert(0, new TopicViewModel
            {
                Level = 1,
                Name = "ROOT",
                Id = -1
            });
            ViewBag.ParentList = topics;
            return View(model);
        }

        [RuleCheckFilter(RuleName = "ADMIN_EDIT_TOPIC")]
        [HttpPost]
        public ActionResult EditTopic(TopicViewModel topic)
        {
            if (!ModelState.IsValid)
            {
                return View(topic);
            }
            DBHelper.UpdateTopic(topic);
            TempData["CompleteMessage"] = "Topic " + topic.Name + " was updated successfully!";
            return RedirectToAction("Topics");
        }

        [RuleCheckFilter(RuleName = "ADMIN_AUTHORS")]
        public ActionResult Authors()
        {
            var context = ApplicationDbContext.Create();
            List<AuthorViewModel> authors = UIHelper.GetAllAuthorsView();
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            ViewBag.CompleteMessage = TempData["CompleteMessage"];
            return View(authors);
        }

        [RuleCheckFilter(RuleName = "ADMIN_ADD_AUTHOR")]
        public ActionResult AddAuthor()
        {
            return View();
        }

        [RuleCheckFilter(RuleName = "ADMIN_ADD_AUTHOR")]
        [HttpPost]
        public ActionResult AddAuthor(AuthorViewModel model, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (file != null)
            {
                file.SaveAs(Path.Combine(Globals.AuthorAvatarPath, model.Id + ".png"));
            }

            DBHelper.AddAuthor(model);

            TempData["CompleteMessage"] = "Author <b>" + model.Name + "</b> was added successfully!";

            return RedirectToAction("Authors");
        }

        [RuleCheckFilter(RuleName = "ADMIN_DELETE_AUTHOR")]
        public ActionResult DeleteAuthor(int id)
        {
            Author del = DBHelper.GetAuthor(id);
            if (del.Books.Count > 0)
            {
                TempData["ErrorMessage"] = "The author you're trying to delete has books.";
            }
            else
            {
                DBHelper.DeleteAuthor(del);
                TempData["CompleteMessage"] = del.Name + " was deleted successfully!";
            }
            return RedirectToAction("Authors");
        }

        [RuleCheckFilter(RuleName = "ADMIN_PUBLISHER")]
        public ActionResult Publishers()
        {
            List<PublisherViewModel> pubs = UIHelper.GetAllPublishersView();

            ViewBag.CompleteMessage = TempData["CompleteMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            return View(pubs);
        }

        [RuleCheckFilter(RuleName = "ADMIN_ADD_PUBLISHER")]
        public ActionResult AddPublisher()
        {
            return View();
        }

        [RuleCheckFilter(RuleName = "ADMIN_ADD_PUBLISHER")]
        [HttpPost]
        public ActionResult AddPublisher(PublisherViewModel model, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            DBHelper.AddPublisher(model);
            if (file != null)
            {
                file.SaveAs(Path.Combine(Globals.PublisherLogoPath, model.Id + ".png"));
            }
            TempData["CompleteMessage"] = model.Name + " was added successfully!";
            return RedirectToAction("Publishers");
        }

        [RuleCheckFilter(RuleName = "ADMIN_DELETE_PUBLISHER")]
        public ActionResult DeletePublisher(int id)
        {
            Publisher delete = DBHelper.GetPublisher(id);
            if (delete.Books.Count > 0)
            {
                TempData["ErrorMessage"] = "The publisher that you're trying to delete owns books.";
            }
            else
            {
                DBHelper.DeletePublisher(delete);
                TempData["CompleteMessage"] = "Publisher <b>" + delete.Name + "</b> was deleted successfully!";
            }
            return RedirectToAction("Publishers");
        }

        [RuleCheckFilter(RuleName = "ADMIN_DELETE_BOOK")]
        public ActionResult DeleteBook(int Id)
        {
            Book delete = DBHelper.GetBook(Id);
            DBHelper.DeleteBook(Id);
            TempData["CompleteMessage"] = "<b>" + delete.Name + "</b> was deleted successfully!";
            return RedirectToAction("Books");
        }

        [RuleCheckFilter(RuleName = "ADMIN_REQUEST")]
        public ActionResult Requests(int type = -1)
        {
            List<SupportTicketView> tickets;
            if (type == -1)
            {
                tickets = UIHelper.GetAllSupportTicketsView();
            }
            else
            {
                SupportTickerType ticketType = (SupportTickerType)type;
                tickets = UIHelper.GetAllSupportTicketsView().Where(x => x.Type == ticketType).OrderByDescending(x => x.Time).ToList();
            }
            return View(tickets);
        }

        [RuleCheckFilter(RuleName = "ADMIN_BLOG")]
        public ActionResult Blog()
        {
            List<BlogEntryView> entries = UIHelper.GetAllBlogEntries();
            ViewBag.CompleteMessage = TempData["CompleteMessage"];
            return View(entries);
        }

        [RuleCheckFilter(RuleName = "ADMIN_ADD_BLOG_ENTRY")]
        public ActionResult AddBlogEntry()
        {
            List<BlogCategoryView> categories = UIHelper.GetBlogCategoryTree();
            ViewBag.CategoryList = categories;
            return View();
        }

        [RuleCheckFilter(RuleName = "ADMIN_ADD_BLOG_ENTRY")]
        [HttpPost]
        public ActionResult AddBlogEntry(BlogEntryView model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            DBHelper.AddBlogEntry(model);
            TempData["CompleteMessage"] = "Bài viết <b>" + model.Title + "</b> đã được đăng thành công!";
            return RedirectToAction("Blog");
        }

        [RuleCheckFilter(RuleName = "ADMIN_BLOG_CATEGORIES")]
        public ActionResult BlogCategories()
        {
            List<BlogCategoryView> categories = UIHelper.GetBlogCategoryTree();
            ViewBag.CompleteMessage = TempData["CompleteMessage"];
            return View(categories);
        }

        [RuleCheckFilter(RuleName = "ADMIN_ADD_BLOG_CATEGORY")]
        public ActionResult AddBlogCategory()
        {
            List<BlogCategoryView> cats = UIHelper.GetBlogCategoryTree();
            cats.Insert(0, new BlogCategoryView
            {
                Level = 1,
                Name = "ROOT",
                Id = -1
            });
            SelectList parentList = new SelectList(cats, "Id", "DisplayName");
            ViewBag.ParentList = parentList;
            return View();
        }

        [RuleCheckFilter(RuleName = "ADMIN_ADD_BLOG_CATEGORY")]
        [HttpPost]
        public ActionResult AddBlogCategory(BlogCategoryView model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            DBHelper.AddBlogCategory(model);
            TempData["CompleteMessage"] = "Danh mục <b>" + model.Name + "</b> đã được thêm thành công!";
            return RedirectToAction("BlogCategories");
        }

        [RuleCheckFilter(RuleName = "ADMIN_ROLES")]
        public ActionResult Roles()
        {
            var roles = UIHelper.GetAllRoles();
            ViewBag.CompleteMessage = TempData["CompleteMessage"];
            return View(roles);
        }

        [RuleCheckFilter(RuleName = "ADMIN_ADD_ROLE")]
        public ActionResult AddRole()
        {
            return View();
        }

        [RuleCheckFilter(RuleName = "ADMIN_ADD_ROLE")]
        [HttpPost]
        public ActionResult AddRole(RoleView model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            DBHelper.AddRole(model);
            TempData["CompleteMessage"] = "Role <b>" + model.Name + "</b> was added successfully!";
            return RedirectToAction("Roles");
        }
        
        
        
        [RuleCheckFilter(RuleName = "ADMIN_ADD_TAG")]
        public ActionResult AddTag()
        {
            return View();
        }

        [RuleCheckFilter(RuleName = "ADMIN_OPEN_COURSE_SESSION")]
        public ActionResult OpenCourseSession(int Id)
        {
            return View();
        }
        

        [RuleCheckFilter(RuleName = "ADMIN_ACCESS_TOKENS")]
        public ActionResult AccessTokens()
        {
            List<AccessTokenView> tokens = UIHelper.GetAllAccessTokens();
            return View(tokens);
        }

        [RuleCheckFilter(RuleName = "ADMIN_ADD_TOKEN")]
        public ActionResult AddToken()
        {
            Guid g = Guid.NewGuid();
            string GuidString = Convert.ToBase64String(g.ToByteArray());
            GuidString = GuidString.Replace("=", "");
            GuidString = GuidString.Replace("+", "");
            GuidString = GuidString.Replace("/", "");

            DBHelper.AddToken(GuidString);
            return RedirectToAction("AccessTokens");
        }

        [RuleCheckFilter(RuleName = "ADMIN_UPDATE_LESSON_CAPTION")]
        [HttpPost]
        public ActionResult UpdateLessonCaption(int lessonId, int sessionId,  HttpPostedFileBase captionFile)
        {
            if (captionFile != null)
            {
                captionFile.SaveAs(Server.MapPath(Path.Combine(Globals.CourseCaptionPath, lessonId + ".vtt")));
            }
            return RedirectToAction("CourseDetail", new { Id = sessionId });
        }

        [RuleCheckFilter(RuleName = "ADMIN_FUNCTIONS")]
        public ActionResult Functions()
        {
            var functions = DBHelper.GetAllFunctions();
            return View(functions);
        }

        [RuleCheckFilter(RuleName = "ADMIN_ACCOUNT_DETAIL")]
        public ActionResult AccountDetail(string Id)
        {
            var user = UIHelper.GetUser(Id);
            ViewBag.Functions = DBHelper.GetAllFunctions();
            ViewBag.Groups = UIHelper.GetAllRoles();  
            return View(user);
        }

        [RuleCheckFilter(RuleName = "ADMIN_UPDATE_USER")]
        [HttpPost]
        public ActionResult UpdateUser(string Id, string[] groups, string[] functions)
        {
            DBHelper.UpdateUser(Id, groups, functions);
            return RedirectToAction("Accounts");
        }
        
        
        [RuleCheckFilter(RuleName ="ADMIN_TRANSLATES")]
        public ActionResult Translates()
        {
            var pendings = UIHelper.GetAllPendingTranslates();
            return View(pendings);
        }

        [RuleCheckFilter(RuleName = "ADMIN_TRANSLATE_DETAIL")]
        public ActionResult TranslateDetail(int Id)
        {
            var translate = UIHelper.GetPendingTranslates(Id);
            Book b = DBHelper.GetBook(translate.BookId);
            string originalHTML = CompressHelper.DecompressString(b.Contents.First(x => x.Code == translate.Code).HTML);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(originalHTML);
            var element = doc.DocumentNode.Descendants("p").First(x => x.GetAttributeValue("id", "no") == translate.Code + "|" + translate.Location);
            ViewBag.Original = element.OuterHtml;
            return View(translate);
        }

        [HttpPost]
        [RuleCheckFilter(RuleName = "ADMIN_APPROVE_TRANSLATE")]
        public ActionResult ApproveTranslate(int Id)
        {
            DBHelper.ApproveTranslate(Id);
            return RedirectToAction("Translates");
        }

        [RuleCheckFilter(RuleName = "ADMIN_DELETE_BLOG_ENTRY")]
        [HttpPost]
        public ActionResult DeleteBlogEntry(int Id)
        {
            DBHelper.DeleteBlogEntry(Id);
            return RedirectToAction("Blog");
        }

        [RuleCheckFilter(RuleName = "ADMIN_EDIT_BLOG_ENTRY")]
        public ActionResult EditBlogEntry(int Id)
        {
            BlogEntryView blog = UIHelper.GetBlogEntry(Id);
            List<BlogCategoryView> categories = UIHelper.GetBlogCategoryTree();
            ViewBag.CategoryList = categories;
            return View(blog);
        }

        [RuleCheckFilter(RuleName = "ADMIN_EDIT_BLOG_ENTRY")]
        [HttpPost]
        public ActionResult EditBlogEntry(BlogEntryView model)
        {
            DBHelper.UpdateBlogCategory(model);
            return RedirectToAction("Blog");
        }

        [RuleCheckFilter(RuleName = "ADMIN_EDIT_BANK_ACCOUNTS")]
        public ActionResult BankAccounts()
        {
            SystemData bankAccounts = DBHelper.GetBankAccountsData();
            return View(ModelConverter.SystemDataToView(bankAccounts));
        }

        [RuleCheckFilter(RuleName = "ADMIN_EDIT_BANK_ACCOUNTS")]
        [HttpPost]
        public ActionResult BankAccounts(SystemDataView model)
        {
            DBHelper.UpdateSystemData(model);
            return RedirectToAction("BankAccounts");
        }

        [RuleCheckFilter(RuleName = "ADMIN_DOCUMENTS")]
        public ActionResult Documents()
        {
            var docs = UIHelper.GetAllDocuments(new int[] { }, 1, "*");
            return View(docs.Item1);
        }

        [RuleCheckFilter(RuleName = "ADMIN_ADD_DOCUMENT")]
        public ActionResult AddDocument()
        {
            List<TopicViewModel> topics = UIHelper.GetTopicTree();
            ViewBag.TopicList = topics;
            return View();
        }

        [RuleCheckFilter(RuleName = "ADMIN_ADD_DOCUMENT")]
        [HttpPost]
        public ActionResult AddDocument(DocumentView model, HttpPostedFileBase cover)
        {
            int id = DBHelper.AddDocument(model);
            cover.SaveAs(Server.MapPath("~/Content/documentcovers/" + id + ".png"));
            return RedirectToAction("Documents");
        }

        public ActionResult DeleteDocument(int id)
        {
            DBHelper.DeleteDocument(id);
            return RedirectToAction("Documents");
        }

        public ActionResult Max3D()
        {
            List<ArchModel>
            return View();
        }
    }
}