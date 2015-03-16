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
    public class AddSessionViewModel
    {
        [Required]
        [Display(Name = "Session Date")]
        public string SessionDate { get; set; }

        [Required]
        [Display(Name = "Session Location")]
        public string Location { get; set; }

        [Required]
        [Display(Name = "SelectS ervice")]
        public string ServiceType { get; set; }

        [Required]
        [Display(Name = "StudentList")]
        public string StudentList { get; set; }

        [Required]
        [Display(Name = "StudentListText")]
        public string StudentListText { get; set; }
        
        [Required]
        [Display(Name = "FellowList")]
        public string FellowList { get; set; }

        [Required]
        [Display(Name = "FellowListText")]
        public string FellowListText { get; set; }

        [Required]
        [Display(Name = "GroupSize")]
        public string GroupSize { get; set; }

        [Required]
        [Display(Name = "Language")]
        public string Language { get; set; }

        
        [Required]
        [Display(Name = "Start Time")]
        public string StartTime { get; set; }

        [Required]
        [Display(Name = "End Time")]
        public string EndTime { get; set; }

        [Display(Name = "Duration")]
        public string Duration { get; set; }

        [Display(Name = "GroupType")]
        public string GroupType { get; set; }

        [Display(Name = "ErrorExist")]
        public string ErrorExist { get; set; }

        [Display(Name = "SID")]
        public string SID { get; set; }

        public string StartAMPM { get; set; }
        public string EndAMPM { get; set; }

        public string LastModified { get; set; }

        [Required]
        [Display(Name = "Month")]
        public string Month { get; set; }
        [Display(Name = "Day")]
        public string Day { get; set; }

        [Display(Name = "ServiceList")]
        public string ServiceList { get; set; }

        [Required]
        public string SessionId { get; set; }

        [Required]
        public bool IsEdit { get; set; }

        public int CPSE { get; set; }
        public int CSE { get; set; }
        public int PP { get; set; }
        public int PI { get; set; }
        public int EI { get; set; }
        public int RSA { get; set; }
        public int CS { get; set; }
        public int PAS { get; set; }
        public int Ohers { get; set; }

        public bool Lock { get; set; }


        public IEnumerable<string> SelectedStud { get; set; }
        public IEnumerable<SelectListItem> Students { get; set; }

        public IEnumerable<SelectListItem> Fellow { get; set; }

        public List<ErrorListViewModel> ErrorArry { get; set; }

        public List<SessionListViewModel> SessionList { get; set; }
        
    }

    public class ErrorListViewModel
        {
           
            public string Child { get; set; }
            public string Error1 { get; set; }
            public string Error2 { get; set; }
            public string Error3 { get; set; }
            public string Error4 { get; set; }
            
        }

    public class SessionListViewModel
    {

        public string SessionID { get; set; }
        public string SName { get; set; }
        public DateTime SessionDate { get; set; }
        public string Group { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Duration { get; set; }
        public string GroupType { get; set; }
        public string ServiceType { get; set; }


    }
}