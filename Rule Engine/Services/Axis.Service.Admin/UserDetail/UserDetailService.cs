using System;
using System.Collections.Specialized;
using Axis.Configuration;
using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Security;
using Axis.Model.Admin;
using System.Globalization;

namespace Axis.Service.Admin
{
    public class UserDetailService : IUserDetailService
    {
        #region Class Variables

        private readonly CommunicationManager _communicationManager;
        private const string BaseRoute = "userDetail/";

        #endregion

        #region Constructors

        public UserDetailService()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        #endregion

        #region Public Methods

        public Response<UserModel> GetUser(int userID)
        {
            string apiUrl = BaseRoute + "GetUser";
            var param = new NameValueCollection { { "userID", userID.ToString() } };

            return _communicationManager.Get<Response<UserModel>>(param, apiUrl);
        }

        public Response<UserModel> AddUser(UserModel userDetail)
        {
            string apiUrl = BaseRoute + "AddUser";
            return _communicationManager.Post<UserModel, Response<UserModel>>(userDetail, apiUrl);
        }

        public Response<UserModel> UpdateUser(UserModel userDetail)
        {
            string apiUrl = BaseRoute + "UpdateUser";
            return _communicationManager.Post<UserModel, Response<UserModel>>(userDetail, apiUrl);
        }


        public Response<CoSignaturesModel> GetCoSignatures(int userID)
        {
            string apiUrl = BaseRoute + "GetCoSignatures";
            var param = new NameValueCollection { { "userID", userID.ToString() } };

            return _communicationManager.Get<Response<CoSignaturesModel>>(param, apiUrl);
        }

        public Response<CoSignaturesModel> AddCoSignatures(CoSignaturesModel signature)
        {
            string apiUrl = BaseRoute + "AddCoSignatures";
            return _communicationManager.Post<CoSignaturesModel, Response<CoSignaturesModel>>(signature, apiUrl);
        }

        public Response<CoSignaturesModel> UpdateCoSignatures(CoSignaturesModel signature)
        {
            string apiUrl = BaseRoute + "UpdateCoSignatures";
            return _communicationManager.Put<CoSignaturesModel, Response<CoSignaturesModel>>(signature, apiUrl);
        }

        public Response<CoSignaturesModel> DeleteCoSignatures(long id, DateTime modifiedOn)
        {
            const string apiUrl = BaseRoute + "DeleteCoSignatures";
            var requestId = new NameValueCollection
            {
                { "id", id.ToString(CultureInfo.InvariantCulture) },
                { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) }
            };
            return _communicationManager.Delete<Response<CoSignaturesModel>>(requestId, apiUrl);
        }

        public Response<UserIdentifierDetailsModel> GetUserIdentifierDetails(int userID)
        {
            string apiUrl = BaseRoute + "GetUserIdentifierDetails";
            var param = new NameValueCollection { { "userID", userID.ToString() } };

            return _communicationManager.Get<Response<UserIdentifierDetailsModel>>(param, apiUrl);
        }

        public Response<UserIdentifierDetailsModel> AddUserIdentifierDetails(UserIdentifierDetailsModel useridentifier)
        {
            string apiUrl = BaseRoute + "AddUserIdentifierDetails";
            return _communicationManager.Post<UserIdentifierDetailsModel, Response<UserIdentifierDetailsModel>>(useridentifier, apiUrl);
        }

        public Response<UserIdentifierDetailsModel> UpdateUserIdentifierDetails(UserIdentifierDetailsModel useridentifier)
        {
            string apiUrl = BaseRoute + "UpdateUserIdentifierDetails";
            return _communicationManager.Put<UserIdentifierDetailsModel, Response<UserIdentifierDetailsModel>>(useridentifier, apiUrl);
        }

        public Response<UserIdentifierDetailsModel> DeleteUserIdentifierDetails(long id, DateTime modifiedOn)
        {
            const string apiUrl = BaseRoute + "DeleteUserIdentifierDetails";
            var requestId = new NameValueCollection
            {
                { "id", id.ToString(CultureInfo.InvariantCulture) },
                { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) }
            };
            return _communicationManager.Delete<Response<UserIdentifierDetailsModel>>(requestId, apiUrl);
        }

        public Response<UserAdditionalDetailsModel> GetUserAdditionalDetails(int userID)
        {
            string apiUrl = BaseRoute + "GetUserAdditionalDetails";
            var param = new NameValueCollection { { "userID", userID.ToString() } };

            return _communicationManager.Get<Response<UserAdditionalDetailsModel>>(param, apiUrl);
        }

        public Response<UserAdditionalDetailsModel> AddUserAdditionalDetails(UserAdditionalDetailsModel details)
        {
            string apiUrl = BaseRoute + "AddUserAdditionalDetails";
            return _communicationManager.Post<UserAdditionalDetailsModel, Response<UserAdditionalDetailsModel>>(details, apiUrl);
        }

        public Response<UserAdditionalDetailsModel> UpdateUserAdditionalDetails(UserAdditionalDetailsModel details)
        {
            string apiUrl = BaseRoute + "UpdateUserAdditionalDetails";
            return _communicationManager.Put<UserAdditionalDetailsModel, Response<UserAdditionalDetailsModel>>(details, apiUrl);
        }

        public Response<UserAdditionalDetailsModel> DeleteUserAdditionalDetails(long id, DateTime modifiedOn)
        {
            const string apiUrl = BaseRoute + "DeleteUserAdditionalDetails";
            var requestId = new NameValueCollection
            {
                { "id", id.ToString(CultureInfo.InvariantCulture) },
                { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) }
            };
            return _communicationManager.Delete<Response<UserAdditionalDetailsModel>>(requestId, apiUrl);
        }

        #endregion
    }
}
