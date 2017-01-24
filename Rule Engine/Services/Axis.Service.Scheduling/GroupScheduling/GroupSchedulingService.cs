using System.Collections.Generic;
using System.Collections.Specialized;
using Axis.Configuration;
using Axis.Model.Common;
using Axis.Model.Scheduling;
using Axis.Security;
using System.Globalization;

namespace Axis.Service.Scheduling.GroupScheduling
{
    public class GroupSchedulingService : IGroupSchedulingService
    {
        #region Class Variables

        private readonly CommunicationManager _communicationManager;
        private const string BaseRoute = "groupscheduling/";

        #endregion

        #region Constructors

        public GroupSchedulingService()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        #endregion

        #region Public Methods

        public Response<GroupSchedulingModel> GetGroupByID(long groupID)
        {
            string apiUrl = BaseRoute + "GetGroupByID";
            var param = new NameValueCollection { { "groupID", groupID.ToString() } };

            return _communicationManager.Get<Response<GroupSchedulingModel>>(param, apiUrl);
        }

        public Response<GroupSchedulingResourceModel> GetGroupSchedulingResource(long groupID)
        {
            string apiUrl = BaseRoute + "GetGroupSchedulingResource";
            var param = new NameValueCollection { { "groupID", groupID.ToString() } };

            return _communicationManager.Get<Response<GroupSchedulingResourceModel>>(param, apiUrl);
        }

        public Response<AppointmentModel> GetAppointmentByGroupID(long groupID)
        {
            string apiUrl = BaseRoute + "GetAppointmentByGroupID";
            var param = new NameValueCollection { { "groupID", groupID.ToString() } };

            return _communicationManager.Get<Response<AppointmentModel>>(param, apiUrl);
        }

        public Response<AppointmentContactModel> GetAllContactResourceNamesByAppointment(long appointmentID)
        {
            string apiUrl = BaseRoute + "GetAllContactResourceNamesByAppointment";
            var param = new NameValueCollection { { "appointmentID", appointmentID.ToString() } };

            return _communicationManager.Get<Response<AppointmentContactModel>>(param, apiUrl);
        }

        public Response<GroupSchedulingModel> AddGroupData(GroupSchedulingModel group)
        {
            string apiUrl = BaseRoute + "AddGroupData";
            return _communicationManager.Post<GroupSchedulingModel, Response<GroupSchedulingModel>>(group, apiUrl);
        }

        public Response<GroupSchedulingResourceModel> AddResources(List<GroupSchedulingResourceModel> resources)
        {
            string apiUrl = BaseRoute + "AddResources";
            return _communicationManager.Post<List<GroupSchedulingResourceModel>, Response<GroupSchedulingResourceModel>>(resources, apiUrl);
        }

        public Response<GroupSchedulingModel> UpdateGroupData(GroupSchedulingModel group)
        {
            string apiUrl = BaseRoute + "UpdateGroupData";
            return _communicationManager.Post<GroupSchedulingModel, Response<GroupSchedulingModel>>(group, apiUrl);
        }

        public Response<GroupSchedulingResourceModel> UpdateResources(List<GroupSchedulingResourceModel> resources)
        {
            string apiUrl = BaseRoute + "UpdateResources";
            return _communicationManager.Post<List<GroupSchedulingResourceModel>, Response<GroupSchedulingResourceModel>>(resources, apiUrl);
        }

        public Response<GroupSchedulingResourceModel> DeleteGroupSchedulingResource(long id, System.DateTime modifiedOn)
        {
            const string apiUrl = BaseRoute + "DeleteGroupSchedulingResource";
            var requestXMLValueNvc = new NameValueCollection
            {
                {"id", id.ToString(CultureInfo.InvariantCulture) },
                {"modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture)}
            };
            return _communicationManager.Delete<Response<GroupSchedulingResourceModel>>(requestXMLValueNvc, apiUrl);
        }

        #endregion
    }
}
