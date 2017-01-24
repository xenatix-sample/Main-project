using System.Collections.Generic;
using Axis.Data.Repository;
using Axis.Model.Common;
using Axis.Model.Registration;
using System.Data.SqlClient;
using Axis.Data.Repository.Schema;
using System.Linq;
using System;


namespace Axis.DataProvider.Registration.Common
{
    public class ContactClientIdentifierDataProvider : IContactClientIdentifierDataProvider
    {
        #region initializations

        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactAddressDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ContactClientIdentifierDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Gets the Client .
        /// </summary>
        /// <param name="contactID">The contact identifier</param>
        /// <param name="contactTypeID">Type of Contact</param>
        /// <returns></returns>
        public Response<ContactClientIdentifierModel> GetContactClientIdentifiers(long contactID)
        {
            var spParameters = new List<SqlParameter> { new SqlParameter("ContactID", contactID) };
            var repository = _unitOfWork.GetRepository<ContactClientIdentifierModel>(SchemaName.Registration);
            var results = repository.ExecuteStoredProc("usp_GetContactClientIdentifiers", spParameters);

            return results;
        }

        /// <summary>
        /// Adds the addresses.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <param name="addresses">The addresses.</param>
        /// <returns></returns>
        public Response<ContactClientIdentifierModel> AddUpdateContactClientIdentifiers(long contactID, List<ContactClientIdentifierModel> identifiers)
        {
            var result = new Response<ContactClientIdentifierModel>();
            result.ResultCode = 0;
            //prepare identifier collection to add/update/delete
            var identifiersToAdd = identifiers.Where(identifier => (identifier.ContactClientIdentifierID <= 0) && (identifier.IsActive ?? true)).ToList();
            var identifiersToUpdate = identifiers.Where(identifier => (identifier.ContactClientIdentifierID != 0) && (identifier.IsActive ?? true)).ToList();
            var identifiersToDelete = identifiers.Where(identifier => !(identifier.IsActive ?? true)).ToList();

            if (identifiersToAdd.Count() > 0)
            {
                foreach (var identifier in identifiersToAdd)
                {
                    AddIdentifier(contactID, identifier);
                }
            }

            if (identifiersToUpdate.Count() > 0)
            {
                foreach (var identifier in identifiersToUpdate)
                {
                    UpdateIdentifier(contactID, identifier);
                }
            }

            if (identifiersToDelete.Count() > 0)
            {
                foreach (var identifier in identifiersToDelete)
                {
                    DeleteIdentifier(identifier.ContactClientIdentifierID);
                }
            }

            return result;
        }

        #endregion exposed functionality

        #region helper methdods

        /// <summary>
        /// Adds the Identifier.
        /// </summary>
        /// <param name="identifierToAdd">The client identifier model to add.</param>
        /// <returns></returns>
        Response<ContactClientIdentifierModel> AddIdentifier(long contactID, ContactClientIdentifierModel identifierToAdd)
        {
            var repository = _unitOfWork.GetRepository<ContactClientIdentifierModel>(SchemaName.Registration);
            var spParameters = BuildParams(contactID, identifierToAdd);
            return _unitOfWork.EnsureInTransaction(repository.ExecuteNQStoredProc, "usp_AddContactClientIdentifier",
                spParameters, forceRollback: identifierToAdd.ForceRollback.GetValueOrDefault(), idResult: true);
        }

        /// <summary>
        /// Update the Identifier.
        /// </summary>
        /// <param name="referralID">The client identifier model to update.</param>
        /// <returns></returns>
        Response<ContactClientIdentifierModel> UpdateIdentifier(long contactID, ContactClientIdentifierModel identifierToUpdate)
        {
            var repository = _unitOfWork.GetRepository<ContactClientIdentifierModel>(SchemaName.Registration);
            var spParameters = BuildParams(contactID, identifierToUpdate);
            return _unitOfWork.EnsureInTransaction(repository.ExecuteNQStoredProc, "usp_UpdateContactClientIdentifier", spParameters, forceRollback: identifierToUpdate.ForceRollback.GetValueOrDefault());
        }

        /// <summary>
        /// Delete the Identifier.
        /// </summary>
        /// <param name="ContactClientIdentifierID">The client identifier.</param>
        /// <returns></returns>
        Response<ContactClientIdentifierModel> DeleteIdentifier(long ContactClientIdentifierID)
        {
            var procsParameters = new List<SqlParameter> { new SqlParameter("ContactClientIdentifierID", ContactClientIdentifierID), new SqlParameter("ModifiedOn", DateTime.Now) };
            var repository = _unitOfWork.GetRepository<ContactClientIdentifierModel>(SchemaName.Registration);
            return repository.ExecuteNQStoredProc("usp_DeleteContactClientIdentifier", procsParameters);
        }

        /// <summary>
        /// Builds the parameters.
        /// </summary>
        /// <param name="model">The client identifier model.</param>
        /// <returns></returns>
        private List<SqlParameter> BuildParams(long contactID, ContactClientIdentifierModel model)
        {
            var spParameters = new List<SqlParameter>();
            spParameters.Add(new SqlParameter("TransactionLogID", (object)model.TransactionID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ModuleComponentID", (object)model.ScreenID ?? DBNull.Value));
            if (model.ContactClientIdentifierID > 0)
                spParameters.Add(new SqlParameter("ContactClientIdentifierID", model.ContactClientIdentifierID));

            spParameters.Add(new SqlParameter("ContactID", contactID));
            spParameters.Add(new SqlParameter("ClientIdentifierTypeID", (object)model.ClientIdentifierTypeID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("AlternateID", (object)model.AlternateID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ExpirationReasonID", (object)model.ExpirationReasonID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("EffectiveDate", (object)model.EffectiveDate ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ExpirationDate", (object)model.ExpirationDate ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ModifiedOn", model.ModifiedOn ?? DateTime.Now));

            return spParameters;
        }

        #endregion helper methods
    }
}
