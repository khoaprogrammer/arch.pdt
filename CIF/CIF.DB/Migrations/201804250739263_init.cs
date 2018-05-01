namespace CIF.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Arch.AccessTokens",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Token = c.String(),
                        ApplicationUserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Arch.AspNetUsers", t => t.ApplicationUserId)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "Arch.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        CIF = c.Double(nullable: false),
                        Level = c.Int(nullable: false),
                        EXP = c.Double(nullable: false),
                        TimeStorage = c.Double(nullable: false),
                        IsAdsFree = c.Boolean(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "Arch.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Arch.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "Arch.Functions",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Arch.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("Arch.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "Arch.PendingTranslates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BookId = c.Int(nullable: false),
                        Code = c.String(),
                        Location = c.Int(nullable: false),
                        Data = c.String(),
                        ApplicationUserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Arch.AspNetUsers", t => t.ApplicationUserId)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "Arch.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("Arch.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("Arch.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "Arch.TranslatedContents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BookId = c.Int(nullable: false),
                        Code = c.String(),
                        Location = c.Int(nullable: false),
                        Data = c.String(),
                        ApplicationUserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Arch.AspNetUsers", t => t.ApplicationUserId)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "Arch.ArchModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        DownloadLink = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Arch.Authors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Introduction = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Arch.Books",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        PublisherId = c.Int(nullable: false),
                        PublishDate = c.DateTime(nullable: false),
                        ISBN = c.String(),
                        Description = c.String(),
                        Cover = c.String(),
                        AddDate = c.DateTime(nullable: false),
                        CSSURL = c.String(),
                        ShortCode = c.String(),
                        CustomCSS = c.String(),
                        VisitCount = c.Int(nullable: false),
                        ReadCount = c.Int(nullable: false),
                        DriveCode = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Arch.Publishers", t => t.PublisherId, cascadeDelete: true)
                .Index(t => t.PublisherId);
            
            CreateTable(
                "Arch.Contents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        HTML = c.String(),
                        BookId = c.Int(nullable: false),
                        Order = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Arch.Books", t => t.BookId, cascadeDelete: true)
                .Index(t => t.BookId);
            
            CreateTable(
                "Arch.TOCLines",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Level = c.Int(nullable: false),
                        Order = c.Int(nullable: false),
                        ContentId = c.Int(nullable: false),
                        Anchor = c.String(),
                        BookId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Arch.Contents", t => t.ContentId, cascadeDelete: true)
                .ForeignKey("Arch.Books", t => t.BookId, cascadeDelete: false)
                .Index(t => t.ContentId)
                .Index(t => t.BookId);
            
            CreateTable(
                "Arch.Publishers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Introduction = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Arch.Topics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Level = c.Int(nullable: false),
                        ParentId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Arch.Topics", t => t.ParentId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "Arch.Documents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ViewCount = c.Int(nullable: false),
                        DownloadCount = c.Int(nullable: false),
                        ReadCount = c.Int(nullable: false),
                        DriveCode = c.String(),
                        AddDate = c.DateTime(nullable: false),
                        PageCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Arch.BlogCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ParentId = c.Int(),
                        Level = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Arch.BlogCategories", t => t.ParentId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "Arch.BlogEntries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        PostTime = c.DateTime(nullable: false),
                        Content = c.String(),
                        ShortDescription = c.String(),
                        PreviewImageUrl = c.String(),
                        CategoryId = c.Int(nullable: false),
                        ReadCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Arch.BlogCategories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "Arch.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "Arch.SessionCounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Arch.SupportTickets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        Detail = c.String(),
                        Time = c.DateTime(nullable: false),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Arch.SystemDatas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Data = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Arch.FunctionApplicationUsers",
                c => new
                    {
                        Function_Id = c.String(nullable: false, maxLength: 128),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Function_Id, t.ApplicationUser_Id })
                .ForeignKey("Arch.Functions", t => t.Function_Id, cascadeDelete: true)
                .ForeignKey("Arch.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .Index(t => t.Function_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "Arch.BookAuthors",
                c => new
                    {
                        Book_Id = c.Int(nullable: false),
                        Author_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Book_Id, t.Author_Id })
                .ForeignKey("Arch.Books", t => t.Book_Id, cascadeDelete: true)
                .ForeignKey("Arch.Authors", t => t.Author_Id, cascadeDelete: true)
                .Index(t => t.Book_Id)
                .Index(t => t.Author_Id);
            
            CreateTable(
                "Arch.TopicBooks",
                c => new
                    {
                        Topic_Id = c.Int(nullable: false),
                        Book_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Topic_Id, t.Book_Id })
                .ForeignKey("Arch.Topics", t => t.Topic_Id, cascadeDelete: true)
                .ForeignKey("Arch.Books", t => t.Book_Id, cascadeDelete: true)
                .Index(t => t.Topic_Id)
                .Index(t => t.Book_Id);
            
            CreateTable(
                "Arch.DocumentTopics",
                c => new
                    {
                        Document_Id = c.Int(nullable: false),
                        Topic_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Document_Id, t.Topic_Id })
                .ForeignKey("Arch.Documents", t => t.Document_Id, cascadeDelete: true)
                .ForeignKey("Arch.Topics", t => t.Topic_Id, cascadeDelete: true)
                .Index(t => t.Document_Id)
                .Index(t => t.Topic_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Arch.AspNetUserRoles", "RoleId", "Arch.AspNetRoles");
            DropForeignKey("Arch.BlogEntries", "CategoryId", "Arch.BlogCategories");
            DropForeignKey("Arch.BlogCategories", "ParentId", "Arch.BlogCategories");
            DropForeignKey("Arch.Topics", "ParentId", "Arch.Topics");
            DropForeignKey("Arch.DocumentTopics", "Topic_Id", "Arch.Topics");
            DropForeignKey("Arch.DocumentTopics", "Document_Id", "Arch.Documents");
            DropForeignKey("Arch.TopicBooks", "Book_Id", "Arch.Books");
            DropForeignKey("Arch.TopicBooks", "Topic_Id", "Arch.Topics");
            DropForeignKey("Arch.TOCLines", "BookId", "Arch.Books");
            DropForeignKey("Arch.Books", "PublisherId", "Arch.Publishers");
            DropForeignKey("Arch.TOCLines", "ContentId", "Arch.Contents");
            DropForeignKey("Arch.Contents", "BookId", "Arch.Books");
            DropForeignKey("Arch.BookAuthors", "Author_Id", "Arch.Authors");
            DropForeignKey("Arch.BookAuthors", "Book_Id", "Arch.Books");
            DropForeignKey("Arch.TranslatedContents", "ApplicationUserId", "Arch.AspNetUsers");
            DropForeignKey("Arch.AspNetUserRoles", "UserId", "Arch.AspNetUsers");
            DropForeignKey("Arch.PendingTranslates", "ApplicationUserId", "Arch.AspNetUsers");
            DropForeignKey("Arch.AspNetUserLogins", "UserId", "Arch.AspNetUsers");
            DropForeignKey("Arch.FunctionApplicationUsers", "ApplicationUser_Id", "Arch.AspNetUsers");
            DropForeignKey("Arch.FunctionApplicationUsers", "Function_Id", "Arch.Functions");
            DropForeignKey("Arch.AspNetUserClaims", "UserId", "Arch.AspNetUsers");
            DropForeignKey("Arch.AccessTokens", "ApplicationUserId", "Arch.AspNetUsers");
            DropIndex("Arch.DocumentTopics", new[] { "Topic_Id" });
            DropIndex("Arch.DocumentTopics", new[] { "Document_Id" });
            DropIndex("Arch.TopicBooks", new[] { "Book_Id" });
            DropIndex("Arch.TopicBooks", new[] { "Topic_Id" });
            DropIndex("Arch.BookAuthors", new[] { "Author_Id" });
            DropIndex("Arch.BookAuthors", new[] { "Book_Id" });
            DropIndex("Arch.FunctionApplicationUsers", new[] { "ApplicationUser_Id" });
            DropIndex("Arch.FunctionApplicationUsers", new[] { "Function_Id" });
            DropIndex("Arch.AspNetRoles", "RoleNameIndex");
            DropIndex("Arch.BlogEntries", new[] { "CategoryId" });
            DropIndex("Arch.BlogCategories", new[] { "ParentId" });
            DropIndex("Arch.Topics", new[] { "ParentId" });
            DropIndex("Arch.TOCLines", new[] { "BookId" });
            DropIndex("Arch.TOCLines", new[] { "ContentId" });
            DropIndex("Arch.Contents", new[] { "BookId" });
            DropIndex("Arch.Books", new[] { "PublisherId" });
            DropIndex("Arch.TranslatedContents", new[] { "ApplicationUserId" });
            DropIndex("Arch.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("Arch.AspNetUserRoles", new[] { "UserId" });
            DropIndex("Arch.PendingTranslates", new[] { "ApplicationUserId" });
            DropIndex("Arch.AspNetUserLogins", new[] { "UserId" });
            DropIndex("Arch.AspNetUserClaims", new[] { "UserId" });
            DropIndex("Arch.AspNetUsers", "UserNameIndex");
            DropIndex("Arch.AccessTokens", new[] { "ApplicationUserId" });
            DropTable("Arch.DocumentTopics");
            DropTable("Arch.TopicBooks");
            DropTable("Arch.BookAuthors");
            DropTable("Arch.FunctionApplicationUsers");
            DropTable("Arch.SystemDatas");
            DropTable("Arch.SupportTickets");
            DropTable("Arch.SessionCounts");
            DropTable("Arch.AspNetRoles");
            DropTable("Arch.BlogEntries");
            DropTable("Arch.BlogCategories");
            DropTable("Arch.Documents");
            DropTable("Arch.Topics");
            DropTable("Arch.Publishers");
            DropTable("Arch.TOCLines");
            DropTable("Arch.Contents");
            DropTable("Arch.Books");
            DropTable("Arch.Authors");
            DropTable("Arch.ArchModels");
            DropTable("Arch.TranslatedContents");
            DropTable("Arch.AspNetUserRoles");
            DropTable("Arch.PendingTranslates");
            DropTable("Arch.AspNetUserLogins");
            DropTable("Arch.Functions");
            DropTable("Arch.AspNetUserClaims");
            DropTable("Arch.AspNetUsers");
            DropTable("Arch.AccessTokens");
        }
    }
}
