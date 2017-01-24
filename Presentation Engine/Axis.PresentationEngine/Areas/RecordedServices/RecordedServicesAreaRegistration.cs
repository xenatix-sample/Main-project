using System.Web.Mvc;

namespace Axis.PresentationEngine.Areas.RecordedServices
{
    public class RecordedServicesAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "RecordedServices";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "RecordedServices_default",
                "RecordedServices/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}