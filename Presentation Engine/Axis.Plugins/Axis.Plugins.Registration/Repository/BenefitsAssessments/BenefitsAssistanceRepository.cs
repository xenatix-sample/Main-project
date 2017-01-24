using System;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Service;
using Axis.Configuration;
using Axis.Helpers;
using Axis.Constant;
using System.Collections.Specialized;
using System.Globalization;
using Axis.Plugins.Registration.Models;
using Axis.Plugins.Registration.Translator;

namespace Axis.Plugins.Registration.Repository
{
    public class BenefitsAssistanceRepository : IBenefitsAssistanceRepository
    {
        readonly CommunicationManager communicationManager;

        const string baseRoute = "BenefitsAssistance/";

        /// <summary>
        /// Initializes a new instance of the <see cref="BenefitsAssistanceRepository"/> class.
        /// </summary>
        public BenefitsAssistanceRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// Gets the benefits assistance.
        /// </summary>
        /// <param name="benefitsAssistanceID">The benefits assistance identifier.</param>
        /// <returns></returns>
        public Response<BenefitsAssistanceViewModel> GetBenefitsAssistance(long benefitsAssistanceID)
        {
            const string apiUrl = baseRoute + "GetBenefitsAssistance";
            var param = new NameValueCollection { { "benefitsAssistanceID", benefitsAssistanceID.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Get<Response<BenefitsAssistanceModel>>(param, apiUrl).ToViewModel();
        }

        /// <summary>
        /// Gets the benefits assistance by contact identifier.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<BenefitsAssistanceViewModel> GetBenefitsAssistanceByContactID(long contactID)
        {
            const string apiUrl = baseRoute + "GetBenefitsAssistanceByContactID";
            var param = new NameValueCollection { { "contactID", contactID.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Get<Response<BenefitsAssistanceModel>>(param, apiUrl).ToViewModel();
        }

        /// <summary>
        /// Adds the benefits assistance.
        /// </summary>
        /// <param name="benefitsAssistance">The benefits assistance.</param>
        /// <returns></returns>
        public Response<BenefitsAssistanceViewModel> AddBenefitsAssistance(BenefitsAssistanceViewModel benefitsAssistance)
        {
            string apiUrl = baseRoute + "AddBenefitsAssistance";
            var response = communicationManager.Post<BenefitsAssistanceModel, Response<BenefitsAssistanceModel>>(benefitsAssistance.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Updates the benefits assistance.
        /// </summary>
        /// <param name="benefitsAssistance">The benefits assistance.</param>
        /// <returns></returns>
        public Response<BenefitsAssistanceViewModel> UpdateBenefitsAssistance(BenefitsAssistanceViewModel benefitsAssistance)
        {
            string apiUrl = baseRoute + "UpdateBenefitsAssistance";
            var response = communicationManager.Put<BenefitsAssistanceModel, Response<BenefitsAssistanceModel>>(benefitsAssistance.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Deletes the benefits assistance.
        /// </summary>
        /// <param name="benefitsAssistanceID">The benefits assistance identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        public Response<BenefitsAssistanceViewModel> DeleteBenefitsAssistance(long benefitsAssistanceID, DateTime modifiedOn)
        {
            string apiUrl = baseRoute;
            var param = new NameValueCollection
            {
                {"benefitsAssistanceID", benefitsAssistanceID.ToString()},
                {"modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture)}
            };
            return communicationManager.Delete<Response<BenefitsAssistanceViewModel>>(param, apiUrl);
        }

    }
}
