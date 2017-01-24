using System;
using Axis.Model.Clinical.ReviewOfSystems;
using Axis.Model.Common;
using Axis.Model.Common.General;
using Axis.Service.Clinical.ReviewOfSystems;

namespace Axis.RuleEngine.Clinical.ReviewOfSystems
{
    /// <summary>
    ///
    /// </summary>
    public class ReviewOfSystemsRuleEngine : IReviewOfSystemsRuleEngine
    {
        #region Class Variables

        /// <summary>
        /// The ros service
        /// </summary>
        private readonly IReviewOfSystemsService rosService;

        #endregion Class Variables

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReviewOfSystemsRuleEngine"/> class.
        /// </summary>
        /// <param name="rosService">The ros service.</param>
        public ReviewOfSystemsRuleEngine(IReviewOfSystemsService rosService)
        {
            this.rosService = rosService;
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Gets the review of systems by contact.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<ReviewOfSystemsModel> GetReviewOfSystemsByContact(long contactID)
        {
            return rosService.GetReviewOfSystemsByContact(contactID);
        }

        /// <summary>
        /// Gets the review of system.
        /// </summary>
        /// <param name="rosID">The ros identifier.</param>
        /// <returns></returns>
        public Response<ReviewOfSystemsModel> GetReviewOfSystem(long rosID)
        {
            return rosService.GetReviewOfSystem(rosID);
        }

        /// <summary>
        /// Gets the last active review of systems.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<ReviewOfSystemsModel> GetLastActiveReviewOfSystems(long contactID)
        {
            return rosService.GetLastActiveReviewOfSystems(contactID);
        }

        /// <summary>
        /// Navigations the validation states.
        /// </summary>
        /// <param name="ContactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<KeyValueModel> NavigationValidationStates(long ContactID)
        {
            return rosService.NavigationValidationStates(ContactID);
        }

        /// <summary>
        /// Adds the review of system.
        /// </summary>
        /// <param name="ros">The ros.</param>
        /// <returns></returns>
        public Response<ReviewOfSystemsModel> AddReviewOfSystem(ReviewOfSystemsModel ros)
        {
            return rosService.AddReviewOfSystem(ros);
        }

        /// <summary>
        /// Updates the review of system.
        /// </summary>
        /// <param name="ros">The ros.</param>
        /// <returns></returns>
        public Response<ReviewOfSystemsModel> UpdateReviewOfSystem(ReviewOfSystemsModel ros)
        {
            return rosService.UpdateReviewOfSystem(ros);
        }

        /// <summary>
        /// Deletes the review of system.
        /// </summary>
        /// <param name="rosID">The ros identifier.</param>
        /// <returns></returns>
        public Response<ReviewOfSystemsModel> DeleteReviewOfSystem(long rosID, DateTime modifiedOn)
        {
            return rosService.DeleteReviewOfSystem(rosID, modifiedOn);
        }

        #endregion Public Methods
    }
}