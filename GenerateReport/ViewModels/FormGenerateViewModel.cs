using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GenerateReport.ViewModels
{
    public class FormGenerateViewModel
    {

       public FormGenerateViewModel()
        {
            StudentList = new List<SelectListItem>();
        }

        [DisplayName("Session Year")]
        [Required(ErrorMessage="Fiscal Year Field Required.")]
        public int FiscalYear { get; set; }


        [DisplayName("Month")]
        [Required(ErrorMessage = "Fiscal Month Field Required")]
        public int FiscalMonth { get; set; }


        [DisplayName("Funding Code")]
        [Required(ErrorMessage = "Report Type Field Required.")]
        public string ReportType { get; set; }

        
        [DisplayName("Student")]
        [Required(ErrorMessage = "Student Field Required.")]
        public int StudentId { get; set; }

        //public string StudentIdList { get; set; }

        public int[] StudentIdList { get; set; }

        public List<SelectListItem> StudentList { get; set; }
    }
}