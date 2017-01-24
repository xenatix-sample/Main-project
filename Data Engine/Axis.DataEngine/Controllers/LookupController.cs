using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Axis.DataEngine.Helpers.Results;
using Axis.Model.Common;
using Axis.DataProvider.Common;

namespace Axis.DataEngine.Service.Controllers
{
    public class LookupController : ApiController
    {
        #region Class Variables

        ILookupDataProvider _lookupDataProvider = null;

        #endregion

        #region Constructors

        public LookupController(ILookupDataProvider lookupDataProvider)
        {
            _lookupDataProvider = lookupDataProvider;
        }

        #endregion

        #region Public Methods

        [HttpGet]

        public IHttpActionResult GetLookupsToCache()
        {
            var lookupTypes = LookupTypeHelper.GetAllLookupTypes();
            new LookupType[] { LookupType.Drug, LookupType.Reports }.ToList().ForEach(
                delegate(LookupType typeToRemove)
                {
                    if (lookupTypes.Contains(typeToRemove))
                        lookupTypes.Remove(typeToRemove);
                });
            return new HttpResult<Response<Dictionary<string, List<dynamic>>>>(_lookupDataProvider.GetLookups(lookupTypes), Request);
        }

        public IHttpActionResult GetReportsToCache()
        {
            return new HttpResult<Response<Dictionary<string, List<dynamic>>>>(_lookupDataProvider.GetLookups((new [] { LookupType.Reports }).ToList()), Request);
        }

        public IHttpActionResult GetDrugsToCache()
        {
            return new HttpResult<Response<Dictionary<string, List<dynamic>>>>(_lookupDataProvider.GetLookups((new [] { LookupType.Drug }).ToList()), Request);
        }

        public IHttpActionResult GetLookupsByType(LookupType lookupType)
        {
            return new HttpResult<Response<Dictionary<string, List<dynamic>>>>(_lookupDataProvider.GetLookups(new[] { lookupType }.ToList()), Request);
        }

        #endregion
    }
}