using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.Configuration;
using System.IO;
using System.Net.Mime;



namespace CSNY_timelog.Helper
{
    public class MailSendHelper
    {
        static MailSendHelper()
        {
            //
            // TODO: Add constructor logic here
            //
        }

       // a2*** Email-2  New Registration Notice - Start***


        public static void RegistratioAdminMail(string firstname, string LastName, string TID, string State, string city, string RegDate,
                string Username, string Title, string ContactName, string ContactEmail, string ContactPhone)
        {
            try
            {
                MailMessage mail = new MailMessage();
                if (Convert.ToBoolean(ConfigurationManager.AppSettings["developement"]))
                {
                    mail.To.Add(new MailAddress("eloraw@citysoundsny.com"));
                    mail.Bcc.Add(new MailAddress(ConfigurationManager.AppSettings["Bcc"].ToString()));
                    mail.CC.Add(new MailAddress(ConfigurationManager.AppSettings["Cc"].ToString()));
                }
                else
                {
                    mail.To.Add("csny.csg@gmail.com");
                }
              
                mail.ReplyToList.Add(new MailAddress(ConfigurationManager.AppSettings["ReplyTo"].ToString(), "reply-to"));
                mail.From = new MailAddress(ConfigurationManager.AppSettings["MailFrom"].ToString(), ConfigurationManager.AppSettings["BehalfOf"].ToString());
                mail.Subject = "CSNY New Registration Notice";

                string Body = "<br/> A New user registered on CSNY:" + RegDate +

                    "<br/><br/> <red>Action: On the user profile page, add <b>SSN</b> and assign the proper <b>User Type</b> and <b>Assigned Services</b></red>" +
                    " <br/><br/><b>Username:  </b><a href='" + ConfigurationManager.AppSettings["SiteUrl"].ToString() + "/admin/EditTherapist/" + TID +
                    "'>" + Username + "</a><br/><b> First Name:</b> " + firstname + " <br/>" + "<b>Last Name:</b> " + LastName + 
                     "</Span>" + " <br/><b>Contact Email:</b> " + ContactEmail + "<br/> <b>Contact Phone:</b> " + ContactPhone +


                     "<br/><br/><br/>" +
                    "<span style=color:gray;>(Please note: You received this email because you are an active registered user on the City Sound of NY portal." +
                    " If you are not the intended recipient, please notify us immediately by return e-mail (including the original message in your reply) and then delete and discard all copies of the e-mail.)";

                mail.Body = Body;

                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["mailserver"].ToString(),
                        Convert.ToInt32(ConfigurationManager.AppSettings["port"]));
               
                    smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["username"].ToString(),
                        ConfigurationManager.AppSettings["password"].ToString());
               
                smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
               //// ILogger _logger = new DefaultLogger();
               // _logger.Error(ex.Message, ex);// return ex.Message.ToString();
            }
        }


        // c1*** Email-1 Therapist Registration Notice - Start***


        public static void RegistratioUserMail(string firstname, string LastName, string TID, string Username, string ContactEmail)
        {
            try
            {
                MailMessage mail = new MailMessage();

                if (Convert.ToBoolean(ConfigurationManager.AppSettings["developement"]))
                {
                    mail.To.Add(ContactEmail.ToString());
                    mail.Bcc.Add(new MailAddress(ConfigurationManager.AppSettings["Bcc"].ToString()));
                }
                else
                {
                    mail.To.Add("csny.csg@gmail.com");
                }

              
                mail.ReplyToList.Add(new MailAddress(ConfigurationManager.AppSettings["ReplyTo"].ToString(), "reply-to"));
                mail.From = new MailAddress(ConfigurationManager.AppSettings["MailFrom"].ToString(), ConfigurationManager.AppSettings["BehalfOf"].ToString());
                mail.Subject = "CSNY - Registration Confirmation";

                string Body = "<br/> Hi " + firstname + " "+ LastName + "," +

                    "<br/><br/> Welcome to the City Sound of NY portal!" +
                    " <br/><br/><b>Username:  </b><a href='" + ConfigurationManager.AppSettings["SiteUrl"].ToString() + "/Therapist/"+
                    "'>" + Username + "</a><br/> Your password has been encrypted and not viewable."+
                    " <br/><br/> Click this link to <a href='http://net.classifiedsolutionsgroup.com/csny-test/account/log_on'> Login </a>" +
                    "<br/><br/> (Please note: After you register, an administrator needs to assign your account the proper user rights, so there may be a delay in seeing your caseload.) " +

                    " <br/><br/>Instructions for the portal are located in this: <a href='https://docs.google.com/document/d/1komXZIr8UFnTQ8Hx5H869D28Xb0loxjbPozpQomi6_A/edit'>  Google Document</a>" +

                    " <br/><br/>If you cannot access this google doc, please contact a CSNY admin and request access rights (i.e. share the link with you)." +

                    "<br/><br/><br/>" +
                     " Please contact us with any questions.<br/><br/>"
                              + " Regards,<br/>"
                              + " The CSNY Team <br/>"
                              + " 212-604-9360 <br/>"
                              + " <a href='" + ConfigurationManager.AppSettings["SiteUrl"].ToString() + "'>" + ConfigurationManager.AppSettings["SiteUrl"].ToString() + " </a>" +
                    "<br/><span style=color:gray;>(Please note: You received this email because you are an active registered user on the City Sound of NY portal." +
                    " This email notification was sent to " + ContactEmail.ToString() + ".)";

                mail.Body = Body;

                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["mailserver"].ToString(),
                        Convert.ToInt32(ConfigurationManager.AppSettings["port"]));
              
                    smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["username"].ToString(),
                        ConfigurationManager.AppSettings["password"].ToString());
               
                smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                //// ILogger _logger = new DefaultLogger();
                // _logger.Error(ex.Message, ex);// return ex.Message.ToString();
            }
        }


      // c2*** Email-2 Therapist Session Lock Notice - Start***


        public static void LockEmail(string TID ,string firstname, string LastName,string Email,string AdminList)
        {
            try
            {
                MailMessage mail = new MailMessage();

                mail.To.Add(Email.ToString());
                mail.Bcc.Add(new MailAddress(ConfigurationManager.AppSettings["Bcc"].ToString()));
                mail.CC.Add(new MailAddress(AdminList));
                mail.ReplyToList.Add(new MailAddress(ConfigurationManager.AppSettings["ReplyTo"].ToString(), "reply-to"));
                mail.From = new MailAddress(ConfigurationManager.AppSettings["MailFrom"].ToString(), ConfigurationManager.AppSettings["BehalfOf"].ToString());
                mail.Subject = "CSNY - 48-Hour Session Lock Notification";

                string Body = "<br/> Hi " + firstname + " " + LastName + "," +

                    "<br/><br/> In 48 hours, your sessions will LOCK for the previous pay period.  You will be <b><u>unable</u></b> to add or edit sessions after the sessions are locked." +
                    " <br/><br/><b><span style=color:red;> Please record all your sessions for the previous pay period before the deadline.</b> </span>" +

                    " <br/><br/> For the <b>1st Half of the month</b>, sessions lock at <b>11:59pm on the 20th of the same month</b>." +
                    " <br/><br/> For the <b>2nd Half of the month</b>, sessions lock at <b>11:59pm on the 5th of the following month</b>." +

                    "<br/><br/><br/>" +
                     " Please contact us with any questions.<br/><br/>"
                              + " Regards,<br/>"
                              + " The CSNY Team <br/>"
                              + " 212-604-9360 <br/>"
                              + " <a href='" + ConfigurationManager.AppSettings["SiteUrl"].ToString() + "'>" + ConfigurationManager.AppSettings["SiteUrl"].ToString() + " </a>" +
                    "<br/><span style=color:gray;>(Please note: You received this email because you have registered on the City Sounds of NY portal. If you are not the intended recipient, please notify us immediately by return e-mail (including the original message in your reply)" +
                     " and then delete and discard all copies of the e-mail. ref: c2)";

                mail.Body = Body;

                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["mailserver"].ToString(),
                        Convert.ToInt32(ConfigurationManager.AppSettings["port"]));
                if (Convert.ToBoolean(ConfigurationManager.AppSettings["developement"]))
                {
                    smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["username"].ToString(),
                        ConfigurationManager.AppSettings["password"].ToString());
                }
                smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                //// ILogger _logger = new DefaultLogger();
                // _logger.Error(ex.Message, ex);// return ex.Message.ToString();
            }
        }


        public static void NotifyUnlockEmail(string TID, string firstname, string LastName, string Email, string AdminList)
        {
            try
            {
                MailMessage mail = new MailMessage();

                if (Convert.ToBoolean(ConfigurationManager.AppSettings["developement"]))
                {
                    mail.To.Add(Email.ToString());
                    mail.Bcc.Add(new MailAddress(ConfigurationManager.AppSettings["Bcc"].ToString()));
                    if (!string.IsNullOrEmpty(AdminList))
                    {
                        mail.CC.Add(AdminList);
                    }
                }
                else {
                    mail.To.Add("csny.csg@gmail.com");
                }

                
                mail.ReplyToList.Add(new MailAddress(ConfigurationManager.AppSettings["ReplyTo"].ToString(), "reply-to"));
                mail.From = new MailAddress(ConfigurationManager.AppSettings["MailFrom"].ToString(), ConfigurationManager.AppSettings["BehalfOf"].ToString());
                mail.Subject = "CSNY - Sessions Unlocked for 72 hours";

                string Body = "<br/> Hi " + firstname + " " + LastName.Trim() + "," +

                    "<br/><br/> A CSNY administrator has unlocked your previous sessions for 72 hours." +
                "<br/><br/><span style=color:red;><b> Please make appropriate changes before the unlock period ends.</b></span>" +


                    " <br/><br/> All changes to previously locked sessions are recorded.</b>" +
                   
                    "<br/><br/><br/>" +
                     " Please contact us with any questions.<br/><br/>"
                              + " Regards,<br/>"
                              + " The CSNY Team <br/>"
                              + " 212-604-9360 <br/>"
                              + " <a href='" + ConfigurationManager.AppSettings["SiteUrl"].ToString() + "'>" + ConfigurationManager.AppSettings["SiteUrl"].ToString() + " </a>" +
                    "<br/><br/><br/><span style=color:gray;>(Please note: You received this email because you have registered on the City Sounds of NY portal. If you are not the intended recipient, please notify us immediately by return e-mail (including the original message in your reply)" +
                     " and then delete and discard all copies of the e-mail. ref: c3)";

                mail.Body = Body;

                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["mailserver"].ToString(),
                        Convert.ToInt32(ConfigurationManager.AppSettings["port"]));
                
                    smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["username"].ToString(),
                        ConfigurationManager.AppSettings["password"].ToString());
                
                smtp.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                //// ILogger _logger = new DefaultLogger();
                // _logger.Error(ex.Message, ex);// return ex.Message.ToString();
            }
        }

    }
}
