using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataEngine.Plugins.Scheduling.Resource;
using Axis.Model.Common;
using Axis.Model.Scheduling;
using System.Collections.Generic;
using System.Web.Http;
using Axis.Model.Account;
using System;

namespace Axis.DataEngine.Plugins.Scheduling
{
    /// <summary>
    ///
    /// </summary>
    public class ResourceController : BaseApiController
    {
        /// <summary>
        /// The resource data provider
        /// </summary>
        private readonly IResourceDataProvider resourceDataProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceController"/> class.
        /// </summary>
        /// <param name="resourceDataProvider">The resource data provider.</param>
        public ResourceController(IResourceDataProvider resourceDataProvider)
        {
            this.resourceDataProvider = resourceDataProvider;
        }

        /// <summary>
        /// Gets the scheduling occurrence.
        /// </summary>
        /// <param name="facilityId">The facility identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetRooms(long? facilityId)
        {
            return new HttpResult<Response<RoomModel>>(resourceDataProvider.GetRooms(facilityId), Request);
        }

        /// <summary>
        /// Gets the type of the credential by appointment.
        /// </summary>
        /// <param name="appointmentTypeID">The appointment type identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetCredentialByAppointmentType(int? appointmentTypeID)
        {
            return new HttpResult<Response<AppointmentCredentialModel>>(resourceDataProvider.GetCredentialByAppointmentType(appointmentTypeID), Request);
        }

        /// <summary>
        /// Gets the provider by credential.
        /// </summary>
        /// <param name="credentialId">The credential identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetProviderByCredential(long? credentialId)
        {
            return new HttpResult<Response<ProviderModel>>(resourceDataProvider.GetProviderByCredential(credentialId), Request);
        }

        /// <summary>
        /// Gets the resources.
        /// </summary>
        /// <param name="resourceTypeId">The appointment type identifier.</param>
        /// <param name="facilityId">The facility identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetResources(int? resourceTypeId, int? facilityId)
        {
            return new HttpResult<Response<ResourceModel>>(resourceDataProvider.GetResources(resourceTypeId, facilityId), Request);
        }

        [HttpGet]
        public IHttpActionResult GetResourceDetails(int? resourceId, short? resourceTypeID)
        {
            return new HttpResult<Response<ResourceModel>>(resourceDataProvider.GetResourceDetails(resourceId, resourceTypeID), Request);
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
            return new HttpResult<Response<ResourceAvailabilityModel>>(resourceDataProvider.GetResourceAvailability(resourceId, resourceTypeID), Request);
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
            return new HttpResult<Response<ResourceOverridesModel>>(resourceDataProvider.GetResourceOverrides(resourceId, resourceTypeID), Request);
        }

        /// <summary>
        ///  Adds a resource availability.
        /// </summary>
        /// <param name="resourceAvailability"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddResourceAvailability(ResourceAvailabilityModel resourceAvailability)
        {
            return new HttpResult<Response<ResourceAvailabilityModel>>(resourceDataProvider.AddResourceAvailability(resourceAvailability), Request);
        }

        /// <summary>
        /// Adds a resource override.
        /// </summary>
        /// <param name="resourceOverrides"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddResourceOverrides(ResourceOverridesModel resourceOverrides)
        {
            return new HttpResult<Response<ResourceOverridesModel>>(resourceDataProvider.AddResourceOverrides(resourceOverrides), Request);
        }

        /// <summary>
        /// Adds a resource detail.
        /// </summary>
        /// <param name="resourceDetails"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddResourceDetails(ResourceModel resourceDetails)
        {
            return new HttpResult<Response<ResourceModel>>(resourceDataProvider.AddResourceDetails(resourceDetails), Request);
        }

        /// <summary>
        /// Updates a resource availability.
        /// </summary>
        /// <param name="resourceAvailability"></param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateResourceAvailability(ResourceAvailabilityModel resourceAvailability)
        {
            return new HttpResult<Response<ResourceAvailabilityModel>>(resourceDataProvider.UpdateResourceAvailability(resourceAvailability), Request);
        }

        /// <summary>
        /// UPdate a room.
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateRoom(RoomModel room)
        {
            return new HttpResult<Response<RoomModel>>(resourceDataProvider.UpdateRoom(room), Request);
        }

        /// <summary>
        /// Updates a resource override.
        /// </summary>
        /// <param name="resourceOverrides"></param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateResourceOverrides(ResourceOverridesModel resourceOverrides)
        {
            return new HttpResult<Response<ResourceOverridesModel>>(resourceDataProvider.UpdateResourceOverrides(resourceOverrides), Request);
        }

        /// <summary>
        /// Deletes a resource availability.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeleteResourceAvailability(long id, DateTime modifiedOn)
        {
            return new HttpResult<Response<ResourceAvailabilityModel>>(resourceDataProvider.DeleteResourceAvailability(id, modifiedOn), Request);
        }

        /// <summary>
        /// Deletes a resource override.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeleteResourceOverrides(long id, DateTime modifiedOn)
        {
            return new HttpResult<Response<ResourceOverridesModel>>(resourceDataProvider.DeleteResourceOverrides(id, modifiedOn), Request);
        }
    }
}