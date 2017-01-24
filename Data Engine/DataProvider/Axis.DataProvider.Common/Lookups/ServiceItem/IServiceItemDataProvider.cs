using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IServiceItemDataProvider
    {
        /// <summary>
        /// Gets the Service Items.
        /// </summary>
        /// <returns></returns>
        Response<ServiceItemModel> GetServiceItems();
    }
}
