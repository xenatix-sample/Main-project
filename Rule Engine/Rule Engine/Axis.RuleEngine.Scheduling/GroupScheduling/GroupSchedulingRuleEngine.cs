using System.Collections.Generic;
using Axis.Model.Common;
using Axis.Model.Scheduling;
using Axis.Service.Scheduling.GroupScheduling;

namespace Axis.RuleEngine.Scheduling.GroupScheduling
{
    public class GroupSchedulingRuleEngine : IGroupSchedulingRuleEngine
    {
        #region Class Variables

        private readonly IGroupSchedulingService _groupSchedulingService;

        #endregion

        #region Constructrs

        public GroupSchedulingRuleEngine(IGroupSchedulingService groupSchedulingService)
        {
            _groupSchedulingService = groupSchedulingService;
        }

        #endregion

        #region Public Methods

        public Response<GroupSchedulingModel> GetGroupByID(long groupID)
        {
            return _groupSchedulingService.GetGroupByID(groupID);
        }

        public Response<GroupSchedulingResourceModel> GetGroupSchedulingResource(long groupID)
        {
            return _groupSchedulingService.GetGroupSchedulingResource(groupID);
        }

        public Response<AppointmentModel> GetAppointmentByGroupID(long groupID)
        {
            return _groupSchedulingService.GetAppointmentByGroupID(groupID);
        }

        public Response<AppointmentContactModel> GetAllContactResourceNamesByAppointment(long appointmentID)
        {
            return _groupSchedulingService.GetAllContactResourceNamesByAppointment(appointmentID);
        }

        public Response<GroupSchedulingModel> AddGroupData(GroupSchedulingModel groupID)
        {
            return _groupSchedulingService.AddGroupData(groupID);
        }

        public Response<GroupSchedulingResourceModel> AddResources(List<GroupSchedulingResourceModel> resources)
        {
            return _groupSchedulingService.AddResources(resources);
        }

        public Response<GroupSchedulingModel> UpdateGroupData(GroupSchedulingModel groupID)
        {
            return _groupSchedulingService.UpdateGroupData(groupID);
        }

        public Response<GroupSchedulingResourceModel> UpdateResources(List<GroupSchedulingResourceModel> resources)
        {
            return _groupSchedulingService.UpdateResources(resources);
        }      

        public Response<GroupSchedulingResourceModel> DeleteGroupSchedulingResource(long id, System.DateTime modifiedOn)
        {
            return _groupSchedulingService.DeleteGroupSchedulingResource(id, modifiedOn);
        }  
        #endregion

    }
}
