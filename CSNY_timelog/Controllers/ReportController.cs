using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CSNY_timelog.Models;
using CSNY_timelog.ViewModel;
using System.Windows;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using ClosedXML.Excel;
using NPOI.HSSF.Model;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;


namespace CSNY_timelog.Controllers
{
    public class ReportController : Controller
    {
        CSNY_NewEntities db = new CSNY_NewEntities();

        //
        // GET: /Report/

        public ActionResult Index()
        {
            if (CheckUserLoginStatus() <= 0)
                return AccessDeniedView();

            var Year = DateTime.Today.Year;


            return View();
        }
        public ActionResult Update_DOE()
        {
            if (CheckUserLoginStatus() <= 0)
                return AccessDeniedView();
            ReportViewModel objview = new ReportViewModel();

            List<SelectListItem> TherapistListItems = new List<SelectListItem>();
            int TherID = 0;
            var TherList = db.SP_TherapistList().ToList();

            SelectListItem selectList = new SelectListItem()
            {
                Text = "ALL",
                Value = "0"

            };
            TherapistListItems.Add(selectList);

            foreach (var Fellow in TherList)
            {

                SelectListItem selectList1 = new SelectListItem()
                {
                    Selected = (Fellow.TID == TherID),
                    Text = Fellow.LastName + " " + Fellow.FirstName,
                    Value = Fellow.TID.ToString()

                };
                TherapistListItems.Add(selectList1);

            }
            // var Result1 = TherapistListItems.Sort();

            objview.TherapistList = TherapistListItems;
            var Year = DateTime.Today.Year;


            return View(objview);
        }

        public ActionResult NIS_List()
        {
            if (CheckUserLoginStatus() <= 0)
                return AccessDeniedView();
            ReportViewModel objview = new ReportViewModel();

            List<SelectListItem> TherapistListItems = new List<SelectListItem>();
            int TherID = 0;
            var TherList = db.SP_TherapistList().ToList();

            SelectListItem selectList = new SelectListItem()
            {
                Text = "ALL",
                Value = "0"

            };
            TherapistListItems.Add(selectList);

            foreach (var Fellow in TherList)
            {

                SelectListItem selectList1 = new SelectListItem()
                {
                    Selected = (Fellow.TID == TherID),
                    Text = Fellow.LastName + " " + Fellow.FirstName,
                    Value = Fellow.TID.ToString()

                };
                TherapistListItems.Add(selectList1);

            }
            // var Result1 = TherapistListItems.Sort();

            objview.TherapistList = TherapistListItems;
            var Year = DateTime.Today.Year;


            return View(objview);
        }
        public ActionResult DOERecorded()
        {
            if (CheckUserLoginStatus() <= 0)
                return AccessDeniedView();
            ReportViewModel objview = new ReportViewModel();

            List<SelectListItem> TherapistListItems = new List<SelectListItem>();
            int TherID = 0;
            var TherList = db.SP_TherapistList().ToList();

            SelectListItem selectList = new SelectListItem()
            {
                Text = "ALL",
                Value = "0"

            };
            TherapistListItems.Add(selectList);

            foreach (var Fellow in TherList)
            {

                SelectListItem selectList1 = new SelectListItem()
                {
                    Selected = (Fellow.TID == TherID),
                    Text = Fellow.LastName + " " + Fellow.FirstName,
                    Value = Fellow.TID.ToString()

                };
                TherapistListItems.Add(selectList1);

            }
            // var Result1 = TherapistListItems.Sort();

            objview.TherapistList = TherapistListItems;
            var Year = DateTime.Today.Year;


            return View(objview);
        }

