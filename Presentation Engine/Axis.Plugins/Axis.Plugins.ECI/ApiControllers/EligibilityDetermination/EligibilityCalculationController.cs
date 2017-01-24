using Axis.Plugins.ECI.Repository.EligibilityDetermination;
using Axis.PresentationEngine.Helpers.Controllers;
using System.Web.Http;
using Axis.Model.Common;
using Axis.Plugins.ECI.Models.EligibilityDetermination;

namespace Axis.Plugins.ECI.ApiControllers
{
    public class EligibilityCalculationController : BaseApiController
    {
        #region Class Variables

        private readonly IEligibilityCalculationRepository _eligibilityCalculationRepository;

        #endregion

        #region Constructors

        public EligibilityCalculationController(IEligibilityCalculationRepository eligibilityCalculationRepository)
        {
            _eligibilityCalculationRepository = eligibilityCalculationRepository;
        }

        #endregion

        #region Json Results

        /// <summary>
        /// Load the saved calculations for the eligibility record
        /// </summary>
        /// <param name="eligibilityID"></param>
        /// <returns></returns>
        [HttpGet]
        public Response<EligibilityCalculationViewModel> GetEligibilityCalculations(long eligibilityID)
        {
            return _eligibilityCalculationRepository.GetEligibilityCalculations(eligibilityID);
        }

        /// <summary>
        /// Add the contact's eligibility calculation data
        /// </summary>
        /// <param name="eligibilityCalculation"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<EligibilityCalculationViewModel> AddEligibilityCalculations(EligibilityCalculationViewModel eligibilityCalculation)
        {
            return _eligibilityCalculationRepository.AddEligibilityCalculations(eligibilityCalculation);
        }

        /// <summary>
        /// Save the contact's eligibility calculation data
        /// </summary>
        /// <param name="eligibilityCalculation"></param>
        /// <returns></returns>
        [HttpPut]
        public Response<EligibilityCalculationViewModel> UpdateEligibilityCalculations(EligibilityCalculationViewModel eligibilityCalculation)
        {
            return _eligibilityCalculationRepository.UpdateEligibilityCalculations(eligibilityCalculation);
        }

        #endregion
    }
}
