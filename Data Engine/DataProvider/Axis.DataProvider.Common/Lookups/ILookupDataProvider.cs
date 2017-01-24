using System.Collections.Generic;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface ILookupDataProvider
    {
        Response<Dictionary<string, List<dynamic>>> GetLookups(List<LookupType> lookupTypes);
    }
}