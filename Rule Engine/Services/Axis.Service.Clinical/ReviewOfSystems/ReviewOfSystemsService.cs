using System;
using Axis.Configuration;
using Axis.Model.Clinical.ReviewOfSystems;
using Axis.Model.Common;
using Axis.Model.Common.General;
using Axis.Security;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.Service.Clinical.ReviewOfSystems
{
    /// <summary>
    ///
    /// </summary>
    public class ReviewOfSystemsService : IReviewOfSystemsService
    {
        #region Class Variables

        /// <summary>
        /// The _communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string BaseRoute = "reviewOfSystems/";

        #endregion Class Variables

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReviewOfSystemsService"/> class.
        /// </summary>
        public ReviewOfSystemsService()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Gets the review of systems by contact.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<ReviewOfSystemsModel> GetReviewOfSystemsByContact(long contactID)
        {
            const string apiUrl = BaseRoute + "GetReviewOfSystemsByContact";
            var requestXMLValueNvc = new NameValueCollection { { "contactID", contactID.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Get<Response<ReviewOfSystemsModel>>(requestXMLValueNvc, apiUrl);
        }

        /// <summary>
        /// Gets the review of system.
        /// </summary>
        /// <param name="rosID">The ros identifier.</param>
        /// <returns></returns>
        public Response<ReviewOfSystemsModel> GetReviewOfSystem(long rosID)
        {
            const string apiUrl = BaseRoute + "GetReviewOfSystem";
            var requestXMLValueNvc = new NameValueCollection { { "rosID", rosID.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Get<Response<ReviewOfSystemsModel>>(requestXMLValueNvc, apiUrl);
        }

        /// <summary>
        /// Gets the last active review of systems.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<ReviewOfSystemsModel> GetLastActiveReviewOfSystems(long contactID)
        {
            const string apiUrl = BaseRoute + "GetLastActiveReviewOfSystems";
            var requestXMLValueNvc = new NameValueCollection { { "contactID", contactID.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Get<Response<ReviewOfSystemsModel>>(requestXMLValueNvc, apiUrl);
        }

        /// <summary>
        /// Navigations the validation states.
        /// </summary>
        /// <param name="ContactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<KeyValueModel> NavigationValidationStates(long ContactID)
        {
            const string apiUrl = BaseRoute + "NavigationValidationStates";
            var requestXMLValueNvc = new NameValueCollection { { "contactID", ContactID.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Get<Response<KeyValueModel>>(requestXMLValueNvc, apiUrl);
        }

        /// <summary>
        /// Adds the review of system.
        /// </summary>
        /// <param name="ros">The ros.</param>
        /// <returns></returns>
        public Response<ReviewOfSystemsModel> AddReviewOfSystem(ReviewOfSystemsModel ros)
        {
            const string apiUrl = BaseRoute + "AddReviewOfSystem";
            return communicationManager.Post<ReviewOfSystemsModel, Response<ReviewOfSystemsModel>>(ros, apiUrl);
        }

        /// <summary>
        /// Updates the review of system.
        /// </summary>
        /// <param name="ros">The ros.</param>
        /// <returns></returns>
        public Response<ReviewOfSystemsModel> UpdateReviewOfSystem(ReviewOfSystemsModel ros)
        {
            const string apiUrl = BaseRoute + "UpdateReviewOfSystem";
            return communicationManager.Put<ReviewOfSystemsModel, Response<ReviewOfSystemsModel>>(ros, apiUrl);
        }

        /// <summary>
        /// Deletes the review of system.
        /// </summary>
        /// <param name="rosID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<ReviewOfSystemsModel> DeleteReviewOfSystem(long rosID, DateTime modifiedOn)
        {
            const string apiUrl = BaseRoute + "DeleteReviewOfSystem";
            var requestXMLValueNvc = new NameValueCollection { { "rosID", rosID.ToString(CultureInfo.InvariantCulture) }, { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Delete<Response<ReviewOfSystemsModel>>(requestXMLValueNvc, apiUrl);
        }

        #endregion Public Methods
    }
}