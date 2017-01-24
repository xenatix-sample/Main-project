using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.BusinessAdmin.HealthRecords;
using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using System.Web.Http;

namespace Axis.DataEngine.Service.Controllers
{
    public class HealthRecordsController : BaseApiController
    {
        #region Class Variables        
        /// <summary>
        /// The _health records data provider
        /// </summary>
        readonly IHealthRecordsDataProvider _healthRecordsDataProvider = null;

        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="HealthRecordsController"/> class.
        /// </summary>
        /// <param name="healthRecordsDataProvider">The health records data provider.</param>
        public HealthRecordsController(IHealthRecordsDataProvider healthRecordsDataProvider)
        {
            _healthRecordsDataProvider = healthRecordsDataProvider;
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Gets the health records.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetHealthRecords()
        {
            var clientMergeResponse = _healthRecordsDataProvider.GetHealthRecords();

            return new HttpResult<Response<HealthRecordsModel>>(clientMergeResponse, Request);
        }
        #endregion
    }
}
