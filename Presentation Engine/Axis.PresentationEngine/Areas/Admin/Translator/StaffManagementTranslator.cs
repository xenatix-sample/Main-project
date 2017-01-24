using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Axis.Model.Admin;
using Axis.Model.Common;
using Axis.PresentationEngine.Areas.Admin.Models;

namespace Axis.PresentationEngine.Areas.Admin.Translator
{
    public static class StaffManagementTranslator
    {
        public static StaffManagementViewModel ToViewModel(this StaffManagementModel entity)
        {
            if (entity == null)
                return null;

            var model = new StaffManagementViewModel
            {
                ContactID = entity.ContactID,
                UserID = entity.UserID,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                ModifiedOn = entity.ModifiedOn,
                ModifiedBy = entity.ModifiedBy
            };

            return model;
        }

        public static Response<StaffManagementViewModel> ToModel(this Response<StaffManagementModel> entity)
        {
            if (entity == null)
                return null;

            var dataItems = new List<StaffManagementViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(StaffManagementModel staffManagementModel)
                {
                    var transformedModel = staffManagementModel.ToViewModel();
                    dataItems.Add(transformedModel);
                });
            }

            var model = new Response<StaffManagementViewModel>
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