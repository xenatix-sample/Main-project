using System.Collections.Generic;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Plugins.Registration.Models;

namespace Axis.Plugins.Registration.Translator
{
    public static class IntakeFormTranslator
    {
        public static FormsViewModel ToViewModel(this FormsModel entity)
        {
            if (entity == null)
                return null;
            var model = new FormsViewModel
            {
                ContactFormsID = entity.ContactFormsID,
                ContactID = entity.ContactID,
                AssessmentID = entity.AssessmentID,
                ResponseID = entity.ResponseID,
                ServiceRecordingID = entity.ServiceRecordingID,
                ServiceRecordingVoidID = entity.ServiceRecordingVoidID,
                IsVoided = entity.IsVoided,
                UserID = entity.UserID,
                ProviderName = entity.ProviderName,
                DocumentStatusID = entity.DocumentStatusID,
                ModifiedOn = entity.ModifiedOn,
                ServiceStartDate = entity.ServiceStartDate,
                ServiceEndDate = entity.ServiceEndDate
            };
            return model;
        }

        public static Response<FormsViewModel> ToViewModel(this Response<FormsModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<FormsViewModel>();
            var FormsList = new List<FormsViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(FormsModel FormsModel)
                {
                    var transformedModel = FormsModel.ToViewModel();
                    FormsList.Add(transformedModel);
                });

                model.DataItems = FormsList;
            }

            return model;
        }

        public static FormsModel ToModel(this FormsViewModel model)
        {
            if (model == null)
                return null;
            var entity = new FormsModel
            {
                ContactFormsID = model.ContactFormsID,
                ContactID = model.ContactID,
                AssessmentID = model.AssessmentID,
                ServiceRecordingID = model.ServiceRecordingID,
                ServiceRecordingVoidID = model.ServiceRecordingVoidID,
                IsVoided=model.IsVoided,
                ResponseID = model.ResponseID,
                UserID = model.UserID,
                ProviderName = model.ProviderName,
                DocumentStatusID = model.DocumentStatusID,
                ModifiedOn = model.ModifiedOn,
                ServiceStartDate = model.ServiceStartDate,
                ServiceEndDate = model.ServiceEndDate
            };
            return entity;
        }
    }
}
