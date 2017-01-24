using Axis.Model.Common;

namespace Axis.DataProvider.Common
{

    public interface IServiceWorkflowTypeDataProvider
    {
        /// <summary>
        /// Gets the service workflow types.
        /// </summary>
        /// <returns></returns>
        Response<ServiceWorkflowTypeModel> GetServiceWorkflowTypes();
    }
}