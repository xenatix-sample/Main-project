using Axis.Constant;
using Axis.Model.Admin;
using Axis.Model.Common;
using Axis.RuleEngine.Admin.UserPhoto;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Filters;
using Axis.RuleEngine.Helpers.Results;
using System;
using System.Web.Http;

namespace Axis.RuleEngine.Service.Controllers
{
    /// <summary>
    ///
    /// </summary>
    
    public class UserPhotoController : BaseApiController
    {
        /// <summary>
        /// The user photo rule engine
        /// </summary>
        private readonly IUserPhotoRuleEngine userPhotoRuleEngine;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserPhotoController"/> class.
        /// </summary>
        /// <param name="userPhotoRuleEngine">The user photo rule engine.</param>
        public UserPhotoController(IUserPhotoRuleEngine userPhotoRuleEngine)
        {
            this.userPhotoRuleEngine = userPhotoRuleEngine;
        }

        #region Site Admin Public Methods
        /// <summary>
        /// Gets the user photo.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new String[] { SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserPhoto, SiteAdministrationPermissionKey.SiteAdministration_MyProfile_UserPhoto }, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetUserPhoto(int userID)
        {
            return new HttpResult<Response<UserPhotoModel>>(userPhotoRuleEngine.GetUserPhoto(userID), Request);
        }

        /// <summary>
        /// Gets the user photo by identifier.
        /// </summary>
        /// <param name="userPhotoID">The user photo identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserPhoto, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetUserPhotoById(long userPhotoID)
        {
            return new HttpResult<Response<UserPhotoModel>>(userPhotoRuleEngine.GetUserPhotoById(userPhotoID), Request);
        }

        /// <summary>
        /// Adds the user photo.
        /// </summary>
        /// <param name="userPhoto">The user photo.</param>
        /// <returns></returns>
        [Authorization(PermissionKey =SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserPhoto, Permission = Permission.Create)]
        [HttpPost]
        public IHttpActionResult AddUserPhoto(UserPhotoModel userPhoto)
        {
            return new HttpResult<Response<UserPhotoModel>>(userPhotoRuleEngine.AddUserPhoto(userPhoto), Request);
        }

        /// <summary>
        /// Updates the user photo.
        /// </summary>
        /// <param name="userPhoto">The user photo.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserPhoto, Permission = Permission.Update)]
        [HttpPut]
        public IHttpActionResult UpdateUserPhoto(UserPhotoModel userPhoto)
        {
            return new HttpResult<Response<UserPhotoModel>>(userPhotoRuleEngine.UpdateUserPhoto(userPhoto), Request);
        }

        /// <summary>
        /// Deletes the user photo.
        /// </summary>
        /// <param name="userPhotoID">The user photo identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserPhoto, Permission = Permission.Delete)]
        [HttpDelete]
        public IHttpActionResult DeleteUserPhoto(long userPhotoID, DateTime modifiedOn)
        {
            return new HttpResult<Response<UserPhotoModel>>(userPhotoRuleEngine.DeleteUserPhoto(userPhotoID, modifiedOn), Request);
        }
        #endregion Site Admin Public Methods

        #region My Profile Public Methods
        /// <summary>
        /// Gets the user photo.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetMyProfilePhoto(int userID)
        {
            return new HttpResult<Response<UserPhotoModel>>(userPhotoRuleEngine.GetUserPhoto(userID), Request);
        }

        /// <summary>
        /// Gets the user photo by identifier.
        /// </summary>
        /// <param name="userPhotoID">The user photo identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetMyProfilePhotoById(long userPhotoID)
        {
            return new HttpResult<Response<UserPhotoModel>>(userPhotoRuleEngine.GetUserPhotoById(userPhotoID), Request);
        }

        /// <summary>
        /// Adds the user photo.
        /// </summary>
        /// <param name="userPhoto">The user photo.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddMyProfilePhoto(UserPhotoModel userPhoto)
        {
            return new HttpResult<Response<UserPhotoModel>>(userPhotoRuleEngine.AddUserPhoto(userPhoto), Request);
        }

        /// <summary>
        /// Updates the user photo.
        /// </summary>
        /// <param name="userPhoto">The user photo.</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateMyProfilePhoto(UserPhotoModel userPhoto)
        {
            return new HttpResult<Response<UserPhotoModel>>(userPhotoRuleEngine.UpdateUserPhoto(userPhoto), Request);
        }

        /// <summary>
        /// Deletes the user photo.
        /// </summary>
        /// <param name="userPhotoID">The user photo identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeleteMyProfilePhoto(long userPhotoID, DateTime modifiedOn)
        {
            return new HttpResult<Response<UserPhotoModel>>(userPhotoRuleEngine.DeleteUserPhoto(userPhotoID, modifiedOn), Request);
        }
        #endregion My Profile Public Methods
    }
}