using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ContactMethodDataProvider : IContactMethodDataProvider
    {
        #region initializations

        readonly IUnitOfWork _unitOfWork;

        public ContactMethodDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<ContactMethodModel> GetContactMethods()
        {
            var repository = _unitOfWork.GetRepository<ContactMethodModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetContactMethodDetails");

            return results;
        }

        #endregion exposed functionality

    }

}
