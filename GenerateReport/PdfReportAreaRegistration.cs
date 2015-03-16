using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GenerateReport
{
    public class PdfReportAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "PdfReport";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "PdfReport_fillForm",
                "GeneratePdfReport/",
                new { controller = "PdfFromReport", action = "fillForm", area = "PdfReport"},
                new[] { "GenerateReport.Controllers" }
            );

            context.MapRoute(
                "PdfReport_default",
                "PdfReport/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", area = "PdfReport", id = "" },
                new[] { "GenerateReport.Controllers" }
            );
        }
    }
}