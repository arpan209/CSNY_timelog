using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CSNY_timelog.Models;
using CSNY_timelog.ViewModel;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.Text.RegularExpressions;


namespace CSNY_timelog.Controllers
{
    public class AdminController : Controller
    {
        //CSNYEntities db = new CSNYEntities();
        CSNY_NewEntities db = new CSNY_NewEntities();
        //
        // GET: /Default1/

        public ActionResult Index()
        {

            if (CheckUserLoginStatus() <= 0)
                return AccessDeniedView();
            return View();
        }

        // GET: /Admin/AddStudent

        public ActionResult addstudent(int id)
        {

            if (CheckUserLoginStatus() <= 0)
                return AccessDeniedView();
            // Generate Therapist List
           
            AddStudentViewModel objView = new AddStudentViewModel();

            objView.SID = "0";
            if (id != 0) {

                var result = db.SP_GetStudent_AllInfo(id.ToString()).LastOrDefault();

              if (result != null) {
                  objView.SID = result.SID.ToString().Trim();
                  objView.PFirstName = (!string.IsNullOrEmpty((string)(result.ParentFirstName))) ? (string)result.ParentFirstName.Trim() : null;
                  objView.PLastName = (!string.IsNullOrEmpty((string)(result.ParentLastName))) ? (string)result.ParentLastName.Trim() : null;
                  objView.Address1 = (!string.IsNullOrEmpty((string)(result.Address1))) ? (string)result.Address1.Trim() : null;
                  objView.PHomePh = (!string.IsNullOrEmpty((string)(result.Parent_HomePhone))) ? (string)result.Parent_HomePhone.Trim() : null;
                  objView.POfficePh = (!string.IsNullOrEmpty((string)(result.Parent_WorkPhone))) ? (string)result.Parent_WorkPhone.Trim() : null;
                  objView.PMobilPh = (!string.IsNullOrEmpty((string)(result.Parent_MobilePhone))) ? (string)result.Parent_MobilePhone.Trim() : null;
                  objView.PEmail = (!string.IsNullOrEmpty((string)(result.Parent_Email))) ? (string)result.Parent_Email.Trim() : null;
                  objView.GName = (!string.IsNullOrEmpty((string)(result.GuardianName))) ? (string)result.GuardianName.Trim() : null;
                  objView.GEmail = (!string.IsNullOrEmpty((string)(result.GuardianEmail))) ? (string)result.GuardianEmail.Trim() : null;
                  objView.GMobilPh = (!string.IsNullOrEmpty((string)(result.GuardianMobile))) ? (string)result.GuardianMobile.Trim() : null;
                  objView.Referral = (!string.IsNullOrEmpty((string)(result.Referral))) ? (string)result.Referral.Trim() : null;
                  
                  ///child information
                  ///
                  objView.Fiscal = (!string.IsNullOrEmpty((string)(result.FiscalYear))) ? (string)result.FiscalYear.Trim() : null;
                  objView.NYCID = (!string.IsNullOrEmpty((string)(result.NYCI))) ? (string)result.NYCI.Trim() : null;
                  objView.FundingCode = (!string.IsNullOrEmpty((string)(result.FundingCode))) ? (string)result.FundingCode.Trim() : null;
                  objView.Location = (!string.IsNullOrEmpty((string)(result.Location))) ? (string)result.Location.Trim() : null;
                  objView.SchoolCode = (!string.IsNullOrEmpty((string)(result.SchoolLocationCode))) ? (string)result.SchoolLocationCode.Trim() : null;
                  objView.FirstName = (!string.IsNullOrEmpty((string)(result.StudentFirstName))) ? (string)result.StudentFirstName.Trim() : null;
                  objView.LastName = (!string.IsNullOrEmpty((string)(result.StudentLastName))) ? (string)result.StudentLastName.Trim() : null;
                  objView.DOB = (!string.IsNullOrEmpty((string)(result.DOB))) ? (string)result.DOB.Trim() : null;
                  objView.Districtcode = (!string.IsNullOrEmpty((string)(result.HomeDistrict))) ? (string)result.HomeDistrict.Trim() : null;

                  objView.Diagnosis = (!string.IsNullOrEmpty((string)(result.Diagnosis))) ? (string)result.Diagnosis.Trim() : null;
                  objView.Comments = (!string.IsNullOrEmpty((string)(result.Comments))) ? (string)result.Comments.Trim() : null;
                  objView.ParentReport = (!string.IsNullOrEmpty((string)(result.ParentReport))) ? (string)result.ParentReport.Trim() : null;


              
              }
            
            }
            List<SelectListItem> therapistSelectListItems = new List<SelectListItem>();
            foreach (TherapistMaster Therapist in db.TherapistMasters)
            {
                if (Therapist.UserType == "LIC")
                {
                    SelectListItem selectList1 = new SelectListItem()
                    {

                        Text = Therapist.FirstName + " " + Therapist.LastName,
                        Value = Therapist.TID.ToString()

                    };
                    therapistSelectListItems.Add(selectList1);
                }
            }

            objView.TherapistList = therapistSelectListItems;
               
            //ViewData["FellowList"] = FellowViewModel;
            return View(objView);

           
        }
       
        public ActionResult DeoSuccess()
        {
            if (CheckUserLoginStatus() <= 0)
                return AccessDeniedView();

            // Do the merging of staging table with existing table.
            AddStudentViewModel objview = new AddStudentViewModel();


            List<MandateAddListViewModel> MandateList = new List<MandateAddListViewModel>();
            List<MandateAddListViewModel> MandateErrorList = new List<MandateAddListViewModel>();

           try {

            foreach (StagingTable Student in db.StagingTables)
            {

                if (Student.FundingCode == "CPSE" || Student.FundingCode == "CSE" || Student.FundingCode == "PP" || Student.FundingCode == "PI" || Student.FundingCode == "EI")
                {
                    var OSIS = db.sp_get_StudentInfo(Student.NYCI.Trim()).SingleOrDefault();
                    if (OSIS == null)
                    {
                        // add the new kid in StudentMaster
                        db.AddStudent("", "", "", "", "",
                    "", "", "", "", "", "", "", Student.NYCI.Trim(), "", Student.StudentFirstName, Student.StudentLastName, Student.DOB, "", "", "", "");
                    }


                    var result = db.sp_get_StudentInfo(Student.NYCI.Trim()).SingleOrDefault();
                    if (result != null)
                    {
                        var GroupSize = Student.GroupSize;
                        var result1 = (from n in db.StagingTables
                                       where n.GroupSize != GroupSize.Trim() && n.NYCI == result.NYCI.Trim()
                                       select n).SingleOrDefault();
                        var addFreq = "";
                        var addGroup = "";
                        var AddDuration = "";

                        if (result1 != null)
                        {
                            if (Student.GroupSize.Trim() == "1")
                            {
                                addFreq = Student.Frequency.Trim() + "," + result1.Frequency.Trim();
                                addGroup = Student.GroupSize.Trim() + "," + result1.GroupSize.Trim();
                                AddDuration = Student.Duration.Trim() + "," + result1.Duration.Trim();
                            }
                            else
                            {

                                addFreq = result1.Frequency.Trim() + "," + Student.Frequency.Trim();
                                addGroup = result1.GroupSize.Trim() + "," + Student.GroupSize.Trim();
                                AddDuration = result1.Duration.Trim() + "," + Student.Duration.Trim();

                            }



                        }
                        else
                        {

                            if (Student.GroupSize.Trim() == "1")
                            {
                                addFreq = Student.Frequency.Trim() + ",0";
                                addGroup = Student.GroupSize.Trim() + ",0";
                                AddDuration = Student.Duration.Trim() + ",00";
                            }
                            else
                            {

                                addFreq = "0," + Student.Frequency.Trim();
                                addGroup = "0," + Student.GroupSize.Trim();
                                AddDuration = "00," + Student.Duration.Trim();

                            }

                        }



                        /// Add Mandatte in the List
                        MandateList.Add(new MandateAddListViewModel
                        {
                            SID = result.SID.ToString(),
                            NPI = Student.NPI.Trim(),
                            Fiscal = Student.FiscalYear,
                            StartDate = Student.ServiceStartDate,
                            EndDate = Student.ServiceEndDate,
                            Frequency = addFreq,
                            Duration = AddDuration,
                            GroupSize = addGroup,
                            Language = Student.Language,
                            FundingCode = Student.FundingCode,
                            BoroCode = "",
                            SchoolCode = "",
                            HomeDistrict = ""


                        });


                    }
                }
                else {

                    MandateErrorList.Add(new MandateAddListViewModel
                    {
                        SID = Student.NYCI.Trim().ToString(),
                        NPI = Student.NPI.Trim(),
                        Fiscal = Student.FiscalYear,
                        StartDate = Student.ServiceStartDate,
                        EndDate = Student.ServiceEndDate,
                        Frequency = Student.Frequency,
                        Duration = Student.Duration,
                        GroupSize = Student.GroupSize,
                        Language = Student.Language,
                        FundingCode = Student.FundingCode,
                        BoroCode = "",
                        SchoolCode = "",
                        HomeDistrict = ""


                    });
                
                }
            }


            var list = MandateList.GroupBy(x => x.SID).Select(x => x.First()).ToList();
         //   var temp = list.Distinct();
            foreach(var val in list)
            {

                db.Sp_AddMandate(val.SID.ToString(), val.NPI.Trim(), val.Fiscal, DateTime.Parse(val.StartDate), DateTime.Parse(val.EndDate),
                            val.Frequency, val.GroupSize, val.Duration, val.Language, val.FundingCode, "", "", "");


            }

               }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex);

            }

