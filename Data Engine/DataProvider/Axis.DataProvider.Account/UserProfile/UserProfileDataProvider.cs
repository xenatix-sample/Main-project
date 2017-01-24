using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Axis.Data.Repository;
using Axis.DataProvider.Common;
using Axis.DataProvider.Common.SecurityQuestion;
using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Logging;

namespace Axis.DataProvider.Account.UserProfile
{
    public class UserProfileDataProvider : IUserProfileDataProvider
    {
        #region Class Variables

        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserSecurityQuestionDataProvider _userSecurityQuestionDataProvider;
        private readonly IPhoneDataProvider _phoneDataProvider;
        private readonly IEmailDataProvider _emailDataProvider;
        private readonly IAddressDataProvider _addressDataProvider;
        private readonly ILogger _logger;

        #endregion Class Variables

        #region Constructors

        public UserProfileDataProvider(IUnitOfWork unitOfWork, IUserSecurityQuestionDataProvider userSecurityQuestionDataProvider, IPhoneDataProvider phoneDataProvider,
                                       IEmailDataProvider emailDataProvider, IAddressDataProvider addressDataProvider, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _userSecurityQuestionDataProvider = userSecurityQuestionDataProvider;
            _phoneDataProvider = phoneDataProvider;
            _emailDataProvider = emailDataProvider;
            _addressDataProvider = addressDataProvider;
            _logger = logger;
        }

        #endregion Constructors

        #region Public Methods

        public Response<UserProfileModel> GetUserProfile(int userID)
        {
            var userProfileRepository = _unitOfWork.GetRepository<UserProfileModel>();

            SqlParameter userIdParam = new SqlParameter("UserID", userID);
            List<SqlParameter> procParams = new List<SqlParameter>() { userIdParam };
            var userProfileResult = userProfileRepository.ExecuteStoredProc("usp_GetUserProfile", procParams);

            if (userProfileResult.ResultCode != 0)
            {
                return userProfileResult;                
            }

            //retrieve the user's security questions
            var userSecurityQuestionResult = _userSecurityQuestionDataProvider.GetUserSecurityQuestions(userID);
            if (userSecurityQuestionResult.ResultCode == 0)
            {
                userProfileResult.DataItems.ForEach(u =>
                {
                    u.SecurityQuestions = userSecurityQuestionResult.DataItems.ToList();
                });
            }

            //retrieve the user's phone data
            var userPhoneResult = _phoneDataProvider.GetUserPhones(userID);
            if (userPhoneResult.ResultCode == 0)
            {
                userProfileResult.DataItems.ForEach(u =>
                {
                    u.Phones = userPhoneResult.DataItems.ToList();
                });
            }

            //retrieve the user's email addresses
            var userEmailResult = _emailDataProvider.GetUserEmails(userID);
            if (userEmailResult.ResultCode == 0)
            {
                userProfileResult.DataItems.ForEach(u =>
                {
                    u.Emails = userEmailResult.DataItems.ToList();
                });
            }

            //retrieve the user's addresses
            var userAddressResult = _addressDataProvider.GetUserAddresses(userID);
            if (userAddressResult.ResultCode == 0)
            {
                userProfileResult.DataItems.ForEach(u =>
                {
                    u.Addresses = userAddressResult.DataItems.ToList();
                });
            }

            return userProfileResult;
        }

        public Response<UserProfileModel> SaveUserProfile(UserProfileModel userProfile)
        {
            var response = new Response<UserProfileModel>(){ ResultCode = -1, ResultMessage = "Error while saving the user's profile"};
            using (var transactionScope = _unitOfWork.BeginTransactionScope())
            {
                try
                {
                    var userProfileRepository = _unitOfWork.GetRepository<UserProfileModel>();

                    SqlParameter userIdParam = new SqlParameter("UserID", userProfile.UserID);
                    SqlParameter updatePasswordParam = new SqlParameter("UpdatePassword", false);
                    SqlParameter newPasswordParam = new SqlParameter("NewPassword", DBNull.Value);
                    SqlParameter currentPasswordParam = new SqlParameter("CurrentPassword", DBNull.Value);
                    SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", userProfile.ModifiedOn ?? DateTime.Now);
                    List<SqlParameter> procParams = new List<SqlParameter>() { userIdParam, updatePasswordParam, newPasswordParam, currentPasswordParam, modifiedOnParam };
                    var userProfileResult = _unitOfWork.EnsureInTransaction(userProfileRepository.ExecuteNQStoredProc, "usp_SaveUserProfile", procParams,
                                    forceRollback: userProfile.ForceRollback.GetValueOrDefault(false));

                    if (userProfileResult.ResultCode != 0)
                        goto end;

                    //Email
                    if (userProfile.Emails.Count > 0)
                    {
                        var emailResult = _emailDataProvider.UpdateUserEmails(userProfile.UserID, userProfile.Emails);
                        if (emailResult.ResultCode != 0)
                            goto end;
                    }

                    //Phone
                    if (userProfile.Phones.Count > 0)
                    {
                        var phoneResult = _phoneDataProvider.SaveUserPhones(userProfile.UserID, userProfile.Phones);
                        if (phoneResult.ResultCode != 0)
                            goto end;
                    }

                    //Address
                    if (userProfile.Addresses.Count > 0)
                    {
                        var addressResult = _addressDataProvider.SaveUserAddresses(userProfile.UserID, userProfile.Addresses);
                        if (addressResult.ResultCode != 0)
                            goto end;
                    }

                    response = userProfileResult;
                    if(!userProfile.ForceRollback.GetValueOrDefault(false))
                        _unitOfWork.TransactionScopeComplete(transactionScope);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    response.ResultCode = -1;
                    response.ResultMessage = "Error while saving the user's profile";
                }
            }

            end:
            return response;
        }

        #endregion
    }
}
