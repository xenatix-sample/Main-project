using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Model.Scheduling;
using Axis.Service.Scheduling;
using System.Collections.Generic;

namespace Axis.RuleEngine.Scheduling.Resource
{
    /// <summary>
    ///
    /// </summary>
    public class ResourceRuleEngine : IResourceRuleEngine
    {
        /// <summary>
        /// The resource service
        /// </summary>
        private readonly IResourceService resourceService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceRuleEngine"/> class.
        /// </summary>
        /// <param name="resourceService">The resource service.</param>
        public ResourceRuleEngine(IResourceService resourceService)
        {
            this.resourceService = resourceService;
        }

        /// <summary>
        /// Gets the rooms.
        /// </summary>
        /// <param name="facilityId">The facility identifier.</param>
        /// <returns></returns>
        public Response<RoomModel> GetRooms(long? facilityId)
        {
            return resourceService.GetRooms(facilityId);
        }

        /// <summary>
        /// Gets the type of the credential by appointment.
        /// </summary>
        /// <param name="appointmentTypeID">The appointment type identifier.</param>
        /// <returns></returns>
        public Response<AppointmentCredentialModel> GetCredentialByAppointmentType(int? appointmentTypeID)
        {
            return resourceService.GetCredentialByAppointmentType(appointmentTypeID);
        }

        /// <summary>
        /// Gets the provider by credential.
        /// </summary>
        /// <param name="credentialId">The credential identifier.</param>
        /// <returns></returns>
        public Response<ProviderModel> GetProviderByCredential(long? credentialId)
        {
            return resourceService.GetProviderByCredential(credentialId);
        }

        /// <summary>
        /// Gets the resources.
        /// </summary>
        /// <param name="resourceTypeId">The resource type identifier.</param>
        /// <returns></returns>
        public Response<ResourceModel> GetResources(int? resourceTypeId, int? facilityId)
        {
            return resourceService.GetResources(resourceTypeId, facilityId);
        }

        /// <summary>
        /// Gets the resource details.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <returns></returns>
        public Response<ResourceModel> GetResourceDetails(int? resourceId, short? resourceTypeID)
        {
            return resourceService.GetResourceDetails(resourceId, resourceTypeID);
        }

        /// <summary>
        /// Gets the resource availability.
        /// </summary>
        /// <param name="resourceId">The resource identifier.</param>
        /// <param name="resourceTypeID">The resource type identifier.</param>
        /// <returns></returns>
        public Response<ResourceAvailabilityModel> GetResourceAvailability(int? resourceId, short? resourceTypeID)
        {
            return resourceService.GetResourceAvailability(resourceId, resourceTypeID);
        }

        /// <summary>
        /// Gets the resource overrides.
        /// </summary>
        /// <param name="resourceId">The resource identifier.</param>
        /// <param name="resourceTypeID">Type of the resource.</param>
        /// <returns></returns>
        public Response<ResourceOverridesModel> GetResourceOverrides(int? resourceId, short? resourceTypeID)
        {
            return resourceService.GetResourceOverrides(resourceId, resourceTypeID);
        }

        /// <summary>
        /// Adds a resource availability.
        /// </summary>
        /// <param name="resourceAvailability"></param>
        /// <returns></returns>
        public Response<ResourceAvailabilityModel> AddResourceAvailability(ResourceAvailabilityModel resourceAvailability)
        {
            return resourceService.AddResourceAvailability(resourceAvailability);
        }

        /// <summary>
        /// Adds a resource override.
        /// </summary>
        /// <param name="resourceOverride"></param>
        /// <returns></returns>
        public Response<ResourceOverridesModel> AddResourceOverrides(ResourceOverridesModel resourceOverride)
        {
            return resourceService.AddResourceOverrides(resourceOverride);
        }

        /// <summary>
        /// Adds a resource detail.
        /// </summary>
        /// <param name="resourceDetail"></param>
        /// <returns></returns>
        public Response<ResourceModel> AddResourceDetails(ResourceModel resourceDetail)
        {
            return resourceService.AddResourceDetails(resourceDetail);
        }

        /// <summary>
        /// Updates a resource availability.
        /// </summary>
        /// <param name="resourceAvailability"></param>
        /// <returns></returns>
        public Response<ResourceAvailabilityModel> UpdateResourceAvailability(ResourceAvailabilityModel resourceAvailability)
        {
            return resourceService.UpdateResourceAvailability(resourceAvailability);
        }

        /// <summary>
        /// Updates a room.
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public Response<RoomModel> UpdateRoom(RoomModel room)
        {
            return resourceService.UpdateRoom(room);
        }

        /// <summary>
        /// Updates a resource override.
        /// </summary>
        /// <param name="resourceOverride"></param>
        /// <returns></returns>
        public Response<ResourceOverridesModel> UpdateResourceOverrides(ResourceOverridesModel resourceOverride)
        {
            return resourceService.UpdateResourceOverrides(resourceOverride);
        }

        /// <summary>
        /// Deletes a resource availability.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<ResourceAvailabilityModel> DeleteResourceAvailability(long id, System.DateTime modifiedOn)
        {
            return resourceService.DeleteResourceAvailability(id, modifiedOn);
        }

        /// <summary>
        /// Deletes a resource override.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<ResourceOverridesModel> DeleteResourceOverrides(long id, System.DateTime modifiedOn)
        {
            return resourceService.DeleteResourceOverrides(id, modifiedOn);
        }
    }
}