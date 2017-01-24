using Axis.Model.Common;
using Axis.RuleEngine.Helpers.Results;
using Axis.RuleEngine.Reporting;
using System.Web.Http;
using Axis.Model.ReportingServices;

namespace Axis.RuleEngine.Service.Controllers
{
    public class ReportingController : ApiController
    {
        private readonly IReportingRuleEngine _ReportingRuleEngine = null;

        public ReportingController(IReportingRuleEngine reportingRuleEngine)
        {
            _ReportingRuleEngine = reportingRuleEngine;
        }

        [HttpGet]
        public IHttpActionResult GetAllReports()
        {
            var responseObject = _ReportingRuleEngine.GetAllReports();
            return new HttpResult<Response<SsrsFolderInfo>>(responseObject, Request);
        }

        [HttpGet]
        public IHttpActionResult GetReportByID(string reportId)
        {
            var responseObject = _ReportingRuleEngine.GetReportByID(reportId);
            return new HttpResult<Response<SsrsReportInfo>>(responseObject, Request);
        }

        [HttpPost]
        public IHttpActionResult LoadReportParams(RunSsrsReportModel reportParams)
        {
            var responseObject = _ReportingRuleEngine.LoadReportParams(reportParams);
            return new HttpResult<Response<SsrsReportParam>>(responseObject, Request);
        }

        [HttpPost]
        public IHttpActionResult RunReport(RunSsrsReportModel reportParams)
        {
            var responseObject = _ReportingRuleEngine.RunReport(reportParams);
            return new HttpResult<RunSsrsReportModel>(responseObject, Request);
        }

        [HttpGet]
        public IHttpActionResult GetReportsByType(string reportTypeName)
        {
            var responseObject = _ReportingRuleEngine.GetReportsByType(reportTypeName);
            return new HttpResult<Response<ReportingServicesModel>>(responseObject, Request);
        }

    }
}
