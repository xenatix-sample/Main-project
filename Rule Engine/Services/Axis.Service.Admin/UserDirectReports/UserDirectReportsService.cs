using System;
using Axis.Model.Common;
using Axis.Model.Admin;
using System.Collections.Specialized;
using System.Globalization;
using Axis.Configuration;
using Axis.Security;

namespace Axis.Service.Admin.UserDirectReports
{
    public class UserDirectReportsService : IUserDirectReportsService
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager _communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string BaseRoute = "UserDirectReports/";

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDirectReportsService"/> class.
        /// </summary>
        public UserDirectReportsService()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        public Response<UserDirectReportsModel> GetUsers(int userID)
        {
            const string apiUrl = BaseRoute + "GetUsers";
            var requestId = new NameValueCollection { { "userID", userID.ToString(CultureInfo.InvariantCulture) } };
            return _communicationManager.Get<Response<UserDirectReportsModel>>(requestId, apiUrl);
        }

        /// <summary>
        /// Gets the users by criteria.
        /// </summary>
        /// <param name="strSearch">The string search.</param>
        /// <returns></returns>
        public Response<UserDirectReportsModel> GetUsersByCriteria(string strSearch)
        {
            const string apiUrl = BaseRoute + "GetUsersByCriteria";
            var requestId = new NameValueCollection { { "strSearch", strSearch.ToString(CultureInfo.InvariantCulture) } };
            return _communicationManager.Get<Response<UserDirectReportsModel>>(requestId, apiUrl);
        }

        /// <summary>
        /// Adds the specified user detail.
        /// </summary>
        /// <param name="userDetail">The user detail.</param>
        /// <returns></returns>
        public Response<UserDirectReportsModel> Add(UserDirectReportsModel userDetail)
        {
            const string apiUrl = BaseRoute + "Add";
            return _communicationManager.Post<UserDirectReportsModel, Response<UserDirectReportsModel>>(userDetail, apiUrl);
        }

        /// <summary>
        /// Removes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        public Response<UserDirectReportsModel> Remove(long id, DateTime modifiedOn)
        {
            const string apiUrl = BaseRoute + "Remove";
            var requestId = new NameValueCollection
            {
                { "id", id.ToString(CultureInfo.InvariantCulture) },
                { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) }
            };
            return _communicationManager.Delete<Response<UserDirectReportsModel>>(requestId, apiUrl);
        }
    }
}