        public ActionResult Billing()
        {
            if (CheckUserLoginStatus() <= 0)
                return AccessDeniedView();
            BillingViewModel objview = new BillingViewModel();

            //List<SelectListItem> TherapistListItems = new List<SelectListItem>();
            //int TherID = 0;
            //var TherList = db.SP_TherapistList().ToList();

            //SelectListItem selectList = new SelectListItem()
            //{
            //    Text = "ALL",
            //    Value = "0"

            //};
            //TherapistListItems.Add(selectList);

            //foreach (var Fellow in TherList)
            //{

            //    SelectListItem selectList1 = new SelectListItem()
            //    {
            //        Selected = (Fellow.TID == TherID),
            //        Text = Fellow.LastName + " " + Fellow.FirstName,
            //        Value = Fellow.TID.ToString()

            //    };
            //    TherapistListItems.Add(selectList1);

            //}
            ////var Result1 = TherapistListItems.Sort();

           // objview.TherapistList = "";

            return View();
        }
        public ActionResult GetTherapistListByMonth(string Month, string Fiscal)
        {

            List<SelectListItem> TherapistListItems = new List<SelectListItem>();

            var days = DateTime.DaysInMonth(Convert.ToInt32(Fiscal), Convert.ToInt32(Month));

            var endDay = Month + "/1/" + Fiscal;


          //  var TherapistList = db.SP_TherapistListByMonthFiscal(Month,Fiscal);

            var TherapistList = db.SP_TherapistListByMonthFiscal(Month,Fiscal,endDay);



            var result = (from s in TherapistList
                          select new { id = s.TID, name = s.LastName + " " +s.FirstName })
                              .ToList();
           
            //if (addEmptyStateIfRequired && result.Count == 0)
              result.Insert(0, new { id = 0, name = "ALL" });

            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult Summary()
        {
            if (CheckUserLoginStatus() <= 0)
                return AccessDeniedView();
            ReportViewModel objview = new ReportViewModel();

          

            return View(objview);
        }

        public ActionResult InvoiceFiles()
        {
            if (CheckUserLoginStatus() <= 0)
                return AccessDeniedView();
            ReportViewModel objview = new ReportViewModel();

            List<DOEFilelist> Filelist = new List<DOEFilelist>();
            string dirPath = Server.MapPath("~/DOEFiles/").ToString();
            try
            {
                var directory = new DirectoryInfo(dirPath);
                var myFile = (from f in directory.GetFiles()
                              orderby f.LastWriteTime descending
                              select f).ToList();


                //string[] fileEntries = Directory.GetFileSystemEntries(dirPath);
                foreach (var fileName in myFile)
                {
                    if (fileName.LastWriteTime.ToString() != DateTime.Now.ToString())
                    {
                        Filelist.Add(new DOEFilelist
                        {
                            FileName = fileName.Name,
                            LastModield = fileName.LastWriteTime.ToString()


                        });

                    }
                }
                objview.Filelist = Filelist;
            }
            catch (UnauthorizedAccessException UAEx)
            {
                Console.WriteLine(UAEx.Message);
            }
            catch (PathTooLongException PathEx)
            {
                Console.WriteLine(PathEx.Message);
            }

            return View(objview);
        }

        // Insert logic for processing found files here. 
        public static void ProcessFile(string path)
        {
            Console.WriteLine("Processed file '{0}'.", path);
        }

        [HttpPost]
        public ActionResult GenerateReport(ReportViewModel objviewmodel)
        {

            if (CheckUserLoginStatus() <= 0)
                return AccessDeniedView();

            string message = "";
            var FileName = objviewmodel.AgeGroup + objviewmodel.Fiscal + objviewmodel.Month;
            //var TID = Session["UserID"].ToString();

            try
            {
                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlWorkSheet;
                object misValue = System.Reflection.Missing.Value;
                var PreCode = string.Empty;
                if (objviewmodel.AgeGroup == "CPSE") {
                    PreCode = "SRAP_";
                }
                else if (objviewmodel.AgeGroup == "CSE")
                {
                    PreCode = "RSAP_";
                }
                else { PreCode = "SIAP_"; }
                xlApp = new Excel.Application();
                
                xlWorkBook = xlApp.Workbooks.Add(misValue);
               
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                // Setup the column name
                

                xlWorkSheet.Cells[1, 1] = PreCode + "FISCAL_YR";
                if (objviewmodel.AgeGroup == "CPSE"){
                    xlWorkSheet.Cells[1, 2] = "/";
                }
                else if (objviewmodel.AgeGroup == "CSE") {
                    xlWorkSheet.Cells[1, 2] = PreCode + "BORO_CD";
                }
                
                xlWorkSheet.Cells[1, 3] = PreCode + "DIST_CD";
                xlWorkSheet.Cells[1, 4] = PreCode + "FUND_CD";
                if (objviewmodel.AgeGroup == "CPSE")
                {
                    xlWorkSheet.Cells[1, 5] = "/";
                }
                else if (objviewmodel.AgeGroup == "CSE")
                {
                    xlWorkSheet.Cells[1, 5] = PreCode + "SCHL ID";
                }
               
                xlWorkSheet.Cells[1, 6] = PreCode + "PROVIDER_TYPE";
                xlWorkSheet.Cells[1, 7] = PreCode + "AGENCY_CD";
                xlWorkSheet.Cells[1, 8] = PreCode + "PROVIDER";
                xlWorkSheet.Cells[1, 9] = "PROVIDER_LAST_NAME";
                xlWorkSheet.Cells[1, 10] = "PROVIDER_FIRST_NAME";
                xlWorkSheet.Cells[1, 11] = PreCode + "ACT_PROVIDER";
                xlWorkSheet.Cells[1, 12] = PreCode + "OSIS_ID";
                xlWorkSheet.Cells[1, 13] = "STUD_FIRST_NAME";
                xlWorkSheet.Cells[1, 14] = "STUD_LAST_NAME";
                xlWorkSheet.Cells[1, 15] = PreCode + "SERV_SUBTYPE";
                xlWorkSheet.Cells[1, 16] = PreCode + "START_DT";
                xlWorkSheet.Cells[1, 17] = PreCode + "END_DT";
                xlWorkSheet.Cells[1, 18] = PreCode + "SESSION";
                xlWorkSheet.Cells[1, 19] = PreCode + "SESS_LEN";
                xlWorkSheet.Cells[1, 20] = PreCode + "GROUP_SIZE";
                xlWorkSheet.Cells[1, 21] = PreCode + "LANG_CD";
                xlWorkSheet.Cells[1, 22] = "SCIN_INVOICE_MONTH";
                xlWorkSheet.Cells[1, 23] = "SCIN_INVOICE_DAYS";
                //// Enter by Therapist Start///
                xlWorkSheet.Cells[1, 24] = "SCIN_ATTEND_CODE";
                xlWorkSheet.Cells[1, 25] = "SCIN_ACT_GRP_SIZE";
                xlWorkSheet.Cells[1, 26] = "SCIN_START_TIME";
                xlWorkSheet.Cells[1, 27] = "SCIN_END_TIME";
                xlWorkSheet.Cells[1, 28] = "SCIN_SCHOOL_OTHER";
                /////// Enter by Therapist END ///
                xlWorkSheet.Cells[1, 29] = "/";
                xlWorkSheet.Cells[1, 30] = "SCIN_VEND_INVOICE";
                xlWorkSheet.Cells[1, 31] = "SCIN_INVOICE_AMT";
                xlWorkSheet.Cells[1, 32] = "SCIN_SED_PROG_ID";
                
                /// Add data into excel sheet  32//
                int i = 2;
                //int j = 0;
                
               var Result = db.sp_Generate_Report(objviewmodel.AgeGroup, objviewmodel.Fiscal, objviewmodel.Month,"NPI").ToList();
                foreach (var item in Result) {
                    xlWorkSheet.Cells[i, 1] = objviewmodel.Fiscal; //SRAP_FISCAL_YR
                    xlWorkSheet.Cells[i, 2] = item.BoroughCode; //SRAP_BORO_CD
                    xlWorkSheet.Cells[i, 3] = item.HomeDistrict; //SRAP_DIST_CD
                    xlWorkSheet.Cells[i, 4] = "4411"; //SIAP_FUND_CD
                    xlWorkSheet.Cells[i, 5] = item.SchoolLocationCode; //SRAP_SCHL_ID
                    xlWorkSheet.Cells[i, 6] = "A"; // "SRAP_PROVIDER_TYPE";
                    xlWorkSheet.Cells[i, 7] = "2022";// "SRAP_AGENCY_CD";
                    xlWorkSheet.Cells[i, 8] = "270698698"; // "SRAP_PROVIDER";
                    // Get therpist information 
                    //var therapistResult = db.Sp_get_Therpist_Info(item.TID).SingleOrDefault();
                    xlWorkSheet.Cells[i, 9] = item.LastName;// "PROVIDER_LAST_NAME";
                    xlWorkSheet.Cells[i, 10] = item.FirstName;// "PROVIDER_FIRST_NAME";
                    xlWorkSheet.Cells[i, 11] = item.NPI.ToString();//therapistResult.NPI; // "SRAP_ACT_PROVIDER";
                    xlWorkSheet.Cells[i, 12] = item.NYCI;  // "SRAP_OSIS_ID";
                    xlWorkSheet.Cells[i, 13] = item.StudentFirstName; // "STUD_FIRST_NAME";
                    xlWorkSheet.Cells[i, 14] = item.StudentLastName; // "STUD_LAST_NAME";
                    xlWorkSheet.Cells[i, 15] = item.MandGroupType; // "SRAP_SERV_SUBTYPE";
                    xlWorkSheet.Cells[i, 16] = item.ServiceStart; // "SRAP_START_DT";
                    xlWorkSheet.Cells[i, 17] = item.ServiceEnd; // "SRAP_END_DT";
                    int StartPos = 0;
                    int DuratStartpos = 0;
                    if(item.MandGroupType.Trim() == "SP"){
                        StartPos = 2; DuratStartpos = 3;
                    }
                    xlWorkSheet.Cells[i, 18] = item.MandFrequency.Substring(StartPos,1); // "SRAP_SESSION";
                    xlWorkSheet.Cells[i, 19] = item.MandDuration.Substring(DuratStartpos, 2); // "SRAP_SESS_LEN";
                    xlWorkSheet.Cells[i, 20] = item.MandGroupSize.Substring(StartPos, 1); // "SRAP_GROUP_SIZE";
                    var language = "";
                    //if (item.Language == "English") { language = "EN";}
                    //else if (item.Language == "Spanish") { language = "SP";}
                    //else if (item.Language =="Russian") {language = "RU";}
                    //else if (item.Language == "German") { language = "GE"; }
                    xlWorkSheet.Cells[i, 21] = item.Language; // "SRAP_LANG_CD";
                    xlWorkSheet.Cells[i, 22] = objviewmodel.Month + "/1/" + objviewmodel.Fiscal; // "SCIN_INVOICE_MONTH";
                    //var day = DateTime.Parse(objviewmodel.Month);

                    xlWorkSheet.Cells[i, 23] = item.Date; // "SCIN_INVOICE_DAYS";
                    //// Enter by Therapist Start///
                    xlWorkSheet.Cells[i, 24] = "P"; // "SCIN_ATTEND_CODE";
                    xlWorkSheet.Cells[i, 25] = item.GroupSize; // "SCIN_ACT_GRP_SIZE_1";
                    xlWorkSheet.Cells[i, 26] = item.StartTime.ToString(); // "SCIN_START_TIME";
                    xlWorkSheet.Cells[i, 27] = item.EndTime.ToString(); // "SCIN_END_TIME";
                    var location = "B";
                    if (item.Location.Trim() == "Home") { location = "H"; }
                    else if (item.Location.Trim() == "School") { location = "S"; }
                    else { location = "B"; }
                    xlWorkSheet.Cells[i, 28] = location; // "SCIN_SCHOOL_OTHER";
                    /////// Enter by Therapist END ///
                    xlWorkSheet.Cells[i, 29] = "";
                    xlWorkSheet.Cells[i, 30] = "";
                    xlWorkSheet.Cells[i, 31] = "0";
                    xlWorkSheet.Cells[i, 32] = "";

                    i += 1;
                
                }
                var noOfRows = i - 1; 
                // Formate cells
                xlWorkSheet.Range["A2", "A" + noOfRows].NumberFormat = "0000"; // Fiscal year
                xlWorkSheet.Range["B2", "B" + noOfRows].NumberFormat = "@"; // Borough Code
                xlWorkSheet.Range["C2", "C" + noOfRows].NumberFormat = "00"; // District Code
                xlWorkSheet.Range["D2", "D" + noOfRows].NumberFormat = "0000"; // Funding Code
                xlWorkSheet.Range["E2", "E" + noOfRows].NumberFormat = "000"; // Scool location Code
                xlWorkSheet.Range["F2", "F" + noOfRows].NumberFormat = "@"; // provider Type
                xlWorkSheet.Range["G2", "G" + noOfRows].NumberFormat = "0000"; // Agency Code
                xlWorkSheet.Range["H2", "H" + noOfRows].NumberFormat = "000000000"; // Vendor tax ID/SSN
                xlWorkSheet.Range["I2", "I" + noOfRows].NumberFormat = "@"; // provide Last Name
                xlWorkSheet.Range["J2", "J" + noOfRows].NumberFormat = "@"; // provide First Name
                xlWorkSheet.Range["K2", "K" + noOfRows].NumberFormat = "000000000"; // NPI/SSN
                xlWorkSheet.Range["L2", "L" + noOfRows].NumberFormat = "000000000"; // OSIS/NYCID
                xlWorkSheet.Range["M2", "M" + noOfRows].NumberFormat = "@"; // Student First Name
                xlWorkSheet.Range["N2", "N" + noOfRows].NumberFormat = "@"; // Student Last Name
                xlWorkSheet.Range["O2", "O" + noOfRows].NumberFormat = "@"; // Service Subtype
                xlWorkSheet.Range["P2", "P" + noOfRows].NumberFormat = "MM/DD/YYYY"; // Start Date
                xlWorkSheet.Range["Q2", "Q" + noOfRows].NumberFormat = "MM/DD/YYYY"; // End Date
                xlWorkSheet.Range["R2", "R" + noOfRows].NumberFormat = "0000"; // Mandate Session/Frequency
                xlWorkSheet.Range["S2", "S" + noOfRows].NumberFormat = "000"; // Mandate Duration 
                xlWorkSheet.Range["T2", "T" + noOfRows].NumberFormat = "00"; // Mandate Group Size 
                xlWorkSheet.Range["U2", "U" + noOfRows].NumberFormat = "@"; // language Code 
                xlWorkSheet.Range["V2", "V" + noOfRows].NumberFormat = "MM/DD/YYYY"; // Service Invoice MOnth 
                xlWorkSheet.Range["W2", "W" + noOfRows].NumberFormat = "MM/DD/YYYY"; // Days within Invoice Month 
                xlWorkSheet.Range["X2", "X" + noOfRows].NumberFormat = "@"; // Attendence Code
                xlWorkSheet.Range["Y2", "Y" + noOfRows].NumberFormat = "00"; // Acctual group Size
                xlWorkSheet.Range["Z2", "Z" + noOfRows].NumberFormat = "hh:mm AM/PM"; // Start Time 
                xlWorkSheet.Range["AA2", "AA" + noOfRows].NumberFormat = "hh:mm AM/PM"; // End Time 
                xlWorkSheet.Range["AB2", "AB" + noOfRows].NumberFormat = "@"; // Session Location 
                xlWorkSheet.Range["AC2", "AC" + noOfRows].NumberFormat = "@"; // Service Location 
                xlWorkSheet.Range["AD2", "AD" + noOfRows].NumberFormat = "@"; // Vendor Invooice
                xlWorkSheet.Range["AE2", "AE" + noOfRows].NumberFormat = "000000000.00"; // Vendor Invoice
                //xlWorkSheet.Columns[32].ColumnWidth = 12; 
                xlWorkSheet.Range["AF2", "AF" + noOfRows].NumberFormat = "@"; // Prog ID
                
                /////

                xlWorkBook.SaveAs(FileName, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();

                releaseObject(xlWorkSheet);
                releaseObject(xlWorkBook);
                releaseObject(xlApp);

               message = "Excel file created , you can find the file in your documents folder (Document\\csharp-Excel.xls)";


            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex);

            }
            return Json(message);
        }

        // Save pdf files in PDFFiles Folder.
        [HttpPost]
        public ContentResult UploadFiles(IEnumerable<HttpPostedFileBase> attachments)
        {


            string Name = string.Empty, Type = string.Empty, savedfilename = string.Empty;
            int Length = 0;
            foreach (var file in attachments)
            {
                // HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
                Guid guid;

                // Create and display the value of two GUIDs.
                guid = Guid.NewGuid();
                var fileName = Path.GetFileName(file.FileName);
                //var fileName = Fname + ".xlsx";
                savedfilename = fileName;
                //savedfilename = guid.ToString() + ".xls";//Save as xls
                //savedfilename = guid.ToString() + ".csv";//Save as xls
                var path = Path.Combine(Server.MapPath("~/DOEFiles/"), fileName);
                
                file.SaveAs(path);
                Length = file.ContentLength;
               
                Session["fileName"] = fileName;
                Session["filePath"] = savedfilename;
            }
            return Content("");
            // return Content("{\"name\":\"" + Name + "\",\"type\":\"" + Type + "\",\"size\":\"" + string.Format("{0}", Length) + 
            //             "\",\"savedfilename\":\"" + savedfilename + "\"}", "application/json");
        }

        public ActionResult GetLogoFileDetail()
        {
            string LogoPath = string.Empty;
            bool FileUploadError = false;
            string LogoName = string.Empty;
            FileUploadError = Convert.ToBoolean(Session["FileUploadError"]);

            if (Session["filePath"] != null)
                LogoPath = Session["filePath"].ToString();
            Session["filePath"] = null;
            LogoName = Session["fileName"].ToString(); ;
            Session.Remove("fileName");


            return Json(new { type = "pdf", savedfilename = LogoPath, Error = FileUploadError, name = LogoName }, JsonRequestBehavior.AllowGet);
        }
        protected string ReWriteFile(string Path1, string AgeGroup)//if any columns are 
        //found null then they are replaced by zero
        {
          
            string FName = Path1;

           ///
            HSSFWorkbook hssfwb;
            using (FileStream file = new FileStream(Path1, FileMode.Open, FileAccess.Read))
            {
                hssfwb = new HSSFWorkbook(file);
                file.Close();
            }


           // HSSFWorkbook wb = new HSSFWorkbook(fsIP); //Access the workbook

            var SheetName = hssfwb.GetSheetAt(0).SheetName.ToString(); ; //Access the worksheet, so that we can update / modify it.
            ISheet sheet = hssfwb.GetSheetAt(0);

           // var name = sheet.SheetName.ToString();

            //ISheet sheet = hssfwb.GetSheet("Arkusz1");
            //for (int row = 0; row <= sheet.LastRowNum; row++)
            //{
            //    if (sheet.GetRow(row) != null) //null is when the row only contains empty cells 
            //    {
            //        MessageBox.Show(string.Format("Row {0} = {1}", row, sheet.GetRow(row).GetCell(0).StringCellValue));
            //    }
            //}
            //var xlWorkBooks = new XLWorkbook(Path1);
            ////var Sheetname = "";
            //var xlWorkSheet = xlWorkBooks.Worksheet(1);
           
           // var worksheet = workbook.Worksheets.First(s => s.Name.tos);
           



            /////


                //Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                //excel.Application.Workbooks.Add(true);

                //Microsoft.Office.Interop.Excel.Workbooks xlWorkBooks = null;
                //Microsoft.Office.Interop.Excel.Workbook xlWorkBook = null;
                //object misValue = System.Reflection.Missing.Value;
                //Excel.Worksheet xlWorkSheet;
                //xlWorkBooks = excel.Workbooks;
                //xlWorkBook = xlWorkBooks.Open(FName);


               
                //xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
               // var SheetName = xlWorkSheet.Name;
                var PreCode = string.Empty;
                if (AgeGroup == "CPSE")
                {
                    PreCode = "RSAP_";
                }
                else if (AgeGroup == "CSE")
                {
                    PreCode = "SRAP_";
                }
                else { PreCode = "SIAP_"; }
                try
                {
                    //xlWorkSheet.Cell(1, 15).Value = "";
                   
                //xlWorkSheet.Cell(1, 11).Value = PreCode + "ACT_PROVIDER";
                sheet.GetRow(0).GetCell(10).SetCellValue(PreCode + "ACT_PROVIDER");
               // xlWorkSheet.Cell(1, 12).Value = PreCode + "OSIS_ID";
                sheet.GetRow(0).GetCell(11).SetCellValue(PreCode + "OSIS_ID");
                //xlWorkSheet.Cell(1, 15).Value = PreCode + "SERV_SUBTYPE";
                sheet.GetRow(0).GetCell(14).SetCellValue(PreCode + "SERV_SUBTYPE");
                //xlWorkSheet.Cell(1, 16).Value = PreCode + "START_DT";
                sheet.GetRow(0).GetCell(15).SetCellValue(PreCode + "START_DT");

                sheet.GetRow(0).GetCell(17).SetCellValue(PreCode + "SESSIONS");

                sheet.GetRow(0).GetCell(18).SetCellValue(PreCode + "SESS_LEN");

                sheet.GetRow(0).GetCell(19).SetCellValue(PreCode + "GROUP_SIZE");

                sheet.GetRow(0).GetCell(20).SetCellValue(PreCode + "LANG_CD");

                //xlWorkSheet.Cell(1, 23).Value = "SCIN_INVOICE_DAYS";
                sheet.GetRow(0).GetCell(22).SetCellValue("SCIN_INVOICE_DAYS");

                //// Enter by Therapist Start///
                //xlWorkSheet.Cell(1, 24).Value = "SCIN_ATTEND_CODE";
                sheet.GetRow(0).GetCell(23).SetCellValue("SCIN_ATTEND_CODE");
                //xlWorkSheet.Cell(1, 25).Value = "SCIN_ACT_GRP_SIZE";
                sheet.GetRow(0).GetCell(24).SetCellValue("SCIN_ACT_GRP_SIZE");
                //xlWorkSheet.Cell(1, 26).Value = "SCIN_START_TIME";
                sheet.GetRow(0).GetCell(25).SetCellValue("SCIN_START_TIME");
                //xlWorkSheet.Cell(1, 27).Value = "SCIN_END_TIME";
                sheet.GetRow(0).GetCell(26).SetCellValue("SCIN_END_TIME");
                //xlWorkSheet.Cell(1, 28).Value = "SCIN_SCHOOL_OTHER";
                sheet.GetRow(0).GetCell(27).SetCellValue("SCIN_SCHOOL_OTHER");

                using (FileStream file = new FileStream(Path1, FileMode.Open, FileAccess.Write))
                {
                    hssfwb.Write(file);
                    file.Close();
                    file.Flush();
                }
                    
                //if (System.IO.File.Exists(Path))
                //{
                //   System.IO.File.Delete(Path);
                //    //File.Delete(Application.StartupPath + "\\Preseaintake\\newtest.xls");

                //}            //if (System.IO.File.Exists(Path))
                //{
                //   System.IO.File.Delete(Path);
                //    //File.Delete(Application.StartupPath + "\\Preseaintake\\newtest.xls");

                //}
                //

               // xlWorkBooks.Save();
                //releaseObject(xlWorkBooks);
               // releaseObject(xlWorkSheet);
                //string tmpName = Path.GetTempFileName();
                //System.IO.File.Delete(tmpName);


                ////xlWorkBook.Save();
                //xlWorkBook.SaveAs(tmpName, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                //xlWorkBook.Close(true, misValue, misValue);
                /////
                //excel.Quit();
                releaseObject(sheet);
                releaseObject(hssfwb);
                
                //releaseObject(excel);
                //System.IO.File.Delete(FName);
                //System.IO.File.Move(tmpName, FName);
                
            }
            catch (Exception ex){
              
                Console.WriteLine(ex.Message);
               // con.Dispose();
            }
                return SheetName.ToString();
        }
       

        [HttpPost]
        public ActionResult GenerateReport1(ReportViewModel objviewmodel)
        {
            if (CheckUserLoginStatus() <= 0)
                return AccessDeniedView();
            
            List<SessionListNotPViewModel> SessionList = new List<SessionListNotPViewModel>();
           
            string message = "";
            List<string> MergeList = new List<string>();
           
            var FileName = objviewmodel.AgeGroup + objviewmodel.Fiscal + objviewmodel.Month;
            var TID = objviewmodel.TID;
            var filename = objviewmodel.Filename;
            var path = Path.Combine(Server.MapPath("~/DOEFiles/"), filename);
            var SheetName = ReWriteFile(path,objviewmodel.AgeGroup).ToString();
            //var SheetName = "Sheet1";
            OleDbConnection con = new OleDbConnection();
            OleDbCommand cmd = new OleDbCommand();
            OleDbCommand ocmd = new OleDbCommand();
           // string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source="+path+";Extended Properties=\"Excel 12.0;ReadOnly=False;HDR=Yes;\"";
            /////////string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=\"Excel 8.0;HDR=Yes;\"";
            //string path = @"c:\teste.xlsx";
           // string connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=\"Excel 8.0;HDR=YES\";");", path);
              string connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; Data source={0};Extended Properties=\"Excel 8.0;HDR=Yes;\"", path);
           //string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=\"Excel 8.0;ReadOnly=False;HDR=Yes;\"";

              int Frequency;
              int GroupSize;
              int Duration;

            try
            {

              
               //// //System.Data.OleDb.OleDbConnection MyConnection;
               //// //System.Data.OleDb.OleDbCommand myCommand = new System.Data.OleDb.OleDbCommand();
               //// string sql = null;
               //// //MyConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='D:\\test.xls';Extended Properties=Excel 8.0;");
               //// string SourceConstr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='D://test.xls';Extended Properties= 'Excel 8.0;HDR=Yes;IMEX=1'";
               //// OleDbConnection MyConnection = new OleDbConnection(SourceConstr);
               //// //MyConnection = new System.Data.OleDb.OleDbConnection(@"provider=Microsoft.Jet.OLEDB.4.0; Data Source=E:\Test.xls; Extended Properties=""Excel 8.0;HDR=Yes;""");
               //// MyConnection.Open();
               ////// MyConnection.Open(); 
               //// /////
               //// //string SourceConstr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + D:\\CSE_SEP1.xls + "';Extended Properties= 'Excel 8.0;HDR=Yes;IMEX=1'";

               //// //OleDbConnection conn = new OleDbConnection(SourceConstr);

               //// //// Truncate the stagging table 
               ////// db.TruncateStaging();
               ////// conn.Open();  
               //// ///
              
               //// //myCommand.Connection = MyConnection;
               //// var Result = db.sp_Generate_Report(objviewmodel.AgeGroup, objviewmodel.Fiscal, objviewmodel.Month);
               //// foreach (var item in Result) {

               ////     //sql = "Update [VendorCafsReportContracted$] set SCIN ATTEND CODE = 'P',SCIN ACT GRP SIZE ='" + item.GroupSize.Trim() + "',SCIN START TIME ='" + item.StartTime +
               ////     //    "',SCIN END TIME='" + item.EndTime + "' where SRAP ACT PROVIDER='" + item.NPI.Trim() + "' AND SRAP OSIS ID='" + item.NYCI.Trim() + "' AND SCIN INVOICE DAYS='"+ item.Date+ "'";

               ////     //sql = "Update [VendorCafsReportContracted$] set SCIN_ATTEND_CODE = 'P'" +
               ////     //  " where SRAP_ACT_PROVIDER=" + item.NPI.Trim(); // AND SRAP_OSIS_ID='" + item.NYCI.Trim() + "' AND SCIN_INVOICE_DAYS='" + item.Date + "'";

               
                CultureInfo ci = CultureInfo.InvariantCulture;
                var NPI = objviewmodel.TID.Substring(0,objviewmodel.TID.Length -1);
                // Generate report for ALL Therapist 
                if (NPI == "0") { 
                    NPI = "%";
                    
                    var Result = db.sp_Generate_Report(objviewmodel.AgeGroup, objviewmodel.Fiscal, objviewmodel.Month, NPI).ToList();
                    //var Result = db.SP_GenerateReportNotP(objviewmodel.AgeGroup, objviewmodel.Fiscal, objviewmodel.Month);
                    foreach (var item in Result)
                    {
                        //message = item.Date + " " + item.NPI + " " + item.NYCI + "<br/>";
                        message = path + "<br/>";
                        var SrNo = item.SrNo.ToString();
                        int npi = Convert.ToInt32(item.NPI.Trim());
                        int osis = Convert.ToInt32(item.NYCI.Trim());
                        var SessDate = item.Date.Value.ToOADate();
                        var StartTime = Convert.ToDateTime(item.StartTime.ToString()).ToString("h:mm tt", ci);
                        var EndTime = Convert.ToDateTime(item.EndTime.ToString()).ToString("h:mm tt", ci);
                        DateTime StartDate = Convert.ToDateTime(item.ServiceStart.ToString());
                        var location = "";
                        if (item.Location.Trim() == "School") { location = "S"; }
                        else if (item.Location.Trim() == "Home") { location = "H"; }
                        else { location = "B"; }

                        var freq = item.MandFrequency.Split(',');
                        var dura = item.MandDuration.Split(',');
                        var grop = item.MandGroupSize.Split(',');
                        var lang = item.Language;
                        if (item.MandGroupType.Trim() == "S1")
                        {

                            Frequency = Convert.ToInt32(freq[0]);
                            Duration = Convert.ToInt32(dura[0]);
                            GroupSize = Convert.ToInt32(grop[0]);
                        }
                        else {
                            Frequency = Convert.ToInt32(freq[1]);
                            Duration = Convert.ToInt32(dura[1]);
                            GroupSize = Convert.ToInt32(grop[1]);
                        }
                        //date formate
                        //var day = item.Date.Value.Day.ToString();
                        //var month = item.Date.Value.Month.ToString();

                        //if (Convert.ToInt32(day) < 10) {
                        //    day = "0" + day;
                        //}
                        //if v vf(Convert.ToInt32(month) < 10)
                        //{
                        //    month = "0" + month;
                        //}
                        ////
                        //var Sdate = day + "/" + month + "/2014";

                        //DateTime Tdate = Convert.ToDateTime(item.Date);
                        if (osis == 236655444)
                        {
                            int k = 0;
                        }

                        string selectString = "";
                        if (objviewmodel.AgeGroup == "CSE")
                        {
                            // CSE SQL
                            //selectString = "Select * from [" + SheetName + "$] where SRAP_ACT_PROVIDER='" + npi.ToString("D9") + "' AND SRAP_OSIS_ID='" + osis.ToString("D9") + "' AND SCIN_INVOICE_DAYS=" + SessDate +
                            //    " AND SRAP_SERV_SUBTYPE='" + item.MandGroupType.Trim() + "'";
                            selectString = "Select * from [" + SheetName + "$] where SRAP_ACT_PROVIDER='" + npi.ToString("D9") + "' AND SRAP_OSIS_ID='" + osis.ToString("D9") + "' AND SCIN_INVOICE_DAYS=" + SessDate +
                              " AND SRAP_SERV_SUBTYPE='" + item.MandGroupType.Trim() + "' ORDER BY SRAP_START_DT DESC";
                        }
                        else
                        {
                            //CPSE SQL
                            selectString = "Select * from [" + SheetName + "$] where RSAP_ACT_PROVIDER='" + npi.ToString("D9") + "' AND RSAP_OSIS_ID='" + osis.ToString("D9") + "' AND SCIN_INVOICE_DAYS=" + SessDate +
                               " AND RSAP_SERV_SUBTYPE='" + item.MandGroupType.Trim() + "' ORDER BY RSAP_START_DT DESC";

                        }

                        //}
                 
                        con = new OleDbConnection(connectionString);
                        ocmd = new OleDbCommand(selectString, con);

                        con.Open();
                        //ocmd.ExecuteNonQuery();
                        OleDbDataReader odr = ocmd.ExecuteReader();

                         string[] ServiceStart = new string[5];
                              string[] ServiceEnd = new string[5];
                              string[] Mandfreq = new string[5];
                              string[] Manddura = new string[5];
                              string[] Mandgrop = new string[5];
                              string[] Mandlang = new string[5];
                              int i = 0;

                              while (odr.Read())
                              {
                                  //var abc = valid(odr, -1).ToString();
                                  ServiceStart[i] = valid(odr, 15).ToString();
                                  ServiceEnd[i] = valid(odr, 16).ToString();
                                  Mandfreq[i] = valid(odr, 17).ToString();
                                  Manddura[i] = valid(odr, 18).ToString();
                                  Mandgrop[i] = valid(odr, 19).ToString();
                                  Mandlang[i] = valid(odr, 20).ToString();
                                 
                                  //NYCID = valid(odr, 2);
                                  //lname = valid(odr, 3);
                                  i += 1;
                              }
                               var CorrectStartDate = "";
                               var CorrectFreq = "";
                               var CorrectDura = "";
                               var CorrectGrop = "";
                               var CorrectLang = "";
                              int j = 0;
                              con.Close();
                              if (i > 0)
                              {
                                  if (i == 1)
                                  {
                                      CorrectStartDate = DateTime.Parse(ServiceStart[j]).ToOADate().ToString();
                                      CorrectFreq = Mandfreq[j].ToString();
                                      CorrectDura = Manddura[j].ToString();
                                      CorrectGrop = Mandgrop[j].ToString();
                                      CorrectLang = Mandlang[j].ToString();

                                  }
                                  else
                                  {

                                      for (j = 0; j < i - 1; j++)
                                      {


                                          if (DateTime.Parse(ServiceStart[j]).ToOADate() <= SessDate && DateTime.Parse(ServiceStart[j + 1]).ToOADate() > SessDate)
                                          {
                                              CorrectStartDate = DateTime.Parse(ServiceStart[j]).ToOADate().ToString();
                                              CorrectFreq = Mandfreq[j].ToString();
                                              CorrectDura = Manddura[j].ToString();
                                              CorrectGrop = Mandgrop[j].ToString();
                                              CorrectLang = Mandlang[j].ToString();


                                          }
                                          else
                                          {
                                              CorrectStartDate = DateTime.Parse(ServiceStart[j + 1]).ToOADate().ToString();
                                              CorrectFreq = Mandfreq[j].ToString();
                                              CorrectDura = Manddura[j].ToString();
                                              CorrectGrop = Mandgrop[j].ToString();
                                              CorrectLang = Mandlang[j].ToString();
                                          }

                                      }
                                  }

                                  //if (osis == 224624791 || osis == 224508259 || osis == 220723365)
                                  //{
                                  //    var abc = 0;
                                  //}


                                  if (Convert.ToInt32(CorrectFreq) == Frequency && Convert.ToInt32(CorrectDura) == Duration && Convert.ToInt32(CorrectGrop) >= GroupSize && CorrectLang == lang)
                                  {
                                      //SRAP_START_DT
                                      string UpdateString = "";
                                      if (objviewmodel.AgeGroup == "CPSE")
                                      {
                                          // string selectString = "UPDATE [Sheet1$] SET Name ='Nameddd' WHERE ID1=8";
                                          //CPSE SQL
                                          UpdateString = "Update [" + SheetName + "$] set SCIN_ATTEND_CODE = 'P',SCIN_ACT_GRP_SIZE ='" + item.GroupSize.Trim() + "',SCIN_START_TIME ='" + StartTime +
                                               "',SCIN_END_TIME='" + EndTime + "',SCIN_SCHOOL_OTHER ='" + location + "' where RSAP_ACT_PROVIDER='" + npi.ToString("D9") + "' AND RSAP_OSIS_ID='" + osis.ToString("D9") + "' AND SCIN_INVOICE_DAYS=" + SessDate +
                                               " AND RSAP_SERV_SUBTYPE='" + item.MandGroupType.Trim() + "' AND RSAP_START_DT=" + CorrectStartDate + " AND RSAP_SESSIONS=" + Frequency.ToString("D1") + " AND  RSAP_GROUP_SIZE=" + GroupSize.ToString("D3") +
                                               "AND RSAP_GROUP_SIZE=" + GroupSize.ToString("D2") + " AND RSAP_LANG_CD='" + lang + "'";
                                      }
                                      else
                                      {
                                          // CSE SQL
                                          UpdateString = "Update [" + SheetName + "$] set SCIN_ATTEND_CODE = 'P',SCIN_ACT_GRP_SIZE ='" + item.GroupSize.Trim() + "',SCIN_START_TIME ='" + StartTime +
                                            "',SCIN_END_TIME='" + EndTime + "' where SRAP_ACT_PROVIDER='" + npi.ToString("D9") + "' AND SRAP_OSIS_ID='" + osis.ToString("D9") + "' AND SCIN_INVOICE_DAYS=" + SessDate +
                                            " AND SRAP_SERV_SUBTYPE='" + item.MandGroupType.Trim() + "' AND SRAP_START_DT=" + CorrectStartDate + " AND SRAP_SESSIONS=" + Frequency.ToString("D1") + " AND  SRAP_GROUP_SIZE=" + GroupSize.ToString("D3") +
                                            "AND SRAP_GROUP_SIZE=" + GroupSize.ToString("D2") + " AND SRAP_LANG_CD='" + lang + "'";
                                      }
                                      //string val = npi.ToString("D9");
                                      //string selectString = "Update [Sheet1$] set SCIN_ACT_GRP_SIZE = '" + item.GroupSize.Trim() +"' where SRAP_OSIS_ID='" + osis.ToString("D9") + "'";
                                      //string selectString = "Update [Sheet1$] set SCIN_ATTEND_CODE = 'P' where SCIN_INVOICE_DAYS =" + SessDate;


                                      /// Updat Atend_code from P to R in Session Detail
                                      MergeList.Add(SrNo);

                                      con = new OleDbConnection(connectionString);
                                      cmd = new OleDbCommand(UpdateString, con);
                                      con.Open();
                                      var val = cmd.ExecuteNonQuery();

                                      con.Close();
                                      //myCommand.CommandText = sql;
                                  }
                              }

                   
                    }
              


                }
                else /// For individual or Selected therepist
                {
                    var TherpistList = NPI.Split(',');
                    int k = 0;
                    foreach (var therid in TherpistList) {

                        var ValNPI = db.Sp_getTherpist_NPI(TherpistList[k]).SingleOrDefault();
                    NPI = ValNPI.Trim() + "%";
                    k += 1;
                          NPI = ValNPI.Trim() + "%";


                        // Generate report 

                          var Result = db.sp_Generate_Report(objviewmodel.AgeGroup, objviewmodel.Fiscal, objviewmodel.Month, NPI).ToList();
                          //var Result = db.SP_GenerateReportNotP(objviewmodel.AgeGroup, objviewmodel.Fiscal, objviewmodel.Month);
                          foreach (var item in Result)
                          {
                              //message = item.Date + " " + item.NPI + " " + item.NYCI + "<br/>";
                              message = path + "<br/>";
                              var SrNo = item.SrNo.ToString();
                              int npi = Convert.ToInt32(item.NPI.Trim());
                              int osis = Convert.ToInt32(item.NYCI.Trim());
                              var SessDate = item.Date.Value.ToOADate();
                              var StartTime = Convert.ToDateTime(item.StartTime.ToString()).ToString("h:mm tt", ci);
                              var EndTime = Convert.ToDateTime(item.EndTime.ToString()).ToString("h:mm tt", ci);
                              DateTime StartDate = Convert.ToDateTime(item.ServiceStart.ToString());
                              var location = "";
                              if (item.Location.Trim() == "School") { location = "S"; }
                              else if (item.Location.Trim() == "Home") { location = "H"; }
                              else { location = "B"; }
                              //date formate
                              //var day = item.Date.Value.Day.ToString();
                              //var month = item.Date.Value.Month.ToString();

                              //if (Convert.ToInt32(day) < 10) {
                              //    day = "0" + day;
                              //}
                              //if (Convert.ToInt32(month) < 10)
                              //{
                              //    month = "0" + month;
                              //}
                              ////
                              //var Sdate = day + "/" + month + "/2014";

                              //DateTime Tdate = Convert.ToDateTime(item.Date);
                              if (osis == 23495640)
                              {
                                  int z = 0;

                              }


                              var freq = item.MandFrequency.Split(',');
                              var dura = item.MandDuration.Split(',');
                              var grop = item.MandGroupSize.Split(',');
                              var lang = item.Language;
                              if (item.MandGroupType.Trim() == "S1")
                              {

                                  Frequency = Convert.ToInt32(freq[0]);
                                  Duration = Convert.ToInt32(dura[0]);
                                  GroupSize = Convert.ToInt32(grop[0]);
                              }
                              else
                              {
                                  Frequency = Convert.ToInt32(freq[1]);
                                  Duration = Convert.ToInt32(dura[1]);
                                  GroupSize = Convert.ToInt32(grop[1]);
                              }


                              string selectString = "";
                              if (objviewmodel.AgeGroup == "CSE")
                              {
                                  // CSE SQL
                                  //selectString = "Select * from [" + SheetName + "$] where SRAP_ACT_PROVIDER='" + npi.ToString("D9") + "' AND SRAP_OSIS_ID='" + osis.ToString("D9") + "' AND SCIN_INVOICE_DAYS=" + SessDate +
                                  //    " AND SRAP_SERV_SUBTYPE='" + item.MandGroupType.Trim() + "'";
                                  selectString = "Select * from [" + SheetName + "$] where SRAP_ACT_PROVIDER='" + npi.ToString("D9") + "' AND SRAP_OSIS_ID='" + osis.ToString("D9") + "' AND SCIN_INVOICE_DAYS=" + SessDate +
                                    " AND SRAP_SERV_SUBTYPE='" + item.MandGroupType.Trim() + "' ORDER BY SRAP_START_DT DESC";
                              }
                              else
                              {
                                  //CPSE SQL
                                  selectString = "Select * from [" + SheetName + "$] where RSAP_ACT_PROVIDER='" + npi.ToString("D9") + "' AND RSAP_OSIS_ID='" + osis.ToString("D9") + "' AND SCIN_INVOICE_DAYS=" + SessDate +
                                     " AND RSAP_SERV_SUBTYPE='" + item.MandGroupType.Trim() + "' ORDER BY RSAP_START_DT DESC";

                              }
                              con.Close();
                              con.Dispose();
                              con = new OleDbConnection(connectionString);
                              ocmd = new OleDbCommand(selectString, con);
                              
                              con.Open();
                              //ocmd.ExecuteNonQuery();
                              OleDbDataReader odr = ocmd.ExecuteReader();

                              string[] ServiceStart = new string[5];
                              string[] ServiceEnd = new string[5];
                              string[] Mandfreq = new string[5];
                              string[] Manddura = new string[5];
                              string[] Mandgrop = new string[5];
                              string[] Mandlang = new string[5];
                              int i = 0;

                              while (odr.Read())
                              {
                                  //var abc = valid(odr, -1).ToString();
                                  ServiceStart[i] = valid(odr, 15).ToString();
                                  ServiceEnd[i] = valid(odr, 16).ToString();
                                  Mandfreq[i] = valid(odr, 17).ToString();
                                  Manddura[i] = valid(odr, 18).ToString();
                                  Mandgrop[i] = valid(odr, 19).ToString();
                                  Mandlang[i] = valid(odr, 20).ToString();
                                 
                                  //NYCID = valid(odr, 2);
                                  //lname = valid(odr, 3);
                                  i += 1;
                              }
                               var CorrectStartDate = "";
                               var CorrectEndDate = ""; 
                              var CorrectFreq = "";
                               var CorrectDura = "";
                               var CorrectGrop = "";
                               var CorrectLang = "";
                              int j = 0;
                              con.Close();
                              if (i > 0)
                              {
                                  if (i == 1)
                                  {
                                      CorrectStartDate = DateTime.Parse(ServiceStart[j]).ToOADate().ToString();
                                      CorrectEndDate = DateTime.Parse(ServiceEnd[j]).ToOADate().ToString();
                                      CorrectFreq =  Mandfreq[j].ToString();
                                      CorrectDura = Manddura[j].ToString();
                                      CorrectGrop =  Mandgrop[j].ToString();
                                      CorrectLang = Mandlang[j].ToString();

                                  }
                                  else
                                  {

                                      for (j = 0; j < i - 1; j++)
                                      {


                                          if (DateTime.Parse(ServiceStart[j]).ToOADate() <= SessDate && DateTime.Parse(ServiceEnd[j]).ToOADate() >= SessDate)
                                          {
                                              CorrectStartDate = DateTime.Parse(ServiceStart[j]).ToOADate().ToString();
                                              CorrectFreq = Mandfreq[j].ToString();
                                              CorrectDura = Manddura[j].ToString();
                                              CorrectGrop = Mandgrop[j].ToString();
                                              CorrectLang = Mandlang[j].ToString();


                                          }
                                          else { 
                                              CorrectStartDate = DateTime.Parse(ServiceStart[j + 1]).ToOADate().ToString();
                                              CorrectFreq = Mandfreq[j].ToString();
                                              CorrectDura = Manddura[j].ToString();
                                              CorrectGrop = Mandgrop[j].ToString();
                                              CorrectLang = Mandlang[j].ToString();
                                          }

                                      }
                                  }

                                  //if (osis == 224624791 || osis == 224508259 || osis == 220723365)
                                  //{
                                  //    var abc = 0;
                                  //}


                                  if (Convert.ToInt32(CorrectFreq) == Frequency && Convert.ToInt32(CorrectDura) == Duration && Convert.ToInt32(CorrectGrop) >= GroupSize && CorrectLang == lang)
                                  { 
                                  //SRAP_START_DT
                                  string UpdateString = "";

                                  ////// OLD WORKING CODE

                                  if (objviewmodel.AgeGroup == "CPSE")
                                  {
                                      // string selectString = "UPDATE [Sheet1$] SET Name ='Nameddd' WHERE ID1=8";
                                      //CPSE SQL
                                      UpdateString = "Update [" + SheetName + "$] set SCIN_ATTEND_CODE = 'P',SCIN_ACT_GRP_SIZE ='" + item.GroupSize.Trim() + "',SCIN_START_TIME ='" + StartTime +
                                           "',SCIN_END_TIME='" + EndTime + "',SCIN_SCHOOL_OTHER ='" + location + "' where RSAP_ACT_PROVIDER='" + npi.ToString("D9") + "' AND RSAP_OSIS_ID='" + osis.ToString("D9") + "' AND SCIN_INVOICE_DAYS=" + SessDate +
                                           " AND RSAP_SERV_SUBTYPE='" + item.MandGroupType.Trim() + "' AND RSAP_START_DT=" + CorrectStartDate;
                                  }
                                  else
                                  {
                                      // CSE SQL
                                      UpdateString = "Update [" + SheetName + "$] set SCIN_ATTEND_CODE = 'P',SCIN_ACT_GRP_SIZE ='" + item.GroupSize.Trim() + "',SCIN_START_TIME ='" + StartTime +
                                        "',SCIN_END_TIME='" + EndTime + "' where SRAP_ACT_PROVIDER='" + npi.ToString("D9") + "' AND SRAP_OSIS_ID='" + osis.ToString("D9") + "' AND SCIN_INVOICE_DAYS=" + SessDate +
                                        " AND SRAP_SERV_SUBTYPE='" + item.MandGroupType.Trim() + "' AND SRAP_START_DT=" + CorrectStartDate;
                                  }
                                  //string val = npi.ToString("D9");
                                  //string selectString = "Update [Sheet1$] set SCIN_ACT_GRP_SIZE = '" + item.GroupSize.Trim() +"' where SRAP_OSIS_ID='" + osis.ToString("D9") + "'";
                                  //string selectString = "Update [Sheet1$] set SCIN_ATTEND_CODE = 'P' where SCIN_INVOICE_DAYS =" + SessDate;

                                  //////////

                                  ////if (objviewmodel.AgeGroup == "CPSE")
                                  ////{
                                  ////    // string selectString = "UPDATE [Sheet1$] SET Name ='Nameddd' WHERE ID1=8";
                                  ////    //CPSE SQL
                                  ////    UpdateString = "Update [" + SheetName + "$] set SCIN_ATTEND_CODE = 'P',SCIN_ACT_GRP_SIZE ='" + item.GroupSize.Trim() + "',SCIN_START_TIME ='" + StartTime +
                                  ////         "',SCIN_END_TIME='" + EndTime + "',SCIN_SCHOOL_OTHER ='" + location + "' where RSAP_ACT_PROVIDER='" + npi.ToString("D9") + "' AND RSAP_OSIS_ID='" + osis.ToString("D9") + "' AND SCIN_INVOICE_DAYS=" + SessDate +
                                  ////         " AND RSAP_SERV_SUBTYPE='" + item.MandGroupType.Trim() + "' AND RSAP_START_DT=" + CorrectStartDate + " AND RSAP_SESSIONS='" + Frequency.ToString("D1") + "' AND  RSAP_GROUP_SIZE='" + GroupSize.ToString("D3") +
                                  ////         "' AND RSAP_GROUP_SIZE='" + GroupSize.ToString("D2") + "' AND RSAP_LANG_CD='" + lang + "'";
                                  ////}
                                  ////else
                                  ////{
                                  ////    // CSE SQL
                                  ////    UpdateString = "Update [" + SheetName + "$] set SCIN_ATTEND_CODE = 'P',SCIN_ACT_GRP_SIZE ='" + item.GroupSize.Trim() + "',SCIN_START_TIME ='" + StartTime +
                                  ////      "',SCIN_END_TIME='" + EndTime + "' where SRAP_ACT_PROVIDER='" + npi.ToString("D9") + "' AND SRAP_OSIS_ID='" + osis.ToString("D9") + "' AND SCIN_INVOICE_DAYS=" + SessDate +
                                  ////      " AND SRAP_SERV_SUBTYPE='" + item.MandGroupType.Trim() + "' AND SRAP_START_DT=" + CorrectStartDate + " AND SRAP_SESSIONS='" + Frequency.ToString("D1") + "'"; 
                                  ////    //AND  SRAP_GROUP_SIZE='" + GroupSize.ToString("D3") +
                                  ////    //  "' AND SRAP_GROUP_SIZE='" + GroupSize.ToString("D2") + "' AND SRAP_LANG_CD='" + lang + "'";
                                  ////}

                                  /// Updat Atend_code from P to R in Session Detail
                                  MergeList.Add(SrNo);

                                  con = new OleDbConnection(connectionString);
                                  cmd = new OleDbCommand(UpdateString, con);
                                  con.Open();
                                  var val = cmd.ExecuteNonQuery();

                                  con.Close();
                                  //myCommand.CommandText = sql;
                                  }
                              }


                          }

                    }
                }
              
                //con.Close();
                foreach (var item in MergeList)
                {

                    db.UpadteSessionAsRecorded(item.ToString());
                }
                var DataValue = objviewmodel.AgeGroup + " " + objviewmodel.Fiscal + " " + objviewmodel.Month;
               message = "Success" + "," + filename;
                //message = "Download the file from here: <a href=" + path + " download> Download Original Uploaded Document </a>";
               // List<Sess> SessionListNotP = SessionList.Cast<SessionListNotPViewModel>().ToList();
                
               //objviewmodel.SessionListNotP = SessionList;

            }
              
            


            catch (Exception ex)
            {
               
                //message += ex.Message ;
                message += ex.InnerException;
                message += "new error";
                message += ex.StackTrace;
                message += "new error";
                message += ex.Message;
                Console.WriteLine(ex.Message);
                con.Dispose();
            }
            finally
            {
                
                con.Dispose();
               
            }
            return Json(message);
        }
        public ActionResult SessionListNotRecorded(string id)

