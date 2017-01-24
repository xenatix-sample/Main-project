using System.Collections.Generic;
using System.Web.Http;
using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.ECI;
using Axis.Model.Common;
using Axis.Model.ECI;

namespace Axis.DataEngine.Plugins.ECI
{

    /// <summary>
    /// 
    /// </summary>
    public class ECIController : BaseApiController
    {
        /// <summary>
        /// The ECI data provider
        /// </summary>
        IECIDataProvider _eciDataProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ECIController"/> class.
        /// </summary>
        /// <param name="eciDataProvider">The ECI data provider.</param>
        public ECIController(IECIDataProvider eciDataProvider)
        {
            _eciDataProvider = eciDataProvider;
        }

        #region Public Methods

        [HttpPost]
        public IHttpActionResult AddECIScreening(ECIScreeningModel ediScreening)
        {
            return new HttpResult<Response<ECIScreeningModel>>(_eciDataProvider.AddECIScreening(ediScreening), Request);
        }

        [HttpGet]
        public IHttpActionResult GetECIScreenings(long contactId)
        {
            return new HttpResult<Response<ECIScreeningModel>>(_eciDataProvider.GetECIScreening(contactId), Request);
        }

        [HttpPost]
        public IHttpActionResult UpdateFinancialAssessment(ECIScreeningModel ediScreening)
        {
            return new HttpResult<Response<ECIScreeningModel>>(_eciDataProvider.UpdateECIScreening(ediScreening), Request);
        }

        [HttpDelete]
        public IHttpActionResult DeleteFinancialAssessmentDetail(long contactID)
        {
            return new HttpResult<Response<bool>>(_eciDataProvider.RemoveECIScreening(contactID), Request);
        }

        #endregion
    }
}