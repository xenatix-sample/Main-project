using Axis.Model.Common;
using Axis.Model.ECI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Plugins.ECI.Translator
{
    public static class ECIAdditionalDemographyTranslator
    {
        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static ECIAdditionalDemographicsViewModel ToViewModel(this ECIAdditionalDemographicsModel entity)
        {
            if (entity == null)
                return null;

            var model = new ECIAdditionalDemographicsViewModel
            {
                AdditionalDemographicID = entity.AdditionalDemographicID,
                ContactID = entity.ContactID,
                ReferralDispositionStatusID = entity.ReferralDispositionStatusID,
                ReportingUnitID = entity.ReportingUnitID,
                ServiceCoordinatorID = entity.ServiceCoordinatorID,
                ServiceCoordinatorPhoneID = entity.ServiceCoordinatorPhoneID,
                SchoolDistrictID = entity.SchoolDistrictID,
                EthnicityID = entity.EthnicityID,
                LanguageID = entity.LanguageID,
                OtherRace = entity.OtherRace,
                OtherEthnicity = entity.OtherEthnicity,
                OtherPreferredLanguage = entity.OtherPreferredLanguage,
                InterpreterRequired = entity.InterpreterRequired,
                IsCPSInvolved = entity.IsCPSInvolved,
                IsChildHospitalized = entity.IsChildHospitalized,
                BirthWeightLbs = entity.BirthWeightLbs,
                BirthWeightOz = entity.BirthWeightOz,
                IsTransfer = entity.IsTransfer,
                ExpectedHospitalDischargeDate = entity.ExpectedHospitalDischargeDate,
                TransferFrom=entity.TransferFrom,
                TransferDate=entity.TransferDate,
                IsOutOfServiceArea = entity.IsOutOfServiceArea,
                MRN = entity.MRN,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<ECIAdditionalDemographicsViewModel> ToViewModel(this Response<ECIAdditionalDemographicsModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<ECIAdditionalDemographicsViewModel>();
            var additionalDemographics = new List<ECIAdditionalDemographicsViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(ECIAdditionalDemographicsModel additionalDemography)
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
        public static ECIAdditionalDemographicsModel ToModel(this ECIAdditionalDemographicsViewModel model)
        {
            if (model == null)
                return null;

            var entity = new ECIAdditionalDemographicsModel
            {
                AdditionalDemographicID = model.AdditionalDemographicID,
                ContactID = model.ContactID,
                ReferralDispositionStatusID = model.ReferralDispositionStatusID,
                ReportingUnitID = model.ReportingUnitID,
                ServiceCoordinatorID = model.ServiceCoordinatorID,
                ServiceCoordinatorPhoneID = model.ServiceCoordinatorPhoneID,
                SchoolDistrictID = model.SchoolDistrictID,
                EthnicityID = model.EthnicityID,
                LanguageID = model.LanguageID,
                OtherRace=model.OtherRace,
                OtherEthnicity=model.OtherEthnicity,
                OtherPreferredLanguage=model.OtherPreferredLanguage,
                InterpreterRequired = model.InterpreterRequired,
                IsCPSInvolved = model.IsCPSInvolved,
                IsChildHospitalized = model.IsChildHospitalized,
                BirthWeightLbs = model.BirthWeightLbs,
                BirthWeightOz = model.BirthWeightOz,
                IsTransfer = model.IsTransfer,
                ExpectedHospitalDischargeDate=model.ExpectedHospitalDischargeDate,
                TransferFrom = model.TransferFrom,
                TransferDate = model.TransferDate,
                IsOutOfServiceArea = model.IsOutOfServiceArea,
                MRN = model.MRN,
                ModifiedOn = model.ModifiedOn
            };

            return entity;
        }
    }
}
