using System.Collections.Generic;
using System.Linq;
using Axis.Model.Clinical.MedicalHistory;
using Axis.Model.Common;
using Axis.Plugins.Clinical.Models.MedicalHistory;
using Axis.Plugins.Clinical.Models.Shared;

namespace Axis.Plugins.Clinical.Translator
{
    public static class MedicalHistoryTranslator
    {
        public static MedicalHistoryViewModel ToViewModel(this MedicalHistoryModel entity)
        {
            if (entity == null)
                return null;

            var model = new MedicalHistoryViewModel
            {
                MedicalHistoryID = entity.MedicalHistoryID,
                ContactID = entity.ContactID,
                EncounterID = entity.EncounterID,
                TakenBy = entity.TakenBy,
                TakenTime = entity.TakenTime,
                Conditions = new List<MedicalHistoryConditionViewModel>(),
                ModifiedOn = entity.ModifiedOn
            };

            if (entity.Conditions != null)
            {
                entity.Conditions.ForEach(delegate(MedicalHistoryConditionModel condition)
                {
                    var transformedModel = condition.ToViewModel();
                    model.Conditions.Add(transformedModel);
                });
            }

            return model;
        }

        public static MedicalHistoryConditionViewModel ToViewModel(this MedicalHistoryConditionModel entity)
        {
            if (entity == null)
                return null;

            var model = new MedicalHistoryConditionViewModel
            {
                MedicalHistoryConditionID = entity.MedicalHistoryConditionID,
                MedicalHistoryID = entity.MedicalHistoryID,
                MedicalConditionID = entity.MedicalConditionID,
                HasCondition = entity.HasCondition,
                IsActive = entity.IsActive,
                Details = new List<MedicalHistoryConditionDetailViewModel>(),
                ModifiedOn = entity.ModifiedOn
            };

            if (entity.Details != null)
            {
                var rowNum = 0;
                entity.Details.ForEach(delegate(MedicalHistoryConditionDetailModel conditionDetail)
                {
                    var transformedModel = conditionDetail.ToViewModel();
                    transformedModel.RowNumber = rowNum;
                    rowNum++;
                    model.Details.Add(transformedModel);
                });

                var lastMedicalHistoryConditionDetailViewModel = model.Details.LastOrDefault();
                if (lastMedicalHistoryConditionDetailViewModel != null) lastMedicalHistoryConditionDetailViewModel.IsLast = true;

                var firstMedicalHistoryConditionDetailViewModel = model.Details.FirstOrDefault();
                if (firstMedicalHistoryConditionDetailViewModel != null) firstMedicalHistoryConditionDetailViewModel.IsFirst = true;

                model.RowCount = model.Details.Any() ? model.Details.Count() : 1;
            }

            return model;
        }

        public static MedicalHistoryConditionDetailViewModel ToViewModel(this MedicalHistoryConditionDetailModel entity)
        {
            if (entity == null)
                return null;

            var model = new MedicalHistoryConditionDetailViewModel
            {
                MedicalHistoryConditionDetailID = entity.MedicalHistoryConditionDetailID,
                MedicalHistoryConditionID = entity.MedicalHistoryConditionID,
                FamilyRelationshipID = entity.FamilyRelationshipID,
                IsSelf = entity.IsSelf,
                Comments = entity.Comments,
                RelationshipTypeID = entity.RelationshipTypeID,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                IsDeceased = entity.IsDeceased,
                IsActive = entity.IsActive,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        public static MedicalHistoryModel ToModel(this MedicalHistoryViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new MedicalHistoryModel
            {
                MedicalHistoryID = entity.MedicalHistoryID,
                EncounterID = entity.EncounterID,
                ContactID = entity.ContactID,
                TakenBy = entity.TakenBy,
                TakenTime = entity.TakenTime,
                Conditions = new List<MedicalHistoryConditionModel>(),
                ModifiedOn = entity.ModifiedOn
            };

            if (entity.Conditions != null)
            {
                entity.Conditions.ForEach(delegate(MedicalHistoryConditionViewModel condition)
                {
                    var transformedModel = condition.ToModel();
                    model.Conditions.Add(transformedModel);
                });
            }

            return model;
        }

        public static MedicalHistoryConditionModel ToModel(this MedicalHistoryConditionViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new MedicalHistoryConditionModel
            {
                MedicalHistoryConditionID = entity.MedicalHistoryConditionID,
                MedicalHistoryID = entity.MedicalHistoryID,
                MedicalConditionID = entity.MedicalConditionID,
                HasCondition = entity.HasCondition,
                IsActive = entity.IsActive,
                Details = new List<MedicalHistoryConditionDetailModel>(),
                ModifiedOn = entity.ModifiedOn
            };

            entity.Details.ForEach(delegate(MedicalHistoryConditionDetailViewModel conditionDetail)
            {
                var transformedModel = conditionDetail.ToModel();
                model.Details.Add(transformedModel);
            });

            return model;
        }

        public static MedicalHistoryConditionDetailModel ToModel(this MedicalHistoryConditionDetailViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new MedicalHistoryConditionDetailModel
            {
                MedicalHistoryConditionDetailID = entity.MedicalHistoryConditionDetailID,
                MedicalHistoryConditionID = entity.MedicalHistoryConditionID,
                FamilyRelationshipID = entity.FamilyRelationshipID,
                IsSelf = entity.IsSelf,
                Comments = entity.Comments,
                RelationshipTypeID = entity.RelationshipTypeID,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                IsDeceased = entity.IsDeceased,
                IsActive = entity.IsActive,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        public static Response<MedicalHistoryViewModel> ToViewModel(this Response<MedicalHistoryModel> entity)
        {
            if (entity == null)
                return null;

            var dataItems = new List<MedicalHistoryViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(MedicalHistoryModel medicalHistoryModel)
                {
                    var transformedModel = medicalHistoryModel.ToViewModel();
                    dataItems.Add(transformedModel);
                });
            }
            else
            {
                dataItems = null;
            }

            var model = new Response<MedicalHistoryViewModel>
            {
                ResultCode = entity.ResultCode,
                ResultMessage = entity.ResultMessage,
                DataItems = dataItems,
                RowAffected = entity.RowAffected,
                AdditionalResult = entity.AdditionalResult,
                ID = entity.ID
            };

            return model;
        }

        public static Response<MedicalHistoryConditionViewModel> ToViewModel(this Response<MedicalHistoryConditionModel> entity)
        {
            if (entity == null)
                return null;

            var dataItems = new List<MedicalHistoryConditionViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(MedicalHistoryConditionModel condition)
                {
                    var transformedModel = condition.ToViewModel();
                    dataItems.Add(transformedModel);
                });
            }
            else
            {
                dataItems = null;
            }

            var model = new Response<MedicalHistoryConditionViewModel>
            {
                ResultCode = entity.ResultCode,
                ResultMessage = entity.ResultMessage,
                DataItems = dataItems,
                RowAffected = entity.RowAffected,
                AdditionalResult = entity.AdditionalResult,
                ID = entity.ID
            };

            return model;
        }
    }
}
