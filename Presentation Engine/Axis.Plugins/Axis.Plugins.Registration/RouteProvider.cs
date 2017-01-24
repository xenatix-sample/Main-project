using Axis.Plugins.Registration.Infrastructure;
using Axis.PresentationEngine.Helpers.Routes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace Axis.Plugins.Registration
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute("Plugin.RegistrationPlugins",
                "Plugins/Registration/{action}",
                new { controller = "Registration", action = "Index" },
                new[] { "Axis.Plugins.Registration.Controllers" }
                );

            routes.MapRoute("Plugin.RegistrationPlugins.Quick",
                "Plugins/QuickRegistration/{action}",
                new { controller = "QuickRegistration", action = "Index" },
                new[] { "Axis.Plugins.Registration.Controllers" }
                );

            routes.MapRoute("Plugin.RegistrationPlugins.Additional",
                "Plugins/AdditionalDemographic/{action}",
                new { controller = "AdditionalDemographic", action = "Index" },
                new[] { "Axis.Plugins.Registration.Controllers" }
                );

            routes.MapRoute("Plugin.RegistrationPlugins.Fianancial",
                "Plugins/FinancialAssessment/{action}",
                new { controller = "FinancialAssessment", action = "Index" },
                new[] { "Axis.Plugins.Registration.Controllers" }
                );

            routes.MapRoute("Plugin.RegistrationPlugins.FinancialSummary",
               "Plugins/FinancialSummary/{action}",
               new { controller = "FinancialSummary", action = "Index" },
               new[] { "Axis.Plugins.Registration.Controllers" }
               );

            routes.MapRoute("Plugin.RegistrationPlugins.ContactBenefit",
                "Plugins/ContactBenefit/{action}",
                new { controller = "ContactBenefit", action = "Index" },
                new[] { "Axis.Plugins.Registration.Controllers" }
                );

            routes.MapRoute("Plugin.RegistrationPlugins.SearchClient",
                "Plugins/ClientSearch/{action}",
                new { controller = "ClientSearch", action = "Index" },
                new[] { "Axis.Plugins.Registration.Controllers" }
                );

            routes.MapRoute("Plugin.RegistrationPlugins.EmergencyContact",
                "Plugins/EmergencyContact/{action}",
                new { controller = "EmergencyContact", action = "Index" },
                new[] { "Axis.Plugins.Registration.Controllers" }
                );

            routes.MapRoute("Plugin.RegistrationPlugins.Collateral",
                "Plugins/Collateral/{action}",
                new { controller = "Collateral", action = "Index" },
                new[] { "Axis.Plugins.Registration.Controllers" }
                );

            routes.MapRoute("Plugin.RegistrationPlugins.Consent",
                "Plugins/Consent/{action}",
                new { controller = "Consent", action = "Index" },
                new[] { "Axis.Plugins.Registration.Controllers" }
                );

            routes.MapRoute("Plugin.RegistrationPlugins.Referral",
                "Plugins/Referral/{action}",
                new { controller = "Referral", action = "Index" },
                new[] { "Axis.Plugins.Registration.Controllers" }
                );

            routes.MapRoute("Plugin.RegistrationPlugins.ContactPhone",
                "Plugins/ContactPhone/{action}",
                new { controller = "ContactPhone", action = "Index" },
                new[] { "Axis.Plugins.Registration.Controllers" }
                );

            routes.MapRoute("Plugin.RegistrationPlugins.ContactPhoto",
                "Plugins/ContactPhoto/{action}",
                new { controller = "ContactPhoto", action = "Index" },
                new[] { "Axis.Plugins.Registration.Controllers" }
                );

            routes.MapRoute("Plugin.RegistrationPlugins.ContactEmail",
                "Plugins/ContactEmail/{action}",
                new { controller = "ContactEmail", action = "Index" },
                new[] { "Axis.Plugins.Registration.Controllers" }
                );

            routes.MapRoute("Plugin.RegistrationPlugins.ContactAlias",
                "Plugins/ContactAlias/{action}",
                new { controller = "ContactAlias", action = "Index" },
                new[] { "Axis.Plugins.Registration.Controllers" }
                );

            routes.MapRoute("Plugin.RegistrationPlugins.PatientProfile",
                "Plugins/PatientProfile/{action}",
                new { controller = "PatientProfile", action = "General" },
                new[] { "Axis.Plugins.Registration.Controllers" }
                );


            routes.MapRoute("Plugin.RegistrationPlugins.ContactAddress",
                "Plugins/ContactAddress/{action}",
                new { controller = "ContactAddress", action = "Index" },
                new[] { "Axis.Plugins.Registration.Controllers" }
                );


            routes.MapRoute("Plugin.RegistrationPlugins.Referral.Header",
               "Plugins/Referrals/Header/{action}",
               new { controller = "ReferralHeader", action = "Index" },
               new[] { "Axis.Plugins.Registration.Controllers.Referrals" }
               );


            routes.MapRoute("Plugin.RegistrationPlugins.Referral.Demographics",
               "Plugins/Referrals/Demographics/{action}",
               new { controller = "ReferralDemographics", action = "Index" },
               new[] { "Axis.Plugins.Registration.Controllers.Referrals" }
               );


            routes.MapRoute("Plugin.RegistrationPlugins.Referral.Address",
               "Plugins/Referrals/Address/{action}",
               new { controller = "ReferralAddress", action = "Index" },
               new[] { "Axis.Plugins.Registration.Controllers.Referrals" }
               );

            routes.MapRoute("Plugin.RegistrationPlugins.Referral.Email",
               "Plugins/Referrals/Email/{action}",
               new { controller = "ReferralEmail", action = "Index" },
               new[] { "Axis.Plugins.Registration.Controllers.Referrals" }
               );

            routes.MapRoute("Plugin.RegistrationPlugins.Referral.Phone",
                "Plugins/Referrals/Phone/{action}",
                new { controller = "ReferralPhone", action = "Index" },
                new[] { "Axis.Plugins.Registration.Controllers.Referrals" }
                );

            routes.MapRoute("Plugin.RegistrationPlugins.Referral.ClientInformation",
              "Plugins/Referrals/ReferralClientInformation/{action}",
              new { controller = "ReferralClientInformation", action = "Index" },
              new[] { "Axis.Plugins.Registration.Controllers.Referrals" }
              );

            routes.MapRoute("Plugin.RegistrationPlugins.Referral.ReferralSearch",
                "Plugins/Referrals/ReferralSearch/{action}",
                new { controller = "ReferralSearch", action = "Index" },
                new[] { "Axis.Plugins.Registration.Controllers" }
                );

            routes.MapRoute("Plugin.RegistrationPlugins.Referral.Information",
              "Plugins/Referrals/Information/{action}",
              new { controller = "ReferralReferredInformation", action = "Index" },
              new[] { "Axis.Plugins.Registration.Controllers.Referrals" }
              );

            routes.MapRoute("Plugin.RegistrationPlugins.Referral.Forwarded",
             "Plugins/Referrals/Forwarded/{action}",
             new { controller = "ReferralForwarded", action = "Index" },
             new[] { "Axis.Plugins.Registration.Controllers.Referrals" }
             );

            routes.MapRoute("Plugin.RegistrationPlugins.Referral.Disposition",
              "Plugins/Referrals/Disposition/{action}",
              new { controller = "ReferralDisposition", action = "Index" },
              new[] { "Axis.Plugins.Registration.Controllers.Referrals" }
              );

            routes.MapRoute("Plugin.RegistrationPlugins.Referral.Followup",
                "Plugins/Referrals/Followup/{action}",
                new { controller = "ReferralFollowup", action = "Index" },
                new[] { "Axis.Plugins.Registration.Controllers.Referrals" }
                );

            routes.MapRoute("Plugin.RegistrationPlugins.Admission",
             "Plugins/Registration/{action}",
             new { controller = "Admission", action = "Index" },
             new[] { "Axis.Plugins.Registration.Controllers" }
             );

            routes.MapRoute("Plugin.RegistrationPlugins.DischargeProgramUnit",
                "Plugins/Registration/DischargeProgramUnit/{action}",
                new { controller = "DischargeProgramUnit", action = "Index" },
                new[] { "Axis.Plugins.Registration.Controllers.DischargeProgramUnit" }
            );

            routes.MapRoute("Plugin.RegistrationPlugins.DischargeCompany",
              "Plugins/Registration/DischargeCompany/{action}",
              new { controller = "DischargeCompany", action = "Index" },
              new[] { "Axis.Plugins.Registration.Controllers.DischargeCompany" }
            );

            routes.MapRoute("Plugin.RegistrationPlugins.SelfPay",
            "Plugins/Registration/{action}",
            new { controller = "SelfPay", action = "Index" },
            new[] { "Axis.Plugins.Registration.Controllers" }
           );

            routes.MapRoute("Plugin.CallCenter.RecordingService",
               "Plugins/Registration/RecordingService/{action}",
               new { controller = "ServiceRecording", action = "Index" },
               new[] { "Axis.Plugins.Registration.Controllers.RecordingService" }
            );

            routes.MapRoute("Plugin.RegistrationPlugins.HistoryLog",
               "Plugins/Registration/HistoryLog/{action}",
               new { controller = "HistoryLog", action = "Index" },
               new[] { "Axis.Plugins.Registration.Controllers.HistoryLog" }
            );

            ViewEngines.Engines.Add(new CustomViewEngine());
        }

        public int Priority
        {
            get
            {
                return 0;
            }
        }
    }
}
