using System;
using Axis.Configuration;
using Axis.Constant;
using Axis.Helpers;
using Axis.Model.Common;
using Axis.Service;
using System.Collections.Specialized;
using System.Globalization;
using System.Threading.Tasks;
using Axis.Model.Clinical;
using Axis.Plugins.Clinical.Models.Vital;
using Axis.Plugins.Clinical.Translator;


namespace Axis.Plugins.Clinical.Repository.Vital
{
    public class VitalRepository : IVitalRepository
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "Vital/";

        /// <summary>
        /// Initializes a new instance of the <see cref="VitalRepository" /> class.
        /// </summary>
        public VitalRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VitalRepository" /> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public VitalRepository(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the contact Vitals.
        /// </summary>
        /// <param name="ContactId">The contact identifier.</param>
        /// <returns></returns>
        public Response<VitalViewModel> GetContactVitals(long ContactId)
        {
            return GetContactVitalsAsync(ContactId).Result;
        }

        /// <summary>
        /// Gets the contact Vitals.
        /// </summary>
        /// <param name="ContactId">The contact identifier.</param>
        /// <returns></returns>
        public async Task<Response<VitalViewModel>> GetContactVitalsAsync(long ContactId)
        {
            string apiUrl = baseRoute + "GetContactVitals";
            var parameters = new NameValueCollection { { "ContactId", ContactId.ToString(CultureInfo.InvariantCulture) } };
            var response = await communicationManager.GetAsync<Response<VitalModel>>(parameters, apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Adds the Vital.
        /// </summary>
        /// <param name="vital">The vital.</param>
        /// <returns></returns>
        public Response<VitalViewModel> AddVital(VitalViewModel vital)
        {
            string apiUrl = baseRoute + "AddVital";
            var response = communicationManager.Post<VitalModel, Response<VitalModel>>(vital.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Updates the Vital.
        /// </summary>
        /// <param name="vital">The Vital.</param>
        /// <returns></returns>
        public Response<VitalViewModel> UpdateVital(VitalViewModel vital)
        {
            string apiUrl = baseRoute + "UpdateVital";
            var response = communicationManager.Put<VitalModel, Response<VitalModel>>(vital.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Deletes the Vital.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<VitalViewModel> DeleteVital(long id, DateTime modifiedOn)
        {
            string apiUrl = baseRoute;
            var param = new NameValueCollection
            {
                {"id", id.ToString()},
                {"modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture)}
            };

            return communicationManager.Delete<Response<VitalViewModel>>(param, apiUrl);
        }
    }
}
