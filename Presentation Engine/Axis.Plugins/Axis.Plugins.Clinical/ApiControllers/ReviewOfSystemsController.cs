using System;
using Axis.Plugins.Clinical.Models.ReviewOfSystems;
using Axis.Plugins.Clinical.Repository.ReviewOfSystems;
using Axis.PresentationEngine.Helpers.Controllers;
using System.Web.Http;
using Axis.Model.Common;

namespace Axis.Plugins.Clinical.ApiControllers
{
    /// <summary>
    ///
    /// </summary>
    public class ReviewOfSystemsController : BaseApiController
    {
        #region Class Variables

        /// <summary>
        /// The review of systems repository
        /// </summary>
        private readonly IReviewOfSystemsRepository reviewOfSystemsRepository;

        #endregion Class Variables

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReviewOfSystemsController"/> class.
        /// </summary>
        /// <param name="reviewOfSystemsRepository">The review of systems repository.</param>
        public ReviewOfSystemsController(IReviewOfSystemsRepository reviewOfSystemsRepository)
        {
            this.reviewOfSystemsRepository = reviewOfSystemsRepository;
        }

        #endregion Constructors

        #region Json Results

        /// <summary>
        /// Gets the review of systems by contact.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<ReviewOfSystemsViewModel> GetReviewOfSystemsByContact(long contactID)
        {
            return reviewOfSystemsRepository.GetReviewOfSystemsByContact(contactID);
        }

        /// <summary>
        /// Gets the review of system.
        /// </summary>
        /// <param name="rosID">The ros identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<ReviewOfSystemsViewModel> GetReviewOfSystem(long rosID)
        {
            return reviewOfSystemsRepository.GetReviewOfSystem(rosID);
        }

        /// <summary>
        /// Gets the last active review of systems.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<ReviewOfSystemsViewModel> GetLastActiveReviewOfSystems(long contactID)
        {
            return reviewOfSystemsRepository.GetLastActiveReviewOfSystems(contactID);
        }

        /// <summary>
        /// Navigations the validation states.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<PresentationEngine.Helpers.Model.General.KeyValueViewModel> NavigationValidationStates(long contactID)
        {
            return reviewOfSystemsRepository.NavigationValidationStates(contactID);
        }

        /// <summary>
        /// Adds the review of system.
        /// </summary>
        /// <param name="ros">The ros.</param>
        /// <returns></returns>
        [HttpPost]
        public Response<ReviewOfSystemsViewModel> AddReviewOfSystem(ReviewOfSystemsViewModel ros)
        {
            ros.DateEntered = ros.DateEntered.ToUniversalTime();
            return reviewOfSystemsRepository.AddReviewOfSystem(ros);
        }

        /// <summary>
        /// Updates the review of system.
        /// </summary>
        /// <param name="ros">The ros.</param>
        /// <returns></returns>
        [HttpPut]
        public Response<ReviewOfSystemsViewModel> UpdateReviewOfSystem(ReviewOfSystemsViewModel ros)
        {
            ros.DateEntered = ros.DateEntered.ToUniversalTime();
            return reviewOfSystemsRepository.UpdateReviewOfSystem(ros);
        }

        /// <summary>
        /// Deletes the review of system.
        /// </summary>
        /// <param name="rosID">The ros identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        public Response<ReviewOfSystemsViewModel> DeleteReviewOfSystem(long rosID, DateTime modifiedOn)
        {
            modifiedOn = modifiedOn.ToUniversalTime();
            return reviewOfSystemsRepository.DeleteReviewOfSystem(rosID, modifiedOn);
        }

        #endregion Json Results
    }
}