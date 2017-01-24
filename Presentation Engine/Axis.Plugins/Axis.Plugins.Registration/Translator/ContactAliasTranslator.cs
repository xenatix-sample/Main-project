using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Plugins.Registration.Model;
using System.Collections.Generic;

namespace Axis.Plugins.Registration.Translator
{
    /// <summary>
    ///
    /// </summary>
    public static class ContactAliasTranslator
    {
        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static ContactAliasModel ToModel(this ContactAliasViewModel model)
        {
            if (model == null)
                return null;

            var entity = new ContactAliasModel
            {
                ContactAliasID = model.ContactAliasID,
                ContactID = model.ContactID,
                AliasFirstName = model.AliasFirstName,
                AliasMiddle = model.AliasMiddle,
                AliasLastName = model.AliasLastName,
                SuffixID = model.SuffixID,
                IsActive = model.IsActive,
                ModifiedBy = model.ModifiedBy,
                ModifiedOn = model.ModifiedOn,
                ForceRollback = model.ForceRollback,
                ScreenID=model.ScreenID,
                TransactionID=model.TransactionID
            };

            return entity;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static ContactAliasViewModel ToViewModel(this ContactAliasModel entity)
        {
            if (entity == null)
                return null;

            var model = new ContactAliasViewModel
            {
                ContactAliasID = entity.ContactAliasID,
                ContactID = entity.ContactID,
                AliasFirstName = entity.AliasFirstName,
                AliasMiddle = entity.AliasMiddle,
                AliasLastName = entity.AliasLastName,
                SuffixID = entity.SuffixID,
                IsActive = entity.IsActive,
                ModifiedBy = entity.ModifiedBy,
                ModifiedOn = entity.ModifiedOn,
                ForceRollback = entity.ForceRollback,
                ScreenID=entity.ScreenID,
                TransactionID=entity.TransactionID
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<ContactAliasViewModel> ToViewModel(this Response<ContactAliasModel> entity)
        {
            if (entity == null)
                return null;
            var model = entity.CloneResponse<ContactAliasViewModel>();
            var lstViewModel = new List<ContactAliasViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(ContactAliasModel m)
                {
                    var transformedModel = m.ToViewModel();
                    lstViewModel.Add(transformedModel);
                });
                model.DataItems = lstViewModel;
            }
            return model;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<ContactAliasViewModel> ToModel(this Response<ContactAliasModel> entity)
        {
            if (entity == null)
                return null;

            var dataItems = new List<ContactAliasViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(ContactAliasModel contactAlias)
                {
                    var transformedModel = contactAlias.ToViewModel();
                    dataItems.Add(transformedModel);
                });
            }
            else
                dataItems = null;

            var model = new Response<ContactAliasViewModel>
            {
                ResultCode = entity.ResultCode,
                ResultMessage = entity.ResultMessage,
                DataItems = dataItems,
                RowAffected = entity.RowAffected,
                AdditionalResult = entity.AdditionalResult
            };

            return model;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static List<ContactAliasModel> ToModel(this List<ContactAliasViewModel> model)
        {
            if (model == null)
                return null;

            var contactAliasModel = new List<ContactAliasModel>();

            model.ForEach(delegate(ContactAliasViewModel contactAlias)
            {
                var transformedModel = contactAlias.ToModel();
                contactAliasModel.Add(transformedModel);
            });
            return contactAliasModel;
        }
    }
}