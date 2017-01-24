using System;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Configuration;
using Axis.Security;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.Service.Registration
{
    public class LettersService : ILettersService
    {
        private readonly CommunicationManager communicationManager;
        private const string BaseRoute = "Letters/";

        /// <summary>
        /// Initializes a new instance of the <see cref="LettersService"/> class.
        /// </summary>
        public LettersService()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        /// <summary>
        /// Gets the letters.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns>Response&lt;LettersModel&gt;.</returns>
        public Response<LettersModel> GetLetters(long contactID)
        {
            const string apiUrl = BaseRoute + "GetLetters";
            var requestParams = new NameValueCollection { { "contactID", contactID.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Get<Response<LettersModel>>(requestParams, apiUrl);
        }

        /// <summary>
        /// Adds the letters.
        /// </summary>
        /// <param name="lettersModel">The letters model.</param>
        /// <returns>Response&lt;LettersModel&gt;.</returns>
        public Response<LettersModel> AddLetters(LettersModel lettersModel)
        {
            const string apiUrl = BaseRoute + "AddLetters";
            return communicationManager.Post<LettersModel, Response<LettersModel>>(lettersModel, apiUrl);
        }

        /// <summary>
        /// Updates the letters.
        /// </summary>
        /// <param name="lettersModel">The letters model.</param>
        /// <returns>Response&lt;LettersModel&gt;.</returns>
        public Response<LettersModel> UpdateLetters(LettersModel lettersModel)
        {
            const string apiUrl = BaseRoute + "UpdateLetters";
            return communicationManager.Put<LettersModel, Response<LettersModel>>(lettersModel, apiUrl);
        }

        /// <summary>
        /// Deletes the letters.
        /// </summary>
        /// <param name="contactLettersID">The contact letters identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns>Response&lt;LettersModel&gt;.</returns>
        public Response<LettersModel> DeleteLetters(long contactLettersID, DateTime modifiedOn)
        {
            const string apiUrl = BaseRoute + "DeleteLetters";
            var requestParams = new NameValueCollection
            {
                { "ContactLettersID", contactLettersID.ToString(CultureInfo.InvariantCulture) },
                { "ModifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) }
            };
            return communicationManager.Delete<Response<LettersModel>>(requestParams, apiUrl);
        }
    }
}
