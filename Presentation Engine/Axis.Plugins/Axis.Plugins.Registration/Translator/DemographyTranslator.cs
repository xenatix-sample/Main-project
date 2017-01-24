using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Plugins.Registration.Model;
using System.Collections.Generic;

namespace Axis.Plugins.Registration.Translator
{
    /// <summary>
    /// Translates demography model to view model
    /// </summary>
    public static class DemographyTranslator
    {
        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static ContactDemographicsViewModel ToViewModel(this ContactDemographicsModel entity)
        {
            if (entity == null)
                return null;

            var model = new ContactDemographicsViewModel
            {
                ContactID = entity.ContactID,
                TransactionID = entity.TransactionID,
                ScreenID = entity.ScreenID,
                ContactTypeID = entity.ContactTypeID,
                ClientTypeID = entity.ClientTypeID,
                ProgramUnit = entity.ProgramUnit,
                FirstName = entity.FirstName,

                Middle = entity.Middle,
                LastName = entity.LastName,
                SuffixID = entity.SuffixID,
                GenderID = entity.GenderID,
                PreferredGenderID = entity.PreferredGenderID,
                TitleID = entity.TitleID,
                DOB = entity.DOB,
                SSN = entity.SSN,

                DOBStatusID = entity.DOBStatusID,
                ReferralSourceID = entity.ReferralSourceID,
                SSNStatusID = entity.SSNStatusID,
                PreferredName = entity.PreferredName,
                IsDeceased = entity.IsDeceased,
                DeceasedDate = entity.DeceasedDate,
                CauseOfDeath = entity.CauseOfDeath,
                ContactMethodID = entity.ContactMethodID,
                DriverLicense = entity.DriverLicense,
                DriverLicenseStateID = entity.DriverLicenseStateID,
                IsPregnant = entity.IsPregnant,
                MRN = entity.MRN,
                ModifiedOn = entity.ModifiedOn,
                //List Item
                Addresses = entity.Addresses,
                Emails = entity.Emails,
                Phones = entity.Phones,
                ClientAlternateIDs = entity.ClientAlternateIDs,
                GestationalAge = entity.GestationalAge,
                ReportingUnit = entity.ReportingUnit,
                ServiceCoordinator = entity.ServiceCoordinator,
                ServiceCoordinatorPhone = entity.ServiceCoordinatorPhone,
                AdjustedAge = entity.AdjustedAge,
                DispositionStatus = entity.DispositionStatus,
                ContactPresentingProblem = entity.ContactPresentingProblem,
                isContactNotDirty = entity.isContactNotDirty,
                IsMerged = entity.IsMerged,
                MergedMRN = entity.MergedMRN
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<ContactDemographicsViewModel> ToViewModel(this Response<ContactDemographicsModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<ContactDemographicsViewModel>();
            var contactDemographics = new List<ContactDemographicsViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate (ContactDemographicsModel contactDemography)
                {
                    var transformedModel = contactDemography.ToViewModel();
                    contactDemographics.Add(transformedModel);
                });

                model.DataItems = contactDemographics;
            }

            return model;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static ContactDemographicsModel ToModel(this ContactDemographicsViewModel model)
        {
            if (model == null)
                return null;

            var entity = new ContactDemographicsModel
            {
                ContactID = model.ContactID,
                TransactionID = model.TransactionID,
                ScreenID = model.ScreenID,
                ContactTypeID = model.ContactTypeID,
                ClientTypeID = model.ClientTypeID,
                ProgramUnit = model.ProgramUnit,
                FirstName = model.FirstName,

                Middle = model.Middle,
                LastName = model.LastName,
                SuffixID = model.SuffixID,
                GenderID = model.GenderID,
                PreferredGenderID = model.PreferredGenderID,
                TitleID = model.TitleID,
                DOB = model.DOB,
                SSN = model.SSN,

                DOBStatusID = model.DOBStatusID,
                ReferralSourceID = model.ReferralSourceID,
                SSNStatusID = model.SSNStatusID,
                PreferredName = model.PreferredName,
                IsDeceased = model.IsDeceased,
                DeceasedDate = model.DeceasedDate,
                CauseOfDeath = model.CauseOfDeath,
                ContactMethodID = model.ContactMethodID,
                DriverLicense = model.DriverLicense,
                DriverLicenseStateID = model.DriverLicenseStateID,

                IsPregnant = model.IsPregnant,
                MRN = model.MRN,
                ModifiedOn = model.ModifiedOn,
                //List Item
                Addresses = model.Addresses,
                Emails = model.Emails,
                Phones = model.Phones,
                ClientAlternateIDs = model.ClientAlternateIDs,

                GestationalAge = model.GestationalAge,
                ReportingUnit = model.ReportingUnit,
                ServiceCoordinator = model.ServiceCoordinator,
                ServiceCoordinatorPhone = model.ServiceCoordinatorPhone,
                AdjustedAge = model.AdjustedAge,
                DispositionStatus = model.DispositionStatus,
                ContactPresentingProblem = model.ContactPresentingProblem,
                isContactNotDirty = model.isContactNotDirty
            };

            return entity;
        }
    }
}
