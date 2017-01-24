using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Axis.Helpers.Infrastructure;
using Axis.Logging;
using System.Web.Http.Controllers;
using HttpFilters = System.Web.Http.Filters;

namespace Axis.PresentationEngine.Helpers.Filters
{
    /// <summary>
    ///  Used for authenticate/authorize each incoming request to IIS
    /// </summary>
    public class HttpAuthorization : HttpFilters.FilterAttribute, HttpFilters.IAuthorizationFilter
    {
        // Set AllowAnonymous to true for bypassing authentication for any controller action.
        public bool AllowAnonymous { get; set; }

        /// <summary>
        /// Default constructors for security filter
        /// </summary>
        public HttpAuthorization()
        {

        }

        /// <summary>
        /// Parameterized constructor for security filter 
        /// </summary>
        /// <param name="AllowAnonymous">Provide true to bypass security</param>
        public HttpAuthorization(bool AllowAnonymous)
        {
            this.AllowAnonymous = AllowAnonymous;
        }

        public Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(HttpActionContext actionContext,
            System.Threading.CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
#if DEBUG
            if (actionContext.Request.Headers.Contains("X-Fake-Offline") &&
                actionContext.Request.Headers.GetValues("X-Fake-Offline").First() == Boolean.TrueString.ToLower())
            {
                actionContext.Response = new HttpResponseMessage(0);
                return Task.FromResult(actionContext.Response);
            }
#endif

            var logger = EngineContext.Current.Resolve<ILogger>();
            logger.Info("Presentation Engine - ExecuteAuthorizationFilterAsync - actionContext action name: " +
                        actionContext.ActionDescriptor.ActionName);

            //Check if request is authenticated
            if (WebSecurity.IsAuthenticated())
            {
                //Gets username and associated roles of authenticated user
                logger.Info("Presentation Engine - ExecuteAuthorizationFilterAsync - Username " +
                            WebSecurity.GetUserName() + " is Authenticated");
            }
            else if (AllowAnonymous)
            {
                logger.Info(
                    "Presentation Engine - ExecuteAuthorizationFilterAsync - User is not Authenticated but Anonymous is Allowed");
            }
            else
            {
                logger.Info("Presentation Engine - ExecuteAuthorizationFilterAsync - User is not Authenticated");
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unauthorized");
                return Task.FromResult(actionContext.Response);
            }

            return continuation();
        }
    }
}