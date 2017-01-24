using Axis.Model.Common;

namespace Axis.DataProvider.Common
{

    public interface IReferralOrganizationDataProvider
    {
        /// <summary>
        /// Gets the Organizations.
        /// </summary>
        /// <returns></returns>
        Response<ReferralOrganizationModel> GetOrganizations();
    }
}