using System.Web;
using System.Web.Mvc;
using Axis.DataEngine.Service.Filters;

namespace Axis.DataEngine.Service
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
