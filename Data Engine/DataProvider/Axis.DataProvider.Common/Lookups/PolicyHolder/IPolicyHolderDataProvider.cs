using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    /// <summary>
    ///
    /// </summary>
    public interface IPolicyHolderDataProvider
    {
        /// <summary>
        /// Gets the policy holders.
        /// </summary>
        /// <returns></returns>
        Response<PolicyHolderModel> GetPolicyHolders();
    }
}