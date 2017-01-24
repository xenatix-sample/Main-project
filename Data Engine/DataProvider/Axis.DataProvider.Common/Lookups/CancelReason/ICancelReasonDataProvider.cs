using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    /// <summary>
    /// Interface cancel reason
    /// </summary>
    public interface ICancelReasonDataProvider
    {
        Response<CancelReasonModel> GetCancelReasons();
    }
}