        {
            if (CheckUserLoginStatus() <= 0)
                return AccessDeniedView();

            ReportViewModel ObjView = new ReportViewModel();
            List<SessionListNotPViewModel> SessionNotPList = new List<SessionListNotPViewModel>();

            var value = id.Split(' ');
            //objviewmodel.AgeGroup + " " + objviewmodel.Fiscal + " " + objviewmodel.Month;
            var AgeGroup = value[0];
            var Fiscal = value[1];
            var Month = value[2];
            var NPI = value[3];
           // ObjView.
            if (NPI == "0") { NPI = "%"; }
            else
            {
                var NPIVal = db.Sp_getTherpist_NPI(NPI).SingleOrDefault();
                NPI = NPIVal.Trim() + "%";
            }
            var result = db.sp_Generate_Report(AgeGroup, Fiscal, Month,NPI).ToList();
            ObjView.Count = result.Count().ToString();

            if (result != null) { 
            
                foreach(var item in result){

                    SessionNotPList.Add(new SessionListNotPViewModel
                    {
                        SessionId = item.SessionID,
                        SrNo = item.SrNo,
                        SessDate = item.Date.Value.Date,
                        StudName = item.StudentFirstName + " " + item.StudentLastName,
                        TherName = item.FirstName + " " + item.LastName,
                        FundingCode = AgeGroup,
                       
                        //SName = list.StudentFirstName + " " + list.StudentLastName,
                     

                    });
                        

                }

                ObjView.SessionListNotP = SessionNotPList;
                ObjView.DataValue = id.ToString();
            }


          //  var SessionNotP = (List<SessionListNotPViewModel>)TempData["SessionNotP"];// TempData["SessionNotP"] as List<string>;
           // List<String> = List1 = TempData.Cast<String>().ToL
            //List<SessionListNotPViewModel> errorList = TempData["SessionNotP"].Cast<SessionListNotPViewModel>().ToList();
            //TempData["ErrorArry"] = errorList;

           //for (var i = 0; i < SessionNotP.Count; i = i + 4){
            
           // SessionNotPList.Add(new SessionListNotPViewModel
           //     {
           //     SessID = SessionNot,
           //     SID = Int32.Parse(SessionNotP[i+1]),
           //     TID = Int32.Parse(SessionNotP[i+2]),
           //     SessDate = DateTime.Parse(SessionNotP[i+3])
                
                              
           //     });
                

            
           // }

           //ObjView.SessionListNotP = SessionNotP;
            return View(ObjView);
        }

