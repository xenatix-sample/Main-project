using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.ECI;
using Axis.Model.Common;
using Axis.Model.ECI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Axis.DataEngine.Plugins.ECI
{
   public class ECIAdditionalDemographicsController :BaseApiController
    {
          /// <summary>
        /// The _additional demographics data provider
        /// </summary>
        private readonly IECIAdditionalDemographicDataProvider _additionalDemographicsDataProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdditionalDemographicController" /> class.
        /// </summary>
        /// <param name="additionalDemographicsDataProvider">The additional demographics data provider.</param>
        public ECIAdditionalDemographicsController(IECIAdditionalDemographicDataProvider additionalDemographicsDataProvider)
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
            return new HttpResult<Response<ECIAdditionalDemographicsModel>>(_additionalDemographicsDataProvider.GetAdditionalDemographic(contactId), Request);
        }

        /// <summary>
        /// Adds the additional demographic.
        /// </summary>
        /// <param name="additional">The additional.</param>
        /// <returns></returns>
        public IHttpActionResult AddAdditionalDemographic(ECIAdditionalDemographicsModel additional)
        {
            return new HttpResult<Response<ECIAdditionalDemographicsModel>>(_additionalDemographicsDataProvider.AddAdditionalDemographic(additional), Request);
        }

        /// <summary>
        /// Updates the additional demographic.
        /// </summary>
        /// <param name="additional">The additional.</param>
        /// <returns></returns>
        public IHttpActionResult UpdateAdditionalDemographic(ECIAdditionalDemographicsModel additional)
        {
            return new HttpResult<Response<ECIAdditionalDemographicsModel>>(_additionalDemographicsDataProvider.UpdateAdditionalDemographic(additional), Request);
        }

        /// <summary>
        /// Deletes the additional demographic.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public IHttpActionResult DeleteAdditionalDemographic(long id, DateTime modifiedOn)
        {
            return new HttpResult<Response<ECIAdditionalDemographicsModel>>(_additionalDemographicsDataProvider.DeleteAdditionalDemographic(id, modifiedOn), Request);
        }
    }
}
