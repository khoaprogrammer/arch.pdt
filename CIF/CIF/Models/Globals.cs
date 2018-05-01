using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIF.Models
{
    public class Globals
    {
        public static string AuthorAvatarPath = "~/Content/authoravatars/";
        public static string PublisherLogoPath = "~/Content/Pubavatars/";
        public static string BookCoverPath = "~/Content/bookcovers/";
        public static string CourseImagePath = "~/Content/courseimages/";
        public static string CourseCaptionPath = "~/Content/coursecaptions/";
        public static int NumOfLastestBookShow = 5;
        public static string ConnectionString = @"Data Source=(LocalDb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\CIF.mdf;Initial Catalog=CIF;Integrated Security=True;MultipleActiveResultSets=True";
        public static List<string> OnlineSessions = new List<string>();
        public static string GGAClientId = "334074431156-9m4h3rlqmbo22ft5j1tnjdqb74rkrrri.apps.googleusercontent.com";
        public static string GGAClientSecret = "-7ytrSZMbsICpmsqvYYoQtor";
        public static string MailServer = "mail.phongdaotao.com";
        public static int SMTPPort = 25;
        public static int POPPort = 110;
        public static int IMAPPort = 143;
        public static int NumOfLastestBlogEntryShow = 4;
        public static int CourseDaysBeforeClose = 3;
        public static int CourseLessonsPerDay = 3;
        public static int DaysBetweenSessions = 3;
        public static int Level1EXP = 200;
        public static double PLIncreaseAmount = 0.05;
        public static double TimeExchangeRate = 1;
    }
}