using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    /// <summary>
    /// Mail permission data provider
    /// </summary>
    public class MailPermissionTypeDataProvider : IMailPermissionTypeDataProvider
    {
        #region initializations

        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="MailPermissionTypeDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public MailPermissionTypeDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Gets the type of the mail permission.
        /// </summary>
        /// <returns></returns>
        public Response<MailPermissionModel> GetMailPermissionType()
        {
            var repository = _unitOfWork.GetRepository<MailPermissionModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetMailPermissionDetails");

            return results;
        }

        #endregion exposed functionality
    }
}