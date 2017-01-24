using Axis.Model.RecordedServices;
using Axis.Model.Common;
using Axis.PresentationEngine.Areas.RecordedServices.Models;
using System.Collections.Generic;

namespace Axis.PresentationEngine.Areas.RecordedServices.Translator
{
    public static class VoidServiceTranslator
    {
        public static VoidServiceModel ToModel(this VoidServiceViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new VoidServiceModel
            {
                ServiceRecordingVoidID = entity.ServiceRecordingVoidID,
                ServiceRecordingID = entity.ServiceRecordingID,
                ServiceRecordingVoidReasonID = entity.ServiceRecordingVoidReasonID,
                IncorrectOrganization = entity.IncorrectOrganization,
                IncorrectServiceType = entity.IncorrectServiceType,
                IncorrectServiceItem = entity.IncorrectServiceItem,
                IncorrectServiceStatus = entity.IncorrectServiceStatus,
                IncorrectSupervisor = entity.IncorrectSupervisor,
                IncorrectAdditionalUser = entity.IncorrectAdditionalUser,
                IncorrectAttendanceStatus = entity.IncorrectAttendanceStatus,
                IncorrectStartDate = entity.IncorrectStartDate,
                IncorrectStartTime = entity.IncorrectStartTime,
                IncorrectEndDate = entity.IncorrectEndDate,
                IncorrectEndTime = entity.IncorrectEndTime,
                IncorrectDeliveryMethod = entity.IncorrectDeliveryMethod,
                IncorrectServiceLocation = entity.IncorrectServiceLocation,
                IncorrectRecipientCode= entity.IncorrectRecipientCode,
                IncorrectTrackingField = entity.IncorrectTrackingField,
                Comments = entity.Comments,
                IsCreateCopyToEdit = entity.IsCreateCopyToEdit,
                ModifiedOn = entity.ModifiedOn,
                ModifiedBy=entity.ModifiedBy,
                ContactID=entity.ContactID,
                NoteHeaderID = entity.NoteHeaderID
            };

            return model;
        }

        public static VoidServiceViewModel ToModel(this VoidServiceModel entity)
        {
            if (entity == null)
                return null;

            var model = new VoidServiceViewModel
            {
                ServiceRecordingVoidID = entity.ServiceRecordingVoidID,
                ServiceRecordingID = entity.ServiceRecordingID,
                ServiceRecordingVoidReasonID = entity.ServiceRecordingVoidReasonID,
                IncorrectOrganization = entity.IncorrectOrganization,
                IncorrectServiceType = entity.IncorrectServiceType,
                IncorrectServiceItem = entity.IncorrectServiceItem,
                IncorrectServiceStatus = entity.IncorrectServiceStatus,
                IncorrectSupervisor = entity.IncorrectSupervisor,
                IncorrectAdditionalUser = entity.IncorrectAdditionalUser,
                IncorrectAttendanceStatus = entity.IncorrectAttendanceStatus,
                IncorrectStartDate = entity.IncorrectStartDate,
                IncorrectStartTime = entity.IncorrectStartTime,
                IncorrectEndDate = entity.IncorrectEndDate,
                IncorrectEndTime = entity.IncorrectEndTime,
                IncorrectDeliveryMethod = entity.IncorrectDeliveryMethod,
                IncorrectServiceLocation = entity.IncorrectServiceLocation,
                IncorrectRecipientCode = entity.IncorrectRecipientCode,
                IncorrectTrackingField = entity.IncorrectTrackingField,
                Comments = entity.Comments,
                IsCreateCopyToEdit = entity.IsCreateCopyToEdit,
                ModifiedOn = entity.ModifiedOn,
                ModifiedBy = entity.ModifiedBy,
                ContactID = entity.ContactID,
                NoteHeaderID = entity.NoteHeaderID
            };

            return model;
        }

        public static Response<VoidServiceViewModel> ToViewModel(this Response<VoidServiceModel> entity)
        {
            if (entity == null)
                return null;

            var dataItems = new List<VoidServiceViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(VoidServiceModel VoidServiceModel)
                {
                    var transformedModel = VoidServiceModel.ToModel();
                    dataItems.Add(transformedModel);
                });
            }
            else
                dataItems = null;

            var model = new Response<VoidServiceViewModel>
            {
                ResultCode = entity.ResultCode,
                ResultMessage = entity.ResultMessage,
                DataItems = dataItems,
                RowAffected = entity.RowAffected,
                AdditionalResult = entity.AdditionalResult
            };

            return model;
        }
    }
}