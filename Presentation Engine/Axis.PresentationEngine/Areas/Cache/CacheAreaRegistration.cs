using System.Web.Mvc;

namespace Axis.PresentationEngine.Areas.Cache
{
    public class CacheAreaRegistration : AreaRegistration
    {
            public override string AreaName
            {
                get
                {
                    return "Cache";
                }
            }

            public override void RegisterArea(AreaRegistrationContext context)
            {
                context.MapRoute(
                    "Cache_default",
                    "Cache/{controller}/{action}/{id}",
                    new { action = "Manifest", id = UrlParameter.Optional }
                );
            }
    }
}