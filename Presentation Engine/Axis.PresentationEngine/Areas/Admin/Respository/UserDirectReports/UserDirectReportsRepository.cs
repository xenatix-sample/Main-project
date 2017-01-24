using Axis.Configuration;
using Axis.Constant;
using Axis.Helpers;
using Axis.Model.Admin;
using Axis.Model.Common;
using Axis.PresentationEngine.Areas.Admin.Models;
using Axis.PresentationEngine.Areas.Admin.Translator;
using Axis.Service;
using System.Collections.Specialized;
using System.Globalization;
using System.Threading.Tasks;

namespace Axis.PresentationEngine.Areas.Admin.Respository
{
    public class UserDirectReportsRepository : IUserDirectReportsRepository
    {
        /// <summary>
        /// The _communication manager
        /// </summary>
        private readonly CommunicationManager _communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string BaseRoute = "UserDirectReports/";

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDirectReportsRepository"/> class.
        /// </summary>
        /// <param name="communicationManager">The communication manager.</param>
        public UserDirectReportsRepository()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDirectReportsRepository"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public UserDirectReportsRepository(string token)
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>

        public Response<UserDirectReportsViewModel> GetUsers(int userID, bool isMyProfile)
        {
            var route = isMyProfile ? "GetMyProfile" : "GetUsers";
            var apiUrl = BaseRoute + route;
            var param = new NameValueCollection { { "userID", userID.ToString() } };
            var response = _communicationManager.Get<Response<UserDirectReportsModel>>(param, apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>

        public async Task<Response<UserDirectReportsViewModel>> GetUsersAsync(int userID, bool isMyProfile)
        {
            var route = isMyProfile ? "GetMyProfile" : "GetUsers";
            var apiUrl = BaseRoute + route;
            var param = new NameValueCollection { { "userID", userID.ToString() } };

            return (await _communicationManager.GetAsync<Response<UserDirectReportsModel>>(param, apiUrl)).ToViewModel();
        }

        /// <summary>
        /// Gets the users by criteria.
        /// </summary>
        /// <param name="strSearch">The string search.</param>
        /// <returns></returns>

        public Response<UserDirectReportsViewModel> GetUsersByCriteria(string strSearch, bool isMyProfile)
        {
            var route = isMyProfile ? "GetMyProfileByCriteria" : "GetUsersByCriteria";
            var apiUrl = BaseRoute + route;
            var param = new NameValueCollection { { "strSearch", strSearch } };
            var response = _communicationManager.Get<Response<UserDirectReportsModel>>(param, apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Adds the specified user detail.
        /// </summary>
        /// <param name="userDetail">The user detail.</param>
        /// <returns></returns>

        public Response<UserDirectReportsViewModel> Add(UserDirectReportsViewModel userDetail, bool isMyProfile)
        {
            var route = isMyProfile ? "AddMyProfile" : "Add";
            var apiUrl = BaseRoute + route;
            var response = _communicationManager.Post<UserDirectReportsModel, Response<UserDirectReportsModel>>(userDetail.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Removes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>

        public Response<UserDirectReportsViewModel> Remove(long id, System.DateTime modifiedOn, bool isMyProfile)
        {
            string route = isMyProfile ? "RemoveMyProfile" : "Remove";
            string apiUrl = BaseRoute + route;
            var requestId = new NameValueCollection { { "id", id.ToString(CultureInfo.InvariantCulture) }, { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) } };
            var response = _communicationManager.Delete<Response<UserDirectReportsModel>>(requestId, apiUrl);
            return response.ToViewModel();
        }
    }
}