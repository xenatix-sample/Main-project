using Axis.Model.BusinessAdmin;
using Axis.Model.Common;

namespace Axis.DataProvider.BusinessAdmin.ServiceDetails
{
    public interface IServiceDetailsDataProvider
    {


        /// <summary>
        /// Gets the service workflows.
        /// </summary>
        /// <param name="servicesID">The services identifier.</param>
        /// <returns></returns>
        Response<ServiceDetailsDataModel> GetServiceWorkflows(int servicesID);

        /// <summary>
        /// Saves the service details.
        /// </summary>
        /// <param name="serviceDetails">The service details.</param>
        /// <returns></returns>
        Response<ServiceDetailsModel> SaveServiceDetails(ServiceDetailsModel serviceDetails);

        /// <summary>
        /// Gets the service details.
        /// </summary>
        /// <param name="servicesID">The services identifier.</param>
        /// <param name="moduleComponentID">The module component identifier.</param>
        /// <returns></returns>
        Response<ServiceDetailsModel> GetServiceDetails(int servicesID, long moduleComponentID);

    }
}
