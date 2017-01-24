using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.Service.BusinessAdmin.HealthRecords;

namespace Axis.RuleEngine.BusinessAdmin.HealthRecords
{
    public class HealthRecordsRuleEngine : IHealthRecordsRuleEngine
    {
        #region Class Variables        
        /// <summary>
        /// The _health records service
        /// </summary>
        private readonly IHealthRecordsService _healthRecordsService;

        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="HealthRecordsRuleEngine"/> class.
        /// </summary>
        /// <param name="healthRecordsService">The health records service.</param>
        public HealthRecordsRuleEngine(IHealthRecordsService healthRecordsService)
        {
            _healthRecordsService = healthRecordsService;
        }

        #endregion

        #region Public Methods        
        /// <summary>
        /// Gets the health records.
        /// </summary>
        /// <returns></returns>
        public Response<HealthRecordsModel> GetHealthRecords()
        {
            return _healthRecordsService.GetHealthRecords();
        }

        #endregion
    }
}
