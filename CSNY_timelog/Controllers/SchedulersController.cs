using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CSNY_timelog.Models;
using CSNY_timelog.ViewModel;

namespace CSNY_timelog.Helper 
{
    public class SchedulersController : Controller
    {
        CSNY_NewEntities db = new CSNY_NewEntities();
        public ActionResult Index()
        {
         
            return View();
        }


        // Lock the session after three day of unlock
        public ActionResult LockSessionScheduler() 
        {
            try
            {

                var TherapistList = db.SP_TherapistList();

                foreach (var List in TherapistList)
                  {
                  
                  
                    if (List.Lock.Trim() != "1")
                    {
                        var Result = List.Lock.ToString().Split(',');
                        var LockValue = Result[0].Trim();
                        var DateValue = Result[1].Trim();
                        var TID = List.TID;
                        DateTime LockDate = Convert.ToDateTime(DateValue);
                        var Diff = (DateTime.Now - LockDate).TotalDays;

                        if (Convert.ToInt32(Diff) >= 3) { db.UpdateTherapistLock(TID.ToString()); }

                    }

                                  
                  }

              
            }
            catch (Exception ex)
            {
                //// ILogger _logger = new DefaultLogger();
                // _logger.Error(ex.Message, ex);// return ex.Message.ToString();
            }
            return View();
        }



    }
}
