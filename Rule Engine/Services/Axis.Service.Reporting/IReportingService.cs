
using Axis.Model.Common;
using Axis.Model.ReportingServices;

namespace Axis.Service.Reporting
{
    /// <summary>
    ///
    /// </summary>
    public interface IReportingService
    {
        Response<SsrsFolderInfo> GetAllReports();

        Response<SsrsReportInfo> GetReportByID(string reportId);

        Response<SsrsReportParam> LoadReportParams(RunSsrsReportModel reportParams);

        RunSsrsReportModel RunReport(RunSsrsReportModel reportParams);

        Response<ReportingServicesModel> GetReportsByType(string reportTypeName);
    }
}