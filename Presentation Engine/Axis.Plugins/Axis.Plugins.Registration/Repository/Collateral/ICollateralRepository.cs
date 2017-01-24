using System;
using System.Threading.Tasks;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Model.Registration.Model;

namespace Axis.Plugins.Registration
{
    public interface ICollateralRepository
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
        /// Gets the collaterals asynchronous.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <param name="contactTypeId">The contact type identifier.</param>
        /// <param name="getContactDetails">if set to <c>true</c> [get contact details].</param>
        /// <returns></returns>
        Task<Response<CollateralModel>> GetCollateralsAsync(long contactID, int contactTypeId, bool getContactDetails);
        /// <summary>
        /// Adds the collateral.
        /// </summary>
        /// <param name="collateral">The collateral.</param>
        /// <returns></returns>
        Response<CollateralViewModel> AddCollateral(CollateralViewModel collateral);
        /// <summary>
        /// Updates the collateral.
        /// </summary>
        /// <param name="collateral">The collateral.</param>
        /// <returns></returns>
        Response<CollateralViewModel> UpdateCollateral(CollateralViewModel collateral);
        /// <summary>
        /// Deletes the collateral.
        /// </summary>
        /// <param name="parentContactID">The parent contact identifier.</param>
        /// <param name="contactID">The contact identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        Response<CollateralViewModel> DeleteCollateral(long parentContactID, long contactID, DateTime modifiedOn);
    }
}
