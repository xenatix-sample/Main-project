using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using System;
using System.Collections.Generic;

namespace Axis.DataProvider.Common
{
    public class LivingStatusDataProvider : ILivingStatusDataProvider
    {
        #region initializations

        readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="unitOfWork"></param>
        public LivingStatusDataProvider(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Fetches the information for living status
        /// </summary>
        /// <returns></returns>
        public Response<LivingStatusModel> GetLivingStatus()
        {
            var repository = unitOfWork.GetRepository<LivingStatusModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetLivingWithClientStatusDetails");

            return results;
        }

        #endregion exposed functionality
    }
}
