using System.Collections.Generic;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface ICountyDataProvider
    {
        Response<CountyModel> GetCounty();

        Response<CountyModel> GetOrganizationCounty();
    }
}
