using System;
using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Registration;
using Axis.Model.Common;
using Axis.Model.Registration;
using System.Web.Http;

namespace Axis.DataEngine.Plugins.Registration
{
    /// <summary>
    ///
    /// </summary>
    public class AdditionalDemographicController : BaseApiController
    {
        /// <summary>
        /// The _additional demographics data provider
        /// </summary>
        private readonly IAdditionalDemographicDataProvider _additionalDemographicsDataProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdditionalDemographicController" /> class.
        /// </summary>
        /// <param name="additionalDemographicsDataProvider">The additional demographics data provider.</param>
        public AdditionalDemographicController(IAdditionalDemographicDataProvider additionalDemographicsDataProvider)
        {
            _additionalDemographicsDataProvider = additionalDemographicsDataProvider;
        }

        /// <summary>
        /// Gets the additional demographic.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        public IHttpActionResult GetAdditionalDemographic(long contactId)
        {
            return new HttpResult<Response<AdditionalDemographicsModel>>(_additionalDemographicsDataProvider.GetAdditionalDemographic(contactId), Request);
        }

        /// <summary>
        /// Adds the additional demographic.
        /// </summary>
        /// <param name="additional">The additional.</param>
        /// <returns></returns>
        public IHttpActionResult AddAdditionalDemographic(AdditionalDemographicsModel additional)
        {
            return new HttpResult<Response<AdditionalDemographicsModel>>(_additionalDemographicsDataProvider.AddAdditionalDemographic(additional), Request);
        }

        /// <summary>
        /// Updates the additional demographic.
        /// </summary>
        /// <param name="additional">The additional.</param>
        /// <returns></returns>
        public IHttpActionResult UpdateAdditionalDemographic(AdditionalDemographicsModel additional)
        {
            return new HttpResult<Response<AdditionalDemographicsModel>>(_additionalDemographicsDataProvider.UpdateAdditionalDemographic(additional), Request);
        }

        /// <summary>
        /// Deletes the additional demographic.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public IHttpActionResult DeleteAdditionalDemographic(long id, DateTime modifiedOn)
        {
            return new HttpResult<Response<AdditionalDemographicsModel>>(_additionalDemographicsDataProvider.DeleteAdditionalDemographic(id, modifiedOn), Request);
        }
    }
}