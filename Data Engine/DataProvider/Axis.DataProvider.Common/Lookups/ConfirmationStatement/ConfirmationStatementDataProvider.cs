using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ConfirmationStatementDataProvider : IConfirmationStatementDataProvider
    {
        #region initializations

        private readonly IUnitOfWork _unitOfWork;

        public ConfirmationStatementDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Gets the confirmation statement.
        /// </summary>
        /// <returns></returns>
        public Response<ConfirmationStatementModel> GetConfirmationStatement()
        {
            var repository = _unitOfWork.GetRepository<ConfirmationStatementModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetConfirmationStatement");

            return results;
        }

        /// <summary>
        /// Gets the confirmation statement group.
        /// </summary>
        /// <returns></returns>
        public Response<ConfirmationStatementGroupModel> GetConfirmationStatementGroup()
        {
            var repository = _unitOfWork.GetRepository<ConfirmationStatementGroupModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetConfirmationStatementGroup");

            return results;
        }

        #endregion exposed functionality
    }
}