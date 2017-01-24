using System.Collections.Generic;
using Axis.Model.Common;
using Axis.Model.Scheduling;
using Axis.Plugins.Scheduling.Models;

namespace Axis.Plugins.Scheduling.Translator
{
    public static class GroupSchedulingTranslator
    {
        public static GroupSchedulingViewModel ToViewModel(this GroupSchedulingModel entity)
        {
            if (entity == null)
                return null;

            List<GroupSchedulingDetailViewModel> details = new List<GroupSchedulingDetailViewModel>();
            foreach (var grpDetail in entity.GroupDetails)
            {
                var detailModel = new GroupSchedulingDetailViewModel();
                detailModel.GroupCapacity = grpDetail.GroupCapacity;
                detailModel.GroupDetailID = grpDetail.GroupDetailID;
                detailModel.GroupName = grpDetail.GroupName;
                detailModel.GroupTypeID = grpDetail.GroupTypeID;
                detailModel.ModifiedOn = grpDetail.ModifiedOn;
                detailModel.IsActive = grpDetail.IsActive;
                details.Add(detailModel);
            }

            var model = new GroupSchedulingViewModel()
            {
                GroupHeaderID = entity.GroupHeaderID,
                GroupDetailID = entity.GroupDetailID,
                Comments = entity.Comments,
                GroupDetails = details,
                ModifiedOn = entity.ModifiedOn,
                IsActive = entity.IsActive
            };

            return model;
        }

        public static GroupSchedulingModel ToModel(this GroupSchedulingViewModel entity)
        {
            if (entity == null)
                return null;

            List<GroupSchedulingDetailModel> details = new List<GroupSchedulingDetailModel>();
            foreach (var grpDetail in entity.GroupDetails)
            {
                var detailModel = new GroupSchedulingDetailModel();
                detailModel.GroupCapacity = grpDetail.GroupCapacity;
                detailModel.GroupDetailID = grpDetail.GroupDetailID;
                detailModel.GroupName = grpDetail.GroupName;
                detailModel.GroupTypeID = grpDetail.GroupTypeID;
                detailModel.ModifiedOn = grpDetail.ModifiedOn;
                detailModel.IsActive = grpDetail.IsActive;
                details.Add(detailModel);
            }

            var model = new GroupSchedulingModel()
            {
                GroupHeaderID = entity.GroupHeaderID,
                GroupDetailID = entity.GroupDetailID,
                Comments = entity.Comments,
                GroupDetails = details,
                ModifiedOn = entity.ModifiedOn,
                IsActive = entity.IsActive
            };

            return model;
        }

        public static Response<GroupSchedulingViewModel> ToModel(this Response<GroupSchedulingModel> entity)
        {
            if (entity == null)
                return null;

            var dataItems = new List<GroupSchedulingViewModel>();
            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(GroupSchedulingModel groupSchedulingModel)
                {
                    var transformedModel = groupSchedulingModel.ToViewModel();
                    dataItems.Add(transformedModel);
                });
            }
            else
                dataItems = null;

            var model = new Response<GroupSchedulingViewModel>()
            {
                ResultCode = entity.ResultCode,
                ResultMessage = entity.ResultMessage,
                DataItems = dataItems,
                RowAffected = entity.RowAffected,
                AdditionalResult = entity.AdditionalResult,
                ID = entity.ID
            };

            return model;
        }
    }
}
