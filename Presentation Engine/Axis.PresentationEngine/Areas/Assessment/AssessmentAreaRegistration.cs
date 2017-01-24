using System.Web.Mvc;

namespace Axis.PresentationEngine.Areas.Assessment
{
    public class AssessmentAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Assessment";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Assessment_default",
                "Assessment/{controller}/{action}/{id}",
                new { action = "Screening", id = UrlParameter.Optional }
            );
        }
    }
}