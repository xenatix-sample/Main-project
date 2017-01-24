using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    /// <summary>
    /// Phone type data provider
    /// </summary>
    public class PhoneTypeDataProvider : IPhoneTypeDataProvider
    {
        #region Initializations

        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="PhoneTypeDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public PhoneTypeDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion Initializations

        #region Exposed Functionality

        /// <summary>
        /// Gets the type of the phone.
        /// </summary>
        /// <returns></returns>
        public Response<PhoneTypeModel> GetPhoneType()
        {
            var repository = _unitOfWork.GetRepository<PhoneTypeModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetPhoneTypeDetails");

            return results;
        }

        #endregion Exposed Functionality
    }
}