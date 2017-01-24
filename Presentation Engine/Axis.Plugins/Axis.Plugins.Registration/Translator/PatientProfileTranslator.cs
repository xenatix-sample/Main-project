using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Plugins.Registration.Model;
using Axis.PresentationEngine.Helpers.Model;
using System.Collections.Generic;
using Axis.PresentationEngine.Helpers.Translator;

namespace Axis.Plugins.Registration.Translator
{
    /// <summary>
    /// Translator to convert view model to model and model to view model for Financial Assessment Screen
    /// </summary>
    public static class PatientProfileTranslator
    {
        public static PatientProfileViewModel ToViewModel(this PatientProfileModel entity)
        {
            if (entity == null)
                return null;

            var model = new PatientProfileViewModel
            {
                ContactID = entity.ContactID,
                ClientTypeID=entity.ClientTypeID,
                FirstName=entity.FirstName,
                Middle= entity.Middle,
                LastName=entity.LastName,
                PreferredName = entity.PreferredName,
                MRN = entity.MRN,
                CareID = entity.CareID,
                GenderID = entity.GenderID,
                DOB = entity.DOB,
                LegalStatusID = entity.LegalStatusID,
                EmergencyContactFirstName = entity.EmergencyContactFirstName,
                EmergencyContactLastName = entity.EmergencyContactLastName,
                EmergencyContactPhoneNumber = entity.EmergencyContactPhoneNumber,
                EmergencyContactExtension = entity.EmergencyContactExtension,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        public static Response<PatientProfileViewModel> ToViewModel(this Response<PatientProfileModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<PatientProfileViewModel>();
            var patientProfileView = new List<PatientProfileViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(PatientProfileModel patientProfile)
                {
                    var transformedModel = patientProfile.ToViewModel();
                    patientProfileView.Add(transformedModel);
                });

                model.DataItems = patientProfileView;
            }

            return model;
        }

        public static PatientProfileModel ToModel(this PatientProfileViewModel model)
        {
            if (model == null)
                return null;

            var entity = new PatientProfileModel
            {
                ContactID = model.ContactID,
                ClientTypeID = model.ClientTypeID,
                FirstName = model.FirstName,
                Middle = model.Middle,
                LastName = model.LastName,
                PreferredName = model.PreferredName,
                MRN = model.MRN,
                CareID = model.CareID,
                GenderID = model.GenderID,
                DOB = model.DOB,
                LegalStatusID = model.LegalStatusID,
                EmergencyContactFirstName = model.EmergencyContactFirstName,
                EmergencyContactLastName = model.EmergencyContactLastName,
                EmergencyContactPhoneNumber = model.EmergencyContactPhoneNumber,
                EmergencyContactExtension = model.EmergencyContactExtension,
                ModifiedOn = model.ModifiedOn
            };
            return entity;
        }

    }
}
