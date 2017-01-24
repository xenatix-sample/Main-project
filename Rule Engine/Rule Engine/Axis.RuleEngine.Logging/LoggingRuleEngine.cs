using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axis.Service.Logging;
using Axis.Model.Logging;

namespace Axis.RuleEngine.Logging
{
    public class LoggingRuleEngine : ILoggingRuleEngine
    {
        private ILoggingService loggingService;
        public LoggingRuleEngine(ILoggingService loggingService)
        {
            this.loggingService = loggingService;
        }

        public void LogException(ExceptionModel exception)
        {
            loggingService.LogException(exception);
        }

        public void LogActivity(ActivityModel activity)
        {
            loggingService.LogActivity(activity);
        }
    }
}
