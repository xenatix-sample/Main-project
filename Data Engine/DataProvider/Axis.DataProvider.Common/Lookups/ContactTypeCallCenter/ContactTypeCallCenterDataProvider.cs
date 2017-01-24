using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public class ContactTypeCallCenterDataProvider : IContactTypeCallCenterDataProvider
    {
        #region initializations

        readonly IUnitOfWork _unitOfWork;

        public ContactTypeCallCenterDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<ContactTypeModel> GetContactTypes()
        {
            var repository = _unitOfWork.GetRepository<ContactTypeModel>(SchemaName.CallCenter);
            var results = repository.ExecuteStoredProc("usp_GetContactTypeHeirarchy");

            return results;
        }

        #endregion exposed functionality
    }
}
