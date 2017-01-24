using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Xml;
using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using Axis.Model.Registration;

namespace Axis.DataProvider.Registration.Consent
{
    public class ConsentDataProvider : IConsentDataProvider
    {
        #region Class Variables

        IUnitOfWork unitOfWork = null;

        #endregion
        
        #region Constructors

        public ConsentDataProvider(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        #endregion

        #region Public Methods

        public Response<ConsentModel> AddConsentSignature(ConsentModel consent)
        {
            var consentRepository = unitOfWork.GetRepository<ConsentModel>(SchemaName.ESignature);
            SqlParameter contactIdParam = new SqlParameter("ContactID", consent.ContactId);
            SqlParameter signatureBlobParam = new SqlParameter("SignatureBlob", consent.SignatureBlob);
            signatureBlobParam.DbType = DbType.Binary;
            SqlParameter isActiveParam = new SqlParameter("IsActive", consent.IsActive);
            SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", consent.ModifiedOn ?? DateTime.Now);
            List<SqlParameter> procParams = new List<SqlParameter>() { contactIdParam, signatureBlobParam, isActiveParam, modifiedOnParam };

            return unitOfWork.EnsureInTransaction<Response<ConsentModel>>(consentRepository.ExecuteNQStoredProc, "usp_AddContactSignatures", procParams, forceRollback: consent.ForceRollback.GetValueOrDefault(false));
        }

        public Response<ConsentModel> GetConsentSignature(long contactId)
        {
        
            var consentRepository = unitOfWork.GetRepository<ConsentModel>(SchemaName.ESignature);
            SqlParameter contactIdParam = new SqlParameter("ContactID", contactId);
            List<SqlParameter> procParams = new List<SqlParameter>() { contactIdParam };
            var result = consentRepository.ExecuteStoredProc("usp_GetContactSignatures", procParams);

            return result;
        }

        #endregion
    }
}
