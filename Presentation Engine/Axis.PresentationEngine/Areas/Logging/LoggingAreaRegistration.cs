using System.Web.Mvc;

namespace Axis.PresentationEngine.Areas.Logging
{
    public class LoggingAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Logging";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Logging_default",
                "Logging/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}