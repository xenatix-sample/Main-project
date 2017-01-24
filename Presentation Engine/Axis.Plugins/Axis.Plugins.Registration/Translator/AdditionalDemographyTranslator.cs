using System;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Plugins.Registration.Model;
using System.Collections.Generic;

namespace Axis.Plugins.Registration.Translator
{
    /// <summary>
    ///
    /// </summary>
    public static class AdditionalDemographyTranslator
    {
        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static AdditionalDemographicsViewModel ToViewModel(this AdditionalDemographicsModel entity)
        {
            if (entity == null)
                return null;

            var model = new AdditionalDemographicsViewModel
            {
                AdditionalDemographicID = entity.AdditionalDemographicID,
                ContactID = entity.ContactID,
                AdvancedDirective = entity.AdvancedDirective,
                SmokingStatusID = entity.SmokingStatusID,
                OtherRace = entity.OtherRace,
                OtherEthnicity = entity.OtherEthnicity,
                LookingForWork = entity.LookingForWork,
                SchoolDistrictID = entity.SchoolDistrictID,
                Name = entity.Name,
                DOB = entity.DOB,
                MRN = entity.MRN,
                EthnicityID = entity.EthnicityID,
                PrimaryLanguageID = entity.PrimaryLanguageID,
                SecondaryLanguageID = entity.SecondaryLanguageID,
                LegalStatusID = entity.LegalStatusID,
                LivingArrangementID = entity.LivingArrangementID,
                CitizenshipID = entity.CitizenshipID,
                MaritalStatusID = entity.MaritalStatusID,
                EmploymentStatusID = entity.EmploymentStatusID,
                PlaceOfEmployment = entity.PlaceOfEmployment,
                EmploymentBeginDate = entity.EmploymentBeginDate,
                EmploymentEndDate = entity.EmploymentEndDate,
                ReligionID = entity.ReligionID,
                VeteranStatusID = entity.VeteranStatusID,
                EducationStatusID = entity.EducationStatusID,
                SchoolAttended = entity.SchoolAttended,
                SchoolBeginDate = entity.SchoolBeginDate,
                SchoolEndDate = entity.SchoolEndDate,
                LivingWill = entity.LivingWill,
                PowerOfAttorney = entity.PowerOfAttorney,
                ModifiedBy = entity.ModifiedBy,
                InterpreterRequired = entity.InterpreterRequired,
                OtherLegalstatus = entity.OtherLegalstatus,
                OtherPreferredLanguage = entity.OtherPreferredLanguage,
                OtherSecondaryLanguage = entity.OtherSecondaryLanguage,
                OtherCitizenship = entity.OtherCitizenship,
                OtherEducation = entity.OtherEducation,
                OtherLivingArrangement = entity.OtherLivingArrangement,
                OtherVeteranStatus = entity.OtherVeteranStatus,
                OtherEmploymentStatus = entity.OtherEmploymentStatus,
                OtherReligion = entity.OtherReligion,
                ModifiedOn = entity.ModifiedOn,
                AdvancedDirectiveTypeID = entity.AdvancedDirectiveTypeID
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<AdditionalDemographicsViewModel> ToViewModel(this Response<AdditionalDemographicsModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<AdditionalDemographicsViewModel>();
            var additionalDemographics = new List<AdditionalDemographicsViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(AdditionalDemographicsModel additionalDemography)
                {
                    var transformedModel = additionalDemography.ToViewModel();
                    additionalDemographics.Add(transformedModel);
                });

                model.DataItems = additionalDemographics;
            }

            return model;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static AdditionalDemographicsModel ToModel(this AdditionalDemographicsViewModel model)
        {
            if (model == null)
                return null;

            var entity = new AdditionalDemographicsModel
            {
                AdditionalDemographicID = model.AdditionalDemographicID,
                ContactID = model.ContactID,
                AdvancedDirective = model.AdvancedDirective,
                SmokingStatusID = model.SmokingStatusID,
                OtherRace = model.OtherRace,
                OtherEthnicity = model.OtherEthnicity,
                LookingForWork = model.LookingForWork,
                SchoolDistrictID = model.SchoolDistrictID,
                Name = model.Name,
                DOB = model.DOB,
                MRN = model.MRN,
                EthnicityID = model.EthnicityID,
                PrimaryLanguageID = model.PrimaryLanguageID,
                SecondaryLanguageID = model.SecondaryLanguageID,
                LegalStatusID = model.LegalStatusID,
                LivingArrangementID = model.LivingArrangementID,
                CitizenshipID = model.CitizenshipID,
                MaritalStatusID = model.MaritalStatusID,
                EmploymentStatusID = model.EmploymentStatusID,
                PlaceOfEmployment = model.PlaceOfEmployment,
                EmploymentBeginDate = model.EmploymentBeginDate,
                EmploymentEndDate = model.EmploymentEndDate,
                ReligionID = model.ReligionID,
                VeteranStatusID = model.VeteranStatusID,
                EducationStatusID = model.EducationStatusID,
                SchoolAttended = model.SchoolAttended,
                SchoolBeginDate = model.SchoolBeginDate,
                SchoolEndDate = model.SchoolEndDate,
                LivingWill = model.LivingWill,
                PowerOfAttorney = model.PowerOfAttorney,
                ModifiedBy = model.ModifiedBy,
                InterpreterRequired = model.InterpreterRequired,
                OtherLegalstatus = model.OtherLegalstatus,
                OtherPreferredLanguage = model.OtherPreferredLanguage,
                OtherSecondaryLanguage = model.OtherSecondaryLanguage,
                OtherCitizenship = model.OtherCitizenship,
                OtherEducation = model.OtherEducation,
                OtherLivingArrangement = model.OtherLivingArrangement,
                OtherVeteranStatus = model.OtherVeteranStatus,
                OtherEmploymentStatus = model.OtherEmploymentStatus,
                OtherReligion = model.OtherReligion,
                ModifiedOn = model.ModifiedOn,
                AdvancedDirectiveTypeID = model.AdvancedDirectiveTypeID,
                GenerateMRN = model.GenerateMRN
            };

            return entity;
        }
    }
}