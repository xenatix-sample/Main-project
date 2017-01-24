using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Axis.Plugins.Scheduling.Infrastructure
{
    public class CustomViewEngine : RazorViewEngine
    {
        public CustomViewEngine()
            : base()
            {
                ViewLocationFormats = new string[] { 
                "~/Plugins/Axis.Plugins.Scheduling/Views/{1}/{0}.cshtml",
                "~/Plugins/Axis.Plugins.Scheduling/Views/Shared/{0}.cshtml"
            };

                PartialViewLocationFormats = new[] { "~/Plugins/Axis.Plugins.Scheduling/Views/{1}/{0}.cshtml", "~/Plugins/Axis.Plugins.Scheduling/Views/Shared/{0}.cshtml" };
        }
        
        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            return base.FindView(controllerContext, viewName, masterName, useCache);
        }

        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            return base.FindPartialView(controllerContext, partialViewName, useCache);
        } 
    }
}
