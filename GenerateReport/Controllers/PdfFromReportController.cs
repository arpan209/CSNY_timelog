using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GenerateReport.Models;
using iTextSharp.text.pdf;
using System.IO;
using GenerateReport.ViewModels;
using Ionic.Zip;
using GenerateReport.Framework;



namespace GenerateReport.Controllers
{
    public class PdfFromReportController : Controller
    {

        PdfReportCsnyEntities db = new PdfReportCsnyEntities();


        #region Method

        private ZipFile FillFormNew(int StduentId, int TherapistId, string reportType, FormGenerateViewModel model)
        {
            ZipFile zip = new ZipFile();

            //int StduentId = 390;
            var studentInformation = db.GetStudentInformation_Pdf(StduentId, reportType).SingleOrDefault();

            var therapistMaster = db.TherapistMasters.Where(t => t.TID == TherapistId).SingleOrDefault();

            var studentSessionDetailList = db.Sp_GetStudentSessionDetail_Pdf(therapistMaster.NPI.Trim(), reportType, StduentId, model.FiscalYear, model.FiscalMonth).ToList();

            if (studentSessionDetailList.Count > 0)
            {

                var groupsType = new string[] { "S1", "SP" };

                foreach (var groupType in groupsType)
                {
                  var  groupWiseStudentSessionDetailList = studentSessionDetailList.Where(s => s.MandGroupType.Trim() == groupType).ToList();
                    bool GetStudentMasterDetail = false;
                    if (groupWiseStudentSessionDetailList.Count > 0)
                    {
                        DateTime dtDate = new DateTime(model.FiscalYear, model.FiscalMonth, 1);
                        string pdfTemplate = @"D:\PdfGenCSpackage\PdfGenerator_CS\PdfGenerator_CS\PdfGenerator\BILLINGFORM.pdf";
                        pdfTemplate = Server.MapPath("~/App_Data/RSInvoicing-Template.pdf");

                        //pdfTemplate = @"D:\PdfGenCSpackage\PdfGenerator_CS\PdfGenerator_CS\PdfGenerator\completed_19e15c8d-28da-406e-ba23-17fa84d536af_BILLINGFORM.pdf";

                        string newFile = @"D:\PdfGenCSpackage\PdfGenerator_CS\PdfGenerator_CS\PdfGenerator\completed_" + Guid.NewGuid().ToString() + "_BILLINGFORM.pdf";
                        newFile = Server.MapPath("~/App_Data/PdfForm/completed_" + reportType + "_" + groupType + "_"+ Guid.NewGuid().ToString() + "_RSInvoicingTemplate.pdf");
                        newFile = Server.MapPath("~/App_Data/PdfForm/" + model.FiscalYear.ToString() + "-" + dtDate.ToString("MMM") + "-" + reportType + "-" + therapistMaster.FirstName.Trim() + "-" + therapistMaster.LastName.Trim() + "-"+groupType + ".pdf");
                        zip.Name = model.FiscalYear.ToString() + "-" + dtDate.ToString("MMM") + "-" + reportType + "-" + therapistMaster.FirstName.Trim() + "-" + therapistMaster.LastName.Trim();
                        
                        zip.AddFile(newFile,"Pdf");


                        PdfReader pdfReader = new PdfReader(pdfTemplate);
                        PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(newFile, FileMode.Create));

                        AcroFields pdfFormFields = pdfStamper.AcroFields;

                        
                        pdfFormFields.SetField("Month", dtDate.ToString("MMMM"));
                        pdfFormFields.SetField("Year", model.FiscalYear.ToString());


                        // fill student section information
                        pdfFormFields.SetField("StudentName", studentInformation.StudentLastName.Trim() + " " + studentInformation.StudentFirstName.Trim());
                        pdfFormFields.SetField("StudentNYCID", studentInformation.NYCI);
                        if (!string.IsNullOrEmpty(studentInformation.DOB))
                        {
                            DateTime DOB;
                            if (DateTime.TryParse(studentInformation.DOB, out DOB))
                            {
                                pdfFormFields.SetField("DOB_MM", DOB.Month.ToString());
                                pdfFormFields.SetField("DOB_DD", DOB.Day.ToString());
                                pdfFormFields.SetField("DOB_YEAR", DOB.Year.ToString());
                            }
                        }

                        pdfFormFields.SetField("Student_District", studentInformation.HomeDistrict);
                        pdfFormFields.SetField("Student_RelatedService", "Speech");


                        pdfFormFields.SetField("Student_Frequency", studentInformation.MandFrequency);
                        pdfFormFields.SetField("Student_Duration", studentInformation.MandDuration);
                        pdfFormFields.SetField("Student_GroupSize", studentInformation.MandGroupSize);
                        pdfFormFields.SetField("Student_Language", studentInformation.Language);

                        //pdfFormFields.SetField("Student_Location", i.ToString());
                        //pdfFormFields.SetField("Comments", i.ToString());



                        //// fill Provider section information

                        pdfFormFields.SetField("Provider_Name", therapistMaster.FirstName.Trim() + " " + therapistMaster.LastName.Trim());
                        pdfFormFields.SetField("Provider__Address1", "134 West 26th Street, Suite # 602");
                        pdfFormFields.SetField("Provider__Address2", "New York, NY 10001");
                        pdfFormFields.SetField("Provider_Telephone", "212-604-9360");
                        pdfFormFields.SetField("Provider_SSID", therapistMaster.NPI);


                        // fill Agency section information


                        pdfFormFields.SetField("AgencyName", "City Sounds of NY");
                        pdfFormFields.SetField("Address", "134 West 26th Street, Suite # 602");
                        pdfFormFields.SetField("Address2", "New York, NY 10001");
                        pdfFormFields.SetField("Address3", "");
                        pdfFormFields.SetField("Agency_Phone", "212-604-9360");
                        pdfFormFields.SetField("Federal Tax ID", "270698698");


                        // Fill Session Detail Information

                        foreach (var sessionDetail in groupWiseStudentSessionDetailList)
                        {
                            DateTime sessionDate = sessionDetail.Date.HasValue ? (DateTime)sessionDetail.Date : new DateTime(1970, 1, 1);

                            if (sessionDate.Year != 1970)
                            {
                                if (GetStudentMasterDetail == false)
                                {
                                    pdfFormFields.SetField("Student_Frequency", sessionDetail.MandFrequency.Contains(",") ? sessionDetail.MandFrequency.Split(',')[groupType=="S1"?0:1] : sessionDetail.MandFrequency);
                                    pdfFormFields.SetField("Student_Duration", sessionDetail.MandDuration.Contains(",") ? sessionDetail.MandDuration.Split(',')[groupType == "S1" ? 0 : 1] : sessionDetail.MandDuration);
                                    pdfFormFields.SetField("Student_GroupSize", sessionDetail.MandGroupSize.Contains(",") ? sessionDetail.MandGroupSize.Split(',')[groupType == "S1" ? 0 : 1] : sessionDetail.MandGroupSize);
                                    pdfFormFields.SetField("Student_Language", studentInformation.Language);
                                    pdfFormFields.SetField("Student_Location", !string.IsNullOrEmpty(sessionDetail.Location) ? sessionDetail.Location.Trim() : "");

                                    GetStudentMasterDetail = true;
                                }


                                DateTime StartTime = DateTime.Today.Add((TimeSpan)sessionDetail.StartTime);
                                DateTime EndTime = DateTime.Today.Add((TimeSpan)sessionDetail.EndTime);

                                pdfFormFields.SetField("FREQUENCY" + sessionDate.Day, "1");
                                pdfFormFields.SetField("START TIME" + sessionDate.Day, StartTime.ToString("hh:mm tt"));
                                pdfFormFields.SetField("END TIME" + sessionDate.Day, EndTime.ToString("hh:mm tt"));
                                pdfFormFields.SetField("GROUP SIZE" + sessionDate.Day, sessionDetail.GroupSize);
                            }

                        }

                        if (groupWiseStudentSessionDetailList.Count>0)
                            pdfFormFields.SetField("Total_Sessions", groupWiseStudentSessionDetailList.Count.ToString());

                        

                        // flatten the form to remove editting options, set it to false
                        // to leave the form open to subsequent manual edits
                        pdfStamper.FormFlattening = true;

                        // close the pdf
                        pdfStamper.Close();
                    }
                }
            }