        public ActionResult SessionListRecorded(string id)
        {
            if (CheckUserLoginStatus() <= 0)
                return AccessDeniedView();

            ReportViewModel ObjView = new ReportViewModel();
            List<SessionListNotPViewModel> SessionNotPList = new List<SessionListNotPViewModel>();

            var value = id.Split(' ');
            //objviewmodel.AgeGroup + " " + objviewmodel.Fiscal + " " + objviewmodel.Month;
            var AgeGroup = value[0];
            var Fiscal = value[1];
            var Month = value[2];
            var NPI = value[3];
            if (NPI == "0") { NPI = "%"; }
            else
            {
                var NPIVal = db.Sp_getTherpist_NPI(NPI).SingleOrDefault();
                NPI = NPIVal.Trim() + "%";
            }
            var result = db.SP_GenerateReportAsR(AgeGroup, Fiscal, Month,NPI).ToList();
            ObjView.Count = result.Count().ToString();

            if (result != null)
            {

                foreach (var item in result)
                {

                    SessionNotPList.Add(new SessionListNotPViewModel
                    {
                        SessionId = item.SessionID,
                        SrNo = item.SrNo,
                        SessDate = item.Date.Value.Date,
                        StudName = item.StudentFirstName + " " + item.StudentLastName,
                        TherName = item.FirstName + " " + item.LastName,
                        FundingCode = AgeGroup,
                        //SName = list.StudentFirstName + " " + list.StudentLastName,


                    });


                }

                ObjView.SessionListNotP = SessionNotPList;
                ObjView.DataValue = id.ToString();
            }


            //     });



            // }

            //ObjView.SessionListNotP = SessionNotP;
            return View(ObjView);
        }

