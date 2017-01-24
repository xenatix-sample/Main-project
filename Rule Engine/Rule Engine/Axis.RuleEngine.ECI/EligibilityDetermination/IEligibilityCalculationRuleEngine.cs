using Axis.Model.Common;
using Axis.Model.ECI;
using Axis.Model.ECI.EligibilityDetermination;

namespace Axis.RuleEngine.ECI.EligibilityDetermination
{
    public interface IEligibilityCalculationRuleEngine
    {
        Response<EligibilityCalculationModel> GetEligibilityCalculations(long eligibilityID);
        Response<EligibilityCalculationModel> AddEligibilityCalculations(EligibilityCalculationModel eligibilityDetermination);
        Response<EligibilityCalculationModel> UpdateEligibilityCalculations(EligibilityCalculationModel eligibilityDetermination);
    }
}
