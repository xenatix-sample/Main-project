using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axis.Model.Common;
using Axis.Model.ReportingServices;
using System.Collections.Specialized;

using System.Globalization;
using Axis.Service;

using Axis.Configuration;
using Axis.Security;

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
        private const string BaseRoute = "ReportingServices/";


        public ReportingServicesRepository()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }


        async Task<Response<ReportingServicesModel>> IReportingServicesRepository.GetReportsByType(string ReportTypeName)
        {
            const string apiUrl = BaseRoute + "GetReportsByType";
            var requestXMLValueNvc = new NameValueCollection { { "ReportTypeName", ReportTypeName.ToString(CultureInfo.InvariantCulture) } };

            return await _communicationManager.GetAsync<Response<ReportingServicesModel>>(requestXMLValueNvc, apiUrl);
        }

        async Task<Response<ReportingServicesGroupModel>> IReportingServicesRepository.GetReportGroupDetail(int reportGroupID)
        {
            const string apiUrl = BaseRoute + "GetReportGroupDetail";
            var requestXMLValueNvc = new NameValueCollection { { "reportGroupID", reportGroupID.ToString(CultureInfo.InvariantCulture) } };
            return await _communicationManager.GetAsync<Response<ReportingServicesGroupModel>>(requestXMLValueNvc, apiUrl);
        }


       
       

       
    }
}
