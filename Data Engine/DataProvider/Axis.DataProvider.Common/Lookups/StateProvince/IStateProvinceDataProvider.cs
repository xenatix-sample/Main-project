using System.Collections.Generic;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IStateProvinceDataProvider
    {
        Response<StateProvinceModel> GetStateProvince();
    }
}
