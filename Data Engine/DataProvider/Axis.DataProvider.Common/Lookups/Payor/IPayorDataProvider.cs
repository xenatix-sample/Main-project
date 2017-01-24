using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    /// <summary>
    ///
    /// </summary>
    public interface IPayorDataProvider
    {
        /// <summary>
        /// Gets the payors.
        /// </summary>
        /// <returns></returns>
        Response<PayorModel> GetPayors();
    }
}