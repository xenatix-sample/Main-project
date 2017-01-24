using System;
using Axis.PresentationEngine.Areas.Security.Model;
using Axis.PresentationEngine.Areas.Security.Repository;
using Axis.PresentationEngine.Helpers.Controllers;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Axis.PresentationEngine.Areas.Security.Controllers
{
    /// <summary>
    ///
    /// </summary>
    public class RoleController : BaseController
    {
        /// <summary>
        /// The security repository
        /// </summary>
        private ISecurityRepository securityRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleController"/> class.
        /// </summary>
        /// <param name="securityRepository">The security repository.</param>
        public RoleController(ISecurityRepository securityRepository)
        {
            this.securityRepository = securityRepository;
        }

        // GET: Security/Security
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Edits this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit()
        {
            return View();
        }

        public ActionResult RoleDetails()
        {
            return View();
        }
        public ActionResult AssignModules()
        {
            return View();
        }
        public ActionResult AssignModuleDetails()
        {
            return View();
        }

        public ActionResult RoleNavigation()
        {
            return View();
        }

        /// <summary>
        /// Gets the roles.
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetRoles(string roleName)
        {
            return Json(securityRepository.GetRoles(roleName), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Adds the role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddRole(RoleViewModel role)
        {
            var response = securityRepository.AddRole(role);
            ClearCache(response);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Updates the role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateRole(RoleViewModel role)
        {
            var response = securityRepository.UpdateRole(role);
            ClearCache(response);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Deletes the role.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        public JsonResult DeleteRole(long id, DateTime modifiedOn)
        {
            var response = securityRepository.DeleteRole(id, modifiedOn);
            modifiedOn = modifiedOn.ToUniversalTime();
            ClearCache(response);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the module.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetModule()
        {
            return Json(securityRepository.GetModule(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Assigns the module to role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AssignModuleToRole(List<RoleModuleViewModel> role)
        {
            var response = securityRepository.AssignModuleToRole(role);
            ClearCache(response);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the role by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetRoleById(long id)
        {
            return Json(securityRepository.GetRoleById(id), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the module by role identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetModuleByRoleId(long id)
        {
            return Json(securityRepository.GetModuleByRoleId(id), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the assigned permission by module identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetAssignedPermissionByModuleId(long id, long roleId)
        {
            return Json(securityRepository.GetAssignedPermissionByModuleId(id, roleId), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the assigned permission by feature identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetAssignedPermissionByFeatureId(long id, long roleId)
        {
            return Json(securityRepository.GetAssignedPermissionByFeatureId(id, roleId), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the feature permission by module identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetFeaturePermissionByModuleId(long id, long roleId)
        {
            return Json(securityRepository.GetFeaturePermissionByModuleId(id, roleId), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the feature permission by role identifier.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetFeaturePermissionByRoleId(long roleId)
        {
            return Json(securityRepository.GetFeaturePermissionByRoleId(roleId), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the module permission by role identifier.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetModulePermissionByRoleId(long roleId)
        {
            return Json(securityRepository.GetModulePermissionByRoleId(roleId), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Assigns the role permission.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AssignRolePermission(RolePermissionsViewModel role)
        {
            var response = securityRepository.AssignRolePermission(role);
            ClearCache(response);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}
