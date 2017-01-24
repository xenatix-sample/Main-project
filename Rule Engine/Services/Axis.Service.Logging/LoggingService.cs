using Axis.Configuration;
using Axis.Model.Logging;
using Axis.Security;

namespace Axis.Service.Logging
{
    public class LoggingService : ILoggingService
    {
        private readonly CommunicationManager communicationManager;
        private const string baseRoute = "logging/";

        public LoggingService()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        public void LogException(ExceptionModel exception)
        {
            var apiUrl = baseRoute + "logException";
            communicationManager.Post<ExceptionModel, int>(exception, apiUrl);
        }

        public void LogActivity(ActivityModel activity)
        {
            var apiUrl = baseRoute + "logActivity";
            communicationManager.Post<ActivityModel, int>(activity, apiUrl);
        }
    }
}