        public ActionResult SummaryList(string id)
        {
            if (CheckUserLoginStatus() <= 0)
                return AccessDeniedView();

            ReportViewModel ObjView = new ReportViewModel();
            List<SummaryListViewModel> SumamryList = new List<SummaryListViewModel>();

            var value = id.Split(' ');
            //objviewmodel.AgeGroup + " " + objviewmodel.Fiscal + " " + objviewmodel.Month;
            var AgeGroup = value[0];
            var Fiscal = value[1];
            var Month = value[2];
            int RTotal = 0;
            int PTotal = 0;
            var NPI = "%";
            var TherList = db.SP_GetTherapistListByFCode(AgeGroup, Fiscal, Month).ToList();
           // var result = db.sp_Generate_Report(AgeGroup, Fiscal, Month, NPI).ToList();
           // ObjView.Count = result.Count().ToString();

            if (TherList != null)
            {

                foreach (var item in TherList)
                {
                    var TID = item.TID.ToString();
                    var RCount = db.SP_GetSessionCount(AgeGroup, Fiscal, Month, item.NPI.Trim(), "R").SingleOrDefault();
                    var PCount = db.SP_GetSessionCount(AgeGroup, Fiscal, Month, item.NPI.Trim(), "P").SingleOrDefault();
                    var Invoice = (from n in db.InvoiceDetails where n.TID == TID && n.Fiscal == Fiscal && n.month == Month && n.FundingCode == AgeGroup select n).SingleOrDefault();
                    int ID = 0;
                    var InvoiceNo = "0";
                    var Note = "";
                    if (Invoice != null) { ID = Invoice.ID; InvoiceNo = Invoice.InvoiceNo; Note = Invoice.Note;}
                    SumamryList.Add(new SummaryListViewModel
                    {
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        RCount = !string.IsNullOrEmpty(RCount.ToString()) ? Convert.ToInt32(RCount) : 0,
                        PCount = !string.IsNullOrEmpty(PCount.ToString()) ? Convert.ToInt32(PCount) : 0,
                        TID = item.TID.ToString(),
                        Invoice = InvoiceNo.Trim(),
                        ID = ID,
                        Note = Note,

                        //SName = list.StudentFirstName + " " + list.StudentLastName,


                    });
                    RTotal += Convert.ToInt32(RCount);
                    PTotal += Convert.ToInt32(PCount);
                }

                ObjView.SummaryList = SumamryList;
                ObjView.Month = Month;
                ObjView.Fiscal = Fiscal;
                ObjView.AgeGroup = AgeGroup;
                ObjView.RTotal = RTotal;
                ObjView.PTotal = PTotal;
                //ObjView.DataValue = id.ToString();
            }


            //  var SessionNotP = (List<SessionListNotPViewModel>)TempData["SessionNotP"];// TempData["SessionNotP"] as List<string>;
            // List<String> = List1 = TempData.Cast<String>().ToL
            //List<SessionListNotPViewModel> errorList = TempData["SessionNotP"].Cast<SessionListNotPViewModel>().ToList();
            //TempData["ErrorArry"] = errorList;

            //for (var i = 0; i < SessionNotP.Count; i = i + 4){

            // SessionNotPList.Add(new SessionListNotPViewModel
            //     {
            //     SessID = SessionNot,
            //     SID = Int32.Parse(SessionNotP[i+1]),
            //     TID = Int32.Parse(SessionNotP[i+2]),
            //     SessDate = DateTime.Parse(SessionNotP[i+3])


            //     });


             
            // }

            //ObjView.SessionListNotP = SessionNotP;
            return View(ObjView);
        }

