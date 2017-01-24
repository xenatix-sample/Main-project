using System.Web.Mvc;

namespace Axis.Plugins.ECI.Infrastructure
{
    public class CustomECIViewEngine : RazorViewEngine
    {
        public CustomECIViewEngine()
            : base()
        {
            ViewLocationFormats = new string[] { 
                "~/Plugins/Axis.Plugins.ECI/Views/{1}/{0}.cshtml",
            };

            PartialViewLocationFormats = new[] { "~/Plugins/Axis.Plugins.ECI/Views/{1}/{0}.cshtml" };
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
