using Axis.Configuration;
using Axis.Helpers;
using Axis.Model.Logging;
using Axis.Service;
using Axis.PresentationEngine.Helpers.Repositories;

namespace Axis.PresentationEngine.Areas.Logging.Repository
{
    public class LoggingRepository : ILoggingRepository
    {
        private readonly CommunicationManager communicationManager;
        private const string baseRoute = "logging/";

        public LoggingRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        public void LogException(ExceptionModel exception)
        {
            string apiUrl = baseRoute + "logException";
            communicationManager.Post<ExceptionModel>(exception, apiUrl);
        }

        public void LogActivity(ActivityModel activity)
        {
            string apiUrl = baseRoute + "logActivity";
            communicationManager.Post<ActivityModel>(activity, apiUrl);
        }
    }
}