using System;
using Axis.Configuration;
using Axis.Constant;
using Axis.Helpers;
using Axis.Model.Clinical.ReviewOfSystems;
using Axis.Model.Common;
using Axis.Model.Common.General;
using Axis.Plugins.Clinical.Models.ReviewOfSystems;
using Axis.Plugins.Clinical.Translator;
using Axis.PresentationEngine.Helpers.Model.General;
using Axis.PresentationEngine.Helpers.Translator;
using Axis.Service;
using System.Collections.Specialized;
using System.Globalization;


namespace Axis.Plugins.Clinical.Repository.ReviewOfSystems
{
    /// <summary>
    ///
    /// </summary>
    public class ReviewOfSystemsRepository : IReviewOfSystemsRepository
    {
        #region Class Variables

        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "reviewOfSystems/";

        #endregion Class Variables

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReviewOfSystemsRepository"/> class.
        /// </summary>
        public ReviewOfSystemsRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReviewOfSystemsRepository"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public ReviewOfSystemsRepository(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Gets the review of systems by contact.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<ReviewOfSystemsViewModel> GetReviewOfSystemsByContact(long contactID)
        {
            const string apiUrl = baseRoute + "GetReviewOfSystemsByContact";
            var param = new NameValueCollection { { "contactID", contactID.ToString() } };
            var viewModel = communicationManager.Get<Response<ReviewOfSystemsModel>>(param, apiUrl);
            return viewModel.ToViewModel();
        }

        /// <summary>
        /// Gets the review of system.
        /// </summary>
        /// <param name="rosID">The ros identifier.</param>
        /// <returns></returns>
        public Response<ReviewOfSystemsViewModel> GetReviewOfSystem(long rosID)
        {
            const string apiUrl = baseRoute + "GetReviewOfSystem";
            var param = new NameValueCollection { { "rosID", rosID.ToString() } };
            var viewModel = communicationManager.Get<Response<ReviewOfSystemsModel>>(param, apiUrl);
            return viewModel.ToViewModel();
        }

        /// <summary>
        /// Gets the last active review of systems.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<ReviewOfSystemsViewModel> GetLastActiveReviewOfSystems(long contactID)
        {
            const string apiUrl = baseRoute + "GetLastActiveReviewOfSystems";
            var param = new NameValueCollection { { "contactID", contactID.ToString() } };
            var viewModel = communicationManager.Get<Response<ReviewOfSystemsModel>>(param, apiUrl);
            return viewModel.ToViewModel();
        }

        /// <summary>
        /// Navigations the validation states.
        /// </summary>
        /// <param name="ContactID">The contact identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Response<KeyValueViewModel> NavigationValidationStates(long ContactID)
        {
            const string apiUrl = baseRoute + "NavigationValidationStates";
            var param = new NameValueCollection { { "contactID", ContactID.ToString() } };
            var viewModel = communicationManager.Get<Response<KeyValueModel>>(param, apiUrl);
            return viewModel.ToViewModel();
        }

        /// <summary>
        /// Adds the review of system.
        /// </summary>
        /// <param name="appointment">The appointment.</param>
        /// <returns></returns>
        public Response<ReviewOfSystemsViewModel> AddReviewOfSystem(ReviewOfSystemsViewModel appointment)
        {
            const string apiUrl = baseRoute + "AddReviewOfSystem";
            return
                communicationManager.Post<ReviewOfSystemsModel, Response<ReviewOfSystemsModel>>(appointment.ToModel(), apiUrl)
                    .ToViewModel();
        }

        /// <summary>
        /// Updates the review of system.
        /// </summary>
        /// <param name="appointment">The appointment.</param>
        /// <returns></returns>
        public Response<ReviewOfSystemsViewModel> UpdateReviewOfSystem(ReviewOfSystemsViewModel appointment)
        {
            const string apiUrl = baseRoute + "UpdateReviewOfSystem";
            return
                communicationManager.Put<ReviewOfSystemsModel, Response<ReviewOfSystemsModel>>(appointment.ToModel(), apiUrl)
                    .ToViewModel();
        }

        /// <summary>
        /// Deletes the review of system.
        /// </summary>
        /// <param name="rosID">The ros identifier.</param>
        /// <returns></returns>
        public Response<ReviewOfSystemsViewModel> DeleteReviewOfSystem(long rosID, DateTime modifiedOn)
        {
            const string apiUrl = baseRoute + "DeleteReviewOfSystem";
            var param = new NameValueCollection
            {
                {"rosID", rosID.ToString(CultureInfo.InvariantCulture)},
                {"modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture)}
            };
            return communicationManager.Delete<Response<ReviewOfSystemsViewModel>>(param, apiUrl);
        }

        #endregion Public Methods
    }
}