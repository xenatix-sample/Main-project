using Axis.Model.CallCenter;
using Axis.Model.Common;
using Axis.Plugins.CallCenter.Models;
using System.Collections.Generic;

namespace Axis.Plugins.CallCenter.Translator
{
    public static class CallCenterSummaryTranslator
    {

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static CallCenterSummaryViewModel ToViewModel(this CallCenterSummaryModel entity)
        {
            if (entity == null)
                return null;
            var model = new CallCenterSummaryViewModel
            {
                CallCenterID = entity.CallCenterID,
                CallCenterTypeID = entity.CallCenterTypeID,
                CallCenterHeaderID = entity.CallCenterHeaderID,
                MRN = entity.MRN,
                IncidentID = entity.IncidentID,
                CallDate = entity.CallDate,
                ClientTypeID = entity.ClientTypeID,
                Caller = entity.Caller,
                CallerContactNumber = entity.CallerContactNumber,
                ClientFirstName = entity.ClientFirstName,
                ClientLastName = entity.ClientLastName,
                DateofIncident = entity.DateofIncident,
                CallStatusID = entity.CallStatusID,
                CallStatus = entity.CallStatus,
                ProviderSubmittedBy = entity.ProviderSubmittedBy,
                ReferringAgencyID = entity.ReferringAgencyID,
                ReferringAgency = entity.ReferringAgency,
                Color = entity.Color,
                ProgramUnitID = entity.ProgramUnitID,
                ContactID = entity.ContactID,
                SignedOn = entity.SignedOn,
                ServiceRecordingID = entity.ServiceRecordingID,
                IsSignedByUser = entity.IsSignedByUser,
                IsVoided = entity.IsVoided,
                ContactProgramUnit = entity.ContactProgramUnit,
                NoteHeaderID = entity.NoteHeaderID,
                CountyID = entity.CountyID,
                ServiceItemID = entity.ServiceItemID,
                ServiceStatusID = entity.ServiceStatusID,
                TrackingField = entity.TrackingField,
                RecipientCodeID = entity.RecipientCodeID,
                AttendanceStatusID = entity.AttendanceStatusID,
                SuicideHomicideID = entity.SuicideHomicideID,
                CallCenterPriorityID = entity.CallCenterPriorityID,
                ReasonCalled = entity.ReasonCalled,
                Disposition = entity.Disposition,
                OtherInformation = entity.OtherInformation,
                BeginDate = entity.BeginDate,
                EndDate = entity.EndDate,
                FollowUpRequired = entity.FollowUpRequired,
                ServiceTypeID = entity.ServiceTypeID,
                ParentCallCenterHeaderID = entity.ParentCallCenterHeaderID,
                HasChild = entity.HasChild,
                Status = (entity.IsVoided == true) ? "Void" : (entity.SignedOn != null) ? "Complete" : "Draft",
                IsCreatorAccess = entity.IsCreatorAccess,
                IsManagerAccess = entity.IsManagerAccess,
                NatureofCall=entity.NatureofCall,
                ServiceEndDate= entity.ServiceEndDate,
                Duration = entity.Duration
            };
            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<CallCenterSummaryViewModel> ToViewModel(this Response<CallCenterSummaryModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<CallCenterSummaryViewModel>();
            var callCenterModel = new List<CallCenterSummaryViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate (CallCenterSummaryModel callModel)
                {
                    var transformedModel = callModel.ToViewModel();
                    callCenterModel.Add(transformedModel);
                });

                model.DataItems = callCenterModel;
            }

            return model;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static CallCenterSummaryModel ToModel(this CallCenterSummaryViewModel model)
        {
            if (model == null)
                return null;

            var entity = new CallCenterSummaryModel
            {
                CallCenterID = model.CallCenterID,
                CallCenterTypeID = model.CallCenterTypeID,
                CallCenterHeaderID = model.CallCenterHeaderID,
                MRN = model.MRN,
                IncidentID = model.IncidentID,
                CallDate = model.CallDate,
                Caller = model.Caller,
                ClientTypeID = model.ClientTypeID,
                CallerContactNumber = model.CallerContactNumber,
                ClientFirstName = model.ClientFirstName,
                ClientLastName = model.ClientLastName,
                DateofIncident = model.DateofIncident,
                CallStatusID = model.CallStatusID,
                CallStatus = model.CallStatus,
                ProviderSubmittedBy = model.ProviderSubmittedBy,
                ReferringAgencyID = model.ReferringAgencyID,
                ReferringAgency = model.ReferringAgency,
                ProgramUnitID = model.ProgramUnitID,
                ContactID = model.ContactID,
                SignedOn = model.SignedOn,
                ServiceRecordingID = model.ServiceRecordingID,
                IsSignedByUser = model.IsSignedByUser,
                IsVoided = model.IsVoided,
                ContactProgramUnit = model.ContactProgramUnit,
                NoteHeaderID = model.NoteHeaderID,
                CountyID = model.CountyID,
                ServiceItemID = model.ServiceItemID,
                ServiceStatusID = model.ServiceStatusID,
                TrackingField = model.TrackingField,
                RecipientCodeID = model.RecipientCodeID,
                AttendanceStatusID = model.AttendanceStatusID,
                SuicideHomicideID = model.SuicideHomicideID,
                CallCenterPriorityID = model.CallCenterPriorityID,
                ReasonCalled = model.ReasonCalled,
                Disposition = model.Disposition,
                OtherInformation = model.OtherInformation,
                BeginDate = model.BeginDate,
                EndDate = model.EndDate,
                FollowUpRequired = model.FollowUpRequired,
                ServiceTypeID = model.ServiceTypeID,
                ParentCallCenterHeaderID = model.ParentCallCenterHeaderID,
                HasChild = model.HasChild,
                IsCreatorAccess = model.IsCreatorAccess,
                IsManagerAccess = model.IsManagerAccess,
                NatureofCall=model.NatureofCall,
                ServiceEndDate= model.ServiceEndDate,
                Duration = model.Duration
            };
            return entity;
        }

    }
}
