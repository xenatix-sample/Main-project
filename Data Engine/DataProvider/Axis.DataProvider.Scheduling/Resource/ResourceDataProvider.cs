using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.DataProvider.Admin;
using Axis.Model.Common;
using Axis.Model.Scheduling;
using System.Collections.Generic;
using System.Data.SqlClient;
using System;


namespace Axis.DataEngine.Plugins.Scheduling.Resource
{
    /// <summary>
    ///
    /// </summary>
    public class ResourceDataProvider : IResourceDataProvider
    {
        #region Class Variables

        /// <summary>
        /// The unit of work
        /// </summary>
        private IUnitOfWork unitOfWork;
        private IAdminDataProvider userManagementDataProvider;
        #endregion Class Variables

        #region Constructors

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ResourceDataProvider(IUnitOfWork unitOfWork, IAdminDataProvider userManagementDataProvider)
        {
            this.unitOfWork = unitOfWork;
            this.userManagementDataProvider = userManagementDataProvider;
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Gets the rooms.
        /// </summary>
        /// <param name="facilityId">The facility identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Response<RoomModel> GetRooms(long? facilityId)
        {
            var roomRepository = unitOfWork.GetRepository<RoomModel>(SchemaName.Scheduling);

            SqlParameter facilityIdParam = new SqlParameter("FacilityID", (object)facilityId ?? DBNull.Value);
            List<SqlParameter> procParams = new List<SqlParameter>() { facilityIdParam };

            var rooms = roomRepository.ExecuteStoredProc("usp_GetRoomDetails", procParams);

            return rooms;
        }

        /// <summary>
        /// Gets the type of the credential by appointment.
        /// </summary>
        /// <param name="appointmentTypeID">The appointment type identifier.</param>
        /// <returns></returns>
        public Response<AppointmentCredentialModel> GetCredentialByAppointmentType(int? appointmentTypeID)
        {
            var appointmentCredentialRepository = unitOfWork.GetRepository<AppointmentCredentialModel>(SchemaName.Scheduling);

            SqlParameter appointmentTypeIdParam = new SqlParameter("AppointmentTypeID", (object)appointmentTypeID ?? DBNull.Value);
            List<SqlParameter> procParams = new List<SqlParameter>() { appointmentTypeIdParam };

            var appointmentCredential = appointmentCredentialRepository.ExecuteStoredProc("usp_GetAppointmentTypeCredential", procParams);

            appointmentCredential.DataItems.ForEach(delegate(AppointmentCredentialModel credential)
            {
                credential.Providers = GetProviderByCredential(credential.CredentialID).DataItems;
            });

            return appointmentCredential;
        }

        /// <summary>
        /// Gets the provider by credential.
        /// </summary>
        /// <param name="credentialId">The credential identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Response<ProviderModel> GetProviderByCredential(long? credentialId)
        {
            SqlParameter credentialIdParam = new SqlParameter("CredentialID", (object)credentialId ?? DBNull.Value);
            List<SqlParameter> procParams = new List<SqlParameter>() { credentialIdParam };

            var usersRepository = unitOfWork.GetRepository<ProviderModel>(SchemaName.Core);
            var providers = usersRepository.ExecuteStoredProc("usp_GetProvidersWithCredential", procParams);

            return providers;
        }

        /// <summary>
        /// Gets the resources.
        /// </summary>
        /// <param name="resourceTypeId">The resource type identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Response<ResourceModel> GetResources(int? resourceTypeId, int? facilityId)
        {
            var resourcesRepository = unitOfWork.GetRepository<ResourceModel>(SchemaName.Scheduling);

            SqlParameter resourceTypeIdParam = new SqlParameter("ResourceTypeID", (object)resourceTypeId ?? DBNull.Value);
            SqlParameter facilityIdParam = new SqlParameter("FacilityID", (object)facilityId ?? DBNull.Value);

            List<SqlParameter> procParams = new List<SqlParameter>() { resourceTypeIdParam, facilityIdParam };

            var resources = resourcesRepository.ExecuteStoredProc("usp_GetResource", procParams);

            return resources;
        }

        /// <summary>
        /// Gets the resource details.
        /// </summary>
        /// <param name="resources">The resource.</param>
        /// <returns></returns>
        public Response<ResourceModel> GetResourceDetails(int? resourceId, short? resourceTypeID)
        {
            var resourceDetails = new Response<ResourceModel>();
            resourceDetails.DataItems = new List<ResourceModel>();

            var resource = new ResourceModel()
            {
                ResourceID = resourceId,
                ResourceTypeID = resourceTypeID
            };

            resource.ResourceAvailabilities = GetResourceAvailability(resource.ResourceID, resource.ResourceTypeID).DataItems;
            resource.ResourceOverrides = GetResourceOverrides(resource.ResourceID, resource.ResourceTypeID).DataItems;

            resourceDetails.DataItems.Add(resource);
            return resourceDetails;
        }

        /// <summary>
        /// Gets the resource availability.
        /// </summary>
        /// <param name="resourceId">The resource identifier.</param>
        /// <param name="resourceTypeID">The resource type identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Response<ResourceAvailabilityModel> GetResourceAvailability(int? resourceId, short? resourceTypeID)
        {
            var resourcesAvailabilityRepository = unitOfWork.GetRepository<ResourceAvailabilityModel>(SchemaName.Scheduling);

            SqlParameter resourceIdParam = new SqlParameter("ResourceID", (object)resourceId ?? DBNull.Value);
            SqlParameter resourceTypeIDParam = new SqlParameter("ResourceTypeID", (object)resourceTypeID ?? DBNull.Value);
            List<SqlParameter> procParams = new List<SqlParameter>() { resourceIdParam, resourceTypeIDParam };

            var resourceAvailabiliity = resourcesAvailabilityRepository.ExecuteStoredProc("usp_GetResourceAvailability", procParams);

            return resourceAvailabiliity;
        }

        /// <summary>
        /// Gets the resource overrides.
        /// </summary>
        /// <param name="resourceId">The resource identifier.</param>
        /// <param name="resourceTypeID">Type of the resource.</param>
        /// <returns></returns>
        public Response<ResourceOverridesModel> GetResourceOverrides(int? resourceId, short? resourceTypeID)
        {
            var resourcesOverridesRepository = unitOfWork.GetRepository<ResourceOverridesModel>(SchemaName.Scheduling);

            SqlParameter resourceIdParam = new SqlParameter("ResourceID", (object)resourceId ?? DBNull.Value);
            SqlParameter resourceTypeParam = new SqlParameter("ResourceTypeID", (object)resourceTypeID ?? DBNull.Value);
            List<SqlParameter> procParams = new List<SqlParameter>() { resourceIdParam, resourceTypeParam };

            var resourceOverrides = resourcesOverridesRepository.ExecuteStoredProc("usp_GetResourceOverrides", procParams);

            return resourceOverrides;
        }

        /// <summary>
        /// Adds a resource availability.
        /// </summary>
        /// <param name="resourceAvailability"></param>
        /// <returns></returns>
        public Response<ResourceAvailabilityModel> AddResourceAvailability(ResourceAvailabilityModel resourceAvailability)
        {
            var resourcesAvailabilityRepository = unitOfWork.GetRepository<ResourceAvailabilityModel>(SchemaName.Scheduling);
            var resourceParameters = BuildResourceAvailabilitySpParams(resourceAvailability);
            return resourcesAvailabilityRepository.ExecuteNQStoredProc("usp_AddResourceAvailability", resourceParameters, idResult: true);
        }       

        /// <summary>
        /// Adds a resource detail.
        /// </summary>
        /// <param name="resourceDetails"></param>
        /// <returns></returns>
        public Response<ResourceModel> AddResourceDetails(ResourceModel resourceDetails)
        {
            var resourcesRepository = unitOfWork.GetRepository<ResourceModel>(SchemaName.Scheduling);

            SqlParameter resourceTypeIdParam = new SqlParameter("ResourceTypeID", (object)resourceDetails.ResourceTypeID ?? DBNull.Value);
            SqlParameter facilityIdParam = new SqlParameter("FacilityID", (object)resourceDetails.FacilityID ?? DBNull.Value);
            // TODO: Add more params here when/if SP is ready

            List<SqlParameter> procParams = new List<SqlParameter>() { resourceTypeIdParam, facilityIdParam };
            return resourcesRepository.ExecuteNQStoredProc("usp_AddResourceDetails", procParams, idResult: true);
        }

        /// <summary>
        /// Adds a resource override.
        /// </summary>
        /// <param name="resourceOverrides"></param>
        /// <returns></returns>
        public Response<ResourceOverridesModel> AddResourceOverrides(ResourceOverridesModel resourceOverrides)
        {
            var resourcesOverridesRepository = unitOfWork.GetRepository<ResourceOverridesModel>(SchemaName.Scheduling);
            var resourceParameters = BuildResourceOverridesSpParams(resourceOverrides);
            return resourcesOverridesRepository.ExecuteNQStoredProc("usp_AddResourceOverrride", resourceParameters, idResult: true);
        }

        /// <summary>
        /// Updates a resource availability.
        /// </summary>
        /// <param name="resourceAvailability"></param>
        /// <returns></returns>
        public Response<ResourceAvailabilityModel> UpdateResourceAvailability(ResourceAvailabilityModel resourceAvailability)
        {
            var resourcesAvailabilityRepository = unitOfWork.GetRepository<ResourceAvailabilityModel>(SchemaName.Scheduling);
            var resourceParameters = BuildResourceAvailabilitySpParams(resourceAvailability, true);
            return resourcesAvailabilityRepository.ExecuteNQStoredProc("usp_UpdateResourceAvailability", resourceParameters);
        }

        /// <summary>
        /// Updates a room.
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public Response<RoomModel> UpdateRoom(RoomModel room)
        {
            var roomRepository = unitOfWork.GetRepository<RoomModel>(SchemaName.Scheduling);
            var roomParams = BuildRoomSpParams(room, true);
            return roomRepository.ExecuteNQStoredProc("usp_UpdateRoom", roomParams);
        }

        /// <summary>
        /// Updates a resource override.
        /// </summary>
        /// <param name="resourceOverrides"></param>
        /// <returns></returns>
        public Response<ResourceOverridesModel> UpdateResourceOverrides(ResourceOverridesModel resourceOverrides)
        {
            var resourcesOverridesRepository = unitOfWork.GetRepository<ResourceOverridesModel>(SchemaName.Scheduling);
            var resourceParameters = BuildResourceOverridesSpParams(resourceOverrides, true);
            return resourcesOverridesRepository.ExecuteNQStoredProc("usp_UpdateResourceOverrride", resourceParameters);
        }

        /// <summary>
        /// Deletes a resource availability.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<ResourceAvailabilityModel> DeleteResourceAvailability(long id, DateTime modifiedOn)
        {
            var repo = unitOfWork.GetRepository<ResourceAvailabilityModel>(SchemaName.Scheduling);
            var param = new SqlParameter("ResourceAvailabilityID", id);
            var modifiedOnParam = new SqlParameter("ModifiedOn", modifiedOn);
            var procParams = new List<SqlParameter> { param, modifiedOnParam };
            var result = repo.ExecuteNQStoredProc("usp_DeleteResourceAvailability", procParams);
            return result;
        }

        /// <summary>
        /// Deletes a resource override.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<ResourceOverridesModel> DeleteResourceOverrides(long id, DateTime modifiedOn)
        {
            var repo = unitOfWork.GetRepository<ResourceOverridesModel>(SchemaName.Scheduling);
            var param = new SqlParameter("ResourceOverrideID", id);
            var modifiedOnParam = new SqlParameter("ModifiedOn", modifiedOn);
            var procParams = new List<SqlParameter> { param, modifiedOnParam };
            var result = repo.ExecuteNQStoredProc("usp_DeleteResourceOverrride", procParams);
            return result;
        }

        #endregion Public Methods

        #region Helpers

        /// <summary>
        /// Builds resource availability sql params.
        /// </summary>
        /// <param name="resourceAvailability"></param>
        /// <param name="isUpdate"></param>
        /// <returns></returns>
        private List<SqlParameter> BuildResourceAvailabilitySpParams(ResourceAvailabilityModel resourceAvailability, bool isUpdate = false)
        {
            var spParameters = new List<SqlParameter>();

            if (isUpdate)
                spParameters.Add(new SqlParameter("ResourceAvailabilityID", resourceAvailability.ResourceAvailabilityID));

            spParameters.Add(new SqlParameter("ResourceID", (object)resourceAvailability.ResourceID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ResourceTypeID", (object)resourceAvailability.ResourceTypeID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("FacilityID", (object)resourceAvailability.FacilityID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("DefaultFacilityID", (object)resourceAvailability.FacilityID ?? DBNull.Value));
            if (isUpdate)
                spParameters.Add(new SqlParameter("ScheduleTypeID", (object)resourceAvailability.ScheduleTypeID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("DayOfWeekID", (object)resourceAvailability.DayOfWeekID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("AvailabilityStartTime", (object)resourceAvailability.AvailabilityStartTime ?? DBNull.Value));
            spParameters.Add(new SqlParameter("AvailabilityEndTime", (object)resourceAvailability.AvailabilityEndTime ?? DBNull.Value));
            if (!isUpdate)
                spParameters.Add(new SqlParameter("ScheduleTypeID", (object)resourceAvailability.ScheduleTypeID ?? DBNull.Value));
                spParameters.Add(new SqlParameter("ModifiedOn", resourceAvailability.ModifiedOn ?? DateTime.Now));

            return spParameters;
        }

        /// <summary>
        /// Builds a room sql params object.
        /// </summary>
        /// <param name="room"></param>
        /// <param name="isUpdate"></param>
        /// <returns></returns>
        private List<SqlParameter> BuildRoomSpParams(RoomModel room, bool isUpdate = false)
        {
            var spParameters = new List<SqlParameter>();

            if (isUpdate)
                spParameters.Add(new SqlParameter("RoomID", room.RoomID));

            spParameters.Add(new SqlParameter("FacilityID", (object)room.FacilityID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("RoomName", (object)room.RoomName ?? DBNull.Value));
            spParameters.Add(new SqlParameter("RoomCapacity", (object)room.RoomCapacity ?? DBNull.Value));
            spParameters.Add(new SqlParameter("IsSchedulable", (object)room.IsSchedulable ?? DBNull.Value));            
            spParameters.Add(new SqlParameter("IsActive", (object)(true)));
            spParameters.Add(new SqlParameter("ModifiedOn", room.ModifiedOn ?? DateTime.Now));

            return spParameters;
        }


        /// <summary>
        /// Builds resource override sql params.
        /// </summary>
        /// <param name="resourceOverrides"></param>
        /// <param name="isUpdate"></param>
        /// <returns></returns>
        private List<SqlParameter> BuildResourceOverridesSpParams(ResourceOverridesModel resourceOverrides, bool isUpdate = false)
        {
            var spParameters = new List<SqlParameter>();

            if (isUpdate)
                spParameters.Add(new SqlParameter("ResourceOverrideID", resourceOverrides.ResourceOverrideID));

            spParameters.Add(new SqlParameter("ResourceID", (object)resourceOverrides.ResourceID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("FacilityID", (object)resourceOverrides.FacilityID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ResourceTypeID", (object)resourceOverrides.ResourceTypeID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("OverrideDate", (object)resourceOverrides.OverrideDate ?? DBNull.Value));
            spParameters.Add(new SqlParameter("Comments", (object)resourceOverrides.Comments ?? DBNull.Value));
            if (!isUpdate)
                spParameters.Add(new SqlParameter("ModifiedOn", resourceOverrides.ModifiedOn ?? DateTime.Now));

            return spParameters;
        }

        #endregion Helpers

    }
}
