using Axis.Data.Repository;
using Axis.Model.Cache;
using Axis.Model.Common;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Axis.DataProvider.Manifest
{
    public class ManifestDataProvider : IManifestDataProvider
    {
        IUnitOfWork unitOfWork = null;

        public ManifestDataProvider(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public Response<ManifestModel> GetFilesToCache()
        {
            var manifestRepository = unitOfWork.GetRepository<ManifestModel>();
            var result = manifestRepository.ExecuteStoredProc("usp_GetFilesToCache");
            return result;
        }
    }
}
