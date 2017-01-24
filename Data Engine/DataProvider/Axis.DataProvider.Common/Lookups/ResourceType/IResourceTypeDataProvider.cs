using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    /// <summary>
    /// 
    /// </summary>
    public interface IResourceTypeDataProvider
    {
        /// <summary>
        /// Gets the type of the resource.
        /// </summary>
        /// <returns></returns>
        Response<ResourceTypeModel> GetResourceTypeDetails();
    }
}