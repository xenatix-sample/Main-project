using System.Threading.Tasks;
using Axis.Model.Common;
using Axis.PresentationEngine.Areas.Account.Model;
using Axis.PresentationEngine.Areas.Admin.Model;

namespace Axis.PresentationEngine.Areas.Admin.Respository
{
    public interface IUserDetailRepository
    {
        Response<UserViewModel> GetUser(int userID);
        Task<Response<UserViewModel>> GetUserAsync(int userID);
        Response<UserViewModel> AddUser(UserViewModel userDetail);
        Response<UserViewModel> UpdateUser(UserViewModel userDetail);
        Response<CoSignaturesViewModel> GetCoSignatures(int userID);
        Task<Response<CoSignaturesViewModel>> GetCoSignaturesAsync(int userID);
        Response<CoSignaturesViewModel> AddCoSignatures(CoSignaturesViewModel signature);
        Response<CoSignaturesViewModel> UpdateCoSignatures(CoSignaturesViewModel signature);
        Response<CoSignaturesViewModel> DeleteCoSignatures(long id, System.DateTime modifiedOn);
        Response<UserIdentifierViewModel> GetUserIdentifierDetails(int userID);
        Task<Response<UserIdentifierViewModel>> GetUserIdentifierDetailsAsync(int userID);
        Response<UserIdentifierViewModel> AddUserIdentifierDetails(UserIdentifierViewModel useridentifier);
        Response<UserIdentifierViewModel> UpdateUserIdentifierDetails(UserIdentifierViewModel useridentifier);
        Response<UserIdentifierViewModel> DeleteUserIdentifierDetails(long id, System.DateTime modifiedOn);
        Response<UserAdditionalDetailsViewModel> GetUserAdditionalDetails(int userID);
        Task<Response<UserAdditionalDetailsViewModel>> GetUserAdditionalDetailsAsync(int userID);
        Response<UserAdditionalDetailsViewModel> AddUserAdditionalDetails(UserAdditionalDetailsViewModel details);
        Response<UserAdditionalDetailsViewModel> UpdateUserAdditionalDetails(UserAdditionalDetailsViewModel details);
        Response<UserAdditionalDetailsViewModel> DeleteUserAdditionalDetails(long id, System.DateTime modifiedOn);
    }
}