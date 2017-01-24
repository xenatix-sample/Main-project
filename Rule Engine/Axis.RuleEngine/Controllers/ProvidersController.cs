using Axis.Model.Common;
using Axis.RuleEngine.Common.Provider;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Results;
using System.Web.Http;

namespace Axis.RuleEngine.Service.Controllers
{
    public class ProvidersController : BaseApiController
    {
        /// <summary>
        /// The providers rule engine
        /// </summary>
        private readonly IProvidersRuleEngine providersRuleEngine = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProvidersController"/> class.
        /// </summary>
        /// <param name="providersRuleEngine">The providers rule engine.</param>
        public ProvidersController(IProvidersRuleEngine providersRuleEngine)
        {
            this.providersRuleEngine = providersRuleEngine;
        }

        /// <summary>
        /// Gets the providers.
        /// </summary>
        /// <param name="filterCriteria">The filter criteria.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetProviders(int filterCriteria)
        {
            return new HttpResult<Response<ProvidersModel>>(providersRuleEngine.GetProviders(filterCriteria), Request);
        }
        /// <summary>
        /// Gets the provider by id.
        /// </summary>
        /// <param name="providerID">The provider ID.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetProviderByid(int providerID)
        {
            return new HttpResult<Response<ProvidersModel>>(providersRuleEngine.GetProviderByid(providerID), Request);
        }
    }
}