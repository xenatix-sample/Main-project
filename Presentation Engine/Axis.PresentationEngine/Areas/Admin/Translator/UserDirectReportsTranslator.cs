using Axis.Model.Admin;
using Axis.Model.Common;
using Axis.PresentationEngine.Areas.Admin.Models;
using System.Collections.Generic;

namespace Axis.PresentationEngine.Areas.Admin.Translator
{
    public static class UserDirectReportsTranslator
    {

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static UserDirectReportsViewModel ToViewModel(this UserDirectReportsModel entity)
        {
            if (entity == null)
                return null;

            var model = new UserDirectReportsViewModel
            {
                IsSupervisor = entity.IsSupervisor,
                MappingID = entity.MappingID,
                Email = entity.Email,
                ParentID = entity.ParentID,
                ModifiedOn = entity.ModifiedOn,
                ForceRollback = entity.ForceRollback,
                UserID = entity.UserID,
                UserGUID = entity.UserGUID,
                ADFlag = entity.ADFlag,
                UserName = entity.UserName,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                MiddleName = entity.MiddleName,
                GenderID = entity.GenderID,
                Password = entity.Password,
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
                Gender = entity.Gender,
                ModifiedBy = entity.ModifiedBy,
                IsActive = entity.IsActive,
                HasSupervisor = entity.HasSupervisor
            };

            return model;
        }

        /// <summary>
        /// To the view model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Response<UserDirectReportsViewModel> ToViewModel(this Response<UserDirectReportsModel> entity)
        {
            if (entity == null)
                return null;

            var model = entity.CloneResponse<UserDirectReportsViewModel>();
            var userDirectReport = new List<UserDirectReportsViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(UserDirectReportsModel directReport)
                {
                    var transformedModel = directReport.ToViewModel();
                    userDirectReport.Add(transformedModel);
                });

                model.DataItems = userDirectReport;
            }

            return model;
        }

        /// <summary>
        /// To the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static UserDirectReportsModel ToModel(this UserDirectReportsViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new UserDirectReportsModel
            {
                IsSupervisor = entity.IsSupervisor,
                MappingID = entity.MappingID,
                ModifiedOn = entity.ModifiedOn,
                ForceRollback = entity.ForceRollback,
                ParentID = entity.ParentID,
                UserID = entity.UserID,
                UserGUID = entity.UserGUID,
                ADFlag = entity.ADFlag,
                UserName = entity.UserName,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                MiddleName = entity.MiddleName,
                GenderID = entity.GenderID,
                Gender = entity.Gender,
                Password = entity.Password,
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
                IsActive = entity.IsActive,
                HasSupervisor = entity.HasSupervisor
            };

            return model;
        }

    }
}