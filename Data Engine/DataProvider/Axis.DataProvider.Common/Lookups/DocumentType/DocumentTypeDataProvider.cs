using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public class DocumentTypeDataProvider : IDocumentTypeDataProvider
    {
        #region initializations

        readonly IUnitOfWork _unitOfWork;

        public DocumentTypeDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<DocumentTypeModel> GetDocumentType()
        {
            var repository = _unitOfWork.GetRepository<DocumentTypeModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetDocumentType");

            return results;
        }

        #endregion exposed functionality

        
    }
}
