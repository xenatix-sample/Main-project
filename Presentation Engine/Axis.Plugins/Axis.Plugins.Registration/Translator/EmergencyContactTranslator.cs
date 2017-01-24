using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Plugins.Registration.Models;
using System.Collections.Generic;

namespace Axis.Plugins.Registration.Translator
{
    /// <summary>
    /// Translates demography model to view model
    /// </summary>
    public static class EmergencyContactTranslator
    {
        /// <summary>
        /// Converting EmergencyContactModel to ViewModel
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns></returns>
        public static EmergencyContactViewModel ToViewModel(this EmergencyContactModel entity)
        {
            if (entity == null)
                return null;
            var model = new EmergencyContactViewModel
            {
                ParentContactID = entity.ParentContactID,
                ContactID = entity.ContactID,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Middle = entity.Middle,
                SuffixID = entity.SuffixID,
                DOB = entity.DOB,
                GenderID = entity.GenderID,
                Addresses = entity.Addresses,
                Emails = entity.Emails,
                Phones = entity.Phones,
                RelationshipTypeID = entity.RelationshipTypeID,
                ContactTypeID = entity.ContactTypeID,
                ContactRelationshipID = entity.ContactRelationshipID,
                LivingWithClientStatusID = entity.LivingWithClientStatusID,
                SSN = entity.SSN,
                DriverLicense = entity.DriverLicense,
                AlternateID = entity.AlternateID,
                IsActive=entity.IsActive,
                ModifiedBy=entity.ModifiedBy,
                ModifiedOn=entity.ModifiedOn,
                ForceRollback=entity.ForceRollback
            };
            return model;
        }

        /// <summary>
        /// Converting List EmergencyContactModel to List of ViewModel
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns></returns>
        public static Response<EmergencyContactViewModel> ToViewModel(this Response<EmergencyContactModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<EmergencyContactViewModel>();
            var emergencyContact = new List<EmergencyContactViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(EmergencyContactModel emergencyContactModel)
                {
                    var transformedModel = emergencyContactModel.ToViewModel();
                    emergencyContact.Add(transformedModel);
                });

                model.DataItems = emergencyContact;
            }
            return model;
        }

        /// <summary>
        /// Converting EmergencyContactViewModel to EmergencyContactModel
        /// </summary>
        /// <param name="model">model</param>
        /// <returns></returns>
        public static EmergencyContactModel ToModel(this EmergencyContactViewModel model)
        {
            if (model == null)
                return null;

            var entity = new EmergencyContactModel
            {
                ParentContactID = model.ParentContactID,
                ContactID = model.ContactID,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Middle = model.Middle,
                SuffixID = model.SuffixID,
                DOB = model.DOB,
                GenderID = model.GenderID,
                Addresses = model.Addresses,
                Emails = model.Emails,
                Phones = model.Phones,
                RelationshipTypeID = model.RelationshipTypeID,
                ContactTypeID = model.ContactTypeID,
                ContactRelationshipID = model.ContactRelationshipID,
                LivingWithClientStatusID = model.LivingWithClientStatusID,
                SSN = model.SSN,
                DriverLicense = model.DriverLicense,
                AlternateID = model.AlternateID,
                IsActive = model.IsActive,
                ModifiedBy = model.ModifiedBy,
                ModifiedOn = model.ModifiedOn,
                ForceRollback = model.ForceRollback
            };
            return entity;
        }
    }
}
