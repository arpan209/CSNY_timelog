using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections.Specialized;
using CSNY_timelog.Models;
using CSNY_timelog.ViewModel;
using System.Globalization;
using System.Text.RegularExpressions;

using Telerik.Web;
using Telerik.Web.Mvc;


namespace CSNY_timelog.Controllers
{
    public class SessionController : Controller

    {

        //CSNYEntities db = new CSNYEntities();
        CSNY_NewEntities db = new CSNY_NewEntities();
        //
        // GET: /Session/

        public ActionResult Index()
        {
            if (CheckUserLoginStatus() <= 0)
                return AccessDeniedView();

            return View();
        }

        [ActionName("Add-Session")]
        public ActionResult AddSession(int id)
        {
            if (CheckUserLoginStatus() <= 0)
                return AccessDeniedView();
            //var StudName = (from ct in db.StudentMasters
            //                orderby ct.SchoolName
            //                select (ct.StudentFirstName + ct.StudentLastNam));
            //SelectList StudentName = new SelectList(StudName);
            //ViewBag.StudentName = StudentName;


            //var StudID = (from ct in db.StudentMasters
            //                orderby ct.SchoolName
            //                select (ct.SID));
            //SelectList StudentID = new SelectList(StudID);
            //ViewBag.StudentID = StudentID;
            int TID = Convert.ToInt32(Session["UserID"].ToString());
            var NPI = db.Sp_getTherpist_NPI(TID.ToString()).SingleOrDefault();
               AddSessionViewModel StudentViewModel = new AddSessionViewModel();
            if (!string.IsNullOrEmpty(NPI))
            {
                // Generate Student List
                Session["NPI"] = NPI;
                var ServiceType = string.Empty;
                var value = (from n in db.TherapistMasters where n.TID.Equals(TID) select n).SingleOrDefault();
                if (value != null)
                {
                    StudentViewModel.ServiceList = value.ServiceType;

                    var LockValue = "true";
                    if (value.Lock.Substring(0, 1) == "1") { LockValue = "True"; }
                    else { LockValue = "False"; }

                    StudentViewModel.Lock = Convert.ToBoolean(LockValue);


                }

                //AddSessionViewModel StudentViewModel = new AddSessionViewModel()
                //{
                //    Fellow = FellowSelectListItems,
                //    Students = listSelectListItems

                //};
                StudentViewModel.IsEdit = false;
                StudentViewModel.SessionId = id.ToString();
                StudentViewModel.ServiceType = "";
                if (id != 0)
                {
                    //   int SessionID = Convert.ToInt32(Request.QueryString["id"].ToString());
                    var result1 = db.Sp_GetSessionByID(id).ToList();
                    foreach (var item in result1)
                    // if (result1 != null)
                    {
                        var SID = item.SID;
                        DateTime sessionDate = DateTime.Parse(item.Date.ToString());
                        StudentViewModel.SessionDate = sessionDate.ToShortDateString();
                        StudentViewModel.Location = item.Location.Trim();

                        StudentViewModel.ServiceType = item.ServiceType.Trim();

                        var StudResult = db.Sp_GetStudentDetail(SID).SingleOrDefault();
                        if (StudResult != null)
                        {
                            StudentViewModel.SID += StudResult.SID.ToString() + ",";
                            StudentViewModel.StudentListText = StudResult.StudentFirstName.Trim() + " " + StudResult.StudentLastName.Trim();
                        }
                        else
                        {
                            StudentViewModel.SID = "";
                        }
                        CultureInfo ci = CultureInfo.InvariantCulture;
                        var Start = item.StartTime.ToString();
                        DateTime StartTime = Convert.ToDateTime(Start); // Date part is 01-01-0001
                        var formattedStartTime = StartTime.ToString("h:mm tt", ci);
                        StudentViewModel.StartAMPM = formattedStartTime;
                        var End = item.EndTime.ToString();
                        DateTime EndTime = Convert.ToDateTime(End); // Date part is 01-01-0001
                        var formattedEndTime = EndTime.ToString("h:mm tt", ci);
                        StudentViewModel.EndAMPM = formattedEndTime;

                       // StudentViewModel.StartTime = formattedStartTime.Substring(0, formattedStartTime.Length - 3);
                        StudentViewModel.StartTime = formattedStartTime;
                        StudentViewModel.LastModified = item.LastModified.ToString();
                        StudentViewModel.GroupType = (!string.IsNullOrEmpty((string)(item.GroupType))) ? (string)item.GroupType : null;
                       // StudentViewModel.EndTime = formattedEndTime.Substring(0, formattedEndTime.Length - 3);
                         StudentViewModel.EndTime = formattedEndTime;

                        TempData["formattedStartTime"] = formattedStartTime;
                        TempData["formattedEndTime"] = formattedEndTime;

                        DateTime date = DateTime.Parse("1/1/2014");
                        var Langresutl = db.sp_get_MandateInfo(SID, NPI, date).FirstOrDefault();
                        if (Langresutl != null)
                        {
                            StudentViewModel.Language = Langresutl.Language.Trim();
                        }
                        StudentViewModel.IsEdit = true;
                    }
                    StudentViewModel.SID = StudentViewModel.SID.Replace(",", " ");
                }



                List<SelectListItem> listSelectListItems = new List<SelectListItem>();

                var result = db.Sp_GetStudentList(NPI.Trim()).ToList();
                // if (result != null) { 
                int StudId = 0;
                foreach (var item in result)
                {
                    if (StudentViewModel.ServiceType.Trim() == "Child")
                    {
                        bool ab = Regex.IsMatch(StudentViewModel.SID, "(^|\\s)" + item.SID.ToString() + "(\\s|$)");
                        //bool ab = StudentViewModel.SID.Contains(item.SID.ToString());
                        if (ab == true) { StudId = item.SID; }
                    }
                    SelectListItem selectList = new SelectListItem()
                    {

                        Selected = (item.SID == StudId),
                        Text = item.StudentFirstName + " " + item.StudentLastName,
                        Value = item.SID.ToString()

                    };
                    listSelectListItems.Add(selectList);



                }

                //  }
                // Generate Fellow List

                List<SelectListItem> FellowSelectListItems = new List<SelectListItem>();
                int TherID = 0;
                foreach (TherapistMaster Fellow in db.TherapistMasters)
                {
                    if (Fellow.UserType == "CFY")
                    {
                        if (StudentViewModel.ServiceType.Trim() == "CS")
                        {
                            bool ab = Regex.IsMatch(StudentViewModel.SID, "(^|\\s)" + Fellow.TID.ToString() + "(\\s|$)");
                            //bool ab = StudentViewModel.SID.Contains(Fellow.TID.ToString());
                            if (ab == true) { TherID = Fellow.TID; }
                        }

                        SelectListItem selectList1 = new SelectListItem()
                        {
                            Selected = (Fellow.TID == TherID),
                            Text = Fellow.FirstName + " " + Fellow.LastName,
                            Value = Fellow.TID.ToString()

                        };
                        FellowSelectListItems.Add(selectList1);
                    }
                }

                StudentViewModel.Fellow = FellowSelectListItems;
                StudentViewModel.Students = listSelectListItems;
                //ViewData["FellowList"] = FellowViewModel;
               // return View(StudentViewModel);
            }
            else {
                //Response.Write("<script> alert('After you register, an administrator needs to assign your account the proper user rights, so there may be a delay in seeing your caseload.  Please contact an administrator.')</script>");
                //return new JavascriptResult { Script = "alert('After you register, an administrator needs to assign your account the proper user rights, so there may be a delay in seeing your caseload.  Please contact an administrator.');" };
                //return Content("<script language='javascript' type='text/javascript'>alert('After you register, an administrator needs to assign your account the proper user rights, so there may be a delay in seeing your caseload.  Please contact an administrator.');</script>");
                Session["NPI"] = "";
                return RedirectToAction("index", "therapist");
            }

            return View(StudentViewModel);
        }

    

