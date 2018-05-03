using CIF.DB;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIF.Models
{
    public class ModelConverter
    {
        public static TopicViewModel TopicToView(Topic top)
        {
            return new TopicViewModel
            {
                Id = top.Id,
                Level = top.Level,
                Name = top.Name,
                Parent = top.Parent,
                Childs = top.Childs.ToList(),
                ChildIds = top.Childs.Select(x => x.Id).ToArray(),
                ParentId = top.ParentId            
            };
        }

        public static Topic ViewToTopic(TopicViewModel top)
        {
            return new Topic
            {
                Name = top.Name,
                ParentId = top.ParentId,
                Level = top.Level,
                Childs = top.Childs,
                Id = top.Id,
                Parent = top.Parent
            };
        }

        public static AuthorViewModel AuthorToView(Author au)
        {
            return new AuthorViewModel
            {
                Id = au.Id,
                Introduction = au.Introduction,
                Name = au.Name
            };
        }
        

        public static ContentViewModel ContentToView(Content c)
        {
            if (c == null)
            {
                return null;
            }
            return new ContentViewModel
            {
                Book = c.Book,
                BookId = c.BookId,
                Code = c.Code,
                HTML = c.HTML,
                Id = c.Id,
                TOCLines = c.TOCLines.ToList(),
                Order = c.Order
            };
        }

        public static Author ViewToAuthor(AuthorViewModel au)
        {
            return new Author
            {
                Id = au.Id,
                Name = au.Name,
                Books = au.Books,
                Introduction = au.Introduction
            };
        }

        public static PublisherViewModel PublisherToView(Publisher pub)
        {
            return new PublisherViewModel
            {
                Id = pub.Id,
                Introduction = pub.Introduction,
                Name = pub.Name
            };
        }

        public static Publisher ViewToPublisher(PublisherViewModel pub)
        {
            return new Publisher
            {
                Id = pub.Id,
                Name = pub.Name,
                Introduction = pub.Introduction
            };
        }

        public static BookViewModel BookToView(Book b)
        {
            return new BookViewModel
            {
                Authors = b.Authors.ToList(),
                Contents = b.Contents.ToList(),
                Description = b.Description,
                Id = b.Id,
                ISBN = b.ISBN,
                Name = b.Name,
                PublishDate = b.PublishDate,
                Publisher = b.Publisher,
                PublisherId = b.PublisherId,
                Topics = b.Topics.ToList(),
                AddDate = b.AddDate,
                TOCLines = b.TOCLines.ToList(),
                ShortCode = b.ShortCode,
                TopicIds = b.Topics.Select(x => x.Id).ToArray(),
                AuthorIds = b.Authors.Select(x => x.Id).ToArray(),
                ReadCount = b.ReadCount,
                VisitCount = b.VisitCount,
                DriveCode = b.DriveCode,
                CSS = b.CustomCSS
            };
        }

        public static SupportTicketView SupportTicketToView(SupportTicket x)
        {
            return new SupportTicketView
            {
                Detail = x.Detail,
                Email = x.Email,
                Id = x.Id,
                Time = x.Time,
                Type = x.Type,
                Messages = MailHelper.GetMessage(x.Id)
            };
        }

        public static BlogEntryView BlogEntryToView(BlogEntry entry)
        {
            return new BlogEntryView
            {
                Category = entry.Category,
                Content = entry.Content,
                Id = entry.Id,
                PostTime = entry.PostTime,
                PreviewImageUrl = entry.PreviewImageUrl,
                ShortDescription = entry.ShortDescription,
                Title = entry.Title,
                ReadCount = entry.ReadCount
            };
        }

        public static BlogCategoryView BlogCategoryToView(BlogCategory x)
        {
            return new BlogCategoryView
            {
                Id = x.Id,
                Childs = x.Childs,
                Entries = x.Entries,
                Level = x.Level,
                Name = x.Name,
                Parent = x.Parent,
                ParentId = x.ParentId
            };
        }

        public static RoleView RoleToView(IdentityRole role)
        {
            return new RoleView
            {
                Id = role.Id,
                Name = role.Name
            };
        }

        public static UserView UserToView(ApplicationUser user)
        {
            UserView userView = new UserView
            {
                Id = user.Id,
                Email = user.Email,
                PaswordHash = user.PasswordHash,
                Roles = user.Roles.ToList(),
                Functions = user.Functions.ToList(),
                CIF = user.CIF,
                EXP = user.EXP,
                Level = user.Level,
                IsAdsFree = user.IsAdsFree,
                TimeStorage = user.TimeStorage
            };
            foreach (var role in user.Roles)
            {
                IdentityRole r = DBHelper.GetRole(role.RoleId);
                userView.RoleIds.Add(r.Name);
            }
            return userView;
        }

        public static AccessTokenView TokenToView(AccessToken token)
        {
            return new AccessTokenView
            {
                Id = token.Id,
                ApplicationUser = token.ApplicationUser,
                ApplicationUserId = token.ApplicationUserId,
                Token = token.Token
            };
        }

        public static TranslateContentView TranslateToView(PendingTranslate translate)
        {
            return new TranslateContentView
            {
                BookId = translate.BookId,
                Code = translate.Code,
                Data = translate.Data,
                Id = translate.Id,
                Location = translate.Location,
                UserId = translate.ApplicationUserId,
            };
        }

        public static SystemDataView SystemDataToView(SystemData data)
        {
            return new SystemDataView
            {
                Id = data.Id,
                Code = data.Code,
                Data = data.Data
            };
        }

        public static DocumentView DocumentToView(Document doc)
        {
            return new DocumentView
            {
                AddDate = doc.AddDate,
                DownloadCount = doc.DownloadCount,
                DriveCode = doc.DriveCode,
                Id = doc.Id,
                Name = doc.Name,
                ReadCount = doc.ReadCount,
                Topics = doc.Topics.ToList(),
                ViewCount = doc.ViewCount,
                PageCount = doc.PageCount
            };
        }
        public static ArchModelView ArchModelToView(ArchModel a)
        {
            return new ArchModelView
            {
                Id = a.Id,
                Name = a.Name,
                DownloadLink = a.DownloadLink,
                Type = a.Type,
                AddDate = a.AddDate,
                DESC = a.DESC
            };
        }
        public static ArchModel ViewToArchModel(ArchModelView a)
        {
            return new ArchModel
            {
                Id = a.Id,
                Name = a.Name,
                DownloadLink = a.DownloadLink,
                Type = a.Type,
                AddDate = a.AddDate,
                DESC = a.DESC
            };
        }
        public static SystemData ViewToSystemDataModel(SystemDataView a)
        {
            return new SystemData
            {
                Id = a.Id,
                Data = a.Data,
                Code = a.Code

            };

        }

        internal static List<ArchModelView> ArchModelToView(Func<List<ArchModel>> getAllArchModel)
        {
            throw new NotImplementedException();
        }
    }
}