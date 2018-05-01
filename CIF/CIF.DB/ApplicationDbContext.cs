using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIF.DB
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("Arch");
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Content> Contents { get; set; }
        public DbSet<TOCLine> TOCLines { get; set; }
        public DbSet<SessionCount> SessionCounts { get; set; }
        public DbSet<SupportTicket> SupportTickets { get; set; }
        public DbSet<BlogEntry> BlogEntries { get; set; }
        public DbSet<BlogCategory> BlogCategories { get; set; }
        public DbSet<AccessToken> AccessTokens { get; set; }
        public DbSet<Function> Functions { get; set; }
        public DbSet<TranslatedContent> TranslatedContents { get; set; }
        public DbSet<PendingTranslate> PendingTranslates { get; set; }
        public DbSet<SystemData> SystemDatas { get; set; }
        public DbSet<Document> Documents { get; set; }

        public DbSet<ArchModel> ArchModels { get; set; }
    }
}