        [HttpPost]
        public ActionResult AddSession(AddSessionViewModel objviewmodel)
        {
            string errormsg = "";
            var data = "";
            int sessionId = Convert.ToInt32(objviewmodel.SessionId);
            
            var TID = Session["UserID"].ToString();
            DateTime Sdate = DateTime.Parse(objviewmodel.SessionDate);
            TimeSpan duration = DateTime.Parse(objviewmodel.EndTime).Subtract(DateTime.Parse(objviewmodel.StartTime));
            if (objviewmodel.ServiceType == "Child")
            {

                // Check for Errors and discrepancy

                var studList = objviewmodel.StudentList.Split(',');
                var studListText = objviewmodel.StudentListText.Split(',');
               
                string Group = "";
                string Duration = "";
                string LangCode = "";
                string Freq = "";
                string SGroup = "";
                string SDuration = "";
                string SLangCode = "";
                string SFreq = "";

                try
                {
                    // DateTime startTime = DateTime.ParseExact(objviewmodel.StartTime, "HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture);
                    // DateTime endTime = DateTime.ParseExact(objviewmodel.EndTime, "HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture);
                    
                    String location = objviewmodel.Location;
                    string langCode = objviewmodel.Language;
                    //TimeSpan duration = endTime.Subtract(startTime);

                    int CurrentGroup = Convert.ToInt32(objviewmodel.GroupSize); // Get the current session Group size to check the mandate
                    var GroupType = objviewmodel.GroupType;
                    int StartPos = 0;
                    int DuratStartpos = 0;
                    TempData["ErrorExist"] = "False";
                    String[,] Error = new string[CurrentGroup, 5];
                    // if (CurrentGroup != 1) { StartPos = 2; DuratStartpos = 3; }
                    if (GroupType == "SP") { StartPos = 2; DuratStartpos = 3; }

                    for (var i = 0; i < studList.Length - 1; i++)
                    {
                        var IsNotGroup = "0"; // Used in sp_get_SessionInfo to not getting that group count
                        GetMandate(studList[i].ToString(),TID,Sdate, out  Group, out Duration, out LangCode, out Freq); // Get mandate information
                        if (GroupType == "S1")
                        {
                            // new Code
                            if (string.IsNullOrEmpty(Group)) {
                                errormsg = "The date selected is outside the service mandate date.";
                            }
                            else
                            {
                                var Group1 = Group.Substring(0, 1);
                                if (Group1.Trim() == "0")
                                {
                                    Error[i, 1] = "Student dosen't have the single session mendate.";
                                    TempData["ErrorExist"] = "True";
                                }
                                Group = "1";
                                //  IsNotGroup = "0";
                            }
                        }
                        else 
                        {
                            if (!string.IsNullOrEmpty(Group))
                            {
                                Group = Group.Substring(StartPos, 1);
                                //IsNotGroup = "1";
                            }
                            else {
                                errormsg = "The date selected is outside the service mandate date.";
                            }
                        }


                        if (string.IsNullOrEmpty(Group)) {
                            errormsg = "The date selected is outside the service mandate date.";
                        }

                        else if (CurrentGroup > Convert.ToInt32(Group))
                        {
                            //Error[i, 0] = studList[i].ToString();
                            Error[i, 0] = studListText[i].ToString();
                            Error[i, 1] = "Student group size dosen't match the mandate group size.";
                            TempData["ErrorExist"] = "True";

                        }
                        else
                        {
                            DateTime SessionDate = DateTime.Parse(objviewmodel.SessionDate);
                            GetSession(studList[i].ToString(), SessionDate, Group, GroupType,TID, out  SGroup, out SDuration, out SLangCode, out SFreq); // Get added session information


                            // Error[i, 0] = studList[i].ToString();
                            Error[i, 0] = studListText[i].ToString();
                            /// Check 1: Group Size(One2One,Many2One) and Freq(One2One,Many2One) of Mandate > Session information on this week(SUN-SAT)
                            if (!string.IsNullOrEmpty(Freq))
                            {
                                if (sessionId == 0)
                                {
                                    if (Convert.ToInt32(Freq.Substring(StartPos, 1)) <= Convert.ToInt32(SFreq))
                                    {
                                        Error[i, 4] = "Frequency of seesson is more then mandate.";
                                        TempData["ErrorExist"] = "True";

                                    }
                                }
                                /// Check 2: Match the currrent seesion information matches with mandate (Group Size should no be > then mandate group size)
                                /// Group Size Checks
                                if (CurrentGroup > Convert.ToInt32(Group))
                                {
                                    Error[i, 1] = "Group Size Execedes!";
                                    TempData["ErrorExist"] = "True";
                                }
                                /// Group Duration Check (session duration should not > Mandate Duration)
                                int min = Convert.ToInt32(Duration.Substring(DuratStartpos, 2));
                                string value = "00";
                                if (min == 60)
                                {
                                    value = "01:00:00";
                                }
                                else if (min > 60)
                                {
                                    var diff = min - 60;
                                    value = "01:" + diff + ":00";

                                }

                                else
                                {
                                    value = "00:" + Duration.Substring(DuratStartpos, 2) + ":00";

                                }
                                TimeSpan ManDuration = TimeSpan.Parse(value);// + Duration.Substring(DuratStartpos,2) + ":00");
                                if (duration > ManDuration)
                                {
                                    Error[i, 2] = "Duration Execedes!";
                                    TempData["ErrorExist"] = "True";
                                }
                                else if (duration.ToString().Contains("-")) {
                                    Error[i, 2] = "Duration is negative!";
                                    TempData["ErrorExist"] = "True";
                                }
                                /// language Check

                                if (langCode != LangCode)
                                {
                                    Error[i, 3] = "Language dosnt match with mandate language!";
                                    TempData["ErrorExist"] = "True";
                                }




                            }
                            else
                            {

                                errormsg = "The date selected is outside the service mandate date.";
                            }

                            //  return RedirectToAction("registration_sucess", "account");



                            //    }

                            //else
                            //{
                            //    ModelState.AddModelError("", errormsg);
                            //}


                        }


                    }
                    if (errormsg == "")
                    {

                        TempData["StudentList"] = objviewmodel.StudentList;
                        TempData["StudentListText"] = objviewmodel.StudentListText;
                        TempData["Date"] = objviewmodel.SessionDate;
                        TempData["Location"] = objviewmodel.Location;
                        TempData["ServiceType"] = objviewmodel.ServiceType;
                        TempData["StartTime"] = objviewmodel.StartTime;
                        TempData["EndTime"] = objviewmodel.EndTime;
                        TempData["Duration"] = duration.ToString();
                        if (duration.ToString().Contains("-"))
                        {
                            TempData["ErrorExist"] = "True";
                        }
                        TempData["Group"] = CurrentGroup.ToString();
                        TempData["Language"] = objviewmodel.Language;
                        TempData["GroupType"] = objviewmodel.GroupType;
                        TempData["SessionID"] = objviewmodel.SessionId;

                        List<string> errorList = Error.Cast<String>().ToList();
                        TempData["ErrorArry"] = errorList;
                        data = "Success";
                    }
                    else
                    {
                        return Json(errormsg);
                    }

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex);

                }
            }
            else  {

                TempData["Date"] = objviewmodel.SessionDate;
                TempData["Location"] = objviewmodel.Location;
                TempData["ServiceType"] = objviewmodel.ServiceType;
                TempData["FellowList"] = objviewmodel.FellowList;
                TempData["FellowListText"] = objviewmodel.FellowListText;
                TempData["StartTime"] = objviewmodel.StartTime;
                TempData["EndTime"] = objviewmodel.EndTime;
                TempData["Duration"] = duration.ToString();
                if (duration.ToString().Contains("-"))
                {
                    TempData["ErrorExist"] = "True";
                }
                TempData["SessionId"] = objviewmodel.SessionId;
                data = "Success";
            
            }
            //return View("confirm-session", objviewmodel);
           
