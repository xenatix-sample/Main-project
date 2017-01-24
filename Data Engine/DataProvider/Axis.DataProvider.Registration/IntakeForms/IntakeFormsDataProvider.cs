using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using Axis.Model.Registration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Axis.DataProvider.Registration
{
    public class IntakeFormsDataProvider : IIntakeFormsDataProvider
    {
         #region initializations

        private IUnitOfWork unitOfWork = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="IntakeFormsDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public IntakeFormsDataProvider(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality


        /// <summary>
        /// Gets the intake form.
        /// </summary>
        /// <param name="contactFormsID">The intake form identifier.</param>
        /// <returns></returns>
        public Response<FormsModel> GetIntakeForm(long contactFormsID)
        {
            var IntakeFormsRepository = unitOfWork.GetRepository<FormsModel>(SchemaName.Registration);
            var procParams = new List<SqlParameter> { new SqlParameter("ContactFormsID", contactFormsID) };

            return IntakeFormsRepository.ExecuteStoredProc("usp_GetContactForms", procParams);
        }

        /// <summary>
        /// Gets the Forms.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns>Response&lt;FormsModel&gt;.</returns>
        public Response<FormsModel> GetIntakeFormsByContactID(long contactID)
        {
            var IntakeFormsRepository = unitOfWork.GetRepository<FormsModel>(SchemaName.Registration);
            var procParams = new List<SqlParameter> { new SqlParameter("ContactID", contactID) };

            return IntakeFormsRepository.ExecuteStoredProc("usp_GetContactFormsByContactID", procParams); 
        }

        /// <summary>
        /// Adds the Forms.
        /// </summary>
        /// <param name="FormsModel">The Forms model.</param>
        /// <returns>Response&lt;FormsModel&gt;.</returns>
        public Response<FormsModel> AddIntakeForms(FormsModel FormsModel)
        {
            var IntakeFormsRepository = unitOfWork.GetRepository<FormsModel>(SchemaName.Registration);
            var procParams = BuildSpParams(FormsModel);

            return unitOfWork.EnsureInTransaction(IntakeFormsRepository.ExecuteNQStoredProc, "usp_AddContactForms", procParams, idResult: true,
                forceRollback: FormsModel.ForceRollback.GetValueOrDefault(false));
        }

        /// <summary>
        /// Updates the Forms.
        /// </summary>
        /// <param name="FormsModel">The Forms model.</param>
        /// <returns>Response&lt;FormsModel&gt;.</returns>
        public Response<FormsModel> UpdateIntakeForms(FormsModel FormsModel)
        {
            var IntakeFormsRepository = unitOfWork.GetRepository<FormsModel>(SchemaName.Registration);
            var procParams = BuildSpParams(FormsModel);

            return unitOfWork.EnsureInTransaction(IntakeFormsRepository.ExecuteNQStoredProc, "usp_UpdateContactForms", procParams,  
                forceRollback: FormsModel.ForceRollback.GetValueOrDefault(false));
        }

        /// <summary>
        /// Deletes the Forms.
        /// </summary>
        /// <param name="contactFormsID">The intake Forms identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns>Response&lt;FormsModel&gt;.</returns>
        public Response<FormsModel> DeleteIntakeForms(long contactFormsID, DateTime modifiedOn)
        {
            var IntakeFormsRepository = unitOfWork.GetRepository<FormsModel>(SchemaName.Registration);
            var procParams = new List<SqlParameter> { new SqlParameter("ContactFormsID", contactFormsID), new SqlParameter("ModifiedOn", modifiedOn) };
            return unitOfWork.EnsureInTransaction(IntakeFormsRepository.ExecuteNQStoredProc, "usp_DeleteContactForms", procParams); 
        }

        #endregion exposed functionality'



        #region Helpers

        private List<SqlParameter> BuildSpParams(FormsModel FormsModel)
        {
            var spParameters = new List<SqlParameter>();

            if (FormsModel.ContactFormsID > 0) // Only in case of Update
                spParameters.Add(new SqlParameter("ContactFormsID", FormsModel.ContactFormsID));

            spParameters.AddRange(new List<SqlParameter> {
                new SqlParameter("ContactID", (object)FormsModel.ContactID),
                new SqlParameter("UserID", (object)FormsModel.UserID ?? DBNull.Value),
                new SqlParameter("AssessmentID", (object)FormsModel.AssessmentID ?? DBNull.Value),
                new SqlParameter("ResponseID", (object)FormsModel.ResponseID ?? DBNull.Value),
                new SqlParameter("DocumentStatusID", (object)FormsModel.DocumentStatusID),
                new SqlParameter("ModifiedOn", FormsModel.ModifiedOn ?? DateTime.Now)
                });
            return spParameters;
        }

        #endregion

    }
}
