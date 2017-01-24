using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface ICallTypeDataProvider
    {
        /// <summary>
        /// Gets the call status.
        /// </summary>
        /// <returns></returns>
        Response<CallTypeModel> GetCallType();
    }
}