            return Json(data);
            //return RedirectToAction("session-confirm", "session");
        }

        [ActionName("session-confirm")]
        public ActionResult SessionConfirm()
        {

            if (CheckUserLoginStatus() <= 0)
                return AccessDeniedView();

            AddSessionViewModel objviewmodel = new AddSessionViewModel();
            List<ErrorListViewModel> objError = new List<ErrorListViewModel>();
             objviewmodel.StudentList = (!string.IsNullOrEmpty((string)(TempData["StudentList"]))) ? (string)TempData["StudentList"] : null;
             objviewmodel.StudentListText = (!string.IsNullOrEmpty((string)(TempData["StudentListText"]))) ? (string)TempData["StudentListText"] : null;
             objviewmodel.FellowList = (!string.IsNullOrEmpty((string)(TempData["FellowList"]))) ? (string)TempData["FellowList"] : null;
             objviewmodel.FellowListText = (!string.IsNullOrEmpty((string)(TempData["FellowListText"]))) ? (string)TempData["FellowListText"] : null;
            //objviewmodel.StudentList = (!string.IsNullOrEmpty((string)TempData["StudentList"])) ? true : false;
            //objviewmodel.SessionDate = (string)TempData["Date"];
             objviewmodel.SessionDate = (!string.IsNullOrEmpty((string)(TempData["Date"]))) ? (string)TempData["Date"] : null;
            //objviewmodel.Location = (string)TempData["Location"];
             objviewmodel.Location = (!string.IsNullOrEmpty((string)(TempData["Location"]))) ? (string)TempData["Location"] : null;
            //objviewmodel.ServiceType = TempData["ServiceType"].ToString();
             objviewmodel.ServiceType = (!string.IsNullOrEmpty((string)(TempData["ServiceType"]))) ? (string)TempData["ServiceType"] : null;
            //objviewmodel.StartTime = (string)TempData["StartTime"];
            objviewmodel.StartTime = (!string.IsNullOrEmpty((string)(TempData["StartTime"]))) ? (string)TempData["StartTime"] : null;
            //objviewmodel.EndTime = (string)TempData["EndTime"];
            objviewmodel.EndTime = (!string.IsNullOrEmpty((string)(TempData["EndTime"]))) ? (string)TempData["EndTime"] : null;
            // objviewmodel.Duration = TempData["Duration"].ToString();
             objviewmodel.Duration = (!string.IsNullOrEmpty((string)(TempData["Duration"]))) ? (string)TempData["Duration"] : null;
             //objviewmodel.GroupSize = TempData["Group"].ToString();
             objviewmodel.GroupSize = (!string.IsNullOrEmpty((string)(TempData["Group"]))) ? (string)TempData["Group"] : null;
             //objviewmodel.Language = TempData["Language"].ToString();
             objviewmodel.Language = (!string.IsNullOrEmpty((string)(TempData["Language"]))) ? (string)TempData["Language"] : null;
             //objviewmodel.GroupType = TempData["GroupType"].ToString();
             objviewmodel.GroupType = (!string.IsNullOrEmpty((string)(TempData["GroupType"]))) ? (string)TempData["GroupType"] : null;
             //var errorExist = TempData["ErrorExist"].ToString();
             var errorExist = (!string.IsNullOrEmpty((string)(TempData["ErrorExist"]))) ? (string)TempData["ErrorExist"] : null;
            //objviewmodel.Duration = (string)TempData["Duration"];
            //objviewmodel.GroupSize = (string)TempData["Group"];
             objviewmodel.SessionId = (!string.IsNullOrEmpty((string)(TempData["SessionId"]))) ? (string)TempData["SessionId"] : null;
            // objviewmodel.SessionId = TempData["SessionID"].ToString();
             //var ErrorList = (!string.IsNullOrEmpty((string)(TempData["ErrorArry"]))) ? TempData["ErrorArry"] as List<String> : null;
           var ErrorList = TempData["ErrorArry"] as List<String>;
            //int i = 0;
           if (objviewmodel.ServiceType == "Child")
           {
               for (var i = 0; i < ErrorList.Count; i = i + 5)
               {

                   objError.Add(new ErrorListViewModel
                   {

                       Child = ErrorList[i],
                       Error1 = ErrorList[i + 1],
                       Error2 = ErrorList[i + 2],
                       Error3 = ErrorList[i + 3],
                       Error4 = ErrorList[i + 4]
                   });

               }

               objviewmodel.ErrorArry = objError;
           }
            if (errorExist == "True")
            {
                objviewmodel.ErrorExist = errorExist;
            }
            return View(objviewmodel);
        }
        [HttpPost]
        public ActionResult SessionConfirm(AddSessionViewModel objviewmodel)
        {
            string errormsg = "";
            var TID = Session["UserID"].ToString();
            int sessionId = Convert.ToInt32(objviewmodel.SessionId);
            var SID = "";
        
            db.SP_DeleteSessionByID(sessionId.ToString());
                    
            if (String.IsNullOrEmpty(objviewmodel.ErrorExist))
            {
                

                    if (objviewmodel.ServiceType == "Child" || objviewmodel.ServiceType == "CS")
                    {
                        var studList = objviewmodel.StudentList.Split(',');
                        for (var i = 0; i < studList.Length - 1; i++)
                        {
                            // delete the session


                            SID = studList[i].ToString();
                            var IsExist = (from n in db.SessionDetails
                                           where n.SessionID == sessionId && n.SID == SID
                                           select n.SID).SingleOrDefault();

                            // IsExist is null then insert or Update
                            if (string.IsNullOrEmpty(IsExist))
                            {
                                var result1 = db.Sp_Insert_sessionDetail(sessionId, studList[i].ToString(), objviewmodel.GroupSize, DateTime.Parse(objviewmodel.SessionDate),
                                    DateTime.Parse(objviewmodel.StartTime), DateTime.Parse(objviewmodel.EndTime), objviewmodel.Location, TID, objviewmodel.ServiceType, objviewmodel.GroupType);

                                var result2 = db.Sp_GetSessionBySID(studList[i].ToString(), DateTime.Parse(objviewmodel.SessionDate)).SingleOrDefault();
                                if (result2 != null) {
                                    sessionId = result2.SessionID;
                                
                                }


                            }
                            else {

                                var result1 = db.Sp_Update_sessionDetail(sessionId.ToString(), studList[i].ToString(), objviewmodel.GroupSize, DateTime.Parse(objviewmodel.SessionDate),
                                    DateTime.Parse(objviewmodel.StartTime), DateTime.Parse(objviewmodel.EndTime), objviewmodel.Location, TID, objviewmodel.ServiceType, objviewmodel.GroupType);
                            
                            }
                          
                            //var Result = db.Sp_Insert_sessionDetail(studList[i].ToString(), objviewmodel.GroupSize, DateTime.Parse(objviewmodel.StartTime),
                            //            DateTime.Parse(objviewmodel.EndTime), DateTime.Parse(objviewmodel.SessionDate), objviewmodel.Location, TID,objviewmodel.ServiceType);
                        }

                    }
                    else
                    {

                        var IsExist = (from n in db.SessionDetails
                                       where n.SessionID == sessionId && n.TID == TID
                                       select n.TID).SingleOrDefault();
                        // IsExist is null then insert or Update
                        if (string.IsNullOrEmpty(IsExist))
                        {
                            var result1 = db.Sp_Insert_sessionDetail(sessionId,"", objviewmodel.GroupSize, DateTime.Parse(objviewmodel.SessionDate),
                                          DateTime.Parse(objviewmodel.StartTime), DateTime.Parse(objviewmodel.EndTime), objviewmodel.Location, TID, objviewmodel.ServiceType, objviewmodel.GroupType);


                        }
                        else
                        {

                           
                            var result1 = db.Sp_Update_sessionDetail(sessionId.ToString(), "", objviewmodel.GroupSize, DateTime.Parse(objviewmodel.SessionDate),
                                    DateTime.Parse(objviewmodel.StartTime), DateTime.Parse(objviewmodel.EndTime), objviewmodel.Location, TID, objviewmodel.ServiceType, objviewmodel.GroupType);
                        }
                       
                    }
             
          
                errormsg = "Success";
            }
            else {
                errormsg = "fail";
            }
            return Json(errormsg);
        }


