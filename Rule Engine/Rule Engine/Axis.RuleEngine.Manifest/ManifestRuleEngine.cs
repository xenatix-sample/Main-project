using Axis.Model.Cache;
using Axis.Model.Common;
using Axis.Service.Manifest;

namespace Axis.RuleEngine.Manifest
{
    public class ManifestRuleEngine : IManifestRuleEngine
    {
        private IManifestService manifestService;

        public ManifestRuleEngine(IManifestService manifestService)
        {
            this.manifestService = manifestService;
        }

        public Response<ManifestModel> GetFilesToCache()
        {
            return manifestService.GetFilesToCache();
        }
    }
}
