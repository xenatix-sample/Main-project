using System.Collections.Specialized;
using System.Globalization;
using System.Threading.Tasks;
using Axis.Configuration;
using Axis.Constant;
using Axis.Helpers;
using Axis.Model.Common;
using Axis.Model.ECI;
using Axis.Plugins.ECI.Models.ECIDemographics;
using Axis.Plugins.ECI.Translator;
using Axis.Service;

namespace Axis.Plugins.ECI.Repository.ECIDemographic
{
    public class ECIDemographicRepository : IECIDemographicRepository
    {
        #region Class Variables

        private readonly CommunicationManager _communicationManager;

        private const string BaseRoute = "ECIDemographic/";

        #endregion

        #region Constructors

        public ECIDemographicRepository()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the contact demographics.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public async Task<Response<ECIContactDemographicsViewModel>> GetContactDemographics(long contactID)
        {
            string apiUrl = string.Format("{0}{1}", BaseRoute, "GetContactDemographics");
            var parameters = new NameValueCollection { { "contactID", contactID.ToString(CultureInfo.InvariantCulture) } };
            var response = await _communicationManager.GetAsync<Response<ECIContactDemographicsModel>>(parameters, apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Adds the contact demographics.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns></returns>
        public Response<ECIContactDemographicsViewModel> AddContactDemographics(ECIContactDemographicsViewModel contact)
        {
            string apiUrl = string.Format("{0}{1}", BaseRoute, "AddContactDemographics");
            var response = _communicationManager.Post<ECIContactDemographicsModel, Response<ECIContactDemographicsModel>>(contact.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Updates the contact demographics.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns></returns>
        public Response<ECIContactDemographicsViewModel> UpdateContactDemographics(ECIContactDemographicsViewModel contact)
        {
            string apiUrl = string.Format("{0}{1}", BaseRoute, "UpdateContactDemographics");
            var response = _communicationManager.Post<ECIContactDemographicsModel, Response<ECIContactDemographicsModel>>(contact.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        #endregion
    }
}
