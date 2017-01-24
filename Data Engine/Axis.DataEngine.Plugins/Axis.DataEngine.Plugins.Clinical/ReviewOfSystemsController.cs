using System;
using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Clinical.ReviewOfSystems;
using Axis.Model.Clinical.ReviewOfSystems;
using Axis.Model.Common;
using Axis.Model.Common.General;
using System.Web.Http;

namespace Axis.DataEngine.Plugins.Clinical
{
    /// <summary>
    ///
    /// </summary>
    public class ReviewOfSystemsController : BaseApiController
    {
        #region Class Variables

        /// <summary>
        /// The ros data provider
        /// </summary>
        private readonly IReviewOfSystemsDataProvider rosDataProvider;

        #endregion Class Variables

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReviewOfSystemsController"/> class.
        /// </summary>
        /// <param name="rosDataProvider">The ros data provider.</param>
        public ReviewOfSystemsController(IReviewOfSystemsDataProvider rosDataProvider)
        {
            this.rosDataProvider = rosDataProvider;
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Gets the review of systems by contact.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetReviewOfSystemsByContact(long contactID)
        {
            return new HttpResult<Response<ReviewOfSystemsModel>>(rosDataProvider.GetReviewOfSystemsByContact(contactID), Request);
        }

        /// <summary>
        /// Gets the review of system.
        /// </summary>
        /// <param name="rosID">The ros identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetReviewOfSystem(long rosID)
        {
            return new HttpResult<Response<ReviewOfSystemsModel>>(rosDataProvider.GetReviewOfSystem(rosID), Request);
        }

        /// <summary>
        /// Gets the last active review of systems.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetLastActiveReviewOfSystems(long contactID)
        {
            return new HttpResult<Response<ReviewOfSystemsModel>>(rosDataProvider.GetLastActiveReviewOfSystems(contactID), Request);
        }

        /// <summary>
        /// Navigations the validation states.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult NavigationValidationStates(long contactID)
        {
            return new HttpResult<Response<KeyValueModel>>(rosDataProvider.NavigationValidationStates(contactID), Request);
        }

        /// <summary>
        /// Adds the review of system.
        /// </summary>
        /// <param name="ros">The ros.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddReviewOfSystem(ReviewOfSystemsModel ros)
        {
            return new HttpResult<Response<ReviewOfSystemsModel>>(rosDataProvider.AddReviewOfSystem(ros), Request);
        }

        /// <summary>
        /// Updates the review of system.
        /// </summary>
        /// <param name="ros">The ros.</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateReviewOfSystem(ReviewOfSystemsModel ros)
        {
            return new HttpResult<Response<ReviewOfSystemsModel>>(rosDataProvider.UpdateReviewOfSystem(ros), Request);
        }

        /// <summary>
        /// Deletes the review of system.
        /// </summary>
        /// <param name="rosID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeleteReviewOfSystem(long rosID, DateTime modifiedOn)
        {
            return new HttpResult<Response<ReviewOfSystemsModel>>(rosDataProvider.DeleteReviewOfSystem(rosID, modifiedOn), Request);
        }

        #endregion Public Methods
    }
}