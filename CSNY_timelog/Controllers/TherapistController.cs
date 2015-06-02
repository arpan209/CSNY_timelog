using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using CSNY_timelog.Models;
using CSNY_timelog.ViewModel;
using System.Globalization;
using CSNY_timelog.Helper;
using System.Drawing;
using System.Drawing.Drawing2D;


namespace CSNY_timelog.Controllers
{
    public class TherapistController : Controller
    {
        //
        // GET: /Therapist/
        CSNY_NewEntities db = new CSNY_NewEntities();


        public ActionResult Index()
        {
            if (CheckUserLoginStatus() <= 0)
                return AccessDeniedView();

            var Id = Session["userId"].ToString();

            var result = db.Sp_get_Therpist_Info(Id).SingleOrDefault();

            Session["NPI"] = result.NPI;
            if (string.IsNullOrEmpty(result.NPI))
            {
                Response.Write("<script> alert('After you register, an administrator needs to assign your account the proper user rights, so there may be a delay in seeing your caseload.  Please contact an administrator.')</script>");
            }

            RegistrationViewModel objView = new RegistrationViewModel();

            objView.FirstName = result.FirstName;

            return View(objView);
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

        public ActionResult StudentList()
        {

            if (CheckUserLoginStatus() <= 0)
                return AccessDeniedView();
            //var TID = Session["UserID"].ToString();
            //int TIDVal = Convert.ToInt32(TID);
            //var TherResult = db.Sp_get_Therpist_Info(TID).SingleOrDefault();
            //Session["NPI"] = TherResult.NPI.Trim();


            return View();
        }

        public ActionResult sign()
        {

            //if (CheckUserLoginStatus() <= 0)
            //    return AccessDeniedView();
            //var TID = Session["UserID"].ToString();
            //int TIDVal = Convert.ToInt32(TID);
            //var TherResult = db.Sp_get_Therpist_Info(TID).SingleOrDefault();
            //Session["NPI"] = TherResult.NPI.Trim();


            return View();
        }

        //[HttpPost]
        //public ActionResult GetSign(SignViewModel objviewmodel)
        //{
        //    ViewBag.Message = Request["output"].ToString();
        //    var name = objviewmodel.name;
        //    var output = objviewmodel.output;

        //    return View();
        //}

        public ActionResult Signature(string name, string output) {

            ViewBag.Message = Request["output"].ToString();
            var signtoImage = new ConsumedByCode.SignatureToImage.SignatureToImage();

            var signatureImage = signtoImage.SigJsonToImage(output);
           // Bitmap resized = new Bitmap(signatureImage, new Size(signatureImage.Width / 2, signatureImage.Height / 2));

            Bitmap newImage = new Bitmap(signatureImage.Width, signatureImage.Height);
         
            using (Graphics gr = Graphics.FromImage(newImage))
            {
                gr.CompositingQuality = CompositingQuality.HighQuality;
                gr.SmoothingMode = SmoothingMode.HighQuality;
                gr.SmoothingMode = SmoothingMode.AntiAlias;
                gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                gr.DrawImage(signatureImage, new Rectangle(0, 0, signatureImage.Width, signatureImage.Height));
            }


            var path = Path.Combine(Server.MapPath("~/signatures/"), name + ".bmp");
            newImage.MakeTransparent();
            newImage.Save(path);
            //signatureImage.Save(path);

            return View();
        
        
        }

        public ActionResult StudentListInfo(string id)
        {

            if (CheckUserLoginStatus() <= 0)
                return AccessDeniedView();

            var fiscal = id;
            FindUserViewModel objviewmodel = new FindUserViewModel();
            try
            {

                List<StudentListViewModel> StudentList = new List<StudentListViewModel>();


                var Value = Session["UserID"].ToString();
               
                //    var result = db.SP_FindUser_ByUserID(Value).ToList();
                var npi = db.Sp_getTherpist_NPI(Value).SingleOrDefault();
               
                var result = db.SP_FindStudent_ByNPI(npi,fiscal).ToList();
                foreach (var item in result)
                {
                    var Freq = item.Frequency.Split(',');
                    var Duration = item.Duration.Split(',');
                var group = item.GroupSize.Split(',');
               

                    StudentList.Add(new StudentListViewModel
                    {
                        SID = item.SID.ToString(),
                        FirstName = item.StudentLastName.Trim() + ","+ item.StudentFirstName.Trim(),
                        
                        OSIS = item.NYCI,
                        FundingCode =  item.FundingCode, //(item.FundingCode == "CSE") ? "1" : "2", // if its CSE then 1 or 2 for CPSE
                        Fiscal = fiscal,
                        S1 = Freq[0] + "X" + Duration[0] + ":" + group[0],
                        SP = Freq[1] + "X" + Duration[1] + ":" + group[1]
                    });

                }
                objviewmodel.StudentList = StudentList;

            }

            catch (Exception ex)
            {
            }

            return View(objviewmodel);
        }

        public ActionResult StudentInfo(string id)
        {
            if (CheckUserLoginStatus() <= 0)
                return AccessDeniedView();

            var StudArry = id.Split(',');
            int Sid = Convert.ToInt32(StudArry[0]);
            var fiscal = StudArry[1];
            //var FundingCode = StudArry[2];
            var info = (from n in db.StudentMasters where n.SID == Sid select n).SingleOrDefault();

            AddStudentViewModel objView = new AddStudentViewModel();

            objView.SID = info.SID.ToString();
            objView.FirstName = info.StudentFirstName + " " + info.StudentLastName;
            objView.Fiscal = fiscal;
          //  objView.FundingCode = FundingCode;
            objView.DOB = DateTime.Now.Month.ToString();
          
            return View(objView);
        }

        public ActionResult StudentInformation(string id)
        {

            if (CheckUserLoginStatus() <= 0)
                return AccessDeniedView();

            var dataVal = id.Split(',');

            var sid = dataVal[0];
            var month = dataVal[1];
            var startDay = dataVal[2];
            var endDay = dataVal[3];
            var year = dataVal[4];

            var TID = Session["UserID"].ToString();
            var TIDVal = Convert.ToInt32(TID);
            var npi = db.Sp_getTherpist_NPI(TID).SingleOrDefault();
            
            //var StudMendtate = (from n in db.MandateMasters where n.NPI == TID && n.SID == sid select n).fit();

            StudentInfoViewModel objview = new StudentInfoViewModel();

            //objview.Frequency = StudMendtate.Frequency;
            //objview.GroupSize = StudMendtate.GroupSize;
            //objview.Duration = StudMendtate.Duration;
            //objview.Language = StudMendtate.Language;

            List<StudSessionListViewModel> SessionList = new List<StudSessionListViewModel>();
            CultureInfo ci = CultureInfo.InvariantCulture;
            var StudList = db.SP_GetSessionListBySID(sid, TID, month,startDay,endDay,year).ToList();
            int i = 0;
            if (StudList != null)
            {
                foreach (var list in StudList)
                {

                    SessionList.Add(new StudSessionListViewModel
                    {

                        SessionID = list.SessionID.ToString(),
                        SessionDate = list.Date.Value.Date.ToShortDateString(),
                        SName = (list.ServiceType != "CS") ? list.StudentFirstName + " " + list.StudentLastName : list.StudentFirstName + " " + list.StudentLastName, //list.StudentFirstName + " " + list.StudentLastName,
                        //SName = list.StudentFirstName + " " + list.StudentLastName,
                        Group = list.GroupSize,
                        StartTime = Convert.ToDateTime(list.StartTime.ToString()).ToString("h:mm tt", ci),
                        EndTime = Convert.ToDateTime(list.EndTime.ToString()).ToString("h:mm tt", ci),
                        Duration = DateTime.Parse(list.EndTime.ToString()).Subtract(DateTime.Parse(list.StartTime.ToString())).ToString(),
                        GroupType = list.GroupType,
                        //ServiceType = list.ServiceType


                    });
                    i += 1;
                }
            }
            objview.SessionList = SessionList;
            var IsLock = (from n in db.TherapistMasters
                          where n.TID == TIDVal
                          select n.Lock).SingleOrDefault();

            var LockValue = "true";
            if (IsLock.Substring(0, 1) == "1") { LockValue = "True"; }
            else { LockValue = "False"; }

            if (IsLock != null)
            {
                objview.Lock = Convert.ToBoolean(LockValue);
            }
            return View(objview);
        }

         [HttpPost]
        public ActionResult SessionConfirm(AddStudentViewModel objviewmodel)
        {
            //AddStudentViewModel objviewModel = new AddStudentViewModel();



            return View();
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
