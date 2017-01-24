using System.Collections.Generic;
using Axis.Model.Common;
using Axis.Model.Scheduling;
using System;

namespace Axis.Service.Scheduling.GroupScheduling
{
    public interface IGroupSchedulingService
    {
        Response<GroupSchedulingModel> GetGroupByID(long groupID);
        Response<GroupSchedulingResourceModel> GetGroupSchedulingResource(long groupID);
        Response<AppointmentModel> GetAppointmentByGroupID(long groupID);
        Response<AppointmentContactModel> GetAllContactResourceNamesByAppointment(long appointmentID);
        Response<GroupSchedulingModel> AddGroupData(GroupSchedulingModel group);
        Response<GroupSchedulingResourceModel> AddResources(List<GroupSchedulingResourceModel> resources);
        Response<GroupSchedulingModel> UpdateGroupData(GroupSchedulingModel group);
        Response<GroupSchedulingResourceModel> UpdateResources(List<GroupSchedulingResourceModel> resources);
        Response<GroupSchedulingResourceModel> DeleteGroupSchedulingResource(long id, DateTime modifiedOn);

    }
}
