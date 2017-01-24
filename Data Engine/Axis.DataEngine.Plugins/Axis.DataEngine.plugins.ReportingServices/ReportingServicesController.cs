using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.ReportingServices;
using Axis.Model.Common;
using Axis.Model.ReportingServices;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Axis.DataEngine.Plugins.ReportingServices
{
    public class ReportingServicesController : BaseApiController
    {
        private readonly IReportingServicesDataProvider _ReportingServicesDataProvider;

        public ReportingServicesController(IReportingServicesDataProvider reportingServicesDataProvider)
        {
            _ReportingServicesDataProvider = reportingServicesDataProvider;
        }

        [HttpGet]
        public IHttpActionResult GetReportsByType(string reportTypeName)
        {
            var reportResponse = _ReportingServicesDataProvider.GetReportsByType(reportTypeName);
            return new HttpResult<Response<ReportingServicesModel>>(reportResponse, Request);
        }

        [HttpGet]
        public HttpResponseMessage GetReportsByGroupId(int reportGroupID)
        {
            var reportingServicesGroupModel = _ReportingServicesDataProvider.GetReportGroupDetail(reportGroupID);

            try
            {
                if (reportingServicesGroupModel != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, reportingServicesGroupModel);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "reportingServicesGroupModel not returning reports.");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Error Occured During reportingServicesGroupModel Retrival");
            }
        }
    }
}