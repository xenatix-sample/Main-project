using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Admin;
using Axis.Helpers;
using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Model.Admin;

namespace Axis.DataEngine.Service.Controllers
{
    public class UserDetailController : BaseApiController
    {
        #region Class Variables

        readonly IUserDetailDataProvider _userDetailDataProvider = null;

        #endregion

        #region Constructors

        public UserDetailController(IUserDetailDataProvider userDetailDataProvider)
        {
            _userDetailDataProvider = userDetailDataProvider;
        }

        #endregion

        #region Public Methods

        [HttpGet]
        public IHttpActionResult GetUser(int userID)
        {
            var userResponse = _userDetailDataProvider.GetUser(userID);
            return new HttpResult<Response<UserModel>>(userResponse, Request);
        }

        [HttpPost]
        public IHttpActionResult AddUser(UserModel userDetail)
        {
            var userResponse = _userDetailDataProvider.AddUser(userDetail);
            var validator = VerifySaveFields(userDetail);
            if (validator.HasError)
            {
                var validationResponse = new Response<UserModel>() { DataItems = new List<UserModel>(), ResultCode = -1, ResultMessage = validator.Message };
                return new HttpResult<Response<UserModel>>(validationResponse, Request);
            }

            if (userResponse.ResultCode != 0)
            {
                if (userResponse.ResultMessage.Contains("Duplicate Email"))
                {
                    userResponse.ResultMessage = "Email already exists";
                }
                else
                    userResponse.ResultMessage = userResponse.ResultMessage.Contains("UNIQUE KEY") ? "This username is not available" : "Error while adding the user";
            }
            else
            {
                //send the new user an email
                try
                {
                    SendNewUserEmail(userDetail);
                }
                catch (Exception exc)
                {
                    _logger.Error(exc);
                    userResponse.ResultCode = 0;
                    userResponse.ResultMessage = "User added successfully. Error while sending an email to the new user";
                    userResponse.AdditionalResult = exc.Message;
                }
            }

            return new HttpResult<Response<UserModel>>(userResponse, Request);
        }

        [HttpPost]
        public IHttpActionResult UpdateUser(UserModel userDetail)
        {
            var validator = VerifySaveFields(userDetail);
            if (validator.HasError)
            {
                var validationResponse = new Response<UserModel>() { DataItems = new List<UserModel>(), ResultCode = -1, ResultMessage = validator.Message };
                return new HttpResult<Response<UserModel>>(validationResponse, Request);
            }

            var userResponse = _userDetailDataProvider.UpdateUser(userDetail);

            if (userResponse.ResultCode != 0)
            {
                userResponse.ResultMessage = userResponse.ResultMessage.Contains("UNIQUE KEY") ? "This username is not available" : "Error while adding the user";
            }

            return new HttpResult<Response<UserModel>>(userResponse, Request);
        }

        [HttpPost]
        public IHttpActionResult SendNewUserEmail(UserModel userDetail)
        {
            var response = _userDetailDataProvider.SendNewUserEmail(userDetail);
            return new HttpResult<Response<UserModel>>(response, Request);
        }

        #endregion

        #region Additional user details

        [HttpGet]
        public IHttpActionResult GetCoSignatures(int userID)
        {
            var userResponse = _userDetailDataProvider.GetCoSignatures(userID);
            return new HttpResult<Response<CoSignaturesModel>>(userResponse, Request);
        }

        [HttpPost]
        public IHttpActionResult AddCoSignatures(CoSignaturesModel signature)
        {
            var userResponse = _userDetailDataProvider.AddCoSignatures(signature);
            return new HttpResult<Response<CoSignaturesModel>>(userResponse, Request);
        }

        [HttpPut]
        public IHttpActionResult UpdateCoSignatures(CoSignaturesModel signature)
        {
            var userResponse = _userDetailDataProvider.UpdateCoSignatures(signature);
            return new HttpResult<Response<CoSignaturesModel>>(userResponse, Request);
        }

        [HttpDelete]
        public IHttpActionResult DeleteCoSignatures(long id, DateTime modifiedOn)
        {
            return new HttpResult<Response<CoSignaturesModel>>(_userDetailDataProvider.DeleteCoSignatures(id, modifiedOn), Request);
        }

        [HttpGet]
        public IHttpActionResult GetUserIdentifierDetails(int userID)
        {
            var userResponse = _userDetailDataProvider.GetUserIdentifierDetails(userID);
            return new HttpResult<Response<UserIdentifierDetailsModel>>(userResponse, Request);
        }

        [HttpPost]
        public IHttpActionResult AddUserIdentifierDetails(UserIdentifierDetailsModel useridentifier)
        {
            var userResponse = _userDetailDataProvider.AddUserIdentifierDetails(useridentifier);
            return new HttpResult<Response<UserIdentifierDetailsModel>>(userResponse, Request);
        }

        [HttpPut]
        public IHttpActionResult UpdateUserIdentifierDetails(UserIdentifierDetailsModel useridentifier)
        {
            var userResponse = _userDetailDataProvider.UpdateUserIdentifierDetails(useridentifier);
            return new HttpResult<Response<UserIdentifierDetailsModel>>(userResponse, Request);
        }

        [HttpDelete]
        public IHttpActionResult DeleteUserIdentifierDetails(long id, DateTime modifiedOn)
        {
            return new HttpResult<Response<UserIdentifierDetailsModel>>(_userDetailDataProvider.DeleteUserIdentifierDetails(id, modifiedOn), Request);

        }

        [HttpGet]
        public IHttpActionResult GetUserAdditionalDetails(int userID)
        {
            var userResponse = _userDetailDataProvider.GetUserAdditionalDetails(userID);
            return new HttpResult<Response<UserAdditionalDetailsModel>>(userResponse, Request);
        }

        [HttpPost]
        public IHttpActionResult AddUserAdditionalDetails(UserAdditionalDetailsModel details)
        {
            var userResponse = _userDetailDataProvider.AddUserAdditionalDetails(details);
            return new HttpResult<Response<UserAdditionalDetailsModel>>(userResponse, Request);
        }

        [HttpPut]
        public IHttpActionResult UpdateUserAdditionalDetails(UserAdditionalDetailsModel details)
        {
            var userResponse = _userDetailDataProvider.UpdateUserAdditionalDetails(details);
            return new HttpResult<Response<UserAdditionalDetailsModel>>(userResponse, Request);
        }

        [HttpDelete]
        public IHttpActionResult DeleteUserAdditionalDetails(long id, DateTime modifiedOn)
        {
            return new HttpResult<Response<UserAdditionalDetailsModel>>(_userDetailDataProvider.DeleteUserAdditionalDetails(id, modifiedOn), Request);

        }
        #endregion

        #region Private Methods

        private ValidationChecker VerifySaveFields(UserModel user)
        {
            StringBuilder sb = new StringBuilder();
            bool hasError = false;

            if (!CommonHelper.IsADUserNameValid(user.UserName))
            {
                sb.Append("UserName is not valid. ");
                hasError = true;
            }
            if (!CommonHelper.IsValidEmail(user.PrimaryEmail))
            {
                sb.Append("Invalid email format.");
                hasError = true;
            }

            ValidationChecker checker = new ValidationChecker
            {
                HasError = hasError,
                Message = sb.ToString()
            };

            return checker;
        }

        #endregion

        #region Structs

        private struct ValidationChecker
        {
            public string Message { get; set; }
            public bool HasError { get; set; }
        };

        #endregion
    }
}