using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CSNY_timelog.ViewModel
{
      
     

    public class FindUserViewModel : Controller
    {
        //
        // GET: /FindUserViewModel/

        public string UserType { get; set; }
        
        public string Fiscal { get; set; }
        public List<StudentListViewModel> StudentList { get; set; }
        public List<TherapistListViewModel> TherapistList { get; set; }

    }

    public class StudentListViewModel
    {

        public string SID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OSIS { get; set; }
        public string FundingCode { get; set; }
        public string Fiscal { get; set; }
        public string S1 { get; set; }
        public string SP { get; set; }
       
    }
    public class TherapistListViewModel
    {

        public string TID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SSN { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }




    }
}
