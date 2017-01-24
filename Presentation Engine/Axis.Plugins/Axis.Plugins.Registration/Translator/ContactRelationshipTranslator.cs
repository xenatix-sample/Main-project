using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Plugins.Registration.Model;
using System.Collections.Generic;

namespace Axis.Plugins.Registration.Translator
{
    /// <summary>
    ///
    /// </summary>
    public static class ContactRelationshipTranslator
    {
        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static ContactRelationshipModel ToModel(this ContactRelationshipViewModel model)
        {
            if (model == null)
                return null;

            var entity = new ContactRelationshipModel
            {
                ContactRelationshipTypeID = model.ContactRelationshipTypeID,
                ContactID = model.ContactID,
                ParentContactID = model.ParentContactID,
                RelationshipGroupID = model.RelationshipGroupID,
                RelationshipTypeID = model.RelationshipTypeID,
                IsPolicyHolder = model.IsPolicyHolder,
                OtherRelationship = model.OtherRelationship,
                EffectiveDate = model.EffectiveDate,
                ExpirationDate = model.ExpirationDate,
                IsActive = model.IsActive,
                ModifiedBy = model.ModifiedBy,
                ModifiedOn = model.ModifiedOn,
                LivingWithClientStatus = model.LivingWithClientStatus,
                IsCollateral = model.IsCollateral
            };

            return entity;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static ContactRelationshipViewModel ToViewModel(this ContactRelationshipModel entity)
        {
            if (entity == null)
                return null;

            var model = new ContactRelationshipViewModel
            {
                ContactRelationshipTypeID = entity.ContactRelationshipTypeID,
                ContactID = entity.ContactID,
                ParentContactID = entity.ParentContactID,
                RelationshipTypeID = entity.RelationshipTypeID,
                RelationshipGroupID = entity.RelationshipGroupID,
                IsPolicyHolder = entity.IsPolicyHolder,
                OtherRelationship = entity.OtherRelationship,
                EffectiveDate = entity.EffectiveDate,
                ExpirationDate = entity.ExpirationDate,
                IsActive = entity.IsActive,
                ModifiedBy = entity.ModifiedBy,
                ModifiedOn = entity.ModifiedOn,
                LivingWithClientStatus = entity.LivingWithClientStatus,
                IsCollateral = entity.IsCollateral
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<ContactRelationshipViewModel> ToViewModel(this Response<ContactRelationshipModel> entity)
        {
            if (entity == null)
                return null;
            var model = entity.CloneResponse<ContactRelationshipViewModel>();
            var lstViewModel = new List<ContactRelationshipViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(ContactRelationshipModel m)
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
        public static Response<ContactRelationshipViewModel> ToModel(this Response<ContactRelationshipModel> entity)
        {
            if (entity == null)
                return null;

            var dataItems = new List<ContactRelationshipViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(ContactRelationshipModel contactRelationship)
                {
                    var transformedModel = contactRelationship.ToViewModel();
                    dataItems.Add(transformedModel);
                });
            }
            else
                dataItems = null;

            var model = new Response<ContactRelationshipViewModel>
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
        public static List<ContactRelationshipModel> ToModel(this List<ContactRelationshipViewModel> model)
        {
            if (model == null)
                return null;

            var contactRelationshipModel = new List<ContactRelationshipModel>();

            model.ForEach(delegate(ContactRelationshipViewModel contactRelationship)
            {
                var transformedModel = contactRelationship.ToModel();
                contactRelationshipModel.Add(transformedModel);
            });
            return contactRelationshipModel;
        }
    }
}