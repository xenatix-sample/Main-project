using Axis.Configuration;
using Axis.Model.Clinical.SocialRelationship;
using Axis.Model.Common;
using Axis.Security;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Service.Clinical.SocialRelationship
{
    public class SocialRelationshipService :ISocialRelationshipService
    {
         #region Class Variables

        /// <summary>
        /// The _communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string BaseRoute = "socialRelationship/";

        #endregion Class Variables

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SocialRelationshipsService"/> class.
        /// </summary>
        public SocialRelationshipService()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Gets the social relationship by contact.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<SocialRelationshipModel> GetSocialRelationshipsByContact(long contactID)
        {
            const string apiUrl = BaseRoute + "GetSocialRelationshipsByContact";
            var requestXMLValueNvc = new NameValueCollection { { "contactID", contactID.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Get<Response<SocialRelationshipModel>>(requestXMLValueNvc, apiUrl);
        }

        /// <summary>
        /// Adds the social relationship.
        /// </summary>
        /// <param name="model">The SocialRelationshipModel.</param>
        /// <returns></returns>
        public Response<SocialRelationshipModel> AddSocialRelationship(SocialRelationshipModel model)
        {
            const string apiUrl = BaseRoute + "AddSocialRelationship";
            return communicationManager.Post<SocialRelationshipModel, Response<SocialRelationshipModel>>(model, apiUrl);
        }

        /// <summary>
        /// Updates the social relationship.
        /// </summary>
        /// <param name="model">The SocialRelationshipModel.</param>
        /// <returns></returns>
        public Response<SocialRelationshipModel> UpdateSocialRelationship(SocialRelationshipModel model)
        {
            const string apiUrl = BaseRoute + "UpdateSocialRelationship";
            return communicationManager.Put<SocialRelationshipModel, Response<SocialRelationshipModel>>(model, apiUrl);
        }

        /// <summary>
        /// Deletes the social relationship.
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<SocialRelationshipModel> DeleteSocialRelationship(long ID, DateTime modifiedOn)
        {
            const string apiUrl = BaseRoute + "DeleteSocialRelationship";
            var requestXMLValueNvc = new NameValueCollection { { "ID", ID.ToString(CultureInfo.InvariantCulture) }, { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Delete<Response<SocialRelationshipModel>>(requestXMLValueNvc, apiUrl);
        }

        #endregion Public Methods
    }
}
