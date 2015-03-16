using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Collections;
using System.ComponentModel;

namespace CSNY_timelog.ViewModel
{
    public class BillingViewModel
    {
        [Required]
        [Display(Name = "Therepist")]
        public string Therepist { get; set; }

        [Required]
        [Display(Name = "AgeGroup")]
        public string AgeGroup { get; set; }

        [Required]
        [Display(Name = "Fiscal Year")]
        public string Fiscal { get; set; }

        [Required]
        [Display(Name = "Month")]
        public string Month { get; set; }

        [Required]
        [Display(Name = "Day")]
        public string Day { get; set; }

        [Display(Name = "StartDate")]
        public string StartDate { get; set; }

        [Display(Name = "EndDate")]
        public string EndDate { get; set; }

        [Display(Name = "MasterCount")]
        public string MasterCount { get; set; }

        [Display(Name = "MasterHour")]
        public string MasterHour { get; set; }



        public List<SelectListItem> TherapistList { get; set; }
        public List<SessionCountList> SessionCount { get; set; }
        public List<OtherList> Other { get; set; }
        public List<MastercheckLsit> MasterCheck { get; set; }
        public List<MastercheckLsit1> MasterCheck1 { get; set; }

    }
    public class MastercheckLsit
    {

        public string Therapist { get; set; }
        public int Session { get; set; }
        public int Billing { get; set; } 
        public int BillingNew { get; set; }
        public int Ps { get; set; }
        public int Rs { get; set; }
      


    }
    public class MastercheckLsit1
    {

        public string TID { get; set; }
        public string Date { get; set; }
        public string Student { get; set; }
        public string Fcode { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public string Group { get; set; }



    }

    public class SessionCountList
    {

        public string FundingCode { get; set; }
        public string Language { get; set; }
        public int SessionCount { get; set; }
        public string TotalHours { get; set; }
        ////
        //public string FundingCode { get; set; }
        //public int EN { get; set; }
        //public int SP { get; set; }
        //public int RU { get; set; }
        //public int PO { get; set; }
        //public int Total { get; set; }
        ////
        //public string StartTime { get; set; }
        //public string EndTime { get; set; }
        //public string Duration { get; set; }
        //public string GroupType { get; set; }
        //public string ServiceType { get; set; }


    }
    public class OtherList
    {

        public string SessionDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Duration { get; set; }
        public string Desc { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        

    }
   





}