
using Axis.Configuration;
using Axis.Security;

namespace Axis.Service.Reporting
{
    /// <summary>
    ///
    /// </summary>
    public class ReportingService : IReportingService
    {
        private readonly CommunicationManager _communicationManager;

        public ReportingService()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        #region Public Methods

       

        #endregion

    }
}