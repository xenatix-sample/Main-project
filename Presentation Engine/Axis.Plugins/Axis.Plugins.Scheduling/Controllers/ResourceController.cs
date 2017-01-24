using Axis.Plugins.Scheduling.Models;
using Axis.Plugins.Scheduling.Repository.Resource;
using Axis.PresentationEngine.Helpers.Controllers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Axis.Plugins.Scheduling.Controllers
{
    /// <summary>
    ///
    /// </summary>
    public class ResourceController : BaseController
    {
        /// <summary>
        /// The resource repository
        /// </summary>
        private readonly IResourceRepository resourceRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceController"/> class.
        /// </summary>
        /// <param name="resourceRepository">The resource repository.</param>
        public ResourceController(IResourceRepository resourceRepository)
        {
            this.resourceRepository = resourceRepository;
        }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Get get cached all the schedule related data
        /// </summary>
        /// <returns>View</returns>
        [HttpGet]
        public ActionResult GetResourcesToCache()
        {
            //TODO: Remove static Ids

            //Get Rooms
            var rooms = resourceRepository.GetRooms(null).DataItems;

            //Get Rooms
            var credentialByAppointmentType = resourceRepository.GetCredentialByAppointmentType(null).DataItems;

            //Get Rooms
            var providerByCredential = resourceRepository.GetProviderByCredential(null).DataItems;

            //Get Rooms
            var resourcesType = resourceRepository.GetResources(null,null).DataItems;

            //Get Rooms
            var resourceDetails = resourceRepository.GetResourceDetails(null,null).DataItems;

            //Get Rooms
            var resourceAvailability = resourceRepository.GetResourceAvailability(null,null).DataItems;

            //Get Rooms
            var resourceOverrides = resourceRepository.GetResourceOverrides(null,null).DataItems;

            var resourceCacheViewModel = new ResourcesToCacheViewModel()
            {
                Rooms = JsonConvert.SerializeObject(rooms),
                CredentialByAppointmentType = JsonConvert.SerializeObject(credentialByAppointmentType),
                ProviderByCredential = JsonConvert.SerializeObject(providerByCredential),
                ResourcesType = JsonConvert.SerializeObject(resourcesType),
                ResourceDetails = JsonConvert.SerializeObject(resourceDetails),
                ResourceAvailability = JsonConvert.SerializeObject(resourceAvailability),
                ResourceOverrides = JsonConvert.SerializeObject(resourceOverrides)
            };

            Response.ContentType = "text/javascript";
            return View(resourceCacheViewModel);
        }
    }
}