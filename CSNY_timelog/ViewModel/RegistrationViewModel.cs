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
    public class RegistrationViewModel
    {
        [Required]
        [Display(Name = "TID")]
        public string TID { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Address1")]
        public string Address1 { get; set; }

        [Display(Name = "Address2")]
        public string Address2 { get; set; }

        [Required]
        [Display(Name = "State Name")]
        public string StateName { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Email]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Confirm Email Address")]
        public string ConfirmEmail { get; set; }


        [Required(ErrorMessage = "Phone Number is required")]
        [RegularExpression(@"^[0-9]{0,15}$", ErrorMessage = "PhoneNumber should contain only numbers")]
        [StringLength(15, ErrorMessage = "Phone Number length Should be less than 15")]
        public string Phone { get; set; }

        // [Required]
        [Display(Name = "Country")]
        public string CountryName { get; set; }


        // [Required]
        [Display(Name = "SSN")]
        public string SSN { get; set; }

        // [Required]
        [Display(Name = "ServiceType")]
        public string ServiceType { get; set; }

        // [Required]
        [Display(Name = "UserTypr")]
        public string UserType { get; set; }


        [Required]
        [StringLength(50, ErrorMessage = "UserName must be 5 to 50 characters.", MinimumLength = 5)]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Password must be 8 to 50 characters.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]

       
        public string OldPassword { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        [Display(Name = "IsActive")]
        public string IsActive { get; set; }

        [Display(Name = "EndService")]
        public string EndService { get; set; }

        public string EnterValidationCode1 { get; set; }

        [Required]
        [Display(Name = "Enter Validation Code")]
        public string EnterValidationCode { get; set; }

        [Required]
        [DisplayName("Accept terms and conditions")]
        public bool Checkterms { get; set; }

        public bool CheckforAddtioninfo { get; set; }

        public class EmailAttribute : RegularExpressionAttribute
        {
            public EmailAttribute()
                : base(@"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}")
            {
                this.ErrorMessage = "Please provide a valid email address.";
            }
        }

        [MustBeTrue(ErrorMessage = "Please Accept the Terms & Conditions")]
        public bool TermsAccepted { get; set; }
    }

    public class MustBeTrueAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return value is bool && (bool)value;
        }
    }



    //public class BooleanTrueAttribute : ValidationAttribute
    //{
    //    public override bool IsValid(object value)
    //    {
    //        return value != null && value is bool && (bool)value;
    //    }
    //}


    //public class BooleanTrueAttribute : RegularExpressionAttribute
    //{
    //    public BooleanTrueAttribute()
    //        : base(@"^true")
    //    {
    //        this.ErrorMessage = "Please Accept terms and conditions.";
    //    }
    //}


}