//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GenerateReport.Models
{
    using System;
    
    public partial class GetStudentInformation_Pdf_Result
    {
        public string BoroughCode { get; set; }
        public string HomeDistrict { get; set; }
        public string SchoolLocationCode { get; set; }
        public string NYCI { get; set; }
        public string StudentFirstName { get; set; }
        public string StudentLastName { get; set; }
        public string DOB { get; set; }
        public Nullable<System.DateTime> ServiceStart { get; set; }
        public Nullable<System.DateTime> ServiceEnd { get; set; }
        public string MandFrequency { get; set; }
        public string MandDuration { get; set; }
        public string MandGroupSize { get; set; }
        public string Language { get; set; }
    }
}