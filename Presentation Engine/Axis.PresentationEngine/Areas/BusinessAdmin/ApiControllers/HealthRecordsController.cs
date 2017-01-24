using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.PresentationEngine.Areas.BusinessAdmin.Respository.HealthRecords;
using Axis.PresentationEngine.Helpers.Controllers;

namespace Axis.PresentationEngine.Areas.BusinessAdmin.ApiControllers
{
    public class HealthRecordsController : BaseApiController
    {

        #region Class Variables        
        /// <summary>
        /// The _health records repository
        /// </summary>
        private readonly IHealthRecordsRepository _healthRecordsRepository;

        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="HealthRecordsController"/> class.
        /// </summary>
        /// <param name="healthRecordsRepository">The health records repository.</param>
        public HealthRecordsController(IHealthRecordsRepository healthRecordsRepository)
        {
            _healthRecordsRepository = healthRecordsRepository;
        }
        #endregion

        #region Public Methods        
        /// <summary>
        /// Gets the health records.
        /// </summary>
        /// <returns></returns>
        public Response<HealthRecordsModel> GetHealthRecords()
        {
            return _healthRecordsRepository.GetHealthRecords();
        }

        #endregion
    }
}