        public ActionResult EditInvoice(string id)
        {
           
            ReportViewModel ObjView = new ReportViewModel();
            List<SessionListNotPViewModel> SessionNotPList = new List<SessionListNotPViewModel>();

            var value = id.Split(',');
            int InvoiceId = Convert.ToInt32(value[0]);
            var AgeGroup = value[1];
            var Fiscal = value[2];
            var Month = value[3];
            var NPI = value[4];
            //if (NPI == "0") { NPI = "%"; }
            //else
            //{
            //    var NPIVal = db.Sp_getTherpist_NPI(NPI).SingleOrDefault();
            //    NPI = NPIVal.Trim() + "%";
            //}
            //var result = db.SP_GenerateReportAsR(AgeGroup, Fiscal, Month, NPI).ToList();
            //ObjView.Count = result.Count().ToString();

            var result1 = (from n in db.InvoiceDetails where n.ID == InvoiceId select n).SingleOrDefault();
            var result = db.Sp_get_Therpist_Info(NPI).SingleOrDefault();
                ObjView.InvoiceID = InvoiceId.ToString();
                ObjView.TID = NPI;
                ObjView.Filename = result.LastName.Trim() + " " + result.FirstName.Trim();
                ObjView.Fiscal =Fiscal;
                ObjView.Month = Month;
                ObjView.AgeGroup = AgeGroup;
                
                 if (result1 != null)
            {
                ObjView.Invoice = (result1.InvoiceNo.Trim() != "0") ? result1.InvoiceNo.Trim() :"0" ;
                ObjView.Note = result1.Note;
            }

            //ObjView.SessionListNotP = SessionNotP;
            return View(ObjView);
        }

