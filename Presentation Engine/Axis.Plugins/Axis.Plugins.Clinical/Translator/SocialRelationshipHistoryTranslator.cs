using Axis.Model.Clinical;
using Axis.Model.Common;
using System.Collections.Generic;

namespace Axis.Plugins.Clinical
{
    public static class SocialRelationshipHistoryTranslator
    {
        /// <summary>
        /// To the ViewModel
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static SocialRelationshipHistoryViewModel ToViewModel(this SocialRelationshipHistoryModel entity)
        {
            if (entity == null)
                return null;

            var model = new SocialRelationshipHistoryViewModel
            {
                ContactID = entity.ContactID,
                EncounterID = entity.EncounterID,
                TakenBy = entity.TakenBy,
                TakenTime = entity.TakenTime,
                SocialRelationshipDetailID = entity.SocialRelationshipDetailID,
                SocialRelationshipID = entity.SocialRelationshipID,
                FamilyRelationshipID = entity.FamilyRelationshipID,
                ChildhoodHistory = entity.ChildhoodHistory,
                RelationShipHistory = entity.RelationShipHistory,
                FamilyHistory = entity.FamilyHistory,
                IsDetailsDirty = entity.IsDetailsDirty,
                IsSocialRelationshipDirty = entity.IsSocialRelationshipDirty,
                ReviewedNoChanges = entity.ReviewedNoChanges,
                IsFamilyRelationshipDirty = entity.IsFamilyRelationshipDirty,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<SocialRelationshipHistoryViewModel> ToViewModel(this Response<SocialRelationshipHistoryModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<SocialRelationshipHistoryViewModel>();
            var listModel = new List<SocialRelationshipHistoryViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(SocialRelationshipHistoryModel relnHist)
                {
                    var transformedModel = relnHist.ToViewModel();
                    listModel.Add(transformedModel);
                });

                model.DataItems = listModel;
            }

            return model;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static SocialRelationshipHistoryModel ToModel(this SocialRelationshipHistoryViewModel model)
        {
            if (model == null)
                return null;

            var entity = new SocialRelationshipHistoryModel
            {
                ContactID = model.ContactID,
                EncounterID = model.EncounterID,
                TakenBy = model.TakenBy,
                TakenTime = model.TakenTime,
                SocialRelationshipDetailID = model.SocialRelationshipDetailID,
                SocialRelationshipID = model.SocialRelationshipID,
                FamilyRelationshipID = model.FamilyRelationshipID,
                ChildhoodHistory = model.ChildhoodHistory,
                RelationShipHistory = model.RelationShipHistory,
                FamilyHistory = model.FamilyHistory,
                IsDetailsDirty = model.IsDetailsDirty,
                IsSocialRelationshipDirty = model.IsSocialRelationshipDirty,
                ReviewedNoChanges = model.ReviewedNoChanges,
                IsFamilyRelationshipDirty = model.IsFamilyRelationshipDirty,
                ModifiedOn = model.ModifiedOn
            };

            return entity;
        }

    }
}
