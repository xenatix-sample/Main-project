using System.Collections.Generic;
using Axis.Model.Common;

namespace Axis.Service.Lookup
{
    public interface ILookupService
    {
        Response<Dictionary<string, List<dynamic>>> GetLookupsToCache();
        Response<Dictionary<string, List<dynamic>>> GetReportsToCache();
        Response<Dictionary<string, List<dynamic>>> GetDrugsToCache();
        Response<Dictionary<string, List<dynamic>>> GetLookupsByType(LookupType lookupType);
    }
}
