using Axis.Model.Address;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Plugins.Registration.Model;
using Axis.Plugins.Registration.Models;
using System.Collections.Generic;

namespace Axis.Plugins.Registration.Translator
{
    /// <summary>
    ///
    /// </summary>
    public static class BenefitTranslator
    {
        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static ContactBenefitViewModel ToViewModel(this  ContactBenefitModel entity)
        {
            if (entity == null)
                return null;

            var model = new ContactBenefitViewModel
            {
                ContactPayorID = entity.ContactPayorID,
                ContactID = entity.ContactID,
                PayorID = entity.PayorID,
                PayorPlanID = entity.PayorPlanID,
                PayorGroupID = entity.PayorGroupID,
                PolicyHolderID = entity.PolicyHolderID,
                PolicyHolderName = entity.PolicyHolderName,
                PayorAddressID = entity.PayorAddressID,
                PolicyID = entity.PolicyID,
                Deductible = entity.Deductible,
                Copay = entity.Copay,
                CoInsurance = entity.CoInsurance,
                EffectiveDate = entity.EffectiveDate,
                ExpirationDate = entity.ExpirationDate,
                PayorExpirationReasonID = entity.PayorExpirationReasonID,
                ExpirationReason = entity.ExpirationReason,
                AddRetroDate = entity.AddRetroDate,
                ContactPayorRank = entity.ContactPayorRank,

                AddressID = entity.AddressID,
                AddressTypeID = entity.AddressTypeID,
                Line1 = entity.Line1,
                Line2 = entity.Line2,
                City = entity.City,
                StateProvince = entity.StateProvince,
                County = entity.County,
                Zip = entity.Zip,

                PlanName = entity.PlanName,
                PlanID = entity.PlanID,

                GroupName = entity.GroupName,
                GroupID = entity.GroupID,

                PolicyHolderFirstName = entity.PolicyHolderFirstName,
                PolicyHolderMiddleName = entity.PolicyHolderMiddleName,
                PolicyHolderLastName = entity.PolicyHolderLastName,
                PolicyHolderEmployer = entity.PolicyHolderEmployer,
                PolicyHolderSuffixID = entity.PolicyHolderSuffixID,
                PayorName = entity.PayorName,
                PayorCode = entity.PayorCode,
                ModifiedOn = entity.ModifiedOn,
                BillingFirstName = entity.BillingFirstName,
                BillingMiddleName=entity.BillingMiddleName,
                BillingLastName =entity.BillingLastName,
                BillingSuffixID=entity.BillingSuffixID,
                AdditionalInformation = entity.AdditionalInformation,
                ElectronicPayorID = entity.ElectronicPayorID,
                HasPolicyHolderSameCardName=entity.HasPolicyHolderSameCardName
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<ContactBenefitViewModel> ToViewModel(this Response<ContactBenefitModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<ContactBenefitViewModel>();
            var contactBenefits = new List<ContactBenefitViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(ContactBenefitModel contactBenefit)
                {
                    var transformedModel = contactBenefit.ToViewModel();
                    contactBenefits.Add(transformedModel);
                });

                model.DataItems = contactBenefits;
            }

            return model;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static ContactBenefitModel ToModel(this ContactBenefitViewModel model)
        {
            if (model == null)
                return null;

            var entity = new ContactBenefitModel
            {
                ContactPayorID = model.ContactPayorID,
                ContactID = model.ContactID,
                PayorID = model.PayorID,
                PayorPlanID = model.PayorPlanID,
                PayorGroupID = model.PayorGroupID,
                PolicyHolderID = model.PolicyHolderID,
                PolicyHolderName = model.PolicyHolderName,
                PayorAddressID = model.PayorAddressID,
                PolicyID = model.PolicyID,
                Deductible = model.Deductible,
                Copay = model.Copay,
                CoInsurance = model.CoInsurance,
                EffectiveDate = model.EffectiveDate,
                ExpirationDate = model.ExpirationDate,
                PayorExpirationReasonID = model.PayorExpirationReasonID,
                ExpirationReason = model.ExpirationReason,
                AddRetroDate = model.AddRetroDate,
                ContactPayorRank = model.ContactPayorRank,

                AddressID = model.AddressID,
                AddressTypeID = model.AddressTypeID,
                Line1 = model.Line1,
                Line2 = model.Line2,
                City = model.City,
                StateProvince = model.StateProvince,
                County = model.County,
                Zip =  !string.IsNullOrEmpty(model.Zip) ? model.Zip : null,
                PlanName = model.PlanName,
                PlanID = model.PlanID,

                GroupName = model.GroupName,
                GroupID = model.GroupID,

                PolicyHolderFirstName = model.PolicyHolderFirstName,
                PolicyHolderMiddleName = model.PolicyHolderMiddleName,
                PolicyHolderLastName = model.PolicyHolderLastName,
                PolicyHolderEmployer = model.PolicyHolderEmployer,
                PolicyHolderSuffixID = model.PolicyHolderSuffixID,

                PayorName = model.PayorName,
                PayorCode = model.PayorCode,
                ForceRollback = model.ForceRollback,
                ModifiedOn = model.ModifiedOn,
                BillingFirstName = model.BillingFirstName,
                BillingMiddleName = model.BillingMiddleName,
                BillingLastName = model.BillingLastName,
                BillingSuffixID = model.BillingSuffixID,
                AdditionalInformation = model.AdditionalInformation,
                ElectronicPayorID=model.ElectronicPayorID,
                HasPolicyHolderSameCardName = model.HasPolicyHolderSameCardName
            };

            return entity;
        }


        public static PlanGroupAndAddressModelViewModel ToViewModel(this PlanGroupAndAddressesModel entity)
        {
            if (entity == null)
                return null;
            var model = new PlanGroupAndAddressModelViewModel
            {
                PayorAddresses = new List<AddressViewModel>()
            };

            if (entity.PayorAddresses != null && entity.PayorAddresses.Count > 0)
                entity.PayorAddresses.ForEach(delegate(AddressModel m)
                {
                    var transformedModel = m.ToViewModel();
                    model.PayorAddresses.Add(transformedModel);
                });
            model.PlanGroups = entity.PlanGroups;
            model.ModifiedOn = entity.ModifiedOn;
            return model;
        }


        public static Response<PlanGroupAndAddressModelViewModel> ToViewModel(this Response<PlanGroupAndAddressesModel> entity)
        {
            if (entity == null)
                return null;
            var model = entity.CloneResponse<PlanGroupAndAddressModelViewModel>();
            var lstViewModel = new List<PlanGroupAndAddressModelViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(PlanGroupAndAddressesModel m)
                {
                    var transformedModel = m.ToViewModel();
                    lstViewModel.Add(transformedModel);
                });
                model.DataItems = lstViewModel;
            }
            return model;
        }


