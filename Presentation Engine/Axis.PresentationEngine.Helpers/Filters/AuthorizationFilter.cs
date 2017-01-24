using System;
using System.Web.Mvc;
using Axis.Helpers.Infrastructure;
using Axis.Logging;

namespace Axis.PresentationEngine.Helpers.Filters
{
    /// <summary>
    ///  Used for authenticate/authorize each incoming request to IIS
    /// </summary>
    public class Authorization : FilterAttribute, IAuthorizationFilter
    {
        // Set AllowAnonymous to true for bypassing authentication for any controller action.
        public bool AllowAnonymous { get; set; }

        /// <summary>
        /// Default constructors for security filter
        /// </summary>
        public Authorization()
        {

        }

        /// <summary>
        /// Parameterized constructor for security filter 
        /// </summary>
        /// <param name="AllowAnonymous">Provide true to bypass security</param>
        public Authorization(bool AllowAnonymous)
        {
            this.AllowAnonymous = AllowAnonymous;
        }

        /// <summary>
        /// Authenticate and authorize every request to IIS
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            #if DEBUG
            if (filterContext.HttpContext.Request.Headers["X-Fake-Offline"] == Boolean.TrueString.ToLower())
            {
                filterContext.HttpContext.Response.StatusCode = 0;
                filterContext.Result = new HttpStatusCodeResult(0);
                return;
            }
            #endif

            var logger = EngineContext.Current.Resolve<ILogger>();
            logger.Info("Presentation Engine - OnAuthorization - filterContext action name: " + filterContext.ActionDescriptor.ActionName + " Username is " + WebSecurity.GetUserName());

            string UserName;
            string[] Roles;

            //Check if request is authenticated
            if (WebSecurity.IsAuthenticated())
            {
                //Gets username and associated roles of authenticated user
                logger.Info("Presentation Engine - OnAuthorization - Username " + WebSecurity.GetUserName() + " is Authenticated");
                UserName = WebSecurity.GetUserName();
                Roles = WebSecurity.GetRoles();

                //check roles based access from database
            }
            else
            {
                logger.Info("Presentation Engine - OnAuthorization - Username " + WebSecurity.GetUserName() + " is NOT Authenticated");
                var controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                var action = filterContext.ActionDescriptor.ActionName;

                if (!AllowAnonymous && filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    logger.Info("Presentation Engine - OnAuthorization - Is Ajax Request, returning BadRequest Code = " + System.Net.HttpStatusCode.BadRequest);
                    filterContext.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                    filterContext.Result = new EmptyResult();
                }
                else if (!(controller == "Account" && action == "Logon") && !AllowAnonymous)
                {
                    logger.Info("Presentation Engine - OnAuthorization - Is Ajax Request, returning BadRequest Code = " + System.Net.HttpStatusCode.BadRequest);
                    var requestedPath = filterContext.HttpContext.Request.RawUrl;
                    requestedPath = "?ReturnUrl=" + requestedPath;

                    filterContext.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;

                    logger.Info("Presentation Engine - OnAuthorization - Redirecting to " + requestedPath);
                    filterContext.Result = new RedirectResult("/Account/Account/Login" + requestedPath);
                }
            }
        }
    }
}