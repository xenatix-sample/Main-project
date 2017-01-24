using Axis.Model.Common;
using Axis.Model.ECI.EligibilityDetermination;

namespace Axis.DataProvider.ECI.EligibilityDetermination
{
    public interface IEligibilityCalculationDataProvider
    {
        Response<EligibilityCalculationModel> GetEligibilityCalculations(long eligibilityID);
        Response<EligibilityCalculationModel> AddEligibilityCalculations(EligibilityCalculationModel calculation);
        Response<EligibilityCalculationModel> UpdateEligibilityCalculations(EligibilityCalculationModel calculation);
    }
}
