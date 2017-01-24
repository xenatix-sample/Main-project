using Axis.Model.Cache;
using Axis.Model.Common;
using Axis.RuleEngine.Manifest;
using Axis.RuleEngine.Helpers.Results;
using System.IO;
using System.Web.Http;
using System.Xml;
using System.Collections.Generic;
using Axis.Security;
using System.Linq;
using Axis.RuleEngine.Helpers.Controllers;

namespace Axis.RuleEngine.Service.Controllers
{

    public class ManifestController : BaseApiController
    {
        IManifestRuleEngine manifestRuleEngine = null;

        public ManifestController(IManifestRuleEngine manifestRuleEngine)
        {
            this.manifestRuleEngine = manifestRuleEngine;
        }

        public IHttpActionResult GetFilesToCache()
        {
            
            return new HttpResult<Response<ManifestModel>>(manifestRuleEngine.GetFilesToCache(), Request);
        }
    }
}