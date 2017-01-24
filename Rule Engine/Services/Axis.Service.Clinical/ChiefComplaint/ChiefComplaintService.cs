using System;
using Axis.Configuration;
using Axis.Model.Clinical;
using Axis.Model.Common;
using Axis.Security;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.Service.Clinical.ChiefComplaint
{
    public class ChiefComplaintService : IChiefComplaintService
    {
        private readonly CommunicationManager _communicationManager;

        private const string BaseRoute = "chiefComplaint/";

        public ChiefComplaintService()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        public Response<ChiefComplaintModel> GetChiefComplaint(long contactID)
        {
            const string apiUrl = BaseRoute + "GetChiefComplaint";
            var requestXMLValueNvc = new NameValueCollection { { "ContactId", contactID.ToString(CultureInfo.InvariantCulture) } };
            return _communicationManager.Get<Response<ChiefComplaintModel>>(requestXMLValueNvc, apiUrl);
        }

        public Response<ChiefComplaintModel> GetChiefComplaints(long contactID)
        {
            const string apiUrl = BaseRoute + "GetChiefComplaints";
            var requestXMLValueNvc = new NameValueCollection { { "ContactId", contactID.ToString(CultureInfo.InvariantCulture) } };
            return _communicationManager.Get<Response<ChiefComplaintModel>>(requestXMLValueNvc, apiUrl);
        }

        public Response<ChiefComplaintModel> AddChiefComplaint(ChiefComplaintModel chiefComplaint)
        {
            const string apiUrl = BaseRoute + "AddChiefComplaint";
            return _communicationManager.Post<ChiefComplaintModel, Response<ChiefComplaintModel>>(chiefComplaint, apiUrl);
        }

        public Response<ChiefComplaintModel> UpdateChiefComplaint(ChiefComplaintModel chiefComplaint)
        {
            const string apiUrl = BaseRoute + "UpdateChiefComplaint";
            return _communicationManager.Put<ChiefComplaintModel, Response<ChiefComplaintModel>>(chiefComplaint, apiUrl);
        }

        public Response<ChiefComplaintModel> DeleteChiefComplaint(long chiefComplaintID, DateTime modifiedOn)
        {
            const string apiUrl = BaseRoute + "DeleteChiefComplaint";
            var requestXMLValueNvc = new NameValueCollection { { "chiefComplaintID", chiefComplaintID.ToString(CultureInfo.InvariantCulture) }, { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) } };
            return _communicationManager.Delete<Response<ChiefComplaintModel>>(requestXMLValueNvc, apiUrl);
        }
    }
}
