using System;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Model.Registration.Model;
using System.Threading.Tasks;
using System.Web.Http;
using Axis.PresentationEngine.Helpers.Controllers;


namespace Axis.Plugins.Registration.ApiControllers
{
    /// <summary>
    /// Controller for Collateral screen
    /// </summary>
    public class CollateralController : BaseApiController
    {
        private readonly ICollateralRepository collateralRepository;

        public CollateralController(ICollateralRepository collateralRepository)
        {
            this.collateralRepository = collateralRepository;
        }

        /// <summary>
        /// To get collateral for patient
        /// </summary>
        /// <param name="contactID">Contact Id of patient</param>
        /// <param name="contactTypeID">Contact type Id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<CollateralModel>> GetCollaterals(int contactID, int contactTypeID, bool getContactDetails)
        {
            var result = await collateralRepository.GetCollateralsAsync(contactID, contactTypeID, getContactDetails);
            return result;
        }

        /// <summary>
        /// Add collateral for patient
        /// </summary>
        /// <param name="collateral">collateral ViewModel</param>
        /// <returns></returns>
        [HttpPost]
        public Response<CollateralViewModel> AddCollateral(CollateralViewModel collateral)
        {
            return collateralRepository.AddCollateral(collateral);
        }

        /// <summary>
        /// Update collateral for patient
        /// </summary>
        /// <param name="collateral">collateral ViewModel</param>
        /// <returns></returns>
        [HttpPut]
        public Response<CollateralViewModel> UpdateCollateral(CollateralViewModel collateral)
        {
            return collateralRepository.UpdateCollateral(collateral);
        }


        /// <summary>
        /// Deletes the collateral.
        /// </summary>
        /// <param name="parentContactID">The parent contact identifier.</param>
        /// <param name="contactID">The contact identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        [HttpDelete]
        public Response<CollateralViewModel> DeleteCollateral(long parentContactID, long contactID, DateTime modifiedOn)
        {
            modifiedOn = modifiedOn.ToUniversalTime();
            return collateralRepository.DeleteCollateral(parentContactID,contactID, modifiedOn);
        }
    }
}