        public ActionResult SessionList(AddSessionViewModel objviewmodel)
        {

            if (CheckUserLoginStatus() <= 0)
                return AccessDeniedView();

            string errormsg = "";
            //DateTime TotDuration = DateTime.Parse(objviewmodel.SessionDate.ToString());
            var total = new TimeSpan();
            var duration = new TimeSpan();
            int SessionId = 0;
            var StudName = "";
            // List<AddSessionViewModel> ResultinComplete = new List<AddSessionViewModel>;
            AddSessionViewModel objSessionView = new AddSessionViewModel();
            int i = 0;
            var CPSE = 0;
            var CSE = 0;
            var CS = 0;
            var PAS = 0;
            var PP = 0;
            var PI = 0;
            var RSA = 0;
            var EI = 0;
            var Other = 0;

            var TID = Session["UserID"].ToString();
            var StartDay = Session["Start"].ToString();
            var EndDay = Session["End"].ToString();
            DateTime datevalue = Convert.ToDateTime(StartDay);
            int Fiscal = datevalue.Year;
            if (datevalue.Month > 8) { Fiscal = Fiscal + 1; }
            CultureInfo ci = CultureInfo.InvariantCulture;
            // var NPI = db.Sp_getTherpist_NPI(TID).SingleOrDefault();
            List<SessionListViewModel> SessionList = new List<SessionListViewModel>();
            int TIDVal = Convert.ToInt32(TID);
            var TherResult = db.Sp_get_Therpist_Info(TID).SingleOrDefault();
            //Session["NPI"] = TherResult.NPI.Trim();
            var NPI = TherResult.NPI;
            var ResultinComplete = db.SP_getSessionList(StartDay, EndDay, TID).ToList();

            if (ResultinComplete != null)
            {
                
                foreach (var list in ResultinComplete)
                {
                    
                   // var SDate = list.Date.Value.Date.ToShortDateString();
                    if (SessionId != Convert.ToInt32(list.SessionID))
                    {

                        SessionList.Add(new SessionListViewModel
                        {

                            SessionID = list.SessionID.ToString(),
                            SessionDate = list.Date.Value.Date,
                            SName = (list.ServiceType != "CS") ? list.StudentLastName + "," + list.StudentFirstName : list.LastName + "," + list.FirstName,//list.StudentFirstName + " " + list.StudentLastName,
                            //SName = list.StudentFirstName + " " + list.StudentLastName,
                            Group = list.GroupSize,
                            //StartTime = Convert.ToDateTime(list.StartTime.ToString()).ToString("h:mm tt", ci),
                           
                            StartTime = Convert.ToDateTime(list.StartTime.ToString()),
                            EndTime = Convert.ToDateTime(list.EndTime.ToString()),
                            //EndTime = Convert.ToDateTime(list.EndTime.ToString()).ToString("h:mm tt", ci),
                            Duration = DateTime.Parse(list.EndTime.ToString()).Subtract(DateTime.Parse(list.StartTime.ToString())).ToString(),
                            GroupType = list.GroupType,
                            ServiceType = list.ServiceType


                        });
                        duration = DateTime.Parse(list.EndTime.ToString()).Subtract(DateTime.Parse(list.StartTime.ToString()));
                        total = total.Add(duration);
                        StudName = list.StudentLastName + " " + list.StudentFirstName;
                    }
                    else
                    {
                        SessionList.RemoveAt(i-1);
                        i = i - 1;

                        SessionList.Add(new SessionListViewModel
                        {

                            SessionID = list.SessionID.ToString(),
                            SessionDate = list.Date.Value.Date,
                            SName = (SessionId == list.SessionID) ? StudName + "/" + list.StudentLastName + " " + list.StudentFirstName : list.StudentLastName + " " + list.StudentFirstName,
                            //SName = list.StudentFirstName + " " + list.StudentLastName,
                            Group = list.GroupSize,
                            //StartTime = Convert.ToDateTime(list.StartTime.ToString()).ToString("h:mm tt", ci),
                            //EndTime = Convert.ToDateTime(list.EndTime.ToString()).ToString("h:mm tt", ci),
                            StartTime = Convert.ToDateTime(list.StartTime.ToString()),
                            EndTime = Convert.ToDateTime(list.EndTime.ToString()),
                            Duration = DateTime.Parse(list.EndTime.ToString()).Subtract(DateTime.Parse(list.StartTime.ToString())).ToString(),
                            GroupType = list.GroupType,
                            ServiceType = list.ServiceType


                        });
                        StudName += "/" + list.StudentFirstName + " " + list.StudentLastName;
                    }
                    //TotDuration = TotDuration.AddHours(DateTime.Parse(list.EndTime.ToString()).Subtract(DateTime.Parse(list.StartTime.ToString())));
                   // duration = DateTime.Parse(list.EndTime.ToString()).Subtract(DateTime.Parse(list.StartTime.ToString()));
                    //total = total.Add(duration);
                    SessionId = list.SessionID;
                   
                    i = i + 1;

                }
                

                }
            // = db.SP_SessCountByFuningCode(StartDay, EndDay, TID, "CPSE",NPI).Count();
           objSessionView.CPSE = db.SP_BillingNew(Fiscal.ToString(), StartDay, EndDay, TID, "CPSE", NPI).Count();
           objSessionView.CSE = db.SP_BillingNew(Fiscal.ToString(), StartDay, EndDay, TID, "CSE", NPI).Count();
           objSessionView.PP = db.SP_BillingNew(Fiscal.ToString(), StartDay, EndDay, TID, "PP", NPI).Count();
           objSessionView.PI = db.SP_BillingNew(Fiscal.ToString(), StartDay, EndDay, TID, "PI", NPI).Count();
           objSessionView.RSA = db.SP_BillingNew(Fiscal.ToString(), StartDay, EndDay, TID, "RSA", NPI).Count();
           objSessionView.EI = db.SP_BillingNew(Fiscal.ToString(), StartDay, EndDay, TID, "EI", NPI).Count();
            objSessionView.PAS = db.SP_SessCountByServiceType(StartDay, EndDay, TID, "SAS").Count();
            objSessionView.CS = db.SP_SessCountByServiceType(StartDay, EndDay, TID, "CS").Count();
            objSessionView.Ohers = db.SP_SessCountByServiceType(StartDay, EndDay, TID, "Other").Count();

                objSessionView.SessionList = SessionList;
                var value = total.Hours;
                var val1 = total.TotalHours.ToString().Split('.');
                objSessionView.Duration = (val1[0].ToString() + ":" + Convert.ToInt32(total.Minutes)).ToString();

           
                var IsLock = (from n in db.TherapistMasters
                              where n.TID == TIDVal
                              select n.Lock).SingleOrDefault();

                var LockValue = "true";
                if (IsLock.Substring(0, 1) == "1") { LockValue = "True"; }
                else {LockValue = "False";}
                if (IsLock != null)
                {
                    objSessionView.Lock = Convert.ToBoolean(LockValue);
                }
                return View(objSessionView);


            }

        
      
