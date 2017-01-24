using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public class AllergyLookupDataProvider : IAllergyLookupDataProvider
    {
        #region Class Variables

        readonly IUnitOfWork _unitOfWork;

        #endregion

        #region Constructors

        public AllergyLookupDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Public Methods

        public Response<AllergyModel> GetAllergies()
        {
            var repository = _unitOfWork.GetRepository<AllergyModel>(SchemaName.Clinical);
            var results = repository.ExecuteStoredProc("usp_GetAllergy");

            return results;
        }

        public Response<AllergySymptomModel> GetAllergySymptoms()
        {
            var repository = _unitOfWork.GetRepository<AllergySymptomModel>(SchemaName.Clinical);
            var results = repository.ExecuteStoredProc("usp_GetAllergySymptoms");

            return results;
        }

        public Response<AllergyTypeModel> GetAllergyTypes()
        {
            var repository = _unitOfWork.GetRepository<AllergyTypeModel>(SchemaName.Clinical);
            var results = repository.ExecuteStoredProc("usp_GetAllergyTypes");

            return results;
        }

        public Response<AllergySeverityModel> GetAllergySeverities()
        {
            var repository = _unitOfWork.GetRepository<AllergySeverityModel>(SchemaName.Clinical);
            var results = repository.ExecuteStoredProc("usp_GetAllergySeverity");

            return results;
        }

        #endregion
    }
}
