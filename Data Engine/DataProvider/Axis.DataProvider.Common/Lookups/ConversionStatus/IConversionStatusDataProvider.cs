using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IConversionStatusDataProvider
    {
        /// <summary>
        /// Gets the Conversion Statuses.
        /// </summary>
        /// <returns></returns>
        Response<ConversionStatusModel> GetConversionStatuses();
    }
}
