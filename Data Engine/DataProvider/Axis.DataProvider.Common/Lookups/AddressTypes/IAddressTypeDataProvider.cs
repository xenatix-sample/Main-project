using System.Collections.Generic;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IAddressTypeDataProvider
    {
        Response<AddressTypeModel> GetAddressTypes();
    }
}