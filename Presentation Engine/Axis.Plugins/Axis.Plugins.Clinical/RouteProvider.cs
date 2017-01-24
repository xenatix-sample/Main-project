using System.Web.Mvc;
using Axis.PresentationEngine.Helpers.Routes;
using System.Web.Routing;
using Axis.Plugins.Clinical.Infrastructure;

namespace Axis.Plugins.Clinical
{
    public class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute("Plugin.Clinical.Allergy",
                "Plugins/Clinical/Allergy/{action}",
                new { controller = "Allergy", action = "Index" },
                new[] { "Axis.Plugins.Clinical.Controllers" }
                );

            routes.MapRoute("Plugin.Clinical.MedicalHistory",
                "Plugins/Clinical/MedicalHistory/{action}",
                new { controller = "MedicalHistory", action = "Index" },
                new[] { "Axis.Plugins.Clinical.Controllers" }
                );

            routes.MapRoute("Plugin.Clinical.ReviewOfSystems",
                "Plugins/Clinical/ReviewOfSystems/{action}",
                new { controller = "ReviewOfSystems", action = "Index" },
                new[] { "Axis.Plugins.Clinical.Controllers" }
                );

            routes.MapRoute("Plugin.Clinical.Note",
                "Plugins/Clinical/Note/{action}",
                new { controller = "Note", action = "Index" },
                new[] { "Axis.Plugins.Clinical.Controllers" }
                );

            routes.MapRoute("Plugin.ClinicalPlugins.ChiefComplaint",
                "Plugins/Clinical/ChiefComplaint/{action}",
                new { controller = "ChiefComplaint", action = "Index" },
                new[] { "Axis.Plugins.Clinical.Controllers" }
                );
            routes.MapRoute("Plugin.ClinicalPlugins.Vital",
               "Plugins/Clinical/Vital/{action}",
               new { controller = "Vital", action = "Index" },
               new[] { "Axis.Plugins.Clinical.Controllers" }
               );

            routes.MapRoute("Plugin.Clinical.ClinicalAssessment",
               "Plugins/Clinical/ClinicalAssessment/{action}",
               new { controller = "ClinicalAssessment", action = "Index" },
               new[] { "Axis.Plugins.Clinical.Controllers" }
               );
            routes.MapRoute("Plugins.Clinical.SocialRelationshipHistory",
                "Plugins/Clinical/SocialRelationshipHistory/{action}",
                new { controller = "SocialRelationshipHistory", action = "Index" },
                new[] { "Axis.Plugins.Clinical.Controllers" }
                );
            routes.MapRoute("Plugin.Clinical.SocialRelationship",
                "Plugins/Clinical/SocialRelationship/{action}",
                new { controller = "SocialRelationship", action = "Index" },
                new[] { "Axis.Plugins.Clinical.Controllers" }
                );

            routes.MapRoute("Plugin.Clinical.PresentIllness",
                "Plugins/Clinical/PresentIllness/{action}",
                new { controller = "PresentIllness", action = "Index" },
                new[] { "Axis.Plugins.Clinical.Controllers" }
                );

            routes.MapRoute("Plugin.Clinical.PresentIllnessDetail",
               "Plugins/Clinical/PresentIllnessDetail/{action}",
               new { controller = "PresentIllnessDetail", action = "Index" },
               new[] { "Axis.Plugins.Clinical.Controllers" }
               );

            ViewEngines.Engines.Add(new CustomClinicalViewEngine());
        }

        public int Priority
        {
            get
            {
                return 3;
            }
        }
    }
}
