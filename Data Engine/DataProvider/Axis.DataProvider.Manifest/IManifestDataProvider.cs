using Axis.Model.Cache;
using Axis.Model.Common;
using System.Collections.Generic;

namespace Axis.DataProvider.Manifest
{
    public interface IManifestDataProvider
    {
        Response<ManifestModel> GetFilesToCache();
    }
}
