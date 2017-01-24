using Axis.Model.Common;
using Axis.PresentationEngine.Areas.Admin.Models;
using Axis.PresentationEngine.Areas.Admin.Respository;
using Axis.PresentationEngine.Helpers.Controllers;
using System;
using System.Web.Http;

namespace Axis.PresentationEngine.Areas.Admin.ApiControllers
{
    public class UserDirectReportsController : BaseApiController
    {
        /// <summary>
        /// The _user direct reports repository
        /// </summary>
        readonly IUserDirectReportsRepository _userDirectReportsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDirectReportsController"/> class.
        /// </summary>
        /// <param name="userDirectReportsRepository">The user direct reports repository.</param>
        public UserDirectReportsController(IUserDirectReportsRepository userDirectReportsRepository)
        {
            _userDirectReportsRepository = userDirectReportsRepository;
        }

        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<UserDirectReportsViewModel> GetUsers(int userID,bool isMyProfile)
        {
            return _userDirectReportsRepository.GetUsers(userID, isMyProfile);
        }

        /// <summary>
        /// Gets the users by criteria.
        /// </summary>
        /// <param name="strSearch">The string search.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<UserDirectReportsViewModel> GetUsersByCriteria(string strSearch,bool isMyProfile)
        {
            return _userDirectReportsRepository.GetUsersByCriteria(strSearch, isMyProfile);
        }
        
        /// <summary>
        /// Adds the specified user detail.
        /// </summary>
        /// <param name="userDetail">The user detail.</param>
        /// <returns></returns>
        [HttpPost]
        public Response<UserDirectReportsViewModel> Add(UserDirectReportsViewModel userDetail,bool isMyProfile)
        {
            return _userDirectReportsRepository.Add(userDetail, isMyProfile);
        }

        /// <summary>
        /// Removes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        [HttpDelete]
        public Response<UserDirectReportsViewModel> Remove(long id, DateTime modifiedOn,bool isMyProfile)
        {
            modifiedOn = modifiedOn.ToUniversalTime();
            return _userDirectReportsRepository.Remove(id, modifiedOn, isMyProfile);
        }

    }
}