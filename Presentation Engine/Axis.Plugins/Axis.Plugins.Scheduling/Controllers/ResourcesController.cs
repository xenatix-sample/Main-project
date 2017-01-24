using Axis.Plugins.Scheduling.Repository.Resource;
using Axis.Model.Common;
using Axis.Plugins.Scheduling.Models;
using Axis.PresentationEngine.Helpers.Controllers;
using System.Web.Mvc;
using System;

namespace Axis.Plugins.Scheduling.Controllers
{
    /// <summary>
    ///
    /// </summary>
    public class ResourcesController : BaseApiController
    {
        /// <summary>
        /// The resource repository
        /// </summary>
        private readonly IResourceRepository resourceRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceController"/> class.
        /// </summary>
        /// <param name="resourceRepository">The resource repository.</param>
        public ResourcesController(IResourceRepository resourceRepository)
        {
            this.resourceRepository = resourceRepository;
        }

        /// <summary>
        /// Gets the resources.
        /// </summary>
        /// <param name="resourceTypeId">The resource type identifier.</param>
        /// <returns></returns>
        public Response<ResourceViewModel> Get(int resourceTypeId, int facilityId)
        {
            return resourceRepository.GetResources(resourceTypeId, facilityId);
        }

        /// <summary>
        /// Gets the rooms.
        /// </summary>
        /// <param name="facilityId">The facility identifier.</param>
        /// <returns></returns>
        public Response<RoomViewModel> GetRooms(long? facilityId)
        {
            return resourceRepository.GetRooms(facilityId);
        }

        /// <summary>
        /// Gets the type of the credential by appointment.
        /// </summary>
        /// <param name="appointmentTypeID">The appointment type identifier.</param>
        /// <returns></returns>
        public Response<AppointmentCredentialViewModel> GetCredentialByAppointmentType(int? appointmentTypeID)
        {
            return resourceRepository.GetCredentialByAppointmentType(appointmentTypeID);
        }

        public Response<ProviderViewModel> GetProviderByCredential(long? credentialId)
        {
            return resourceRepository.GetProviderByCredential(credentialId);
        }

        /// <summary>
        /// Gets the resources.
        /// </summary>
        /// <param name="resourceTypeId">The resource type identifier.</param>
        /// <returns></returns>
        public Response<ResourceViewModel> GetResources(int? resourceTypeId, int? facilityId=null)
        {
            return resourceRepository.GetResources(resourceTypeId, facilityId);
        }

        /// <summary>
        /// Gets the resource details.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <returns></returns>
        public Response<ResourceViewModel> GetResourceDetails(int? resourceId, short? resourceTypeID)
        {
            return resourceRepository.GetResourceDetails(resourceId, resourceTypeID);
        }

        /// <summary>
        /// Gets the resource availability.
        /// </summary>
        /// <param name="resourceId">The resource identifier.</param>
        /// <param name="resourceTypeID">The resource type identifier.</param>
        /// <returns></returns>
        public Response<ResourceAvailabilityViewModel> GetResourceAvailability(int? resourceId, short? resourceTypeID)
        {
            return resourceRepository.GetResourceAvailability(resourceId, resourceTypeID);
        }

        /// <summary>
        /// Gets the resource overrides.
        /// </summary>
        /// <param name="resourceId">The resource identifier.</param>
        /// <param name="resourceTypeID">Type of the resource.</param>
        /// <returns></returns>
        public Response<ResourceOverridesViewModel> GetResourceOverrides(int? resourceId, short? resourceTypeID)
        {
            return resourceRepository.GetResourceOverrides(resourceId, resourceTypeID);
        }

        /// <summary>
        /// Adds a resource availability.
        /// </summary>
        /// <param name="resourceAvailability"></param>
        [HttpPost]
        public Response<ResourceAvailabilityViewModel> AddResourceAvailability(ResourceAvailabilityViewModel resourceAvailability)
        {
            return resourceRepository.AddResourceAvailability(resourceAvailability);
        }

        /// <summary>
        /// Adds a resource override.
        /// </summary>
        /// <param name="resourceOverride"></param>
        [HttpPost]
        public Response<ResourceOverridesViewModel> AddResourceOverrides(ResourceOverridesViewModel resourceOverride)
        {
            return resourceRepository.AddResourceOverrides(resourceOverride);
        }

        /// <summary>
        /// Adds a resource detail.
        /// </summary>
        /// <param name="resourceDetail"></param>
        [HttpPost]
        public Response<ResourceViewModel> AddResourceDetails(ResourceViewModel resourceDetail)
        {
            return resourceRepository.AddResourceDetails(resourceDetail);
        }

        /// <summary>
        /// Updates a resource availability.
        /// </summary>
        /// <param name="resourceAvailability"></param>
        [HttpPut]
        public Response<ResourceAvailabilityViewModel> UpdateResourceAvailability(ResourceAvailabilityViewModel resourceAvailability)
        {
            var response = resourceRepository.UpdateResourceAvailability(resourceAvailability);
            return response;
        }

        /// <summary>
        /// Updates a room.
        /// </summary>
        /// <param name="room"></param>
        [HttpPut]
        public Response<RoomViewModel> UpdateRoom(RoomViewModel room)
        {
            var response = resourceRepository.UpdateRoom(room);
            return response;
        }

        /// <summary>
        /// Updates a resource override.
        /// </summary>
        /// <param name="resourceOverride"></param>
        [HttpPut]
        public Response<ResourceOverridesViewModel> UpdateResourceOverrides(ResourceOverridesViewModel resourceOverride)
        {
            var response = resourceRepository.UpdateResourceOverrides(resourceOverride);
            return response;
        }

        /// <summary>
        /// Deletes a resource availability.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        [HttpDelete]
        public Response<ResourceAvailabilityViewModel> DeleteResourceAvailability(long id, DateTime modifiedOn)
        {
            modifiedOn = modifiedOn.ToUniversalTime();
            return resourceRepository.DeleteResourceAvailability(id, modifiedOn);
        }

        /// <summary>
        /// Deletes a resource override.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        [HttpDelete]
        public Response<ResourceOverridesViewModel> DeleteResourceOverrides(long id, DateTime modifiedOn)
        {
            modifiedOn = modifiedOn.ToUniversalTime();
            return resourceRepository.DeleteResourceOverrides(id, modifiedOn);
        }
    }
}