using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public class EligibilityCategoryDataProvider : IEligibilityCategoryDataProvider
    {
         #region Class Variables

        private readonly IUnitOfWork _unitOfWork;

        #endregion

        #region Constructors

        public EligibilityCategoryDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the list of eligibility categories.
        /// </summary>
        /// <returns></returns>
        public Response<EligibilityCategoryModel> GetEligibilityCategories()
        {
            var repository = _unitOfWork.GetRepository<EligibilityCategoryModel>(SchemaName.ECI);
            var data = repository.ExecuteStoredProc("usp_GetEligibilityCategory");

            return data;
        }

        #endregion
    }
}
