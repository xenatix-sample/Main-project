using Axis.Plugins.Registration.Repository;
using Axis.PresentationEngine.Helpers.Controllers;
using Axis.Model.Common;
using Axis.Model.Registration;
using System.Web.Http;
using System.Web;

namespace Axis.Plugins.Registration.ApiControllers
{
    /// <summary>
    /// This class is use to perform action for client search screen
    /// </summary>
    public class ClientSearchController : BaseApiController
    {
        #region Private Variables

        private readonly IClientSearchRepository _clientSearchRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes the ClientSearchRepository
        /// </summary>
        /// <param name="clientSearchRepository">object of ClientSearchRepository interface, injected through dependency injection</param>
        public ClientSearchController(IClientSearchRepository clientSearchRepository)
        {
            this._clientSearchRepository = clientSearchRepository;
        }

        #endregion

        #region Data API

        /// <summary>
        /// Get the patients detail based on the pass search criteria.
        /// </summary>
        /// <param name="SearchCriteria">text to search in Contact data</param>
        /// <param name="contactType">contact type of contact</param>
        /// <returns>Result contact data in json format</returns>
        [HttpGet]
        public Response<ContactDemographicsModel> GetClientSummary(string searchCriteria,string contactType)
        {
            searchCriteria = HttpUtility.UrlEncode(searchCriteria);
            if (string.IsNullOrEmpty(searchCriteria) || string.IsNullOrWhiteSpace(searchCriteria))
                searchCriteria = string.Empty;
            return _clientSearchRepository.GetClientSummary(searchCriteria, contactType);
        }

        #endregion
    }
}
