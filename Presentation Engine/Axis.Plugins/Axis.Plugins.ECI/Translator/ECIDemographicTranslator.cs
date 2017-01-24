using System.Collections.Generic;
using Axis.Model.Common;
using Axis.Model.ECI;
using Axis.Plugins.ECI.Models.ECIDemographics;

namespace Axis.Plugins.ECI.Translator
{
    public static class ECIDemographicTranslator
    {
        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static ECIContactDemographicsViewModel ToViewModel(this ECIContactDemographicsModel entity)
        {
            if (entity == null)
                return null;

            var model = new ECIContactDemographicsViewModel
            {
                ContactID = entity.ContactID,
                ContactTypeID = entity.ContactTypeID,
                ClientTypeID = entity.ClientTypeID,
                FirstName = entity.FirstName,
                Middle = entity.Middle,
                LastName = entity.LastName,
                SuffixID = entity.SuffixID,
                GenderID = entity.GenderID,
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
                MRN = entity.MRN,
                //List Item
                Addresses = entity.Addresses,
                Emails = entity.Emails,
                Phones = entity.Phones,
                ClientAlternateIDs = entity.ClientAlternateIDs,
                ContactPresentingProblem = entity.ContactPresentingProblem,
                GestationalAge = entity.GestationalAge,
                ModifiedOn = entity.ModifiedOn,
                TransactionID = entity.TransactionID,
                ScreenID = entity.ScreenID,
                isContactNotDirty = entity.isContactNotDirty
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<ECIContactDemographicsViewModel> ToViewModel(this Response<ECIContactDemographicsModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<ECIContactDemographicsViewModel>();
            var contactDemographics = new List<ECIContactDemographicsViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(ECIContactDemographicsModel contactDemography)
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
        public static ECIContactDemographicsModel ToModel(this ECIContactDemographicsViewModel model)
        {
            if (model == null)
                return null;

            var entity = new ECIContactDemographicsModel
            {
                ContactID = model.ContactID,
                ContactTypeID = model.ContactTypeID,
                ClientTypeID = model.ClientTypeID,
                FirstName = model.FirstName,
                Middle = model.Middle,
                LastName = model.LastName,
                SuffixID = model.SuffixID,
                GenderID = model.GenderID,
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
                MRN = model.MRN,
                //List Item
                Addresses = model.Addresses,
                Emails = model.Emails,
                Phones = model.Phones,
                ClientAlternateIDs = model.ClientAlternateIDs,
                ContactPresentingProblem = model.ContactPresentingProblem,
                GestationalAge = model.GestationalAge,
                ModifiedOn = model.ModifiedOn,
                TransactionID = model.TransactionID,
                ScreenID = model.ScreenID,
                isContactNotDirty = model.isContactNotDirty
            };

            return entity;
        }
    }
}
