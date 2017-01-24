using Axis.Model.Common;
using Axis.Model.Scheduling;
using Axis.Plugins.Scheduling.Models;
using System.Collections.Generic;

namespace Axis.Plugins.Scheduling.Translator
{
    /// <summary>
    /// Translates model to viewmodel and vice versa
    /// </summary>
    public static class AppointmentDetailTranslator
    {
        public static AppointmentDetailViewModel ToViewModel(this AppointmentDetailModel entity)
        {
            if (entity == null)
                return null;

            var model = new AppointmentDetailViewModel
            {
                AppointmentID = entity.AppointmentID,
                ProgramID = entity.ProgramID,
                AppointmentTypeID = entity.AppointmentTypeID,
                ServicesID = entity.ServicesID,
                AppointmentDate = entity.AppointmentDate,
                AppointmentStartTime = entity.AppointmentStartTime,
                AppointmentLength = entity.AppointmentLength,
                SupervisionVisit = entity.SupervisionVisit,
                ReferredBy = entity.ReferredBy,
                ReasonForVisit = entity.ReasonForVisit,
                IsCancelled = entity.IsCancelled,
                AppointmentType = entity.AppointmentType,
                ProviderId = entity.ProviderId,
                ProviderName = entity.ProviderName,
                RoomId = entity.RoomId,
                RoomName = entity.RoomName,
                LocationID = entity.LocationID,
                LocationName = entity.LocationName,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        public static Response<AppointmentDetailViewModel> ToViewModel(this Response<AppointmentDetailModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<AppointmentDetailViewModel>();
            var appointmentList = new List<AppointmentDetailViewModel>();

            if (entity.DataItems == null) return model;

            entity.DataItems.ForEach(delegate(AppointmentDetailModel appointment)
            {
                var transformModel = appointment.ToViewModel();
                appointmentList.Add(transformModel);
            });
            model.DataItems = appointmentList;

            return model;
        }

        /// <summary>
        /// Converts viewmodel to model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static AppointmentDetailModel ToModel(this AppointmentDetailViewModel model)
        {
            if (model == null)
                return null;
            var entity = new AppointmentDetailModel
            {
                AppointmentID = model.AppointmentID,
                ProgramID = model.ProgramID,
                AppointmentTypeID = model.AppointmentTypeID,
                ServicesID = model.ServicesID,
                AppointmentDate = model.AppointmentDate,
                AppointmentStartTime = model.AppointmentStartTime,
                AppointmentLength = model.AppointmentLength,
                SupervisionVisit = model.SupervisionVisit,
                ReferredBy = model.ReferredBy,
                ReasonForVisit = model.ReasonForVisit,
                IsCancelled = model.IsCancelled,
                AppointmentType = model.AppointmentType,
                ProviderId = model.ProviderId,
                ProviderName = model.ProviderName,
                RoomId = model.RoomId,
                RoomName = model.RoomName,
                LocationID = model.LocationID,
                LocationName = model.LocationName,
                ModifiedOn = model.ModifiedOn
            };
            return entity;
        }
    }
}