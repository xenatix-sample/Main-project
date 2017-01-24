using System;
using System.Linq;
using Axis.Model.Common;
using Axis.Model.ESignature;
using Axis.Plugins.Registration.Models;
using Axis.Plugins.ESignature.Models;

namespace Axis.Plugins.ESignature.Translator
{
    public static class ESignatureTranslator
    {
        public static EntitySignatureModel ToModel(this EntitySignatureViewModel model)
        {
            if (model == null)
                return null;

            var entity = new EntitySignatureModel
            {
                EntitySignatureId = model.EntitySignatureId,
                SignatureId = model.SignatureId,
                SignatureTypeId = model.SignatureTypeId,
                EntityId = model.EntityId,
                EntityTypeId = model.EntityTypeId,
                SignatureBlob = Convert.FromBase64String(model.SignatureBlob),
                IsActive = model.IsActive,
                ModifiedOn = model.ModifiedOn,
                ModifiedBy = model.ModifiedBy,
                CredentialID = model.CredentialID,
                ForceRollback = model.ForceRollback
            };

            return entity;
        }

        public static EntitySignatureViewModel ToViewModel(this EntitySignatureModel entity)
        {
            if (entity == null)
                return null;

            var model = new EntitySignatureViewModel
            {
                EntitySignatureId = entity.EntitySignatureId,
                SignatureId = entity.SignatureId,
                SignatureTypeId = entity.SignatureTypeId,
                EntityId = entity.EntityId,
                EntityTypeId = entity.EntityTypeId,
                SignatureBlob = Convert.ToBase64String(entity.SignatureBlob),
                IsActive = entity.IsActive,
                ModifiedOn = entity.ModifiedOn,
                ModifiedBy = entity.ModifiedBy,
                CredentialID = entity.CredentialID,
                ForceRollback = entity.ForceRollback
            };

            return model;
        }

        public static Response<EntitySignatureViewModel> ToViewModel(this Response<EntitySignatureModel> entity)
        {
            if (entity == null)
                return null;

            var viewModel = entity.CloneResponse<EntitySignatureViewModel>();

            viewModel.DataItems = entity.DataItems.Select(x => x.ToViewModel()).ToList();

            return viewModel;
        }
    }
}
