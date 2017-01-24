using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Plugins.Registration.Models;
using System;

namespace Axis.Plugins.Registration.Repository
{
    public interface ISelfPayRepository
    {
        /// <summary>
        /// Gets the self pay list.
        /// </summary>
        /// <param name="selfPayID">The self pay identifier.</param>
        /// <returns></returns>
        Response<SelfPayViewModel> GetSelfPayDetails(long selfPayID);

        /// <summary>
        /// Adds the self pay
        /// </summary>
        /// <param name="selfPay">The self pay.</param>
        /// <returns></returns>
        Response<SelfPayViewModel> AddSelfPay(SelfPayViewModel selfPay);

        /// <summary>
        /// Adds the self pay header
        /// </summary>
        /// <param name="selfPay">The self pay.</param>
        /// <returns></returns>
        Response<SelfPayViewModel> AddSelfPayHeader(SelfPayViewModel selfPay);

        /// <summary>
        /// Updates the self pay.
        /// </summary>
        /// <param name="selfPay">The self pay.</param>
        /// <returns></returns>
        Response<SelfPayViewModel> UpdateSelfPay(SelfPayViewModel selfPay);

        /// <summary>
        /// delete the self pay.
        /// </summary>
        /// <param name="selfPayID">The self pay identifier.</param>
        /// <param name="modifiedOn">The modified on</param>
        /// <returns></returns>
        Response<SelfPayViewModel> DeleteSelfPay(long selfPayID, DateTime modifiedOn);
    }
}
