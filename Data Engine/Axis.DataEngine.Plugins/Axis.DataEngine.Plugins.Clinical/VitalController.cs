using System;
using Axis.DataEngine.Helpers.Results;
using Axis.DataEngine.Helpers.Controllers;
using Axis.Model.Clinical;
using Axis.Model.Common;
using System.Web.Http;
using Axis.DataProvider.Clinical.Vital;

namespace Axis.DataEngine.Plugins.Clinical
{
    public class VitalController : BaseApiController
    {
        /// <summary>
        /// The Vital data provider
        /// </summary>
        private readonly IVitalDataProvider vitalDataProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="VitalController"/> class.
        /// </summary>
        /// <param name="vitalDataProvider">The contact Vital data provider.</param>
        public VitalController(IVitalDataProvider vitalDataProvider)
        {
            this.vitalDataProvider = vitalDataProvider;
        }

        /// <summary>
        /// Gets the Vitals.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetContactVitals(long contactId)
        {
            return new HttpResult<Response<VitalModel>>(vitalDataProvider.GetContactVitals(contactId), Request);
        }

        /// <summary>
        /// Adds the Vital.
        /// </summary>
        /// <param name="vital">The vital.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddVital(VitalModel vital)
        {
            return new HttpResult<Response<VitalModel>>(vitalDataProvider.AddVital(vital), Request);
        }

        /// <summary>
        /// Updates the Vital.
        /// </summary>
        /// <param name="vital">The vital.</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateVital(VitalModel vital)
        {
            return new HttpResult<Response<VitalModel>>(vitalDataProvider.UpdateVital(vital), Request);
        }

        /// <summary>
        /// Deletes the Vital.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeleteVital(long id, DateTime modifiedOn)
        {
            return new HttpResult<Response<VitalModel>>(vitalDataProvider.DeleteVital(id, modifiedOn), Request);
        }
    }
}
