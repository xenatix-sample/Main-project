using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using Axis.Model.Registration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Axis.DataProvider.Registration.Common
{
    /// <summary>
    ///
    /// </summary>
    public class ContactPresentingProblemProvider : IContactPresentingProblemProvider
    {
        #region initializations

        /// <summary>
        /// The unit of work
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactPresentingProblemProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ContactPresentingProblemProvider(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region Public Methods

        /// <summary>
        /// Gets the contact presenting problem.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<ContactPresentingProblemModel> GetContactPresentingProblem(long contactID)
        {
            var spParameters = new List<SqlParameter> { new SqlParameter("ContactID", contactID) };

            var repository = unitOfWork.GetRepository<ContactPresentingProblemModel>(SchemaName.Registration);
            var results = repository.ExecuteStoredProc("usp_GetContactPresentingProblem", spParameters);

            return results;
        }

        /// <summary>
        /// Adds the contact presenting problem.
        /// </summary>
        /// <param name="presentingProblem">The presenting problem.</param>
        /// <returns></returns>
        public Response<ContactPresentingProblemModel> AddContactPresentingProblem(ContactPresentingProblemModel presentingProblem)
        {
            var repository = unitOfWork.GetRepository<ContactPresentingProblemModel>(SchemaName.Registration);

            var spParameters = BuildPresentingProblemSpParams(presentingProblem, false);
            return unitOfWork.EnsureInTransaction<Response<ContactPresentingProblemModel>>(repository.ExecuteNQStoredProc, "usp_AddContactPresentingProblem", spParameters, forceRollback: presentingProblem.ForceRollback ?? false, idResult: true);
        }

        /// <summary>
        /// Updates the contact presenting problem.
        /// </summary>
        /// <param name="presentingProblem">The presenting problem.</param>
        /// <returns></returns>
        public Response<ContactPresentingProblemModel> UpdateContactPresentingProblem(ContactPresentingProblemModel presentingProblem)
        {
            var repository = unitOfWork.GetRepository<ContactPresentingProblemModel>(SchemaName.Registration);

            var spParameters = BuildPresentingProblemSpParams(presentingProblem, true);
            return unitOfWork.EnsureInTransaction<Response<ContactPresentingProblemModel>>(repository.ExecuteNQStoredProc, "usp_UpdateContactPresentingProblem", spParameters, forceRollback: presentingProblem.ForceRollback ?? false);
        }

        /// <summary>
        /// Deletes the contact presenting problem.
        /// </summary>
        /// <param name="contactPresentingProblemID">The contact presenting problem identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        public Response<ContactPresentingProblemModel> DeleteContactPresentingProblem(long contactPresentingProblemID, DateTime modifiedOn)
        {
            List<SqlParameter> procsParameters = new List<SqlParameter> { new SqlParameter("ContactPresentingProblemID", contactPresentingProblemID), new SqlParameter("ModifiedOn", modifiedOn) };
            var contactBenefitRepository = unitOfWork.GetRepository<ContactPresentingProblemModel>(SchemaName.Registration);
            Response<ContactPresentingProblemModel> spResults = new Response<ContactPresentingProblemModel>();
            spResults = contactBenefitRepository.ExecuteNQStoredProc("usp_DeleteContactPresentingProblem", procsParameters);
            return spResults;
        }

        #endregion Public Methods

        #region Helpers

        /// <summary>
        /// Builds the presenting problem sp parameters.
        /// </summary>
        /// <param name="presentingProblem">The contact alias.</param>
        /// <param name="isUpdate">if set to <c>true</c> [is update].</param>
        /// <returns></returns>
        private List<SqlParameter> BuildPresentingProblemSpParams(ContactPresentingProblemModel presentingProblem, bool isUpdate)
        {
            var spParameters = new List<SqlParameter>();
            spParameters.Add(new SqlParameter("TransactionLogID", (object)presentingProblem.TransactionID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ModuleComponentID", (object)presentingProblem.ScreenID ?? DBNull.Value));
            if (isUpdate)
                spParameters.Add(new SqlParameter("ContactPresentingProblemID", presentingProblem.ContactPresentingProblemID));

            spParameters.Add(new SqlParameter("ContactID", presentingProblem.ContactID));
            spParameters.Add(new SqlParameter("PresentingProblemTypeID", (object)presentingProblem.PresentingProblemTypeID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("EffectiveDate", (object)presentingProblem.EffectiveDate ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ExpirationDate", (object)presentingProblem.ExpirationDate ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ModifiedOn", (object)presentingProblem.ModifiedOn ?? DateTime.Now));

           
            

            return spParameters;
        }

        #endregion Helpers
    }
}
