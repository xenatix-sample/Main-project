using System;
using Axis.Model.Clinical.ReviewOfSystems;
using Axis.Model.Common;
using Axis.Model.Common.General;
using Axis.RuleEngine.Clinical.ReviewOfSystems;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Filters;
using Axis.RuleEngine.Helpers.Results;
using System.Web.Http;
using Axis.Constant;

namespace Axis.RuleEngine.Plugins.Clinical
{
    /// <summary>
    ///
    /// </summary>
    public class ReviewOfSystemsController : BaseApiController
    {
        #region Class Variables

        /// <summary>
        /// The ros rule engine
        /// </summary>
        private readonly IReviewOfSystemsRuleEngine rosRuleEngine;

        #endregion Class Variables

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReviewOfSystemsController"/> class.
        /// </summary>
        /// <param name="rosRuleEngine">The ros rule engine.</param>
        public ReviewOfSystemsController(IReviewOfSystemsRuleEngine rosRuleEngine)
        {
            this.rosRuleEngine = rosRuleEngine;
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Gets the review of systems by contact.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Authorization(PermissionKey = ClinicalPermissionKey.Clinical_ReviewOfSystems_ReviewofSystems, Permission = Permission.Read)]
        public IHttpActionResult GetReviewOfSystemsByContact(long contactID)
        {
            return new HttpResult<Response<ReviewOfSystemsModel>>(rosRuleEngine.GetReviewOfSystemsByContact(contactID), Request);
        }

        /// <summary>
        /// Gets the review of system.
        /// </summary>
        /// <param name="rosID">The ros identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Authorization(PermissionKey = ClinicalPermissionKey.Clinical_ReviewOfSystems_ReviewofSystems, Permission = Permission.Read)]
        public IHttpActionResult GetReviewOfSystem(long rosID)
        {
            return new HttpResult<Response<ReviewOfSystemsModel>>(rosRuleEngine.GetReviewOfSystem(rosID), Request);
        }

        /// <summary>
        /// Gets the last active review of systems.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Authorization(PermissionKey = ClinicalPermissionKey.Clinical_ReviewOfSystems_ReviewofSystems, Permission = Permission.Read)]
        public IHttpActionResult GetLastActiveReviewOfSystems(long contactID)
        {
            return new HttpResult<Response<ReviewOfSystemsModel>>(rosRuleEngine.GetLastActiveReviewOfSystems(contactID), Request);
        }

        /// <summary>
        /// Navigations the validation states.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Authorization(PermissionKey = ClinicalPermissionKey.Clinical_ReviewOfSystems_ReviewofSystems, Permission = Permission.Read)]
        public IHttpActionResult NavigationValidationStates(long contactID)
        {
            return new HttpResult<Response<KeyValueModel>>(rosRuleEngine.NavigationValidationStates(contactID), Request);
        }

        /// <summary>
        /// Adds the review of system.
        /// </summary>
        /// <param name="ros">The ros.</param>
        /// <returns></returns>
        [HttpPost]
        [Authorization(PermissionKey = ClinicalPermissionKey.Clinical_ReviewOfSystems_ReviewofSystems, Permission = Permission.Create)]
        public IHttpActionResult AddReviewOfSystem(ReviewOfSystemsModel ros)
        {
            return new HttpResult<Response<ReviewOfSystemsModel>>(rosRuleEngine.AddReviewOfSystem(ros), Request);
        }

        /// <summary>
        /// Updates the review of system.
        /// </summary>
        /// <param name="ros">The ros.</param>
        /// <returns></returns>
        [HttpPut]
        [Authorization(PermissionKey = ClinicalPermissionKey.Clinical_ReviewOfSystems_ReviewofSystems, Permission = Permission.Update)]
        public IHttpActionResult UpdateReviewOfSystem(ReviewOfSystemsModel ros)
        {
            return new HttpResult<Response<ReviewOfSystemsModel>>(rosRuleEngine.UpdateReviewOfSystem(ros), Request);
        }

        /// <summary>
        /// Deletes the review of system.
        /// </summary>
        /// <param name="rosID">The ros identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        [Authorization(PermissionKey = ClinicalPermissionKey.Clinical_ReviewOfSystems_ReviewofSystems, Permission = Permission.Delete)]
        public IHttpActionResult DeleteReviewOfSystem(long rosID, DateTime modifiedOn)
        {
            return new HttpResult<Response<ReviewOfSystemsModel>>(rosRuleEngine.DeleteReviewOfSystem(rosID, modifiedOn), Request);
        }

        #endregion Public Methods
    }
}