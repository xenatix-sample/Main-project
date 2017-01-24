using System.Web.Mvc;

namespace Axis.Plugins.Clinical.Infrastructure
{
    public class CustomClinicalViewEngine : RazorViewEngine
    {
        public CustomClinicalViewEngine()
            : base()
        {
            ViewLocationFormats = new string[] { 
                "~/Plugins/Axis.Plugins.Clinical/Views/{1}/{0}.cshtml",
            };

            PartialViewLocationFormats = new[] { "~/Plugins/Axis.Plugins.Clinical/Views/{1}/{0}.cshtml" };
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
