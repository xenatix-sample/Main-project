using System.Collections.Generic;
using System.Data.SqlClient;
using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using Axis.Model.Common.Lookups.MedicalCondition;

namespace Axis.DataProvider.Common
{
    public class MedicalConditionLookupDataProvider : IMedicalConditionLookupDataProvider
    {
        #region Class Variables

        readonly IUnitOfWork _unitOfWork;

        #endregion

        #region Constructors

        public MedicalConditionLookupDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Public Methods

        public Response<MedicalConditionModel> GetMedicalConditions()
        {
            var repository = _unitOfWork.GetRepository<MedicalConditionModel>(SchemaName.Clinical);
            SqlParameter isSystemParam = new SqlParameter("IsSystem", true);
            List<SqlParameter> procParams = new List<SqlParameter>() { isSystemParam };
            var results = repository.ExecuteStoredProc("usp_GetMedicalConditions", procParams);

            return results;
        }

        #endregion
    }
}
