using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Model.Security;
using Axis.PresentationEngine.Areas.Account.Model;
using Axis.PresentationEngine.Areas.Account.Repository;
using Axis.PresentationEngine.Helpers;
using Axis.PresentationEngine.Helpers.Controllers;
using Axis.PresentationEngine.Helpers.Filters;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;

namespace Axis.PresentationEngine.Areas.Account.Controllers
{
    public class AccountController : BaseController
    {
        private IAccountRepository accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        [HttpGet]
        [Authorization(AllowAnonymous = true)]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Authorization(AllowAnonymous = true)]
        public ActionResult Login(UserViewModel user)
        {
            if (ModelState.IsValid)
            {
                //Retrive Client IP Address
                user.IPAddress = Request.UserHostAddress;
                //Retrive session id
                user.SessionID = Session.SessionID;
                var authModel = accountRepository.Authenticate(user);
                //authModel.User.DaysToExpire = 0; // password expiration 0 days left -- Test.
                if (authModel.IsAuthenticated && authModel.User.DaysToExpire > 0)
                {
                    WebSecurity.SignIn(user.UserName, authModel.Token.Token);
                    Session["UserID"] = authModel.User.UserID;
                    Session["UserName"] = user.UserName;
                    Session["UserFirstName"] = authModel.User.FirstName;
                    Session["UserFullName"] = string.Format("{0} {1}", authModel.User.FirstName, authModel.User.LastName);
                    Session["UserRolePrimary"] = (authModel.User.Roles.FirstOrDefault() ?? new RoleModel()).Name;
                    Session["PasswordHash"] =
                        Convert.ToBase64String(new SHA256Managed().ComputeHash(Encoding.UTF8.GetBytes(user.Password)));
                    Session["DaysToExpire"] = authModel.User.DaysToExpire;
                    return RedirectToAction("Xenatix", "Home", new { area = string.Empty });
                }
                else if (authModel.IsAuthenticated && authModel.User.DaysToExpire <= 0)
                {
                    Session["UserID"] = authModel.User.UserID;
                    return RedirectToAction("ResetPassword", "ForgotPassword");
                }
                else
                {
                    ViewBag.LoginError = false;
                    ViewBag.PromptMessageCode = authModel.Resultcode;
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public JsonResult GetNavigationItems()
        {
            Response<NavigationModel> navigationItems = accountRepository.GetNavigationItems();
            navigationItems.DataItems.First().PasswordHash = (string)Session["PasswordHash"];
            Session["UserName"] = navigationItems.DataItems.First().UserName;
            Session["UserID"] = navigationItems.DataItems.First().UserID;
            return Json(navigationItems, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetNavigationItemsOnline()
        {
            Response<NavigationModel> navigationItems = accountRepository.GetNavigationItems();
            navigationItems.DataItems.First().PasswordHash = (string)Session["PasswordHash"];
            Session["UserName"] = navigationItems.DataItems.First().UserName;
            Session["UserID"] = navigationItems.DataItems.First().UserID;
            return Json(navigationItems, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult PostNavigationItems(NavigationModel model = null)
        {
            return Json(string.Empty, JsonRequestBehavior.AllowGet);
        }

        public void Logoff()
        {
            WebSecurity.SignOut();
            //return RedirectToAction("Login");
        }
    }
}