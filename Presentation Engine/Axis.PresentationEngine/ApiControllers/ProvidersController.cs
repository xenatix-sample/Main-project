using Axis.Constant;
using Axis.Helpers.Caching;
using Axis.Model.Common;
using Axis.PresentationEngine.Areas.Admin.Respository.Provider;
using Axis.PresentationEngine.Helpers.Controllers;
using System.Collections.Generic;
using System.Web.Http;

namespace Axis.PresentationEngine.ApiControllers
{
    public class ProvidersController : BaseApiController
    {
        /// <summary>
        /// The providers repository
        /// </summary>
        private readonly IProvidersRepository providersRepository;
        private const int cacheTime = 24 * 60 * 60;
        /// <summary>
        /// The cache manager
        /// </summary>
        private readonly ICacheManager cacheManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProvidersController"/> class.
        /// </summary>
        /// <param name="providersRepository">The providers repository.</param>
        /// <param name="cacheManager">The cache manager.</param>
        public ProvidersController(IProvidersRepository providersRepository, ICacheManager cacheManager)
        {
            this.providersRepository = providersRepository;
            this.cacheManager = cacheManager;
        }

        /// <summary>
        /// Gets the providers.
        /// </summary>
        /// <param name="filterCriteria">The filter criteria.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<ProvidersModel> GetProviders(int filterCriteria)
        {
            var cacheKey = GetCacheKey(filterCriteria);
            var providers = new Response<ProvidersModel>();
            var cachedProviders = cacheManager.Get<List<ProvidersModel>>(cacheKey);
            if (cachedProviders == null)
            {
                providers = providersRepository.GetProviders(filterCriteria);
                cacheManager.Add(cacheKey, providers.DataItems, cacheTime);
            }
            else
            {
                providers.DataItems = cachedProviders;
            }
            return providers;
        }
        /// <summary>
        /// Gets the provider by id.
        /// </summary>
        /// <param name="providerID">The provider ID.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<ProvidersModel> GetProviderByid(int providerID)
        {
            return providersRepository.GetProviderByid(providerID);
        }

        /// <summary>
        /// Gets the cache key.
        /// </summary>
        /// <param name="filterCriteria">The filter criteria.</param>
        /// <returns></returns>
        private string GetCacheKey(int filterCriteria)
        {
            var key = (ProviderFilterType)filterCriteria;
            return string.Format("{0}/{1}", "providers", key);
        }
    }
}