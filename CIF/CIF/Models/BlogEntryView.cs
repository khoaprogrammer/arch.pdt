using CIF.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CIF.Models
{
    public class BlogEntryView
    {
        public int Id { get; set; }
        [Display(Name = "Tiêu đề")]
        public string Title { get; set; }
        [Display(Name = "Thời gian")]
        public DateTime PostTime { get; set; }
        [Display(Name = "Nội dung")]
        [AllowHtml]
        public string Content { get; set; }
        [Display(Name = "Tóm tắt")]
        [AllowHtml]
        public string ShortDescription { get; set; }
        [Display(Name = "Hình miêu tả")]
        public string PreviewImageUrl { get; set; }
        public BlogCategory Category { get; set; }
        public int CategoryId { get; set; }
        public int ReadCount { get; set; }
    }
}