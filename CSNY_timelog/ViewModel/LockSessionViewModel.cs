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
    public class LockSessionViewModel
    {
        //
        // GET: /LockSessionViewModel/

      
        [Display(Name = "Therepist")]
        public string Therepist { get; set; }

        [Required]
        [Display(Name = "TID")]
        public string TID { get; set; }
        public string SelectedTID { get; set; }
        public string NotSelectedTID { get; set; }

       
        public bool Lock { get; set; }



        //public IEnumerable<SelectListItem> TherapistList { get; set; }
        public List<TherapistLockList> LockList { get; set; }

    }

    public class TherapistLockList
    {

        public string TID { get; set; }
        public string FirstName { get; set; }
        public bool LockSession { get; set; }
        public bool Notify { get; set; }
        //public string Duration { get; set; }
        //public string GroupType { get; set; }
        //public string ServiceType { get; set; }


    }
}
