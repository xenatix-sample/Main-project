using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public class DocumentTypeGroupDataProvider : IDocumentTypeGroupDataProvider
    {
        #region initializations

        readonly IUnitOfWork _unitOfWork;

        public DocumentTypeGroupDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<DocumentTypeGroupModel> GetDocumentTypeGroup()
        {
            var repository = _unitOfWork.GetRepository<DocumentTypeGroupModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetDocumentTypeGroup");

            return results;
        }

        #endregion exposed functionality
       
    }
}
