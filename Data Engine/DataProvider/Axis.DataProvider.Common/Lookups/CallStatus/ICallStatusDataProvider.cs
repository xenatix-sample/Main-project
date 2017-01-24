using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface ICallStatusDataProvider
    {
        /// <summary>
        /// Gets the call status.
        /// </summary>
        /// <returns></returns>
        Response<CallStatusModel> GetCallStatus();
    }
}
