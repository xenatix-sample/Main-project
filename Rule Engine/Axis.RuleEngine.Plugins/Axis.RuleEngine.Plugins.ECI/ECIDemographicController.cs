using System.Collections.Generic;
using System.Web.Http;
using Axis.Helpers.Validation;
using Axis.Model.Common;
using Axis.Model.ECI;
using Axis.RuleEngine.ECI.Demographic;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Results;
using Axis.RuleEngine.Helpers.Filters;
using Axis.Constant;

namespace Axis.RuleEngine.Plugins.ECI
{

    public class ECIDemographicController : BaseApiController
    {
        #region Class Variables

        readonly IECIDemographicRuleEngine _eciDemographicRuleEngine = null;

        #endregion

        #region Constructors

        public ECIDemographicController(IECIDemographicRuleEngine eciDemographicRuleEngine)
        {
            _eciDemographicRuleEngine = eciDemographicRuleEngine;
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
            return new HttpResult<Response<ECIContactDemographicsModel>>(_eciDemographicRuleEngine.GetContactDemographics(contactID), Request);
        }

        /// <summary>
        /// Adds the contact demographics.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new string[] { ECIPermissionKey.ECI_Registration_Demographics, ECIPermissionKey.ECI_Registration_Referral }, Permission = Permission.Create)]
        public IHttpActionResult AddContactDemographics(ECIContactDemographicsModel contact)
        {
            if (ModelState.IsValid)
            {
                return new HttpResult<Response<ECIContactDemographicsModel>>(_eciDemographicRuleEngine.AddContactDemographics(contact), Request);
            }
            else
            {
                var errorMessage = ModelState.ParseError();
                var validationResponse = new Response<ECIContactDemographicsModel>() { DataItems = new List<ECIContactDemographicsModel>(), ResultCode = -1, ResultMessage = errorMessage };
                return new HttpResult<Response<ECIContactDemographicsModel>>(validationResponse, Request);
            }
        }

        /// <summary>
        /// Updates the contact demographics.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns></returns>
        [Authorization(Modules = new string[] { Module.General }, PermissionKeys = new string[] { ECIPermissionKey.ECI_Registration_Demographics, ECIPermissionKey.ECI_Registration_Referral }, Permission = Permission.Update)]
        public IHttpActionResult UpdateContactDemographics(ECIContactDemographicsModel contact)
        {
            if (ModelState.IsValid)
            {
                return new HttpResult<Response<ECIContactDemographicsModel>>(_eciDemographicRuleEngine.UpdateContactDemographics(contact), Request);
            }
            else
            {
                var errorMessage = ModelState.ParseError();
                var validationResponse = new Response<ECIContactDemographicsModel>() { DataItems = new List<ECIContactDemographicsModel>(), ResultCode = -1, ResultMessage = errorMessage };
                return new HttpResult<Response<ECIContactDemographicsModel>>(validationResponse, Request);
            }
        }

        #endregion
    }
}
