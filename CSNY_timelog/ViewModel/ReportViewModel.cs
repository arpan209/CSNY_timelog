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
    public class ReportViewModel
    {
        [Required]
        [Display(Name = "AgeGroup")]
        public string AgeGroup { get; set; }

        [Required]
        [Display(Name = "Fiscal Year")]
        public string Fiscal { get; set; }

        [Required]
        [Display(Name = "Month")]
        public string Month { get; set; }

        [Display(Name = "InvoiceID")]
        public string InvoiceID { get; set; }

        [Required]
        [Display(Name = "TID")]
        public string TID { get; set; }

        [Display(Name = "Invoice")]
        public string Invoice { get; set; }

        [Display(Name = "Note")]
        public string Note { get; set; }

        [Required]
        [Display(Name = "Filename")]
        public string Filename { get; set; }

        [Required]
        [Display(Name = "Day")]
        public string Day { get; set; }

        [Display(Name = "StartDate")]
        public string StartDate { get; set; }

        [Display(Name = "EndDate")]
        public string EndDate { get; set; }

        [Display(Name = "Count")]
        public string Count { get; set; }
        public string DataValue { get; set; }
        public string SrNo { get; set; }
        public int RTotal { get; set; } 
        public int PTotal { get; set; }

        public List<SessionListNotPViewModel> SessionListNotP { get; set; }
        public IEnumerable<SelectListItem> TherapistList { get; set; }
        public List<SummaryListViewModel> SummaryList { get; set; }
        public List<DOEFilelist> Filelist { get; set; }
    }

        


    public class SessionListNotPViewModel
    {
        public int SessionId { get; set; }
        public int SrNo { get; set; }
        public DateTime? SessDate { get; set; }
        public string StudName { get; set; }
        public string TherName { get; set; }
        public string FundingCode { get; set; }
        public string Check { get; set; }
        //public string ServiceType { get; set; }


    }

    public class SummaryListViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int RCount { get; set; }
        public int PCount { get; set; }
        public string Invoice { get; set; }
        public string TID { get; set; }
        public int ID { get; set; }
        public string Note { get; set; }


    }

         public class DOEFilelist
    {
        public string FileName { get; set; }
        public string LastModield { get; set; }
      
        //public string GroupType { get; set; }
        //public string ServiceType { get; set; }


    }
   
}
  


   

