using Axis.Model.Common;
using System.Collections.Generic;

namespace Axis.PresentationEngine.Helpers.Repositories
{
    public interface ILookupRepository
    {
        Response<Dictionary<string, List<dynamic>>> GetLookupsToCache();
        Response<Dictionary<string, List<dynamic>>> GetReportsToCache();
        Response<Dictionary<string, List<dynamic>>> GetDrugsToCache();
        Response<Dictionary<string, List<dynamic>>> GetLookupsByType(LookupType lookupType);
    }
}