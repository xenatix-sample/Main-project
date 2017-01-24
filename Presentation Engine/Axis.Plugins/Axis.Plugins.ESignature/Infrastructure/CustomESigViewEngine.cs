using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Axis.Plugins.ESignature.Infrastructure
{
    public class CustomESigViewEngine : RazorViewEngine
    {
        public CustomESigViewEngine()
            : base()
        {
            ViewLocationFormats = new string[] { 
                "~/Plugins/Axis.Plugins.ESignature/Views/{1}/{0}.cshtml",
            };

            PartialViewLocationFormats = new[] { "~/Plugins/Axis.Plugins.ESignature/Views/{1}/{0}.cshtml" };
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
