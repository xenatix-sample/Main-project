using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Model.Scheduling;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Filters;
using Axis.RuleEngine.Helpers.Results;
using Axis.RuleEngine.Scheduling.Resource;
using System;
using System.Collections.Generic;
using System.Web.Http;
using Axis.Constant;

namespace Axis.RuleEngine.Plugins.Scheduling
{
    /// <summary>
    ///
    /// </summary>
    public class ResourceController : BaseApiController
    {
        /// <summary>
        /// The resource rule engine
        /// </summary>
        private readonly IResourceRuleEngine resourceRuleEngine;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceController"/> class.
        /// </summary>
        /// <param name="resourceRuleEngine">The resource rule engine.</param>
        public ResourceController(IResourceRuleEngine resourceRuleEngine)
        {
            this.resourceRuleEngine = resourceRuleEngine;
        }

        /// <summary>
        /// Gets the rooms.
        /// </summary>
        /// <param name="facilityId">The facility identifier.</param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public IHttpActionResult GetRooms(long? facilityId)
        {
            return new HttpResult<Response<RoomModel>>(resourceRuleEngine.GetRooms(facilityId), Request);
        }

        /// <summary>
        /// Gets the appointment credential.
        /// </summary>
        /// <param name="appointmentTypeID">The appointment type identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetCredentialByAppointmentType(int? appointmentTypeID)
        {
            return new HttpResult<Response<AppointmentCredentialModel>>(resourceRuleEngine.GetCredentialByAppointmentType(appointmentTypeID), Request);
        }

        /// <summary>
        /// Gets the provider by credential.
        /// </summary>
        /// <param name="credentialId">The credential identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetProviderByCredential(long? credentialId)
        {
            return new HttpResult<Response<ProviderModel>>(resourceRuleEngine.GetProviderByCredential(credentialId), Request);
        }

        /// <summary>
        /// Gets the resources.
        /// </summary>
        /// <param name="resourceTypeId">The resource type identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetResources(int? resourceTypeId, int? facilityId)
        {
            return new HttpResult<Response<ResourceModel>>(resourceRuleEngine.GetResources(resourceTypeId, facilityId), Request);
        }

        /// <summary>
        /// Gets the resource details.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetResourceDetails(int? resourceId, short? resourceTypeID)
        {
            return new HttpResult<Response<ResourceModel>>(resourceRuleEngine.GetResourceDetails(resourceId, resourceTypeID), Request);
        }

        /// <summary>
        /// Gets the resource availability.
        /// </summary>
        /// <param name="resourceId">The resource identifier.</param>
        /// <param name="resourceTypeID">The resource type identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetResourceAvailability(int? resourceId, short? resourceTypeID)
        {
            return new HttpResult<Response<ResourceAvailabilityModel>>(resourceRuleEngine.GetResourceAvailability(resourceId, resourceTypeID), Request);
        }

        /// <summary>
        /// Gets the resource overrides.
        /// </summary>
        /// <param name="resourceId">The resource identifier.</param>
        /// <param name="resourceTypeID">The resource type identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetResourceOverrides(int? resourceId, short? resourceTypeID)
        {
            return new HttpResult<Response<ResourceOverridesModel>>(resourceRuleEngine.GetResourceOverrides(resourceId, resourceTypeID), Request);
        }

        /// <summary>
        /// Adds a resource availability.
        /// </summary>
        /// <param name="resourceAvailability"></param>
        [Authorization(PermissionKey = SchedulingPermissionKey.Scheduling_Appointment_SingleAppointment, Permission = Permission.Create)]
        [HttpPost]
        public IHttpActionResult AddResourceAvailability(ResourceAvailabilityModel resourceAvailability)
        {
            return new HttpResult<Response<ResourceAvailabilityModel>>(resourceRuleEngine.AddResourceAvailability(resourceAvailability), Request);
        }

        /// <summary>
        /// Adds a resource override.
        /// </summary>
        /// <param name="resourceOverride"></param>
        [Authorization(PermissionKey = SchedulingPermissionKey.Scheduling_Appointment_SingleAppointment, Permission = Permission.Create)]
        [HttpPost]
        public IHttpActionResult AddResourceOverrides(ResourceOverridesModel resourceOverride)
        {
            return new HttpResult<Response<ResourceOverridesModel>>(resourceRuleEngine.AddResourceOverrides(resourceOverride), Request);
        }

        /// <summary>
        /// Adds a resource detail.
        /// </summary>
        /// <param name="resourceDetail"></param>
        [Authorization(PermissionKey = SchedulingPermissionKey.Scheduling_Appointment_SingleAppointment, Permission = Permission.Create)]
        [HttpPost]
        public IHttpActionResult AddResourceDetails(ResourceModel resourceDetail)
        {
            return new HttpResult<Response<ResourceModel>>(resourceRuleEngine.AddResourceDetails(resourceDetail), Request);
        }

        /// <summary>
        /// Updates a resource availability.
        /// </summary>
        /// <param name="resourceAvailability"></param>
        [Authorization(PermissionKey = SchedulingPermissionKey.Scheduling_Appointment_SingleAppointment, Permission = Permission.Update)]
        [HttpPut]
        public IHttpActionResult UpdateResourceAvailability(ResourceAvailabilityModel resourceAvailability)
        {
            return new HttpResult<Response<ResourceAvailabilityModel>>(resourceRuleEngine.UpdateResourceAvailability(resourceAvailability), Request);
        }

        /// <summary>
        /// Updates a room.
        /// </summary>
        /// <param name="room"></param>
        [Authorization(PermissionKey = SchedulingPermissionKey.Scheduling_Appointment_SingleAppointment, Permission = Permission.Update)]
        [HttpPut]
        public IHttpActionResult UpdateRoom(RoomModel room)
        {
            return new HttpResult<Response<RoomModel>>(resourceRuleEngine.UpdateRoom(room), Request);
        }

        /// <summary>
        /// Updates a resource override.
        /// </summary>
        /// <param name="resourceOverride"></param>
        [Authorization(PermissionKey = SchedulingPermissionKey.Scheduling_Appointment_SingleAppointment, Permission = Permission.Update)]
        [HttpPut]
        public IHttpActionResult UpdateResourceOverrides(ResourceOverridesModel resourceOverride)
        {
            return new HttpResult<Response<ResourceOverridesModel>>(resourceRuleEngine.UpdateResourceOverrides(resourceOverride), Request);

        }

        /// <summary>
        /// Deletes a resource availability.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        [Authorization(PermissionKey = SchedulingPermissionKey.Scheduling_Appointment_SingleAppointment, Permission = Permission.Delete)]
        [HttpDelete]
        public IHttpActionResult DeleteResourceAvailability(long id, DateTime modifiedOn)
        {
            return new HttpResult<Response<ResourceAvailabilityModel>>(resourceRuleEngine.DeleteResourceAvailability(id, modifiedOn), Request);

        }

        /// <summary>
        /// Deletes a resource override.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        [Authorization(PermissionKey = SchedulingPermissionKey.Scheduling_Appointment_SingleAppointment, Permission = Permission.Delete)]
        [HttpDelete]
        public IHttpActionResult DeleteResourceOverrides(long id, DateTime modifiedOn)
        {
            return new HttpResult<Response<ResourceOverridesModel>>(resourceRuleEngine.DeleteResourceOverrides(id, modifiedOn), Request);
        }
    }
}