using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public class SuicidalHomicidalDataProvider : ISuicidalHomicidalDataProvider
    {   
        readonly IUnitOfWork _unitOfWork;
        public SuicidalHomicidalDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Response<SuicidalHomicidalModel> GetSuicidalHomicidal()
        {
            var repository = _unitOfWork.GetRepository<SuicidalHomicidalModel>(SchemaName.CallCenter);
            return repository.ExecuteStoredProc("usp_GetSuicideHomicide");
        }
    }
}
