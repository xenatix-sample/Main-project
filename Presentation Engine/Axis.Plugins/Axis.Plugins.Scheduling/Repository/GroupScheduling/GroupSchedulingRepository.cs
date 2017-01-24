using System.Collections.Generic;
using System.Collections.Specialized;
using Axis.Configuration;
using Axis.Helpers;
using Axis.Model.Common;
using Axis.Model.Scheduling;
using Axis.Plugins.Scheduling.Models;
using Axis.Plugins.Scheduling.Translator;
using Axis.Service;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Axis.Plugins.Scheduling.Repository.GroupScheduling
{
    public class GroupSchedulingRepository : IGroupSchedulingRepository
    {
        #region Class Variables

        private readonly CommunicationManager _communicationManager;
        private const string BaseRoute = "groupscheduling/";

        #endregion Class Variables

        #region Constructors

        public GroupSchedulingRepository()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        public GroupSchedulingRepository(string token)
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        #endregion

        #region Public Methods

        public Response<GroupSchedulingViewModel> GetGroupByID(long groupID)
        {
            var apiUrl = BaseRoute + "GetGroupByID";
            var param = new NameValueCollection { { "groupID", groupID.ToString() } };

            var response = _communicationManager.Get<Response<GroupSchedulingModel>>(param, apiUrl);
            return response.ToModel();
        }
        
        public Response<GroupSchedulingResourceModel> GetGroupSchedulingResource(long groupID)
        {
            var apiUrl = BaseRoute + "GetGroupSchedulingResource";
            var param = new NameValueCollection { { "groupID", groupID.ToString() } };

            var response = _communicationManager.Get<Response<GroupSchedulingResourceModel>>(param, apiUrl);
            return response;//.ToModel();
        }

        public Response<AppointmentModel> GetAppointmentByGroupID(long groupID)
        {
            var apiUrl = BaseRoute + "GetAppointmentByGroupID";
            var param = new NameValueCollection { { "groupID", groupID.ToString() } };

            var response = _communicationManager.Get<Response<AppointmentModel>>(param, apiUrl);
            return response;//.ToModel();
        }

        public async Task<Response<AppointmentModel>> GetAppointmentByGroupIDAsync(long groupID)
        {
            var apiUrl = BaseRoute + "GetAppointmentByGroupID";
            var param = new NameValueCollection { { "groupID", groupID.ToString() } };

            return (await _communicationManager.GetAsync<Response<AppointmentModel>>(param, apiUrl));
        }

        public Response<AppointmentContactModel> GetAllContactResourceNamesByAppointment(long appointmentID)
        {
            var apiUrl = BaseRoute + "GetAllContactResourceNamesByAppointment";
            var param = new NameValueCollection { { "appointmentID", appointmentID.ToString() } };

            var response = _communicationManager.Get<Response<AppointmentContactModel>>(param, apiUrl);
            return response;//.ToModel();
        }

        public Response<GroupSchedulingViewModel> AddGroupData(GroupSchedulingViewModel group)
        {
            var apiUrl = BaseRoute + "AddGroupData";
            var response = _communicationManager.Post<GroupSchedulingModel, Response<GroupSchedulingModel>>(group.ToModel(), apiUrl);

            return response.ToModel();
        }

        public Response<GroupSchedulingResourceModel> AddResources(List<GroupSchedulingResourceModel> resources)
        {
            var apiUrl = BaseRoute + "AddResources";
            var response = _communicationManager.Post<List<GroupSchedulingResourceModel>, Response<GroupSchedulingResourceModel>>(resources, apiUrl);

            return response;
        }

        public Response<GroupSchedulingViewModel> UpdateGroupData(GroupSchedulingViewModel group)
        {
            var apiUrl = BaseRoute + "UpdateGroupData";
            var response = _communicationManager.Post<GroupSchedulingModel, Response<GroupSchedulingModel>>(group.ToModel(), apiUrl);

            return response.ToModel();
        }

        public Response<GroupSchedulingResourceModel> UpdateResources(List<GroupSchedulingResourceModel> resources)
        {
            var apiUrl = BaseRoute + "UpdateResources";
            var response = _communicationManager.Post<List<GroupSchedulingResourceModel>, Response<GroupSchedulingResourceModel>>(resources, apiUrl);

            return response;
        }

        public Response<GroupSchedulingResourceModel> DeleteGroupSchedulingResource(long id, DateTime modifiedOn)
        {
            const string apiUrl = BaseRoute + "DeleteGroupSchedulingResource";
            var requestId = new NameValueCollection { { "id", id.ToString(CultureInfo.InvariantCulture) }, { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) } };
            return _communicationManager.Delete<Response<GroupSchedulingResourceModel>>(requestId, apiUrl);
        }

        #endregion
    }
}
