using System;
using Axis.Model.Clinical.ReviewOfSystems;
using Axis.Model.Common;
using Axis.Model.Common.General;

namespace Axis.Service.Clinical.ReviewOfSystems
{
    /// <summary>
    ///
    /// </summary>
    public interface IReviewOfSystemsService
    {
        /// <summary>
        /// Gets the review of systems by contact.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        Response<ReviewOfSystemsModel> GetReviewOfSystemsByContact(long contactID);

        /// <summary>
        /// Gets the review of system.
        /// </summary>
        /// <param name="rosID">The ros identifier.</param>
        /// <returns></returns>
        Response<ReviewOfSystemsModel> GetReviewOfSystem(long rosID);

        /// <summary>
        /// Gets the last active review of systems.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        Response<ReviewOfSystemsModel> GetLastActiveReviewOfSystems(long contactID);

        /// <summary>
        /// Navigations the validation states.
        /// </summary>
        /// <param name="ContactID">The contact identifier.</param>
        /// <returns></returns>
        Response<KeyValueModel> NavigationValidationStates(long ContactID);

        /// <summary>
        /// Adds the review of system.
        /// </summary>
        /// <param name="ros">The ros.</param>
        /// <returns></returns>
        Response<ReviewOfSystemsModel> AddReviewOfSystem(ReviewOfSystemsModel ros);

        /// <summary>
        /// Updates the review of system.
        /// </summary>
        /// <param name="ros">The ros.</param>
        /// <returns></returns>
        Response<ReviewOfSystemsModel> UpdateReviewOfSystem(ReviewOfSystemsModel ros);

        /// <summary>
        /// Deletes the review of system.
        /// </summary>
        /// <param name="rosID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        Response<ReviewOfSystemsModel> DeleteReviewOfSystem(long rosID, DateTime modifiedOn);
    }
}