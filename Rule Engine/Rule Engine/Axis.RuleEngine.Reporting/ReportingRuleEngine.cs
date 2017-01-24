using Axis.Model.Common;
using Axis.Model.ReportingServices;
using Axis.Service.Reporting;

namespace Axis.RuleEngine.Reporting
{
    /// <summary>
    ///
    /// </summary>
    public class ReportingRuleEngine : IReportingRuleEngine
    {
        #region Class Variables

        private IReportingService _reportingServiceService;

        #endregion

        #region Constructors

        public ReportingRuleEngine(IReportingService reportingService)
        {
            _reportingServiceService = reportingService;
        }

        #endregion

        #region Public Methods

        public Response<SsrsFolderInfo> GetAllReports()
        {
            return _reportingServiceService.GetAllReports();
        }

        public Response<SsrsReportInfo> GetReportByID(string reportId)
        {
            return _reportingServiceService.GetReportByID(reportId);
        }

        public Response<SsrsReportParam> LoadReportParams(RunSsrsReportModel reportParams)
        {
            return _reportingServiceService.LoadReportParams(reportParams);
        }

        public RunSsrsReportModel RunReport(RunSsrsReportModel reportParams)
        {
            return _reportingServiceService.RunReport(reportParams);
        }

        public Response<ReportingServicesModel> GetReportsByType(string reportTypeName)
        {
            return _reportingServiceService.GetReportsByType(reportTypeName);
        }

        #endregion
    }
}