using Axis.Model.Common;
using Axis.Model.Scheduling;
using Axis.Plugins.Scheduling.Models;
using System.Collections.Generic;

namespace Axis.Plugins.Scheduling.Translator
{
    /// <summary>
    /// Translates model to viewmodel and vice versa
    /// </summary>
    public static class AppointmentTranslator
    {
        /// <summary>
        /// Converts model to viewmodel
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static AppointmentViewModel ToViewModel(this AppointmentModel entity)
        {
            if (entity == null)
                return null;

            var model = new AppointmentViewModel
            {
                AppointmentID = entity.AppointmentID,
                ContactID = entity.ContactID,
                ProgramID = entity.ProgramID,
                AppointmentTypeID = entity.AppointmentTypeID,
                ServicesID = entity.ServicesID,
                ServiceStatusID = entity.ServiceStatusID,
                AppointmentDate = entity.AppointmentDate,
                AppointmentStartTime = entity.AppointmentStartTime,
                AppointmentLength = entity.AppointmentLength,
                SupervisionVisit = entity.SupervisionVisit,
                ReferredBy = entity.ReferredBy,
                ReasonForVisit = entity.ReasonForVisit,
                RecurrenceID = entity.RecurrenceID,
                IsCancelled = entity.IsCancelled,
                Comments = entity.Comments,
                IsInterpreterRequired = entity.IsInterpreterRequired,
                NonMHMRAppointment = entity.NonMHMRAppointment,
                FacilityID = entity.FacilityID,
                ModifiedOn = entity.ModifiedOn,
                CancelComment = entity.CancelComment,
                CancelReasonID = entity.CancelReasonID,
                IsGroupAppointment = entity.IsGroupAppointment,
                AppointmentStatusID = entity.AppointmentStatusID,
                GroupName = entity.GroupName,
                GroupType = entity.GroupType,
                AppointmentType = entity.AppointmentType,
                ServiceName = entity.ServiceName,
                GroupID = entity.GroupID,
                RecurrenceDay = entity.RecurrenceDay,
                RecurrenceEndDate = entity.RecurrenceEndDate,
                RecurrenceFrequency = entity.RecurrenceFrequency,
                NumberOfOccurences = entity.NumberOfOccurences,
                RecurrID = entity.RecurrID,
                IsRecurringAptEdit = entity.IsRecurringAptEdit
            };

            if (entity.Recurrence != null)
                model.Recurrence = entity.Recurrence.ToViewModel();

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<AppointmentViewModel> ToViewModel(this Response<AppointmentModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<AppointmentViewModel>();
            var appointmentList = new List<AppointmentViewModel>();

            if (entity.DataItems == null) return model;

            entity.DataItems.ForEach(delegate(AppointmentModel appointment)
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
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static AppointmentModel ToModel(this AppointmentViewModel model)
        {
            if (model == null)
                return null;
            var entity = new AppointmentModel
            {
                AppointmentID = model.AppointmentID,
                ContactID = model.ContactID,
                ProgramID = model.ProgramID,
                AppointmentTypeID = model.AppointmentTypeID,
                ServicesID = model.ServicesID,
                ServiceStatusID = model.ServiceStatusID,
                AppointmentDate = model.AppointmentDate,
                AppointmentStartTime = model.AppointmentStartTime,
                AppointmentLength = model.AppointmentLength,
                SupervisionVisit = model.SupervisionVisit,
                ReferredBy = model.ReferredBy,
                ReasonForVisit = model.ReasonForVisit,
                RecurrenceID = model.RecurrenceID,
                IsCancelled = model.IsCancelled,
                Comments = model.Comments,
                IsInterpreterRequired = model.IsInterpreterRequired,
                NonMHMRAppointment = model.NonMHMRAppointment,
                FacilityID = model.FacilityID,
                ModifiedOn = model.ModifiedOn,
                IsCancelAllAppoitment = model.IsCancelAllAppoitment,
                CancelReasonID = model.CancelReasonID,
                CancelComment = model.CancelComment,
                IsGroupAppointment = model.IsGroupAppointment,
                GroupName = model.GroupName,
                GroupType = model.GroupType,
                AppointmentType = model.AppointmentType,
                ServiceName = model.ServiceName,
                RecurrenceDay = model.RecurrenceDay,
                RecurrenceEndDate = model.RecurrenceEndDate,
                RecurrenceFrequency = model.RecurrenceFrequency,
                NumberOfOccurences = model.NumberOfOccurences,
                RecurrID = model.RecurrID,
                IsRecurringAptEdit = model.IsRecurringAptEdit
            };

            if (model.Recurrence != null)
                entity.Recurrence = model.Recurrence.ToModel();

            return entity;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static AppointmentResourceViewModel ToViewModel(this AppointmentResourceModel entity)
        {
            if (entity == null)
                return null;

            var model = new AppointmentResourceViewModel
            {
                AppointmentID = entity.AppointmentID,
                AppointmentResourceID = entity.AppointmentResourceID,
                ResourceID = entity.ResourceID,
                ResourceTypeID = entity.ResourceTypeID,
                ParentID = entity.ParentID,
                ModifiedOn = entity.ModifiedOn,
                IsActive = entity.IsActive,
                IsNoShow = entity.IsNoShow,
                AppointmentStatusID = entity.AppointmentStatusID,
                GroupHeaderID = entity.GroupHeaderID,
                AppointmentStatusDetailID = entity.AppointmentStatusDetailID
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<AppointmentResourceViewModel> ToViewModel(this Response<AppointmentResourceModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<AppointmentResourceViewModel>();
            var appointmentResourceList = new List<AppointmentResourceViewModel>();

            if (entity.DataItems == null) return model;

            entity.DataItems.ForEach(delegate(AppointmentResourceModel appointmentResource)
            {
                var transformModel = appointmentResource.ToViewModel();
                appointmentResourceList.Add(transformModel);
            });
            model.DataItems = appointmentResourceList;

            return model;
        }

        /// <summary>
        /// Converts viewmodel to model
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static AppointmentResourceModel ToModel(this AppointmentResourceViewModel model)
        {
            if (model == null)
                return null;
            var entity = new AppointmentResourceModel
            {
                AppointmentID = model.AppointmentID,
                AppointmentResourceID = model.AppointmentResourceID,
                ResourceID = model.ResourceID,
                ResourceTypeID = model.ResourceTypeID,
                ParentID = model.ParentID,
                ModifiedOn = model.ModifiedOn,
                IsActive = model.IsActive,
                IsNoShow = model.IsNoShow
            };
            return entity;
        }

        /// <summary>
        /// Converts viewmodel to model
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static AppointmentStatusDetailModel ToModel(this AppointmentStatusDetailViewModel model)
        {
            if (model == null)
                return null;
            var entity = new AppointmentStatusDetailModel
            {
                AppointmentResourceID = model.AppointmentResourceID,
                AppointmentStatusDetailID = model.AppointmentStatusDetailID,
                AppointmentStatusID = model.AppointmentStatusID,
                ModifiedOn = model.ModifiedOn,
                IsActive = model.IsActive,
                IsCancelled = model.IsCancelled,
                CancelReasonID = model.CancelReasonID,
                Comments = model.Comments
            };
            return entity;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static AppointmentStatusDetailViewModel ToViewModel(this AppointmentStatusDetailModel entity)
        {
            if (entity == null)
                return null;

            var model = new AppointmentStatusDetailViewModel
            {
                AppointmentStatusDetailID = entity.AppointmentStatusDetailID,
                AppointmentStatusID = entity.AppointmentStatusID,
                AppointmentResourceID = entity.AppointmentResourceID,
                ModifiedOn = entity.ModifiedOn,
                IsCancelled = entity.IsCancelled,
                CancelReasonID = entity.CancelReasonID,
                Comments = entity.Comments
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<AppointmentStatusDetailViewModel> ToViewModel(this Response<AppointmentStatusDetailModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<AppointmentStatusDetailViewModel>();
            var applist = new List<AppointmentStatusDetailViewModel>();

            if (entity.DataItems == null) return model;

            entity.DataItems.ForEach(delegate(AppointmentStatusDetailModel appointmentLength)
            {
                var transformModel = appointmentLength.ToViewModel();
                applist.Add(transformModel);
            });
            model.DataItems = applist;

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static AppointmentLengthViewModel ToViewModel(this AppointmentLengthModel entity)
        {
            if (entity == null)
                return null;

            var model = new AppointmentLengthViewModel
            {
                AppointmentTypeID = entity.AppointmentTypeID,
                AppointmentLength = entity.AppointmentLength,
                AppointmentLengthID = entity.AppointmentLengthID,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<AppointmentLengthViewModel> ToViewModel(this Response<AppointmentLengthModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<AppointmentLengthViewModel>();
            var appointmentLengthList = new List<AppointmentLengthViewModel>();

            if (entity.DataItems == null) return model;

            entity.DataItems.ForEach(delegate(AppointmentLengthModel appointmentLength)
            {
                var transformModel = appointmentLength.ToViewModel();
                appointmentLengthList.Add(transformModel);
            });
            model.DataItems = appointmentLengthList;

            return model;
        }

        /// <summary>
        /// Converts viewmodel to model
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static AppointmentLengthModel ToModel(this AppointmentLengthViewModel model)
        {
            if (model == null)
                return null;
            var entity = new AppointmentLengthModel
            {
                AppointmentTypeID = model.AppointmentTypeID,
                AppointmentLengthID = model.AppointmentLengthID,
                AppointmentLength = model.AppointmentLength,
                ModifiedOn = model.ModifiedOn
            };
            return entity;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static AppointmentTypeViewModel ToViewModel(this AppointmentTypeModel entity)
        {
            if (entity == null)
                return null;

            var model = new AppointmentTypeViewModel
            {
                AppointmentTypeID = entity.AppointmentTypeID,
                AppointmentType = entity.AppointmentType,
                ProgramID = entity.ProgramID,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<AppointmentTypeViewModel> ToViewModel(this Response<AppointmentTypeModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<AppointmentTypeViewModel>();
            var appointmentTypeList = new List<AppointmentTypeViewModel>();

            if (entity.DataItems == null) return model;

            entity.DataItems.ForEach(delegate(AppointmentTypeModel appointmentType)
            {
                var transformModel = appointmentType.ToViewModel();
                appointmentTypeList.Add(transformModel);
            });
            model.DataItems = appointmentTypeList;

            return model;
        }

        /// <summary>
        /// Converts viewmodel to model
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static AppointmentTypeModel ToModel(this AppointmentTypeViewModel model)
        {
            if (model == null)
                return null;
            var entity = new AppointmentTypeModel
            {
                AppointmentTypeID = model.AppointmentTypeID,
                AppointmentType = model.AppointmentType,
                ProgramID = model.ProgramID,
                ModifiedOn = model.ModifiedOn
            };
            return entity;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static AppointmentContactViewModel ToViewModel(this AppointmentContactModel entity)
        {
            if (entity == null)
                return null;

            var model = new AppointmentContactViewModel
            {
                AppointmentContactID = entity.AppointmentContactID,
                AppointmentID = entity.AppointmentID,
                ContactID = entity.ContactID,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<AppointmentContactViewModel> ToViewModel(this Response<AppointmentContactModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<AppointmentContactViewModel>();
            var appointmentContactList = new List<AppointmentContactViewModel>();

            if (entity.DataItems == null) return model;

            entity.DataItems.ForEach(delegate(AppointmentContactModel appointmentContact)
            {
                var transformModel = appointmentContact.ToViewModel();
                appointmentContactList.Add(transformModel);
            });
            model.DataItems = appointmentContactList;

            return model;
        }

        /// <summary>
        /// Converts viewmodel to model
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static AppointmentContactModel ToModel(this AppointmentContactViewModel model)
        {
            if (model == null)
                return null;
            var entity = new AppointmentContactModel
            {
                AppointmentContactID = model.AppointmentContactID,
                AppointmentID = model.AppointmentID,
                ContactID = model.ContactID,
                ModifiedOn = model.ModifiedOn
            };
            return entity;
        }

        public static AppointmentNoteModel ToModel(this AppointmentNoteViewModel model)
        {
            if (model == null)
                return null;
            var entity = new AppointmentNoteModel
            {
                AppointmentNoteID = model.AppointmentNoteID,
                AppointmentID = model.AppointmentID,
                ContactID = model.ContactID,
                GroupID = model.GroupID,
                UserID = model.UserID,
                NoteText = model.NoteText,
                NoteTypeID = model.NoteTypeID,
                ModifiedOn = model.ModifiedOn,
                IsActive = model.IsActive
            };
            return entity;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<AppointmentNoteViewModel> ToViewModel(this Response<AppointmentNoteModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<AppointmentNoteViewModel>();
            var appointmentList = new List<AppointmentNoteViewModel>();

            if (entity.DataItems == null) return model;

            entity.DataItems.ForEach(delegate(AppointmentNoteModel appointment)
            {
                var transformModel = appointment.ToViewModel();
                appointmentList.Add(transformModel);
            });
            model.DataItems = appointmentList;

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static AppointmentNoteViewModel ToViewModel(this AppointmentNoteModel entity)
        {
            if (entity == null)
                return null;

            var model = new AppointmentNoteViewModel
            {
                AppointmentNoteID = entity.AppointmentNoteID,
                AppointmentID = entity.AppointmentID,
                ContactID = entity.ContactID,
                GroupID = entity.GroupID,
                UserID = entity.UserID,
                NoteText = entity.NoteText,
                NoteTypeID = entity.NoteTypeID,
                ModifiedOn = entity.ModifiedOn,
                IsActive = entity.IsActive
            };

            return model;
        }
    }
}