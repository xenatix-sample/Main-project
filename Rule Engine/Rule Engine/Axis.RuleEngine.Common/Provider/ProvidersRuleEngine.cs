using Axis.Model.Common;
using Axis.Service.Common.Provider;

namespace Axis.RuleEngine.Common.Provider
{
    public class ProvidersRuleEngine : IProvidersRuleEngine
    {
        /// <summary>
        /// The providers service
        /// </summary>
        private IProvidersService providersService;
        /// <summary>
        /// Initializes a new instance of the <see cref="ProvidersRuleEngine"/> class.
        /// </summary>
        /// <param name="providersService">The providers service.</param>
        public ProvidersRuleEngine(IProvidersService providersService)
        {
            this.providersService = providersService;
        }
        /// <summary>
        /// Gets the providers.
        /// </summary>
        /// <param name="filterCriteria">The filter criteria.</param>
        /// <returns></returns>
        public Response<ProvidersModel> GetProviders(int filterCriteria)
        {
            return providersService.GetProviders(filterCriteria);
        }
        /// <summary>
        /// Gets the provider by id.
        /// </summary>
        /// <param name="providerID">The provider ID.</param>
        /// <returns></returns>
        public Response<ProvidersModel> GetProviderByid(int providerID)
        {
            return providersService.GetProviderByid(providerID);
        }
    }
}