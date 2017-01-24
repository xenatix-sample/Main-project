using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;
using Axis.Model.Logging;
using System.Net.Http;
using System.Web.Http;
using Axis.RuleEngine.Logging;
using Axis.Configuration;
using Axis.Constant;
using Axis.Logging;
using Axis.Helpers.Infrastructure;

namespace Axis.RuleEngine.Service.Filters
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext filterContext)
        {

            //HttpStatusCode status = HttpStatusCode.InternalServerError;


            var exType = filterContext.Exception.GetType();

            //if (exType == typeof(UnauthorizedAccessException))
            //    status = HttpStatusCode.Unauthorized;
            //else if (exType == typeof(ArgumentException))
            //    status = HttpStatusCode.NotFound;

            if (exType != typeof(XenatixException))
            {
                if (ApplicationSettings.EnableLogging)
                {

                    var logger = System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(ILogger)) as ILogger;
                    string hostAddress = "";
                    string sURL = "";
                    try
                    {
                        hostAddress = ((HttpContextWrapper)filterContext.Request.Properties["MS_HttpContext"]).Request.UserHostAddress;
                        sURL = filterContext.Request.RequestUri.AbsoluteUri.ToString();
                    }
                    catch (Exception e)
                    {
                        //We were just swallowing the exception
                        logger.Error(e);
                        hostAddress = "unknown";// Need to change it
                        sURL = "unknown";
                    }
                    //log the error in file
                    logger.Error(hostAddress + " :: " + sURL + " :: " + filterContext.Exception.Message, filterContext.Exception);

                    if (ApplicationSettings.LoggingMode == (int)LoggingMode.DataBase)
                    {
                        // log the error in data base
                        var loggingRuleEngine = System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(ILoggingRuleEngine)) as ILoggingRuleEngine;
                        loggingRuleEngine.LogException(new ExceptionModel()
                        {
                            Message = filterContext.Exception.Message,
                            Source = filterContext.Exception.Source
                        });
                    }
                }
            }

            //Throwing a proper message for the client side
            string errorCode = "4001";
            var message = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent(errorCode),
                ReasonPhrase = errorCode
            };

            filterContext.Response = message;
            filterContext.Response.Headers.Add("ErrorCode", errorCode);

            base.OnException(filterContext);
        }
    }
}