using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public class CCPriorityDataProvider : ICCPriorityDataProvider
    {
         /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CCPriorityDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public CCPriorityDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        /// <summary>
        /// Gets the categories.
        /// </summary>
        /// <returns></returns>
        public Response<CCPriorityModel> GetCCPriorities()
        {
            var repository = _unitOfWork.GetRepository<CCPriorityModel>(SchemaName.CallCenter);
            var results = repository.ExecuteStoredProc("usp_GetCallCenterPriorityDetails");

            return results;
        }
    }
}
