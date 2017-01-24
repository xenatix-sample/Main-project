using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    /// <summary>
    ///
    /// </summary>
    public interface IPayorExpirationReasonDataProvider
    {
        /// <summary>
        /// Gets the payor expiration reason.
        /// </summary>
        /// <returns></returns>
        Response<PayorExpirationReasonModel> GetPayorExpirationReason();
    }
}