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
    public class AddStudentViewModel
    {
        [Required]
        [Display(Name = "Student ID")]
        public string SID { get; set; }


            [Required]
            [Display(Name = "Pay Type")]
            public string PayType { get; set; }

            [Required]
            [Display(Name = "Referral")]
            public string Referral { get; set; }

            [Required]
            [Display(Name = "Patent First Name")]
            public string PFirstName { get; set; }

            [Required]
            [Display(Name = "Parent Last Name")]
            public string PLastName { get; set; }

            [Required]
            [Display(Name = "Address1")]
            public string Address1 { get; set; }

        [Display(Name = "Parent Email Address")]
            public string PEmail { get; set; }

        [Display(Name = "Parent Home Phone")]
            public string PHomePh { get; set; }

         [Display(Name = "Parent office Phone")]
            public string POfficePh { get; set; }

         [Display(Name = "Parent Mobil Phone")]
            public string PMobilPh { get; set; }

         [Required]
            [Display(Name = "Guardian Name")]
            public string GName { get; set; }

         [Required]
            [Display(Name = "Guardian Mobile Phone")]
            public string GMobilPh { get; set; }

         [Required]
            [Display(Name = "Guardian Email")]
            public string GEmail { get; set; }

         [Required]
            [Display(Name = "Fiscal Year")]
            public string Fiscal { get; set; }

         [Required]
            [Display(Name = "Service Start Date SP")]
            public string StartDate { get; set; }

         

          [Required]
            [Display(Name = "Service End Date SP")]
            public string EndDate { get; set; }

          [Required]
          [Display(Name = "SP Check")]
          public bool CheckSP { get; set; }

          [Required]
          [Display(Name = "S1 Check")]
          public bool CheckS1 { get; set; }

         [Required]
            [Display(Name = "NYCID")]
            public string NYCID { get; set; }

         [Required]
            [Display(Name = "Funding code")]
            public string FundingCode { get; set; }

         [Required]
            [Display(Name = "Location")]
            public string Location { get; set; }

         [Required]
            [Display(Name = "School Name")]
            public string SchoolName { get; set; }

         [Required]
            [Display(Name = "School Code")]
            public string SchoolCode { get; set; }


        [Required]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

           
            // [Required]
            [Display(Name = "DOB")]
            public string DOB { get; set; }


            // [Required]
            [Display(Name = "School District Code")]
            public string Districtcode { get; set; }

            // [Required]
            [Display(Name = "Borough Code")]
            public string BoroughCode { get; set; }

            // [Required]
            [Display(Name = "Service Type")]
            public string ServiceType { get; set; }

            // [Required]
            [Display(Name = "Therapist")]
            public string Terapist { get; set; }

            // [Required]
            [Display(Name = "Frequency")]
            public string Frequency { get; set; }

        

          // [Required]
            [Display(Name = "Duration")]
            public string Duration { get; set; }

           // [Required]
            [Display(Name = "Group Size")]
            public string GroupSize { get; set; }

            // [Required]
            [Display(Name = "Therapist1")]
            public string Terapist1 { get; set; }

            // [Required]
            [Display(Name = "Frequency1")]
            public string Frequency1 { get; set; }



            // [Required]
            [Display(Name = "Duration1")]
            public string Duration1 { get; set; }

            // [Required]
            [Display(Name = "Group Size1")]
            public string GroupSize1 { get; set; }

            [Required]
            [Display(Name = "language")]
            public string language { get; set; }

            [Required]
            [Display(Name = "Diagnosis")]
            public string Diagnosis { get; set; }

            [Required]
            [Display(Name = "ParentReport")]
            public string ParentReport { get; set; }

            [Required]
            [Display(Name = "Comments")]
            public string Comments { get; set; }

            
            [Display(Name = "MandateID")]
            public string MID { get; set; }

            [Required]
            [Display(Name = "TID")]
            public string TID { get; set; }

            public IEnumerable<SelectListItem> TherapistList { get; set; }
            public List<MandateListViewModel> MandateList { get; set; }
    }

    public class MandateListViewModel
    {

        public string MandateID { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string TID { get; set; }
        public string FundingCode { get; set; }
       


    }
}