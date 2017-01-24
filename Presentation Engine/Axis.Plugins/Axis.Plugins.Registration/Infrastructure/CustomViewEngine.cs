using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Axis.Plugins.Registration.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomViewEngine : RazorViewEngine
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomViewEngine"/> class.
        /// </summary>
        public CustomViewEngine()
            : base()
            {
                ViewLocationFormats = new string[] { 
                "~/Plugins/Axis.Plugins.Registration/Views/{1}/{0}.cshtml",
                "~/Plugins/Axis.Plugins.Registration/Views/Referrals/{1}/{0}.cshtml"
            };

            PartialViewLocationFormats = new[] { 
                "~/Plugins/Axis.Plugins.Registration/Views/{1}//{0}.cshtml",
                "~/Plugins/Axis.Plugins.Registration/Views/Referrals/{1}//{0}.cshtml"
            };
        }

        /// <summary>
        /// Finds the specified view by using the specified controller context and master view name.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="viewName">The name of the view.</param>
        /// <param name="masterName">The name of the master view.</param>
        /// <param name="useCache">true to use the cached view.</param>
        /// <returns>
        /// The page view.
        /// </returns>
        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            return base.FindView(controllerContext, viewName, masterName, useCache);
        }

        /// <summary>
        /// Finds the specified partial view by using the specified controller context.
        /// </summary>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="partialViewName">The name of the partial view.</param>
        /// <param name="useCache">true to use the cached partial view.</param>
        /// <returns>
        /// The partial view.
        /// </returns>
        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            return base.FindPartialView(controllerContext, partialViewName, useCache);
        } 
    }
}
