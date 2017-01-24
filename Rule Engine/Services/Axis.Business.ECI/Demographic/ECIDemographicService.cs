using System.Collections.Specialized;
using System.Globalization;
using Axis.Configuration;
using Axis.Model.Common;
using Axis.Model.ECI;
using Axis.Security;

namespace Axis.Service.ECI.Demographic
{
    public class ECIDemographicService : IECIDemographicService
    {
        #region Class Variables

        private readonly CommunicationManager _communicationManager;
        private const string BaseRoute = "eciDemographic/";

        #endregion

        #region Constructors

        public ECIDemographicService()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the contact demographics.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<ECIContactDemographicsModel> GetContactDemographics(long contactID)
        {
            string apiUrl = BaseRoute + "GetContactDemographics";
            var requestXMLValueNvc = new NameValueCollection { { "contactID", contactID.ToString(CultureInfo.InvariantCulture) } };

            return _communicationManager.Get<Response<ECIContactDemographicsModel>>(requestXMLValueNvc, apiUrl);
        }

        /// <summary>
        /// Adds the contact demographics.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns></returns>
        public Response<ECIContactDemographicsModel> AddContactDemographics(ECIContactDemographicsModel contact)
        {
            string apiUrl = BaseRoute + "AddContactDemographics";
            return _communicationManager.Post<ECIContactDemographicsModel, Response<ECIContactDemographicsModel>>(contact, apiUrl);
        }

        /// <summary>
        /// Updates the contact demographics.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns></returns>
        public Response<ECIContactDemographicsModel> UpdateContactDemographics(ECIContactDemographicsModel contact)
        {
            string apiUrl = BaseRoute + "UpdateContactDemographics";
            return _communicationManager.Post<ECIContactDemographicsModel, Response<ECIContactDemographicsModel>>(contact, apiUrl);
        }

        #endregion
    }
}
