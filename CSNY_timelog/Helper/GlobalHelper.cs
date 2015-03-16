using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace CSNY_timelog.Helper
{
    public class GlobalHelper
    {



        //for very doc setting start

        public static string AppSettingHostURL = ConfigurationManager.AppSettings["HostURL"].ToString();
        public static string AppSettingResumes = ConfigurationManager.AppSettings["Resumes"].ToString();

        //for very doc setting End

        //public static string metaKeywords = Convert.ToString(ConfigurationManager.AppSettings["metaKeywords"]);
        //public static string metaDescription = Convert.ToString(ConfigurationManager.AppSettings["metaDescription"]);

        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }

        public static string Sitename = ConfigurationManager.AppSettings["Sitename"].ToString();
        // public static int SiteID = Convert.ToInt32(ConfigurationManager.AppSettings["SiteID"].ToString());
        public static string SiteUrl = ConfigurationManager.AppSettings["SiteUrl"].ToString();
        public static string BehalfOf = ConfigurationManager.AppSettings["BehalfOf"].ToString();
        public static string Sender = ConfigurationManager.AppSettings["Sender"].ToString();
        public static string Bcc = ConfigurationManager.AppSettings["Bcc"].ToString();

        //public static string Email_Info = ConfigurationManager.AppSettings["Email_Info"].ToString();
        //public static string Email1 = ConfigurationManager.AppSettings["Email1"].ToString();
        //public static string SiteProfessionals = ConfigurationManager.AppSettings["SiteProfessionals"].ToString();
        //public static bool ExistsJobSearchAgent = Convert.ToBoolean(ConfigurationManager.AppSettings["ExistsJobSearchAgent"].ToString());
        //public static bool ExitsPaypalAccount = Convert.ToBoolean(ConfigurationManager.AppSettings["ExitsPaypalAccount"].ToString());
        //public static string ContactUsNo = ConfigurationManager.AppSettings["ContactUsNo"].ToString();

        //public static string Email_NoReply = Convert.ToString(ConfigurationManager.AppSettings["Email_NoReply"]);
        //public static string Email_Billing = Convert.ToString(ConfigurationManager.AppSettings["Email_Billing"]);
        //public static string Email_Sales = Convert.ToString(ConfigurationManager.AppSettings["Email_Sales"]);

        public string BuildPageMetaTagKeywords()
        {
            return "";
        }





    }
}