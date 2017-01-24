using Axis.Model.Common;
using Axis.Model.ECI;
using Axis.Plugins.ECI.Models;
using System.Collections.Generic;

namespace Axis.Plugins.ECI.Translator
{
    public static class DischargeTranslator
    {
        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static DischargeViewModel ToViewModel(this DischargeModel entity) {
            if (entity == null)
                return null;
            var model = new DischargeViewModel
            {
                DischargeID = entity.DischargeID,
                ProgressNoteID = entity.ProgressNoteID,
                DischargeTypeID = entity.DischargeTypeID,
                TakenBy = entity.TakenBy,
                DischargeDate = entity.DischargeDate,
                DischargeReasonID = entity.DischargeReasonID,
                ModifiedOn = entity.ModifiedOn
            };
            return model;
        }
        
        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<DischargeViewModel> ToViewModel(this Response<DischargeModel> entity) {
            if (entity == null)
                return null;
            var model = entity.CloneResponse<DischargeViewModel>();
            var dischargeList = new List<DischargeViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(DischargeModel noteModel)
                {
                    var transformedModel = noteModel.ToViewModel();
                    dischargeList.Add(transformedModel);
                });
                model.DataItems = dischargeList;
            }
            return model;
        }
        
        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static DischargeModel ToModel(this DischargeViewModel model) {
            if (model == null)
                return null;

            var entity = new DischargeModel
            {
                DischargeID = model.DischargeID,
                ProgressNoteID = model.ProgressNoteID,
                DischargeTypeID = model.DischargeTypeID,
                TakenBy = model.TakenBy,
                DischargeDate = model.DischargeDate,
                DischargeReasonID = model.DischargeReasonID,
                ModifiedOn = model.ModifiedOn
            };
            return entity;
        }
    }
}
