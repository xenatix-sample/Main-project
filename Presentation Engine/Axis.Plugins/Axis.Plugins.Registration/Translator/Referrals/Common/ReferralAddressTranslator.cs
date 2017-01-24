using Axis.Model.Common;
using Axis.Model.Registration.Referrals.Common;
using Axis.Plugins.Registration.Models.Referrals.Common;
using System.Collections.Generic;

namespace Axis.Plugins.Registration.Translator.Referrals.Common
{
    public static class ReferralAddressTranslator
    {
        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static ReferralAddressViewModel ToViewModel(this ReferralAddressModel entity)
        {
            if (entity == null)
                return null;

            var model = new ReferralAddressViewModel
            {
                ReferralAddressID = entity.ReferralAddressID,
                ReferralID = entity.ReferralID,
                MailPermissionID = entity.MailPermissionID,
                IsPrimary = entity.IsPrimary,
                IsDeleted = entity.IsDeleted,
                AddressID = entity.AddressID,
                AddressTypeID = entity.AddressTypeID,
                Line1 = entity.Line1,
                Line2 = entity.Line2,
                City = entity.City,
                StateProvince = entity.StateProvince,
                County = entity.County,
                Zip = entity.Zip,
                ComplexName = entity.ComplexName,
                GateCode = entity.GateCode,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<ReferralAddressViewModel> ToViewModel(this Response<ReferralAddressModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<ReferralAddressViewModel>();
            var referralAddress = new List<ReferralAddressViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(ReferralAddressModel referralAddressCome)
                {
                    var transformedModel = referralAddressCome.ToViewModel();
                    referralAddress.Add(transformedModel);
                });

                model.DataItems = referralAddress;
            }

            return model;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static ReferralAddressModel ToModel(this ReferralAddressViewModel model)
        {
            if (model == null)
                return null;

            var entity = new ReferralAddressModel
            {
                ReferralAddressID = model.ReferralAddressID,
                ReferralID = model.ReferralID,
                MailPermissionID = model.MailPermissionID,
                IsPrimary = model.IsPrimary,
                IsDeleted = model.IsDeleted,
                AddressID = model.AddressID,
                AddressTypeID = model.AddressTypeID,
                Line1 = model.Line1,
                Line2 = model.Line2,
                City = model.City,
                StateProvince = model.StateProvince,
                County = model.County,
                Zip = model.Zip,
                ComplexName = model.ComplexName,
                GateCode = model.GateCode,
                ModifiedOn = model.ModifiedOn
            };

            return entity;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static List<ReferralAddressModel> ToModel(this List<ReferralAddressViewModel> model)
        {
            if (model == null)
                return null;

            var entity = new List<ReferralAddressModel>();

            model.ForEach(delegate(ReferralAddressViewModel address)
            {
                var transformedModel = address.ToModel();
                entity.Add(transformedModel);
            });

            return entity;
        }
    }
}