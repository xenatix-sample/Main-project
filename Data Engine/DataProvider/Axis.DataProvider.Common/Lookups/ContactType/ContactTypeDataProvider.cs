using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ContactTypeDataProvider : IContactTypeDataProvider
    {
        #region initializations

        readonly IUnitOfWork _unitOfWork;

        public ContactTypeDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<ContactTypeModel> GetContactTypes()
        {
            var repository = _unitOfWork.GetRepository<ContactTypeModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetContactTypeDetails");

            return results;
        }

        #endregion exposed functionality

    }

}
