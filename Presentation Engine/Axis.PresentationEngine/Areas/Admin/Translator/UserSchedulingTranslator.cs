using System.Linq;
using Axis.Model.Admin.UserScheduling;
using System.Collections.Generic;
using Axis.Model.Common;
using Axis.PresentationEngine.Areas.Admin.Models;
using Axis.PresentationEngine.Areas.Admin.Models.UserScheduling;

namespace Axis.PresentationEngine.Areas.Admin.Translator
{
    public static class UserSchedulingTranslator
    {
        public static UserSchedulingViewModel ToViewModel(this UserSchedulingModel entity)
        {
            if (entity == null)
                return null;

            var model = new UserSchedulingViewModel
            {
                FacilityID = entity.FacilityID,
                ResourceID = entity.ResourceID,
                UserFacilitySchedule = new List<UserFacilitySchedulingViewModel>(),
                FacilitySchedule = new List<UserFacilitySchedulingViewModel>(),
                ModifiedOn = entity.ModifiedOn
            };

            if (entity.UserFacilitySchedule != null)
            {
                entity.UserFacilitySchedule.ForEach(delegate(UserFacilitySchedulingModel schedule)
                {
                    var transformedModel = schedule.ToViewModel();
                    model.UserFacilitySchedule.Add(transformedModel);
                    model.RowCount += transformedModel.RowCount;
                });
            }

            if (entity.FacilitySchedule != null)
            {
                entity.FacilitySchedule.ForEach(delegate(UserFacilitySchedulingModel schedule)
                {
                    var transformedModel = schedule.ToViewModel();
                    model.FacilitySchedule.Add(transformedModel);
                });
            }

            var firstUserFacility = model.UserFacilitySchedule.FirstOrDefault();
            if (firstUserFacility != null) firstUserFacility.IsFirst = true;

            var lastUserFacility = model.UserFacilitySchedule.LastOrDefault();
            if (lastUserFacility != null) lastUserFacility.IsLast = true;

            return model;
        }

        public static UserFacilitySchedulingViewModel ToViewModel(this UserFacilitySchedulingModel entity)
        {
            if (entity == null)
                return null;

            var model = new UserFacilitySchedulingViewModel
            {
                DayOfWeekID = entity.DayOfWeekID,
                Days = entity.Days,
                ScheduleTypeID = (entity.ScheduleTypeID != null) ? entity.ScheduleTypeID : 4,
                UserFacilityTimeSchedule = new List<UserFacilityTimeSchedulingViewModel>(),
                ModifiedOn = entity.ModifiedOn
            };

            if (entity.UserFacilityTimeSchedule != null)
            {
                var rowNum = 0;
                entity.UserFacilityTimeSchedule.ForEach(delegate(UserFacilityTimeSchedulingModel userFacilityTime)
                    {
                        var transformedModel = userFacilityTime.ToViewModel();
                        transformedModel.RowNumber = rowNum;
                        transformedModel.IsActive = true;
                        rowNum++;
                        model.UserFacilityTimeSchedule.Add(transformedModel);
                    });
            }

            model.RowCount = model.UserFacilityTimeSchedule.Any() ? model.UserFacilityTimeSchedule.Count() : 1;
            var firstUserFacilityTime = model.UserFacilityTimeSchedule.FirstOrDefault();
            if (firstUserFacilityTime != null) firstUserFacilityTime.IsFirst = true;

            var lastUserFacilityTime = model.UserFacilityTimeSchedule.LastOrDefault();
            if (lastUserFacilityTime != null) lastUserFacilityTime.IsLast = true;

            return model;
        }

        public static UserFacilityTimeSchedulingViewModel ToViewModel(this UserFacilityTimeSchedulingModel entity)
        {
            if (entity == null)
                return null;

            var model = new UserFacilityTimeSchedulingViewModel
            {
                ResourceAvailabilityID = entity.ResourceAvailabilityID,
                AvailabilityStartTime = entity.AvailabilityStartTime,
                AvailabilityEndTime = entity.AvailabilityEndTime,
                IsActive = entity.IsActive,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        public static Response<UserSchedulingViewModel> ToViewModel(this Response<UserSchedulingModel> entity)
        {
            if (entity == null)
                return null;

            var dataItems = new List<UserSchedulingViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(UserSchedulingModel userScheduleModel)
                {
                    var transformedModel = userScheduleModel.ToViewModel();
                    dataItems.Add(transformedModel);
                });
            }
            else
            {
                dataItems = null;
            }

            var model = new Response<UserSchedulingViewModel>
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

        public static UserSchedulingModel ToModel(this UserSchedulingViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new UserSchedulingModel
            {
                FacilityID = entity.FacilityID,
                ResourceID = entity.ResourceID,
                UserFacilitySchedule = new List<UserFacilitySchedulingModel>(),
                ModifiedOn = entity.ModifiedOn
            };

            if (entity.UserFacilitySchedule != null)
            {
                entity.UserFacilitySchedule.ForEach(delegate(UserFacilitySchedulingViewModel schedule)
                {
                    var transformedModel = schedule.ToModel();
                    model.UserFacilitySchedule.Add(transformedModel);
                });
            }

            return model;
        }

        public static UserFacilitySchedulingModel ToModel(this UserFacilitySchedulingViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new UserFacilitySchedulingModel
            {
                DayOfWeekID = entity.DayOfWeekID,
                Days = entity.Days,
                ScheduleTypeID = entity.ScheduleTypeID,
                UserFacilityTimeSchedule = new List<UserFacilityTimeSchedulingModel>(),
                ModifiedOn = entity.ModifiedOn
            };

            if (entity.UserFacilityTimeSchedule != null)
            {
                entity.UserFacilityTimeSchedule.ForEach(delegate(UserFacilityTimeSchedulingViewModel userFacilityTime)
                {
                    var transformedModel = userFacilityTime.ToModel();
                    model.UserFacilityTimeSchedule.Add(transformedModel);
                });
            }

            return model;
        }

        public static UserFacilityTimeSchedulingModel ToModel(this UserFacilityTimeSchedulingViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new UserFacilityTimeSchedulingModel
            {
                ResourceAvailabilityID = entity.ResourceAvailabilityID,
                AvailabilityStartTime = entity.AvailabilityStartTime,
                AvailabilityEndTime = entity.AvailabilityEndTime,
                IsActive = entity.IsActive,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

    }
}