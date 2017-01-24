using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Axis.DataProvider.Common
{
    public class ProvidersDataProvider : IProvidersDataProvider
    {
        /// <summary>
        /// The unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProvidersDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ProvidersDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gets the providers.
        /// </summary>
        /// <param name="filterCriteria">The filter criteria.</param>
        /// <returns></returns>
        public Response<ProvidersModel> GetProviders(int filterCriteria)
        {
            var spParameters = new List<SqlParameter> { new SqlParameter("filterCriteria", filterCriteria) };
            var repository = _unitOfWork.GetRepository<ProvidersModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetProviders", spParameters);

            return results;
        }
        /// <summary>
        /// Gets the provider by id.
        /// </summary>
        /// <param name="providerID">The provider ID.</param>
        /// <returns></returns>
        public Response<ProvidersModel> GetProviderByid(int providerID)
        {
            var spParameters = new List<SqlParameter> { new SqlParameter("providerID", providerID) };
            var repository = _unitOfWork.GetRepository<ProvidersModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetProviderById", spParameters);
            return results;
        }
    }
}