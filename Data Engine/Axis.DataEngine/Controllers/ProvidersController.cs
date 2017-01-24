using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Common;
using Axis.Model.Common;
using System.Web.Http;

namespace Axis.DataEngine.Service.Controllers
{
    public class ProvidersController : BaseApiController
    {
        /// <summary>
        /// The providers data provider
        /// </summary>
        private readonly IProvidersDataProvider providersDataProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProvidersController"/> class.
        /// </summary>
        /// <param name="providersDataProvider">The providers data provider.</param>
        public ProvidersController(IProvidersDataProvider providersDataProvider)
        {
            this.providersDataProvider = providersDataProvider;
        }

        /// <summary>
        /// Gets the providers.
        /// </summary>
        /// <param name="filterCriteria">The filter criteria.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetProviders(int filterCriteria)
        {
            return new HttpResult<Response<ProvidersModel>>(providersDataProvider.GetProviders(filterCriteria), Request);
        }

        [HttpGet]
        public IHttpActionResult GetProviderByid(int providerID)
        {
            return new HttpResult<Response<ProvidersModel>>(providersDataProvider.GetProviderByid(providerID), Request);
        }


    }
}