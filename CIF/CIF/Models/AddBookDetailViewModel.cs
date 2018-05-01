using CIF.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CIF.Models
{
    public class AddBookDetailViewModel
    {
        public AddBookDetailViewModel()
        {
            this.Topics = new List<Topic>();
        }
        public string Name { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public string ISBN { get; set; }
        public string CoverUrl { get; set; }
        public string CoverMethod { get; set; }
        public DateTime PublishDate { get; set; }
        public List<Topic> Topics { get; set; }
        public int[] TopicIds { get; set; }
        public Publisher Publisher { get; set; }
        public int PublisherId { get; set; }
        public List<Author> Authors { get; set; }
        public int[] AuthorIds { get; set; }
        public List<Content> Contents { get; set; }
    }
}