using Axis.Model.Common;

namespace Axis.DataProvider.Common
{

    public interface IReferralCategorySourceDataProvider
    {
        /// <summary>
        /// Gets the referral category.
        /// </summary>
        /// <returns></returns>
        Response<ReferralCategorySourceModel> GetReferralCategorySource();
    }
}