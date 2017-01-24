using Axis.Logging;
using Axis.Security;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using System;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace Axis.DataEngine.Service.Filters
{
    public class LogActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.ActionContext.ActionDescriptor.GetCustomAttributes<SkipLogActionFilterAttribute>().Any())
                return;

            var enableAppInsights = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("EnableAppInsights"));
            if (!enableAppInsights)
                return;

            var telemetry = new TelemetryClient();
            telemetry.InstrumentationKey = ConfigurationManager.AppSettings.Get("AppInsightsKey");
            var rt = new RequestTelemetry();
            rt.Name = "Data Engine -" + actionExecutedContext.ActionContext.ControllerContext.ControllerDescriptor.ControllerName + "-" + actionExecutedContext.ActionContext.ActionDescriptor.ActionName;
            rt.HttpMethod = actionExecutedContext.Request.Method.Method;
            rt.Url = actionExecutedContext.Request.RequestUri;
            rt.ResponseCode = actionExecutedContext.Response.ReasonPhrase;
            rt.Success = actionExecutedContext.Response.IsSuccessStatusCode;
            rt.Context.User.Id = AuthContext.Auth.User.UserName;
            rt.Properties.Add("Controller: ", actionExecutedContext.ActionContext.ControllerContext.ControllerDescriptor.ControllerName);
            rt.Properties.Add("Action: ", actionExecutedContext.ActionContext.ActionDescriptor.ActionName);

            telemetry.TrackRequest(rt);
        }
    }
}