        public ActionResult EditSession()
        {
            if (CheckUserLoginStatus() <= 0)
                return AccessDeniedView();
            return View();
        }
        [HttpPost]
        public ActionResult EditSession(ReportViewModel objviewmodel)
        {

            var StartDay = objviewmodel.StartDate;
            var EndDay = objviewmodel.EndDate;
            var message = "success";
         
           Session["Start"] = StartDay.ToString();
           Session["End"] = EndDay.ToString();
         
            return Json(message);
        }

        [HttpPost]
        public ActionResult CheckSessioDate(AddSessionViewModel objviewmodel)
        {
            var studList = objviewmodel.StudentList.Split(',');
            var studText = objviewmodel.StudentListText.Split(',');
            var result = "";
            var result1 = "";
            int sessionId = Convert.ToInt32(objviewmodel.SessionId);
            DateTime SDate = DateTime.Parse(objviewmodel.SessionDate);
                //Convert.ToDateTime(objviewmodel.SessionDate);
            var SuccessResult = "false";
            for (var i = 0; i < studList.Length - 1; i++)
            {
                var studId = studList[i].ToString();
                result1 = (from n in db.SessionDetails
                          where n.Date == SDate && n.SID == studId && n.SessionID != sessionId && n.GroupSize != null
                              select n.SID).SingleOrDefault();
                if (!string.IsNullOrEmpty(result1))
                {
                    SuccessResult = "true";
                    result = studText[i].ToString() + ",";
                }
            }

           

            if (result != "")
            {
                SuccessResult = "true";
                return Json(result);
            }
            else
            {
                SuccessResult = "false";
                return Json(result);
            }


        }

