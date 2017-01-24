using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Registration;
using Axis.Model.Common;
using Axis.Model.Registration;
using System;
using System.Web.Http;

namespace Axis.DataEngine.Plugins.Registration
{
    public class BenefitsAssistanceController : BaseApiController
    {
        readonly IBenefitsAssistanceDataProvider _benefitsAssistanceDataProvider;


        /// <summary>
        /// Initializes a new instance of the <see cref="BenefitsAssistanceController"/> class.
        /// </summary>
        /// <param name="benefitsAssistanceDataProvider">The benefits assistance data provider.</param>
        public BenefitsAssistanceController(IBenefitsAssistanceDataProvider benefitsAssistanceDataProvider)
        {
            _benefitsAssistanceDataProvider = benefitsAssistanceDataProvider;
        }

        /// <summary>
        /// Gets the benefits assistance.
        /// </summary>
        /// <param name="benefitsAssistanceID">The benefits assistance identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetBenefitsAssistance(long benefitsAssistanceID)
        {
            return new HttpResult<Response<BenefitsAssistanceModel>>(_benefitsAssistanceDataProvider.GetBenefitsAssistance(benefitsAssistanceID), Request);
        }

        /// <summary>
        /// Gets the benefits assistance by contact identifier.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetBenefitsAssistanceByContactID(long contactID)
        {
            return new HttpResult<Response<BenefitsAssistanceModel>>(_benefitsAssistanceDataProvider.GetBenefitsAssistanceByContactID(contactID), Request);
        }

        /// <summary>
        /// Adds the benefits assistance.
        /// </summary>
        /// <param name="benefitsAssistanceModel">The benefits assistance model.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddBenefitsAssistance(BenefitsAssistanceModel benefitsAssistanceModel)
        {
            return new HttpResult<Response<BenefitsAssistanceModel>>(_benefitsAssistanceDataProvider.AddBenefitsAssistance(benefitsAssistanceModel), Request);
        }

        /// <summary>
        /// Updates the benefits assistance.
        /// </summary>
        /// <param name="benefitsAssistanceModel">The benefits assistance model.</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateBenefitsAssistance(BenefitsAssistanceModel benefitsAssistanceModel)
        {
            return new HttpResult<Response<BenefitsAssistanceModel>>(_benefitsAssistanceDataProvider.UpdateBenefitsAssistance(benefitsAssistanceModel), Request);
        }

        /// <summary>
        /// Deletes the benefits assistance.
        /// </summary>
        /// <param name="benefitsAssistanceID">The benefits assistance identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeleteBenefitsAssistance(long benefitsAssistanceID, DateTime modifiedOn)
        {
            return new HttpResult<Response<BenefitsAssistanceModel>>(_benefitsAssistanceDataProvider.DeleteBenefitsAssistance(benefitsAssistanceID, modifiedOn), Request);
        }
    }
}
