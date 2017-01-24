using Axis.Configuration;
using Axis.Constant;
using Axis.Helpers;
using Axis.Model.Common;
using Axis.Service;
using System.Collections.Specialized;
using System.Collections.Generic;
using Axis.Model.Admin.UserScheduling;
using Axis.PresentationEngine.Areas.Account.Translator;
using Axis.PresentationEngine.Areas.Account.Model;
using Axis.Model.Admin;
using System.Threading.Tasks;
using Axis.Model.Account;

namespace Axis.PresentationEngine.Areas.Account.Respository
{
    /// <summary>
    /// User security repository implimentation
    /// </summary>
    public class UserSecurityRepository : IUserSecurityRepository
    {
        #region Class Variables

        private readonly CommunicationManager _communicationManager;

        private const string BaseRoute = "UserSecurity/";

        #endregion Class Variables

        #region Constructors
        public UserSecurityRepository()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        public UserSecurityRepository(string token)
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }
        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Get User security questions
        /// </summary>
        /// <param name="userID">user id</param>
        /// <returns></returns>       
        public Response<UserSecurityQuestionAnswerViewModel> GetUserSecurityQuestions(int userID)
        {
            var apiUrl = BaseRoute + "GetUserSecurityQuestions";
            var param = new NameValueCollection { { "userID", userID.ToString() } };

            var response = _communicationManager.Get<Response<UserSecurityQuestionAnswerModel>>(param, apiUrl);
            return response.ToModel();
        }



        /// <summary>
        /// Save update security questions
        /// </summary>
        /// <param name="securityQuestions">security question and answer object</param>
        /// <returns></returns>       
        public Response<UserSecurityQuestionAnswerViewModel> SaveUserSecurityQuestions(List<UserSecurityQuestionAnswerViewModel> securityQuestions)
        {
            var apiUrl = BaseRoute + "SaveUserSecurityQuestions";

            var securityModal = new List<UserSecurityQuestionAnswerModel>();

            if (securityQuestions == null)
            {
                return new Response<UserSecurityQuestionAnswerViewModel>()
                {
                    ResultCode = -1,
                    ResultMessage = "Select answer for a question."
                };
            }

            securityQuestions.ForEach(delegate(UserSecurityQuestionAnswerViewModel item)
            {
                securityModal.Add(item.ToViewModel());
            });

            var response = _communicationManager.Post<List<UserSecurityQuestionAnswerModel>, Response<UserSecurityQuestionAnswerModel>>(securityModal, apiUrl);
            return response.ToModel();

        }


        /// <summary>
        /// Get User security questions
        /// </summary>
        /// <param name="userID">user id</param>
        /// <returns></returns>
        public async Task<Response<UserSecurityQuestionAnswerModel>> GetUserSecurityAsync(int userID)
        {
            var apiUrl = BaseRoute + "GetUserSecurityQuestions";
            var param = new NameValueCollection { { "userID", userID.ToString() } };
            return (await _communicationManager.GetAsync<Response<UserSecurityQuestionAnswerModel>>(param, apiUrl));
        }

        /// <summary>
        /// Save User password 
        /// </summary>
        /// <param name="userPassowrd">user password model</param>
        /// <returns></returns>
        public Response<UserProfileViewModel> SaveUserPassword(UserProfileViewModel userProfile)
        {
            string apiUrl = BaseRoute + "SaveUserPassword";

            return _communicationManager.Post<UserProfileViewModel, Response<UserProfileViewModel>>(userProfile, apiUrl);
        }

        /// <summary>
        /// Update user signature details
        /// </summary>
        /// <param name="userProfile"></param>
        /// <returns></returns>
        public Response<UserProfileViewModel> UpdateUserSignatureDetails(UserProfileViewModel userProfile)
        {
            string apiUrl = BaseRoute + "UpdateUserSignatureDetails";

            return _communicationManager.Post<UserProfileViewModel, Response<UserProfileViewModel>>(userProfile, apiUrl);
        }
        #endregion


    }
}