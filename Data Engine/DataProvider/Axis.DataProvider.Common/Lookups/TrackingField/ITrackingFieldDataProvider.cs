using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface ITrackingFieldDataProvider
    {
        /// <summary>
        /// Gets the tracking fields.
        /// </summary>
        /// <returns></returns>
        Response<TrackingFieldModel> GetTrackingFields();


        /// <summary>
        /// Gets the configured tracking fields.
        /// </summary>
        /// <returns></returns>
        Response<TrackingFieldModel> GetTrackingFieldsConfigured();

    }
}
