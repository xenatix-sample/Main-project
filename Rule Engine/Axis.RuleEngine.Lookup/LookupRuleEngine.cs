using System.Collections.Generic;
using Axis.Model.Common;
using Axis.Service.Lookup;

namespace Axis.RuleEngine.Lookup
{
    public class LookupRuleEngine : ILookupRuleEngine
    {
        #region Class Variables

        private ILookupService _lookupService;

        #endregion

        #region Constructors

        public LookupRuleEngine(ILookupService lookupService)
        {
            _lookupService = lookupService;
        }

        #endregion

        #region Public Methods

        public Response<Dictionary<string, List<dynamic>>> GetLookupsToCache()
        {
            return _lookupService.GetLookupsToCache();
        }

        public Response<Dictionary<string, List<dynamic>>> GetReportsToCache()
        {
            return _lookupService.GetReportsToCache();
        }
        
        public Response<Dictionary<string, List<dynamic>>> GetDrugsToCache()
        {
            return _lookupService.GetDrugsToCache();
        }

        public Response<Dictionary<string, List<dynamic>>> GetLookupsByType(LookupType lookupType)
        {
            return _lookupService.GetLookupsByType(lookupType);
        }

        #endregion
    }
}
