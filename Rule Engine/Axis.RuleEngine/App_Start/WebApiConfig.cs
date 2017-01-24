using Axis.RuleEngine.Service.Filters;
using Axis.RuleEngine.Service.Helpers;
using System.Web.Http;
using System.Web.Http.Dispatcher;

namespace Axis.RuleEngine.Service
{
    public static class WebApiConfig
    {
        public const string ControllerOnlyRoute = "ControllerOnlyRoute";
        public const string ControllerWithIdRoute = "ControllerWithId";
        public const string ControllerWithActionRoute = "ControllerWithAction";
        public const string ControllerWithActionAndIdRoute = "ControllerWithActionAndId";

        public static void Register(HttpConfiguration config)
        {
            //Register token inspector
            TokenInspector tokenInspector = new TokenInspector() { InnerHandler = new HttpControllerDispatcher(config) };

            // Web API configuration and services
            config.EnableCors();

            // Web API routes
            config.MapHttpAttributeRoutes();

            #region Anonymous Route

            config.Routes.MapHttpRoute(
                name: "Authentication",
                routeTemplate: "api/account/authenticate",
                defaults: new { controller = "Account", action = "Authenticate" },
                constraints: null
            );

            config.Routes.MapHttpRoute(
                name: "IsValidServerIP",
                routeTemplate: "api/account/isValidServerIP",
                defaults: new { controller = "Account", action = "IsValidServerIP" },
                constraints: null
            );

            config.Routes.MapHttpRoute(
                name: "GetTokenIssueExpireDate",
                routeTemplate: "api/account/getTokenIssueExpireDate",
                defaults: new { controller = "Account", action = "GetTokenIssueExpireDate" },
                constraints: null
            );

            config.Routes.MapHttpRoute(
               name: "SendResetLink",
               routeTemplate: "api/forgotPassword/sendResetLink",
               defaults: new { controller = "ForgotPassword", action = "SendResetLink" },
               constraints: null
           );

            config.Routes.MapHttpRoute(
               name: "GetSecurityQuestions",
               routeTemplate: "api/forgotPassword/getSecurityQuestions",
               defaults: new { controller = "ForgotPassword", action = "GetSecurityQuestions" },
               constraints: null
           );

            config.Routes.MapHttpRoute(
               name: "VerifySecurityDetails",
               routeTemplate: "api/forgotPassword/verifySecurityDetails",
               defaults: new { controller = "ForgotPassword", action = "VerifySecurityDetails" },
               constraints: null
           );

            config.Routes.MapHttpRoute(
               name: "ResetPassword",
               routeTemplate: "api/forgotPassword/resetPassword",
               defaults: new { controller = "ForgotPassword", action = "ResetPassword" },
               constraints: null
           );

            config.Routes.MapHttpRoute(
               name: "VerifyResetIdentifier",
               routeTemplate: "api/forgotPassword/verifyResetIdentifier",
               defaults: new { controller = "ForgotPassword", action = "VerifyResetIdentifier" },
               constraints: null
           );

            config.Routes.MapHttpRoute(
               name: "GetSettingsToCacheOnStart",
               routeTemplate: "api/settings/getSettingsToCacheOnStart",
               defaults: new { controller = "Settings", action = "GetSettingsToCacheOnStart" },
               constraints: null
           );

            #endregion Anonymous Route

            #region Secure Route

            config.Routes.MapHttpRoute(
                name: ControllerOnlyRoute,
                routeTemplate: "api/{controller}",
                defaults: null,
                constraints: null,
                handler: tokenInspector
            );

            config.Routes.MapHttpRoute(
                name: ControllerWithIdRoute,
                routeTemplate: "api/{controller}/{id}",
                defaults: null,
                constraints: new { id = @"\d+" },
                handler: tokenInspector
            );

            config.Routes.MapHttpRoute(
                name: ControllerWithActionRoute,
                routeTemplate: "api/{controller}/{action}",
                defaults: null,
                constraints: null,
                handler: tokenInspector
            );

            config.Routes.MapHttpRoute(
                name: ControllerWithActionAndIdRoute,
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: null,
                constraints: null,
                handler: tokenInspector
            );

            #endregion Secure Route

            config.Filters.Add(new ExceptionFilter());
            config.Filters.Add(new LogActionFilter());
        }
    }
}