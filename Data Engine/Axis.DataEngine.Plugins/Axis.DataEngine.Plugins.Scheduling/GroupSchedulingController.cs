using System.Collections.Generic;
using System.Web.Http;
using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Scheduling.GroupScheduling;
using Axis.Model.Common;
using Axis.Model.Scheduling;
using System;

namespace Axis.DataEngine.Plugins.Scheduling
{
    public class GroupSchedulingController : BaseApiController
    {
        #region Class Variables

        private readonly IGroupSchedulingDataProvider _groupSchedulingDataProvider;

        #endregion

        #region Constructors

        public GroupSchedulingController(IGroupSchedulingDataProvider groupSchedulingDataProvider)
        {
            _groupSchedulingDataProvider = groupSchedulingDataProvider;
        }

        #endregion

        #region Public Methods

        [HttpGet]
        public IHttpActionResult GetGroupByID(long groupID)
        {
            var groupResponse = _groupSchedulingDataProvider.GetGroupByID(groupID);
            return new HttpResult<Response<GroupSchedulingModel>>(groupResponse, Request);
        }

        [HttpGet]
        public IHttpActionResult GetGroupSchedulingResource(long groupID)
        {
            var groupResponse = _groupSchedulingDataProvider.GetGroupSchedulingResource(groupID);
            return new HttpResult<Response<GroupSchedulingResourceModel>>(groupResponse, Request);
        }

        [HttpGet]
        public IHttpActionResult GetAppointmentByGroupID(long groupID)
        {
            var groupResponse = _groupSchedulingDataProvider.GetAppointmentByGroupID(groupID);
            return new HttpResult<Response<AppointmentModel>>(groupResponse, Request);
        }

        [HttpGet]
        public IHttpActionResult GetAllContactResourceNamesByAppointment(long appointmentID)
        {
            var groupResponse = _groupSchedulingDataProvider.GetAllContactResourceNamesByAppointment(appointmentID);
            return new HttpResult<Response<AppointmentContactModel>>(groupResponse, Request);
        }

        [HttpPost]
        public IHttpActionResult AddGroupData(GroupSchedulingModel group)
        {
            var groupResponse = _groupSchedulingDataProvider.AddGroupData(group);
            return new HttpResult<Response<GroupSchedulingModel>>(groupResponse, Request);
        }

        [HttpPost]
        public IHttpActionResult AddResources(List<GroupSchedulingResourceModel> resources)
        {
            var groupResponse = _groupSchedulingDataProvider.AddResources(resources);
            return new HttpResult<Response<GroupSchedulingResourceModel>>(groupResponse, Request);
        }

        [HttpPost]
        public IHttpActionResult UpdateGroupData(GroupSchedulingModel group)
        {
            var groupResponse = _groupSchedulingDataProvider.UpdateGroupData(group);
            return new HttpResult<Response<GroupSchedulingModel>>(groupResponse, Request);
        }

        [HttpPost]
        public IHttpActionResult UpdateResources(List<GroupSchedulingResourceModel> resources)
        {
            var groupResponse = _groupSchedulingDataProvider.UpdateResources(resources);
            return new HttpResult<Response<GroupSchedulingResourceModel>>(groupResponse, Request);
        }

        [HttpDelete]
        public IHttpActionResult DeleteGroupSchedulingResource(long id, DateTime modifiedOn)
        {
            return new HttpResult<Response<GroupSchedulingResourceModel>>(_groupSchedulingDataProvider.DeleteGroupSchedulingResource(id, modifiedOn), Request);
        }

        #endregion
    }
}
