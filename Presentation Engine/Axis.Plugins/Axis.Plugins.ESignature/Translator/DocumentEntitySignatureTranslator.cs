using System;
using System.Linq;
using Axis.Model.Common;
using Axis.Model.ESignature;
using Axis.Plugins.Registration.Models;

namespace Axis.Plugins.ESignature.Translator
{
    public static class ConsentTranslator
    {
        public static DocumentEntitySignatureModel ToModel(this DocumentEntitySignatureViewModel model)
        {
            if (model == null)
                return null;

            var entity = new DocumentEntitySignatureModel
            {
                DocumentId = model.DocumentId,
                DocumentTypeId = model.DocumentTypeId,
                EntitySignatureId = model.EntitySignatureId,
                EntityId = model.EntityId,
                EntityName = model.EntityName,
                EntityTypeId = model.EntityTypeId,
                SignatureBlob = (model.SignatureBlob != null) ? Convert.FromBase64String(model.SignatureBlob) : null,
                IsActive = model.IsActive,
                ModifiedOn = model.ModifiedOn,
                ModifiedBy = model.ModifiedBy,
                CredentialID = model.CredentialID,
                SignatureID = model.SignatureID,
                ForceRollback = model.ForceRollback
            };

            return entity;
        }

        public static DocumentEntitySignatureViewModel ToViewModel(this DocumentEntitySignatureModel entity)
        {
            if (entity == null)
                return null;

            var model = new DocumentEntitySignatureViewModel
            {
                DocumentId = entity.DocumentId,
                DocumentTypeId = entity.DocumentTypeId,
                EntitySignatureId = entity.EntitySignatureId,
                EntityId = entity.EntityId,
                EntityName = entity.EntityName,
                EntityTypeId = entity.EntityTypeId,
                SignatureBlob = (entity.SignatureBlob != null) ? Convert.ToBase64String(entity.SignatureBlob) : null,
                IsActive = entity.IsActive,
                ModifiedOn = entity.ModifiedOn,
                ModifiedBy = entity.ModifiedBy,
                CredentialID = entity.CredentialID,
                SignatureID = entity.SignatureID,
                ForceRollback = entity.ForceRollback
            };

            return model;
        }

        public static Response<DocumentEntitySignatureViewModel> ToViewModel(this Response<DocumentEntitySignatureModel> entity)
        {
            if (entity == null)
                return null;

            var viewModel = entity.CloneResponse<DocumentEntitySignatureViewModel>();

            viewModel.DataItems = entity.DataItems.Select(x => x.ToViewModel()).ToList();

            return viewModel;
        }
    }
}
