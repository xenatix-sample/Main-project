using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Axis.Model.Logging;
using Axis.RuleEngine.Logging;
using Axis.RuleEngine.Helpers.Results;

namespace Axis.RuleEngine.Service.Controllers
{
    public class LoggingController : ApiController
    {
        ILoggingRuleEngine loggingRuleEngine;
        public LoggingController(ILoggingRuleEngine loggingRuleEngine)
        {
            this.loggingRuleEngine = loggingRuleEngine;
        }

        [HttpPost]
        public IHttpActionResult LogException(ExceptionModel exception)
        {
            loggingRuleEngine.LogException(exception);
            return new HttpResult<int>((int)HttpStatusCode.Created, Request);
        }

        [HttpPost]
        public IHttpActionResult LogActivity(ActivityModel activity)
        {
            loggingRuleEngine.LogActivity(activity);
            return new HttpResult<int>((int)HttpStatusCode.Created, Request);
        }
    }
}
