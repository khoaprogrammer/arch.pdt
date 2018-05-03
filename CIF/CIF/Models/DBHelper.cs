using CIF.DB;
using HtmlAgilityPack;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace CIF.Models
{
    class DBHelper
    {
        public static SupportTicket GetSupportTickets(int ticketId)
        {
            var ContextInstance = ApplicationDbContext.Create();
            return ContextInstance.SupportTickets.FirstOrDefault(x => x.Id == ticketId);
        }

        internal static void TimeExchange(int cif, string id)
        {
            var context = ApplicationDbContext.Create();
            var user = DBHelper.GetUser(id, context);

            user.CIF -= cif;
            user.IsAdsFree = true;
            user.TimeStorage += (cif * Globals.TimeExchangeRate) * 60;

            context.SaveChanges();

        }

        internal static void AddBlogEntryRead(int id)
        {
            var context = ApplicationDbContext.Create();
            BlogEntry entry = GetBlogEntry(id, context);
            entry.ReadCount += 1;
            context.SaveChanges();
        }
        

        public static int AddSupportTicket(SupportTicketView model)
        {
            SupportTicket add = new SupportTicket
            {
                Email = model.Email,
                Detail = model.Detail,
                Type = model.Type,
                Time = DateTime.Now
            };
            var ContextInstance = ApplicationDbContext.Create();
            ContextInstance.SupportTickets.Add(add);
            ContextInstance.SaveChanges();
            return add.Id;
        }
        public static int AddBookVisit(int id)
        {
            var ContextInstance = ApplicationDbContext.Create();
            Book b = GetBook(id, ContextInstance);
            b.VisitCount += 1;
            return ContextInstance.SaveChanges();
        }

        internal static double AdsFreeMonitor(string id)
        {
            var context = ApplicationDbContext.Create();
            var user = GetUser(id, context);
            user.TimeStorage -= 5;
            if (user.TimeStorage <= 0)
            {
                user.TimeStorage = 0;
                user.IsAdsFree = false;
            }
            context.SaveChanges();
            return user.TimeStorage;
        }

        internal static TranslatedContent GetTranslatedContent(int bookId, string pCode, int pPos)
        {
            var context = ApplicationDbContext.Create();
            TranslatedContent content = context.TranslatedContents.FirstOrDefault(x => x.BookId == bookId && x.Code == pCode && x.Location == pPos);
            return content;
        }

        internal static void AddUserCIF(string id, int gainCIF)
        {
            var context = ApplicationDbContext.Create();
            var user = GetUser(id, context);
            user.CIF += gainCIF;
            context.SaveChanges();
        }
        
        public static int GetTotalBookCount()
        {
            var context = ApplicationDbContext.Create();
            return context.Books.Count();
        }

        public static Content GetContent(int id, string code)
        {
            var ContextInstance = ApplicationDbContext.Create();
            Book b = GetBook(id, ContextInstance);
            return b.Contents.First(x => x.Code == code);
        }

        public static List<Author> GetAllAuthors()
        {
            var context = ApplicationDbContext.Create();
            List<Author> result = context.Authors.ToList();
            return result;
        }

        public static int AddAuthor(Author author)
        {
            ApplicationDbContext context = ApplicationDbContext.Create();
            context.Authors.Add(author);
            context.SaveChanges();
            return author.Id;
        }

        public static int AddBookRead(int id, ApplicationDbContext context = null)
        {
            var ContextInstance = context != null ? context : ApplicationDbContext.Create();
            Book b = GetBook(id, ContextInstance);
            b.ReadCount += 1;
            return ContextInstance.SaveChanges(); 
        }

        internal static void AddPendingTranslate(string userId, string translateContent, int bookId, string code, int location)
        {
            var context = ApplicationDbContext.Create();
            context.PendingTranslates.Add(new PendingTranslate
            {
                ApplicationUserId = userId,
                BookId = bookId,
                Code = code,
                Data = translateContent,
                Location = location
            });
            context.SaveChanges();
        }

        public static Content GetPrevContent(int bookId, string contentCode, ApplicationDbContext context = null)
        {
            var ContextInstance = context != null ? context : ApplicationDbContext.Create();
            Book b = GetBook(bookId, ContextInstance);
            Content current = b.Contents.First(x => x.Code == contentCode);

            return b.Contents.FirstOrDefault(x => x.Order == current.Order - 1);
        }

        public static Content GetNextContent(int bookId, string contentCode, ApplicationDbContext context = null)
        {
            var ContextInstance = context != null ? context : ApplicationDbContext.Create();
            Book b = GetBook(bookId, ContextInstance);
            Content current = b.Contents.First(x => x.Code == contentCode);

            return b.Contents.FirstOrDefault(x => x.Order == current.Order + 1);
        }


        public static List<Topic> GetAllTopics(ApplicationDbContext context = null)
        {
            var ContextInstance = context != null ? context : ApplicationDbContext.Create();
            return ContextInstance.Topics.ToList();
        }
        

        public static Book GetBook(int id, ApplicationDbContext context = null)
        {
            ApplicationDbContext ContextInstance = context != null ? context : ApplicationDbContext.Create();
            return ContextInstance.Books.FirstOrDefault(x => x.Id == id);
        }

        public static int AddAuthor(AuthorViewModel author)
        {
            Author addAuthor = ModelConverter.ViewToAuthor(author);
            return AddAuthor(addAuthor);
        }

        public static Author GetAuthor(int id, ApplicationDbContext context = null)
        {
            var ContextInstance = context != null ? context : ApplicationDbContext.Create();
            Author result = ContextInstance.Authors.First(x => x.Id == id);
            return result;
        }

        public static List<Book> GetLastestBooks(int number)
        {
            var context = ApplicationDbContext.Create();
            List<Book> lastest = context.Books.OrderByDescending(x => x.AddDate).Take(number).ToList();
            return lastest;
        }

        internal static bool CheckToken(string accessToken)
        {
            var context = ApplicationDbContext.Create();
            if (context.AccessTokens.Any(x => x.Token == accessToken))
            {
                AccessToken token = GetToken(accessToken, context);
                if (token.ApplicationUserId != null)
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        internal static void AssignUserToken(string id, string accessToken)
        {
            var context = ApplicationDbContext.Create();
            AccessToken token = GetToken(accessToken, context);
            token.ApplicationUserId = id;
            context.SaveChanges();
        }

        internal static Tuple<List<Document>, int> GetAllDocuments(int[] topicIds, int page, string search, ApplicationDbContext context = null)
        {
            var ContextInstance = context != null ? context : ApplicationDbContext.Create();
            IQueryable<Document> query = ContextInstance.Documents;
            if (topicIds != null)
            {
                query = query.Where(x => x.Topics.Any(y => topicIds.Contains(y.Id)));
            }

            if (search != "*")
            {
                query = query.Where(x => x.Name.ToUpper().Contains(search.ToUpper()));
            }
            int total = query.Count();
            return new Tuple<List<Document>, int>(query.OrderByDescending(x => x.AddDate).Skip((page - 1) * 20).Take(20).ToList(), total);
        }

        private static AccessToken GetToken(string accessToken, ApplicationDbContext context = null)
        {
            var ContextInstance = context != null ? context : ApplicationDbContext.Create();
            return ContextInstance.AccessTokens.FirstOrDefault(x => x.Token == accessToken);
        }

        public static Author GetAuthor(string name, ApplicationDbContext context = null)
        {
            var ContextInstance = context != null ? context : ApplicationDbContext.Create();
            return ContextInstance.Authors.FirstOrDefault(x => x.Name.ToUpper() == name.ToUpper());
        }

        public static int DeleteAuthor(Author author, ApplicationDbContext context = null)
        {
            var ContextInstance = context != null ? context : ApplicationDbContext.Create();
            ContextInstance.Authors.Remove(author);
            return ContextInstance.SaveChanges();
        }

        public static List<Publisher> GetAllPublishers(ApplicationDbContext context = null)
        {
            var ContextInstance = context != null ? context : ApplicationDbContext.Create();
            List<Publisher> publishers = ContextInstance.Publishers.ToList();
            return publishers;
        }

        public static int AddPublisher(PublisherViewModel model)
        {
            return AddPublisher(ModelConverter.ViewToPublisher(model));
        }

        public static Tuple<List<Book>, int> GetAllBooks(int[] topicIds = null, int authorId = -1, int publisherId = -1, int page = 1, string search = "*", ApplicationDbContext context = null)
        {
            var ContextInstance = context != null ? context : ApplicationDbContext.Create();
            IQueryable<Book> query = ContextInstance.Books;
            if (topicIds != null)
            {
                query = query.Where(x => x.Topics.Any(y => topicIds.Contains(y.Id)));
            }
            if (authorId != -1)
            {
                query = query.Where(x => x.Authors.Any(y => y.Id == authorId));
            }
            if (publisherId != -1)
            {
                query = query.Where(x => x.PublisherId == publisherId);
            }

            if (search != "*")
            {
                query = query.Where(x => x.Name.ToUpper().Contains(search.ToUpper()));
            }
            int total = query.Count();
            return new Tuple<List<Book>, int>(query.OrderByDescending(x => x.AddDate).Skip((page - 1) * 20).Take(20).ToList(), total);
        }

        internal static PendingTranslate GetPendingTranslate(int id, ApplicationDbContext context = null)
        {
            var ContextInstance = context != null ? context : ApplicationDbContext.Create();
            return ContextInstance.PendingTranslates.FirstOrDefault(x => x.Id == id);
        }

        internal static List<PendingTranslate> GetAllPendingTranslate()
        {
            var context = ApplicationDbContext.Create();
            return context.PendingTranslates.ToList();
        }

        public static int AddPublisher(Publisher pub, ApplicationDbContext context = null)
        {
            var ContextInstance = context != null ? context : ApplicationDbContext.Create();
            ContextInstance.Publishers.Add(pub);
            ContextInstance.SaveChanges();
            return pub.Id;
        }
        
        public static Publisher GetPublisher(int id, ApplicationDbContext context = null)
        {
            var ContextInstance = context != null ? context : ApplicationDbContext.Create();
            return ContextInstance.Publishers.First(x => x.Id == id);
        }

        public static List<AccessToken> GetAllAccessTokens()
        {
            var context = ApplicationDbContext.Create();
            return context.AccessTokens.ToList();
        }

        public static Publisher GetPublisher(string name, ApplicationDbContext context = null)
        {
            var ContextInstance = context != null ? context : ApplicationDbContext.Create();
            return ContextInstance.Publishers.FirstOrDefault(x => x.Name.ToUpper() == name.ToUpper());
        }

        public static int DeletePublisher(Publisher delete, ApplicationDbContext context = null)
        {
            var ContextInstance = context != null ? context : ApplicationDbContext.Create();
            ContextInstance.Publishers.Remove(delete);
            return ContextInstance.SaveChanges();
        }

        public static List<SupportTicket> GetAllSupportTickets(ApplicationDbContext context = null)
        {
            var ContextInstance = context != null ? context : ApplicationDbContext.Create();
            return ContextInstance.SupportTickets.ToList();
        }

        public static int AddBook(BookViewModel model, HttpPostedFileBase coverFile, List<TOCLine> toc, string css, string shortCode, string customCss, ApplicationDbContext context = null)
        {
            var ContextInstance = context != null ? context : ApplicationDbContext.Create();
            Book add = new Book
            {
                Name = model.Name,
                ISBN = model.ISBN,
                Description = model.Description,
                PublishDate = model.PublishDate,
                PublisherId = model.PublisherId,
                Contents = model.Contents,
                AddDate = DateTime.Now,
                Authors = model.AuthorIds.Select(x => ContextInstance.Authors.First(y => y.Id == x)).ToList(),
                Topics = model.TopicIds.Select(x => ContextInstance.Topics.First(y => y.Id == x)).ToList(),
                CSSURL = css,
                ShortCode = shortCode,
                CustomCSS = customCss,
                DriveCode = model.DriveCode
            };
            ContextInstance.Books.Add(add);
            ContextInstance.SaveChanges();

            if (toc != null)
            {
                foreach (var tocLine in toc)
                {
                    tocLine.BookId = add.Id;
                    tocLine.Content = add.Contents.First(x => x.Code == tocLine.Content.Code);

                    ContextInstance.TOCLines.Add(tocLine);
                    ContextInstance.SaveChanges();
                }
            }
            coverFile.SaveAs(Path.Combine(HttpContext.Current.Server.MapPath(Globals.BookCoverPath), add.Id + ".png"));

            return add.Id;
        }

        public static int GetTodayViewCount()
        {
            SessionCount todayCount = GetTodaySesionCount();
            return todayCount.Count;
        }

        public static Topic GetTopic(int id, ApplicationDbContext context = null)
        {
            var ContextInstance = context != null ? context : ApplicationDbContext.Create();
            return ContextInstance.Topics.First(x => x.Id == id);
        }

        public static void UpdateTopic(TopicViewModel topic, ApplicationDbContext context = null)
        {
            var ContextInstance = context != null ? context : ApplicationDbContext.Create();
            Topic update = GetTopic(topic.Id, ContextInstance);
            if (topic.ParentId == -1)
            {
                update.Level = 0;
                topic.ParentId = null;
            } else {
                Topic parent = GetTopic(topic.ParentId.Value, ContextInstance);
                update.Level = parent.Level + 1;
                update.ParentId = topic.ParentId;
            }
            update.Name = topic.Name;
            if (topic.ChildIds != null)
            {
                update.Childs = topic.ChildIds.Select(x => ContextInstance.Topics.First(y => y.Id == x)).ToList();
            }

            ContextInstance.SaveChanges();
        }

        public static int UpdateBook(BookViewModel model, ApplicationDbContext context = null)
        {
            var ContextInstance = context != null ? context : ApplicationDbContext.Create();
            Book update = GetBook(model.Id, ContextInstance);
            update.Authors.Clear();
            update.Topics.Clear();
            update.ISBN = model.ISBN;
            update.Authors = ContextInstance.Authors.Where(x => model.AuthorIds.ToList().Contains(x.Id)).ToList();
            update.PublisherId = model.PublisherId;
            update.Name = model.Name;
            update.PublishDate = model.PublishDate;
            update.CustomCSS = model.CSS;
            update.Topics = ContextInstance.Topics.Where(x => model.TopicIds.ToList().Contains(x.Id)).ToList();

            return ContextInstance.SaveChanges();
        }

        public static int DeleteBook(int id, ApplicationDbContext context = null)
        {
            var ContextInstance = context != null ? context : ApplicationDbContext.Create();
            Book delete = GetBook(id, ContextInstance);
            ContextInstance.Books.Remove(delete);
            return ContextInstance.SaveChanges();
        }

        public static SessionCount GetSessionCount(DateTime date, ApplicationDbContext context = null)
        {
            var ContextInstance = context != null ? context : ApplicationDbContext.Create();
            SessionCount count = ContextInstance.SessionCounts.FirstOrDefault(x => x.Date == date);
            return count;
        }

        public static SessionCount GetTodaySesionCount(ApplicationDbContext context = null)
        {
            var ContextInstance = context != null ? context : ApplicationDbContext.Create();
            SessionCount todayCount = ContextInstance.SessionCounts.FirstOrDefault(x => x.Date == DateTime.Today);
            if (todayCount == null)
            {
                todayCount = new SessionCount
                {
                    Date = DateTime.Today,
                    Count = 0
                };
                ContextInstance.SessionCounts.Add(todayCount);
                ContextInstance.SaveChanges();
            }
            return todayCount;
        }

        public static int IncreaseTodayView(ApplicationDbContext context = null)
        {
            var ContextInstance = context != null ? context : ApplicationDbContext.Create();
            SessionCount obj = GetTodaySesionCount();
            obj.Count += 1;
            return ContextInstance.SaveChanges();
        }

        public static int AddBlogEntry(BlogEntryView model, ApplicationDbContext context = null)
        {
            var ContextInstance = context != null ? context : ApplicationDbContext.Create();
            BlogEntry add = new BlogEntry
            {
                PostTime = DateTime.Now,
                Content = model.Content,
                Title = model.Title,
                PreviewImageUrl = model.PreviewImageUrl,
                ShortDescription = model.ShortDescription,
                CategoryId = model.CategoryId
            };
            ContextInstance.BlogEntries.Add(add);
            return ContextInstance.SaveChanges();
        }

        public static int AddBlogCategory(BlogCategoryView model, ApplicationDbContext context = null)
        {
            var ContextInstance = context != null ? context : ApplicationDbContext.Create();
            BlogCategory add = new BlogCategory
            {
                Name = model.Name,
                ParentId = model.ParentId
            };
            add.ParentId = model.ParentId == -1 ? null : model.ParentId;
            if (add.ParentId == null)
            {
                add.Level = 0;
            }
            else
            {
                BlogCategory parent = GetBlogCategory(add.ParentId.Value);
                add.Level = parent.Level + 1;
            }
            ContextInstance.BlogCategories.Add(add);
            return ContextInstance.SaveChanges();
        }

        public static BlogCategory GetBlogCategory(int parentId)
        {
            var ContextInstance = ApplicationDbContext.Create();
            return ContextInstance.BlogCategories.FirstOrDefault(x => x.Id == parentId);
        }

        public static List<BlogCategory> GetAllBlogCategories()
        {
            var ContextInstance = ApplicationDbContext.Create();
            return ContextInstance.BlogCategories.ToList();
        }

        public static BlogEntry GetBlogEntry(int id, ApplicationDbContext context = null)
        {
            var ContextInstance = context != null ? context : ApplicationDbContext.Create();
            return ContextInstance.BlogEntries.FirstOrDefault(x => x.Id == id);
        }

        public static List<BlogEntry> GetAllBlogEntries()
        {
            var ContextInstance = ApplicationDbContext.Create();
            return ContextInstance.BlogEntries.OrderByDescending(x => x.PostTime).ToList();
        }

        public static void AddRole(RoleView model)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            if (!roleManager.RoleExists(model.Name))
            {
                roleManager.Create(new IdentityRole
                {
                    Name = model.Name
                });
            }
        }

        public static IdentityRole GetRole(string Id)
        {
            var context = ApplicationDbContext.Create();
            return context.Roles.First(x => x.Id == Id);
        }
        
        
        public static List<IdentityRole> GetAllRoles()
        {
            var context = ApplicationDbContext.Create();
            return context.Roles.ToList();
        }
        

        public static ApplicationUser GetUser(string id, ApplicationDbContext context = null)
        {
            var ContextIntance = context != null ? context : ApplicationDbContext.Create();
            return ContextIntance.Users.FirstOrDefault(x => x.Id == id);
        }
        
        
        
        

        internal static List<Function> GetAllFunctions()
        {
            var context = ApplicationDbContext.Create();
            return context.Functions.ToList();
        }

        internal static void AddToken(string guidString)
        {
            var context = ApplicationDbContext.Create();
            context.AccessTokens.Add(new AccessToken
            {
                Token = guidString
            });
            context.SaveChanges();
        }

        internal static void UpdateUser(string id, string[] groups, string[] functions)
        {
            ApplicationDbContext context = ApplicationDbContext.Create();
            ApplicationUserManager uManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            var user = DBHelper.GetUser(id, context);
            user.Roles.Clear();
            context.SaveChanges();
            if (groups != null)
            {
                foreach (var group in groups)
                {
                    uManager.AddToRole(id, group);
                }
            }
            user.Functions.Clear();
            context.SaveChanges();
            if (functions != null)
            {
                foreach (var function in functions)
                {
                    Function f = context.Functions.First(x => x.Id == function);
                    user.Functions.Add(f);
                    context.SaveChanges();
                }
            }
        }

        internal static void ApproveTranslate(int id)
        {
            var context = ApplicationDbContext.Create();
            PendingTranslate translate = GetPendingTranslate(id, context);
            var document = new HtmlDocument();
            document.LoadHtml(translate.Data);
            var user = DBHelper.GetUser(translate.ApplicationUserId, context);
            document.DocumentNode.FirstChild.InnerHtml = document.DocumentNode.FirstChild.InnerHtml + "<span class=\"contributor\">" + user.Email + "</span>";
            context.TranslatedContents.Add(new TranslatedContent
            {
                ApplicationUserId = translate.ApplicationUserId,
                BookId = translate.BookId,
                Code = translate.Code,
                Data = translate.Data,
                Id = translate.Id,
                Location = translate.Location
            });

            if (translate.ApplicationUserId != null)
            {
                var regex = new Regex("<[^>]+>", RegexOptions.IgnoreCase);
                string raw = System.Web.HttpUtility.HtmlDecode((regex.Replace(translate.Data, ""))).Trim();

                int expGain = raw.Split(' ').Count();

                AddUserEXP(translate.ApplicationUserId, expGain);
                AddUserCIF(translate.ApplicationUserId, expGain);
            }
            
            context.PendingTranslates.Remove(translate);
            context.SaveChanges();
        }

        public static void AddUserEXP(string applicationUserId, int expGain)
        {
            var context = ApplicationDbContext.Create();
            ApplicationUser user = GetUser(applicationUserId, context);
            user.EXP += expGain;

            context.SaveChanges();
            CheckUserLevelUp(applicationUserId);
        }

        private static void CheckUserLevelUp(string applicationUserId)
        {
            double reqEXP = Globals.Level1EXP;

            var context = ApplicationDbContext.Create();
            var user = DBHelper.GetUser(applicationUserId, context);

            for (int i = 1; i < (user.Level + 1); i++)
            {
                reqEXP += 0.5 * reqEXP;
            }

            if (user.EXP >= reqEXP)
            {
                user.EXP -= reqEXP;
                user.Level += 1;
                context.SaveChanges();
            }

            reqEXP = reqEXP + (0.5 * reqEXP);

            if (user.EXP >= reqEXP)
            {
                CheckUserLevelUp(applicationUserId);
            }
        }

        internal static void DeleteBlogEntry(int id)
        {
            var context = ApplicationDbContext.Create();
            BlogEntry entry = GetBlogEntry(id, context);
            context.BlogEntries.Remove(entry);
            context.SaveChanges();
        }

        internal static void UpdateBlogCategory(BlogEntryView model)
        {
            var context = ApplicationDbContext.Create();
            BlogEntry entry = GetBlogEntry(model.Id, context);
            entry.CategoryId = model.CategoryId;
            entry.Content = model.Content;
            entry.PreviewImageUrl = model.PreviewImageUrl;
            entry.ShortDescription = model.ShortDescription;
            entry.Title = model.Title;
            context.SaveChanges();
        }

        internal static SystemData GetBankAccountsData()
        {
            var context = ApplicationDbContext.Create();
            if (!context.SystemDatas.Any(x => x.Code == "BANK_ACCOUNTS"))
            {
                context.SystemDatas.Add(new SystemData
                {
                    Code = "BANK_ACCOUNTS",
                    Data = ""
                });
                context.SaveChanges();
            }
            return context.SystemDatas.First(x => x.Code == "BANK_ACCOUNTS");
        }

        internal static void UpdateSystemData(SystemDataView model)
        {
            var context = ApplicationDbContext.Create();

            SystemData data = context.SystemDatas.First(x => x.Code == model.Code);
            data.Data = model.Data;
            context.SaveChanges();
        }

        internal static int AddDocument(DocumentView model)
        {
            var context = ApplicationDbContext.Create();
            var add = new Document
            {
                AddDate = DateTime.Now,
                DownloadCount = 0,
                DriveCode = model.DriveCode,
                Name = model.Name,
                ReadCount = 0,
                Topics = context.Topics.Where(x => model.TopicIds.Contains(x.Id)).ToList(),
                ViewCount = 0
            };
            context.Documents.Add(add);
            context.SaveChanges();
            return add.Id;
        }

        internal static void DeleteDocument(int id)
        {
            var context = ApplicationDbContext.Create();
            Document doc = GetDocument(id, context);
            context.Documents.Remove(doc);
            context.SaveChanges();
        }

        public static Document GetDocument(int id, ApplicationDbContext context = null)
        {
            var contextIns = context != null ? context : ApplicationDbContext.Create();
            return contextIns.Documents.First(x => x.Id == id);
        }

        internal static Tuple<int, int, int> GetStats()
        {
            var context = ApplicationDbContext.Create();
            return new Tuple<int, int, int>(context.Books.Count(), 0, context.Users.Count());
        }
      
        public static List<ArchModel> GetAll3DMax() {
            ApplicationDbContext db = new ApplicationDbContext();
            List<ArchModel> list = db.ArchModels.Where(x=>x.Type==ModelType.MAX3D).OrderByDescending(x => x.AddDate).ToList();
            return list;
        }
        public static List<ArchModel> GetAllSketChup()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<ArchModel> list = db.ArchModels.Where(x => x.Type == ModelType.SKETCHUP).OrderByDescending(x => x.AddDate).ToList();
            return list;
        }
        public static int Add3DMax(ArchModelView a) {
            ApplicationDbContext db = new ApplicationDbContext();
            ArchModel add = ModelConverter.ViewToArchModel(a);
            db.ArchModels.Add(add);
            db.SaveChanges();
            return add.Id;
        }
        public static void Edit3DMax(ArchModelView a)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            ArchModel edit = db.ArchModels.FirstOrDefault(x => x.Id == a.Id);
            edit.Name = a.Name;
            edit.Type = a.Type;
            edit.Id = a.Id;
            edit.DownloadLink = a.DownloadLink;
            edit.AddDate = a.AddDate;
            edit.DESC = a.DESC;
            db.SaveChanges();
        }
        public static void Delete3DMax(ArchModelView a) {
            ApplicationDbContext db = new ApplicationDbContext();
            ArchModel delete = db.ArchModels.FirstOrDefault(x => x.Id == a.Id);
            db.ArchModels.Remove(delete);
            db.SaveChanges();
        }
        public static int AddSketChup(ArchModelView a)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            ArchModel add = ModelConverter.ViewToArchModel(a);
            db.ArchModels.Add(add);
            db.SaveChanges();
            return add.Id;
        }
        public static void EditSketChup(ArchModelView a)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            ArchModel edit = db.ArchModels.FirstOrDefault(x => x.Id == a.Id);
            edit.Name = a.Name;
            edit.Type = a.Type;
            edit.Id = a.Id;
            edit.DownloadLink = a.DownloadLink;
            edit.AddDate = a.AddDate;
            edit.DESC = a.DESC;
            db.SaveChanges();
        }
        public static void DeleteSketChup(ArchModelView a)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            ArchModel delete = db.ArchModels.FirstOrDefault(x => x.Id == a.Id);
            db.ArchModels.Remove(delete);
            db.SaveChanges();
        }
        public static ArchModel getArchModel(int id) {
            ApplicationDbContext db = new ApplicationDbContext();
            return db.ArchModels.FirstOrDefault(x => x.Id == id);
        }
        public static List<ArchModel> GetAllArchModel()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return db.ArchModels.ToList();

        }

        public static List<SystemData> GetAllSystemData()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return db.SystemDatas.ToList();
        }
        public static void AddDataSystem(SystemDataView data) {
            ApplicationDbContext db = new ApplicationDbContext();
            SystemData a = ModelConverter.ViewToSystemDataModel(data);
            db.SystemDatas.Add(a);
            db.SaveChanges();
        }
        public static SystemData GetSystemData(int id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            SystemData a = db.SystemDatas.FirstOrDefault(x => x.Id == id);
            return a;
        }
        public static void EditSystemData(SystemDataView model) {
            ApplicationDbContext db = new ApplicationDbContext();
            SystemData a = db.SystemDatas.FirstOrDefault(x => x.Id == model.Id);
            a.Id = model.Id;
            a.Code = model.Code;
            a.Data = model.Data;
            db.SaveChanges();
        }
        public static void DeleteSystemData(int id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            SystemData a = db.SystemDatas.FirstOrDefault(x => x.Id == id);
            db.SystemDatas.Remove(a);
            db.SaveChanges();
        }
     
    }
}
