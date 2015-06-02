using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CSNY_timelog.ViewModel
{
    public class SignViewModel
    {
        
        [Display(Name = "name")]
        public string name { get; set; }

        
        [Display(Name = "output")]
        public string output { get; set; }

    }
}