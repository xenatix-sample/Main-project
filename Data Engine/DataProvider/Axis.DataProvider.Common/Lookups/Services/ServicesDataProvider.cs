using Axis.Model.Common;
using Axis.Model.Common.Lookups.Services;
using Axis.Data.Repository.Schema;
using Axis.Data.Repository;

namespace Axis.DataProvider.Common.Lookups.Services
{
    public class ServicesDataProvider : IServicesDataProvider
    {
        #region initializations

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServicesDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ServicesDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Gets the Services.
        /// </summary>
        /// <returns></returns>
        public Response<ServicesModel> GetServicesModuleComponents()
        {
            var repository = _unitOfWork.GetRepository<ServicesModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("Usp_GetServicesModuleComponentDetails");

            return results;
        }

        #endregion exposed functionality

    }
}
