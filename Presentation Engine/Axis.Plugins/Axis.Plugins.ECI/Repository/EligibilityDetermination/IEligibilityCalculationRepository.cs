using Axis.Model.Common;
using Axis.Plugins.ECI.Models.EligibilityDetermination;

namespace Axis.Plugins.ECI.Repository.EligibilityDetermination
{
    public interface IEligibilityCalculationRepository
    {
        Response<EligibilityCalculationViewModel> GetEligibilityCalculations(long eligibilityID);
        Response<EligibilityCalculationViewModel> AddEligibilityCalculations(EligibilityCalculationViewModel calculation);
        Response<EligibilityCalculationViewModel> UpdateEligibilityCalculations(EligibilityCalculationViewModel calculation);
    }
}
