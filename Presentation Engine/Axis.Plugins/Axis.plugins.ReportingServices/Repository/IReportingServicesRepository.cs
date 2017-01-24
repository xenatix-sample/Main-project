using Axis.Model.Common;
using Axis.Model.ReportingServices;
using System.Collections.Generic;

namespace Axis.Plugins.ReportingServices.Respository
{
    public interface IReportingServicesRepository
    {
        /// <summary>
        /// Collect all reports from Reporting Services
        /// </summary>
        /// <remarks>
        /// Uses SSRS Web Service
        /// </remarks>
        /// <returns></returns>
        Response<SsrsFolderInfo> GetAllReports();

        /// <summary>
        /// Collect all reports from Reporting Services
        /// </summary>
        /// <remarks>
        /// Uses SSRS Web Service
        /// </remarks>
        /// <param name="reportId"></param>
        /// <returns></returns>
        Response<SsrsReportInfo> GetReportByID(string reportId);

        /// <summary>
        /// Gets base parameters for 
        /// </summary>
        /// <remarks>
        /// Uses SSRS Web Service
        /// </remarks>
        /// <param name="reportId"></param>
        /// <returns></returns>
        Response<SsrsReportParam> LoadReportParams(RunSsrsReportModel reportParams);

        /// <summary>
        /// Executes a specified report with any included parameters.
        /// </summary>
        /// <remarks>
        /// Uses SSRS Web Service
        /// </remarks>
        /// <param name="reportParams"></param>
        /// <returns>
        /// Returns generated report in specified format.
        /// </returns>
        RunSsrsReportModel RunReport(RunSsrsReportModel reportParams);

        /// <summary>
        /// Collect the reports from Reporting Services BasedOn Report TypeName
        /// /// </summary>
        /// <remarks>
        /// SP:Core.usp_GetReportsByType
        /// </remarks>
        /// <param name="ReportTypeName"></param>
        /// <returns></returns>
        Response<ReportingServicesModel> GetReportsByType(string ReportTypeName);
    }
}