        [HttpPost]
        public ActionResult CheckSessioTime(AddSessionViewModel objviewmodel)
        {

            var result = "No Overlap";
           

            DateTime SDate = DateTime.Parse(objviewmodel.SessionDate);
            DateTime StartTime = DateTime.Parse(objviewmodel.StartTime);
            DateTime EndTime = DateTime.Parse(objviewmodel.EndTime);
            var sessionId = objviewmodel.SessionId;
            
            var SuccessResult = "false";
            var TID = Session["UserID"].ToString();
            var SessionResult = db.Sp_getTherpist_SessionInDay(SDate,TID).ToList();

            foreach (var list in SessionResult)
            {
                if (sessionId != list.SessionID.ToString()) // Checks if the edit SessId and result sessionId is same then by pass the session overlaps condition
                {
                    if (StartTime < DateTime.Parse(list.EndTime.ToString()) && (EndTime > DateTime.Parse(list.StartTime.ToString())))  // Checks the overlap of the session time
                    {
                        result = "Session time overlaps with other session time.";
                        break;

                    }
                    else
                    {
                        result = "No Overlap";

                    }
                }
                else {

                    result = "No Overlap";
                
                }
            
            }
            
           

                return Json(result);


        }


       static void GetMandate(string SID,string TID, DateTime SDate, out string Group, out string Duration, out string Lang,out string Freq)
    {
           CSNY_NewEntities db1 = new CSNY_NewEntities();
	Group = Duration= Lang= Freq = "";

    var NPI = db1.Sp_getTherpist_NPI(TID).SingleOrDefault();
    var mandateResult = db1.sp_get_MandateInfo(SID,NPI.Trim(),SDate).ToList();
           
    foreach (var list in mandateResult)
    {
        if (SDate >= list.ServiceStart && SDate <= list.ServiceEnd)
        {
            Group = list.GroupSize;
            Duration = list.Duration;
            Lang = list.Language;
            Freq = list.Frequency;
        }
       
    }


    }