        [HttpPost]
        public ActionResult EditInvoice(ReportViewModel objviewmodel)
        {
            if (CheckUserLoginStatus() <= 0)
                return AccessDeniedView();
            var msg = "";
            var Invoice = !string.IsNullOrEmpty(objviewmodel.Invoice) ? objviewmodel.Invoice : "0";
            if (Convert.ToInt32(objviewmodel.InvoiceID) > 0)
            {

                db.UpdateInvocie(objviewmodel.InvoiceID.ToString(), Invoice, objviewmodel.Note);
                msg = "Success";


            }
            else {
                db.CreateInvoice(objviewmodel.TID, objviewmodel.AgeGroup, objviewmodel.Fiscal, objviewmodel.Month, Invoice, objviewmodel.Note);
                msg = "Success";
            }


            return Json(msg);

        }
        [HttpPost]
        public ActionResult MergeSession(ReportViewModel objviewmodel)
        {
            if (CheckUserLoginStatus() <= 0)
                return AccessDeniedView();
            var SessionList = objviewmodel.SrNo.Split(',');
            var message = "";
            foreach (var item in SessionList) { 
            message = "Submit Successfully!";

            var result = db.UpadteSessionAsRecorded(item);
            }
            message += "," + objviewmodel.DataValue;
            return Json(message);

        }
        [HttpPost]
        public ActionResult UnMergeSession(ReportViewModel objviewmodel)
        {
            if (CheckUserLoginStatus() <= 0)
                return AccessDeniedView();

            var SessionList = objviewmodel.SrNo.Split(',');
            var message = "";
            foreach (var item in SessionList)
            {
                message = "Submit Successfully!";
                var result = db.UpadteSessionAsNotRecorded(item);
            }
            message += "," + objviewmodel.DataValue;
            return Json(message);

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

        public ActionResult MasterCheckForm()
        {

            if (CheckUserLoginStatus() <= 0)
                return AccessDeniedView();

        return View();

        }

        public ActionResult MasterCheck(string id)
        {
            if (CheckUserLoginStatus() <= 0)
                return AccessDeniedView();
             BillingViewModel objview = new BillingViewModel();
             var msg = "";
            try
            {
               
                var dataVal = id.Split(',');

                var Year = dataVal[0];
                var month = dataVal[1];
                objview.Fiscal = Year;
                objview.Month = month;
                var days = DateTime.DaysInMonth(Convert.ToInt32(Year), Convert.ToInt32(month));
                var startDay = month + "/" + 1 + "/" + Year;
                var endDay = month + "/" + days + "/" + Year;

                int Fiscal = Convert.ToInt32(Year);
                if (Convert.ToInt32(month) > 8) { Fiscal = Fiscal + 1; }
                List<MastercheckLsit> MasterList = new List<MastercheckLsit>();

                var therapistList = db.SP_TherapistList();
                string[] FundingCode = { "CSE", "CPSE", "PP", "PI", "RSA", "EI", "SAS", "CS", "Other" };
                foreach (var item in therapistList)
                {

                    msg = item.LastName + "," + item.FirstName;
                    var tid = item.TID.ToString();
                    int CountBilling = 0;
                    int CountBilling1 = 0;
                    var SessionCount = 0;
                    int Count = 0;
                    if (!string.IsNullOrEmpty(item.NPI) || item.NPI != "0")
                    {
                        var npi = item.NPI.Trim();
               
                     SessionCount = db.BillingSession(startDay,   endDay, tid.ToString()).Count();
                    foreach (var FCode in FundingCode)
                    {
                        if (FCode == "CSE" || FCode == "CPSE" || FCode == "PP" || FCode == "PI" || FCode == "RSA" || FCode == "EI")
                        {
                            var BillingFcode = db.SP_SessCountByFuningCode(startDay, endDay, tid, FCode, npi).Count();
                            var BillingFcode1 = db.SP_BillingNew(Fiscal.ToString(), startDay, endDay, tid, FCode, npi).Count();
                            //Count = BillingFcode ? BillingFcode.ToString() : "0");
                            CountBilling += BillingFcode;
                            CountBilling1 += BillingFcode1;
                        }
                        else
                        {

                            var billingService = db.SP_SessCountByServiceType(startDay, endDay, tid, FCode).Count();
                            //Count = Convert.ToInt32(!string.IsNullOrEmpty(billingService.ToString()) ? billingService.ToString() : "0");
                            CountBilling += billingService;
                            CountBilling1 += billingService;

                        }
                    }
                    }
                    int year1 = Convert.ToInt32(Fiscal);
                    int month1 = Convert.ToInt32(month);

                    var Rs = (from n in db.SessionDetails where n.TID == tid && n.Date.Value.Month == month1 && n.Date.Value.Year == year1 && n.AttnCode == "R" select n).Count();
                    var RCount = !string.IsNullOrEmpty(Rs.ToString()) ? Rs.ToString() : "0";
                    var Ps = (from n in db.SessionDetails where n.TID == tid && n.Date.Value.Month == month1 && n.Date.Value.Year == year1 && n.AttnCode == "P" select n).Count();
                    var PCount = !string.IsNullOrEmpty(Ps.ToString()) ? Ps.ToString() : "0";


                    MasterList
                       .Add(new MastercheckLsit
                    {
                        Therapist = item.LastName + "," + item.FirstName,
                        Session = SessionCount,
                        Billing = CountBilling,
                        BillingNew = CountBilling1,
                        Rs = Convert.ToInt32(RCount),
                        Ps = Convert.ToInt32(PCount),

                    });
                }

                objview.MasterCheck = MasterList;
            }
            catch (Exception ex)
            {
                var val = msg;
                Console.WriteLine(ex.Message);
}
            return View(objview);
        }

        public ActionResult MasterCheck1(string id)
        {
            if (CheckUserLoginStatus() <= 0)
                return AccessDeniedView();
            BillingViewModel objview = new BillingViewModel();
            var msg = "";
            try
            {

                var dataVal = id.Split(',');

                var Year = dataVal[0];
                var month = dataVal[1];
                var FCode = dataVal[2];

                objview.Fiscal = Year;
                objview.Month = month;
                objview.AgeGroup = FCode;

                var days = DateTime.DaysInMonth(Convert.ToInt32(Year), Convert.ToInt32(month));
                var startDay = month + "-" + 1 + "-" + Year;
                var endDay = month + "-" + days + "-" + Year;
                objview.StartDate = startDay;
                objview.EndDate = endDay;
                int Fiscal = Convert.ToInt32(Year);
                if (Convert.ToInt32(month) > 8) { Fiscal = Fiscal + 1; }
                List<MastercheckLsit1> MasterList1 = new List<MastercheckLsit1>();
                int count = 0;
                var therapistList = db.SP_TherapistList();
               // string[] FundingCode = { "CSE", "CPSE", "PP", "PI", "RSA", "EI", "SAS", "CS", "Other" };
                foreach (var item in therapistList)
                {
                    msg = item.LastName + "," + item.FirstName;
                    var tid = item.TID.ToString();
                    int CountBilling = 0;
                    int CountBilling1 = 0;
                    int Count = 0;
                   
                    if (!string.IsNullOrEmpty(item.NPI) || item.NPI != "0")
                    {
                        var npi = item.NPI.Trim();

                        if (FCode == "CSE" || FCode == "CPSE" || FCode == "PP" || FCode == "PI" || FCode == "RSA" || FCode == "EI")
                        {
                            // var BillingFcode = db.SP_SessCountByFuningCode(startDay, endDay, tid, FCode, npi).ToList();
                            var BillingFcode1 = db.SP_BillingNew(Fiscal.ToString(), startDay, endDay, tid, FCode, npi).Count();
                            //Count = BillingFcode ? BillingFcode.ToString() : "0");
                            //CountBilling += BillingFcode;
                            CountBilling1 += BillingFcode1;

                            MasterList1.Add(new MastercheckLsit1
                            {
                                TID = item.TID.ToString(),
                                TName = item.LastName + "," + item.FirstName,
                                Fcode = FCode,
                                Group = BillingFcode1.ToString(),
                            });

                        }
                        else
                        {

                            var billingService = db.SP_SessCountByServiceType(startDay, endDay, tid, FCode).Count();
                            //Count = Convert.ToInt32(!string.IsNullOrEmpty(billingService.ToString()) ? billingService.ToString() : "0");
                            CountBilling1 += billingService;
                            MasterList1.Add(new MastercheckLsit1
                            {
                                TID = item.TID.ToString(),
                                TName = item.LastName + "," + item.FirstName,
                                Fcode = FCode,
                                Group = billingService.ToString(),
                            });


                        }
                    }
                    else
                    {
                       
                        MasterList1.Add(new MastercheckLsit1
                        {
                            TID = item.LastName + "," + item.FirstName + ", NO NPI",
                            Fcode = FCode,
                            Group = "0",
                        });

                    }
                    int year1 = Convert.ToInt32(Fiscal);
                    int month1 = Convert.ToInt32(month);

                    //var Rs = (from n in db.SessionDetails where n.TID == tid && n.Date.Value.Month == month1 && n.Date.Value.Year == year1 && n.AttnCode == "R" select n).Count();
                    //var RCount = !string.IsNullOrEmpty(Rs.ToString()) ? Rs.ToString() : "0";
                    //var Ps = (from n in db.SessionDetails where n.TID == tid && n.Date.Value.Month == month1 && n.Date.Value.Year == year1 && n.AttnCode == "P" select n).Count();
                    //var PCount = !string.IsNullOrEmpty(Ps.ToString()) ? Ps.ToString() : "0";
                    count += (CountBilling + CountBilling1);

                }

                objview.MasterCheck1 = MasterList1;
                objview.MasterCount = count.ToString();
            }
            catch (Exception ex)
            {
                var val = msg;
                Console.WriteLine(ex.Message);
            }
            return View(objview);
        }




        public ActionResult BillingInformation(string id)
        {

            if (CheckUserLoginStatus() <= 0)
                return AccessDeniedView();

            var dataVal = id.Split(',');
           
            var TID = dataVal[0];
            
            var month = dataVal[1];
           
            
           
            var dtf = CultureInfo.CurrentCulture.DateTimeFormat;
            string monthName = dtf.GetMonthName(Convert.ToInt32(month)); //DateTimeFormatInfo.CurrentInfo.GetMonthName(Convert.ToInt32(dtf));

            var Half = "First half - " + monthName;
            var Year = dataVal[4];
            int fiscal = Convert.ToInt32(Year);
            if (Convert.ToInt32(month) > 8) { fiscal = fiscal + 1; }

            var days = DateTime.DaysInMonth(Convert.ToInt32(Year),Convert.ToInt32(month));
            var startDay = month+ "/"+ dataVal[2] + "/" + Year;
            var endDay = month + "/" + dataVal[3] + "/" + Year;
            if ( Convert.ToInt32(dataVal[3]) > 16) {
                endDay = month + "/" + days + "/" + Year;
            Half = "Second half - " + monthName;
            }
            if (Convert.ToInt32(dataVal[2]) == 1 && Convert.ToInt32(dataVal[3]) == 31)
            {
                endDay = month + "/" + days + "/" + Year;
                Half = "Full Month - " + monthName;  }
            Session["Half"] = Half;
            int MasterCount = 0;
            var MasterTotalValue = "";
            var MasterTotal = new TimeSpan();
            
            // var TID = Session["UserID"].ToString();
            //var npi = db.Sp_getTherpist_NPI(TID).SingleOrDefault();
            //var StudMendtate = (from n in db.MandateMasters where n.NPI == TID && n.SID == sid select n).fit();
            string[] FundingCode = {"CSE","CPSE","PP","PI","RSA","EI","SAS","CS","Other"};
            int j = 0; // Count for Funding Code
            BillingViewModel objview = new BillingViewModel();
            Session["TID"] = TID;
            Session["StartDate"] = startDay;
            Session["EndDate"] = endDay;
            var Thername = "";
            var NPI = "";
            if (TID == "0") {

                Thername = "ALL";
               // TID = "%";
                NPI = "%";
                
            }
            else
            {
                var TherResult = db.Sp_get_Therpist_Info(TID).SingleOrDefault();
               
                if (TherResult != null)
                {

                    Thername = TherResult.LastName + " " + TherResult.FirstName;

                }
               // TID = TID;
               NPI = TherResult.NPI.Trim();
            }
            Session["Thername"] = Thername;
            List<SessionCountList> SessionCount = new List<SessionCountList>();
            CultureInfo ci = CultureInfo.InvariantCulture;
            foreach (string item in FundingCode)
            {
                int ENCount = 0;
                int SPCount = 0;
                int RUCount = 0;
                int POCount = 0;
                int totalCount = 0;
                var total = new TimeSpan();
                var ENTotal = new TimeSpan();
                var SPTotal = new TimeSpan();
                var RUTotal = new TimeSpan();
                var POTotal = new TimeSpan();
                int groupsize = 0;
                TimeSpan Duration1 = new TimeSpan();
                TimeSpan Duration2 = new TimeSpan();

               
                //var duration = new TimeSpan();
                //var Result =new  List<CSNY_timelog.Models.SP_SessCountByFuningCode_Result>();
                //var Result1 = new List<CSNY_timelog.Models.SP_SessCountByServiceType_Result>();
                //var Result = "";
                var FCode = item.ToString();
                if (FCode == "CSE" || FCode == "CPSE" || FCode == "PP" || FCode == "PI" ||FCode == "RSA" || FCode == "EI" )
                {

                  // var Result = db.SP_SessCountByFuningCode(startDay, endDay, TID, FCode, NPI).ToList();
                    var Result = db.SP_BillingNew(fiscal.ToString(), startDay, endDay, TID, FCode, NPI).ToList();
                  
                     foreach (var list in Result)
                     {
                         Duration1 = new TimeSpan();
                         Duration2 = new TimeSpan();
                         groupsize = Int16.Parse(list.GroupSize);
                         if (FCode != "SAS" || FCode != "CS" || !FCode.Contains("Other"))
                         {
                             if (list.Language == "EN") { 
                                 ENCount += 1;
                                 //groupsize = Int16.Parse(list.GroupSize);
                                 Duration1 = DateTime.Parse(list.EndTime.ToString()).Subtract(DateTime.Parse(list.StartTime.ToString()));
                                 Duration1 = new TimeSpan(Duration1.Ticks / groupsize);
                                 ENTotal = ENTotal.Add(Duration1);
                             }
                             else if (list.Language == "SP") { 
                               //  SPCount += 1; SPTotal = SPTotal.Add(DateTime.Parse(list.EndTime.ToString()).Subtract(DateTime.Parse(list.StartTime.ToString())));
                                 SPCount += 1;
                                 //groupsize = Int16.Parse(list.GroupSize);
                                 Duration1 = DateTime.Parse(list.EndTime.ToString()).Subtract(DateTime.Parse(list.StartTime.ToString()));
                                 Duration1 = new TimeSpan(Duration1.Ticks / groupsize);
                                 SPTotal = SPTotal.Add(Duration1);
                             }
                             else if (list.Language == "RU") { 
                                // RUCount += 1; RUTotal = RUTotal.Add(DateTime.Parse(list.EndTime.ToString()).Subtract(DateTime.Parse(list.StartTime.ToString())));
                                 RUCount += 1;
                                 //groupsize = Int16.Parse(list.GroupSize);
                                 Duration1 = DateTime.Parse(list.EndTime.ToString()).Subtract(DateTime.Parse(list.StartTime.ToString()));
                                 Duration1 = new TimeSpan(Duration1.Ticks / groupsize);
                                 RUTotal = RUTotal.Add(Duration1);
                             }
                             else { 
                                 //POCount += 1; POTotal = POTotal.Add(DateTime.Parse(list.EndTime.ToString()).Subtract(DateTime.Parse(list.StartTime.ToString()))); 
                                 POCount += 1;
                                // groupsize = Int16.Parse(list.GroupSize);
                                 Duration1 = DateTime.Parse(list.EndTime.ToString()).Subtract(DateTime.Parse(list.StartTime.ToString()));
                                 Duration1 = new TimeSpan(Duration1.Ticks / groupsize);
                                 POTotal = POTotal.Add(Duration1);
                             }
                         }
                         totalCount += 1;
                         Duration2 = DateTime.Parse(list.EndTime.ToString()).Subtract(DateTime.Parse(list.StartTime.ToString()));
                         Duration2 = new TimeSpan(Duration2.Ticks / groupsize);
                         total = total.Add(Duration2);

                         
                         MasterTotal = MasterTotal.Add(Duration2);

                     }
                     var TotHours = "";
                     int TotCount = 0;
                     //var value = total;
                     var val1 = total.TotalHours.ToString().Split('.');
                     TotHours = (val1[0].ToString() + ":" + Convert.ToInt32(total.Minutes)).ToString();
                     SessionCount.Add(new SessionCountList
                     {
                         FundingCode = FCode,
                         Language = " ",
                         SessionCount = totalCount,
                         TotalHours = TotHours.ToString(),

                     });
                     if (Result.Count > 0)
                     {
                         if (FCode != "SAS" || FCode != "CS" || !FCode.Contains("Other"))
                         {
                             string[] language = { "EN", "SP", "RU", "PO" };
                             int k = 0;
                             foreach (var lang in language)
                             {
                                 TotHours = "";
                                 TotCount = 0;
                                 if (lang == "EN")
                                 {
                                     //value = ENTotal;
                                     val1 = ENTotal.TotalHours.ToString().Split('.');
                                     TotHours = (val1[0].ToString() + ":" + Convert.ToInt32(ENTotal.Minutes)).ToString();
                                     TotCount = ENCount;

                                 }
                                 if (lang == "SP")
                                 {
                                     //value = SPTotal;
                                     val1 = SPTotal.TotalHours.ToString().Split('.');
                                     TotHours = (val1[0].ToString() + ":" + Convert.ToInt32(SPTotal.Minutes)).ToString();
                                     TotCount = SPCount;

                                 }
                                 if (lang == "RU")
                                 {
                                     //value = RUTotal;
                                     val1 = RUTotal.TotalHours.ToString().Split('.');
                                     TotHours = (val1[0].ToString() + ":" + Convert.ToInt32(RUTotal.Minutes)).ToString();
                                     TotCount = RUCount;

                                 }
                                 if (lang == "PO")
                                 {
                                     //value = POTotal;
                                     val1 = POTotal.TotalHours.ToString().Split('.');
                                     TotHours = (val1[0].ToString() + ":" + Convert.ToInt32(POTotal.Minutes)).ToString();
                                     TotCount = POCount;

                                 }

                                 SessionCount.Add(new SessionCountList
                                 {
                                     FundingCode = "" ,
                                     Language = language[k].ToString(),
                                     SessionCount = TotCount,
                                     TotalHours = TotHours,

                                 });
                                 k += 1;
                             }
                         }
                     }
                     
                         
                     
                     }
                else
                {
                    int ServiceTypeCount = 0;
                    TimeSpan ServiceTypeHours = new TimeSpan(); 

                    var Result1 = db.SP_SessCountByServiceType(startDay, endDay, TID, FCode).ToList();
                    foreach (var list in Result1)
                    {

                        totalCount += 1;
                        if(FCode != "Other"){
                        MasterTotal = MasterTotal.Add(DateTime.Parse(list.EndTime.ToString()).Subtract(DateTime.Parse(list.StartTime.ToString())));
                            }
                        ServiceTypeHours = ServiceTypeHours.Add(DateTime.Parse(list.EndTime.ToString()).Subtract(DateTime.Parse(list.StartTime.ToString())));
                        ServiceTypeCount += 1;

                    }
                    //var TotHours1 = "";
                    //var value1 = total;
                   // var val2 = total.TotalHours.ToString().Split('.');
                  // var TotHours1 = (val2[0].ToString() + ":" + Convert.ToInt32(total.Minutes)).ToString();

                   var val2 = ServiceTypeHours.TotalHours.ToString().Split('.');
                   var TotHours1 = (val2[0].ToString() + ":" + Convert.ToInt32(ServiceTypeHours.Minutes)).ToString();

                    SessionCount.Add(new SessionCountList
                    {
                        FundingCode = FCode,
                        Language = " ",
                        SessionCount = totalCount,
                        TotalHours = TotHours1.ToString(),

                    });

                    

                }
                MasterCount += totalCount;
                //var value3 = total.Add(total);
                var val3 = MasterTotal.TotalHours.ToString().Split('.');
                MasterTotalValue = (val3[0].ToString() + ":" + Convert.ToInt32(MasterTotal.Minutes)).ToString();
                 
                //objview.MasterHour = "";
                
              
                 j += 1;
            }
           // objview.SessionList = SessionList;
            objview.MasterCount = MasterCount.ToString();
            objview.MasterHour = MasterTotalValue.ToString();
            objview.SessionCount = SessionCount;

            return View(objview);
        }

        public ActionResult OtherList(string id)

        {
            if (CheckUserLoginStatus() <= 0)
                return AccessDeniedView();

            BillingViewModel objview = new BillingViewModel();
              List<OtherList> Other = new List<OtherList>();
              var TherapistID = "";
              var startDay = "";
              var endDay = "";
              var TID = "";
              var value = id.Split(',');
              var FCode = value[0];
              if (value.Count() > 1)
              {
                  TID = value[1];
                  startDay = value[2];
                  endDay = value[3];
                  @Session["Half"] = "Full Month";
              }
              else {
                  TID = Session["TID"].ToString();
                  startDay = Session["StartDate"].ToString();
                  endDay = Session["EndDate"].ToString();
              
              }
            
              //objviewmodel.AgeGroup + " " + objviewmodel.Fiscal + " " + objviewmodel.Month;
             
             var NPI = "";
            
              DateTime datevalue = Convert.ToDateTime(startDay);
              int Fisacl = datevalue.Year;
              if (datevalue.Month > 8) { Fisacl = Fisacl + 1; }
              objview.AgeGroup = FCode;
              if (TID == "0")
              {
                  NPI = "%";
              }
              else
              {
                  var TherResult = db.Sp_get_Therpist_Info(TID).SingleOrDefault();
                  NPI = TherResult.NPI.Trim();
                  objview.Therepist = TherResult.FirstName + "," + TherResult.LastName;
              }
             // var NPI = TherResult.NPI;
              if (FCode == "CSE" || FCode == "CPSE" || FCode == "PP" || FCode == "PI" || FCode == "RSA" || FCode == "EI")
              {
                  //var result1 = db.SP_SessCountByFuningCode(startDay, endDay, TID, FCode,NPI).ToList();
                  var result1 = db.SP_BillingNew(Fisacl.ToString(), startDay, endDay, TID, FCode, NPI).ToList();
                  if (result1 != null)
                  {

                      foreach (var item in result1)
                      {

                          Other.Add(new OtherList
                          {
                              SessionDate = item.Date.Value.Date.ToShortDateString(),
                              Firstname = item.StudentFirstName.ToString(),
                              Lastname = item.StudentLastName.ToString(),
                              StartTime = Convert.ToDateTime(item.StartTime.ToString()).ToShortTimeString(),
                              EndTime = Convert.ToDateTime(item.EndTime.ToString()).ToShortTimeString(),
                              Duration = DateTime.Parse(item.EndTime.ToString()).Subtract(DateTime.Parse(item.StartTime.ToString())).ToString(),
                              Desc = item.Language,

                              //SName = list.StudentFirstName + " " + list.StudentLastName,


                          });


                      }

                      objview.Other = Other;

                  }
              }
              else { 
              var result = db.SP_SessCountByServiceType(startDay, endDay, TID, FCode).ToList();

              if (result != null)
              {
                  var FirstName = "";
                  var LastName = "";
                  foreach (var item in result)
                  {
                      if (FCode == "CS") {

                          var name = db.Sp_get_Therpist_Info(item.SID).SingleOrDefault();
                          FirstName = name.FirstName.Trim();
                          LastName = name.LastName.Trim();
                      
                      }

                      Other.Add(new OtherList
                      {
                          SessionDate = item.Date.Value.Date.ToShortDateString(),
                          Firstname = FirstName,
                          Lastname = LastName,
                          StartTime = Convert.ToDateTime(item.StartTime.ToString()).ToShortTimeString(),
                          EndTime = Convert.ToDateTime(item.EndTime.ToString()).ToShortTimeString(),
                          Duration = DateTime.Parse(item.EndTime.ToString()).Subtract(DateTime.Parse(item.StartTime.ToString())).ToString(),
                          Desc = item.ServiceType,

                          //SName = list.StudentFirstName + " " + list.StudentLastName,


                      });


                  }

                  objview.Other = Other;

              }

              }

             
              

              




            return View(objview);
        }

        private void releaseObject(object obj)
        {
            var error = "";
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                error = "Exception Occured while releasing object " + ex.ToString();
            }
            finally
            {
                GC.Collect();
            }
        }


        public int CheckUserLoginStatus()
        {
            int LoginUserId = 0;
         
            if (Session["UserId"] != null)
            {
                var UserType = db.Sp_get_Therpist_Info(Session["UserId"].ToString()).SingleOrDefault();
                if(UserType.UserType == "ADMIN"){
                LoginUserId = Convert.ToInt32(Session["UserId"]);}
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
