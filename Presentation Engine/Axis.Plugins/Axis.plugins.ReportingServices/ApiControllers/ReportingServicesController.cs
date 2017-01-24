using Axis.Model.Common;
using Axis.Model.ReportingServices;
using Axis.Plugins.ReportingServices.Respository;
using Axis.PresentationEngine.Helpers.Controllers;
using System.Web.Http;

namespace Axis.Plugins.ReportingServices.ApiControllers
{
    public class ReportingServicesController: BaseApiController
    {
        #region Class Variables

        private readonly IReportingServicesRepository _reportingServicesRepository;

        #endregion

        #region Constructors

        public ReportingServicesController(IReportingServicesRepository reportingServicesRepository)
        {
            this._reportingServicesRepository = reportingServicesRepository;
        }

        #endregion

        #region Public Methods

        [HttpGet]
        public Response<SsrsFolderInfo> GetAllReports()
        {
            return _reportingServicesRepository.GetAllReports();
        }

        [HttpPost]
        public Response<SsrsReportInfo> GetReportByID(string reportId)
        {
            return _reportingServicesRepository.GetReportByID(reportId);
        }

        [HttpPost]
        public Response<SsrsReportParam> LoadReportParams(RunSsrsReportModel reportParams)
        {
            return _reportingServicesRepository.LoadReportParams(reportParams);
        }

        [HttpGet]
        public Response<ReportingServicesModel> GetReportsByType(string reportTypeName)
        {
            var result = _reportingServicesRepository.GetReportsByType(reportTypeName);
            return result;
        }
        
        #endregion
    }
}
