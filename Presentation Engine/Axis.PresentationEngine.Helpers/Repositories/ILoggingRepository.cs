using Axis.Model.Logging;

namespace Axis.PresentationEngine.Helpers.Repositories
{
    public interface ILoggingRepository
    {
        void LogException(ExceptionModel exception);
        void LogActivity(ActivityModel activity);
    }
}
