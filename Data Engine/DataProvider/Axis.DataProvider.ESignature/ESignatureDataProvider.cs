using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using Axis.Model.ESignature;

namespace Axis.DataProvider.ESignature
{
    public class ESignatureDataProvider : IESignatureDataProvider
    {
        #region Class Variables

        private IUnitOfWork unitOfWork = null;

        #endregion

        #region Constructors

        public ESignatureDataProvider(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        #endregion

        #region Public Methods

        public Response<DocumentEntitySignatureModel> SaveDocumentSignature(DocumentEntitySignatureModel documentSignature)
        {
            var signatureRepository = unitOfWork.GetRepository<DocumentEntitySignatureModel>(SchemaName.ESignature);
            if ((documentSignature.SignatureID == null || documentSignature.SignatureID == 0) && documentSignature.SignatureBlob != null)
            {
                documentSignature.SignatureID = AddSignatureFromBlob(documentSignature.SignatureBlob, documentSignature.IsActive, documentSignature.ModifiedOn).ID; 
            }

            List<SqlParameter> procParams = new List<SqlParameter>()
            {
                new SqlParameter("DocumentID", documentSignature.DocumentId),
                new SqlParameter("DocumentTypeID", documentSignature.DocumentTypeId),
                new SqlParameter("EntitySignatureID", (object)documentSignature.EntitySignatureId ?? DBNull.Value),
                new SqlParameter("EntityID", (object)documentSignature.EntityId  ?? DBNull.Value),
                new SqlParameter("EntityName", (object)documentSignature.EntityName  ?? DBNull.Value),
                new SqlParameter("EntityTypeID", (object)documentSignature.EntityTypeId  ?? DBNull.Value),
                new SqlParameter("SignatureID", (object)documentSignature.SignatureID ?? DBNull.Value), 
                new SqlParameter("CredentialID", (object)documentSignature.CredentialID ?? DBNull.Value),
                new SqlParameter("IsActive", documentSignature.IsActive ?? true),
                new SqlParameter("ModifiedOn", documentSignature.ModifiedOn ?? DateTime.Now)
            };
            return unitOfWork.EnsureInTransaction<Response<DocumentEntitySignatureModel>>(signatureRepository.ExecuteNQStoredProc, "usp_SaveDocumentEntitySignature", procParams, forceRollback: documentSignature.ForceRollback.GetValueOrDefault(false));
        }

        public Response<DocumentEntitySignatureModel> GetDocumentSignatures(long documentId, int documentTypeId)
        {
            var signatureRepository = unitOfWork.GetRepository<DocumentEntitySignatureModel>(SchemaName.ESignature);

            List<SqlParameter> procParams = new List<SqlParameter>()
            {
                new SqlParameter("DocumentID", documentId),
                new SqlParameter("DocumentTypeID", documentTypeId)
            };

            return signatureRepository.ExecuteStoredProc("usp_GetDocumentEntitySignatures", procParams);
        }


        public Response<EntitySignatureModel> GetEntitySignature(long entityId, int entityTypeId, int? signatureTypeId)
        {
            var repo = unitOfWork.GetRepository<EntitySignatureModel>(SchemaName.ESignature);

            List<SqlParameter> procParams = new List<SqlParameter>()
            {
                new SqlParameter("EntityID", entityId),
                new SqlParameter("EntityTypeID", entityTypeId),
                new SqlParameter("SignatureTypeId", (object)signatureTypeId ?? DBNull.Value)
            };

            return repo.ExecuteStoredProc("usp_GetEntitySingatures", procParams);
        }

        public Response<DocumentEntitySignatureModel> AddDocumentEntitySignatures(DocumentEntitySignatureModel documentEntitySignature)
        {
            var signatureRepository = unitOfWork.GetRepository<DocumentEntitySignatureModel>(SchemaName.ESignature);
            List<SqlParameter> procParams = new List<SqlParameter>()
            {
                new SqlParameter("DocumentID", documentEntitySignature.DocumentId),
                new SqlParameter("EntitySignatureID", documentEntitySignature.EntitySignatureId),
                new SqlParameter("DocumentTypeID", documentEntitySignature.DocumentTypeId),
                new SqlParameter("IsActive", documentEntitySignature.IsActive ?? true),
                new SqlParameter("ModifiedOn", documentEntitySignature.ModifiedOn ?? DateTime.Now)                
            };
            return signatureRepository.ExecuteNQStoredProc("usp_AddDocumentEntitySignatures", procParams, false, true);
        }

        public Response<EntitySignatureModel> AddEntitySignature(EntitySignatureModel entitySignature)
        {
            var signatureRepository = unitOfWork.GetRepository<EntitySignatureModel>(SchemaName.ESignature);
            List<SqlParameter> procParams = new List<SqlParameter>()
            {
                new SqlParameter("EntityID", entitySignature.EntityId),
                new SqlParameter("SignatureID", entitySignature.SignatureId),
                new SqlParameter("EntityTypeID", entitySignature.EntityTypeId),
                new SqlParameter("SignatureTypeID", (object)entitySignature.SignatureTypeId ?? DBNull.Value),
                new SqlParameter("CredentialID", (object)entitySignature.CredentialID ?? DBNull.Value),
                new SqlParameter("IsActive", entitySignature.IsActive ?? true),
                new SqlParameter("ModifiedOn", entitySignature.ModifiedOn ?? DateTime.Now)                
            };
            return signatureRepository.ExecuteNQStoredProc("usp_AddEntitySignatures", procParams, false, true);
        }

        public Response<EntitySignatureModel> UpdateEntitySignature(EntitySignatureModel entitySignature)
        {
            var signatureRepository = unitOfWork.GetRepository<EntitySignatureModel>(SchemaName.ESignature);
            List<SqlParameter> procParams = new List<SqlParameter>()
            {
                new SqlParameter("EntitySignatureID", entitySignature.EntitySignatureId),
                new SqlParameter("EntityID", entitySignature.EntityId),
                new SqlParameter("SignatureID", entitySignature.SignatureId),
                new SqlParameter("EntityTypeID", entitySignature.EntityTypeId),
                new SqlParameter("SignatureTypeID", (object)entitySignature.SignatureTypeId ?? DBNull.Value),
                new SqlParameter("CredentialID", (object)entitySignature.CredentialID ?? DBNull.Value),
                new SqlParameter("ModifiedOn", entitySignature.ModifiedOn ?? DateTime.Now)                
            };
            return signatureRepository.ExecuteNQStoredProc("usp_UpdateEntitySingatures", procParams, false, false);
        }

        public Response<EntitySignatureModel> AddSignature(EntitySignatureModel entitySignature)
        {
            return AddSignatureFromBlob(entitySignature.SignatureBlob, entitySignature.IsActive, entitySignature.ModifiedOn);
        }

        #endregion

        private Response<EntitySignatureModel> AddSignatureFromBlob(byte[] signatureBLOB, bool? isActive, DateTime? modifiedOn)
        {
            var signatureRepository = unitOfWork.GetRepository<EntitySignatureModel>(SchemaName.ESignature);
            SqlParameter blobParam = new SqlParameter("SignatureBLOB", signatureBLOB);
            blobParam.DbType = DbType.Binary;
            List<SqlParameter> procParams = new List<SqlParameter>()
            {
                blobParam,
                new SqlParameter("IsActive", isActive ?? true),
                new SqlParameter("ModifiedOn", modifiedOn ?? DateTime.Now)                
            };
            return signatureRepository.ExecuteNQStoredProc("usp_AddSignatures", procParams, false, true);
        }
    }
}
