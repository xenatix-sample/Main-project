using Axis.Model.Common;
using Axis.Model.ECI.EligibilityDetermination;

namespace Axis.Service.ECI.EligibilityDetermination
{
    public interface IEligibilityCalculationService
    {
        Response<EligibilityCalculationModel> GetEligibilityCalculations(long eligibilityID);
        Response<EligibilityCalculationModel> AddEligibilityCalculations(EligibilityCalculationModel calculation);
        Response<EligibilityCalculationModel> UpdateEligibilityCalculations(EligibilityCalculationModel calculation);
    }
}
