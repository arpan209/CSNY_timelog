using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CSNY_timelog.Models;
using CSNY_timelog.ViewModel;
using System.Security.Principal;
using System.Web.Security;
using CSNY_timelog.Helper;
//using System.Net.Mail;

namespace CSNY_timelog.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/
        //OpBidsDBEntities1 db = new OpBidsDBEntities1();
        //CSNYEntities db = new CSNYEntities();
        CSNY_NewEntities db = new CSNY_NewEntities();
        // GET: /Account/
        //public ActionResult Index()
        //{

        //    try
        //    {
        //        Session["BidId"] = "0";
        //        Session["OrderNo"] = "0";
        //        Session["AdTypeId"] = "0";
        //        Session["DistributionId"] = "0";
        //        Session["PostId"] = "0";
        //        int loginid = 0;
        //        if (Session["UserId"] != null)
        //        {
        //            loginid = Convert.ToInt32(Session["UserId"]);
        //            //loginid = 1;
        //            var getData = (from n in db.tblRegistrations where n.ID.Equals(loginid) select n).SingleOrDefault();
        //            string fullName = string.Empty;
        //            string Email = string.Empty;
        //            if (getData != null)
        //            {
        //                ViewData["FullName"] = getData.FirstName + " " + getData.LastName;
        //                ViewData["LastLogin"] = getData.LastLoginDate;
        //                if (getData.LastLoginDate != null)
        //                {
        //                    ViewData["LastLogin"] = "Welcome Back! You last logged in on " + ViewData["LastLogin"] + ".";
        //                }
        //                else
        //                {

        //                    ViewData["LastLogin"] = "Welcome " + ViewData["FullName"] + ", This is your First Time!";
        //                }
        //                // Update Last Modify date
        //                db.User_UpdateLastLoginDateTime(Convert.ToInt32(Session["UserId"]), DateTime.Now.ToString());
        //            }

        //            int InProccesscount = 0;
        //            int InComplete = 0;
        //            int IsComplete = 0;
        //            int ActiveBid = 0;
        //            int InactiveBid = 0;

        //            // Get Bid In Details
        //            var getinfo = db.Sp_UserGetBidInformation(loginid).ToList();
        //            if (getinfo != null)
        //            {
        //                foreach (var info in getinfo)
        //                {

        //                    if (info.Status == "IsApprove" || info.Status == "Complete" || info.Status == "IsReject" || info.Status == "Process" || info.Status == "InProgress")
        //                    {
        //                        InProccesscount += 1;
        //                    }

        //                    if (info.Status == "Active" || info.Status == "Publish" || info.Status == "Completed")
        //                    {
        //                        ActiveBid += 1;
        //                    }

        //                    if (info.Status == "InComplete")
        //                    {
        //                        InComplete += 1;
        //                    }

        //                    if (info.Status == "Complete")
        //                    {
        //                        IsComplete += 1;
        //                    }

        //                    if (info.Status == "In Active")
        //                    {
        //                        InactiveBid += 1;
        //                    }

        //                }
        //            }
        //            var ResultInProcess = db.Sp_User_Get_InProcess_Bid_Proposal(loginid).ToList();
        //            ViewData["BidInProccess"] = ResultInProcess.Count;

        //            var ResultinComplete = db.Sp_Get_InComplete_Bid_Proposal(loginid).ToList();
        //            ViewData["BIdInComplete"] = ResultinComplete.Count;

        //            ViewData["BIdIsComplete"] = IsComplete;

        //            var ResultisActive = db.Sp_Get_Active_Bid_Proposal(loginid).ToList();
        //            ViewData["BIdisActice"] = ResultisActive.Count;

        //            var ResultinActive = db.Sp_Get_Inactive_Bid_Proposal(loginid).ToList();
        //            ViewData["BIdInActive"] = ResultinActive.Count;
        //        }
        //        else
        //        {
        //            return RedirectToAction("log_on", "account");
        //        }
        //    }
        //    catch
        //    {


        //    }
        //    return View();
        //}


        public ActionResult log_on(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            //if (!Request.IsSecureConnection && !Request.IsLocal)
            //{
            //    string redirectUrl = Request.Url.ToString().Replace("http:", "https:");
            //    Response.Redirect(redirectUrl);
            //}


            return View();
        }

        [HttpPost]

        public ActionResult log_on(LogOnViewModel model, string returnUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var username = string.Empty;
                    string Id = string.Empty;
                    username = model.UserName.ToLower();

                    //var password = db.SP_get_user_password(username).SingleOrDefault();
                    var password = db.SP_get_user_password(username.Trim()).SingleOrDefault();
                    var userid = db.Sp_getUserId(username.Trim()).SingleOrDefault();
                    string pwd = PwdSequrityManager.Encrypt(model.Password.ToString());

                    if (pwd == password)
                    {
                       
                           
                            string roleid = null;
                            roleid = db.SP_get_user_role(username).SingleOrDefault().Trim();
                            if (!string.IsNullOrEmpty(username))
                            {
                                FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                                var pagename = "";
                                Session["UserType"] = roleid;
                                Session["Username"] = username;
                                Session["UserID"] = userid;
                                var therapist = db.Sp_get_Therpist_Info(userid.ToString()).SingleOrDefault();
                                Session["Fullname"] = therapist.FirstName;
                                db.sp_Update_LastLogin(username);
                                if (roleid == "1")
                                {
                                    pagename = "admin";
                                }
                                else {
                                    pagename = "therapist";
                                    Session["NPI"] = (!string.IsNullOrEmpty(therapist.NPI)) ? therapist.NPI.Trim() : "";
                                }
                                if (!string.IsNullOrEmpty(returnUrl))
                                {
                                    if (Url.IsLocalUrl(returnUrl))
                                    {
                                        return Redirect(returnUrl);
                                    }
                                    else
                                    {
                                        return RedirectToAction("index", pagename);
                                    }
                                }

                                return RedirectToAction("index", pagename);
                            }
                       

                    }
                    else
                    {

                        ModelState.AddModelError("", "The User Name or Password provided is incorrect.");

                    }
                }
            }
            catch
            { }
            return View(model);
        }



        public ActionResult log_off()
        {
            try
            {
              
                FormsAuthentication.SignOut();
                Session.Remove("UserType");
                Session.Remove("Username");
                Session.Remove("UserID");
                //Session.Remove("OrderNo");
                //Session.Remove("AdTypeId");
                //Session.Remove("DistributionId");
                //Session.Remove("PostId");
                Request.RequestContext.HttpContext.Session.Clear();

                //Request.RequestContext.HttpContext.Current.Session.Clear();
                FormsAuthentication.SignOut();
                HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName);
                cookie.Expires = DateTime.Now.AddDays(-10000.0);
                Request.RequestContext.HttpContext.Response.Cookies.Add(cookie);

                return RedirectToAction("log_on", "account");
            }
            catch
            {
                return RedirectToAction("log_on", "account");
            }
        }

        public ActionResult registration()
        {
           RegistrationViewModel objviewmodel = new RegistrationViewModel();


            //fillCity();
            //Random rand = new Random();
            //rand.Next(0, 1000);
            //ViewData["No"] = rand.Next(0, 1000);
            ////
            //ViewData["TakeNo"] = GetRandomString();
            //Session["captcha"] = ViewData["TakeNo"];
            Session["statename"] = "";
            fillValues();
            return View(objviewmodel);
        }

    //    public string GetRandomString()
    //    {
    //        string[] arrStr = "A,B,C,D,1,2,3,4,5,6,7,8,9,0".Split(",".ToCharArray());
    //        string strDraw = string.Empty;
    //        Random r = new Random();
    //        for (int i = 0; i < 5; i++)
    //        {
    //            strDraw += arrStr[r.Next(0, arrStr.Length - 1)];
    //        }

    //        return strDraw;

    //    }

        public ActionResult registration_sucess()
        {
            return View();
        }

    //    // Example of showing Url name
    //    [ActionName("user-submit-bid")]
    //    public ActionResult userSubmit()
    //    {
    //        return View();
    //    }

        [HttpPost]
        public ActionResult registration(RegistrationViewModel objviewmodel)
        {
            string errormsg = "";

            //if (!captchaValid)
            //{
            //    ModelState.AddModelError("captcha", "Invalid captcha Code.");
            //    errormsg = "Invalid captcha Code.";
            //}

          
            RegistrationViewModel reg = new RegistrationViewModel();
         

          
            if (!objviewmodel.Checkterms)
            {
                ModelState.AddModelError("Checkterms", "You must accept the terms & conditions.");
                errormsg += "You must accept the terms & conditions.";
            }
            try
            {
              

              
                       
                        reg.FirstName = (!string.IsNullOrEmpty((string)(objviewmodel.FirstName))) ? (string)objviewmodel.FirstName.Trim() : "";
                        reg.LastName = (!string.IsNullOrEmpty((string)(objviewmodel.LastName))) ? (string)objviewmodel.LastName.Trim() : "";
                        reg.Address1 = (!string.IsNullOrEmpty((string)(objviewmodel.Address1))) ? (string)objviewmodel.Address1.Trim() : "";
                        reg.Address2 = (!string.IsNullOrEmpty((string)(objviewmodel.Address2))) ? (string)objviewmodel.Address2.Trim() : "";
                        reg.City = (!string.IsNullOrEmpty((string)(objviewmodel.City))) ? (string)objviewmodel.City.Trim() : "";
                        reg.StateName = (!string.IsNullOrEmpty((string)(objviewmodel.StateName))) ? (string)objviewmodel.StateName.Trim() : "";
                        reg.Email = (!string.IsNullOrEmpty((string)(objviewmodel.Email))) ? (string)objviewmodel.Email.Trim() : "";
                        reg.Phone = (!string.IsNullOrEmpty((string)(objviewmodel.Phone))) ? (string)objviewmodel.Phone.Trim() : "";
                      
                       reg.UserName = objviewmodel.UserName;
                          reg.Password = PwdSequrityManager.Encrypt(objviewmodel.Password);

                      
                       
                       
                            // Session["Id"] = intLoginId;
                         
                            // FormsAuthentication.SetAuthCookie(objL.UserName, false /* createPersistentCookie */);
                            //objLR.SendMailOnReg(str, objL.Email, objL.UserName, objL.Password);
                
                          db.AddTherapist(reg.FirstName, reg.LastName, reg.Address1, reg.Address2, reg.City,
                              reg.StateName, reg.Email, reg.Phone, "", "", reg.UserName, reg.Password);

                          var getReg = (from n in db.TherapistMasters where n.UserName.Equals(reg.UserName) select n).SingleOrDefault();

                          db.SP_AddUserRole(getReg.TID.ToString());

                            //OpBids.Helper.MailSendHelper.SendMailOnReg(str, objL.Email, objL.UserName, objL.Password);
                            
                            //OpBids.Helper.MailSendHelper.SendMailToAdminOnReg(objL.FirstName, objL.LastName, objL.CompanyName,
                            //    objL.State, objL.City, DateTime.Now.ToString(), objL.UserName, objL.JobTitle, objL.FirstName,
                            //    objL.Email, objviewmodel.Phone);

                        

                          CSNY_timelog.Helper.MailSendHelper.RegistratioAdminMail(reg.FirstName, reg.LastName, getReg.TID.ToString(), reg.StateName, reg.City, DateTime.Now.ToString(),
                              reg.UserName, "", "", reg.Email, reg.Phone);

                          CSNY_timelog.Helper.MailSendHelper.RegistratioUserMail(reg.FirstName, reg.LastName,getReg.TID.ToString(),reg.UserName, reg.Email);

                            return RedirectToAction("registration_sucess", "account");
                       
                           
                      

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex);
               
            }
            fillValues();
            return View(objviewmodel);
        }

        [HttpGet]
        public ActionResult edit_profile()
        {
            if (CheckUserLoginStatus() <= 0)
                return AccessDeniedView();

            //EditProfileViewModel objviewmodel = new EditProfileViewModel();
            RegistrationViewModel objviewmodel = new RegistrationViewModel();
            try
            {
                int loginid = 0;
                if (Session["UserId"] != null)
                {
                    string StateName = string.Empty;

                    loginid = Convert.ToInt32(Session["UserId"]);

                    var getReg = (from n in db.TherapistMasters where n.TID.Equals(loginid) select n).SingleOrDefault();
                    string fullName = string.Empty;
                    string Email = string.Empty;
                    if (getReg != null)
                    {
                        objviewmodel.FirstName = getReg.FirstName.Trim();
                        objviewmodel.LastName = getReg.LastName.Trim();
                        objviewmodel.Address1 = getReg.Address1.Trim();
                        objviewmodel.Address2 = (!string.IsNullOrEmpty((string)(getReg.address2))) ? (string)objviewmodel.Address2.Trim() : "";
                        objviewmodel.StateName = getReg.State.Trim();
                        objviewmodel.City = getReg.City.Trim();
                        objviewmodel.Email = getReg.Email.Trim();
                        objviewmodel.Phone = getReg.Phone.Trim();
                        objviewmodel.IsActive = getReg.IsActive.ToString();
                        //objviewmodel.SSN = (getReg.UserType == "ADMIN") ? "" : objviewmodel.SSN; ;
                        
                       
                    }
                }

               
            }
            catch
            {
                Response.Write("<script> alert('Error')</script>");
            }
            fillValues();
            fillCity(objviewmodel.StateName);
            return View(objviewmodel);
        }

        [HttpPost]
        public ActionResult edit_profile(RegistrationViewModel objviewmodel)
        {
            if (CheckUserLoginStatus() <= 0)
                return AccessDeniedView();

            string msgerror = string.Empty;
            try
            {
                int loginid = 0;
                if (Session["UserId"] != null)
                {
                    string StateName = string.Empty;

                    loginid = Convert.ToInt32(Session["UserId"]);
                    RegistrationViewModel reg = new RegistrationViewModel();

                    
                        RegistrationViewModel objLR = new RegistrationViewModel();
                        var getReg = (from n in db.TherapistMasters where n.TID.Equals(loginid) select n).SingleOrDefault();
                        string fullName = string.Empty;
                        string Email = string.Empty;
                        if (getReg != null)
                        {
                            objviewmodel.Email = getReg.Email.Trim();
                        }




                        reg.FirstName = objviewmodel.FirstName.Trim();
                        reg.LastName = objviewmodel.LastName.Trim();

                        reg.Address1 = objviewmodel.Address1.Trim();
                        reg.Address2 = (!string.IsNullOrEmpty((string)(objviewmodel.Address2))) ? (string)objviewmodel.Address2.Trim() : "";
                        reg.City = objviewmodel.City.Trim();
                        reg.StateName = objviewmodel.StateName.Trim();
                        reg.Email = objviewmodel.Email.ToLower().Trim();
                        reg.Phone = objviewmodel.Phone.Trim();
                       // reg.SSN = objviewmodel.SSN;
                        reg.ServiceType = objviewmodel.ServiceType;
                        reg.UserType = getReg.UserType;
                        //reg.EndService = Convert.ToDateTime(objviewmodel.EndService);
                        //reg.IsActive = getReg.IsActive.ToString();
                        db.SP_UpdateTherapistInfo(loginid.ToString(), reg.FirstName, reg.LastName, reg.Address1, reg.Address2, reg.City, reg.StateName, reg.Email, reg.Phone, reg.SSN, reg.ServiceType, reg.UserType, Convert.ToBoolean(reg.IsActive), Convert.ToDateTime(reg.EndService));
                       
                        //db.Sp_User_Update_profile(loginid, reg.CompanyName, reg.JobTitle, reg.FirstName, reg.LastName, reg.Country, reg.Address1,
                        //    reg.Address2, reg.City, reg.State, reg.ZipCode, reg.Email, reg.PhoneNo);
                        
                        // Response.Write("<script> alert('Updated Sucessfully')</script>");
                       var roleid = db.SP_get_user_role(getReg.UserName).SingleOrDefault().Trim();
                       var pagename = "";
                       if (roleid == "1")
                       {
                               return RedirectToAction("index", "Admin");

                       }
                       else
                       {
                           
                            return RedirectToAction("Index", "Therapist");
                       }


                       msgerror = pagename;
                    
                    //var getReg1 = (from n in db.tblRegistrations where n.ID.Equals(loginid) select n).SingleOrDefault();
                    //string fullName1 = string.Empty;
                    //string Email1 = string.Empty;
                    //if (getReg1 != null)
                    //{
                    //    objviewmodel.Email = getReg1.Email;
                    //    objviewmodel.City = getReg1.City;
                    //    objviewmodel.CompanyName = getReg1.CompanyName;
                    //    // objviewmodel.Company = getReg1.CompanyName;
                    //}

                }
                else
                {
                    return RedirectToAction("log_on", "account");
                }
            }
            catch (Exception ex)
            {
                msgerror = ex.Message.ToString();
            }
            fillValues();
            fillCity(objviewmodel.StateName);
            return Json(msgerror);
        }
       
        public void fillValues()
        {
            try
            {

               

                string StateName = string.Empty;
                var GetStateName = (from st in db.ZipCodes orderby st.StateName select st.StateName).ToList().Distinct();
                SelectList ShowState = new SelectList(GetStateName);
                ViewBag.State = ShowState;
                if (string.IsNullOrEmpty(Session["statename"].ToString()))
                {
                    StateName = Session["statename"].ToString();
                }
                if (StateName != null)
                {
                    var CityName = (from ct in db.ZipCodes
                                    orderby ct.CityName
                                    where (ct.StateName.Equals(StateName) && ct.CityType.Equals("D"))
                                    select ct.CityName).ToList().Distinct();
                    SelectList city = new SelectList(CityName);
                    ViewBag.City = city;
                }
                else
                {
                    var CityName = (from ct in db.ZipCodes orderby ct.CityName where ct.StateName.Equals(StateName) select ct.CityName).ToList().Distinct();
                    SelectList city = new SelectList(CityName);
                    ViewBag.City = city;
                }
            }
            catch
            {

            }
        }

        public void fillCity(string StateName)
        {
            if (StateName != null)
            {
                var CityName = (from ct in db.ZipCodes orderby ct.CityName where (ct.StateName.Equals(StateName) && ct.CityType.Equals("D")) select ct.CityName).ToList().Distinct();
                SelectList city = new SelectList(CityName);
                ViewBag.City = city;
                
            }
        }

        // Check for UserName Already Exit

        [HttpPost]
        public ActionResult CheckUserNameAvailable(string Username)
        {
            string returnValue = string.Empty;
            
            var result = (from n in db.TherapistMasters
                          where n.UserName.Equals(Username)
                          select n);

            if (result.Count() > 0)
            {
                returnValue = "1";
            }
            else
            {
                returnValue = "0";
            }
            return Json(returnValue);
        }

        // Check for Email Already Exit
        [HttpPost]
        public ActionResult CheckEmailAvailable(string Email)
        {
            string returnValue = string.Empty;
             var result = (from n in db.TherapistMasters
                            where n.Email.Equals(Email)
                            select n);

             if (result.Count() > 0)
             {
                 returnValue = "1";
             }
             else {
                 returnValue = "0";
             }
            return Json(returnValue);
        }

        // Get cities According to states.
        [HttpPost]
        public ActionResult GetCitiesShow(string StateName)
        {
            var CityName1 = (from ct in db.ZipCodes
                             where ct.StateName == StateName && ct.CityType == "D"
                             group ct by new { ct.CityName, ct.AreaCode } into g
                             select new { g.Key.CityName }).ToList().Distinct();

            IList<string> CityValue = new List<string>();
            foreach (var item in CityName1)
            {
                //CityValue.Add(item.CityName.Substring(10));
                CityValue.Add(item.CityName);

            }

            SelectList city = new SelectList(CityValue);

            ViewBag.City = city;
            Session["statename"] = StateName;
            return Json(city);
        }

    //    // Get Zipcode According to Cities.
    //    [HttpPost]
    //    public ActionResult GetZipcodeShow(string city)
    //    {
    //        var ZipcodeName = (from ct in db.ZipCodes
    //                           orderby ct.ZIPCode1
    //                           where ct.CityName.Equals(city) && ct.CityType.Equals("D")
    //                           select ct.ZIPCode1).Distinct().ToList();
    //        // SelectList city = new SelectList(CityName);
    //        // var ZipcodeName = db.Sp_Get_Zipcode_According_to_City(city).ToString().ToList();
    //        ViewBag.City = ZipcodeName;
    //        return Json(ZipcodeName);
    //    }

    //    public ActionResult forgot_password()
    //    {
    //        ForgotPasswordViewModel objviewmodel = new ForgotPasswordViewModel();

    //        return View();
    //    }

    //    [HttpPost]
    //    public ActionResult forgot_password(ForgotPasswordViewModel objviewmodel)
    //    {
    //        try
    //        {
    //            string answer = "";
    //            answer = Request.Form["rd"];
    //            if (ModelState.IsValid)
    //            {

    //                string password = GeneratePassword();
    //                string strMailId = "";
    //                string strusername = "";
    //                int intId = 0;

    //                if (answer == "UserName")
    //                {
    //                    strusername = Request.Form["reserpasswordanswer"]; //;

    //                    var result = (from n in db.tblRegistrations
    //                                  where n.UserName.Equals(strusername)
    //                                  select n);
    //                    foreach (var ans in result)
    //                    {
    //                        strusername = ans.UserName;
    //                        strMailId = ans.Email;
    //                        intId = ans.ID;
    //                    }
    //                }
    //                else
    //                {
    //                    strMailId = Request.Form["reserpasswordanswer"];
    //                    var result = (from n in db.tblRegistrations
    //                                  where n.Email.Equals(strMailId)
    //                                  select n);
    //                    foreach (var ans in result)
    //                    {
    //                        strusername = ans.UserName;
    //                        strMailId = ans.Email;
    //                        intId = ans.ID;
    //                    }
    //                }

    //                if (intId != 0)
    //                {
    //                    string NewPassword = PwdSequrityManager.Encrypt(password);
    //                    db.Sp_User_Change_Password(intId, NewPassword);
    //                    MailSendHelper.SendMailResetPassword(strusername, strMailId, intId, answer, Request.Form["reserpasswordanswer"].ToString(), password);
    //                    Response.Write("<script>alert('Check your mail.')</script>");
    //                    // ModelState.AddModelError("", "I have Sent a mail please check your mail and reset your password.");
    //                }
    //                else
    //                {

    //                    if (answer == "UserName")
    //                    {
    //                        Response.Write("<script>alert('Username does not exist.')</script>");
    //                    }

    //                    if (answer == "EmailId")
    //                    {
    //                        var atpos = strMailId.IndexOf('@');
    //                        var dotpos = strMailId.LastIndexOf('.');
    //                        if (atpos < 1 || dotpos < atpos + 2 || dotpos + 2 >= strMailId.Length)
    //                        {
    //                            Response.Write("<script>alert('Please provide a valid email address.')</script>");

    //                        }
    //                        else
    //                        {
    //                            Response.Write("<script>alert('Email id does not exist.')</script>");

    //                        }
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                if (answer == "UserName")
    //                {
    //                    Response.Write("<script>alert('Please enter the username.')</script>");
    //                    // ModelState.AddModelError("", "Please Enter the UserName.");
    //                }
    //                if (answer == "EmailId")
    //                {
    //                    Response.Write("<script>alert('Please enter the email-id.')</script>");
    //                    // ModelState.AddModelError("", "Please Enter the EmailId.");
    //                }
    //            }
    //        }
    //        catch
    //        {
    //            Response.Write("<script>alert('Error.')</script>");
    //            // ModelState.AddModelError("", "Can Not Reset Password.");
    //        }

    //        return View(objviewmodel);
    //    }

        public ActionResult reset_password()
        {
            if (CheckUserLoginStatus() <= 0)
                return AccessDeniedView();

            return View();
        }

        [HttpPost]
        public ActionResult reset_password(RegistrationViewModel objviewmodel)
        {
            if (CheckUserLoginStatus() <= 0)
                return AccessDeniedView();
            var message = "";
            try
            {
                
                    if (Session["UserId"].ToString() != "")
                    {
                        int loginid = Convert.ToInt32(Session["UserId"]);
                        string resultStr = string.Empty;

                        RegistrationViewModel objreg = new RegistrationViewModel();
                     
                        string Id = string.Empty;
                        var UName = (from n in db.TherapistMasters where n.TID.Equals(loginid) select n).ToList();
                        foreach (var item in UName)
                        {
                            objreg.UserName = item.UserName.ToLower();
                        }
                        var password = db.Sp_get_Therpist_Info(loginid.ToString()).SingleOrDefault();

                        string pwd = PwdSequrityManager.Encrypt(objviewmodel.OldPassword.ToString());

                        if (pwd == password.Password.ToString())
                        {

                            objreg.Password = objviewmodel.Password;
                            if (loginid == 0)
                            {
                                Response.Write("<script>alert('Cant reset password.')</script>");
                                //resetviewmodel.CustomErrorDescription = "Cant Reset Password..\n";
                                //  ModelState.AddModelError("", "Cant Reset Password.");
                            }
                            else
                            {
                                loginid = Convert.ToInt32(loginid);
                                pwd = PwdSequrityManager.Encrypt(objreg.Password.ToString());
                                db.SP_UpdateTherapistPassword(loginid.ToString(), pwd);
                                // ModelState.AddModelError("", "Reset Password Successfully.");
                                //resetviewmodel.CustomErrorDescription = "Reset Password Successfully.\n";
                                Response.Write("<script>alert('Reset password successfully.')</script>");
                                return RedirectToAction("reset_password_confirmation", "account");
                               
                            }
                        }
                        else
                        {
                            Response.Write("<script>alert('Incorrect Old Password.')</script>");
                        }
                    }
                    else
                    {
                        return RedirectToAction("log_on", "account");
                    }
               
            }
            catch
            {
                //resetviewmodel.CustomErrorDescription = "Cant Reset Password..\n";
                // ModelState.AddModelError("", "Cant Reset Password.");
                Response.Write("<script>alert('Cant reset password.')</script>");
            }


            return Json(message);
        }


        //public ActionResult CheckUserLoginStatus() { 
        
        
        //}
        
        
        public ActionResult reset_password_confirmation()
        {
            //Session.Remove("Id");
            return View();
        }

    //    public ActionResult change_password()
    //    {
    //        ChangePasswordModel objviewmodel = new ChangePasswordModel();
    //        return View(objviewmodel);
    //    }

    //    [HttpPost]
    //    public ActionResult change_password(ChangePasswordModel objviewmodel)
    //    {
    //        try
    //        {
    //            if (ModelState.IsValid)
    //            {
    //                int loginid = Convert.ToInt32(Session["UserId"]);
    //                string resultStr = string.Empty;
    //                tblRegistration objreg = new tblRegistration();
    //                objreg.Password = objviewmodel.ConfirmPassword;

    //                string pwd = PwdSequrityManager.Encrypt(objviewmodel.ConfirmPassword.ToString());

    //                var Password = (from ct in db.tblRegistrations where ct.ID.Equals(loginid) select ct.Password).SingleOrDefault();

    //                if (Password == pwd)
    //                {
    //                    objreg.ID = Convert.ToInt32(loginid);
    //                    db.Sp_User_Change_Password(objreg.ID, pwd);
    //                    Response.Write("<script>alert('Change password successfully.')</script>");
    //                    return RedirectToAction("chnage_password_success", "account");
    //                }
    //                else
    //                {
    //                    Response.Write("<script>alert('Current password not match.')</script>");
    //                }
    //            }
    //            else
    //            {

    //            }
    //        }
    //        catch
    //        {
    //            //resetviewmodel.CustomErrorDescription = "Cant Reset Password..\n";
    //            // ModelState.AddModelError("", "Cant Reset Password.");
    //            Response.Write("<script>alert('Can't change password.')</script>");
    //        }
    //        return View(objviewmodel);
    //    }

    //    public ActionResult change_password_success()
    //    {
    //        return View();
    //    }

    //    [HttpPost]
    //    public ActionResult CheckCaptchaValid(string CaptchaText)
    //    {
    //        int Taken = 0;
    //        if (Session["captcha"].ToString().ToLower() == CaptchaText.ToLower().Trim())
    //        {
    //            Taken = 1;
    //        }
    //        return Json(Taken);
    //    }

    //    public ActionResult te()
    //    {
    //        return View();
    //    }

    //    #region Status Codes
    //    private static string ErrorCodeToString(MembershipCreateStatus createStatus)
    //    {
    //        // See http://go.microsoft.com/fwlink/?LinkID=177550 for
    //        // a full list of status codes.
    //        switch (createStatus)
    //        {
    //            case MembershipCreateStatus.DuplicateUserName:
    //                return "User name already exists. Please enter a different user name.";

    //            case MembershipCreateStatus.DuplicateEmail:
    //                return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

    //            case MembershipCreateStatus.InvalidPassword:
    //                return "The password provided is invalid. Please enter a valid password value.";

    //            case MembershipCreateStatus.InvalidEmail:
    //                return "The e-mail address provided is invalid. Please check the value and try again.";

    //            case MembershipCreateStatus.InvalidAnswer:
    //                return "The password retrieval answer provided is invalid. Please check the value and try again.";

    //            case MembershipCreateStatus.InvalidQuestion:
    //                return "The password retrieval question provided is invalid. Please check the value and try again.";

    //            case MembershipCreateStatus.InvalidUserName:
    //                return "The user name provided is invalid. Please check the value and try again.";

    //            case MembershipCreateStatus.ProviderError:
    //                return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

    //            case MembershipCreateStatus.UserRejected:
    //                return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

    //            default:
    //                return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
    //        }
    //    }
    //    #endregion


    //    public ActionResult Register()
    //    {
    //        return View();
    //    }

    //    internal string GetPasswordRegex()
    //    {
    //        XDocument xmlDoc = XDocument.Load(Request.MapPath("~/xml/PasswordPolicy.xml"));

    //        var passwordSetting = (from p in xmlDoc.Descendants("Password")
    //                               select new PasswordSetting
    //                               {
    //                                   Duration = int.Parse(p.Element("duration").Value),
    //                                   MinLength = int.Parse(p.Element("minLength").Value),
    //                                   MaxLength = int.Parse(p.Element("maxLength").Value),
    //                                   NumsLength = int.Parse(p.Element("numsLength").Value),
    //                                   SpecialLength = int.Parse(p.Element("specialLength").Value),
    //                                   UpperLength = int.Parse(p.Element("upperLength").Value),
    //                                   SpecialChars = p.Element("specialChars").Value
    //                               }).First();

    //        StringBuilder sbPasswordRegx = new StringBuilder(string.Empty);
    //        //min and max
    //        sbPasswordRegx.Append(@"(?=^.{" + passwordSetting.MinLength + "," + passwordSetting.MaxLength + "}$)");

    //        //numbers length
    //        sbPasswordRegx.Append(@"(?=(?:.*?\d){" + passwordSetting.NumsLength + "})");

    //        //a-z characters
    //        sbPasswordRegx.Append(@"(?=.*[a-z])");

    //        //A-Z length
    //        sbPasswordRegx.Append(@"(?=(?:.*?[A-Z]){" + passwordSetting.UpperLength + "})");

    //        //special characters length
    //        sbPasswordRegx.Append(@"(?=(?:.*?[" + passwordSetting.SpecialChars + "]){" + passwordSetting.SpecialLength + "})");

    //        //(?!.*\s) - no spaces
    //        //[0-9a-zA-Z!@#$%*()_+^&] -- valid characters
    //        sbPasswordRegx.Append(@"(?!.*\s)[0-9a-zA-Z" + passwordSetting.SpecialChars + "]*$");

    //        return sbPasswordRegx.ToString();
    //    }

    //    public string GeneratePassword()
    //    {
    //        //Since I'm big on security, I wanted to generate passwords that contained numbers, letters and special
    //        //characters - and not easily hacked.

    //        //I started with creating three string variables.
    //        //This one tells you how many characters the string will contain.
    //        string PasswordLength = "8";
    //        //This one, is empty for now - but will ultimately hold the finised randomly generated password
    //        string NewPassword = "";

    //        //This one tells you which characters are allowed in this new password
    //        string allowedChars = "";
    //        allowedChars = "1,A,a,@";
    //        allowedChars += "B,3,b,#";
    //        allowedChars += "c,$,5,C";
    //        allowedChars += "%,D,d,9";

    //        //Then working with an array...

    //        char[] sep = { ',' };
    //        string[] arr = allowedChars.Split(sep);

    //        string IDString = "";
    //        string temp = "";

    //        //utilize the "random" class
    //        Random rand = new Random();

    //        //and lastly - loop through the generation process...
    //        for (int i = 0; i < Convert.ToInt32(PasswordLength); i++)
    //        {
    //            temp = arr[rand.Next(0, arr.Length)];
    //            IDString += temp;
    //            NewPassword = IDString;

    //            //For Testing purposes, I used a label on the front end to show me the generated password.
    //            //lblProduct.Text = IDString;
    //        }

    //        return NewPassword;
    //    }



        public int CheckUserLoginStatus()
        {
             int LoginUserId=0;
            if (Session["UserId"] != null)
            {
                LoginUserId = Convert.ToInt32(Session["UserId"]);
            }

            return LoginUserId;
        }

        protected ActionResult AccessDeniedView()
        {



            if (Request.IsAjaxRequest())
            {
                //Response.TrySkipIisCustomErrors = true;
                Response.StatusCode = 4;

                string RedirectUrl = Url.Content("~/account/log_on");
                return Json(new { RedirectUrl = RedirectUrl });
            }

            //return RedirectToAction("AccessDenied", "Error", new { pageUrl = this.Request.RawUrl });

            return RedirectToAction("log_on", "account", new { returnUrl = this.Request.RawUrl });
        }

    }
}
