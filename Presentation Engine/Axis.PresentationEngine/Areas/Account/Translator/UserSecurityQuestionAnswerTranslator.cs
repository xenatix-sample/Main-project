using System.Linq;
using Axis.Model.Admin.UserScheduling;
using System.Collections.Generic;
using Axis.Model.Common;
using Axis.PresentationEngine.Areas.Account.Model;
using Axis.Model.Admin;
using Axis.Model.Account;

namespace Axis.PresentationEngine.Areas.Account.Translator
{
    /// <summary>
    /// User security questions translator
    /// </summary>
    public static class UserSecurityQuestionAnswerTranslator
    {
        public static UserSecurityQuestionAnswerViewModel ToViewModel(this UserSecurityQuestionAnswerModel entity)
        {
            if (entity == null)
                return null;

            var model = new UserSecurityQuestionAnswerViewModel
            {
                UserID = entity.UserID,
                UserSecurityQuestionID = entity.UserSecurityQuestionID,
                SecurityAnswer = entity.SecurityAnswer,
                SecurityQuestionID=entity.SecurityQuestionID,
                ADFlag = entity.ADFlag
            };
           

            return model;
        }

        public static UserSecurityQuestionAnswerModel ToViewModel(this UserSecurityQuestionAnswerViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new UserSecurityQuestionAnswerModel
            {
                UserID = entity.UserID,
                UserSecurityQuestionID = entity.UserSecurityQuestionID,
                SecurityAnswer = entity.SecurityAnswer,
                SecurityQuestionID = entity.SecurityQuestionID
            };


            return model;
        }

        public static Response<UserSecurityQuestionAnswerViewModel> ToModel(this Response<UserSecurityQuestionAnswerModel> entity)
        {
            if (entity == null)
                return null;

            var dataItems = new List<UserSecurityQuestionAnswerViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(UserSecurityQuestionAnswerModel userSecurityQuestionAnswer)
                {
                    var transformedModel = userSecurityQuestionAnswer.ToViewModel();
                    dataItems.Add(transformedModel);
                });
            }
            else
            {
                dataItems = null;
            }

            var model = new Response<UserSecurityQuestionAnswerViewModel>
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




        public static List<UserSecurityQuestionAnswerModel> ToModel(this List<UserSecurityQuestionAnswerViewModel> entity)
        {
            if (entity == null)
                return null;

            var models = new List<UserSecurityQuestionAnswerModel>();
            entity.ForEach(x =>
                models.Add(x.ToViewModel()));

            return models;
        }

    }
}