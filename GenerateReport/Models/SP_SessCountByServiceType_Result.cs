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
    
    public partial class SP_SessCountByServiceType_Result
    {
        public int SrNo { get; set; }
        public int SessionID { get; set; }
        public string SID { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string AttnCode { get; set; }
        public string GroupSize { get; set; }
        public Nullable<System.TimeSpan> StartTime { get; set; }
        public Nullable<System.TimeSpan> EndTime { get; set; }
        public string Location { get; set; }
        public string TID { get; set; }
        public string ServiceType { get; set; }
        public string GroupType { get; set; }
        public Nullable<System.DateTime> LastModified { get; set; }
    }
}
