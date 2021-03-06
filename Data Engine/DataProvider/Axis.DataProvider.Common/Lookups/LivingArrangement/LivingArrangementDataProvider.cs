﻿using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class LivingArrangementDataProvider : ILivingArrangementDataProvider
    {
        #region initializations

        readonly IUnitOfWork _unitOfWork;

        public LivingArrangementDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<LivingArrangementModel> GetLivingArrangements()
        {
            var repository = _unitOfWork.GetRepository<LivingArrangementModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetLivingArrangementDetails");

            return results;
        }

        #endregion exposed functionality

    }

}
