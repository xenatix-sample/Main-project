using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.BusinessAdmin;
using Axis.Model.Common;

namespace Axis.DataProvider.BusinessAdmin.HealthRecords
{
    public class HealthRecordsDataProvider : IHealthRecordsDataProvider
    {
        #region Class Variables        
        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        #endregion Class 

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="HealthRecordsDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public HealthRecordsDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Public Methods        
        /// <summary>
        /// Gets the health records.
        /// </summary>
        /// <returns></returns>
        public Response<HealthRecordsModel> GetHealthRecords()
        {
            var repository = _unitOfWork.GetRepository<HealthRecordsModel>(SchemaName.Core);
            var results = repository.ExecuteStoredProc("");//TODO: inculde sp name 
            return results;
        }
        #endregion
    }
}
