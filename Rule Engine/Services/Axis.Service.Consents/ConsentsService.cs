using Axis.Configuration;
using Axis.Model.Common;
using Axis.Model.Consents;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axis.Service;
using Axis.Security;
using Axis.Model.Account;

namespace Axis.Service.Consents
{
    public class ConsentsService : IConsentsService
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private CommunicationManager _communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "Consents/";

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsentsService" /> class.
        /// </summary>
        public ConsentsService()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        /// <summary>
        /// Gets the Consents.
        /// </summary>
        /// <param name="contactID">The contactID identifier.</param>
        /// <returns></returns>
        public Response<ConsentsModel> GetConsents(long contactID)
        {
            var apiUrl = baseRoute + "GetConsents";
            var param = new NameValueCollection();
            param.Add("contactID", contactID.ToString());
            return _communicationManager.Get<Response<ConsentsModel>>(param, apiUrl);
        }

        /// <summary>
        /// Adds the consent.
        /// </summary>
        /// <param name="consent">The consent.</param>
        /// <returns></returns>
        public Response<ConsentsModel> AddConsent(ConsentsModel consent)
        {
            string apiUrl = baseRoute + "AddConsent";
            return _communicationManager.Post<ConsentsModel, Response<ConsentsModel>>(consent, apiUrl);
        }

        /// <summary>
        /// Updates the consents.
        /// </summary>
        /// <param name="consent">The consent.</param>
        /// <returns></returns>
        public Response<ConsentsModel> UpdateConsent(ConsentsModel consent)
        {
            string apiUrl = baseRoute + "UpdateConsent";
            return _communicationManager.Post<ConsentsModel, Response<ConsentsModel>>(consent, apiUrl);
        }
    }
}
