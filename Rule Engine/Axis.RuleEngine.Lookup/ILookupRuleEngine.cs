using System.Collections.Generic;
using Axis.Model.Common;

namespace Axis.RuleEngine.Lookup
{
    public interface ILookupRuleEngine
    {
        Response<Dictionary<string, List<dynamic>>> GetLookupsToCache();
        Response<Dictionary<string, List<dynamic>>> GetReportsToCache();
        Response<Dictionary<string, List<dynamic>>> GetDrugsToCache();
        Response<Dictionary<string, List<dynamic>>> GetLookupsByType(LookupType lookupType);
    }
}
