using Axis.Configuration;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Security;
using System;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.Service.Registration
{
    public class BenefitsAssistanceService : IBenefitsAssistanceService
    {
        private readonly CommunicationManager communicationManager;
        private const string BaseRoute = "BenefitsAssistance/";

        /// <summary>
        /// Initializes a new instance of the <see cref="BenefitsAssistanceService"/> class.
        /// </summary>
        public BenefitsAssistanceService()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        /// <summary>
        /// Gets the benefits assistance.
        /// </summary>
        /// <param name="benefitsAssistanceID">The benefits assistance identifier.</param>
        /// <returns></returns>
        public Response<BenefitsAssistanceModel> GetBenefitsAssistance(long benefitsAssistanceID)
        {
            const string apiUrl = BaseRoute + "GetBenefitsAssistance";
            var requestParams = new NameValueCollection { { "benefitsAssistanceID", benefitsAssistanceID.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Get<Response<BenefitsAssistanceModel>>(requestParams, apiUrl);
        }

        /// <summary>
        /// Gets the benefits assistance by contact identifier.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<BenefitsAssistanceModel> GetBenefitsAssistanceByContactID(long contactID)
        {
            const string apiUrl = BaseRoute + "GetBenefitsAssistanceByContactID";
            var requestParams = new NameValueCollection { { "contactID", contactID.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Get<Response<BenefitsAssistanceModel>>(requestParams, apiUrl);
        }

        /// <summary>
        /// Adds the benefits assistance.
        /// </summary>
        /// <param name="benefitsAssistanceModel">The benefits assistance model.</param>
        /// <returns></returns>
        public Response<BenefitsAssistanceModel> AddBenefitsAssistance(BenefitsAssistanceModel benefitsAssistanceModel)
        {
            const string apiUrl = BaseRoute + "AddBenefitsAssistance";
            return communicationManager.Post<BenefitsAssistanceModel, Response<BenefitsAssistanceModel>>(benefitsAssistanceModel, apiUrl);
        }

        /// <summary>
        /// Updates the benefits assistance.
        /// </summary>
        /// <param name="benefitsAssistanceModel">The benefits assistance model.</param>
        /// <returns></returns>
        public Response<BenefitsAssistanceModel> UpdateBenefitsAssistance(BenefitsAssistanceModel benefitsAssistanceModel)
        {
            const string apiUrl = BaseRoute + "UpdateBenefitsAssistance";
            return communicationManager.Put<BenefitsAssistanceModel, Response<BenefitsAssistanceModel>>(benefitsAssistanceModel, apiUrl);
        }

        /// <summary>
        /// Deletes the benefits assistance.
        /// </summary>
        /// <param name="benefitsAssistanceID">The benefits assistance identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        public Response<BenefitsAssistanceModel> DeleteBenefitsAssistance(long benefitsAssistanceID, DateTime modifiedOn)
        {
            const string apiUrl = BaseRoute + "DeleteBenefitsAssistance";
            var requestParams = new NameValueCollection
            {
                { "benefitsAssistanceID", benefitsAssistanceID.ToString(CultureInfo.InvariantCulture) },
                { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) }
            };
            return communicationManager.Delete<Response<BenefitsAssistanceModel>>(requestParams, apiUrl);
        }
    }
}
