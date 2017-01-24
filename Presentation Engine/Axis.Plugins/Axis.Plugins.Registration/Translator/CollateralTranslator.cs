
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Model.Registration.Model;
using Axis.Plugins.Registration.Models;
using System.Collections.Generic;

namespace Axis.Plugins.Registration.Translator
{
    /// <summary>
    /// Translates demography model to view model
    /// </summary>
    public static class CollateralTranslator
    {
        /// <summary>
        /// Converting CollateralModel to ViewModel
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns></returns>
        public static CollateralViewModel ToViewModel(this CollateralModel entity)
        {
            if (entity == null)
                return null;
            var model = new CollateralViewModel
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
                ClientAlternateIDs = entity.ClientAlternateIDs,
                RelationshipTypeID = entity.RelationshipTypeID,
                ContactTypeID = entity.ContactTypeID,
                ContactRelationshipID = entity.ContactRelationshipID,
                LivingWithClientStatus = entity.LivingWithClientStatus,
                ReceiveCorrespondenceID = entity.ReceiveCorrespondenceID,
                SSN = entity.SSN,
                DriverLicenseStateID = entity.DriverLicenseStateID,
                IsActive = entity.IsActive,
                ModifiedBy = entity.ModifiedBy,
                ModifiedOn = entity.ModifiedOn,
                ForceRollback = entity.ForceRollback,
                EmergencyContact = entity.EmergencyContact,
                CopyContactAddress = entity.CopyContactAddress,
                EmploymentStatusID = entity.EmploymentStatusID,
                EducationStatusID = entity.EducationStatusID,
                SchoolAttended = entity.SchoolAttended,
                SchoolBeginDate = entity.SchoolBeginDate,
                SchoolEndDate = entity.SchoolEndDate,
                VeteranStatusID = entity.VeteranStatusID,
                RelationshipGroupID = entity.RelationshipGroupID,
                OtherRelationship = entity.OtherRelationship,
                IsPolicyHolder = entity.IsPolicyHolder,
                CollateralEffectiveDate = entity.CollateralEffectiveDate,
                CollateralExpirationDate = entity.CollateralExpirationDate,
                Relationships = entity.Relationships,
                CollateralTypes = entity.CollateralTypes
            };
            return model;
        }

        /// <summary>
        /// Converting List CollateralModel to List of ViewModel
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns></returns>
        public static Response<CollateralViewModel> ToViewModel(this Response<CollateralModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<CollateralViewModel>();
            var collateral = new List<CollateralViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate (CollateralModel CollateralModel)
                {
                    var transformedModel = CollateralModel.ToViewModel();
                    collateral.Add(transformedModel);
                });

                model.DataItems = collateral;
            }
            return model;
        }

        /// <summary>
        /// Converting CollateralViewModel to CollateralModel
        /// </summary>
        /// <param name="model">model</param>
        /// <returns></returns>
        public static CollateralModel ToModel(this CollateralViewModel model)
        {
            if (model == null)
                return null;

            var entity = new CollateralModel
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
                ClientAlternateIDs = model.ClientAlternateIDs,
                RelationshipTypeID = model.RelationshipTypeID,
                ContactTypeID = model.ContactTypeID,
                ContactRelationshipID = model.ContactRelationshipID,
                LivingWithClientStatus = model.LivingWithClientStatus,
                ReceiveCorrespondenceID = model.ReceiveCorrespondenceID,
                SSN = model.SSN,
                DriverLicense = model.DriverLicense,
                DriverLicenseStateID = model.DriverLicenseStateID,
                IsActive = model.IsActive,
                ModifiedBy = model.ModifiedBy,
                ModifiedOn = model.ModifiedOn,
                ForceRollback = model.ForceRollback,
                EmergencyContact = model.EmergencyContact,
                CopyContactAddress = model.CopyContactAddress,
                EmploymentStatusID = model.EmploymentStatusID,
                EducationStatusID = model.EducationStatusID,
                SchoolAttended = model.SchoolAttended,
                SchoolBeginDate = model.SchoolBeginDate,
                SchoolEndDate = model.SchoolEndDate,
                VeteranStatusID = model.VeteranStatusID,
                RelationshipGroupID = model.RelationshipGroupID,
                OtherRelationship = model.OtherRelationship,
                IsPolicyHolder = model.IsPolicyHolder,
                CollateralEffectiveDate = model.CollateralEffectiveDate,
                CollateralExpirationDate = model.CollateralExpirationDate,
                Relationships = model.Relationships,
                CollateralTypes = model.CollateralTypes
            };
            return entity;
        }
    }
}
