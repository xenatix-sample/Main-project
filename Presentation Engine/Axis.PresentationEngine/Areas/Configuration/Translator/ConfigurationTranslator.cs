using Axis.Model.Common;
using Axis.Model.Setting;
using Axis.PresentationEngine.Areas.Configuration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Axis.PresentationEngine.Areas.Configuration.Translator
{
    public static class ConfigurationTranslator
    {
        public static SettingModel ToModel(this SettingsViewModel entity)
        {
            if (entity == null)
                return null;

            var model = new SettingModel
            {
                SettingsID = entity.SettingID,
                Settings = entity.SettingName,
                Value = entity.SettingValue,
                SettingsTypeID = entity.SettingTypeID,
                SettingsType = entity.SettingType,
                EntityID = entity.EntityID,
                SettingValuesID = entity.SettingValuesID,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        public static SettingsViewModel ToModel(this SettingModel entity)
        {
            if (entity == null)
                return null;

            var model = new SettingsViewModel
            {
                SettingID = entity.SettingsID,
                SettingName = entity.Settings,
                SettingValue = entity.Value,
                SettingTypeID = entity.SettingsTypeID,
                SettingType = entity.SettingsType,
                EntityID = entity.EntityID,
                SettingValuesID = entity.SettingValuesID,
                ModifiedOn = entity.ModifiedOn
            };

            return model;
        }

        public static Response<SettingsViewModel> ToModel(this Response<SettingModel> entity)
        {
            if (entity == null)
                return null;

            var dataItems = new List<SettingsViewModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(SettingModel settingModel)
                {
                    var transformedModel = settingModel.ToModel();
                    dataItems.Add(transformedModel);
                });
            }
            else
                dataItems = null;

            var model = new Response<SettingsViewModel>
            {
                ResultCode = entity.ResultCode,
                ResultMessage = entity.ResultMessage,
                DataItems = dataItems,
                RowAffected = entity.RowAffected,
                AdditionalResult = entity.AdditionalResult
            };

            return model;
        }

        public static Response<SettingModel> ToModel(this Response<SettingsViewModel> entity)
        {
            if (entity == null)
                return null;

            var dataItems = new List<SettingModel>();

            if (entity.DataItems != null)
            {
                entity.DataItems.ForEach(delegate(SettingsViewModel settingModel)
                {
                    var transformedModel = settingModel.ToModel();
                    dataItems.Add(transformedModel);
                });
            }
            else
                dataItems = null;

            var model = new Response<SettingModel>
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