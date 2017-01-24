
using System;
using Axis.Configuration;
using Axis.Helpers;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Service;
using System.Collections.Specialized;
using System.Globalization;
using System.Threading.Tasks;
using Axis.Plugins.Registration.Translator;
using Axis.Model.Registration.Model;
using Axis.Constant;


namespace Axis.Plugins.Registration.Repository
{
    /// <summary>
    /// Repository for Collateral to call web api methods.
    /// </summary>
    public class CollateralRepository : ICollateralRepository
    {
        private readonly CommunicationManager communicationManager;
        private const string baseRoute = "collateral/";

        /// <summary>
        /// constructor
        /// </summary>
        public CollateralRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="token">token</param>
        public CollateralRepository(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// To get Collateral list for contact
        /// </summary>
        /// <param name="contactID">Contact Id of contact</param>
        /// <param name="contactTypeId">Type of Contact</param>
        /// <returns>Response of type CollateralModel</returns>
        public Response<CollateralModel> GetCollaterals(long contactID, int contactTypeId, bool getContactDetails)
        {
            return GetCollateralsAsync(contactID, contactTypeId, getContactDetails).Result;
        }

        /// <summary>
        /// To get Collateral list for contact
        /// </summary>
        /// <param name="contactID">Contact Id of contact</param>
        /// <param name="contactTypeId">Type of Contact</param>
        /// <returns>Response of type CollateralModel</returns>
        public async Task<Response<CollateralModel>> GetCollateralsAsync(long contactID, int contactTypeId, bool getContactDetails)
        {
            const string apiUrl = baseRoute + "GetCollaterals";
            var param = new NameValueCollection { 
                            { "contactID", contactID.ToString(CultureInfo.InvariantCulture) },
                            { "contactTypeId", contactTypeId.ToString(CultureInfo.InvariantCulture) },
                            { "getContactDetails", getContactDetails.ToString(CultureInfo.InvariantCulture) }
            };
            return await communicationManager.GetAsync<Response<CollateralModel>>(param, apiUrl);
        }

        /// <summary>
        /// To add collateral for contact
        /// </summary>
        /// <param name="collatral">Collateral Model</param>
        /// <returns>Response of type CollateralViewModel</returns>
        public Response<CollateralViewModel> AddCollateral(CollateralViewModel collateral)
        {
            const string apiUrl = baseRoute + "AddCollateral";
            var response = communicationManager.Post<CollateralModel, Response<CollateralModel>>(collateral.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// To update collateral for contact
        /// </summary>
        /// <param name="collateral"></param>
        /// <returns></returns>
        public Response<CollateralViewModel> UpdateCollateral(CollateralViewModel collateral)
        {
            const string apiUrl = baseRoute + "UpdateCollateral";
            var response = communicationManager.Put<CollateralModel, Response<CollateralModel>>(collateral.ToModel(), apiUrl);
            return response.ToViewModel();
        }


        /// <summary>
        /// Deletes the collateral.
        /// </summary>
        /// <param name="parentContactID">The parent contact identifier.</param>
        /// <param name="contactID">The contact identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        public Response<CollateralViewModel> DeleteCollateral(long parentContactID, long contactID, DateTime modifiedOn)
        {
            const string apiUrl = baseRoute + "DeleteCollateral";
            var requestId = new NameValueCollection { { "parentContactID", parentContactID.ToString(CultureInfo.InvariantCulture) }, { "contactID", contactID.ToString(CultureInfo.InvariantCulture) }, { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) } };
            var response = communicationManager.Delete<Response<CollateralModel>>(requestId, apiUrl);
            return response.ToViewModel();
        }
    }
}