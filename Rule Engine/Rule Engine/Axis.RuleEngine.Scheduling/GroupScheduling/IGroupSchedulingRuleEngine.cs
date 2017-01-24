using System.Collections.Generic;
using Axis.Model.Common;
using Axis.Model.Scheduling;
using System;

namespace Axis.RuleEngine.Scheduling.GroupScheduling
{
    public interface IGroupSchedulingRuleEngine
    {
        Response<GroupSchedulingModel> GetGroupByID(long groupID);
        Response<GroupSchedulingResourceModel> GetGroupSchedulingResource(long groupID);
        Response<AppointmentModel> GetAppointmentByGroupID(long groupID);
        Response<AppointmentContactModel> GetAllContactResourceNamesByAppointment(long appointmentID);
        Response<GroupSchedulingModel> AddGroupData(GroupSchedulingModel groupID);
        Response<GroupSchedulingResourceModel> AddResources(List<GroupSchedulingResourceModel> resources);
        Response<GroupSchedulingModel> UpdateGroupData(GroupSchedulingModel groupID);
        Response<GroupSchedulingResourceModel> UpdateResources(List<GroupSchedulingResourceModel> resources);
        Response<GroupSchedulingResourceModel> DeleteGroupSchedulingResource(long id, DateTime modifiedOn);

    }
}
