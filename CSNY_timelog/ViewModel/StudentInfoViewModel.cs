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
    public class StudentInfoViewModel
    {
     

        [Required]
        [Display(Name = "Frequency")]
        public string Frequency { get; set; }

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

        [Required]
        [Display(Name = "Month")]
        public string Month { get; set; }
        [Display(Name = "Day")]
        public string Day { get; set; }

        public bool Lock { get; set; }

        [Display(Name = "ServiceList")]
        public string ServiceList { get; set; }

        [Required]
        public string SessionId { get; set; }

        [Required]
        public bool IsEdit { get; set; }

        public IEnumerable<string> SelectedStud { get; set; }

        public List<StudSessionListViewModel> SessionList { get; set; }

    }



    public class StudSessionListViewModel
    {

        public string SessionID { get; set; }
        public string SName { get; set; }
        public string SessionDate { get; set; }
        public string Group { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Duration { get; set; }
        public string GroupType { get; set; }
        public string ServiceType { get; set; }


    }
}