       static void GetSession(string SID,DateTime Sdate,string SessionGroup,string GroupType,string TID, out string SGroup, out string SDuration, out string SLang, out string SFreq)
       {
           CSNY_NewEntities db1 = new CSNY_NewEntities();
           SGroup = SDuration = SLang = SFreq = "";

         // var SessionResult = db1.sp_get_SessionInfo(SID,Sdate,SGroup).ToList();
           var SessionFreq = db1.sp_get_SessionInfo(SID, Sdate, SessionGroup, GroupType,TID).SingleOrDefault();

           if (SessionFreq != null) {
               SFreq = SessionFreq.GCount.ToString();
           }


       }

       public int CheckUserLoginStatus()
       {
           int LoginUserId = 0;
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

       [HttpPost]
       public ActionResult delete_session(string id)
       {
           var message = "Submit Successfully!";
         var result =   db.SP_DeleteSessionByID(id);
           
           return Json(message);

       }


       [HttpPost]
       public ActionResult ShowMandate(AddSessionViewModel objviewmodel)
       {

           
           DateTime SDate = DateTime.Parse(objviewmodel.SessionDate);
           var studList = objviewmodel.StudentList.Split(',');
           var TID = Session["UserID"].ToString();
           var Group = "";
           var Duration = "";
           var LangCode = "";
           var Freq = "";
           string Body = "";
           var NPI = db.Sp_getTherpist_NPI(TID).SingleOrDefault();

           for (var i = 0; i < studList.Length - 1; i++)
           {

               //GetMandate(studList[i].ToString(), TID, SDate, out  Group, out Duration, out LangCode, out Freq); // Get mandate information

               var result = db.SP_GetStudentMandate(NPI.TrimEnd(), studList[i].ToString(), SDate).SingleOrDefault();
               if (result != null)
               {
                   var OldFreqVal = result.Frequency.Split(',');
                   var OldDuraVal = result.Duration.Split(',');
                   var OldGroupVal = result.GroupSize.Split(',');

                   Body +=
                         " <br/> Student Name : " + result.StudentFirstName.TrimEnd() + " "+ result.StudentLastName.TrimEnd() +
                         " <br/> Service Start : " + result.ServiceStart.Value.ToShortDateString() +
                         " <br/> Service End : " + result.ServiceEnd.Value.ToShortDateString() +
                         " <br/> S1 : " + OldFreqVal[0] + " X " + OldDuraVal[0] + ":" + OldGroupVal[0] +
                         " <br/> SP : " + OldFreqVal[1] + " X " + OldDuraVal[1] + ":" + OldGroupVal[1] +
                         " <br/> Language : " + result.Language +

                         "<br/>";

               }
               else {
                   var names = db.Sp_GetStudentDetail(studList[i].ToString()).SingleOrDefault();
                   Body +=
                       " <br/> Student Name : " + names.StudentFirstName.TrimEnd() + " " + names.StudentLastName.TrimEnd() +
                       " <br/> No Mandate/ Mandate Outside" + "<br/>";
               }
           }
           

           return Json(Body);


       }

    }
}
