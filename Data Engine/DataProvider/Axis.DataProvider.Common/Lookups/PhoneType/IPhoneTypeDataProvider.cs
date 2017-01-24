using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    /// <summary>
    /// Phone type data provider
    /// </summary>
    public interface IPhoneTypeDataProvider
    {
        /// <summary>
        /// Gets the type of the phone.
        /// </summary>
        /// <returns></returns>
        Response<PhoneTypeModel> GetPhoneType();
    }
}