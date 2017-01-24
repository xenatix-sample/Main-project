using Axis.Model.Address;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Plugins.Registration.Model;
using Axis.Plugins.Registration.Models;
using System.Collections.Generic;

namespace Axis.Plugins.Registration.Translator
{
    public static class PayorDetailTranslator
    {

        public static PayorDetailViewModel ToViewModel(this PayorDetailModel entity)
        {
            if (entity == null)
                return null;

            var model = new PayorDetailViewModel
            {
                PayorID = entity.PayorID,
                PayorName = entity.PayorName,
                PayorCode = entity.PayorCode,
                PlanID = entity.PlanID,
                GroupName = entity.GroupName,
                GroupID = entity.GroupID,
                PayorGroupID=entity.PayorGroupID,
                ElectronicPayorID=entity.ElectronicPayorID,
                PayorAddressID=entity.PayorAddressID,
                PlanName=entity.PlanName,
                PayorPlanID=entity.PayorPlanID,
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
                StateProvinceName=entity.StateProvinceName,
                Zip = entity.Zip
            };

            return model;
        }

        public static Response<PayorDetailViewModel> ToViewModel(this Response<PayorDetailModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<PayorDetailViewModel>();
            var payorDetails = new List<PayorDetailViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate (PayorDetailModel payorDetail)
                {
                    var transformedModel = payorDetail.ToViewModel();
                    payorDetails.Add(transformedModel);
                });

                model.DataItems = payorDetails;
            }

            return model;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static PayorDetailModel ToModel(this PayorDetailViewModel model)
        {
            if (model == null)
                return null;

            var entity = new PayorDetailModel
            {
                PayorID = model.PayorID,
                PlanID = model.PlanID,
                GroupName = model.GroupName,
                GroupID = model.GroupID,
                PayorGroupID=model.PayorGroupID,
                PayorName = model.PayorName,
                PayorCode = model.PayorCode,
                ElectronicPayorID = model.ElectronicPayorID,
                PayorAddressID = model.PayorAddressID,
                PlanName = model.PlanName,
                PayorPlanID=model.PayorPlanID,
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
                Zip = model.Zip,
                StateProvinceName = model.StateProvinceName

            };

            return entity;
        }

        
       

    }
}
