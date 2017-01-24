using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Axis.Constant;
using Axis.Data.Repository;
using Axis.Model.Common;
using Axis.Model.Setting;
using Axis.Security;

namespace Axis.DataProvider.Settings
{
    public class SettingsDataProvider : ISettingsDataProvider
    {
        #region Class Variables

        IUnitOfWork unitOfWork = null;

        #endregion

        #region Constructors

        public SettingsDataProvider(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        #endregion

        #region Public Methods

        public Response<SettingModel> GetSettingsByType(List<SettingType> settingTypes)
        {
            var settingRepository = unitOfWork.GetRepository<SettingModel>();

            SqlParameter settingTypesParam = new SqlParameter("SettingTypes", GenerateCsv(settingTypes));
            List<SqlParameter> procParams = new List<SqlParameter>() { settingTypesParam };

            var result = settingRepository.ExecuteStoredProc("usp_GetSettingsByType", procParams);

            return result;
        }

        public Response<SettingModel> UpdateSetting(SettingModel setting)
        {
            var settingRepository = unitOfWork.GetRepository<SettingModel>();

            SqlParameter settingValuesIdParam = new SqlParameter("SettingValuesID", setting.SettingValuesID);
            SqlParameter settingTypeIdParam = new SqlParameter("SettingTypeID", setting.SettingsTypeID);
            SqlParameter settingValueParam = new SqlParameter("SettingValue", setting.Value);
            SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", setting.ModifiedOn ?? DateTime.Now);
            List<SqlParameter> procParams = new List<SqlParameter>() { settingValuesIdParam, settingTypeIdParam, settingValueParam, modifiedOnParam };

            return settingRepository.ExecuteNQStoredProc("usp_UpdateSetting", procParams);
        }

        public Response<SettingModel> GetSettingsToCache()
        {
            var settingRepository = unitOfWork.GetRepository<SettingModel>();
            int userId = AuthContext.Auth.User.UserID;

            SqlParameter userIdParam = new SqlParameter("UserID", userId);
            List<SqlParameter> procParams = new List<SqlParameter>() { userIdParam };

            var result = settingRepository.ExecuteStoredProc("usp_GetSettingsToCache", procParams);

            return result;
        }

        #endregion

        #region Private Methods

        private string GenerateCsv(List<SettingType> settingTypes)
        {
            return string.Join(",", settingTypes.Cast<int>().Select(id => id.ToString()).ToArray());
        }

        #endregion
    }
}
