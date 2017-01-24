using System;
using Axis.Model.Clinical;
using Axis.Model.Common;
using System.Collections.Generic;
using Axis.Plugins.Clinical.Models.ChiefComplaint;

namespace Axis.Plugins.Clinical.Translator
{
    public static class ChiefComplaintTranslator
    {
        public static ChiefComplaintViewModel ToViewModel(this ChiefComplaintModel entity)
        {
            if (entity == null)
                return null;

            var model = new ChiefComplaintViewModel 
            {
                ChiefComplaintID = entity.ChiefComplaintID,
                ContactID = entity.ContactID,
                TakenBy = entity.TakenBy,
                TakenDate = entity.TakenTime,
                TakenTime = entity.TakenTime,
                Complaint = entity.ChiefComplaint,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        public static Response<ChiefComplaintViewModel> ToViewModel(this Response<ChiefComplaintModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<ChiefComplaintViewModel>();
            var chiefComplaints = new List<ChiefComplaintViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(ChiefComplaintModel chiefComplaint)
                {
                    var transformedModel = chiefComplaint.ToViewModel();
                    chiefComplaints.Add(transformedModel);
                });

                model.DataItems = chiefComplaints;
            }

            return model;
        }

        public static ChiefComplaintModel  ToModel(this ChiefComplaintViewModel  model)
        {
            if (model == null)
                return null;
            
            var entity = new ChiefComplaintModel 
            {
                ChiefComplaintID = model.ChiefComplaintID,
                ContactID = model.ContactID,
                TakenBy = model.TakenBy,
                TakenTime = model.TakenTime,
                ChiefComplaint = model.Complaint,
                ModifiedOn = model.ModifiedOn
            };

            return entity;
        }
    }
}
