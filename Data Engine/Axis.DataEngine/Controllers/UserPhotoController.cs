using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Admin.UserPhoto;
using Axis.Model.Admin;
using Axis.Model.Common;
using System;
using System.Web.Http;

namespace Axis.DataEngine.Service.Controllers
{
    /// <summary>
    ///
    /// </summary>
    public class UserPhotoController : BaseApiController
    {
        /// <summary>
        /// The user photo data provider
        /// </summary>
        private IUserPhotoDataProvider userPhotoDataProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserPhotoController"/> class.
        /// </summary>
        /// <param name="userPhotoDataProvider">The user photo data provider.</param>
        public UserPhotoController(IUserPhotoDataProvider userPhotoDataProvider)
        {
            this.userPhotoDataProvider = userPhotoDataProvider;
        }

        /// <summary>
        /// Gets the user photo.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetUserPhoto(int userID)
        {
            return new HttpResult<Response<UserPhotoModel>>(userPhotoDataProvider.GetUserPhoto(userID), Request);
        }

        /// <summary>
        /// Gets the user photo by identifier.
        /// </summary>
        /// <param name="userPhotoID">The user photo identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetUserPhotoById(long userPhotoID)
        {
            return new HttpResult<Response<UserPhotoModel>>(userPhotoDataProvider.GetUserPhotoById(userPhotoID), Request);
        }

        /// <summary>
        /// Adds the user photo.
        /// </summary>
        /// <param name="userPhoto">The user photo.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddUserPhoto(UserPhotoModel userPhoto)
        {
            return new HttpResult<Response<UserPhotoModel>>(userPhotoDataProvider.AddUserPhoto(userPhoto), Request);
        }

        /// <summary>
        /// Updates the user photo.
        /// </summary>
        /// <param name="userPhoto">The user photo.</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateUserPhoto(UserPhotoModel userPhoto)
        {
            return new HttpResult<Response<UserPhotoModel>>(userPhotoDataProvider.UpdateUserPhoto(userPhoto), Request);
        }

        /// <summary>
        /// Deletes the user photo.
        /// </summary>
        /// <param name="userPhotoID">The user photo identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeleteUserPhoto(long userPhotoID, DateTime modifiedOn)
        {
            return new HttpResult<Response<UserPhotoModel>>(userPhotoDataProvider.DeleteUserPhoto(userPhotoID, modifiedOn), Request);
        }
    }
}