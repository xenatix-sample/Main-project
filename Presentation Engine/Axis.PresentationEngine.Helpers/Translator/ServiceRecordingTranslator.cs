using Axis.Model.CallCenter;
using Axis.Model.Common;
using Axis.PresentationEngine.Helpers.Model;
using System;
using System.Collections.Generic;


namespace Axis.PresentationEngine.Helpers.Translator.ServiceRecording
{
    public static class ServiceRecordingTranslator
    {
        public static ServiceRecordingViewModel ToViewModel(this ServiceRecordingModel model)
        {
            if (model == null)
                return null;

            var entity = new ServiceRecordingViewModel
            {
                ServiceRecordingID = model.ServiceRecordingID,
                ServiceRecordingSourceID = model.ServiceRecordingSourceID,
                ServiceRecordingHeaderID=model.ServiceRecordingHeaderID,
                ParentServiceRecordingID = model.ParentServiceRecordingID,
                CallCenterHeaderID = model.CallCenterHeaderID,
                ServiceItemID = model.ServiceItemID,
                AttendanceStatusID = model.AttendanceStatusID,
                DeliveryMethodID = model.DeliveryMethodID,
                ServiceStatusID = model.ServiceStatusID,
                ServiceLocationID = model.ServiceLocationID,
                RecipientCodeID = model.RecipientCodeID,
                RecipientCode = model.RecipientCode,
                TrackingFieldID=model.TrackingFieldID,
                NumberOfRecipients = model.NumberOfRecipients,
                ConversionStatusID = model.ConversionStatusID,
                ServiceRecordingVoidID = model.ServiceRecordingVoidID,
                ConversionDateTime = model.ConversionDateTime,
                SourceHeaderID=model.SourceHeaderID,
                EndDate = model.EndDate,
                UserID = model.UserID,
                OrganizationID = model.OrganizationID,
                ServiceTypeID = model.ServiceTypeID,
                ServiceStartDate = model.ServiceStartDate,
                ServiceStartTime = model.ServiceStartTime,
                ServiceEndDate = model.ServiceEndDate,
                ServiceEndTime = model.ServiceEndTime,
                SupervisorUserID = model.SupervisorUserID,
                AttendedList = model.AttendedList,
                AdditionalUserList = model.AdditionalUserList,
                DocumentStatusID = model.DocumentStatusID,
                ModifiedOn = model.ModifiedOn,
                CallStatusID = model.CallStatusID,
                IsVoided = model.IsVoided,
                SignedOn = model.SignedOn,
                SentToCMHCDate = model.SentToCMHCDate,
                SystemModifiedOn = model.SystemModifiedOn,
                Duration=model.Duration,
                ServiceDurationID=model.ServiceDurationID
            };
            return entity;
        }


        public static Response<ServiceRecordingViewModel> ToViewModel(this Response<ServiceRecordingModel> model)
        {
            if (model == null)
                return null;

            var entity = model.CloneResponse<ServiceRecordingViewModel>();
            var serviceRecordingModel = new List<ServiceRecordingViewModel>();

            if (model.DataItems != null)
            {
                model.DataItems.ForEach(delegate(ServiceRecordingModel callerInfoModel)
                {
                    var transformedModel = callerInfoModel.ToViewModel();
                    serviceRecordingModel.Add(transformedModel);
                });

                entity.DataItems = serviceRecordingModel;
            }

            return entity;
        }

        public static ServiceRecordingModel ToModel(this ServiceRecordingViewModel model)
        {
            if (model == null)
                return null;

            var entity = new ServiceRecordingModel
            {
                ServiceRecordingID = model.ServiceRecordingID,
                RecipientCode = model.RecipientCode,
                ServiceRecordingSourceID = model.ServiceRecordingSourceID,
                ParentServiceRecordingID=model.ParentServiceRecordingID,
                ServiceRecordingHeaderID = model.ServiceRecordingHeaderID,
                CallCenterHeaderID = model.CallCenterHeaderID,
                ServiceItemID = model.ServiceItemID,
                AttendanceStatusID = model.AttendanceStatusID,
                ServiceRecordingVoidID=model.ServiceRecordingVoidID,
                DeliveryMethodID = model.DeliveryMethodID,
                ServiceStatusID = model.ServiceStatusID,
                SourceHeaderID = model.SourceHeaderID,
                ServiceLocationID = model.ServiceLocationID,
                RecipientCodeID = model.RecipientCodeID,
                TrackingFieldID = model.TrackingFieldID,
                NumberOfRecipients = model.NumberOfRecipients,
                ConversionStatusID = model.ConversionStatusID,
                ConversionDateTime = model.ConversionDateTime,
                EndDate = model.EndDate,
                UserID = model.UserID,
                OrganizationID = model.OrganizationID,
                ServiceTypeID = model.ServiceTypeID,
                ServiceStartDate = model.ServiceStartDate,
                ServiceStartTime = model.ServiceStartTime,
                ServiceEndDate = model.ServiceEndDate,
                ServiceEndTime = model.ServiceEndTime,
                SupervisorUserID = model.SupervisorUserID,
                AttendedList = model.AttendedList,
                AdditionalUserList = model.AdditionalUserList,
                DocumentStatusID = model.DocumentStatusID,
                ModifiedOn = model.ModifiedOn,
                CallStatusID = model.CallStatusID,
                IsVoided = model.IsVoided,
                SignedOn = model.SignedOn,
                SentToCMHCDate = model.SentToCMHCDate,
                SystemModifiedOn = model.SystemModifiedOn,
                Duration=model.Duration,
                ServiceDurationID=model.ServiceDurationID
                
            };
            return entity;
        }
    }
}
