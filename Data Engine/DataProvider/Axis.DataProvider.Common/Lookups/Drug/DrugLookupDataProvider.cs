using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public class DrugLookupDataProvider : IDrugLookupDataProvider
    {
        #region Class Variables

        readonly IUnitOfWork _unitOfWork;

        #endregion

        #region Constructors

        public DrugLookupDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Public Methods

        public Response<DrugModel> GetDrugs()
        {
            var repository = _unitOfWork.GetRepository<DrugModel>(SchemaName.Clinical);
            var results = repository.ExecuteStoredProc("usp_GetDrug");

            return results;
        }

        #endregion
    }
}
