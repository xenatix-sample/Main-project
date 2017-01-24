using System.Collections.Generic;
using Axis.Model.Common;
using Axis.Model.Scheduling;
using Axis.Plugins.Scheduling.Models;
using System;
using System.Threading.Tasks;

namespace Axis.Plugins.Scheduling.Repository.GroupScheduling
{
    public interface IGroupSchedulingRepository
    {
        Response<GroupSchedulingViewModel> GetGroupByID(long groupID);
        Response<GroupSchedulingResourceModel> GetGroupSchedulingResource(long groupID);
        Response<AppointmentModel> GetAppointmentByGroupID(long groupID);
        Task<Response<AppointmentModel>> GetAppointmentByGroupIDAsync(long groupID);
        Response<AppointmentContactModel> GetAllContactResourceNamesByAppointment(long appointmentID);
        Response<GroupSchedulingViewModel> AddGroupData(GroupSchedulingViewModel group);
        Response<GroupSchedulingResourceModel> AddResources(List<GroupSchedulingResourceModel> resources);
        Response<GroupSchedulingViewModel> UpdateGroupData(GroupSchedulingViewModel group);
        Response<GroupSchedulingResourceModel> UpdateResources(List<GroupSchedulingResourceModel> resources);
        Response<GroupSchedulingResourceModel> DeleteGroupSchedulingResource(long id, DateTime modifiedOn);
    }
}
