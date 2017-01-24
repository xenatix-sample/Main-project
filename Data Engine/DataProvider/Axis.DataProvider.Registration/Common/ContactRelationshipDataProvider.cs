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
    /// <seealso cref="Axis.DataProvider.Registration.Common.IContactRelationshipDataProvider" />
    public class ContactRelationshipDataProvider : IContactRelationshipDataProvider
    {
        #region initializations

        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactAddressDataProvider" /> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ContactRelationshipDataProvider(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region Public Methods

        /// <summary>
        /// Gets the contact Relationship.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Response<ContactRelationshipModel> GetContactRelationship(long contactID, long parentContactID, long? referralHeaderID = null)
        {
            var spParameters = new List<SqlParameter> {
                                    new SqlParameter("ContactID", contactID),
                                    new SqlParameter("ParentContactID", parentContactID),
                                    new SqlParameter("ReferralHeaderID", referralHeaderID == null ?(object)DBNull.Value : referralHeaderID)
            };

            var repository = _unitOfWork.GetRepository<ContactRelationshipModel>(SchemaName.Registration);
            return repository.ExecuteStoredProc("usp_GetContactRelationshipTypes", spParameters);
        }

        /// <summary>
        /// Adds the contact Relationship.
        /// </summary>
        /// <param name="contactRelationship">The contact Relationship.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Response<ContactRelationshipModel> AddContactRelationship(List<ContactRelationshipModel> contactRelationship)
        {
            var repository = _unitOfWork.GetRepository<ContactRelationshipModel>(SchemaName.Registration);

            var spParameters = BuildContactRelationshipSpParams(contactRelationship.FirstOrDefault(), false);
            return _unitOfWork.EnsureInTransaction<Response<ContactRelationshipModel>>(repository.ExecuteNQStoredProc, "usp_AddContactRelationshipType", spParameters, forceRollback: contactRelationship.Any(x => x.ForceRollback.GetValueOrDefault(false)), idResult: true);
        }

        /// <summary>
        /// Updates the contact Relationship.
        /// </summary>
        /// <param name="contactRelationship">The contact Relationship.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Response<ContactRelationshipModel> UpdateContactRelationship(List<ContactRelationshipModel> contactRelationship)
        {
            var repository = _unitOfWork.GetRepository<ContactRelationshipModel>(SchemaName.Registration);

            var spParameters = BuildContactRelationshipSpParams(contactRelationship.FirstOrDefault(), true);
            return _unitOfWork.EnsureInTransaction<Response<ContactRelationshipModel>>(repository.ExecuteNQStoredProc, "usp_UpdateContactRelationshipType", spParameters, forceRollback: contactRelationship.Any(x => x.ForceRollback.GetValueOrDefault(false)));
        }

        /// <summary>
        /// Deletes the contact Relationship.
        /// </summary>
        /// <param name="contactRelationshipID">The contact address identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Response<ContactRelationshipModel> DeleteContactRelationship(long contactRelationshipTypeID, DateTime modifiedOn)
        {
            List<SqlParameter> procsParameters = new List<SqlParameter> { new SqlParameter("contactRelationshipTypeID", contactRelationshipTypeID), new SqlParameter("ModifiedOn", modifiedOn) };
            var contactBenefitRepository = _unitOfWork.GetRepository<ContactRelationshipModel>(SchemaName.Registration);
            return contactBenefitRepository.ExecuteNQStoredProc("usp_DeleteContactRelationshipType", procsParameters);
        }

        #endregion Public Methods

        #region Helpers

        private List<SqlParameter> BuildContactRelationshipSpParams(ContactRelationshipModel contactRelationship, bool isUpdate)
        {
            var spParameters = new List<SqlParameter>();
            if (isUpdate)
                spParameters.Add(new SqlParameter("ContactRelationshipTypeID", contactRelationship.ContactRelationshipTypeID));

            spParameters.AddRange(new List<SqlParameter> {
                new SqlParameter("ParentContactID", (object)contactRelationship.ParentContactID),
                new SqlParameter("ContactID", (object)contactRelationship.ContactID),
                new SqlParameter("RelationshipTypeID", contactRelationship.@RelationshipTypeID),
                new SqlParameter("IsPolicyHolder", (object)contactRelationship.IsPolicyHolder ?? DBNull.Value),
                new SqlParameter("OtherRelationship", (object)contactRelationship.@OtherRelationship ?? DBNull.Value),
                new SqlParameter("EffectiveDate", (object)contactRelationship.EffectiveDate ?? DBNull.Value),
                new SqlParameter("ExpirationDate", (object)contactRelationship.ExpirationDate ?? DBNull.Value),
                new SqlParameter("ModifiedOn", contactRelationship.ModifiedOn ?? DateTime.UtcNow)
                });

            return spParameters;
        }

        #endregion Helpers
    }
}
