using Axis.Model.Clinical.SocialRelationship;
using Axis.Model.Common;
using Axis.Plugins.Clinical.Models.SocialRelationship;
using System.Collections.Generic;

namespace Axis.Plugins.Clinical.Translator
{
    /// <summary>
    ///
    /// </summary>
    public static class SocialRelationshipTranslator
    {
        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static SocialRelationshipViewModel ToViewModel(this SocialRelationshipModel entity)
        {
            if (entity == null)
                return null;

            var model = new SocialRelationshipViewModel
            {
                SocialRelationshipID = entity.SocialRelationshipID,
                ContactID = entity.ContactID,
                TakenBy = entity.TakenBy,
                TakenTime = entity.TakenTime,
                EncounterID = entity.EncounterID,
                ChildhoodHistory = entity.ChildhoodHistory,
                RelationShipHistory = entity.RelationShipHistory,
                FamilyHistory = entity.FamilyHistory,
                ForceRollback = entity.ForceRollback,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<SocialRelationshipViewModel> ToViewModel(this Response<SocialRelationshipModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<SocialRelationshipViewModel>();
            var rosList = new List<SocialRelationshipViewModel>();

            if (entity.DataItems == null) return model;

            entity.DataItems.ForEach(delegate(SocialRelationshipModel ros)
            {
                var transformModel = ros.ToViewModel();
                rosList.Add(transformModel);
            });
            model.DataItems = rosList;

            return model;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static SocialRelationshipModel ToModel(this SocialRelationshipViewModel model)
        {
            if (model == null)
                return null;
            var entity = new SocialRelationshipModel
            {
                SocialRelationshipID = model.SocialRelationshipID,
                ContactID = model.ContactID,
                TakenBy = model.TakenBy,
                TakenTime = model.TakenTime,
                EncounterID = model.EncounterID,
                ChildhoodHistory = model.ChildhoodHistory,
                RelationShipHistory = model.RelationShipHistory,
                FamilyHistory = model.FamilyHistory,
                ForceRollback = model.ForceRollback,
                ModifiedOn = model.ModifiedOn
            };
            return entity;
        }
    }
}