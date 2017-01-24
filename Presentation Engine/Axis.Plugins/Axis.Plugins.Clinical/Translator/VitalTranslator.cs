using System;
using Axis.Model.Clinical;
using Axis.Model.Common;
using System.Collections.Generic;
using Axis.Plugins.Clinical.Models.Vital;

namespace Axis.Plugins.Clinical.Translator
{
    /// <summary>
    ///
    /// </summary>
    public static class VitalTranslator
    {
        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static VitalViewModel ToViewModel(this VitalModel entity)
        {
            if (entity == null)
                return null;

            var model = new VitalViewModel
            {
                VitalID = entity.VitalID,
                ContactID = entity.ContactID,
                TakenBy = entity.TakenBy,
                TakenTime = entity.TakenTime,
                HeightFeet = entity.HeightFeet,
                HeightInches = entity.HeightInches,
                WeightLbs = entity.WeightLbs,
                WeightOz = entity.WeightOz,
                BMI = entity.BMI,
                LMP = entity.LMP,
                BPMethodID = entity.BPMethodID,
                Systolic = entity.Systolic,
                Diastolic = entity.Diastolic,
                OxygenSaturation = entity.OxygenSaturation,
                RespiratoryRate = entity.RespiratoryRate,
                Pulse = entity.Pulse,
                Temperature = entity.Temperature,
                Glucose = entity.Glucose,
                WaistCircumference = entity.WaistCircumference,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<VitalViewModel> ToViewModel(this Response<VitalModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<VitalViewModel>();
            var contactVitals = new List<VitalViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(VitalModel contactVital)
                {
                    var transformedModel = contactVital.ToViewModel();
                    contactVitals.Add(transformedModel);
                });

                model.DataItems = contactVitals;
            }

            return model;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static VitalModel ToModel(this VitalViewModel model)
        {
            if (model == null)
                return null;

            var entity = new VitalModel
            {
                VitalID = model.VitalID,
                ContactID = model.ContactID,
                TakenBy = model.TakenBy,
                TakenTime = model.TakenTime,
                HeightFeet = model.HeightFeet,
                HeightInches = model.HeightInches,
                WeightLbs = model.WeightLbs,
                WeightOz = model.WeightOz,
                BMI = model.BMI,
                LMP = model.LMP,
                BPMethodID = model.BPMethodID,
                Systolic = model.Systolic,
                Diastolic = model.Diastolic,
                OxygenSaturation = model.OxygenSaturation,
                RespiratoryRate = model.RespiratoryRate,
                Pulse = model.Pulse,
                Temperature = model.Temperature,
                Glucose = model.Glucose,
                WaistCircumference = model.WaistCircumference,
                ModifiedOn = model.ModifiedOn
            };

            return entity;
        }
    }
}
