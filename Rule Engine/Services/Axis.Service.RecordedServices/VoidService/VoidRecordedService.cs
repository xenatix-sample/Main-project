using Axis.Configuration;
using Axis.Model.RecordedServices;
using Axis.Model.Common;
using System.Collections.Specialized;
using System.Diagnostics;
using Axis.Security;
using System.Globalization;

namespace Axis.Service.RecordedServices.VoidService
{
    public class VoidRecordedService : IVoidRecordedService
    {
        private CommunicationManager communicationManager;
        private const string baseRoute = "voidservice/";

        public VoidRecordedService()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        public Response<VoidServiceModel> VoidService(VoidServiceModel voidService)
        {
            var apiUrl = baseRoute + "/VoidRecordedService";
            return communicationManager.Post<VoidServiceModel,Response<VoidServiceModel>>(voidService, apiUrl);
        }

        public Response<VoidServiceModel> VoidServiceCallCenter(VoidServiceModel voidService)
        {
            var apiUrl = baseRoute + "/VoidServiceCallCenter";
            return communicationManager.Post<VoidServiceModel, Response<VoidServiceModel>>(voidService, apiUrl);
        }

        public Response<VoidServiceModel> GetVoidService(int serviceRecordingID)
        {
            var apiUrl = baseRoute + "GetVoidService";
            var serviceRecID = new NameValueCollection { { "serviceRecordingID", serviceRecordingID.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Get<Response<VoidServiceModel>>(serviceRecID, apiUrl);
        }
    }
}
