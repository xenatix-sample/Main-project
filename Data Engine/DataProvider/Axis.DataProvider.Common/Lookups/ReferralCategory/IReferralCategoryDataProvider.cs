using Axis.Model.Common;

namespace Axis.DataProvider.Common
{

    public interface IReferralCategoryDataProvider
    {
        /// <summary>
        /// Gets the referral category.
        /// </summary>
        /// <returns></returns>
        Response<ReferralCategoryModel> GetReferralCategory();
    }
}