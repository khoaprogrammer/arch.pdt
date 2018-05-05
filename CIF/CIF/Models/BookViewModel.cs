using CIF.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CIF.Models
{
    public class BookViewModel
    {
        public BookViewModel()
        {
            this.Contents = new List<Content>();
            this.TOCLines = new List<TOCLine>();
          //  this.Authors = new List<Author>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        //public int PublisherId { get; set; }
     //   public Publisher Publisher { get; set; }
      //  public List<Author> Authors { get; set; }
        public DateTime PublishDate { get; set; }
        public string ISBN { get; set; }
        public List<Topic> Topics { get; set; }
        public List<Content> Contents { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public DateTime AddDate { get; set; }
        public List<TOCLine> TOCLines { get; set; }
        public string ShortCode { get; set; }
        public string CSS { get; set; }
        public int[] TopicIds { get; set; }
        //public int[] AuthorIds { get; set; }
        public string CoverUrl { get; set; }
        public int ReadCount { get; set; }
        public int VisitCount { get; set; }
        [Display(Name = "Link")]
        public string DriveCode { get; set; }
        public double Price { get; set; }

        public List<BookOrder> BookOrders { get; set; }

        public List<List<TopicViewModel>> BreadCrums { get {
                List<List<TopicViewModel>> result = new List<List<TopicViewModel>>();
                foreach (var topic in this.Topics)
                {
                    result.Add(ModelConverter.TopicToView(topic).BreadCrum);
                }
                return result;
        } set { } }
    }
}