using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Service.Registration;
using System;

namespace Axis.RuleEngine.Registration
{
    /// <summary>
    /// Rule engine class for calling service method of Collateral Service
    /// </summary>
    /// <seealso cref="Axis.RuleEngine.Registration.ICollateralRuleEngine" />
    public class CollateralRuleEngine : ICollateralRuleEngine
    {
        private readonly ICollateralService collateralService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="collateralService"></param>
        public CollateralRuleEngine(ICollateralService collateralService)
        {
            this.collateralService = collateralService;
        }

        /// <summary>
        /// To get list of collaterals for contact
        /// </summary>
        /// <param name="contactID"></param>
        /// <param name="contactTypeId"></param>
        /// <returns></returns>
        public Response<CollateralModel> GetCollaterals(long contactID, int contactTypeId, bool getContactDetails)
        {
            return collateralService.GetCollaterals(contactID, contactTypeId, getContactDetails);
        }

        /// <summary>
        /// To add new collateral for contact
        /// </summary>
        /// <param name="collateralModel"></param>
        /// <returns></returns>
        public Response<CollateralModel> AddCollateral(CollateralModel collateralModel)
        {
            return collateralService.AddCollateral(collateralModel);
        }

        /// <summary>
        /// To update collateral for contact
        /// </summary>
        /// <param name="collateralModel"></param>
        /// <returns></returns>
        public Response<CollateralModel> UpdateCollateral(CollateralModel collateralModel)
        {
            return collateralService.UpdateCollateral(collateralModel);
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
            return collateralService.DeleteCollateral(parentContactID, contactID, modifiedOn);
        }
    }
}
