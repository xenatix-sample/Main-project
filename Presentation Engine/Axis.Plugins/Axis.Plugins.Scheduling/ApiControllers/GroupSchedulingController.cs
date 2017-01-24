using System.Collections.Generic;
using System.Web.Http;
using Axis.Model.Common;
using Axis.Model.Scheduling;
using Axis.Plugins.Scheduling.Models;
using Axis.Plugins.Scheduling.Repository.GroupScheduling;
using Axis.PresentationEngine.Helpers.Controllers;
using System;

namespace Axis.Plugins.Scheduling.ApiControllers
{
    public class GroupSchedulingController : BaseApiController
    {
        #region Class Variables

        private readonly IGroupSchedulingRepository _groupSchedulingRepository;

        #endregion

        #region Constructors

        public GroupSchedulingController(IGroupSchedulingRepository groupSchedulingRepository)
        {
            _groupSchedulingRepository = groupSchedulingRepository;
        }

        #endregion

        #region Public Methods

        [HttpGet]
        public Response<GroupSchedulingViewModel> GetGroupByID(long groupID)
        {
            return _groupSchedulingRepository.GetGroupByID(groupID);
        }

        [HttpGet]
        public Response<GroupSchedulingResourceModel> GetGroupSchedulingResource(long groupID)
        {
            return _groupSchedulingRepository.GetGroupSchedulingResource(groupID);
        }

        [HttpGet]
        public Response<AppointmentModel> GetAppointmentByGroupID(long groupID)
        {
            return _groupSchedulingRepository.GetAppointmentByGroupID(groupID);
        }

        [HttpGet]
        public Response<AppointmentContactModel> GetAllContactResourceNamesByAppointment(long appointmentID)
        {
            return _groupSchedulingRepository.GetAllContactResourceNamesByAppointment(appointmentID);
        }

        [HttpPost]
        public Response<GroupSchedulingViewModel> AddGroupData(GroupSchedulingViewModel group)
        {
            return _groupSchedulingRepository.AddGroupData(group);
        }

        [HttpPost]
        public Response<GroupSchedulingResourceModel> AddResources(List<GroupSchedulingResourceModel> resources)
        {
            return _groupSchedulingRepository.AddResources(resources);
        }

        [HttpPost]
        public Response<GroupSchedulingViewModel> UpdateGroupData(GroupSchedulingViewModel group)
        {
            return _groupSchedulingRepository.UpdateGroupData(group);
        }

        [HttpPost]
        public Response<GroupSchedulingResourceModel> UpdateResources(List<GroupSchedulingResourceModel> resources)
        {
            return _groupSchedulingRepository.UpdateResources(resources);
        }

        [HttpDelete]
        public Response<GroupSchedulingResourceModel> DeleteGroupSchedulingResource(long id, DateTime modifiedOn)
        {
            modifiedOn = modifiedOn.ToUniversalTime();
            return _groupSchedulingRepository.DeleteGroupSchedulingResource(id, modifiedOn);
        }

        #endregion
    }
}
