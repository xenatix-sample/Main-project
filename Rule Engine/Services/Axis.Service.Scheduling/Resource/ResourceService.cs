using Axis.Configuration;
using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Model.Scheduling;
using Axis.Security;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.Service.Scheduling
{
    /// <summary>
    ///
    /// </summary>
    public class ResourceService : IResourceService
    {
        /// <summary>
        /// The _communication manager
        /// </summary>
        private readonly CommunicationManager _communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string BaseRoute = "resource/";

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceService"/> class.
        /// </summary>
        public ResourceService()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        /// <summary>
        /// Gets the rooms.
        /// </summary>
        /// <param name="facilityId">The facility identifier.</param>
        /// <returns></returns>
        public Response<RoomModel> GetRooms(long? facilityId)
        {
            const string apiUrl = BaseRoute + "GetRooms";
            var requestXMLValueNvc = new NameValueCollection { { "facilityId", facilityId.ToString() } };
            return _communicationManager.Get<Response<RoomModel>>(requestXMLValueNvc, apiUrl);
        }

        /// <summary>
        /// Gets the type of the credential by appointment.
        /// </summary>
        /// <param name="appointmentTypeID">The appointment type identifier.</param>
        /// <returns></returns>
        public Response<AppointmentCredentialModel> GetCredentialByAppointmentType(int? appointmentTypeID)
        {
            const string apiUrl = BaseRoute + "GetCredentialByAppointmentType";
            var requestXMLValueNvc = new NameValueCollection { { "appointmentTypeID", appointmentTypeID.ToString() } };
            return _communicationManager.Get<Response<AppointmentCredentialModel>>(requestXMLValueNvc, apiUrl);
        }

        /// <summary>
        /// Gets the provider by credential.
        /// </summary>
        /// <param name="credentialId">The credential identifier.</param>
        /// <returns></returns>
        public Response<ProviderModel> GetProviderByCredential(long? credentialId)
        {
            const string apiUrl = BaseRoute + "GetProviderByCredential";
            var requestXMLValueNvc = new NameValueCollection { { "credentialId", credentialId.ToString() } };
            return _communicationManager.Get<Response<ProviderModel>>(requestXMLValueNvc, apiUrl);
        }

        /// <summary>
        /// Gets the resources.
        /// </summary>
        /// <param name="resourceTypeId">The resource type identifier.</param>
        /// <returns></returns>
        public Response<ResourceModel> GetResources(int? resourceTypeId, int? facilityId)
        {
            const string apiUrl = BaseRoute + "GetResources";
            var requestXMLValueNvc = new NameValueCollection { 
                { "resourceTypeId", resourceTypeId.ToString() }, 
                { "facilityId", facilityId.ToString() } 
            };

            return _communicationManager.Get<Response<ResourceModel>>(requestXMLValueNvc, apiUrl);
        }

        /// <summary>
        /// Gets the resource details.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <returns></returns>
        public Response<ResourceModel> GetResourceDetails(int? resourceId, short? resourceTypeID)
        {
            const string apiUrl = BaseRoute + "GetResourceDetails";

            var requestXMLValueNvc = new NameValueCollection();
            requestXMLValueNvc.Add("resourceId", resourceId.ToString());
            requestXMLValueNvc.Add("resourceTypeID", resourceTypeID.ToString());

            return _communicationManager.Get<Response<ResourceModel>>(requestXMLValueNvc, apiUrl);
        }

        /// <summary>
        /// Gets the resource availability.
        /// </summary>
        /// <param name="resourceId">The resource identifier.</param>
        /// <param name="resourceTypeID">The resource type identifier.</param>
        /// <returns></returns>
        public Response<ResourceAvailabilityModel> GetResourceAvailability(int? resourceId, short? resourceTypeID)
        {
            const string apiUrl = BaseRoute + "GetResourceAvailability";
            var requestXMLValueNvc = new NameValueCollection();
            requestXMLValueNvc.Add("resourceId", resourceId.ToString());
            requestXMLValueNvc.Add("resourceTypeID", resourceTypeID.ToString());
            return _communicationManager.Get<Response<ResourceAvailabilityModel>>(requestXMLValueNvc, apiUrl);
        }

        /// <summary>
        /// Gets the resource overrides.
        /// </summary>
        /// <param name="resourceId">The resource identifier.</param>
        /// <param name="resourceTypeID">Type of the resource.</param>
        /// <returns></returns>
        public Response<ResourceOverridesModel> GetResourceOverrides(int? resourceId, short? resourceTypeID)
        {
            const string apiUrl = BaseRoute + "GetResourceOverrides";
            var requestXMLValueNvc = new NameValueCollection();
            requestXMLValueNvc.Add("resourceId", resourceId.ToString());
            requestXMLValueNvc.Add("resourceTypeID", resourceTypeID.ToString());
            return _communicationManager.Get<Response<ResourceOverridesModel>>(requestXMLValueNvc, apiUrl);
        }


        /// <summary>
        /// Adds a resource availability.
        /// </summary>
        /// <param name="resourceAvailability"></param>
        /// <returns></returns>
        public Response<ResourceAvailabilityModel> AddResourceAvailability(ResourceAvailabilityModel resourceAvailability)
        {
            const string apiUrl = BaseRoute + "AddResourceAvailability";
            return _communicationManager.Post<ResourceAvailabilityModel, Response<ResourceAvailabilityModel>>(resourceAvailability, apiUrl);
        }

        /// <summary>
        /// Adds a resource override.
        /// </summary>
        /// <param name="resourceOverride"></param>
        /// <returns></returns>
        public Response<ResourceOverridesModel> AddResourceOverrides(ResourceOverridesModel resourceOverride)
        {
            const string apiUrl = BaseRoute + "AddResourceOverrides";
            return _communicationManager.Post<ResourceOverridesModel, Response<ResourceOverridesModel>>(resourceOverride, apiUrl);
        }

        /// <summary>
        /// Adds a resource detail.
        /// </summary>
        /// <param name="resourceDetail"></param>
        /// <returns></returns>
        public Response<ResourceModel> AddResourceDetails(ResourceModel resourceDetail)
        {
            const string apiUrl = BaseRoute + "AddResourceDetails";
            return _communicationManager.Post<ResourceModel, Response<ResourceModel>>(resourceDetail, apiUrl);
        }

        /// <summary>
        /// Updates a resource availability.
        /// </summary>
        /// <param name="resourceAvailability"></param>
        /// <returns></returns>
        public Response<ResourceAvailabilityModel> UpdateResourceAvailability(ResourceAvailabilityModel resourceAvailability)
        {
            const string apiUrl = BaseRoute + "UpdateResourceAvailability";
            return _communicationManager.Put<ResourceAvailabilityModel, Response<ResourceAvailabilityModel>>(resourceAvailability, apiUrl);
        }

        /// <summary>
        /// Updates a room
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public Response<RoomModel> UpdateRoom(RoomModel room)
        {
            const string apiUrl = BaseRoute + "UpdateRoom";
            return _communicationManager.Put<RoomModel, Response<RoomModel>>(room, apiUrl);
        }

        /// <summary>
        /// Updates a resource override.
        /// </summary>
        /// <param name="resourceOverride"></param>
        /// <returns></returns>
        public Response<ResourceOverridesModel> UpdateResourceOverrides(ResourceOverridesModel resourceOverride)
        {
            const string apiUrl = BaseRoute + "UpdateResourceOverrides";
            return _communicationManager.Put<ResourceOverridesModel, Response<ResourceOverridesModel>>(resourceOverride, apiUrl);
        }

        /// <summary>
        /// Deletes a resource availability.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<ResourceAvailabilityModel> DeleteResourceAvailability(long id, System.DateTime modifiedOn)
        {
            const string apiUrl = BaseRoute + "DeleteResourceAvailability";
            var requestXMLValueNvc = new NameValueCollection
            {
                { "Id", id.ToString(CultureInfo.InvariantCulture) },
                {"modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture)}
            };
            return _communicationManager.Delete<Response<ResourceAvailabilityModel>>(requestXMLValueNvc, apiUrl);
        }

        /// <summary>
        /// Deletes a resource override.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<ResourceOverridesModel> DeleteResourceOverrides(long id, System.DateTime modifiedOn)
        {
            const string apiUrl = BaseRoute + "DeleteResourceOverrides";
            var requestXMLValueNvc = new NameValueCollection
            {
                { "Id", id.ToString(CultureInfo.InvariantCulture) },
                {"modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture)}
            };
            return _communicationManager.Delete<Response<ResourceOverridesModel>>(requestXMLValueNvc, apiUrl);
        }

    }
}