using System.Web.Http;
using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.ECI.EligibilityDetermination;
using Axis.Model.Common;
using Axis.Model.ECI;

namespace Axis.DataEngine.Plugins.ECI
{
    public class EligibilityDeterminationController : BaseApiController
    {
        #region Class Variables

        readonly IEligibilityDeterminationDataProvider _eligibilityDeterminationDataProvider = null;

        #endregion

        #region Constructors

        public EligibilityDeterminationController(IEligibilityDeterminationDataProvider eligibilityDeterminationDataProvider)
        {
            _eligibilityDeterminationDataProvider = eligibilityDeterminationDataProvider;
        }

        #endregion

        #region Public Methods

        [HttpGet]
        public IHttpActionResult GetEligibilityDetermination(long contactID)
        {
            return new HttpResult<Response<EligibilityDeterminationModel>>(_eligibilityDeterminationDataProvider.GetEligibilityDetermination(contactID), Request);
        }

        [HttpPost]
        public IHttpActionResult SaveEligibilityDetermination(EligibilityDeterminationModel eligibilityDetermination)
        {
            return new HttpResult<Response<EligibilityDeterminationModel>>(_eligibilityDeterminationDataProvider.SaveEligibilityDetermination(eligibilityDetermination), Request);
        }

        #endregion
    }
}
