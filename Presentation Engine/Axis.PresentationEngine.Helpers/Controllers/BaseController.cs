using Axis.Helpers.Caching;
using Axis.Helpers.Infrastructure;
using Axis.Logging;
using Axis.Model.Common;
using Axis.Model.Logging;
using ServiceStack.Text;
using System;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Axis.PresentationEngine.Helpers.Controllers
{
    public class BaseController : Controller
    {
        protected Logger _logger = new Logger();
        private const string keyPattern = "providers/";

        #region Constructors

        public BaseController()
        {
        }

        // For debugging purposes
        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
        }

        #endregion Constructors

        #region Protected Methods

        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new ServiceStackJsonResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding
            };
        }

        public class ServiceStackJsonResult : JsonResult
        {
            public override void ExecuteResult(ControllerContext context)
            {
                JsConfig.DateHandler = DateHandler.ISO8601;
                JsConfig.TreatEnumAsInteger = true;

                HttpResponseBase response = context.HttpContext.Response;
                response.ContentType = !String.IsNullOrEmpty(ContentType) ? ContentType : "application/json";

                if (ContentEncoding != null)
                {
                    response.ContentEncoding = ContentEncoding;
                }

                if (Data != null)
                {
                    response.Write(JsonSerializer.SerializeToString(Data));
                }
            }
        }

        #endregion Protected Methods

        #region Public Methods

        public virtual PartialViewResult GetPartial(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                Response.StatusCode = 404;
                Response.StatusDescription = "Not Found - Invalid Path";
            }

            if (path[0] == '/')
                path = path.Substring(1, path.Length - 1);

            if (path.Contains("/"))
                path = string.Format("~/Views/{0}.cshtml", path.Replace(".cshtml", string.Empty));
            else
                path = string.Format("~/Views/Shared/{0}.cshtml", path.Replace(".cshtml", string.Empty));

            return PartialView(path);
        }

        public void ClearCache<T>(Response<T> request)
        {
            ICacheManager cacheManager = EngineContext.Current.Resolve<ICacheManager>();

            if (request == null || request.ResultCode == 0)
            {
                cacheManager.RemoveByPattern(keyPattern);
            }
        }
        #endregion Public Methods
    }
}
