using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Clinical.ReviewOfSystems;
using Axis.Model.Common;
using Axis.Model.Common.General;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Axis.DataProvider.Clinical.ReviewOfSystems
{
    /// <summary>
    ///
    /// </summary>
    public class ReviewOfSystemsDataProvider : IReviewOfSystemsDataProvider
    {
        #region Class Variables

        /// <summary>
        /// The unit of work
        /// </summary>
        private IUnitOfWork unitOfWork;

        #endregion Class Variables

        #region Constructors

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ReviewOfSystemsDataProvider(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Gets the review of systems by contact.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Response<ReviewOfSystemsModel> GetReviewOfSystemsByContact(long contactID)
        {
            var rosRepository = unitOfWork.GetRepository<ReviewOfSystemsModel>(SchemaName.Clinical);

            SqlParameter contactIdParam = new SqlParameter("ContactID", contactID);
            List<SqlParameter> procParams = new List<SqlParameter>() { contactIdParam };

            var rosByContact = rosRepository.ExecuteStoredProc("usp_GetReviewOfSystemsByContact", procParams);

            return rosByContact;
        }

        /// <summary>
        /// Gets the review of system.
        /// </summary>
        /// <param name="rosID">The ros identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Response<ReviewOfSystemsModel> GetReviewOfSystem(long rosID)
        {
            var rosRepository = unitOfWork.GetRepository<ReviewOfSystemsModel>(SchemaName.Clinical);

            SqlParameter rosIdParam = new SqlParameter("RoSID", rosID);
            List<SqlParameter> procParams = new List<SqlParameter>() { rosIdParam };

            var ros = rosRepository.ExecuteStoredProc("usp_GetReviewOfSystem", procParams);

            return ros;
        }

        /// <summary>
        /// Gets the last active review of systems.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<ReviewOfSystemsModel> GetLastActiveReviewOfSystems(long contactID)
        {
            var rosRepository = unitOfWork.GetRepository<ReviewOfSystemsModel>(SchemaName.Clinical);

            SqlParameter contactIdParam = new SqlParameter("ContactID", contactID);
            List<SqlParameter> procParams = new List<SqlParameter>() { contactIdParam };

            var rosByContact = rosRepository.ExecuteStoredProc("usp_GetLastActiveReviewOfSystems", procParams);

            return rosByContact;
        }

        /// <summary>
        /// Navigations the validation states.
        /// </summary>
        /// <param name="ContactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<KeyValueModel> NavigationValidationStates(long ContactID)
        {
            var rosRepository = unitOfWork.GetRepository<KeyValueModel>(SchemaName.Clinical);

            SqlParameter contactIdParam = new SqlParameter("ContactID", ContactID);
            List<SqlParameter> procParams = new List<SqlParameter>() { contactIdParam };

            var rosNavigation = rosRepository.ExecuteStoredProc("usp_GetClinicalIntakeStatus", procParams);

            return rosNavigation;
        }

        /// <summary>
        /// Adds the review of system.
        /// </summary>
        /// <param name="ros">The ros.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Response<ReviewOfSystemsModel> AddReviewOfSystem(ReviewOfSystemsModel ros)
        {
            var rosParameters = BuildRosSpParams(ros, false);
            var rosRepository = unitOfWork.GetRepository<ReviewOfSystemsModel>(SchemaName.Clinical);
            return unitOfWork.EnsureInTransaction(rosRepository.ExecuteNQStoredProc, "usp_AddReviewOfSystem", rosParameters,
                forceRollback: ros.ForceRollback.GetValueOrDefault(false), idResult: true);
        }

        /// <summary>
        /// Updates the review of system.
        /// </summary>
        /// <param name="ros">The ros.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Response<ReviewOfSystemsModel> UpdateReviewOfSystem(ReviewOfSystemsModel ros)
        {
            var rosParameters = BuildRosSpParams(ros, true);
            var rosRepository = unitOfWork.GetRepository<ReviewOfSystemsModel>(SchemaName.Clinical);
            return unitOfWork.EnsureInTransaction(rosRepository.ExecuteNQStoredProc, "usp_UpdateReviewOfSystem", rosParameters,
                          forceRollback: ros.ForceRollback.GetValueOrDefault(false));
        }

        /// <summary>
        /// Deletes the review of system.
        /// </summary>
        /// <param name="rosID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<ReviewOfSystemsModel> DeleteReviewOfSystem(long rosID, DateTime modifiedOn)
        {
            var rosRepository = unitOfWork.GetRepository<ReviewOfSystemsModel>(SchemaName.Clinical);
            var param = new SqlParameter("RoSID", rosID);
            var modifiedOnParam = new SqlParameter("ModifiedOn", modifiedOn);
            var procParams = new List<SqlParameter> { param, modifiedOnParam };
            var result = rosRepository.ExecuteNQStoredProc("usp_DeleteReviewOfSystem", procParams);
            return result;
        }

        #endregion Public Methods

        #region Helpers

        /// <summary>
        /// Builds the ros sp parameters.
        /// </summary>
        /// <param name="ros">The ros.</param>
        /// <param name="isUpdate">if set to <c>true</c> [is update].</param>
        /// <returns></returns>
        private List<SqlParameter> BuildRosSpParams(ReviewOfSystemsModel ros, bool isUpdate)
        {
            var spParameters = new List<SqlParameter>();

            if (isUpdate)
                spParameters.Add(new SqlParameter("RoSID", ros.RoSID));

            spParameters.Add(new SqlParameter("ContactID", (object)ros.ContactID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("DateEntered", (object)ros.DateEntered ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ReviewdBy", (object)ros.ReviewdBy ?? DBNull.Value));
            spParameters.Add(new SqlParameter("AssessmentID", (object)ros.AssessmentID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ResponseID", (object)ros.ResponseID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("IsReviewChanged", (object)ros.IsReviewChanged ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ModifiedOn", ros.ModifiedOn ?? DateTime.Now));

            return spParameters;
        }

        #endregion Helpers
    }
}
