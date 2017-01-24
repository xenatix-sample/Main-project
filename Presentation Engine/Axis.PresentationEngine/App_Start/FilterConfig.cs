using System.Web.Mvc;
using Axis.PresentationEngine.Helpers.Filters;

namespace Axis.PresentationEngine
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new ExceptionFilter());
            filters.Add(new Authorization());
        }
    }
}
