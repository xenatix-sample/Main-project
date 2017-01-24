using Axis.Configuration;
using Axis.Helpers;
using Axis.Model.Common;
using Axis.Model.Scheduling;
using Axis.Plugins.Scheduling.Models;
using Axis.Plugins.Scheduling.Translator;
using Axis.Service;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.Plugins.Scheduling.Repository.Resource
{
    /// <summary>
    ///
    /// </summary>
    public class ResourceRepository : IResourceRepository
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "resource/";

        /// <summary>
        /// constructor
        /// </summary>
        public ResourceRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="token">The token</param>
        public ResourceRepository(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the rooms.
        /// </summary>
        /// <param name="facilityId">The facility identifier.</param>
        /// <returns></returns>
        public Response<RoomViewModel> GetRooms(long? facilityId)
        {
            const string apiUrl = baseRoute + "GetRooms";
            var param = new NameValueCollection { { "facilityId", facilityId.ToString() } };
            var response = communicationManager.Get<Response<RoomModel>>(param, apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Gets the type of the credential by appointment.
        /// </summary>
        /// <param name="appointmentTypeID">The appointment type identifier.</param>
        /// <returns></returns>
        public Response<AppointmentCredentialViewModel> GetCredentialByAppointmentType(int? appointmentTypeID)
        {
            const string apiUrl = baseRoute + "GetCredentialByAppointmentType";
            var param = new NameValueCollection { { "appointmentTypeID", appointmentTypeID.ToString() } };
            var response = communicationManager.Get<Response<AppointmentCredentialModel>>(param, apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Gets the provider by credential.
        /// </summary>
        /// <param name="credentialId">The credential identifier.</param>
        /// <returns></returns>
        public Response<ProviderViewModel> GetProviderByCredential(long? credentialId)
        {
            const string apiUrl = baseRoute + "GetProviderByCredential";
            var param = new NameValueCollection { { "credentialId", credentialId.ToString() } };
            var response = communicationManager.Get<Response<ProviderModel>>(param, apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Gets the resources.
        /// </summary>
        /// <param name="resourceTypeId">The resource type identifier.</param>
        /// <returns></returns>
        public Response<ResourceViewModel> GetResources(int? resourceTypeId, int? facilityId)
        {
            const string apiUrl = baseRoute + "GetResources";
            var param = new NameValueCollection { 
                { "resourceTypeId", resourceTypeId.ToString() },
                { "facilityId", facilityId.ToString() }
            };
            var response = communicationManager.Get<Response<ResourceModel>>(param, apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Gets the resource details.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <returns></returns>
        public Response<ResourceViewModel> GetResourceDetails(int? resourceId, short? resourceTypeID)
        {
            const string apiUrl = baseRoute + "GetResourceDetails";
            var param = new NameValueCollection();
            param.Add("resourceId", resourceId.ToString());
            param.Add("resourceTypeID", resourceTypeID.ToString());

            var response = communicationManager.Get<Response<ResourceModel>>(param, apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Gets the resource availability.
        /// </summary>
        /// <param name="resourceId">The resource identifier.</param>
        /// <param name="resourceTypeID">The resource type identifier.</param>
        /// <returns></returns>
        public Response<ResourceAvailabilityViewModel> GetResourceAvailability(int? resourceId, short? resourceTypeID)
        {
            const string apiUrl = baseRoute + "GetResourceAvailability";
            var param = new NameValueCollection();
            param.Add("resourceId", resourceId.ToString());
            param.Add("resourceTypeID", resourceTypeID.ToString());
            var response = communicationManager.Get<Response<ResourceAvailabilityModel>>(param, apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Gets the resource overrides.
        /// </summary>
        /// <param name="resourceId">The resource identifier.</param>
        /// <param name="resourceTypeID">Type of the resource.</param>
        /// <returns></returns>
        public Response<ResourceOverridesViewModel> GetResourceOverrides(int? resourceId, short? resourceTypeID)
        {
            const string apiUrl = baseRoute + "GetResourceOverrides";
            var param = new NameValueCollection();
            param.Add("resourceId", resourceId.ToString());
            param.Add("resourceTypeID", resourceTypeID.ToString());
            var response = communicationManager.Get<Response<ResourceOverridesModel>>(param, apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Adds a resource availability.
        /// </summary>
        /// <param name="resourceAvailability"></param>
        /// <returns></returns>
        public Response<ResourceAvailabilityViewModel> AddResourceAvailability(ResourceAvailabilityViewModel resourceAvailability)
        {
            const string apiUrl = baseRoute + "AddResourceAvailability";
            return
                communicationManager.Post<ResourceAvailabilityModel, Response<ResourceAvailabilityModel>>(resourceAvailability.ToModel(), apiUrl)
                    .ToViewModel();
        }

        /// <summary>
        /// Adds a resource override.
        /// </summary>
        /// <param name="resourceOverride"></param>
        /// <returns></returns>
        public Response<ResourceOverridesViewModel> AddResourceOverrides(ResourceOverridesViewModel resourceOverride)
        {
            const string apiUrl = baseRoute + "AddResourceOverrides";
            return
                communicationManager.Post<ResourceOverridesModel, Response<ResourceOverridesModel>>(resourceOverride.ToModel(), apiUrl)
                    .ToViewModel();
        }

        /// <summary>
        /// Adds a resource detail.
        /// </summary>
        /// <param name="resourceDetail"></param>
        /// <returns></returns>
        public Response<ResourceViewModel> AddResourceDetails(ResourceViewModel resourceDetail)
        {
            const string apiUrl = baseRoute + "AddResourceDetails";
            return
                communicationManager.Post<ResourceModel, Response<ResourceModel>>(resourceDetail.ToModel(), apiUrl)
                    .ToViewModel();
        }

        /// <summary>
        /// Updates a resource availability.
        /// </summary>
        /// <param name="resourceAvailability"></param>
        /// <returns></returns>
        public Response<ResourceAvailabilityViewModel> UpdateResourceAvailability(ResourceAvailabilityViewModel resourceAvailability)
        {
            const string apiUrl = baseRoute + "UpdateResourceAvailability";
            return
                communicationManager.Put<ResourceAvailabilityModel, Response<ResourceAvailabilityModel>>(resourceAvailability.ToModel(), apiUrl)
                    .ToViewModel();
        }

        /// <summary>
        /// Updates a room
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public Response<RoomViewModel> UpdateRoom(RoomViewModel room)
        {
            const string apiUrl = baseRoute + "UpdateRoom";
            return
                communicationManager.Put<RoomModel, Response<RoomModel>>(room.ToModel(), apiUrl)
                    .ToViewModel();
        }

        /// <summary>
        /// Updates a resource override.
        /// </summary>
        /// <param name="resourceOverride"></param>
        /// <returns></returns>
        public Response<ResourceOverridesViewModel> UpdateResourceOverrides(ResourceOverridesViewModel resourceOverride)
        {
            const string apiUrl = baseRoute + "UpdateResourceOverrides";
            return
                communicationManager.Put<ResourceOverridesModel, Response<ResourceOverridesModel>>(resourceOverride.ToModel(), apiUrl)
                    .ToViewModel();
        }

        /// <summary>
        /// Deletes a resource availability.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<ResourceAvailabilityViewModel> DeleteResourceAvailability(long id, System.DateTime modifiedOn)
        {
            const string apiUrl = baseRoute + "DeleteResourceAvailability";
            var requestId = new NameValueCollection { { "id", id.ToString(CultureInfo.InvariantCulture) }, { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Delete<Response<ResourceAvailabilityViewModel>>(requestId, apiUrl);
        }

        /// <summary>
        /// Deletes a resource override.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<ResourceOverridesViewModel> DeleteResourceOverrides(long id, System.DateTime modifiedOn)
        {
            const string apiUrl = baseRoute + "DeleteResourceOverrides";
            var requestId = new NameValueCollection { { "id", id.ToString(CultureInfo.InvariantCulture) }, { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Delete<Response<ResourceOverridesViewModel>>(requestId, apiUrl);
        }
    }
}