using CIF.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CIF.Models
{
    public class ArchModelView
    {
        public int Id { get; set; }
        [Display(Name="Tên")]
        public string Name { get; set; }
        [Display(Name = "Link dowload")]
        public string DownloadLink { get; set; }
        [Display(Name = "Ngày add")]
        public DateTime AddDate { get; set; }
        [Display(Name = "Mô tả")]
        public string DESC { get; set; }
        public ModelType Type { get; set; }
    }
}