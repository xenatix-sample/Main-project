using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    /// <summary>
    ///
    /// </summary>
    public interface IClientIdentifierTypeDataProvider
    {
        /// <summary>
        /// Gets the client identifier types.
        /// </summary>
        /// <returns></returns>
        Response<ClientIdentifierTypeModel> GetClientIdentifierTypes();
    }
}