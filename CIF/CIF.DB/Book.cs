using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIF.DB
{
    public class Book
    {

        public Book()
        {
            this.Contents = new List<Content>();
            this.TOCLines = new List<TOCLine>();
            this.Topics = new List<Topic>();
           // this.Authors = new List<Author>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
      //  public int PublisherId { get; set; }
     //   public virtual Publisher Publisher { get; set; }
     //   public virtual ICollection<Author> Authors { get; set; }
        public DateTime PublishDate { get; set; }
        public string ISBN { get; set; }
        public virtual ICollection<Topic> Topics { get; set; }
        public virtual ICollection<Content> Contents { get; set; }
        public string Description { get; set; }
        public string Cover { get; set; }
        public DateTime AddDate { get; set; }
        [InverseProperty("Book")]
        public virtual ICollection<TOCLine> TOCLines { get; set; }
        public string CSSURL { get; set; }
        public string ShortCode { get; set; }
        public string CustomCSS { get; set; }
        public int VisitCount { get; set; }
        public int ReadCount { get; set; }
        public string DriveCode { get; set; }
        public double Price { get; set; }
        public virtual List<BookOrder> BookOrders { get; set; }
    }
}
