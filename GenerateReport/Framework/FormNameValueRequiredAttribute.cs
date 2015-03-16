using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace GenerateReport.Framework
{
    public class FormNameValueRequiredAttribute : ActionMethodSelectorAttribute
    {
         private readonly string[] _submitButtonNames;
        private readonly FormValueRequirement _requirement;
        private readonly string _fieldValue;

        public FormNameValueRequiredAttribute(string fieldValue, params string[] submitButtonNames) :
            this(FormValueRequirement.Equal,fieldValue, submitButtonNames)
        {
        }

        public FormNameValueRequiredAttribute(FormValueRequirement requirement, string fieldValue, params string[] submitButtonNames)
        {
            //at least one submit button should be found
            this._submitButtonNames = submitButtonNames;
            this._requirement = requirement;
            this._fieldValue = fieldValue;
        }

        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            foreach (string buttonName in _submitButtonNames)
            {
                try
                {
                    string value = "";
                    switch (this._requirement)
                    {
                        case FormValueRequirement.Equal:
                            {
                                //do not iterate because "Invalid request" exception can be thrown
                                value = controllerContext.HttpContext.Request.Form[buttonName];
                            }
                            break;
                        case FormValueRequirement.StartsWith:
                            {
                                foreach (var formValue in controllerContext.HttpContext.Request.Form.AllKeys)
                                {
                                    if (formValue.StartsWith(buttonName, StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        value = controllerContext.HttpContext.Request.Form[formValue];
                                        break;
                                    }
                                }
                            }
                            break;
                    }
                    if (!String.IsNullOrEmpty(value))
                        if(_fieldValue==value)
                        return true;
                }
                catch (Exception exc)
                {
                    //try-catch to ensure that 
                    Debug.WriteLine(exc.Message);
                }
            }
            return false;
        }
    }

    
}