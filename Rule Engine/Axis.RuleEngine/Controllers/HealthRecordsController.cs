using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.RuleEngine.BusinessAdmin.HealthRecords;
using Axis.RuleEngine.Helpers.Controllers;

namespace Axis.RuleEngine.Service.Controllers
{
    public class HealthRecordsController : BaseApiController
    {
        #region Class Variables        
        /// <summary>
        /// The _health records rule engine
        /// </summary>
        private readonly IHealthRecordsRuleEngine _healthRecordsRuleEngine;

        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="HealthRecordsController"/> class.
        /// </summary>
        /// <param name="healthRecordsRuleEngine">The health records rule engine.</param>
        public HealthRecordsController(IHealthRecordsRuleEngine healthRecordsRuleEngine)
        {
            _healthRecordsRuleEngine = healthRecordsRuleEngine;
        }
        #endregion

        #region Public Methods        
        /// <summary>
        /// Gets the health records.
        /// </summary>
        /// <returns></returns>
        public Response<HealthRecordsModel> GetHealthRecords()
        {
            return _healthRecordsRuleEngine.GetHealthRecords();
        }

        #endregion
    }
}
