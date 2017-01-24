using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public class EligibilityTypeDataProvider : IEligibilityTypeDataProvider
    {
        #region Class Variables

        private readonly IUnitOfWork _unitOfWork;

        #endregion

        #region Constructors

        public EligibilityTypeDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the list of eligibility types.
        /// </summary>
        /// <returns></returns>
        public Response<EligibilityTypeModel> GetEligibilityTypes()
        {
            var repository = _unitOfWork.GetRepository<EligibilityTypeModel>(SchemaName.ECI);
            var data = repository.ExecuteStoredProc("usp_GetEligibilityType");

            return data;
        }

        #endregion
    }
}
