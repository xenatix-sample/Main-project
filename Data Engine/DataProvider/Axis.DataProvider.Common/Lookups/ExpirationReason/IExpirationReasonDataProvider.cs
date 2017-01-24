using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IExpirationReasonDataProvider
    {
        /// <summary>
        /// Gets the expiration reasons.
        /// </summary>
        /// <returns></returns>
        Response<ExpirationReasonModel> GetExpirationReasons();

        /// <summary>
        /// Gets the assessment expiration reasons.
        /// </summary>
        /// <returns></returns>
        Response<ExpirationReasonModel> GetAssessmentExpirationReasons();

        /// <summary>
        /// Gets the Other ID expiration reasons.
        /// </summary>
        /// <returns></returns>
        Response<ExpirationReasonModel> GetOtherIDExpirationReasons();

    }
}