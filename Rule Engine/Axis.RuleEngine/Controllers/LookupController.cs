using Axis.Model.Common;
using Axis.RuleEngine.Helpers.Results;
using Axis.RuleEngine.Lookup;
using System.Collections.Generic;
using System.Web.Http;

namespace Axis.RuleEngine.Service.Controllers
{
    public class LookupController : ApiController
    {
        #region Class Variables

        private ILookupRuleEngine _lookupRuleEngine = null;

        #endregion

        #region Constructors

        public LookupController(ILookupRuleEngine lookupRuleEngine)
        {
            _lookupRuleEngine = lookupRuleEngine;
        }

        #endregion

        #region Public Methods

        public IHttpActionResult GetLookupsToCache()
        {
            Response<Dictionary<string, List<dynamic>>> data = _lookupRuleEngine.GetLookupsToCache();

            return new HttpResult<Response<Dictionary<string, List<dynamic>>>>(data, Request);
        }

        public IHttpActionResult GetReportsToCache()
        {
            Response<Dictionary<string, List<dynamic>>> data = _lookupRuleEngine.GetReportsToCache();

            return new HttpResult<Response<Dictionary<string, List<dynamic>>>>(data, Request);
        }

        public IHttpActionResult GetDrugsToCache()
        {
            Response<Dictionary<string, List<dynamic>>> data = _lookupRuleEngine.GetDrugsToCache();

            return new HttpResult<Response<Dictionary<string, List<dynamic>>>>(data, Request);
        }

        public IHttpActionResult GetLookupsByType(LookupType lookupType)
        {
            Response<Dictionary<string, List<dynamic>>> data = _lookupRuleEngine.GetLookupsByType(lookupType);

            return new HttpResult<Response<Dictionary<string, List<dynamic>>>>(data, Request);
        }

        #endregion
    }
}