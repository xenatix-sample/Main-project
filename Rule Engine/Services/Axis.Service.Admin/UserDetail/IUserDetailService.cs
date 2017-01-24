using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Model.Admin;

namespace Axis.Service.Admin
{
    public interface IUserDetailService
    {
        Response<UserModel> GetUser(int userID);
        Response<UserModel> AddUser(UserModel userDetail);
        Response<UserModel> UpdateUser(UserModel userDetail);
        Response<CoSignaturesModel> GetCoSignatures(int userID);
        Response<CoSignaturesModel> AddCoSignatures(CoSignaturesModel signature);
        Response<CoSignaturesModel> UpdateCoSignatures(CoSignaturesModel signature);
        Response<CoSignaturesModel> DeleteCoSignatures(long id, DateTime modifiedOn);
        Response<UserIdentifierDetailsModel> GetUserIdentifierDetails(int userID);
        Response<UserIdentifierDetailsModel> AddUserIdentifierDetails(UserIdentifierDetailsModel useridentifier);
        Response<UserIdentifierDetailsModel> UpdateUserIdentifierDetails(UserIdentifierDetailsModel useridentifier);
        Response<UserIdentifierDetailsModel> DeleteUserIdentifierDetails(long id, DateTime modifiedOn);
        Response<UserAdditionalDetailsModel> GetUserAdditionalDetails(int userID);
        Response<UserAdditionalDetailsModel> AddUserAdditionalDetails(UserAdditionalDetailsModel details);
        Response<UserAdditionalDetailsModel> UpdateUserAdditionalDetails(UserAdditionalDetailsModel details);
        Response<UserAdditionalDetailsModel> DeleteUserAdditionalDetails(long id, DateTime modifiedOn);
    }
}
