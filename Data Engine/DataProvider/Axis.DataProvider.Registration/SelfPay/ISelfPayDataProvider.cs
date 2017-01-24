using Axis.Model.Common;
using Axis.Model.Registration;
using System;

namespace Axis.DataProvider.Registration
{
    public interface ISelfPayDataProvider
    {
        /// <summary>
        /// Gets the self pay list.
        /// </summary>
        /// <param name="selfPayID">The self pay identifier.</param>
        /// <returns></returns>
        Response<SelfPayModel> GetSelfPayDetails(long selfPayID);

        /// <summary>
        /// Adds the self pay
        /// </summary>
        /// <param name="selfPay">The self pay.</param>
        /// <returns></returns>
        Response<SelfPayModel> AddSelfPay(SelfPayModel selfPay);
                
        /// <summary>
        /// Adds the self pay header
        /// </summary>
        /// <param name="selfPay">The self pay.</param>
        /// <returns></returns>
        Response<SelfPayModel> AddSelfPayHeader(SelfPayModel selfPay);

        /// <summary>
        /// Updates the self pay.
        /// </summary>
        /// <param name="selfPay">The self pay.</param>
        /// <returns></returns>
        Response<SelfPayModel> UpdateSelfPay(SelfPayModel selfPay);

        /// <summary>
        /// delete the self pay.
        /// </summary>
        /// <param name="contactDischargeNoteID">The self pay header identifier.</param>
        /// <param name="modifiedOn">The modified on</param>
        /// <returns></returns>
        Response<SelfPayModel> DeleteSelfPay(long selfPayHeaderID, DateTime modifiedOn);
    }
}
