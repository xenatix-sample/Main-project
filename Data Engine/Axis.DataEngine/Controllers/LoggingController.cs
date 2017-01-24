using System.Net;
using System.Web.Http;
using Axis.DataProvider.Logging;
using Axis.Model.Logging;
using Axis.DataEngine.Helpers.Results;

namespace Axis.DataEngine.Service.Controllers
{
    public class LoggingController : ApiController
    {
        ILoggingDataProvider loggingDataProvider;
        public LoggingController(ILoggingDataProvider loggingDataProvider)
        {
            this.loggingDataProvider = loggingDataProvider;
        }

        [HttpPost]
        public IHttpActionResult LogException(ExceptionModel exception)
        {
            loggingDataProvider.LogException(exception);
            return new HttpResult<int>((int)HttpStatusCode.Created, Request);
        }

        [HttpPost]
        public IHttpActionResult LogActivity(ActivityModel activity)
        {
            loggingDataProvider.LogActivity(activity);
            return new HttpResult<int>((int)HttpStatusCode.Created, Request);
        }
    }
}
