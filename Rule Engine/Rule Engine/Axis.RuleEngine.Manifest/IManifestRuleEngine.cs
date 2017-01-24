using Axis.Model.Cache;
using Axis.Model.Common;

namespace Axis.RuleEngine.Manifest
{
    public interface IManifestRuleEngine
    {
        Response<ManifestModel> GetFilesToCache();
    }
}
