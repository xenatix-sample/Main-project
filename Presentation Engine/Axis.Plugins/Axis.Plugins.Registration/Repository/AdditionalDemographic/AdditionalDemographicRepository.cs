using System;
using Axis.Configuration;
using Axis.Constant;
using Axis.Helpers;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Plugins.Registration.Model;
using Axis.Plugins.Registration.Translator;
using Axis.Service;
using System.Collections.Specialized;
using System.Globalization;
using System.Threading.Tasks;

namespace Axis.Plugins.Registration.Repository
{
    /// <summary>
    ///
    /// </summary>
    public class AdditionalDemographicRepository : IAdditionalDemographicRepository
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "additionalDemographic/";

        /// <summary>
        /// Initializes a new instance of the <see cref="AdditionalDemographicRepository"/> class.
        /// </summary>
        public AdditionalDemographicRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdditionalDemographicRepository"/> class.
        /// </summary>
        /// <param name="token">The token.</param>        
        public AdditionalDemographicRepository(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the additional demographic.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        public Response<AdditionalDemographicsViewModel> GetAdditionalDemographic(long contactId)
        {
            return GetAdditionalDemographicAsync(contactId).Result;
        }

        /// <summary>
        /// Gets the additional demographic.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        public async Task<Response<AdditionalDemographicsViewModel>> GetAdditionalDemographicAsync(long contactId)
        {
            const string apiUrl = baseRoute + "GetAdditionalDemographic";

            var param = new NameValueCollection();
            param.Add("contactId", contactId.ToString());

            return (await communicationManager.GetAsync<Response<AdditionalDemographicsModel>>(param, apiUrl)).ToViewModel();
        }

        /// <summary>
        /// Adds the additional demographic.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <returns></returns>
        public Response<AdditionalDemographicsViewModel> AddAdditionalDemographic(AdditionalDemographicsViewModel patient)
        {
            string apiUrl = baseRoute + "addAdditionalDemographic";
            var response = communicationManager.Post<AdditionalDemographicsModel, Response<AdditionalDemographicsModel>>(patient.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Updates the additional demographic.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <returns></returns>
        public Response<AdditionalDemographicsViewModel> UpdateAdditionalDemographic(AdditionalDemographicsViewModel patient)
        {
            string apiUrl = baseRoute + "updateAdditionalDemographic";
            var response = communicationManager.Post<AdditionalDemographicsModel, Response<AdditionalDemographicsModel>>(patient.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Deletes the additional demographic.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public void DeleteAdditionalDemographic(long id, DateTime modifiedOn)
        {
            string apiUrl = baseRoute;
            var param = new NameValueCollection
            {
                {"id", id.ToString()},
                {"modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture)}
            };
            communicationManager.Delete(param, apiUrl);
        }
    }
}