using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using Axis.Model.Registration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Axis.DataProvider.Registration
{
    public class LettersDataProvider : ILettersDataProvider
    {
        #region initializations

        private IUnitOfWork unitOfWork = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="LettersDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public LettersDataProvider(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Gets the letters.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns>Response&lt;LettersModel&gt;.</returns>
        public Response<LettersModel> GetLetters(long contactID)
        {
            var lettersRepository = unitOfWork.GetRepository<LettersModel>(SchemaName.Registration);
            var procParams = new List<SqlParameter> { new SqlParameter("ContactID", contactID) };

            return lettersRepository.ExecuteStoredProc("usp_GetContactLettersByContactID", procParams); 
        }

        /// <summary>
        /// Adds the letters.
        /// </summary>
        /// <param name="lettersModel">The letters model.</param>
        /// <returns>Response&lt;LettersModel&gt;.</returns>
        public Response<LettersModel> AddLetters(LettersModel lettersModel)
        {
            var lettersRepository = unitOfWork.GetRepository<LettersModel>(SchemaName.Registration);
            var procParams = BuildSpParams(lettersModel);

            return unitOfWork.EnsureInTransaction(lettersRepository.ExecuteNQStoredProc, "usp_AddContactLetters", procParams, idResult: true,
                forceRollback: lettersModel.ForceRollback.GetValueOrDefault(false));
        }

        /// <summary>
        /// Updates the letters.
        /// </summary>
        /// <param name="lettersModel">The letters model.</param>
        /// <returns>Response&lt;LettersModel&gt;.</returns>
        public Response<LettersModel> UpdateLetters(LettersModel lettersModel)
        {
            var lettersRepository = unitOfWork.GetRepository<LettersModel>(SchemaName.Registration);
            var procParams = BuildSpParams(lettersModel);

            return unitOfWork.EnsureInTransaction(lettersRepository.ExecuteNQStoredProc, "usp_UpdateContactLetters", procParams,  
                forceRollback: lettersModel.ForceRollback.GetValueOrDefault(false));
        }

        /// <summary>
        /// Deletes the letters.
        /// </summary>
        /// <param name="contactLettersID">The intake letters identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns>Response&lt;LettersModel&gt;.</returns>
        public Response<LettersModel> DeleteLetters(long contactLettersID, DateTime modifiedOn)
        {
            var lettersRepository = unitOfWork.GetRepository<LettersModel>(SchemaName.Registration);
            var procParams = new List<SqlParameter> { new SqlParameter("ContactLettersID", contactLettersID), new SqlParameter("ModifiedOn", modifiedOn) };
            return unitOfWork.EnsureInTransaction(lettersRepository.ExecuteNQStoredProc, "usp_DeleteContactLetters", procParams); 
        }

        #endregion exposed functionality'



        #region Helpers

        private List<SqlParameter> BuildSpParams(LettersModel lettersModel)
        {
            var spParameters = new List<SqlParameter>();

            if (lettersModel.ContactLettersID > 0) // Only in case of Update
                spParameters.Add(new SqlParameter("ContactLettersID", lettersModel.ContactLettersID));

            spParameters.AddRange(new List<SqlParameter> {
                new SqlParameter("ContactID", (object)lettersModel.ContactID),
                new SqlParameter("SentDate", (object)lettersModel.SentDate ?? DBNull.Value),
                new SqlParameter("UserID", (object)lettersModel.UserID ?? DBNull.Value),
                new SqlParameter("AssessmentID", (object)lettersModel.AssessmentID ?? DBNull.Value),
                new SqlParameter("ResponseID", (object)lettersModel.ResponseID ?? DBNull.Value),
                new SqlParameter("Comments", (object)lettersModel.Comments?? DBNull.Value),
                new SqlParameter("LetterOutcomeID", (object)lettersModel.LetterOutcomeID ?? DBNull.Value),
                new SqlParameter("ModifiedOn", lettersModel.ModifiedOn ?? DateTime.Now)
                });
            return spParameters;
        }

        #endregion
    }
}
