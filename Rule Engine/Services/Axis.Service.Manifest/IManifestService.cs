using Axis.Model.Cache;
using Axis.Model.Common;

namespace Axis.Service.Manifest
{
    public interface IManifestService
    {
        Response<ManifestModel> GetFilesToCache();
    }
}
