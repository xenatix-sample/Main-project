using System;
using Axis.Model.Common;
using Axis.Service;
using Axis.Configuration;
using Axis.Helpers;
using System.Collections.Specialized;
using System.Globalization;
using Axis.Plugins.Registration.Translator;
using Axis.Plugins.Registration.Models;
using Axis.Model.Registration;

namespace Axis.Plugins.Registration.Repository
{
    public class LettersRepository : ILettersRepository
    {
        readonly CommunicationManager communicationManager;

        const string baseRoute = "Letters/";

        /// <summary>
        /// Initializes a new instance of the <see cref="LettersRepository"/> class.
        /// </summary>
        public LettersRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// Gets the letters.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns>Response&lt;LettersViewModel&gt;.</returns>
        public Response<LettersViewModel> GetLetters(long contactID)
        {
            const string apiUrl = baseRoute + "GetLetters";
            var param = new NameValueCollection { { "contactID", contactID.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Get<Response<LettersModel>>(param, apiUrl).ToViewModel();
        }

        /// <summary>
        /// Adds the letters.
        /// </summary>
        /// <param name="lettersModel">The letters model.</param>
        /// <returns>Response&lt;LettersViewModel&gt;.</returns>
        public Response<LettersViewModel> AddLetters(LettersViewModel lettersModel)
        {
            string apiUrl = baseRoute + "AddLetters";
            var response = communicationManager.Post<LettersModel, Response<LettersModel>>(lettersModel.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Updates the letters.
        /// </summary>
        /// <param name="lettersModel">The letters model.</param>
        /// <returns>Response&lt;LettersViewModel&gt;.</returns>
        public Response<LettersViewModel> UpdateLetters(LettersViewModel lettersModel)
        {
            string apiUrl = baseRoute + "UpdateLetters";
            var response = communicationManager.Put<LettersModel, Response<LettersModel>>(lettersModel.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Deletes the letters.
        /// </summary>
        /// <param name="contactLettersID">The contact letters identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns>Response&lt;LettersViewModel&gt;.</returns>
        public Response<LettersViewModel> DeleteLetters(long contactLettersID, DateTime modifiedOn)
        {
            string apiUrl = baseRoute;
            var param = new NameValueCollection
            {
                {"ContactLettersID", contactLettersID.ToString()},
                {"ModifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture)}
            };
            return communicationManager.Delete<Response<LettersViewModel>>(param, apiUrl);
        }
    }
}
