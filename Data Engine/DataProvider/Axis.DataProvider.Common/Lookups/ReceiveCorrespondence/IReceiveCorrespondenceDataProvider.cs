using Axis.Model.Common;

namespace Axis.DataProvider.Common
{

    /// <summary>
    /// interface for receive correspondence data provider
    /// </summary>
    public interface IReceiveCorrespondenceDataProvider
    {
        /// <summary>
        /// Gets the receive correspondence.
        /// </summary>
        /// <returns></returns>
        Response<ReceiveCorrespondenceModel> GetReceiveCorrespondence();
    }
}
