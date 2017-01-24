using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.ECI.EligibilityDetermination;
using Axis.Model.Common;
using Axis.Model.ECI.EligibilityDetermination;
using System.Web.Http;

namespace Axis.DataEngine.Plugins.ECI
{
    public class EligibilityCalculationController : BaseApiController
    {
        #region Class Variables

        readonly IEligibilityCalculationDataProvider _eligibilityCalculationDataProvider = null;

        #endregion

        #region Constructors

        public EligibilityCalculationController(IEligibilityCalculationDataProvider eligibilityCalculationDataProvider)
        {
            _eligibilityCalculationDataProvider = eligibilityCalculationDataProvider;
        }

        #endregion

        #region Public Methods

        [HttpGet]
        public IHttpActionResult GetEligibilityCalculations(long eligibilityID)
        {
            return new HttpResult<Response<EligibilityCalculationModel>>(_eligibilityCalculationDataProvider.GetEligibilityCalculations(eligibilityID), Request);
        }

        [HttpPost]
        public IHttpActionResult AddEligibilityCalculations(EligibilityCalculationModel calculation)
        {
            return new HttpResult<Response<EligibilityCalculationModel>>(_eligibilityCalculationDataProvider.AddEligibilityCalculations(calculation), Request);
        }

        [HttpPut]
        public IHttpActionResult UpdateEligibilityCalculations(EligibilityCalculationModel calculation)
        {
            return new HttpResult<Response<EligibilityCalculationModel>>(_eligibilityCalculationDataProvider.UpdateEligibilityCalculations(calculation), Request);
        }

        #endregion
    }
}
