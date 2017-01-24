using Axis.Model.Common;
using Axis.Model.ReportingServices;

namespace Axis.DataProvider.ReportingServices
{
    public interface IReportingServicesDataProvider
    {
        /// <summary>
        /// Collect the reports from Reporting Services BasedOn Report TypeName
        /// /// </summary>
        /// <remarks>
        /// SP:Core.usp_GetReportsByType
        /// </remarks>
        /// <param name="ReportTypeName"></param>
        /// <returns></returns>
        Response<ReportingServicesModel> GetReportsByType(string reportTypeName);

        /// <summary>
        /// Collect the reports group based on groupID.
        /// </summary>
        /// <remarks>
        /// SP:Reference.usp_GetReportGroupDetails
        /// </remarks>
        /// <param name="reportGroupID"></param>
        /// <returns></returns>
        Response<ReportingServicesGroupModel> GetReportGroupDetail(int reportGroupID);
    }
}