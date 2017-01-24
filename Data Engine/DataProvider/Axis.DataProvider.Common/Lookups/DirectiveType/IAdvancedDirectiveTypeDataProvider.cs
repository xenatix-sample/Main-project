using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    /// <summary>
    /// interface for advanced directive type
    /// </summary>
    public interface IAdvancedDirectiveTypeDataProvider
    {
        Response<AdvancedDirectiveTypeModel> GetDirectiveTypes();
    }
}
