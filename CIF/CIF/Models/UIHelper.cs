using CIF.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIF.Models
{
    public class UIHelper
    {
        public static List<TopicViewModel> GetTopicTree()
        {
            var context = new ApplicationDbContext();
            List<TopicViewModel> topics = new List<TopicViewModel>();
            Stack<Topic> topicQ = new Stack<Topic>();
            var firstLevels = context.Topics.Where(x => x.Level == 0).ToList();
            foreach (Topic top in firstLevels)
            {
                topicQ.Push(top);
            }

            while (topicQ.Count > 0)
            {
                var current = topicQ.Pop();
                topics.Add(ModelConverter.TopicToView(current));
                foreach (var child in current.Childs)
                {
                    topicQ.Push(child);
                }
            }
            return topics;
        }

        internal static ArchModelView Get3DMax()
        {
            throw new NotImplementedException();
        }

        internal static DocumentView GetDocument(int id)
        {
            Document doc = DBHelper.GetDocument(id);
            return ModelConverter.DocumentToView(doc);
        }
        
        public static UserView GetUser(string v)
        {
            ApplicationUser user = DBHelper.GetUser(v);
            return ModelConverter.UserToView(user);
        }

        public static ContentViewModel GetPrevContent(ContentViewModel c)
        {
            Content prev = DBHelper.GetPrevContent(c.BookId, c.Code);
            return ModelConverter.ContentToView(prev);
        }

        public static ContentViewModel GetNextContent(ContentViewModel c)
        {
            Content next = DBHelper.GetNextContent(c.BookId, c.Code);
            return ModelConverter.ContentToView(next);
        }

        public static List<TopicViewModel> GetAllTopicView()
        {
            List<Topic> topics = DBHelper.GetAllTopics();
            return topics.Select(x => ModelConverter.TopicToView(x)).ToList();
        }

        public static ContentViewModel GetContentView(int id, string code)
        {
            Content c = DBHelper.GetContent(id, code);
            return ModelConverter.ContentToView(c);
        }

        internal static BookViewModel GetBook(int id)
        {
            Book b = DBHelper.GetBook(id);
            if (b == null)
            {
                return null;
            }
            return ModelConverter.BookToView(b);
        }

        public static List<PublisherViewModel> GetAllPublishersView()
        {
            List<Publisher> publishers = DBHelper.GetAllPublishers();
            List<PublisherViewModel> result = new List<PublisherViewModel>();
            foreach (var pub in publishers)
            {
                result.Add(ModelConverter.PublisherToView(pub));
            }
            return result;
        }

        public static List<AuthorViewModel> GetAllAuthorsView()
        {
            List<AuthorViewModel> authorModels = new List<AuthorViewModel>();
            List<Author> authors = DBHelper.GetAllAuthors();
            foreach (var author in authors)
            {
                authorModels.Add(ModelConverter.AuthorToView(author));
            }
            return authorModels;
        }
        

        public static Tuple<List<BookViewModel>, int> GetAllBooksView(int[] topicIds = null, int authorId = -1, int publisherId = -1, int page = 1, string search = "*")
        {
            var books = DBHelper.GetAllBooks(topicIds, authorId, publisherId, page, search);
            List<BookViewModel> result = new List<BookViewModel>();
            foreach (var book in books.Item1)
            {
                result.Add(ModelConverter.BookToView(book));
            }
            return new Tuple<List<BookViewModel>, int>(result, books.Item2);
        }
        
        //public static List<BookViewModel> SearchBooks(string pattern)
        //{
        //    return GetAllBooksView().Where(x => x.Name.ToUpper().Contains(pattern.ToUpper())).ToList();
        //}

        public static List<BookViewModel> GetLastestBooks(int number)
        {
            return DBHelper.GetLastestBooks(number).Select(x => ModelConverter.BookToView(x)).ToList();
        }

        public static TopicViewModel GetTopicView(int id)
        {
            Topic top = DBHelper.GetTopic(id);
            return ModelConverter.TopicToView(top);
        }

        internal static ApplicationUser GetBook(string v)
        {
            throw new NotImplementedException();
        }
        public static BookViewModel Get1Book(int id) {

            Book a = DBHelper.GetBook(id, null);
            return ModelConverter.BookToView(a);

        }

        internal static UserView Get1Book(string v)
        {
            throw new NotImplementedException();
        }

        public static int GetTodayViewCount()
        {
            return DBHelper.GetTodayViewCount();
        }

        public static int GetCurrentOnlineCount()
        {
            return Globals.OnlineSessions.Count;
        }

        public static List<SupportTicketView> GetAllSupportTicketsView()
        {
            List<SupportTicket> tickets = DBHelper.GetAllSupportTickets();
            return tickets.OrderByDescending(x => x.Time).Select(x => ModelConverter.SupportTicketToView(x)).ToList();
        }

        internal static Tuple<List<ArchModel>, int> GetArchModelView(string search, int page)
        {
            throw new NotImplementedException();
        }

        public static List<BlogEntryView> GetAllBlogEntries()
        {
            List<BlogEntry> entries = DBHelper.GetAllBlogEntries();
            return entries.Select(x => ModelConverter.BlogEntryToView(x)).ToList();
        }

        public static List<BlogCategoryView> GetAllBlogCategories()
        {
            List<BlogCategory> categories = DBHelper.GetAllBlogCategories();
            return categories.Select(x => ModelConverter.BlogCategoryToView(x)).ToList();
        }

        public static BlogEntryView GetBlogEntry(int id)
        {
            BlogEntry entry = DBHelper.GetBlogEntry(id);
            return ModelConverter.BlogEntryToView(entry);
        }

        public static List<BlogCategoryView> GetBlogCategoryTree()
        {
            var context = new ApplicationDbContext();
            List<BlogCategoryView> topics = new List<BlogCategoryView>();
            Stack<BlogCategory> topicQ = new Stack<BlogCategory>();
            var firstLevels = context.BlogCategories.Where(x => x.Level == 0).ToList();
            foreach (BlogCategory top in firstLevels)
            {
                topicQ.Push(top);
            }

            while (topicQ.Count > 0)
            {
                var current = topicQ.Pop();
                topics.Add(ModelConverter.BlogCategoryToView(current));
                foreach (var child in current.Childs)
                {
                    topicQ.Push(child);
                }
            }
            return topics;
        }

        public static List<BlogEntryView> GetLastestBlogEntries(int numOfLastestBlogEntryShow)
        {
            return UIHelper.GetAllBlogEntries().Take(numOfLastestBlogEntryShow).ToList();
        }

        public static List<RoleView> GetAllRoles()
        {
            var roles = DBHelper.GetAllRoles();
            return roles.Select(x => ModelConverter.RoleToView(x)).ToList();
        }

        public static List<AccessTokenView> GetAllAccessTokens()
        {
            List<AccessToken> tokens = DBHelper.GetAllAccessTokens();
            return tokens.Select(x => ModelConverter.TokenToView(x)).ToList();
        }

        internal static List<TranslateContentView> GetAllPendingTranslates()
        {
            List<PendingTranslate> result = DBHelper.GetAllPendingTranslate();
            return result.Select(x => ModelConverter.TranslateToView(x)).ToList();
        }

        internal static TranslateContentView GetPendingTranslates(int id)
        {
            PendingTranslate translate = DBHelper.GetPendingTranslate(id);
            return ModelConverter.TranslateToView(translate);
        }

        internal static Tuple<List<DocumentView>, int> GetAllDocuments(int[] topicIds, int page, string search)
        {
            var docs = DBHelper.GetAllDocuments(topicIds, page, search);
            return new Tuple<List<DocumentView>, int>(docs.Item1.Select(x => ModelConverter.DocumentToView(x)).ToList(), docs.Item2);
        }
        public static List<ArchModelView> GetAll3DMax()
        {
            var list3DMax = DBHelper.GetAll3DMax();
            return list3DMax.Select(x => ModelConverter.ArchModelToView(x)).ToList();
        }
        public static List<ArchModelView> GetAllSketChup()
        {
            var listSketChup = DBHelper.GetAllSketChup();
            return listSketChup.Select(x => ModelConverter.ArchModelToView(x)).ToList();
        }
        public static ArchModelView Get3DMax(int id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            ArchModel edit = db.ArchModels.FirstOrDefault(x => x.Id == id && x.Type == ModelType.MAX3D);
            return ModelConverter.ArchModelToView(edit);
        }
        public static ArchModelView GetSketChup(int id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            ArchModel edit = db.ArchModels.FirstOrDefault(x => x.Id == id && x.Type == ModelType.SKETCHUP);
            return ModelConverter.ArchModelToView(edit);
        }
        public static ArchModelView getArchModelView(int id)
        {
            ArchModelView a = ModelConverter.ArchModelToView(DBHelper.getArchModel(id));
            return a;
        }
        public static List<SystemDataView> GetAllSystemDataView() {
            List<SystemData> a = DBHelper.GetAllSystemData();
            return a.Select(x => ModelConverter.SystemDataToView(x)).ToList();
        }
        public static SystemDataView GetSystemDataView(int id) {
            SystemData a = DBHelper.GetSystemData(id);
            return ModelConverter.SystemDataToView(a);
        }
        public static List<ArchModelView> GetAllArchModelView() {
            List<ArchModel> a = DBHelper.GetAllArchModel();
            return a.Select(x => ModelConverter.ArchModelToView(x)).ToList();
        }
        public static Tuple<List<ArchModelView>, int> GetAll_Max3D(string search, int page = 1) {
            List<ArchModelView> list = UIHelper.GetAll3DMax();
            if (search != "*")
            {
                //list = list.Where(x => x.Id == 3).ToList();
                list = list.Where(x => x.Name.ToUpper().Contains(search.ToUpper())).ToList();
                // query = query.Where(x => x.Name.ToUpper().Contains(search.ToUpper()));
            }
            return new Tuple<List<ArchModelView>, int>(list.OrderByDescending(x => x.AddDate).Skip((page - 1) * 20).Take(20).ToList(), list.Count);
        }
        public static Tuple<List<ArchModelView>, int> GetAll_SketChup(string search, int page = 1)
        {
            List<ArchModelView> list = UIHelper.GetAllSketChup();
            if (search != "*")
            {
                //list = list.Where(x => x.Id == 3).ToList();
                list = list.Where(x => x.Name.ToUpper().Contains(search.ToUpper())).ToList();
                // query = query.Where(x => x.Name.ToUpper().Contains(search.ToUpper()));
            }
            return new Tuple<List<ArchModelView>, int>(list.OrderByDescending(x => x.AddDate).Skip((page - 1) * 20).Take(20).ToList(), list.Count);
        }
        public static List<BookOrderView> GetBookOrderIdUser(string user) {
            List<BookOrder> book = DBHelper.GetBookOrderIdUser(user);
            return book.Select(x => ModelConverter.BookOrderToView(x)).ToList();

        }
    }
}