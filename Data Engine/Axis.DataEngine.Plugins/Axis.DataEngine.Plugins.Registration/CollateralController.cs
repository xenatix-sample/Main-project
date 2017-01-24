using System;
using System.Web.Http;
using Axis.DataProvider.Registration;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.DataEngine.Helpers.Results;
using Axis.DataEngine.Helpers.Controllers;

namespace Axis.DataEngine.Plugins.Registration
{
    public class CollateralController : BaseApiController
    {
        readonly ICollateralDataProvider collateralDataProvider;

        public CollateralController(ICollateralDataProvider collateralDataProvider)
        {
            this.collateralDataProvider = collateralDataProvider;
        }

        /// <summary>
        /// To get Emergency Contact list
        /// </summary>
        /// <param name="contactID">Contact Id</param>
        /// <param name="contactTypeId">Type of Contact</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetCollaterals(long contactID, int contactTypeID, bool getContactDetails)
        {
            return new HttpResult<Response<CollateralModel>>(collateralDataProvider.GetCollaterals(contactID, contactTypeID, getContactDetails), Request);
        }

        /// <summary>
        /// To add emergency contact
        /// </summary>
        /// <param name="CollateralModel">Emergency Contact Model</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddCollateral(CollateralModel collateralModel)
        {
            return new HttpResult<Response<CollateralModel>>(collateralDataProvider.AddCollateral(collateralModel), Request);
        }

        /// <summary>
        /// To update emergency contact
        /// </summary>
        /// <param name="CollateralModel">Emergency Contact Model</param>
        /// <returns>Response of type CollateralModel</returns>
        [HttpPut]
        public IHttpActionResult UpdateCollateral(CollateralModel collateralModel)
        {
            return new HttpResult<Response<CollateralModel>>(collateralDataProvider.UpdateCollateral(collateralModel), Request);
        }


        /// <summary>
        /// Deletes the collateral.
        /// </summary>
        /// <param name="parentContactID">The parent contact identifier.</param>
        /// <param name="contactID">The contact identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeleteCollateral(long parentContactID, long contactID, DateTime modifiedOn)
        {
            return new HttpResult<Response<CollateralModel>>(collateralDataProvider.DeleteCollateral(parentContactID, contactID, modifiedOn), Request);
        }

    }
}
