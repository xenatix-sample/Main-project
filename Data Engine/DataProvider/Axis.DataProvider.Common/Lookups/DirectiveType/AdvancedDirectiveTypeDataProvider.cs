using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using System;
using System.Collections.Generic;

namespace Axis.DataProvider.Common
{
    public class AdvancedDirectiveTypeDataProvider : IAdvancedDirectiveTypeDataProvider
    {
        #region initializations

        readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="unitOfWork"></param>
        public AdvancedDirectiveTypeDataProvider(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Fetches the directive types
        /// </summary>
        /// <returns></returns>
        Response<AdvancedDirectiveTypeModel> IAdvancedDirectiveTypeDataProvider.GetDirectiveTypes()
        {
            var repository = unitOfWork.GetRepository<AdvancedDirectiveTypeModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetAdvancedDirectiveTypes");
            return  results;
        }

        #endregion exposed functionality


    }
}
