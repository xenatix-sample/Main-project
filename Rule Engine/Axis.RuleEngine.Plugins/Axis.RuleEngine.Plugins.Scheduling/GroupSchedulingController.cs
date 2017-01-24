using System.Collections.Generic;
using System.Web.Http;
using Axis.Model.Common;
using Axis.Model.Scheduling;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Results;
using Axis.RuleEngine.Scheduling.GroupScheduling;
using System;
using Axis.Constant;
using Axis.RuleEngine.Helpers.Filters;

namespace Axis.RuleEngine.Plugins.Scheduling
{
    public class GroupSchedulingController : BaseApiController
    {
        #region Class Variables

        private readonly IGroupSchedulingRuleEngine _groupSchedulingRuleEngine;

        #endregion

        #region Constructors

        public GroupSchedulingController(IGroupSchedulingRuleEngine groupSchedulingRuleEngine)
        {
            _groupSchedulingRuleEngine = groupSchedulingRuleEngine;
        }

        #endregion

        #region Public Methods
        [Authorization(PermissionKey = SchedulingPermissionKey.Scheduling_Appointment_GroupAppointment, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetGroupByID(long groupID)
        {
            Response<GroupSchedulingModel> responseObject = _groupSchedulingRuleEngine.GetGroupByID(groupID);
            return new HttpResult<Response<GroupSchedulingModel>>(responseObject, Request);
        }

        [Authorization(PermissionKey = SchedulingPermissionKey.Scheduling_Appointment_GroupAppointment, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetGroupSchedulingResource(long groupID)
        {
            Response<GroupSchedulingResourceModel> responseObject = _groupSchedulingRuleEngine.GetGroupSchedulingResource(groupID);
            return new HttpResult<Response<GroupSchedulingResourceModel>>(responseObject, Request);
        }

        [Authorization(PermissionKey = SchedulingPermissionKey.Scheduling_Appointment_GroupAppointment, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetAppointmentByGroupID(long groupID)
        {
            Response<AppointmentModel> responseObject = _groupSchedulingRuleEngine.GetAppointmentByGroupID(groupID);
            return new HttpResult<Response<AppointmentModel>>(responseObject, Request);
        }

        [Authorization(PermissionKey = SchedulingPermissionKey.Scheduling_Appointment_GroupAppointment, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetAllContactResourceNamesByAppointment(long appointmentID)
        {
            Response<AppointmentContactModel> responseObject = _groupSchedulingRuleEngine.GetAllContactResourceNamesByAppointment(appointmentID);
            return new HttpResult<Response<AppointmentContactModel>>(responseObject, Request);
        }

        [Authorization(PermissionKey = SchedulingPermissionKey.Scheduling_Appointment_GroupAppointment, Permission = Permission.Create)]
        [HttpPost]
        public IHttpActionResult AddGroupData(GroupSchedulingModel group)
        {
            Response<GroupSchedulingModel> responseObject = _groupSchedulingRuleEngine.AddGroupData(group);
            return new HttpResult<Response<GroupSchedulingModel>>(responseObject, Request);
        }

        [Authorization(PermissionKey = SchedulingPermissionKey.Scheduling_Appointment_GroupAppointment, Permission = Permission.Create)]
        [HttpPost]
        public IHttpActionResult AddResources(List<GroupSchedulingResourceModel> resources)
        {
            Response<GroupSchedulingResourceModel> responseObject = _groupSchedulingRuleEngine.AddResources(resources);
            return new HttpResult<Response<GroupSchedulingResourceModel>>(responseObject, Request);
        }

        [Authorization(PermissionKey = SchedulingPermissionKey.Scheduling_Appointment_GroupAppointment, Permission = Permission.Update)]
        [HttpPost]
        public IHttpActionResult UpdateGroupData(GroupSchedulingModel group)
        {
            Response<GroupSchedulingModel> responseObject = _groupSchedulingRuleEngine.UpdateGroupData(group);
            return new HttpResult<Response<GroupSchedulingModel>>(responseObject, Request);
        }

        [Authorization(PermissionKey = SchedulingPermissionKey.Scheduling_Appointment_GroupAppointment, Permission = Permission.Update)]
        [HttpPost]
        public IHttpActionResult UpdateResources(List<GroupSchedulingResourceModel> resources)
        {
            Response<GroupSchedulingResourceModel> responseObject = _groupSchedulingRuleEngine.UpdateResources(resources);
            return new HttpResult<Response<GroupSchedulingResourceModel>>(responseObject, Request);
        }

        [Authorization(PermissionKey = SchedulingPermissionKey.Scheduling_Appointment_GroupAppointment, Permission = Permission.Delete)]
        [HttpDelete]
        public IHttpActionResult DeleteGroupSchedulingResource(long id, DateTime modifiedOn)
        {
            return new HttpResult<Response<GroupSchedulingResourceModel>>(_groupSchedulingRuleEngine.DeleteGroupSchedulingResource(id, modifiedOn), Request);

        }

        #endregion
    }
}
