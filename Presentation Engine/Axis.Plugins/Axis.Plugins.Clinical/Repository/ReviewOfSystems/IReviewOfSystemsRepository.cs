using System;
using Axis.Model.Common;
using Axis.Plugins.Clinical.Models.ReviewOfSystems;
using Axis.PresentationEngine.Helpers.Model.General;

namespace Axis.Plugins.Clinical.Repository.ReviewOfSystems
{
    /// <summary>
    ///
    /// </summary>
    public interface IReviewOfSystemsRepository
    {
        /// <summary>
        /// Gets the review of systems by contact.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        Response<ReviewOfSystemsViewModel> GetReviewOfSystemsByContact(long contactID);

        /// <summary>
        /// Gets the review of system.
        /// </summary>
        /// <param name="rosID">The ros identifier.</param>
        /// <returns></returns>
        Response<ReviewOfSystemsViewModel> GetReviewOfSystem(long rosID);

        /// <summary>
        /// Gets the last active review of systems.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        Response<ReviewOfSystemsViewModel> GetLastActiveReviewOfSystems(long contactID);

        /// <summary>
        /// Navigations the validation states.
        /// </summary>
        /// <param name="ContactID">The contact identifier.</param>
        /// <returns></returns>
        Response<KeyValueViewModel> NavigationValidationStates(long ContactID);

        /// <summary>
        /// Adds the review of system.
        /// </summary>
        /// <param name="ros">The ros.</param>
        /// <returns></returns>
        Response<ReviewOfSystemsViewModel> AddReviewOfSystem(ReviewOfSystemsViewModel ros);

        /// <summary>
        /// Updates the review of system.
        /// </summary>
        /// <param name="ros">The ros.</param>
        /// <returns></returns>
        Response<ReviewOfSystemsViewModel> UpdateReviewOfSystem(ReviewOfSystemsViewModel ros);

        /// <summary>
        /// Deletes the review of system.
        /// </summary>
        /// <param name="rosID">The ros identifier.</param>
        /// <returns></returns>
        Response<ReviewOfSystemsViewModel> DeleteReviewOfSystem(long rosID, DateTime modifiedOn);
    }
}