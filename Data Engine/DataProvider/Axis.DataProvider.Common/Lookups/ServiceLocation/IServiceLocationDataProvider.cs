using Axis.Model.Common;

namespace Axis.DataProvider.Common
{

    public interface IServiceLocationDataProvider
    {

        /// <summary>
        /// Gets the service location.
        /// </summary>
        /// <returns></returns>
        Response<ServiceLocationModel> GetServiceLocation();


        /// <summary>
        /// Gets the service location.
        /// </summary>
        /// <returns></returns>
        Response<ServiceLocationModel> GetServiceLocationModuleComponents();
    }
}
