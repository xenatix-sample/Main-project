using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    /// <summary>
    /// Email permission data provider
    /// </summary>
    public class EmailPermissionDataProvider : IEmailPermissionDataProvider
    {
        #region initializations

        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailPermissionDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public EmailPermissionDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Gets the email permissions.
        /// </summary>
        /// <returns></returns>
        public Response<EmailPermissionModel> GetEmailPermissions()
        {
            var repository = _unitOfWork.GetRepository<EmailPermissionModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetEmailPermissionDetails");

            return results;
        }

        #endregion exposed functionality
    }
}