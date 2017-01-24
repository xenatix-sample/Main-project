using Axis.Model.Common;
using Axis.Plugins.Scheduling.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Plugins.Scheduling.Repository.Resource
{
    /// <summary>
    /// 
    /// </summary>
    public interface IResourceRepository
    {
        /// <summary>
        /// Gets the rooms.
        /// </summary>
        /// <param name="facilityId">The facility identifier.</param>
        /// <returns></returns>
        Response<RoomViewModel> GetRooms(long? facilityId);

        /// <summary>
        /// Gets the type of the credential by appointment.
        /// </summary>
        /// <param name="appointmentTypeID">The appointment type identifier.</param>
        /// <returns></returns>
        Response<AppointmentCredentialViewModel> GetCredentialByAppointmentType(int? appointmentTypeID);

        /// <summary>
        /// Gets the provider by credential.
        /// </summary>
        /// <param name="credentialId">The credential identifier.</param>
        /// <returns></returns>
        Response<ProviderViewModel> GetProviderByCredential(long? credentialId);

        /// <summary>
        /// Gets the resources.
        /// </summary>
        /// <param name="resourceTypeId">The resource type identifier.</param>
        /// <returns></returns>
        Response<ResourceViewModel> GetResources(int? resourceTypeId, int? facilityId);

        /// <summary>
        /// Gets the resource details.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <returns></returns>
        Response<ResourceViewModel> GetResourceDetails(int? resourceId, short? resourceTypeID);

        /// <summary>
        /// Gets the resource availability.
        /// </summary>
        /// <param name="resourceId">The resource identifier.</param>
        /// <param name="resourceTypeID">The resource type identifier.</param>
        /// <returns></returns>
        Response<ResourceAvailabilityViewModel> GetResourceAvailability(int? resourceId, short? resourceTypeID);

        /// <summary>
        /// Gets the resource overrides.
        /// </summary>
        /// <param name="resourceId">The resource identifier.</param>
        /// <param name="resourceTypeID">Type of the resource.</param>
        /// <returns></returns>
        Response<ResourceOverridesViewModel> GetResourceOverrides(int? resourceId, short? resourceTypeID);

        /// <summary>
        /// Adds a resource availability.
        /// </summary>
        /// <param name="resourceAvailability"></param>
        /// <returns></returns>
        Response<ResourceAvailabilityViewModel> AddResourceAvailability(ResourceAvailabilityViewModel resourceAvailability);

        /// <summary>
        /// Adds a resource override.
        /// </summary>
        /// <param name="resourceOverride"></param>
        /// <returns></returns>
        Response<ResourceOverridesViewModel> AddResourceOverrides(ResourceOverridesViewModel resourceOverride);

        /// <summary>
        /// Adds a resource detail.
        /// </summary>
        /// <param name="resourceDetail"></param>
        /// <returns></returns>
        Response<ResourceViewModel> AddResourceDetails(ResourceViewModel resourceDetail);

        /// <summary>
        /// Updates a resource availability.
        /// </summary>
        /// <param name="resourceAvailability"></param>
        /// <returns></returns>
        Response<ResourceAvailabilityViewModel> UpdateResourceAvailability(ResourceAvailabilityViewModel resourceAvailability);

        /// <summary>
        /// Updates a room.
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        Response<RoomViewModel> UpdateRoom(RoomViewModel room);

        /// <summary>
        /// Updates a resource override.
        /// </summary>
        /// <param name="resourceOverride"></param>
        /// <returns></returns>
        Response<ResourceOverridesViewModel> UpdateResourceOverrides(ResourceOverridesViewModel resourceOverride);

        /// <summary>
        /// Deletes a resource availability.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        Response<ResourceAvailabilityViewModel> DeleteResourceAvailability(long id, DateTime modifiedOn);

        /// <summary>
        /// Deletes a resource override.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        Response<ResourceOverridesViewModel> DeleteResourceOverrides(long id, DateTime modifiedOn);
    }
}
