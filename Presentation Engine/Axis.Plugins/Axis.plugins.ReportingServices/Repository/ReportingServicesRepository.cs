using Axis.Configuration;
using Axis.Helpers;
using Axis.Model.Common;
using Axis.Model.ReportingServices;
using Axis.Service;
using System.Collections.Specialized;

namespace Axis.Plugins.ReportingServices.Respository
{
    public class ReportingServicesRepository : IReportingServicesRepository
    {
        /// <summary>
        /// The _communication manager
        /// </summary>
        private readonly CommunicationManager _communicationManager;


        /// <summary>
        /// The base route
        /// </summary>
        private const string BaseRoute = "Reporting/";
        //private const string BaseRoute = "Lookup/";


        public ReportingServicesRepository()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        public ReportingServicesRepository(string token)
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        public Response<SsrsFolderInfo> GetAllReports()
        {
            const string apiUrl = BaseRoute + "GetAllReports";
            return _communicationManager.Get<Response<SsrsFolderInfo>>(apiUrl);
        }

        public Response<SsrsReportInfo> GetReportByID(string reportId)
        {
            const string apiUrl = BaseRoute + "GetReportByID";
            var requestXMLValueNvc = new NameValueCollection { { "reportId", reportId } };
            return _communicationManager.Get<Response<SsrsReportInfo>>(requestXMLValueNvc, apiUrl);
        }

        public Response<SsrsReportParam> LoadReportParams(RunSsrsReportModel reportParams)
        {
            const string apiUrl = BaseRoute + "LoadReportParams";
            return _communicationManager.Post<RunSsrsReportModel, Response<SsrsReportParam>>(reportParams, apiUrl);
        }

        public RunSsrsReportModel RunReport(RunSsrsReportModel reportParams)
        {
            const string apiUrl = BaseRoute + "RunReport";
            return _communicationManager.Post<RunSsrsReportModel, RunSsrsReportModel>(reportParams, apiUrl);
        }

        public Response<ReportingServicesModel> GetReportsByType(string reportTypeName)
        {
            const string apiUrl = BaseRoute + "GetReportsByType";
            var requestXMLValueNvc = new NameValueCollection { { "reportTypeName", reportTypeName } };
            return _communicationManager.Get<Response<ReportingServicesModel>>(requestXMLValueNvc, apiUrl);
        }
    }
}
