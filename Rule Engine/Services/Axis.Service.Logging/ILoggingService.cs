using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axis.Model.Logging;

namespace Axis.Service.Logging
{
    public interface ILoggingService
    {
        void LogException(ExceptionModel exception);
        void LogActivity(ActivityModel activity);
    }
}
