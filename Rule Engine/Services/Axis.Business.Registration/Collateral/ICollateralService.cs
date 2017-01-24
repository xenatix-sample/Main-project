using System;
using Axis.Model.Common;
using Axis.Model.Registration;

namespace Axis.Service.Registration
{
    /// <summary>
    /// Interface of Collateral Service class to call the web api methods
    /// </summary>
    public interface ICollateralService
    {
        /// <summary>
        /// Gets the collaterals.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <param name="contactTypeId">The contact type identifier.</param>
        /// <param name="getContactDetails">if set to <c>true</c> [get contact details].</param>
        /// <returns></returns>
        Response<CollateralModel> GetCollaterals(long contactID, int contactTypeId, bool getContactDetails);
        /// <summary>
        /// Adds the collateral.
        /// </summary>
        /// <param name="collateralModel">The collateral model.</param>
        /// <returns></returns>
        Response<CollateralModel> AddCollateral(CollateralModel collateralModel);
        /// <summary>
        /// Updates the collateral.
        /// </summary>
        /// <param name="collateralModel">The collateral model.</param>
        /// <returns></returns>
        Response<CollateralModel> UpdateCollateral(CollateralModel collateralModel);
        /// <summary>
        /// Deletes the collateral.
        /// </summary>
        /// <param name="parentContactID">The parent contact identifier.</param>
        /// <param name="contactID">The contact identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        Response<CollateralModel> DeleteCollateral(long parentContactID, long contactID, DateTime modifiedOn);
    }
}