            return zip;

        }



        private ZipFile CPSEFormNew(int StduentId, int TherapistId, string reportType, FormGenerateViewModel model)
        {
            ZipFile zip = new ZipFile();

            

            //int StduentId = 390;
            var studentInformation = db.GetStudentInformation_Pdf(StduentId, reportType).SingleOrDefault();

            var therapistMaster = db.TherapistMasters.Where(t => t.TID == TherapistId).SingleOrDefault();

            var studentSessionDetailList = db.Sp_GetStudentSessionDetail_Pdf(therapistMaster.NPI.Trim(), reportType, StduentId, model.FiscalYear, model.FiscalMonth).ToList();

            if (studentSessionDetailList.Count > 0)
            {

                var groupsType = new string[] { "S1", "SP" };

                foreach (var groupType in groupsType)
                {
                    var groupWiseStudentSessionDetailList = studentSessionDetailList.Where(s => s.MandGroupType.Trim() == groupType).ToList();
                    bool GetStudentMasterDetail = false;
                    if (groupWiseStudentSessionDetailList.Count > 0)
                    {
                        DateTime dtDate = new DateTime(model.FiscalYear, model.FiscalMonth, 1);

                        string pdfTemplate = @"D:\PdfGenCSpackage\PdfGenerator_CS\PdfGenerator_CS\PdfGenerator\BILLINGFORM.pdf";
                        pdfTemplate = Server.MapPath("~/App_Data/BILLINGFORM-Templeate.pdf");

                        //pdfTemplate = @"D:\PdfGenCSpackage\PdfGenerator_CS\PdfGenerator_CS\PdfGenerator\completed_19e15c8d-28da-406e-ba23-17fa84d536af_BILLINGFORM.pdf";

                        string newFile = @"D:\PdfGenCSpackage\PdfGenerator_CS\PdfGenerator_CS\PdfGenerator\completed_" + Guid.NewGuid().ToString() + "_BILLINGFORM.pdf";
                        
                        newFile = Server.MapPath("~/App_Data/PdfForm/" + model.FiscalYear.ToString() + "-" + dtDate.ToString("MMM") + "-" + reportType + "-" + therapistMaster.FirstName.Trim() + "-" + therapistMaster.LastName.Trim() +"-"+ groupType + ".pdf");
                        zip.Name = model.FiscalYear.ToString() +"-"+dtDate.ToString("MMM") + "-" + reportType + "-" + therapistMaster.FirstName.Trim() + "-" + therapistMaster.LastName.Trim();
                        
                        zip.AddFile(newFile, "Pdf");


                        PdfReader pdfReader = new PdfReader(pdfTemplate);
                        PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(newFile, FileMode.Create));

                        AcroFields pdfFormFields = pdfStamper.AcroFields;



                       
                        pdfFormFields.SetField("Month", dtDate.ToString("MMMM"));
                        pdfFormFields.SetField("Year", model.FiscalYear.ToString());

                        // set zipfileName

                        // fill student section information
                        pdfFormFields.SetField("StudentName", studentInformation.StudentLastName.Trim() + " " + studentInformation.StudentFirstName.Trim());
                        pdfFormFields.SetField("StudentNYCID", studentInformation.NYCI);
                        if (!string.IsNullOrEmpty(studentInformation.DOB))
                        {
                            DateTime DOB;
                            if (DateTime.TryParse(studentInformation.DOB, out DOB))
                            {
                                pdfFormFields.SetField("DOB_MM", DOB.Month.ToString());
                                pdfFormFields.SetField("DOB_DD", DOB.Day.ToString());
                                pdfFormFields.SetField("DOB_YEAR", DOB.Year.ToString());
                            }
                        }

                        pdfFormFields.SetField("Student_District", studentInformation.HomeDistrict);
                        pdfFormFields.SetField("Student_RelatedService", "Speech");


                        pdfFormFields.SetField("Student_Frequency", studentInformation.MandFrequency);
                        pdfFormFields.SetField("Student_Duration", studentInformation.MandDuration);
                        pdfFormFields.SetField("Student_GroupSize", studentInformation.MandGroupSize);
                        pdfFormFields.SetField("Student_Language", studentInformation.Language);

                        //pdfFormFields.SetField("Student_Location", i.ToString());
                        //pdfFormFields.SetField("Comments", i.ToString());



                        //// fill Provider section information

                        pdfFormFields.SetField("Provider_Name", therapistMaster.FirstName.Trim() + " " + therapistMaster.LastName.Trim());
                        pdfFormFields.SetField("Provider__Address1", "134 West 26th Street, Suite # 602");
                        pdfFormFields.SetField("Provider__Address2", "New York, NY 10001");
                        pdfFormFields.SetField("Provider_Telephone", "212-604-9360");
                        pdfFormFields.SetField("Provider_SSID", therapistMaster.NPI);

                       
                        // fill Agency section information


                        pdfFormFields.SetField("AgencyName", "City Sounds of NY");
                        pdfFormFields.SetField("Address", "134 West 26th Street, Suite # 602");
                        pdfFormFields.SetField("Address2", "New York, NY 10001");
                        pdfFormFields.SetField("Address3", "");
                        pdfFormFields.SetField("Agency_Phone", "212-604-9360");
                        pdfFormFields.SetField("Federal Tax ID", "270698698");

                        pdfFormFields.SetField("Agency_Rep_print_name", "Amy Grillo");


                        // Fill Session Detail Information

                        foreach (var sessionDetail in groupWiseStudentSessionDetailList)
                        {
                            DateTime sessionDate = sessionDetail.Date.HasValue ? (DateTime)sessionDetail.Date : new DateTime(1970, 1, 1);

                            if (sessionDate.Year != 1970)
                            {
                                if (GetStudentMasterDetail == false)
                                {
                                    pdfFormFields.SetField("Student_Frequency", sessionDetail.MandFrequency.Contains(",") ? sessionDetail.MandFrequency.Split(',')[groupType == "S1" ? 0 : 1] : sessionDetail.MandFrequency);
                                    pdfFormFields.SetField("Student_Duration", sessionDetail.MandDuration.Contains(",") ? sessionDetail.MandDuration.Split(',')[groupType == "S1" ? 0 : 1] : sessionDetail.MandDuration);
                                    pdfFormFields.SetField("Student_GroupSize", sessionDetail.MandGroupSize.Contains(",") ? sessionDetail.MandGroupSize.Split(',')[groupType == "S1" ? 0 : 1] : sessionDetail.MandGroupSize);
                                    pdfFormFields.SetField("Student_Language", studentInformation.Language);
                                    pdfFormFields.SetField("Student_Location", !string.IsNullOrEmpty(sessionDetail.Location) ? sessionDetail.Location.Trim() : "");

                                    GetStudentMasterDetail = true;
                                }


                                DateTime StartTime = DateTime.Today.Add((TimeSpan)sessionDetail.StartTime);
                                DateTime EndTime = DateTime.Today.Add((TimeSpan)sessionDetail.EndTime);

                                pdfFormFields.SetField("FREQUENCY" + sessionDate.Day, "1");
                                pdfFormFields.SetField("START TIME" + sessionDate.Day, StartTime.ToString("hh:mm tt"));
                                pdfFormFields.SetField("END TIME" + sessionDate.Day, EndTime.ToString("hh:mm tt"));
                                pdfFormFields.SetField("GROUP SIZE" + sessionDate.Day, sessionDetail.GroupSize);
                            }

                        }

                        if (groupWiseStudentSessionDetailList.Count > 0)
                            pdfFormFields.SetField("Total_Sessions", groupWiseStudentSessionDetailList.Count.ToString());



                        // flatten the form to remove editting options, set it to false
                        // to leave the form open to subsequent manual edits
                        pdfStamper.FormFlattening = true;

                        // close the pdf
                        pdfStamper.Close();
                    }
                }
            }

