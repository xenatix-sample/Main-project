using System.Web.Http;
using Axis.Model.Common;
using Axis.Model.Security;
using Axis.PresentationEngine.Areas.Account.Model;
using Axis.PresentationEngine.Areas.Admin.Respository;
using Axis.PresentationEngine.Helpers.Controllers;
using System;
using Axis.PresentationEngine.Areas.Admin.Model;

namespace Axis.PresentationEngine.Areas.Admin.ApiControllers
{
    public class UserAdditionalDetailsController : BaseApiController
    {
        #region Class Variables

        private readonly IUserDetailRepository _userDetailRepository;

        #endregion

        #region Constructors

        public UserAdditionalDetailsController(IUserDetailRepository userDetailRepository)
        {
            _userDetailRepository = userDetailRepository;
        }

        #endregion

        #region Public Methods

        [HttpGet]
        public Response<CoSignaturesViewModel> GetCoSignatures(int userID)
        {
            return _userDetailRepository.GetCoSignatures(userID);
        }

        [HttpPost]
        public Response<CoSignaturesViewModel> AddCoSignatures(CoSignaturesViewModel signature)
        {
            return _userDetailRepository.AddCoSignatures(signature);
        }

        [HttpPut]
        public Response<CoSignaturesViewModel> UpdateCoSignatures(CoSignaturesViewModel signature)
        {
            return _userDetailRepository.UpdateCoSignatures(signature);
        }

        [HttpDelete]
        public Response<CoSignaturesViewModel> DeleteCoSignatures(long id, DateTime modifiedOn)
        {
            modifiedOn = modifiedOn.ToUniversalTime();
            return _userDetailRepository.DeleteCoSignatures(id, modifiedOn);
        }

        [HttpGet]
        public Response<UserIdentifierViewModel> GetUserIdentifierDetails(int userID)
        {
            return _userDetailRepository.GetUserIdentifierDetails(userID);
        }

        [HttpPost]
        public Response<UserIdentifierViewModel> AddUserIdentifierDetails(UserIdentifierViewModel useridentifier)
        {
            return _userDetailRepository.AddUserIdentifierDetails(useridentifier);
        }

        [HttpPut]
        public Response<UserIdentifierViewModel> UpdateUserIdentifierDetails(UserIdentifierViewModel useridentifier)
        {
            return _userDetailRepository.UpdateUserIdentifierDetails(useridentifier);
        }

        [HttpDelete]
        public Response<UserIdentifierViewModel> DeleteUserIdentifierDetails(long id, DateTime modifiedOn)
        {
            modifiedOn = modifiedOn.ToUniversalTime();
            return _userDetailRepository.DeleteUserIdentifierDetails(id, modifiedOn);
        }

        [HttpGet]
        public Response<UserAdditionalDetailsViewModel> GetUserAdditionalDetails(int userID)
        {
            return _userDetailRepository.GetUserAdditionalDetails(userID);
        }

        [HttpPost]
        public Response<UserAdditionalDetailsViewModel> AddUserAdditionalDetails(UserAdditionalDetailsViewModel details)
        {
            return _userDetailRepository.AddUserAdditionalDetails(details);
        }

        [HttpPut]
        public Response<UserAdditionalDetailsViewModel> UpdateUserAdditionalDetails(UserAdditionalDetailsViewModel details)
        {
            return _userDetailRepository.UpdateUserAdditionalDetails(details);
        }

        [HttpDelete]
        public Response<UserAdditionalDetailsViewModel> DeleteUserAdditionalDetails(long id, DateTime modifiedOn)
        {
            modifiedOn = modifiedOn.ToUniversalTime();
            return _userDetailRepository.DeleteUserAdditionalDetails(id, modifiedOn);
        }

        #endregion
    }
}