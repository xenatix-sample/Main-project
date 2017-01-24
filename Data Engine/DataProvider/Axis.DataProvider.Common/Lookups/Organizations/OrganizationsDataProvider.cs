using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using System;
using System.Collections.Generic;

namespace Axis.DataProvider.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Axis.DataProvider.Common.IOrganizationsDataProvider" />
    public class OrganizationsDataProvider : IOrganizationsDataProvider
    {
        #region initializations

        readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="unitOfWork"></param>
        public OrganizationsDataProvider(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Fetches the organization
        /// </summary>
        /// <returns></returns>
        Response<OrganizationsModel> IOrganizationsDataProvider.GetOrganizations()
        {
            var repository = unitOfWork.GetRepository<OrganizationsModel>(SchemaName.Core);
            var results = repository.ExecuteStoredProc("usp_GetOrganizationLookup");
            return results;
        }

        Response<OrganizationsModel> IOrganizationsDataProvider.GetOrganizationDetails()
        {
            var repository = unitOfWork.GetRepository<OrganizationsModel>(SchemaName.Core);
            var results = repository.ExecuteStoredProc("usp_GetOrganizationDetailsLookup");
            return results;
        }
        
        #endregion exposed functionality

    }
}
