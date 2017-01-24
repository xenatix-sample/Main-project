using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Plugins.Registration.Model;
using System.Collections.Generic;

namespace Axis.Plugins.Registration.Translator
{
    /// <summary>
    /// Translates Contact Address modle to view model
    /// </summary>
    public static class ContactAddressTranslator
    {
        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static ContactAddressViewModel ToViewModel(this  ContactAddressModel entity)
        {
            if (entity == null)
                return null;

            var model = new ContactAddressViewModel
            {
                ContactAddressID = entity.ContactAddressID,
                ContactID = entity.ContactID,
                IsPrimary = entity.IsPrimary,
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
                ModifiedOn = entity.ModifiedOn,
                EffectiveDate=entity.EffectiveDate,
                ExpirationDate=entity.ExpirationDate,
                MailPermissionID=entity.MailPermissionID
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<ContactAddressViewModel> ToViewModel(this Response<ContactAddressModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<ContactAddressViewModel>();
            var contactAddress = new List<ContactAddressViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(ContactAddressModel address)
                {
                    var transformedModel = address.ToViewModel();
                    contactAddress.Add(transformedModel);
                });

                model.DataItems = contactAddress;
            }

            return model;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static ContactAddressModel ToModel(this ContactAddressViewModel model)
        {
            if (model == null)
                return null;

            var entity = new ContactAddressModel
            {
                ContactAddressID = model.ContactAddressID,
                ContactID = model.ContactID,
                IsPrimary = model.IsPrimary,
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
                ModifiedOn = model.ModifiedOn,
                ExpirationDate=model.ExpirationDate,
                EffectiveDate=model.EffectiveDate,
                MailPermissionID=model.MailPermissionID
            };

            return entity;
        }

        public static List<ContactAddressModel> ToModel(this List<ContactAddressViewModel> model)
        {
            if (model == null)
                return null;

            var retAddress = new List<ContactAddressModel>();


            model.ForEach(delegate(ContactAddressViewModel address)
            {
                var transformedModel = address.ToModel();
                retAddress.Add(transformedModel);
            });
            return retAddress;
        }
    }
}