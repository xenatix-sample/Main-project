using Axis.Model.Account;
using Axis.Model.Admin;
using Axis.Model.Common;

namespace Axis.RuleEngine.Admin
{
    public interface IUserDetailRuleEngine
    {
        Response<UserModel> GetUser(int userID);
        Response<UserModel> AddUser(UserModel userDetail);
        Response<UserModel> UpdateUser(UserModel userDetail);
        Response<CoSignaturesModel> GetCoSignatures(int userID);
        Response<CoSignaturesModel> AddCoSignatures(CoSignaturesModel signature);
        Response<CoSignaturesModel> UpdateCoSignatures(CoSignaturesModel signature);
        Response<CoSignaturesModel> DeleteCoSignatures(long id, System.DateTime modifiedOn);
        Response<UserIdentifierDetailsModel> GetUserIdentifierDetails(int userID);
        Response<UserIdentifierDetailsModel> AddUserIdentifierDetails(UserIdentifierDetailsModel useridentifier);
        Response<UserIdentifierDetailsModel> UpdateUserIdentifierDetails(UserIdentifierDetailsModel useridentifier);
        Response<UserIdentifierDetailsModel> DeleteUserIdentifierDetails(long id, System.DateTime modifiedOn);
        Response<UserAdditionalDetailsModel> GetUserAdditionalDetails(int userID);
        Response<UserAdditionalDetailsModel> AddUserAdditionalDetails(UserAdditionalDetailsModel details);
        Response<UserAdditionalDetailsModel> UpdateUserAdditionalDetails(UserAdditionalDetailsModel details);
        Response<UserAdditionalDetailsModel> DeleteUserAdditionalDetails(long id, System.DateTime modifiedOn);
    }
}
