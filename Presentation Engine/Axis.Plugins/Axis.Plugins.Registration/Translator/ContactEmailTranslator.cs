using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Plugins.Registration.Models;
using Axis.Model.Email;
using Axis.Plugins.Registration.Model;

namespace Axis.Plugins.Registration.Translator
{
    public static class ContactEmailTranslator
    {

        public static ContactEmailModel ToModel(this ContactEmailViewModel model)
        {
            if (model == null)
                return null;

            var entity = new ContactEmailModel
            {
                Email = model.Email,
                EmailID = model.EmailID,
                EmailPermissionID = model.EmailPermissionID,
                IsPrimary = model.IsPrimary,
                EffectiveDate = model.EffectiveDate,
                ExpirationDate = model.ExpirationDate,
                IsActive = model.IsActive,
                ModifiedOn = model.ModifiedOn,
                ModifiedBy = model.ModifiedBy,
                ForceRollback = model.ForceRollback,
                ContactID=model.ContactID,
                ContactEmailID=model.ContactEmailID
            };

            return entity;
        }

        public static ContactEmailViewModel ToViewModel(this ContactEmailModel entity)
        {
            if (entity == null)
                return null;

            var model = new ContactEmailViewModel
            {
                Email = entity.Email,
                EmailID = entity.EmailID,
                EmailPermissionID = entity.EmailPermissionID,
                IsPrimary = entity.IsPrimary,
                EffectiveDate = entity.EffectiveDate,
                ExpirationDate = entity.ExpirationDate,
                IsActive = entity.IsActive,
                ModifiedOn = entity.ModifiedOn,
                ModifiedBy = entity.ModifiedBy,
                ForceRollback = entity.ForceRollback,
                ContactID=entity.ContactID,
                ContactEmailID = entity.ContactEmailID
            };

            return model;
        }

        public static Response<ContactEmailViewModel> ToViewModel(this Response<ContactEmailModel> entity)
        {
            if (entity == null)
                return null;
            var model = entity.CloneResponse<ContactEmailViewModel>();
            var lstViewModel = new List<ContactEmailViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(ContactEmailModel m)
                {
                    var transformedModel = m.ToViewModel();
                    lstViewModel.Add(transformedModel);
                });
                model.DataItems = lstViewModel;
            }
            return model;
        }

        public static Response<ContactEmailViewModel> ToModel(this Response<ContactEmailModel> entity)
        {
            if (entity == null)
                return null;

            var dataItems = new List<ContactEmailViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(ContactEmailModel emailModal)
                {
                    var transformedModel = emailModal.ToViewModel();
                    dataItems.Add(transformedModel);
                });
            }
            else
                dataItems = null;

            var model = new Response<ContactEmailViewModel>
            {
                ResultCode = entity.ResultCode,
                ResultMessage = entity.ResultMessage,
                DataItems = dataItems,
                RowAffected = entity.RowAffected,
                AdditionalResult = entity.AdditionalResult
            };

            return model;
        }

        public static List<ContactEmailModel> ToModel(this List<ContactEmailViewModel> model)
        {
            if (model == null)
                return null;

            var retAddress = new List<ContactEmailModel>();


            model.ForEach(delegate(ContactEmailViewModel address)
            {
                var transformedModel = address.ToModel();
                retAddress.Add(transformedModel);
            });
            return retAddress;
        }
    
    }
}
