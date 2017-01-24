using System;
using Axis.Configuration;
using Axis.Helpers;
using Axis.Service;
using System.Threading.Tasks;
using Axis.Constant;
using Axis.Model.Common;
using System.Collections.Specialized;
using System.Globalization;
using Axis.Plugins.ECI.Translator;
using Axis.Model.ECI;
namespace Axis.Plugins.ECI
{
    public class ECIAdditionalDemographicRepository :IECIAdditionalDemographicRepository
    {
             /// <summary>
        /// The communication manager
        /// </summary> 
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "eciAdditionalDemographic/";

        /// <summary>
        /// Initializes a new instance of the <see cref="AdditionalDemographicRepository"/> class.
        /// </summary>
        public ECIAdditionalDemographicRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdditionalDemographicRepository"/> class.
        /// </summary>
        /// <param name="token">The token.</param>        
        public ECIAdditionalDemographicRepository(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the additional demographic.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        public Response<ECIAdditionalDemographicsViewModel> GetAdditionalDemographic(long contactId)
        {
            return GetAdditionalDemographicAsync(contactId).Result;
        }

        /// <summary>
        /// Gets the additional demographic.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        public async Task<Response<ECIAdditionalDemographicsViewModel>> GetAdditionalDemographicAsync(long contactId)
        {
            const string apiUrl = baseRoute + "GetAdditionalDemographic";

            var param = new NameValueCollection();
            param.Add("contactId", contactId.ToString());
            return (await communicationManager.GetAsync<Response<ECIAdditionalDemographicsModel>>(param, apiUrl)).ToViewModel();
        }

        /// <summary>
        /// Adds the additional demographic.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <returns></returns>
        public Response<ECIAdditionalDemographicsViewModel> AddAdditionalDemographic(ECIAdditionalDemographicsViewModel patient)
        {
            string apiUrl = baseRoute + "addAdditionalDemographic";
            var response = communicationManager.Post<ECIAdditionalDemographicsModel, Response<ECIAdditionalDemographicsModel>>(patient.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Updates the additional demographic.
        /// </summary>
        /// <param name="patient">The patient.</param>
        /// <returns></returns>
        public Response<ECIAdditionalDemographicsViewModel> UpdateAdditionalDemographic(ECIAdditionalDemographicsViewModel patient)
        {
            string apiUrl = baseRoute + "updateAdditionalDemographic";
            var response = communicationManager.Post<ECIAdditionalDemographicsModel, Response<ECIAdditionalDemographicsModel>>(patient.ToModel(), apiUrl);
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
