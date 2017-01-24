using Axis.Model.Common;
using Axis.Model.ECI.EligibilityDetermination;
using Axis.Service.ECI.EligibilityDetermination;

namespace Axis.RuleEngine.ECI.EligibilityDetermination
{
    public class EligibilityCalculationRuleEngine : IEligibilityCalculationRuleEngine
    {
        #region Class Variables

        private readonly IEligibilityCalculationService _eligibilityCalculationService;

        #endregion

        #region Constructors

        public EligibilityCalculationRuleEngine(IEligibilityCalculationService eligibilityCalculationService)
        {
            _eligibilityCalculationService = eligibilityCalculationService;
        }

        #endregion

        #region Public Methods

        public Response<EligibilityCalculationModel> GetEligibilityCalculations(long eligibilityID)
        {
            return _eligibilityCalculationService.GetEligibilityCalculations(eligibilityID);
        }

        public Response<EligibilityCalculationModel> AddEligibilityCalculations(EligibilityCalculationModel calculation)
        {
            return _eligibilityCalculationService.AddEligibilityCalculations(calculation);
        }

        public Response<EligibilityCalculationModel> UpdateEligibilityCalculations(EligibilityCalculationModel calculation)
        {
            return _eligibilityCalculationService.UpdateEligibilityCalculations(calculation);
        }

        #endregion
    }
}
