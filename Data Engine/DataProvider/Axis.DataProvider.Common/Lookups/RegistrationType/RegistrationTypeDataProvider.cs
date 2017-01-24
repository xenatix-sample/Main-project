using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class RegistrationTypeDataProvider : IRegistrationTypeDataProvider
    {
        #region initializations

        readonly IUnitOfWork _unitOfWork;

        public RegistrationTypeDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<RegistrationTypeModel> GetRegistrationType()
        {
            var repository = _unitOfWork.GetRepository<RegistrationTypeModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetRegistrationTypeDetails");

            return results;
        }

        #endregion exposed functionality

    }

}
