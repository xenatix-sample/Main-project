using System;
using System.Collections.Generic;
using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public class DurationDataProvider : IDurationDataProvider
    {
         #region Class Variables

        private readonly IUnitOfWork _unitOfWork;

        #endregion

        #region Constructors

        public DurationDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the list of durations in increments of 15 minutes.
        /// </summary>
        /// <returns></returns>
        public Response<DurationModel> GetQuarterDurations()
        {
            var repository = _unitOfWork.GetRepository<DurationModel>(SchemaName.ECI);
            var data = repository.ExecuteStoredProc("usp_GetEligibilityDuration");

            return data;
        }

        #endregion
    }
}
