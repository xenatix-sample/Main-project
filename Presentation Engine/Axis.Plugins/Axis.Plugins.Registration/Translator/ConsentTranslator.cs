using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Plugins.Registration.Models;

namespace Axis.Plugins.Registration.Translator
{
    public static class ConsentTranslator
    {
        public static ConsentModel ToModel(this ConsentViewModel model)
        {
            if (model == null)
                return null;

            var entity = new ConsentModel
            {
                SignatureId = model.SignatureId,
                SignatureBlob = Convert.FromBase64String(model.SignatureBlob),
                ContactId = model.ContactId,
                AuthorizedBy = model.AuthorizedBy,
                ContactName = model.ContactName,
                ContactDateofBirth = model.ContactDateofBirth,
                IsActive = model.IsActive,
                ModifiedOn = model.ModifiedOn,
                ModifiedBy = model.ModifiedBy,
                ForceRollback = model.ForceRollback
            };

            return entity;
        }

        public static ConsentViewModel ToViewModel(this ConsentModel entity)
        {
            if (entity == null)
                return null;

            var model = new ConsentViewModel
            {
                SignatureId = entity.SignatureId,
                SignatureBlob = entity.SignatureBlob == null ? string.Empty : Convert.ToBase64String(entity.SignatureBlob),
                ContactId = entity.ContactId,
                AuthorizedBy = entity.AuthorizedBy,
                ContactName = entity.ContactName,
                ContactDateofBirth = entity.ContactDateofBirth,
                IsActive = entity.IsActive,
                ModifiedOn = entity.ModifiedOn,
                ModifiedBy = entity.ModifiedBy
            };

            return model;
        }

        public static Response<ConsentViewModel> ToModel(this Response<ConsentModel> entity)
        {
            if (entity == null)
                return null;

            var dataItems = new List<ConsentViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(ConsentModel consentModel)
                {
                    var transformedModel = consentModel.ToViewModel();
                    dataItems.Add(transformedModel);
                });
            }
            else
                dataItems = null;

            var model = new Response<ConsentViewModel>
            {
                ResultCode = entity.ResultCode,
                ResultMessage = entity.ResultMessage,
                DataItems = dataItems,
                RowAffected = entity.RowAffected,
                AdditionalResult = entity.AdditionalResult
            };

            return model;
        }
    }
}
