using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    /// <summary>
    ///
    /// </summary>
    public class PolicyHolderDataProvider : IPolicyHolderDataProvider
    {
        #region initializations

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="PolicyHolderDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public PolicyHolderDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Gets the policy holders.
        /// </summary>
        /// <returns></returns>
        public Response<PolicyHolderModel> GetPolicyHolders()
        {
            var repository = _unitOfWork.GetRepository<PolicyHolderModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetPolicyHolderDetails");

            return results;
        }

        #endregion exposed functionality
    }
}