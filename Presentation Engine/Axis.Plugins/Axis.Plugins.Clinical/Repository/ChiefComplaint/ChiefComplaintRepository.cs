using System;
using Axis.Configuration;
using Axis.Constant;
using Axis.Helpers;
using Axis.Model.Common;
using Axis.Service;
using System.Collections.Specialized;
using System.Globalization;
using System.Threading.Tasks;
using Axis.Model.Clinical;
using Axis.Plugins.Clinical.Models.ChiefComplaint;
using Axis.Plugins.Clinical.Translator;


namespace Axis.Plugins.Clinical.Repository.ChiefComplaint
{
    public class ChiefComplaintRepository : IChiefComplaintRepository
    {
        private readonly CommunicationManager _communicationManager;

        private const string BaseRoute = "ChiefComplaint/";

        public ChiefComplaintRepository()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        public ChiefComplaintRepository(string token)
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        public Response<ChiefComplaintViewModel> GetChiefComplaint(long contactID)
        {
            string apiUrl = BaseRoute + "GetChiefComplaint";
            var parameters = new NameValueCollection { { "contactID", contactID.ToString(CultureInfo.InvariantCulture) } };
            var response = _communicationManager.Get<Response<ChiefComplaintModel>>(parameters, apiUrl);
            return response.ToViewModel();
        }

        public Response<ChiefComplaintViewModel> GetChiefComplaints(long contactID)
        {
            string apiUrl = BaseRoute + "GetChiefComplaints";
            var parameters = new NameValueCollection { { "contactID", contactID.ToString(CultureInfo.InvariantCulture) } };
            var response = _communicationManager.Get<Response<ChiefComplaintModel>>(parameters, apiUrl);
            return response.ToViewModel();
        }

        public Response<ChiefComplaintViewModel> AddChiefComplaint(ChiefComplaintViewModel chiefComplaint)
        {
            string apiUrl = BaseRoute + "AddChiefComplaint";
            var response = _communicationManager.Post<ChiefComplaintModel, Response<ChiefComplaintModel>>(chiefComplaint.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        public Response<ChiefComplaintViewModel> UpdateChiefComplaint(ChiefComplaintViewModel chiefComplaint)
        {
            string apiUrl = BaseRoute + "UpdateChiefComplaint";
            var response = _communicationManager.Put<ChiefComplaintModel, Response<ChiefComplaintModel>>(chiefComplaint.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        public Response<ChiefComplaintViewModel> DeleteChiefComplaint(long chiefComplaintID, DateTime modifiedOn)
        {
            const string apiUrl = BaseRoute + "DeleteChiefComplaint";
            var requestId = new NameValueCollection { { "chiefComplaintID", chiefComplaintID.ToString(CultureInfo.InvariantCulture) }, { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) } };
            return _communicationManager.Delete<Response<ChiefComplaintViewModel>>(requestId, apiUrl);
        }
    }
}