        public static AddressModel ToModel(this AddressViewModel model)
        {
            if (model == null)
                return null;
            var entity = new AddressModel
            {
                AddressID = model.AddressID,
                AddressTypeID = model.AddressTypeID,
                ComplexName = model.ComplexName,
                City = model.City,
                County = model.County,
                ForceRollback = model.ForceRollback,
                GateCode = model.GateCode,
                IsActive = model.IsActive,
                Line1 = model.Line1,
                Line2 = model.Line2,
                ModifiedBy = model.ModifiedBy,
                ModifiedOn = model.ModifiedOn,
                StateProvince = model.StateProvince,
                Zip = model.Zip
            };
            return entity;
        }

        public static AddressViewModel ToViewModel(this AddressModel entity)
        {
            if (entity == null)
                return null;

            var model = new AddressViewModel
            {
                AddressID = entity.AddressID,
                AddressTypeID = entity.AddressTypeID,
                ComplexName = entity.ComplexName,
                City = entity.City,
                County = entity.County,
                ForceRollback = entity.ForceRollback,
                GateCode = entity.GateCode,
                IsActive = entity.IsActive,
                Line1 = entity.Line1,
                Line2 = entity.Line2,
                ModifiedBy = entity.ModifiedBy,
                ModifiedOn = entity.ModifiedOn,
                StateProvince = entity.StateProvince,
                Zip = entity.Zip
            };
            return model;
        }

    }
}