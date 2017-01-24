using Axis.DataEngine.Service.Filters;
using Axis.DataEngine.Service.Helpers;
using System.Web.Http;
using System.Web.Http.Dispatcher;

namespace Axis.DataEngine.Service
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
                name: "GetValidUserInfoByToken",
                routeTemplate: "api/account/getValidUserInfoByToken",
                defaults: new { controller = "Account", action = "GetValidUserInfoByToken" },
                constraints: null
            );

            config.Routes.MapHttpRoute(
                name: "LogAccessToken",
                routeTemplate: "api/account/logAccessToken",
                defaults: new { controller = "Account", action = "LogAccessToken" },
                constraints: null
            );

            config.Routes.MapHttpRoute(
                name: "SetLoginData",
                routeTemplate: "api/account/setLoginData",
                defaults: new { controller = "Account", action = "SetLoginData" },
                constraints: null
            );

            config.Routes.MapHttpRoute(
                 name: "GetFilesToCache",
                 routeTemplate: "api/Manifest/GetFilesToCache",
                 defaults: new { controller = "Manifest", action = "GetFilesToCache" },
                 constraints: null
            );

            config.Routes.MapHttpRoute(
                 name: "VerifyRolePermission",
                 routeTemplate: "api/Security/VerifyRolePermission",
                 defaults: new { controller = "Security", action = "VerifyRolePermission" },
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
                 name: "GetSettingsToCache",
                 routeTemplate: "api/settings/getSettingsToCache",
                 defaults: new { controller = "Settings", action = "GetSettingsToCache" },
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