using System;
using Axis.Configuration;
using Axis.Constant;
using Axis.Helpers;
using Axis.Model.Clinical.SocialRelationship;
using Axis.Model.Common;
using Axis.Plugins.Clinical.Models.SocialRelationship;
using Axis.Service;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axis.Plugins.Clinical.Translator;
using System.Globalization;


namespace Axis.Plugins.Clinical.Repository.SocialRelationship
{
    public class SocialRelationshipRepository:ISocialRelationshipRepository
    {
        #region Class Variables

        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "socialRelationship/";

        #endregion Class Variables

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SocialRelationshipRepository"/> class.
        /// </summary>
        public SocialRelationshipRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SocialRelationshipRepository"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public SocialRelationshipRepository(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Gets the social relationships by contact.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<SocialRelationshipViewModel> GetSocialRelationshipsByContact(long contactID)
        {
            const string apiUrl = baseRoute + "GetSocialRelationshipsByContact";
            var param = new NameValueCollection { { "contactID", contactID.ToString() } };
            var viewModel = communicationManager.Get<Response<SocialRelationshipModel>>(param, apiUrl);
            return viewModel.ToViewModel();
        }

        /// <summary>
        /// Adds the social relationship.
        /// </summary>
        /// <param name="model">The SocialRelationshipViewModel.</param>
        /// <returns></returns>
        public Response<SocialRelationshipViewModel> AddSocialRelationship(SocialRelationshipViewModel model)
        {
            const string apiUrl = baseRoute + "AddSocialRelationship";
            return
                communicationManager.Post<SocialRelationshipModel, Response<SocialRelationshipModel>>(model.ToModel(), apiUrl)
                    .ToViewModel();
        }

        /// <summary>
        /// Updates the social relationship.
        /// </summary>
         /// <param name="appointment">The SocialRelationshipViewModel.</param>
        /// <returns></returns>
        public Response<SocialRelationshipViewModel> UpdateSocialRelationship(SocialRelationshipViewModel appointment)
        {
            const string apiUrl = baseRoute + "UpdateSocialRelationship";
            return
                communicationManager.Put<SocialRelationshipModel, Response<SocialRelationshipModel>>(appointment.ToModel(), apiUrl)
                    .ToViewModel();
        }

        /// <summary>
        /// Deletes the social relationship.
        /// </summary>
        /// <param name="ID">The identifier.</param>
        /// <returns></returns>
         public Response<SocialRelationshipViewModel> DeleteSocialRelationship(long ID, DateTime modifiedOn)
        {
            const string apiUrl = baseRoute + "DeleteSocialRelationship";
            var requestId = new NameValueCollection { { "ID", ID.ToString(CultureInfo.InvariantCulture) }, { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Delete<Response<SocialRelationshipViewModel>>(requestId, apiUrl);
        }

        #endregion Public Methods
    }
}
