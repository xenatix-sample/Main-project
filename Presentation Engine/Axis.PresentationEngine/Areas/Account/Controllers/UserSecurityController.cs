using System.Web.Mvc;
using Axis.PresentationEngine.Helpers.Controllers;
using Axis.PresentationEngine.Areas.Account.Respository;
using Axis.PresentationEngine.Areas.Account.Model;
using System.Collections.Generic;
using Axis.Model.Account;

namespace Axis.PresentationEngine.Areas.Account.Controllers
{
    /// <summary>
    /// View controller
    /// </summary>
    public class UserSecurityController : BaseController
    {
        /// <summary>
        /// User security repository instance
        /// </summary>
        private readonly IUserSecurityRepository _securityRepository;


        #region Constructors

        public UserSecurityController(IUserSecurityRepository schedulingRepository)
        {
            _securityRepository = schedulingRepository;
        }

        #endregion

        /// <summary>
        /// View
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }


        #region Json Results

        /// <summary>
        /// Get User security questions
        /// </summary>
        /// <param name="userID">user id</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetUserSecurityQuestions(int userID)
        {
            var questions = _securityRepository.GetUserSecurityQuestions(userID);
            return Json(questions, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Save security questions
        /// </summary>
        /// <param name="securityQuestions">security question and answer object</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveUserSecurityQuestions(List<UserSecurityQuestionAnswerViewModel> securityQuestions)
        {
            //need to create repo method
            return Json(_securityRepository.SaveUserSecurityQuestions(securityQuestions), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Save User password 
        /// </summary>
        /// <param name="userPassowrd">user password model</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveUserPassword(UserProfileViewModel userProfile)
        {
            var questions = _securityRepository.SaveUserPassword(userProfile);
            return Json(questions, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Update user signature password details 
        /// </summary>
        /// <param name="userPassowrd">user profile model</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateUserSignatureDetails(UserProfileViewModel userProfile)
        {
            var questions = _securityRepository.UpdateUserSignatureDetails(userProfile);
            return Json(questions, JsonRequestBehavior.AllowGet);
        }


        #endregion
    }
}