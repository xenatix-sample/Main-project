using System.Web;
using System.Web.Mvc;
using Axis.Configuration;
using Axis.Constant;
using Axis.Logging;
using Axis.Model.Logging;
using Axis.Helpers.Infrastructure;
using Axis.PresentationEngine.Helpers.Repositories;

namespace Axis.PresentationEngine.Helpers.Filters
{
    public class ExceptionFilter : HandleErrorAttribute
    {
        public override void OnException(System.Web.Mvc.ExceptionContext filterContext)
        {
            var controllerName = string.Empty;
            var actionName = string.Empty;

            if (filterContext.ExceptionHandled)
            {
                return;
            }

            if (new HttpException(null, filterContext.Exception).GetHttpCode() != 500)
            {
                return;
            }

            if (!ExceptionType.IsInstanceOfType(filterContext.Exception))
            {
                return;
            }

            // if the request is AJAX return JSON else view.
            if (filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                filterContext.Result = new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new
                    {
                        error = true,
                        message = filterContext.Exception.Message
                    }
                };
            }
            else
            {
                controllerName = (string)filterContext.RouteData.Values["controller"];
                actionName = (string)filterContext.RouteData.Values["action"];
                var model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);

                filterContext.Result = new ViewResult
                {
                    ViewName = View,
                    MasterName = Master,
                    ViewData = new ViewDataDictionary<HandleErrorInfo>(model),
                    TempData = filterContext.Controller.TempData
                };
            }

            if (filterContext.Exception.GetType() != typeof(XenatixException))
            {
                if (ApplicationSettings.EnableLogging)
                {
                   
                    var logger = EngineContext.Current.Resolve<ILogger>();

                    //log the error in file
                    logger.Error(controllerName + " > " + actionName + " - " + filterContext.Exception.Message, filterContext.Exception);
                   
                    if (ApplicationSettings.LoggingMode == (int)LoggingMode.DataBase)
                    {
                        // log the error in data base
                        ILoggingRepository loggingRepository = EngineContext.Current.Resolve<ILoggingRepository>();
                        loggingRepository.LogException(new ExceptionModel()
                        {
                            Message = filterContext.Exception.Message,
                            Source = filterContext.Exception.Source,
                            Comments = string.Empty
                        });
                    }
                }
            }

            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.StatusCode = 500;

            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }

    }
}