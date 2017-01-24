using Axis.Configuration;
using Axis.Helpers;
using Axis.Model.Common;
using Axis.Model.Consents;
using Axis.PresentationEngine.Areas.Consents.Models;
using Axis.Service;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axis.PresentationEngine.Areas.Consents.Translator;

namespace Axis.PresentationEngine.Areas.Consents.Repository
{

    public class ConsentsRepository : IConsentsRepository
    {

        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager _communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "Consents/";

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsentsRepository" /> class.
        /// </summary>
        public ConsentsRepository()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsentsRepository" /> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public ConsentsRepository(string token)
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Get Consents.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>       
        public Response<ConsentsViewModel> GetConsents(long contactID)
        {
            string apiUrl = baseRoute + "GetConsents";
            var parameters = new NameValueCollection { { "contactID", contactID.ToString(CultureInfo.InvariantCulture) } };
            var response = _communicationManager.Get<Response<ConsentsModel>>(parameters, apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Adds the consent.
        /// </summary>
        /// <param name="consent">The consent.</param>
        /// <returns></returns>
        public Response<ConsentsViewModel> AddConsent(ConsentsViewModel consent)
        {
            string apiUrl = baseRoute + "AddConsent";
            var response = _communicationManager.Post<ConsentsModel, Response<ConsentsModel>>(consent.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Updates the consent.
        /// </summary>
        /// <param name="consent">The consent.</param>
        /// <returns></returns>
        public Response<ConsentsViewModel> UpdateConsent(ConsentsViewModel consent)
        {
            string apiUrl = baseRoute + "UpdateConsent";
            var response = _communicationManager.Put<ConsentsModel, Response<ConsentsModel>>(consent.ToModel(), apiUrl);
            return response.ToViewModel();
        }

    }
}
