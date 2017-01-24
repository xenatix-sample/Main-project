using System.Web.Http;
using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.ECI.Demographic;
using Axis.Model.Common;
using Axis.Model.ECI;

namespace Axis.DataEngine.Plugins.ECI
{
    public class ECIDemographicController : BaseApiController
    {
         #region Class Variables

        readonly IECIDemographicDataProvider _eciDemographicDataProvider = null;

        #endregion

        #region Constructors

        public ECIDemographicController(IECIDemographicDataProvider eciDemographicDataProvider)
        {
            _eciDemographicDataProvider = eciDemographicDataProvider;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the contact demographics.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public IHttpActionResult GetContactDemographics(long contactID)
        {
            return new HttpResult<Response<ECIContactDemographicsModel>>(_eciDemographicDataProvider.GetContactDemographics(contactID), Request);
        }

        /// <summary>
        /// Adds the contact demographics.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns></returns>
        public IHttpActionResult AddContactDemographics(ECIContactDemographicsModel contact)
        {
            return new HttpResult<Response<ECIContactDemographicsModel>>(_eciDemographicDataProvider.AddContactDemographics(contact), Request);
        }

        /// <summary>
        /// Updates the contact demographics.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns></returns>
        public IHttpActionResult UpdateContactDemographics(ECIContactDemographicsModel contact)
        {
            return new HttpResult<Response<ECIContactDemographicsModel>>(_eciDemographicDataProvider.UpdateContactDemographics(contact), Request);
        }

        #endregion
    }
}
