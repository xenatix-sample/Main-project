using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Model.Common.User;
using Axis.PresentationEngine.Areas.Account.Model;
using System.Collections.Generic;

namespace Axis.PresentationEngine.Areas.Account.Translator
{
    public static class UserProfileTranslator
    {
        public static UserProfileModel ToModel(this UserProfileViewModel entity)
        {
            if (entity == null)
                return null;

             List<UserSecurityQuestionModel> securityQuestions = new List<UserSecurityQuestionModel>();
             if (entity.SaveSecurityQuestions)
             {
                 securityQuestions.Add(new UserSecurityQuestionModel(){ UserSecurityQuestionID = entity.UserSecurityQuestionID1, SecurityQuestionID = entity.SecurityQuestionID1, SecurityAnswer = entity.SecurityAnswer1 });
                 securityQuestions.Add(new UserSecurityQuestionModel(){ UserSecurityQuestionID = entity.UserSecurityQuestionID2, SecurityQuestionID = entity.SecurityQuestionID2, SecurityAnswer = entity.SecurityAnswer2 });
             }

            var model = new UserProfileModel
            {
                UserID = entity.UserID,
                UserName = entity.UserName,
                IsTemporaryPassword = entity.IsTemporaryPassword,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                MiddleName = entity.MiddleName,
                UserGUID = entity.UserGUID,
                ADFlag = entity.ADFlag,
                CurrentPassword = entity.CurrentPassword,
                NewPassword = entity.NewPassword,
                ConfirmPassword = entity.ConfirmPassword,
                CurrentDigitalPassword = entity.CurrentDigitalPassword,
                NewDigitalPassword = entity.NewDigitalPassword,
                ConfirmDigitalPassword = entity.ConfirmDigitalPassword,
                PrintSignature = entity.PrintSignature,
                Addresses = entity.Addresses,
                Emails = entity.Emails,
                Phones = entity.Phones,
                SecurityQuestions = securityQuestions,
                ModifiedOn = entity.ModifiedOn,
                IsActive = entity.IsActive
            };

            return model;
        }

        public static UserProfileViewModel ToModel(this UserProfileModel entity)
        {
            if (entity == null)
                return null;

            int securityQuestionID1 = 0;
            int securityQuestionID2 = 0;
            long userSecurityQuestionID1 = 0;
            long userSecurityQuestionID2 = 0;

            if(entity.SecurityQuestions.Count > 0)
            {
                securityQuestionID1 = entity.SecurityQuestions[0].SecurityQuestionID;
                securityQuestionID2 = entity.SecurityQuestions[1].SecurityQuestionID;
                userSecurityQuestionID1 = entity.SecurityQuestions[0].UserSecurityQuestionID;
                userSecurityQuestionID2 = entity.SecurityQuestions[1].UserSecurityQuestionID;
            }

            var model = new UserProfileViewModel
            {                
                UserID = entity.UserID,
                UserName = entity.UserName,
                IsTemporaryPassword = entity.IsTemporaryPassword,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                MiddleName = entity.MiddleName,
                UserGUID = entity.UserGUID,
                ADFlag = entity.ADFlag,
                Addresses = entity.Addresses,
                Emails = entity.Emails,
                Phones = entity.Phones,
                SecurityQuestionID1 = securityQuestionID1,
                SecurityQuestionID2 = securityQuestionID2,
                SecurityAnswer1 = securityQuestionID1 == 0 ? "" : "*****",
                SecurityAnswer2 = securityQuestionID2 == 0 ? "" : "*****",
                UserSecurityQuestionID1 = userSecurityQuestionID1,
                UserSecurityQuestionID2 = userSecurityQuestionID2,
                CurrentPassword = string.Empty,
                NewPassword = string.Empty,
                ConfirmPassword = string.Empty,
                CurrentDigitalPassword = !string.IsNullOrEmpty(entity.CurrentDigitalPassword) ? "Password Set" : string.Empty,
                NewDigitalPassword = string.Empty,
                ConfirmDigitalPassword = string.Empty,
                PrintSignature = entity.PrintSignature,
                SaveSecurityQuestions = true,
                ModifiedOn = entity.ModifiedOn,
                IsActive = entity.IsActive,
                ADUserPasswordResetMessage = entity.ADUserPasswordResetMessage,
                ThumbnailBLOB = entity.ThumbnailBLOB
            };

            return model;
        }

        public static Response<UserProfileViewModel> ToModel(this Response<UserProfileModel> entity)
        {
            if (entity == null)
                return null;

            var dataItems = new List<UserProfileViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(UserProfileModel userProfileModel)
                {
                    var transformedModel = userProfileModel.ToModel();
                    dataItems.Add(transformedModel);
                });
            }
            else
                dataItems = null;

            var model = new Response<UserProfileViewModel>
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