           objview.MandateErrorList = MandateErrorList;
            return View(objview);
        }

        [HttpPost]
        public ActionResult addstudent(AddStudentViewModel objviewmodel)
        {
            string errormsg = "";

           
            AddStudentViewModel reg = new AddStudentViewModel();



            //if (!objviewmodel.Checkterms)
            //{
            //    ModelState.AddModelError("Checkterms", "You must accept the terms & conditions.");
            //    errormsg += "You must accept the terms & conditions.";
            //}
            try
            {


                reg.SID = objviewmodel.SID.Trim();
                // Parents/ guardian info

                reg.PFirstName = objviewmodel.PFirstName;
                reg.PLastName = objviewmodel.PLastName;

                reg.PHomePh = objviewmodel.PHomePh;
                reg.POfficePh = objviewmodel.POfficePh;
                reg.PMobilPh = objviewmodel.PMobilPh;
                reg.PEmail = objviewmodel.PEmail;
                reg.GName = objviewmodel.GName;
                reg.GEmail = objviewmodel.GEmail;
                reg.GMobilPh = objviewmodel.GMobilPh;

                //  CHILD/doe info
                reg.Fiscal = objviewmodel.Fiscal;
                reg.StartDate = objviewmodel.StartDate;
                reg.EndDate = objviewmodel.EndDate;
                reg.FundingCode = objviewmodel.FundingCode;
                reg.SchoolName = objviewmodel.SchoolName;
                reg.SchoolCode = objviewmodel.SchoolCode;
                reg.Districtcode = objviewmodel.Districtcode;
                reg.BoroughCode = objviewmodel.BoroughCode;
                reg.Frequency = objviewmodel.Frequency;
                reg.Duration = objviewmodel.Duration;
                reg.GroupSize = objviewmodel.GroupSize;

                // Student Info
                reg.FirstName = objviewmodel.FirstName;
                reg.LastName = objviewmodel.LastName;
                reg.Address1 = objviewmodel.Address1;
                reg.NYCID = objviewmodel.NYCID;
                reg.DOB = objviewmodel.DOB;
                reg.ServiceType = objviewmodel.ServiceType;
                reg.Location = objviewmodel.Location;
                reg.Diagnosis = objviewmodel.Diagnosis;
                reg.ParentReport = objviewmodel.ParentReport;
                reg.Comments = objviewmodel.Comments;
                reg.PayType = objviewmodel.PayType;
                reg.Referral = objviewmodel.Referral;

                Session["OSIS"] = objviewmodel.NYCID;
                Session["SName"] = objviewmodel.FirstName + objviewmodel.LastName;
                if (Convert.ToInt32(reg.SID) > 0)
                { // update student information
                    var OSIS = (from n in db.StudentMasters where n.NYCI.Equals(reg.NYCID) select n.NYCI).SingleOrDefault();
                    if (OSIS != null)
                    {

                        var SID = (from n in db.StudentMasters where n.NYCI.Equals(reg.NYCID) select n.SID).SingleOrDefault();
                        if (SID != null)
                        {
                            if (SID == Convert.ToInt32(reg.SID))
                            {
                                db.SP_UpdateStudentInfo(reg.SID, reg.PFirstName, reg.PLastName, reg.PHomePh, reg.POfficePh, reg.PMobilPh,
                               reg.PEmail, reg.GName, reg.GEmail, reg.GMobilPh, reg.NYCID, reg.Location, reg.FirstName,
                               reg.LastName, reg.Address1, reg.DOB, reg.PayType, reg.Referral, reg.ServiceType, reg.Diagnosis,
                               reg.ParentReport, reg.Comments);
                                return RedirectToAction("registration_sucess", "account");
                            }
                            else { Response.Write("<script>alert('OSIS already exist in system.')</script>"); }
                        }
                   


                    }
                    else {
                        db.SP_UpdateStudentInfo(reg.SID, reg.PFirstName, reg.PLastName, reg.PHomePh, reg.POfficePh, reg.PMobilPh,
                                   reg.PEmail, reg.GName, reg.GEmail, reg.GMobilPh, reg.NYCID, reg.Location, reg.FirstName,
                                   reg.LastName, reg.Address1, reg.DOB, reg.PayType, reg.Referral, reg.ServiceType, reg.Diagnosis,
                                   reg.ParentReport, reg.Comments);
                        return RedirectToAction("registration_sucess", "account");
                    }

                    /////


                }
                else // add new student
                {
                    var OSIS = (from n in db.StudentMasters where n.NYCI.Equals(reg.NYCID) select n.NYCI).SingleOrDefault();
                    if (OSIS == null)
                    {
                        db.AddStudent(reg.PayType, reg.Referral, reg.PFirstName, reg.PLastName, reg.Address1,
                            reg.PHomePh, reg.POfficePh, reg.PMobilPh, reg.PEmail, reg.GName, reg.GEmail, reg.GMobilPh,
                            reg.NYCID, reg.Location, reg.FirstName, reg.LastName, reg.DOB, reg.ServiceType,
                            reg.Diagnosis, reg.ParentReport, reg.Comments);
                        return RedirectToAction("AddStudentSuccess", "admin");
                    }
                    else
                    {
                        Response.Write("<script>alert('OSIS already exist in system.')</script>");  }
                }
                //OpBids.Helper.MailSendHelper.SendMailOnReg(str, objL.Email, objL.UserName, objL.Password);

                //OpBids.Helper.MailSendHelper.SendMailToAdminOnReg(objL.FirstName, objL.LastName, objL.CompanyName,
                //    objL.State, objL.City, DateTime.Now.ToString(), objL.UserName, objL.JobTitle, objL.FirstName,
                //    objL.Email, objviewmodel.Phone);

              



                //    }

                //else
                //{
                //    ModelState.AddModelError("", errormsg);
                //}

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex);

            }
           
            return View(objviewmodel);
        }
        
        public ActionResult AddStudentSuccess()
        {
            if (CheckUserLoginStatus() <= 0)
                return AccessDeniedView();

            return View();
        }
      
    // Class add DOE data
          [ActionName("Upload-DOE-data")]
        public ActionResult uploadDOEData()
          {
              if (CheckUserLoginStatus() <= 0)
                  return AccessDeniedView();

            return View();
        }

         [ActionName("Upload-DOE-data")]
          [HttpPost]
          public ActionResult uploadDOEData(HttpPostedFileBase uploadFile) // OR IEnumerable<HttpPostedFileBase> uploadFile
          {
              //For checking purpose 
              HttpPostedFileBase File1 = Request.Files["uploadFile"];
              var error = "";
              string filename = DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString()+"-"+DateTime.Now.Year.ToString()+DateTime.Now.Minute.ToString()+"."+"xls";
              if (File1 != null)
              {
                  //alert(Path.GetFileName(uploadFile.FileName));
                  //If this is True, then its Working.,
                  //string temp1= date.Replace("/","-");
                  //string temp=date.re
                  string filePath = Path.Combine(HttpContext.Server.MapPath("../Uploads/"),
                                                filename);
                 
                  uploadFile.SaveAs(filePath);

                  /// Upload Data
                 
                  string SourceConstr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + filePath + "';Extended Properties= 'Excel 8.0;HDR=Yes;IMEX=1'";

                  OleDbConnection conn = new OleDbConnection(SourceConstr);

                  // Truncate the stagging table 
                  db.TruncateStaging();
                  conn.Open();  
                  // Get the data table containg the schema guid.
                  System.Data.DataTable dt = null;
                  dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                  if (dt == null)
                  {
                      return null;
                  }

                  
                  string[] excelSheet = new string[dt.Rows.Count];
                  int j =0;
                  var sheetname = "";
                  foreach(DataRow row in dt.Rows)
                     {
                      sheetname = row["TABLE_NAME"].ToString();
                      if (sheetname.Substring(0, 1) != "_")
                      {
                          excelSheet[j] = row["TABLE_NAME"].ToString();
                          j++;
                      }
                       }

                  if (excelSheet == null)
                  {
                      return null;
                  }

                 
                  
                  
                 
                  try
                  {
                      for (var k = 0; k < excelSheet.Length; k++)
                      { 
                      //After connecting to the Excel sheet here we are selecting the data 
                      //using select statement from the Excel sheet
                          if(!string.IsNullOrEmpty(excelSheet[k])){
                          var command = "select * from [" + excelSheet[k] + "]";
                      
                      OleDbCommand ocmd = new OleDbCommand(command, conn);
                     //Here [Sheet1$] is the name of the sheet 
                      //in the Excel file where the data is present
                      OleDbDataReader odr = ocmd.ExecuteReader();
                      string fiscal = "";
                      string boroCode = "";
                      string DistCode = "";
                     // string FundCode = "";
                      string SchoolCode = "";
                      // Assign in StudMaster with SID Start //
                      string NYCID = "";
                      string fname = "";
                      string lname = "";
                      string FundingCode = "";
                      // Assign in StudMaster with SID End //
                      string SSN = ""; // Assign in therapistMaster
                      string StartDate = "";
                      string EndDate = "";
                      string Freq = "";
                      string Duration = "";
                      string GroupSize = "";
                      string Language = "";
                      string DOB = "";
                      int i = 0;
                      while (odr.Read())
                      {
                       
                          fiscal = valid(odr, 0);
                          SSN = valid(odr, 2);
                          NYCID = valid(odr, 3);
                          lname = valid(odr, 4);
                          fname = valid(odr, 5);//Here we are calling the valid method
                          Freq = valid(odr, 6);
                          Duration = valid(odr, 7);
                          GroupSize = valid(odr, 8);
                          Language = valid(odr, 9);
                          StartDate = valid(odr, 10);
                          EndDate = valid(odr, 11);
                          FundingCode = valid(odr, 12);
                          DOB = valid(odr, 13);

                         

                          //Here using this method we are inserting the data into the database
                          db.AddStaging(fiscal, SSN, NYCID, lname, fname, Freq,
                               GroupSize, Duration, Language, FundingCode, StartDate, EndDate,DOB);
                          
                          i = i + 1;
                         // insertdataintosql(fname, lname, mobnum, city, state, zip);
                      }
                      }
                  }
                      conn.Close();
                      return RedirectToAction("deoSuccess", "admin");
                  }
                  catch (DataException ee)
                  {
                     error = ee.Message;
                      
                  }
                  


              }
              ViewBag.error = error;
            
              return View();
          }

         public ActionResult FindUser()
         {

             if (CheckUserLoginStatus() <= 0)
                 return AccessDeniedView();
             return View();
         }

         public ActionResult FindTherapist()
         {

             if (CheckUserLoginStatus() <= 0)
                 return AccessDeniedView();
             return View();
         }

         public ActionResult StudentList()
         {

             if (CheckUserLoginStatus() <= 0)
                 return AccessDeniedView();

             FindUserViewModel objviewmodel = new FindUserViewModel();
             int UserId = 0;
             try
             {
                 if (Session["UserId"] != null)
                 {

                     //fillValues();

                     // add therapist multiselect
                     List<StudentListViewModel> StudentList = new List<StudentListViewModel>();
                    
                    
                     string SerachBy = Request.QueryString["SerachBy"].ToString();
                     string Value = Request.QueryString["Value"].ToString();
                     if (SerachBy == "ByOSIS")
                     {
                         var result = db.SP_FindUser_ByOsis(Value).ToList();
                         foreach (var item in result) { 
                          StudentList.Add(new StudentListViewModel
                          {
                              SID = item.SID.ToString(),
                              FirstName = item.StudentFirstName,
                              LastName = item.StudentLastName,
                              OSIS = item.NYCI,
                              FundingCode = item.FundingCode
                          });
                         
                         }
                         objviewmodel.StudentList = StudentList;
                     }
                     else if (SerachBy == "Byid")
                     {
                         var result = db.SP_FindUser_ByUserID(Value).ToList();
                         foreach (var item in result)
                         {
                             StudentList.Add(new StudentListViewModel
                             {
                                 SID = item.SID.ToString(),
                                 FirstName = item.StudentFirstName,
                                 LastName = item.StudentLastName,
                                 OSIS = item.NYCI,
                                 FundingCode = item.FundingCode
                             });

                         }
                         objviewmodel.StudentList = StudentList;
                     }
                     else if (SerachBy == "ByFirst")
                     {
                         var result = db.SP_FindUser_ByFirstName(Value).ToList();
                         foreach (var item in result)
                         {
                             StudentList.Add(new StudentListViewModel
                             {
                                 SID = item.SID.ToString(),
                                 FirstName = item.StudentFirstName,
                                 LastName = item.StudentLastName,
                                 OSIS = item.NYCI,
                                 FundingCode = item.FundingCode
                             });

                         }
                         objviewmodel.StudentList = StudentList;
                     }
                     else if (SerachBy == "ByLast")
                     {
                         var result = db.SP_FindUser_ByLastName(Value).ToList();
                         foreach (var item in result)
                         {
                             StudentList.Add(new StudentListViewModel
                             {
                                 SID = item.SID.ToString(),
                                 FirstName = item.StudentFirstName,
                                 LastName = item.StudentLastName,
                                 OSIS = item.NYCI,
                                 FundingCode = item.FundingCode
                             });

                         }
                         objviewmodel.StudentList = StudentList;
                     
                     }
                 else 
                 {
                     var fiscal = DateTime.Now.Year;
                         var Month = DateTime.Now.Month;
                          //var fiscal = '@Model.Fiscal';
                         if (Month > 8) { fiscal = fiscal + 1; }

                         var result = db.SP_FindStudent_ByNPI(Value, fiscal.ToString()).ToList();
                         foreach (var item in result)
                         {
                             StudentList.Add(new StudentListViewModel
                             {
                                 SID = item.SID.ToString(),
                                 FirstName = item.StudentFirstName,
                                 LastName = item.StudentLastName,
                                 OSIS = item.NYCI,
                                 FundingCode = item.FundingCode
                             });

                         }
                         objviewmodel.StudentList = StudentList;
                     
                     }
                  
                 }
                 else
                 {
                     return RedirectToAction("log_on", "account");
                 }
             }
             catch (Exception ex)
             {
             }
             
             return View(objviewmodel);


            // return View();
         }

         public ActionResult TherapistList()
         {

             if (CheckUserLoginStatus() <= 0)
                 return AccessDeniedView();

             FindUserViewModel objviewmodel = new FindUserViewModel();
             int TID = 0;
             try
             {
                 if (Session["UserId"] != null)
                 {

                     //fillValues();

                     // add therapist multiselect
                     List<TherapistListViewModel> TherapistList = new List<TherapistListViewModel>();


                     string SerachBy = Request.QueryString["SerachBy"].ToString();
                     string Value = Request.QueryString["Value"].ToString();
                     if (SerachBy == "BySSN")
                     {
                         var result = db.SP_FindTherapist_BySSN(Value).ToList();
                         foreach (var item in result)
                         {
                             TherapistList.Add(new TherapistListViewModel
                             {
                                 TID = item.TID.ToString(),
                                 FirstName = item.FirstName,
                                 LastName = item.LastName,
                                 SSN = item.NPI,
                                 UserName = item.UserName,
                                 Email = item.Email
                             });

                         }
                         objviewmodel.TherapistList = TherapistList;
                     }
                     else if (SerachBy == "ByTID")
                     {
                         var result = db.SP_FindTherapist_ByTID(Value).ToList();
                         foreach (var item in result)
                         {
                             TherapistList.Add(new TherapistListViewModel
                             {
                                 TID = item.TID.ToString(),
                                 FirstName = item.FirstName,
                                 LastName = item.LastName,
                                 SSN = item.NPI,
                                 UserName = item.UserName,
                                 Email = item.Email
                             });

                         }
                         objviewmodel.TherapistList = TherapistList;
                     }
                     else if (SerachBy == "ByFirst")
                     {
                         var result = db.SP_FindTherapist_ByFirstName(Value).ToList();
                         foreach (var item in result)
                         {
                             TherapistList.Add(new TherapistListViewModel
                             {
                                 TID = item.TID.ToString(),
                                 FirstName = item.FirstName,
                                 LastName = item.LastName,
                                 SSN = item.NPI,
                                 UserName = item.UserName,
                                 Email = item.Email
                             });

                         }
                         objviewmodel.TherapistList = TherapistList;
                     }
                     else if (SerachBy == "ByLast")
                     {

                         var result = db.SP_FindTherapist_ByLastName(Value).ToList();
                         foreach (var item in result)
                         {
                             TherapistList.Add(new TherapistListViewModel
                             {
                                 TID = item.TID.ToString(),
                                 FirstName = item.FirstName,
                                 LastName = item.LastName,
                                 SSN = item.NPI,
                                 UserName = item.UserName,
                                 Email = item.Email
                             });

                         }
                         objviewmodel.TherapistList = TherapistList;

                     }
                     else if (SerachBy == "ByUser")
                     {

                         var result = db.SP_FindTherapist_ByUserName(Value).ToList();
                         foreach (var item in result)
                         {
                             TherapistList.Add(new TherapistListViewModel
                             {
                                 TID = item.TID.ToString(),
                                 FirstName = item.FirstName,
                                 LastName = item.LastName,
                                 SSN = item.NPI,
                                 UserName = item.UserName,
                                 Email = item.Email
                             });

                         }
                         objviewmodel.TherapistList = TherapistList;

                     }
                     else
                     {

                         var result = db.SP_FindTherapist_ByEmail(Value).ToList();
                         foreach (var item in result)
                         {
                             TherapistList.Add(new TherapistListViewModel
                             {
                                 TID = item.TID.ToString(),
                                 FirstName = item.FirstName,
                                 LastName = item.LastName,
                                 SSN = item.NPI,
                                 UserName = item.UserName,
                                 Email = item.Email
                             });

                         }
                         objviewmodel.TherapistList = TherapistList;

                     }

                  
                 }
                 else
                 {
                     return RedirectToAction("log_on", "account");
                 }
             }
             catch (Exception ex)
             {
             }

             return View(objviewmodel);


             // return View();
         }

         public ActionResult Adminlist()
         {

             if (CheckUserLoginStatus() <= 0)
                 return AccessDeniedView();

             FindUserViewModel objviewmodel = new FindUserViewModel();
             int TID = 0;
             try
             {
                 if (Session["UserId"] != null)
                 {

                     //fillValues();

                     // add therapist multiselect
                     List<TherapistListViewModel> TherapistList = new List<TherapistListViewModel>();


                     // var Result = (from n in db.tblRegistrations where n.ID.Equals(param) select n).SingleOrDefault();
                     
                       var AdminList= (from m in db.TherapistMasters where m.UserType.Equals("ADMIN") select m).ToList();
                         //var result = db.SP_FindTherapist_ByEmail(Value).ToList();
                       foreach (var item in AdminList)
                         {
                             TherapistList.Add(new TherapistListViewModel
                             {
                                 TID = item.TID.ToString(),
                                 FirstName = item.FirstName,
                                 LastName = item.LastName,
                                 UserName = item.UserName,
                                 Email = item.Email
                             });

                         }
                         objviewmodel.TherapistList = TherapistList;

                   


                 }
                 else
                 {
                     return RedirectToAction("log_on", "account");
                 }
             }
             catch (Exception ex)
             {
             }

             return View(objviewmodel);


             // return View();
         }

         [HttpPost]
         public ActionResult FindUser(FindUserViewModel objviewmodel)
         {
             string searchby = "";
             searchby = Request.Form["Finfuser"];
             string paramvalue = objviewmodel.UserType;
             string SID = "";
             try
             {
                 if (searchby == "ByOSIS")
                 {
                     var param = objviewmodel.UserType;
                     // var Result = (from n in db.tblRegistrations where n.ID.Equals(param) select n).SingleOrDefault();
                     var Result = db.SP_FindUser_ByOsis(param).FirstOrDefault();
                     if (Result != null)
                     {
                         //SID = pa.SID.ToString();
                         // Session["AdminUserId"] = Result.ID;
                         return RedirectToAction("StudentList", new { Value = param, SerachBy = searchby });
                     }
                     else
                     {
                         Response.Write("<script>alert('Invalid Id')</script>");
                     }
                 }
                 else if (searchby == "Byid")
                 {
                     var Result = db.SP_FindUser_ByUserID(paramvalue).FirstOrDefault();
                     // var Result = (from n in db.tblRegistrations where n.UserName.Equals(objviewmodel.UserType) select n).SingleOrDefault();
                     if (Result != null)
                     {
                         //SID = Result.SID.ToString();
                         //Session["AdminUserId"] = Result.ID;
                         return RedirectToAction("StudentList", new { Value = paramvalue,SerachBy = searchby });
                     }
                     else
                     {
                         Response.Write("<script>alert('Invalid Student ID')</script>");
                     }
                 }
                 else if (searchby == "ByFirst")
                 {
                     var Result = db.SP_FindUser_ByFirstName(paramvalue).FirstOrDefault();
                     // var Result = (from n in db.tblRegistrations where n.CompanyName.Equals(objviewmodel.UserType) select n).ToList();
                     if (Result != null)
                     {
                         return RedirectToAction("StudentList", new { Value = paramvalue, SerachBy = searchby });
                     }
                     else
                     {
                         Response.Write("<script>alert('Records could not find')</script>");
                     }

                 }
                 else if (searchby == "ByLast")
                 {
                     var Result = db.SP_FindUser_ByLastName(paramvalue).FirstOrDefault();
                     // var Result = (from n in db.tblRegistrations where n.LastName.Equals(objviewmodel.UserType) select n).ToList();
                     if (Result != null)
                     {   
                         return RedirectToAction("StudentList", new { Value = paramvalue, SerachBy = searchby });
                     }
                     else
                     {
                         Response.Write("<script>alert('Records could not find')</script>");
                     }

                 }
                 
                 else
                 {
                     Response.Write("<script>alert('Invalid Please Try Again.')</script>");
                 }
             }
             catch (Exception ex)
             {
                
             }

             return View(objviewmodel);
         }

         [HttpPost]
         public ActionResult FindTherapist(FindUserViewModel objviewmodel)
         {
             string searchby = "";
             searchby = Request.Form["Finfuser"];
             string paramvalue = objviewmodel.UserType;
             string SID = "";
             try
             {
                 if (searchby == "BySSN")
                 {
                     var param = objviewmodel.UserType;
                     // var Result = (from n in db.tblRegistrations where n.ID.Equals(param) select n).SingleOrDefault();
                     var Result = db.SP_FindTherapist_BySSN(param).FirstOrDefault();
                     if (Result != null)
                     {
                         //SID = pa.SID.ToString();
                         // Session["AdminUserId"] = Result.ID;
                         return RedirectToAction("TherapistList", new { Value = param, SerachBy = searchby });
                     }
                     else
                     {
                         Response.Write("<script>alert('Invalid SSN')</script>");
                     }
                 }
                 else if (searchby == "ByTID")
                 {
                     var Result = db.SP_FindTherapist_ByTID(paramvalue).FirstOrDefault();
                     // var Result = (from n in db.tblRegistrations where n.UserName.Equals(objviewmodel.UserType) select n).SingleOrDefault();
                     if (Result != null)
                     {
                         //SID = Result.SID.ToString();
                         //Session["AdminUserId"] = Result.ID;
                         return RedirectToAction("TherapistList", new { Value = paramvalue, SerachBy = searchby });
                     }
                     else
                     {
                         Response.Write("<script>alert('Invalid Therapist ID')</script>");
                     }
                 }
                 else if (searchby == "ByFirst")
                 {
                     var Result = db.SP_FindTherapist_ByFirstName(paramvalue).FirstOrDefault();
                     // var Result = (from n in db.tblRegistrations where n.CompanyName.Equals(objviewmodel.UserType) select n).ToList();
                     if (Result != null)
                     {
                         return RedirectToAction("TherapistList", new { Value = paramvalue, SerachBy = searchby });
                     }
                     else
                     {
                         Response.Write("<script>alert('Records could not find')</script>");
                     }

                 }
                 else if (searchby == "ByLast")
                 {
                     var Result = db.SP_FindTherapist_ByLastName(paramvalue).FirstOrDefault();
                     // var Result = (from n in db.tblRegistrations where n.LastName.Equals(objviewmodel.UserType) select n).ToList();
                     if (Result != null)
                     {
                         return RedirectToAction("TherapistList", new { Value = paramvalue, SerachBy = searchby });
                     }
                     else
                     {
                         Response.Write("<script>alert('Records could not find')</script>");
                     }

                 }
                 else if (searchby == "ByEmail")
                 {
                     var Result = db.SP_FindTherapist_ByEmail(paramvalue).FirstOrDefault();
                     // var Result = (from n in db.tblRegistrations where n.LastName.Equals(objviewmodel.UserType) select n).ToList();
                     if (Result != null)
                     {
                         return RedirectToAction("TherapistList", new { Value = paramvalue, SerachBy = searchby });
                     }
                     else
                     {
                         Response.Write("<script>alert('Records could not find')</script>");
                     }

                 }
                 else if (searchby == "ByUser")
                 {
                     var Result = db.SP_FindUser_ByLastName(paramvalue).FirstOrDefault();
                     // var Result = (from n in db.tblRegistrations where n.LastName.Equals(objviewmodel.UserType) select n).ToList();
                     if (Result != null)
                     {
                         return RedirectToAction("TherapistList", new { Value = paramvalue, SerachBy = searchby });
                     }
                     else
                     {
                         Response.Write("<script>alert('Records could not find')</script>");
                     }

                 }

                 else
                 {
                     Response.Write("<script>alert('Invalid Please Try Again.')</script>");
                 }
             }
             catch (Exception ex)
             {

             }

             return View(objviewmodel);
         }
         public ActionResult MandateList(int id)
         {

             if (CheckUserLoginStatus() <= 0)
                 return AccessDeniedView();
             AddStudentViewModel Objview = new AddStudentViewModel();
             
             List<MandateListViewModel> MandateList = new List<MandateListViewModel>();
             CultureInfo ci = CultureInfo.InvariantCulture;
             var SID = id.ToString();
             var result = db.SP_GetMandatListByID(SID).ToList();
             if (result != null) { 
             
              foreach (var list in result)
                {
                    
                        MandateList.Add(new MandateListViewModel
                        {
                        MandateID = list.ID.ToString(),
                        StartDate = list.ServiceStart.Value.Date.ToShortDateString(),
                        EndDate = list.ServiceEnd.Value.Date.ToShortDateString(),
                        TID = list.FirstName + " " + list.LastName,
                        FundingCode = list.FundingCode

                        });
                    
              }
              Objview.SID = id.ToString();
              var name = db.Sp_GetStudentDetail(id.ToString()).SingleOrDefault();
              if (name != null) {
                  Objview.FirstName = name.StudentFirstName + " " + name.StudentLastName;
                  Objview.NYCID = name.NYCI;
              }
             
             }
             Objview.MandateList = MandateList;
             //Objview.SID = id.ToString();
             Session["SID"] = id.ToString();
             Session["SName"] = Objview.FirstName;
             Session["OSIS"] = Objview.NYCID;
             return View(Objview);

            
         }
          public ActionResult EditMandate(int id)
         {
             if (CheckUserLoginStatus() <= 0)
                 return AccessDeniedView();
             AddStudentViewModel objView = new AddStudentViewModel();
             objView.FirstName = Session["SName"].ToString().Trim() ;
            
                var osis =  Session["OSIS"].ToString();
                var result2 = db.sp_get_StudentInfo(osis).SingleOrDefault();
                if (result2 != null) {
                    objView.SID = result2.SID.ToString();
                }
            
             objView.MID = id.ToString();
             //objView.SID = Session["SID"].ToString();
             var TID = "";
              var npi = "";
             if (id != 0)
             {

                // var result = db.SP_GetStudent_AllInfo(id.ToString()).SingleOrDefault();
                 var result = db.SP_GetMandateByID(id.ToString()).SingleOrDefault();
                 if (result != null)
                 {
                     objView.SID = result.SID.ToString();
                    
                     ///
                     objView.Fiscal = result.FiscalYear.Trim();
                     objView.FundingCode = result.FundingCode;
                     objView.SchoolCode = result.SchoolLocationCode;
                     objView.Districtcode = result.HomeDistrict;
                     objView.BoroughCode = result.BoroughCode;
                     DateTime StartDate = DateTime.Parse(result.ServiceStart.ToString());
                     objView.StartDate = StartDate.ToShortDateString();
                     DateTime EndDate = DateTime.Parse(result.ServiceEnd.ToString());
                     objView.EndDate = EndDate.ToShortDateString();
                     var Frequency = result.Frequency;
                     objView.Frequency = Frequency.Substring(0, 1).ToString();
                     objView.Frequency1 = Frequency.Substring(2, 1);
                     var Duration = result.Duration;
                     objView.Duration = Duration.Substring(0, 2).ToString();
                     objView.Duration1 = Duration.Substring(3,2).ToString();
                     var GroupSize = result.GroupSize;
                     objView.GroupSize = GroupSize.Substring(0, 1).ToString();
                     objView.GroupSize1 = GroupSize.Substring(2,1).ToString();
                     objView.language = result.Language.Trim().ToString();
                     npi = result.NPI;
                    
                     

                 }

               
             
             var result1 = db.SP_GetTherapistInfoByNPI(npi).SingleOrDefault();
             if (result1 != null)
             {
                 TID = result1.TID.ToString();

             }

             }

             List<SelectListItem> therapistSelectListItems = new List<SelectListItem>();
           
             
             foreach (TherapistMaster Therapist in db.TherapistMasters.Where(x =>x.UserType != "ADMIN"))
             {
                
                     bool ab = Regex.IsMatch(TID.ToString(), "(^|\\s)" + Therapist.TID.ToString() + "(\\s|$)");
                     //bool ab = StudentViewModel.SID.Contains(item.SID.ToString());
                     if (ab == true) { TID  = Therapist.TID.ToString(); }
                 
                     SelectListItem selectList1 = new SelectListItem()
                     {
                         Selected = (Therapist.TID.ToString() == TID),
                         Text = Therapist.FirstName + " " + Therapist.LastName,
                         Value = Therapist.TID.ToString()

                     };
                     therapistSelectListItems.Add(selectList1);
                 
             }
             
             objView.TherapistList = therapistSelectListItems;

             //ViewData["FellowList"] = FellowViewModel;
             return View(objView);

           


         }

          [HttpPost]
          public ActionResult EditMandate(AddStudentViewModel objviewmodel)
          {
              var sid = objviewmodel.SID;
              var id = objviewmodel.MID;
              var TID = "";
              var FName = "";
              var LName = "";
              var EMail = "";
              var StudFname = "";
              var StudLname = "";
              var NPI = "0";
              var message = "";
              objviewmodel.BoroughCode = (!string.IsNullOrEmpty((string)(objviewmodel.BoroughCode))) ? (string)objviewmodel.BoroughCode.Trim() : null;
              objviewmodel.SchoolCode = (!string.IsNullOrEmpty((string)(objviewmodel.SchoolCode))) ? (string)objviewmodel.SchoolCode.Trim() : null;
              objviewmodel.Districtcode = (!string.IsNullOrEmpty((string)(objviewmodel.Districtcode))) ? (string)objviewmodel.Districtcode.Trim() : null;
             
              var result = db.Sp_get_Therpist_Info(objviewmodel.TID.Substring(0,objviewmodel.TID.Length-1)).SingleOrDefault();
              if (result != null) { 
                  NPI = result.NPI;
                  FName = result.FirstName;
                  LName = result.LastName;
                
                  TID = result.TID.ToString();
                  EMail = result.Email;
              
              }
              var result1 = db.Sp_GetStudentDetail(sid).SingleOrDefault();
              StudFname = result1.StudentFirstName.TrimEnd();
              StudLname = result1.StudentLastName.TrimEnd();

              

              try
              {
                  var NewFreq = objviewmodel.Frequency;
                  var NewDura = objviewmodel.Duration;
                  var NewGroup = objviewmodel.GroupSize;
                  var Newlang = objviewmodel.language;
                  var NewStart = objviewmodel.StartDate;
                  var NewEnd = objviewmodel.EndDate;


                  if (id == "0") {
                      db.Sp_AddMandate(sid, NPI, objviewmodel.Fiscal, DateTime.Parse(objviewmodel.StartDate), DateTime.Parse(objviewmodel.EndDate),
                          objviewmodel.Frequency, objviewmodel.GroupSize, objviewmodel.Duration, objviewmodel.language, objviewmodel.FundingCode,
                          objviewmodel.BoroughCode, objviewmodel.SchoolCode, objviewmodel.Districtcode);


                      Helper.MailSendHelper.AddMandateEmail(TID, sid, NewFreq, NewGroup,
                          NewDura, Newlang, NewStart, NewEnd, FName, LName, EMail, StudFname, StudLname);


                  }
                  else
                  {
                      /// Fetch old mandate value to send email for change in mandate.
                      int IdVal = Convert.ToInt32(id);
                      var mandate = (from n in db.MandateMasters where n.ID.Equals(IdVal) select n).SingleOrDefault();
                      var OldFreq = mandate.Frequency;
                      var OldDura = mandate.Duration;
                      var OldGroup = mandate.GroupSize;
                      var OldLang = mandate.Language;
                      var OldStart = mandate.ServiceStart.Value.ToShortDateString();
                      var OldEnd = mandate.ServiceEnd.Value.ToShortDateString();
                   

                      db.SP_Update_MandateByID(sid, id, NPI, objviewmodel.Fiscal, DateTime.Parse(objviewmodel.StartDate), DateTime.Parse(objviewmodel.EndDate),
                          objviewmodel.Frequency, objviewmodel.GroupSize, objviewmodel.Duration, objviewmodel.language, objviewmodel.FundingCode,
                          objviewmodel.BoroughCode, objviewmodel.SchoolCode, objviewmodel.Districtcode);

                    

                         
                          Helper.MailSendHelper.UpdateMandateEmail(TID, sid, OldFreq, OldGroup, OldDura,OldLang,OldStart,OldEnd, NewFreq, NewGroup,
                              NewDura, Newlang, NewStart,NewEnd,FName, LName, EMail, StudFname, StudLname);
                     
                     
                     
                  }

                  message = sid;
              }
              catch (Exception ex)
              {

              }

              return Json(message);
          }


          [HttpPost]
          public ActionResult CheckOsisExist(AddStudentViewModel objviewmodel)
          {
              var sid = objviewmodel.SID.Trim();
              var Osis = objviewmodel.NYCID.Trim();
              var message = "Osis available";
              try
              {
              var result = db.SP_OSISExist(Osis).ToList();
              if (result.Count > 0) {
                 var result1 = db.sp_get_StudentInfo(Osis).SingleOrDefault();
                 if (result1.SID.ToString().Trim()!= sid) 
                 {
                     message = "OSIS number already exist!"; 
                 }
              
              
              }

              
                

                 
              }
              catch (Exception ex)
              {

              }

              return Json(message);
          }

          public ActionResult EditTherapist(int id)
          {
              if (CheckUserLoginStatus() <= 0)
                  return AccessDeniedView();

              //EditProfileViewModel objviewmodel = new EditProfileViewModel();
              RegistrationViewModel objviewmodel = new RegistrationViewModel();
              try
              {
                  int loginid = id;
                  if (loginid != null)
                  {
                      string StateName = string.Empty;

                      //loginid = Convert.ToInt32(Session["UserId"]);

                      var getReg = (from n in db.TherapistMasters where n.TID.Equals(loginid) select n).SingleOrDefault();
                      string fullName = string.Empty;
                      string Email = string.Empty;
                      if (getReg != null)
                      {
                          objviewmodel.TID = (!string.IsNullOrEmpty((string)(getReg.TID.ToString()))) ? (string)getReg.TID.ToString().Trim() : "";
                          objviewmodel.FirstName = (!string.IsNullOrEmpty((string)(getReg.FirstName))) ? (string)getReg.FirstName.Trim() : "";
                          objviewmodel.LastName = (!string.IsNullOrEmpty((string)(getReg.LastName))) ? (string)getReg.LastName.Trim() : "";
                          objviewmodel.Address1 = (!string.IsNullOrEmpty((string)(getReg.Address1))) ? (string)getReg.Address1.Trim() : "";
                          objviewmodel.Address2 = (!string.IsNullOrEmpty((string)(getReg.address2))) ? (string)getReg.address2.Trim() : "";
                          objviewmodel.StateName = (!string.IsNullOrEmpty((string)(getReg.State))) ? (string)getReg.State.Trim() : "";
                          objviewmodel.City = (!string.IsNullOrEmpty((string)(getReg.City))) ? (string)getReg.City.Trim() : "";
                          objviewmodel.UserName = (!string.IsNullOrEmpty((string)(getReg.UserName))) ? (string)getReg.UserName.Trim() : "";
                          objviewmodel.Email = (!string.IsNullOrEmpty((string)(getReg.Email))) ? (string)getReg.Email.Trim() : "";
                          objviewmodel.Phone = (!string.IsNullOrEmpty((string)(getReg.Phone))) ? (string)getReg.Phone.Trim() : "";
                          objviewmodel.SSN = (!string.IsNullOrEmpty((string)(getReg.NPI))) ? (string)getReg.NPI.Trim() : "";
                          objviewmodel.UserType = (!string.IsNullOrEmpty((string)(getReg.UserType))) ? (string)getReg.UserType.Trim() : "";
                          objviewmodel.ServiceType = (!string.IsNullOrEmpty((string)(getReg.ServiceType))) ? (string)getReg.ServiceType.Trim() : "";
                          objviewmodel.IsActive = (getReg.IsActive.ToString() == "True") ? "1" : "0";
                          objviewmodel.EndService = (!string.IsNullOrEmpty(getReg.EndService.ToString())) ? getReg.EndService.Value.ToShortDateString() : "";


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

            string msgerror = "Success";
            try
            {
                var loginid = "0";
                if (Session["UserId"] != null)
                {
                    string StateName = string.Empty;

                    loginid = objviewmodel.TID;
                    RegistrationViewModel reg = new RegistrationViewModel();

                        reg.FirstName = (!string.IsNullOrEmpty((string)(objviewmodel.FirstName))) ? (string)objviewmodel.FirstName.Trim() : "";
                        reg.LastName = (!string.IsNullOrEmpty((string)(objviewmodel.LastName))) ? (string)objviewmodel.LastName.Trim() : "";
                        reg.Address1 = (!string.IsNullOrEmpty((string)(objviewmodel.Address1))) ? (string)objviewmodel.Address1.Trim() : "";
                        reg.Address2 = (!string.IsNullOrEmpty((string)(objviewmodel.Address2))) ? (string)objviewmodel.Address2.Trim() : "";
                        reg.City = (!string.IsNullOrEmpty((string)(objviewmodel.City))) ? (string)objviewmodel.City.Trim() : "";
                        //reg.City = "New York";
                        //reg.StateName = "New York";
                        reg.StateName = (!string.IsNullOrEmpty((string)(objviewmodel.StateName))) ? (string)objviewmodel.StateName.Trim() : "";
                        reg.Email = (!string.IsNullOrEmpty((string)(objviewmodel.Email))) ? (string)objviewmodel.Email.Trim() : "";
                        reg.Phone = (!string.IsNullOrEmpty((string)(objviewmodel.Phone))) ? (string)objviewmodel.Phone.Trim() : "";
                        reg.SSN = (!string.IsNullOrEmpty((string)(objviewmodel.SSN))) ? (string)objviewmodel.SSN.TrimStart(new Char[] { '0' }) : "";
                        reg.ServiceType = (!string.IsNullOrEmpty((string)(objviewmodel.ServiceType))) ? (string)objviewmodel.ServiceType.Trim() : "";
                        reg.UserType = (!string.IsNullOrEmpty((string)(objviewmodel.UserType))) ? (string)objviewmodel.UserType.Trim() : "";
                        //reg.IsActive = "true";
                        reg.EndService = objviewmodel.EndService;
                        if (objviewmodel.IsActive == "1") { reg.IsActive = "true"; }
                        else { reg.IsActive = "false"; }
                 
                        if (!string.IsNullOrEmpty(objviewmodel.EndService) && objviewmodel.IsActive == "0")
                        {
                            DateTime EndService = Convert.ToDateTime(objviewmodel.EndService);
                            if (EndService <= DateTime.Now.Date)
                            {
                    
                           
                            var fiscal = EndService.Year;
                            var Month = EndService.Month;
                            //var fiscal = '@Model.Fiscal';
                            if (Month > 8) { fiscal = fiscal + 1; }
                            db.SetServiceEndInMandate(reg.SSN.Trim(), fiscal.ToString(), reg.EndService.Trim());
                            reg.IsActive = objviewmodel.IsActive;

                            
                                reg.IsActive = "false";
                                reg.Password = PwdSequrityManager.Encrypt("CSNYreset134!");
                          
                            db.SP_UpdateTherapistPassword(loginid.ToString(), reg.Password);    
                            }
                        }
                        //db.Sp_User_Update_profile(loginid, reg.CompanyName, reg.JobTitle, reg.FirstName, reg.LastName, reg.Country, reg.Address1,
                        //    reg.Address2, reg.City, reg.State, reg.ZipCode, reg.Email, reg.PhoneNo);
                        
                        // Response.Write("<script> alert('Updated Sucessfully')</script>");



                       var getReg1 = (from n in db.TherapistMasters where n.NPI.Equals(reg.SSN) select n).SingleOrDefault();
                       if (getReg1 != null) {
                           if (getReg1.TID.ToString() != loginid)
                           {
                               msgerror = "SSN already exist in system.";
                               // objviewmodel.Company = getReg1.CompanyName;
                           }
                           else {
                               db.SP_UpdateTherapistInfo(loginid.ToString(), reg.FirstName, reg.LastName, reg.Address1, reg.Address2, reg.City, reg.StateName, reg.Email, reg.Phone, reg.SSN, reg.ServiceType, reg.UserType, Convert.ToBoolean(reg.IsActive), Convert.ToDateTime(objviewmodel.EndService));
                               
                           }
                           }
                       
                       else {
                           db.SP_UpdateTherapistInfo(loginid.ToString(), reg.FirstName, reg.LastName, reg.Address1, reg.Address2, reg.City, reg.StateName, reg.Email, reg.Phone, reg.SSN, reg.ServiceType, reg.UserType, Convert.ToBoolean(reg.IsActive), Convert.ToDateTime(objviewmodel.EndService));
                          
                       }

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

          // For Login As Redirect to User Home page
          public ActionResult LoginToUserHome(int UserId)
          {
              var str = "";
              var username = "";
              var result = db.Sp_get_Therpist_Info(UserId.ToString()).SingleOrDefault();
              if (result != null)
              {
                  Session["UserId"] = UserId;
                  Session["Username"] = result.UserName;
                  username = result.UserName;
                  Session["Fullname"] = result.FirstName;
                  Session["SName"] = "";
                  Session["OSIS"] = "";
              }
             var roleid = db.SP_get_user_role(username).SingleOrDefault().Trim();
             if (roleid != null) { Session["UserType"] = roleid; }
              //int intVal = Convert.ToInt32(Session["UserId"]);
              //var UName = from n in db.tblRegistrations where n.ID.Equals(intVal) select n;
              //foreach (var item in UName)
              //{
              //    FormsAuthentication.SetAuthCookie(item.UserName, false);
              //}
              Session.Remove("Id");
              str = "Success";
              //return RedirectToAction("Index", "account");

              return Json(str);
          }
          // For reset password
          public ActionResult ResetPassword(int UserId)
          {
              var str = "";
              try { 
              var Password = PwdSequrityManager.Encrypt("Csnytest!23");

              db.SP_UpdateTherapistPassword(UserId.ToString(), Password);

              str = "Success";
              
            }
        catch (Exception ex){
    
       }
              return Json(str);
          }

          public ActionResult LockSession()
          {
              if (CheckUserLoginStatus() <= 0)
                  return AccessDeniedView();

              LockSessionViewModel objView = new LockSessionViewModel();
              List<TherapistLockList> TherLock = new List<TherapistLockList>();

              foreach (TherapistMaster Therapist in db.TherapistMasters)
              {
                  if (Therapist.UserType != "ADMIN")
                  {
                      var lockValue = Therapist.Lock.Substring(0, 1);
                      bool Lock = true;
                      if (lockValue == "1") { Lock = true; }
                      else {Lock = false;}
                      TherLock.Add(new TherapistLockList
                      {

                          TID = Therapist.TID.ToString(),
                          FirstName = Therapist.FirstName + " " + Therapist.LastName,
                          LockSession = Lock

                      });
                  }
              }

              objView.LockList = TherLock;

              return View(objView);
          }

          [HttpPost]
          public ActionResult UpdateLockSession(LockSessionViewModel objviewmodel)
          {
              if (CheckUserLoginStatus() <= 0)
                  return AccessDeniedView();

              var msgerror = "Success";
              try
              {
                  var loginid = "0";
                  //var tid = objviewmodel.SelectedTID;
                  //var result =  db.ResetTherapistLock(tid).ToList();
                  if (Session["UserId"] != null)
                  {

                    /// Not Selected ids
                      if (!string.IsNullOrEmpty(objviewmodel.NotSelectedTID))
                      {
                          var NotSelectedTID = objviewmodel.NotSelectedTID.Split(',');

                          foreach (var item in NotSelectedTID)
                          {
                              if (item != "")
                              {
                                  var value = db.Sp_get_Therpist_Info(item).SingleOrDefault();
                                  var lockVal = value.Lock.ToString();

                                  if (lockVal.Trim() == "1")
                                  {
                                      db.SP_Unlock(item);
                                      //db.UpdateTherapistLock(item);
                                  }
                              }
                          }
                      }
                      if (!string.IsNullOrEmpty(objviewmodel.SelectedTID))
                      {
                          var SelectedTID = objviewmodel.SelectedTID.Split(',');
                          foreach (var item in SelectedTID)
                          {
                              if (item != "")
                              {
                                  db.UpdateTherapistLock(item);
                              }
                          }


                          

                      }
                   

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
             
              return Json(msgerror);
          }

          public ActionResult LockNotify(int id)
          {
              if (CheckUserLoginStatus() <= 0)
                  return AccessDeniedView();

              //LockSessionViewModel objView = new LockSessionViewModel();
              //List<TherapistLockList> TherLock = new List<TherapistLockList>();

              int TID = id;
              db.SP_Unlock(TID.ToString());
             // db.UpdateTherapistLock(TID.ToString());
              var AdminList = "";

              var List = db.AdminList().ToList();
              foreach (var email in List) {
                  AdminList += email.Email.ToString().Trim() + ",";
              }

              var TherapistInfo = db.Sp_get_Therpist_Info(TID.ToString()).SingleOrDefault();
              CSNY_timelog.Helper.MailSendHelper.NotifyUnlockEmail(TID.ToString(), TherapistInfo.FirstName, TherapistInfo.LastName, TherapistInfo.Email.Trim(), AdminList.Substring(0,AdminList.Length -1));


              return RedirectToAction("LockSession", "admin");
          }

          public void fillValues()
          {
              try
              {



                  string StateName = string.Empty;
                  var GetStateName = (from st in db.ZipCodes orderby st.StateName select st.StateName).ToList().Distinct();
                  SelectList ShowState = new SelectList(GetStateName);
                  ViewBag.State = ShowState;
                  //if (string.IsNullOrEmpty(Session["statename"].ToString()))
                  //{
                  //    StateName = Session["statename"].ToString();
                  //}
                  //if (StateName != null)
                  //{
                  //    var CityName = (from ct in db.ZipCodes
                  //                    orderby ct.CityName
                  //                    where (ct.StateName.Equals(StateName) && ct.CityType.Equals("D"))
                  //                    select ct.CityName).ToList().Distinct();
                  //    SelectList city = new SelectList(CityName);
                  //    ViewBag.City = city;
                  //}
                  //else
                  //{
                  //    var CityName = (from ct in db.ZipCodes orderby ct.CityName where ct.StateName.Equals(StateName) select ct.CityName).ToList().Distinct();
                  //    SelectList city = new SelectList(CityName);
                  //    ViewBag.City = city;
                  //}
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


         protected string valid(OleDbDataReader myreader, int stval)//if any columns are 
         //found null then they are replaced by zero
         {
             object val = myreader[stval];
             if (val != DBNull.Value)
                 return val.ToString();
             else
                 return Convert.ToString(0);
         }

         public int CheckUserLoginStatus()
         {
             int LoginUserId = 0;
             if (Session["UserId"] != null)
             {
                 var UserType = db.Sp_get_Therpist_Info(Session["UserId"].ToString()).SingleOrDefault();
                 if (UserType.UserType == "ADMIN")
                 {
                     LoginUserId = Convert.ToInt32(Session["UserId"]);
                 }
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
