using Axis.Model.Common;
using Axis.Model.ReportingServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Plugins.ReportingServices.Respository
{
    public interface IReportingServicesRepository
    {
        /// <summary>
        /// Collect the reports from Reporting Services BasedOn Report TypeName
        /// /// </summary>
        /// <remarks>
        /// SP:Core.usp_GetReportsByType
        /// </remarks>
        /// <param name="ReportTypeName"></param>
        /// <returns></returns>
        Task<Response<ReportingServicesModel>> GetReportsByType(string ReportTypeName);

        /// <summary>
        /// Collect the reports group based on groupID.
        /// </summary>
        /// <remarks>
        /// SP:Reference.usp_GetReportGroupDetails
        /// </remarks>
        /// <param name="reportGroupID"></param>
        /// <returns></returns>
        Task<Response<ReportingServicesGroupModel>> GetReportGroupDetail(int reportGroupID);

    }
}
