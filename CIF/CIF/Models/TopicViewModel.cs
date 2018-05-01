using CIF.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CIF.Models
{
    public class TopicViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int Level { get; set; }
        public int? ParentId { get; set; }
        public Topic Parent { get; set; }
        public List<Topic> Childs { get; set; }
        public int[] ChildIds { get; set; }

        public string DisplayName {
            get {
                if (this.Parent != null)
                {
                    return new string('-', this.Level * 4) + ' ' + this.Name;
                }
                return this.Name;
            }
            set {}
        }

        public List<TopicViewModel> BreadCrum
        {
            get
            {
                List<TopicViewModel> result = new List<TopicViewModel>();
                TopicViewModel current = this;
                while (true)
                {
                    result.Add(current);
                    if (current.Parent == null)
                    {
                        break;
                    }
                    current = ModelConverter.TopicToView(current.Parent);
                }
                result.Reverse();
                return result;
            }
        }
    }
}