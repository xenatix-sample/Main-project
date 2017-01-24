using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    /// <summary>
    ///
    /// </summary>
    public interface IPrefixTypeDataProvider
    {
        /// <summary>
        /// Gets the type of the prefix.
        /// </summary>
        /// <returns></returns>
        Response<TitleModel> GetPrefixType();
    }
}