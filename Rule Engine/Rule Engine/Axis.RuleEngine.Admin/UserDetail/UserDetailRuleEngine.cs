using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Service.Admin;
using Axis.Model.Admin;
using System;


namespace Axis.RuleEngine.Admin
{
    public class UserDetailRuleEngine : IUserDetailRuleEngine
    {
        #region Class Variables

        private readonly IUserDetailService _userDetailService;

        #endregion

        #region Constructors

        public UserDetailRuleEngine(IUserDetailService userDetailService)
        {
            _userDetailService = userDetailService;
        }

        #endregion

        #region Public Methods

        public Response<UserModel> GetUser(int userID)
        {
            return _userDetailService.GetUser(userID);
        }

        public Response<UserModel> AddUser(UserModel userDetail)
        {
            return _userDetailService.AddUser(userDetail);
        }

        public Response<UserModel> UpdateUser(UserModel userDetail)
        {
            return _userDetailService.UpdateUser(userDetail);
        }

        public Response<CoSignaturesModel> GetCoSignatures(int userID)
        {
            return _userDetailService.GetCoSignatures(userID);
        }

        public Response<CoSignaturesModel> AddCoSignatures(CoSignaturesModel signature)
        {
            return _userDetailService.AddCoSignatures(signature);
        }

        public Response<CoSignaturesModel> UpdateCoSignatures(CoSignaturesModel signature)
        {
            return _userDetailService.UpdateCoSignatures(signature);
        }

        public Response<CoSignaturesModel> DeleteCoSignatures(long id, DateTime modifiedOn)
        {
            return _userDetailService.DeleteCoSignatures(id, modifiedOn);
        }

        public Response<UserIdentifierDetailsModel> GetUserIdentifierDetails(int userID)
        {
            return _userDetailService.GetUserIdentifierDetails(userID);
        }

        public Response<UserIdentifierDetailsModel> AddUserIdentifierDetails(UserIdentifierDetailsModel useridentifier)
        {
            return _userDetailService.AddUserIdentifierDetails(useridentifier);
        }

        public Response<UserIdentifierDetailsModel> UpdateUserIdentifierDetails(UserIdentifierDetailsModel useridentifier)
        {
            return _userDetailService.UpdateUserIdentifierDetails(useridentifier);
        }

        public Response<UserIdentifierDetailsModel> DeleteUserIdentifierDetails(long id, System.DateTime modifiedOn)
        {
            return _userDetailService.DeleteUserIdentifierDetails(id, modifiedOn);
        }

        public Response<UserAdditionalDetailsModel> GetUserAdditionalDetails(int userID)
        {
            return _userDetailService.GetUserAdditionalDetails(userID);
        }

        public Response<UserAdditionalDetailsModel> AddUserAdditionalDetails(UserAdditionalDetailsModel details)
        {
            return _userDetailService.AddUserAdditionalDetails(details);
        }

        public Response<UserAdditionalDetailsModel> UpdateUserAdditionalDetails(UserAdditionalDetailsModel details)
        {
            return _userDetailService.UpdateUserAdditionalDetails(details);
        }

        public Response<UserAdditionalDetailsModel> DeleteUserAdditionalDetails(long id, System.DateTime modifiedOn)
        {
            return _userDetailService.DeleteUserAdditionalDetails(id, modifiedOn);
        }


        #endregion
    }
}
