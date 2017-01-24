using Axis.Configuration;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Security;
using System;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.Service.Registration
{
    /// <summary>
    /// Emergency Contact Service class to call the web api methods
    /// </summary>
    public class CollateralService : ICollateralService
    {
        private readonly CommunicationManager communicationManager;
        private const string BaseRoute = "collateral/";

        /// <summary>
        /// Constructor
        /// </summary>
        public CollateralService()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="token"></param>
        public CollateralService(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// To get the list of collateral
        /// </summary>
        /// <param name="contactID">Contact Id</param>
        /// <param name="contactTypeId">Type of contact</param>
        /// <returns></returns>
        public Response<CollateralModel> GetCollaterals(long contactID, int contactTypeId, bool getContactDetails)
        {
            const string apiUrl = BaseRoute + "GetCollaterals";
            var requestId = new NameValueCollection { 
                            { "contactID", contactID.ToString(CultureInfo.InvariantCulture) },
                            { "contactTypeId", contactTypeId.ToString(CultureInfo.InvariantCulture) }, 
                            { "getContactDetails", getContactDetails.ToString(CultureInfo.InvariantCulture) } 
                            };
            return communicationManager.Get<Response<CollateralModel>>(requestId, apiUrl);
        }

        /// <summary>
        /// To add collateral
        /// </summary>
        /// <param name="collateralModel"></param>
        /// <returns></returns>
        public Response<CollateralModel> AddCollateral(CollateralModel collateralModel)
        {
            const string apiUrl = BaseRoute + "AddCollateral";
            return communicationManager.Post<CollateralModel, Response<CollateralModel>>(collateralModel, apiUrl);
        }

        /// <summary>
        /// To update collateral
        /// </summary>
        /// <param name="collateralModel"></param>
        /// <returns></returns>
        public Response<CollateralModel> UpdateCollateral(CollateralModel collateralModel)
        {
            const string apiUrl = BaseRoute + "UpdateCollateral";
            return communicationManager.Put<CollateralModel, Response<CollateralModel>>(collateralModel, apiUrl);
        }


        /// <summary>
        /// Deletes the collateral.
        /// </summary>
        /// <param name="parentContactID">The parent contact identifier.</param>
        /// <param name="contactID">The contact identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        public Response<CollateralModel> DeleteCollateral(long parentContactID, long contactID, DateTime modifiedOn)
        {
            const string apiUrl = BaseRoute + "DeleteCollateral";
            var requestId = new NameValueCollection
            {
                { "parentContactID", parentContactID.ToString(CultureInfo.InvariantCulture) },
                { "contactID", contactID.ToString(CultureInfo.InvariantCulture) },
                { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) }
            };
            return communicationManager.Delete<Response<CollateralModel>>(requestId, apiUrl);
        }
    }
}
