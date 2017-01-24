using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class CitizenshipDataProvider : ICitizenshipDataProvider
    {
        #region initializations

        readonly IUnitOfWork _unitOfWork;

        public CitizenshipDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<CitizenshipModel> GetCitizenships()
        {
            var repository = _unitOfWork.GetRepository<CitizenshipModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetCitizenshipDetails");

            return results;
        }

        #endregion exposed functionality

    }

}
