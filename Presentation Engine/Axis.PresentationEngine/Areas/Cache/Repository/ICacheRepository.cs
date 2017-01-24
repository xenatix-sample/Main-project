using Axis.Model.Cache;
using Axis.Model.Common;

namespace Axis.PresentationEngine.Areas.Cache.Repository
{
    public interface ICacheRepository
    {
        Response<ManifestModel> GetFilesToCache();
    }
}
