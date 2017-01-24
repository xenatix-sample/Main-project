using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Plugins.Registration.Models;
using System.Collections.Generic;

namespace Axis.Plugins.Registration.Translator
{
    public static class BenefitsAssistanceTranslator
    {
        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static BenefitsAssistanceViewModel ToViewModel(this BenefitsAssistanceModel entity)
        {
            if (entity == null)
                return null;

            var model = new BenefitsAssistanceViewModel
            {
                BenefitsAssistanceID = entity.BenefitsAssistanceID,
                ContactID = entity.ContactID,
                DateEntered = entity.DateEntered,
                UserID = entity.UserID,
                ProviderName = entity.ProviderName,
                ServiceRecordingID=entity.ServiceRecordingID,
                AssessmentID = entity.AssessmentID,
                ResponseID = entity.ResponseID,
                ServiceRecordingVoidID = entity.ServiceRecordingVoidID,
                DocumentStatusID = entity.DocumentStatusID,
                ModifiedOn = entity.ModifiedOn,
                IsVoided = entity.IsVoided,
                ServiceStartDate = entity.ServiceStartDate,
                ServiceEndDate = entity.ServiceEndDate,
                ServiceItemID = entity.ServiceItemID,
                TrackingFieldID = entity.TrackingFieldID
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<BenefitsAssistanceViewModel> ToViewModel(this Response<BenefitsAssistanceModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<BenefitsAssistanceViewModel>();
            var benefitsAssistanceList = new List<BenefitsAssistanceViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(BenefitsAssistanceModel benefitsAssistanceModel)
                {
                    var transformedModel = benefitsAssistanceModel.ToViewModel();
                    benefitsAssistanceList.Add(transformedModel);
                });

                model.DataItems = benefitsAssistanceList;
            }

            return model;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static BenefitsAssistanceModel ToModel(this BenefitsAssistanceViewModel model)
        {
            if (model == null)
                return null;

            var entity = new BenefitsAssistanceModel
            {
                BenefitsAssistanceID = model.BenefitsAssistanceID,
                ContactID = model.ContactID,
                DateEntered = model.DateEntered,
                UserID = model.UserID,
                ServiceRecordingID = model.ServiceRecordingID,
                ProviderName = model.ProviderName,
                AssessmentID = model.AssessmentID,
                ResponseID = model.ResponseID,
                DocumentStatusID = model.DocumentStatusID,
                ServiceRecordingVoidID = model.ServiceRecordingVoidID,
                ModifiedOn = model.ModifiedOn,
                IsVoided = model.IsVoided,
                ServiceStartDate = model.ServiceStartDate,
                ServiceEndDate = model.ServiceEndDate,
                ServiceItemID = model.ServiceItemID,
                TrackingFieldID = model.TrackingFieldID
            };

            return entity;
        }
    }
}
