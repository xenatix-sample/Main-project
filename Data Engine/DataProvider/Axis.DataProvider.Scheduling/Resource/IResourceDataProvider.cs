using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Model.Scheduling;
using System;
using System.Collections.Generic;

namespace Axis.DataEngine.Plugins.Scheduling.Resource
{
    /// <summary>
    /// 
    /// </summary>
    public interface IResourceDataProvider
    {
        /// <summary>
        /// Gets the rooms.
        /// </summary>
        /// <param name="facilityId">The facility identifier.</param>
        /// <returns></returns>
        Response<RoomModel> GetRooms(long? facilityId);

        /// <summary>
        /// Gets the type of the credential by appointment.
        /// </summary>
        /// <param name="appointmentTypeID">The appointment type identifier.</param>
        /// <returns></returns>
        Response<AppointmentCredentialModel> GetCredentialByAppointmentType(int? appointmentTypeID);

        /// <summary>
        /// Gets the provider by credential.
        /// </summary>
        /// <param name="credentialId">The credential identifier.</param>
        /// <returns></returns>
        Response<ProviderModel> GetProviderByCredential(long? credentialId);

        /// <summary>
        /// Gets the resources.
        /// </summary>
        /// <param name="resourceTypeId">The resource type identifier.</param>
        /// <param name="facilityId">The facility identifier.</param>
        /// <returns></returns>
        Response<ResourceModel> GetResources(int? resourceTypeId, int? facilityId);

        /// <summary>
        /// Gets the resource details.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <returns></returns>
        Response<ResourceModel> GetResourceDetails(int? resourceId, short? resourceTypeID);

        /// <summary>
        /// Gets the resource availability.
        /// </summary>
        /// <param name="resourceId">The resource identifier.</param>
        /// <param name="resourceTypeID">The resource type identifier.</param>
        /// <returns></returns>
        Response<ResourceAvailabilityModel> GetResourceAvailability(int? resourceId, short? resourceTypeID);

        /// <summary>
        /// Gets the resource overrides.
        /// </summary>
        /// <param name="resourceId">The resource identifier.</param>
        /// <param name="resourceTypeID">Type of the resource.</param>
        /// <returns></returns>
        Response<ResourceOverridesModel> GetResourceOverrides(int? resourceId, short? resourceTypeID);

        /// <summary>
        /// Adds a resource aviailability.
        /// </summary>
        /// <param name="resourceAvailability"></param>
        /// <returns></returns>
        Response<ResourceAvailabilityModel> AddResourceAvailability(ResourceAvailabilityModel resourceAvailability);

        /// <summary>
        /// Adds a resource detail.
        /// </summary>
        /// <param name="resourceDetails"></param>
        /// <returns></returns>
        Response<ResourceModel> AddResourceDetails(ResourceModel resourceDetails);

        /// <summary>
        /// Adds a resource override.
        /// </summary>
        /// <param name="resourceOverrides"></param>
        /// <returns></returns>
        Response<ResourceOverridesModel> AddResourceOverrides(ResourceOverridesModel resourceOverrides);

        /// <summary>
        /// Updates a resource aviailability.
        /// </summary>
        /// <param name="resourceAvailability"></param>
        /// <returns></returns>
        Response<ResourceAvailabilityModel> UpdateResourceAvailability(ResourceAvailabilityModel resourceAvailability);

        /// <summary>
        /// Updates a room.
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        Response<RoomModel> UpdateRoom(RoomModel room);

        /// <summary>
        /// Update a resource override.
        /// </summary>
        /// <param name="resourceOverrides"></param>
        /// <returns></returns>
        Response<ResourceOverridesModel> UpdateResourceOverrides(ResourceOverridesModel resourceOverrides);

        /// <summary>
        /// Deletes a resource availability.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        Response<ResourceAvailabilityModel> DeleteResourceAvailability(long id, DateTime modifiedOn);

        /// <summary>
        /// Deletes a resource override.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        Response<ResourceOverridesModel> DeleteResourceOverrides(long id, DateTime modifiedOn);
    }
}