            return zip;

        }


        #endregion


        public ActionResult fillForm()
        {
            if (CheckUserLoginStatus() <= 0)
                return AccessDeniedView();

            FormGenerateViewModel model = new FormGenerateViewModel();
            return View(model);
        }

        [HttpPost]
        [ActionName("fillForm")]
        [FormNameValueRequired("CSE", "ReportType")]
        public ActionResult fillForm(FormGenerateViewModel model)
        {
            if (CheckUserLoginStatus() <= 0)
                return AccessDeniedView();

            int TID = CheckUserLoginStatus();

            if (ModelState.IsValid)
            {

                ZipFile zip = FillFormNew(model.StudentId, TID, model.ReportType, model);

                string ReadmeText = "This is a zip file dynamically generated at " + System.DateTime.Now.ToString("G");
                string filename = model.StudentId + "_" + model.ReportType + "_" + model.FiscalMonth + "_" + model.FiscalYear + "_" + TID + ".zip";

                return new ZipFileResult(zip, zip.Name+".zip");
                //return new ZipFileResult(zip, filename);
            }

            var therapistMaster = db.TherapistMasters.Where(t => t.TID == TID).SingleOrDefault();
            var StudentList = db.Sp_GetStudentListBasedOnFundingCode_Pdf(therapistMaster.NPI.Trim(), model.ReportType).ToList();
            if (StudentList.Count > 0)
            {
                model.StudentList = new List<SelectListItem>();
                foreach (var student in StudentList)
                {
                    model.StudentList.Add(new SelectListItem() { Text = student.StudentName, Value = student.SID.ToString(), Selected = model.StudentId==student.SID?true:false });
                }
            }

            return View(model);
        }

        [ActionName("fillForm")]
        [HttpPost]
        [FormNameValueRequired("CPSE", "ReportType")]
        public ActionResult fillFormCPSE(FormGenerateViewModel model)
        {
            if (CheckUserLoginStatus() <= 0)
                return AccessDeniedView();

            int TID = CheckUserLoginStatus();

            if (ModelState.IsValid)
            {

                ZipFile zip = CPSEFormNew(model.StudentId, TID, model.ReportType, model);

                string ReadmeText = "This is a zip file dynamically generated at " + System.DateTime.Now.ToString("G");
                string filename = model.StudentId + "_" + model.ReportType + "_" + model.FiscalMonth + "_" + model.FiscalYear + ".zip";

                return new ZipFileResult(zip, zip.Name+".zip");
            }

            var therapistMaster = db.TherapistMasters.Where(t => t.TID == TID).SingleOrDefault();
            var StudentList = db.Sp_GetStudentListBasedOnFundingCode_Pdf(therapistMaster.NPI.Trim(), model.ReportType).ToList();
            if (StudentList.Count > 0)
            {
                model.StudentList = new List<SelectListItem>();
                foreach (var student in StudentList)
                {
                    model.StudentList.Add(new SelectListItem() { Text = student.StudentName, Value = student.SID.ToString(), Selected = model.StudentId == student.SID ? true : false });
                }
            }

            return View(model);
        }



        public ActionResult GetStudentListBasedOnFundingCode(string ReportType)
        {

            int TID = CheckUserLoginStatus();

            var therapistMaster = db.TherapistMasters.Where(t => t.TID == TID).SingleOrDefault();

            var StudentList = db.Sp_GetStudentListBasedOnFundingCode_Pdf(therapistMaster.NPI.Trim().ToString(), ReportType);


            var result = (from s in StudentList
                          select new { id = s.SID, name = s.StudentName })
                              .ToList();

            //if (addEmptyStateIfRequired && result.Count == 0)
            //    result.Insert(0, new { id = 0, name = "Other" });

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        //
        // GET: /Test/

        public ActionResult Index()
        {
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

            return RedirectToAction("log_on", "account", new { returnUrl = this.Request.RawUrl, area="" });
        }

        public int CheckUserLoginStatus()
        {
            int LoginUserId = 0;

            if (Session["UserId"] != null)
            {               
                    LoginUserId = Convert.ToInt32(Session["UserId"]);                
            }

            //LoginUserId = 5;
            return LoginUserId;
        }

    }
}
