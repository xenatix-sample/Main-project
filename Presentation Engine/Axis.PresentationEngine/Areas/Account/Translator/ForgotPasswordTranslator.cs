using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Model.Common.Lookups.SecurityQuestion;
using Axis.PresentationEngine.Areas.Account.Model;
using Axis.PresentationEngine.Helpers.Model;
using Axis.PresentationEngine.Helpers.Translator;
using System.Collections.Generic;

namespace Axis.PresentationEngine.Areas.Account.Translator
{
    /// <summary>
    ///
    /// </summary>
    public static class ForgotPasswordTranslator
    {
        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static ResetPasswordViewModel ToModel(this ResetPasswordModel entity)
        {
            if (entity == null)
                return null;

            var model = new ResetPasswordViewModel
            {
                ResetPasswordID = entity.ResetPasswordID,
                UserID = entity.UserID,
                Email = entity.Email,
                ResetIdentifier = entity.ResetIdentifier,
                Phone = entity.Phone,
                PhoneID = entity.PhoneID,
                OTPCode = entity.OTPCode,
                SecurityQuestionID = entity.SecurityQuestionID,
                SecurityAnswer = entity.SecurityAnswer,
                NewPassword = entity.NewPassword,
                RequestorIPAddress = entity.RequestorIPAddress,
                ExpiresOn = entity.ExpiresOn,
                ModifiedBy = entity.ModifiedBy,
                ModifiedOn = entity.ModifiedOn,
                ADFlag = entity.ADFlag,
                ADUserPasswordResetMessage = entity.ADUserPasswordResetMessage,
                IsDigitalPassword = entity.IsDigitalPassword
            };

            return model;
        }

        /// <summary>
        /// To the entity.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static ResetPasswordModel ToEntity(this ResetPasswordViewModel model)
        {
            if (model == null)
                return null;

            var entity = new ResetPasswordModel
            {
                ResetPasswordID = model.ResetPasswordID,
                UserID = model.UserID,
                ResetIdentifier = model.ResetIdentifier,
                Email = model.Email,
                Phone = model.Phone,
                PhoneID = model.PhoneID,
                OTPCode = model.OTPCode,
                SecurityQuestionID = model.SecurityQuestionID,
                SecurityAnswer = model.SecurityAnswer,
                NewPassword = model.NewPassword,
                RequestorIPAddress = model.RequestorIPAddress,
                ExpiresOn = model.ExpiresOn,
                ModifiedBy = 1,
                ModifiedOn = model.ModifiedOn,
                IsDigitalPassword = model.IsDigitalPassword
            };

            return entity;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<ResetPasswordViewModel> ToModel(this Response<ResetPasswordModel> entity)
        {
            if (entity == null)
                return null;

            var dataItems = new List<ResetPasswordViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(ResetPasswordModel resetPasswordModel)
                {
                    var transformedModel = resetPasswordModel.ToModel();
                    dataItems.Add(transformedModel);
                });
            }
            else
                dataItems = null;

            var model = new Response<ResetPasswordViewModel>
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

        /// <summary>
        /// To the entity.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static Response<ResetPasswordModel> ToEntity(this Response<ResetPasswordViewModel> model)
        {
            if (model == null)
                return null;

            var dataItems = new List<ResetPasswordModel>();

            if (model.DataItems != null)
            {
                model.DataItems.ForEach(delegate(ResetPasswordViewModel resetPasswordViewModel)
                {
                    var transformedModel = resetPasswordViewModel.ToEntity();
                    dataItems.Add(transformedModel);
                });
            }
            else
                dataItems = null;

            var entity = new Response<ResetPasswordModel>
            {
                ResultCode = model.ResultCode,
                ResultMessage = model.ResultMessage,
                DataItems = dataItems,
                RowAffected = model.RowAffected,
                AdditionalResult = model.AdditionalResult,
                ID = model.ID
            };

            return entity;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<SecurityQuestionViewModel> ToModel(this Response<SecurityQuestionModel> entity)
        {
            if (entity == null)
                return null;

            var dataItems = new List<SecurityQuestionViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(SecurityQuestionModel resetPasswordModel)
                {
                    var transformedModel = resetPasswordModel.ToViewModel();
                    dataItems.Add(transformedModel);
                });
            }
            else
                dataItems = null;

            var model = new Response<SecurityQuestionViewModel>
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