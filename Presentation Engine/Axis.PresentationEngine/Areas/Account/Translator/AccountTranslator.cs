using Axis.Model.Account;
using Axis.Model.Common;
using Axis.PresentationEngine.Areas.Account.Model;
using System.Collections.Generic;
using Axis.PresentationEngine.Areas.Admin.Translator;

namespace Axis.PresentationEngine.Areas.Account.Translator
{
    public static class AccountTranslator
    {
        public static UserModel ToModel(this UserViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new UserModel
            {
                UserID = entity.UserID,
                UserGUID = entity.UserGUID,
                ADFlag = entity.ADFlag,
                UserName = entity.UserName,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                MiddleName = entity.MiddleName,
                GenderID = entity.GenderID,
                Password = entity.Password,
                EffectiveFromDate = entity.EffectiveFromDate,
                EffectiveToDate = entity.EffectiveToDate,
                IsActive = entity.IsActive,
                Roles = entity.Roles,
                PrimaryEmail = entity.PrimaryEmail,
                EmailID = entity.EmailID,
                Credentials = entity.Credentials.ToModel(),
                LoginAttempts = entity.LoginAttempts,
                ModifiedBy = 1,
                ForceRollback = entity.ForceRollback,
                ModifiedOn = entity.ModifiedOn,
                IsInternal = entity.IsInternal
            };

            return model;
        }

        public static UserViewModel ToModel(this UserModel entity)
        {
            if (entity == null)
                return null;

            var model = new UserViewModel
            {
                //Update this
                UserID = entity.UserID,
                UserGUID = entity.UserGUID,
                ADFlag = entity.ADFlag,
                UserName = entity.UserName,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                MiddleName = entity.MiddleName,
                GenderID = entity.GenderID,
                Password = entity.Password,
                EffectiveFromDate = entity.EffectiveFromDate,
                EffectiveToDate = entity.EffectiveToDate,
                Roles = entity.Roles,
                Credentials = entity.Credentials.ToModel(),
                LoginAttempts = entity.LoginAttempts,
                LoginCount = entity.LoginCount,
                LastLogin = entity.LastLogin,
                IPAddress = entity.IPAddress,
                SessionID = entity.SessionID,
                PrimaryEmail = entity.PrimaryEmail,
                EmailID = entity.EmailID,
                ModifiedBy = entity.ModifiedBy,
                ModifiedOn = entity.ModifiedOn,
                IsActive = entity.IsActive,
                ThumbnailBLOB = entity.ThumbnailBLOB,
                IsInternal = entity.IsInternal
            };

            return model;
        }

        public static Response<UserViewModel> ToModel(this Response<UserModel> entity)
        {
            if (entity == null)
                return null;

            var dataItems = new List<UserViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(UserModel userModel)
                {
                    var transformedModel = userModel.ToModel();
                    dataItems.Add(transformedModel);
                });
            }
            else
                dataItems = null;

            var model = new Response<UserViewModel>
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