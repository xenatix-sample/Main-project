
using Axis.Model.Common;
using Axis.Model.ReportingServices;

namespace Axis.RuleEngine.Reporting
{
    /// <summary>
    ///
    /// </summary>
    public interface IReportingRuleEngine
    {
        Response<SsrsFolderInfo> GetAllReports();

        Response<SsrsReportInfo> GetReportByID(string reportId);

        Response<SsrsReportParam> LoadReportParams(RunSsrsReportModel reportParams);

        RunSsrsReportModel RunReport(RunSsrsReportModel reportParams);

        Response<ReportingServicesModel> GetReportsByType(string reportTypeName);
    }
}