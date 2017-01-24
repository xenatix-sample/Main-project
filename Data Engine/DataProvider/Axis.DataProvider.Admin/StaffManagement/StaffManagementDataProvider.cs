using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Axis.Constant;
using Axis.Data.Repository;
using Axis.Helpers;
using Axis.Model.Account;
using Axis.Model.Admin;
using Axis.Model.Common;

namespace Axis.DataProvider.Admin
{
    public class StaffManagementDataProvider : IStaffManagementDataProvider
    {
        #region Class Variables

        private readonly IUnitOfWork _unitOfWork;

        #endregion Class Variables

        #region Constructors

        public StaffManagementDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Public Methods

        public Response<UserModel> GetStaff(string searchText)
        {
            var staffManagementRepository = _unitOfWork.GetRepository<UserModel>();
            SqlParameter searchParam = new SqlParameter("UserSch", searchText ?? String.Empty);
            List<SqlParameter> procParams = new List<SqlParameter>() { searchParam };
            var result = staffManagementRepository.ExecuteStoredProc("usp_GetUsers", procParams);

            return result;
        }

        public Response<UserModel> DeleteUser(int userID)
        {
            var staffManagementRepository = _unitOfWork.GetRepository<UserModel>();
            SqlParameter userIDParam = new SqlParameter("UserID", userID);
            SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", DateTime.Now);
            List<SqlParameter> procParams = new List<SqlParameter>() { userIDParam, modifiedOnParam };
            var result = staffManagementRepository.ExecuteNQStoredProc("usp_DeleteUser", procParams);

            return result;
        }

        public Response<UserModel> ActivateUser(int userID)
        {
            var staffManagementRepository = _unitOfWork.GetRepository<UserModel>();
            SqlParameter userIDParam = new SqlParameter("UserID", userID);
            SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", DateTime.Now);
            List<SqlParameter> procParams = new List<SqlParameter>() { userIDParam, modifiedOnParam };
            var result = staffManagementRepository.ExecuteNQStoredProc("usp_UpdateUserStatus", procParams);

            return result;
        }

        public Response<UserModel> UnlockUser(int userID)
        {
            var staffManagementRepository = _unitOfWork.GetRepository<UserModel>();
            SqlParameter userIDParam = new SqlParameter("UserID", userID);
            SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", DateTime.Now);
            List<SqlParameter> procParams = new List<SqlParameter>() { userIDParam, modifiedOnParam };
            var result = staffManagementRepository.ExecuteNQStoredProc("usp_UpdateUserUnlock", procParams);

            return result;
        }

        #endregion
    }
}
