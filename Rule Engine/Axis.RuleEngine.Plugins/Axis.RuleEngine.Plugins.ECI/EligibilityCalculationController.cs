using Axis.Model.Common;
using Axis.Model.ECI.EligibilityDetermination;
using Axis.RuleEngine.ECI.EligibilityDetermination;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Results;
using System.Web.Http;

namespace Axis.RuleEngine.Plugins.ECI
{
    public class EligibilityCalculationController : BaseApiController
    {
        #region Class Variables

        readonly IEligibilityCalculationRuleEngine _eligibilityCalculationRuleEngine = null;

        #endregion

        #region Constructors

        public EligibilityCalculationController(IEligibilityCalculationRuleEngine eligibilityCalculationRuleEngine)
        {
            _eligibilityCalculationRuleEngine = eligibilityCalculationRuleEngine;
        }

        #endregion

        #region Public Methods

        [HttpGet]
        public IHttpActionResult GetEligibilityCalculations(long eligibilityID)
        {
            return new HttpResult<Response<EligibilityCalculationModel>>(_eligibilityCalculationRuleEngine.GetEligibilityCalculations(eligibilityID), Request);
        }

        [HttpPost]
        public IHttpActionResult AddEligibilityCalculations(EligibilityCalculationModel calculation)
        {
            return new HttpResult<Response<EligibilityCalculationModel>>(_eligibilityCalculationRuleEngine.AddEligibilityCalculations(calculation), Request);
        }

        [HttpPut]
        public IHttpActionResult UpdateEligibilityCalculations(EligibilityCalculationModel calculation)
        {
            return new HttpResult<Response<EligibilityCalculationModel>>(_eligibilityCalculationRuleEngine.UpdateEligibilityCalculations(calculation), Request);
        }

        #endregion
    }
}
