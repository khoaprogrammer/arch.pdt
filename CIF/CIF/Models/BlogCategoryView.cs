using CIF.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIF.Models
{
    public class BlogCategoryView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<BlogCategory> Childs { get; set; }
        public int? ParentId { get; set; }
        public BlogCategory Parent { get; set; }
        public List<BlogEntry> Entries { get; set; }
        public int Level { get; set; }

        public string DisplayName
        {
            get
            {
                if (this.Parent != null)
                {
                    return new string('-', this.Level * 4) + ' ' + this.Name;
                }
                return this.Name;
            }
            set { }
        }
    }
}