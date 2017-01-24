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
    /// <seealso cref="Axis.DataProvider.Registration.Common.IContactAliasDataProvider" />
    public class ContactAliasDataProvider : IContactAliasDataProvider
    {
        #region initializations

        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactAddressDataProvider" /> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ContactAliasDataProvider(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region Public Methods

        /// <summary>
        /// Gets the contact alias.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Response<ContactAliasModel> GetContactAlias(long contactID)
        {
            var spParameters = new List<SqlParameter> { new SqlParameter("ContactID", contactID) };

            var repository = unitOfWork.GetRepository<ContactAliasModel>(SchemaName.Registration);
            var results = repository.ExecuteStoredProc("usp_GetContactAliases", spParameters);

            return results;
        }

        /// <summary>
        /// Adds the contact alias.
        /// </summary>
        /// <param name="contactAlias">The contact alias.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Response<ContactAliasModel> AddContactAlias(List<ContactAliasModel> contactAlias)
        {
            var repository = unitOfWork.GetRepository<ContactAliasModel>(SchemaName.Registration);

            var spParameters = BuildContactAliasSpParams(contactAlias.FirstOrDefault(), false);
            return unitOfWork.EnsureInTransaction<Response<ContactAliasModel>>(repository.ExecuteNQStoredProc, "usp_AddContactAlias", spParameters, forceRollback: contactAlias.Any(x => x.ForceRollback.GetValueOrDefault(false)), idResult: true);
        }

        /// <summary>
        /// Updates the contact alias.
        /// </summary>
        /// <param name="contactAlias">The contact alias.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Response<ContactAliasModel> UpdateContactAlias(List<ContactAliasModel> contactAlias)
        {
            var repository = unitOfWork.GetRepository<ContactAliasModel>(SchemaName.Registration);

            var spParameters = BuildContactAliasSpParams(contactAlias.FirstOrDefault(), true);
            return unitOfWork.EnsureInTransaction<Response<ContactAliasModel>>(repository.ExecuteNQStoredProc, "usp_UpdateContactAlias", spParameters, forceRollback: contactAlias.Any(x => x.ForceRollback.GetValueOrDefault(false)));
        }

        /// <summary>
        /// Deletes the contact alias.
        /// </summary>
        /// <param name="contactAliasID">The contact address identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Response<ContactAliasModel> DeleteContactAlias(long contactAliasID, DateTime modifiedOn)
        {
            List<SqlParameter> procsParameters = new List<SqlParameter> { new SqlParameter("contactAliasID", contactAliasID), new SqlParameter("ModifiedOn", modifiedOn) };
            var contactBenefitRepository = unitOfWork.GetRepository<ContactAliasModel>(SchemaName.Registration);
            Response<ContactAliasModel> spResults = new Response<ContactAliasModel>();
            spResults = contactBenefitRepository.ExecuteNQStoredProc("usp_DeleteContactAlias", procsParameters);
            return spResults;
        }

        #endregion Public Methods

        #region Helpers

        /// <summary>
        /// Builds the contact alias sp parameters.
        /// </summary>
        /// <param name="contactAlias">The contact alias.</param>
        /// <param name="isUpdate">if set to <c>true</c> [is update].</param>
        /// <returns></returns>
        private List<SqlParameter> BuildContactAliasSpParams(ContactAliasModel contactAlias, bool isUpdate)
        {
            var spParameters = new List<SqlParameter>();
            spParameters.Add(new SqlParameter("TransactionLogID", (object)contactAlias.TransactionID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ModuleComponentID", (object)contactAlias.ScreenID ?? DBNull.Value));
            if (isUpdate)
                spParameters.Add(new SqlParameter("ContactAliasID", contactAlias.ContactAliasID));
            spParameters.Add(new SqlParameter("ContactID", contactAlias.ContactID));
            spParameters.Add(new SqlParameter("AliasFirstName", (object)contactAlias.AliasFirstName ?? DBNull.Value));
            spParameters.Add(new SqlParameter("AliasMiddle", (object)contactAlias.AliasMiddle ?? DBNull.Value));
            spParameters.Add(new SqlParameter("AliasLastName", (object)contactAlias.AliasLastName ?? DBNull.Value));
            spParameters.Add(new SqlParameter("SuffixID", (object)contactAlias.SuffixID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("IsActive", (object)contactAlias.IsActive ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ModifiedOn", (object)contactAlias.ModifiedOn ?? DateTime.Now));

            return spParameters;
        }

        #endregion Helpers
    }
}
