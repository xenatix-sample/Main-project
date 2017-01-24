using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Model.Account;
using Axis.PresentationEngine.Areas.Account.Controllers;
using Axis.PresentationEngine.Areas.Account.Repository;
using System.Configuration;
using Axis.PresentationEngine.Areas.Account.Model;
using System.Web.Mvc;
using Axis.Model.Common;

namespace Axis.PresentationEngine.Tests.Controllers.Login
{
    [TestClass]
    public class AccountControllerTest
    {
        private AccountRepository repo = null;

        [TestInitialize]
        public void Initialize()
        {

            repo = new AccountRepository();
        }

        [TestMethod]
        public void Login_Success()
        {
            // Arrange
            UserViewModel userViewModel = new UserViewModel()
            {
                UserName = "achoudhary",
                Password = "Welcome1",
            };

            // Act
            var result = repo.Authenticate(userViewModel);
            var response = result as AuthenticationModel;

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(true, response.IsAuthenticated);
        }

        [TestMethod]
        public void Login_Failed()
        {
            // Arrange
            UserViewModel userViewModel = new UserViewModel()
            {
                UserName = "achoudhary",
                Password = "Welcome0",
            };

            // Act
            var result = repo.Authenticate(userViewModel);
            var response = result as AuthenticationModel;

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(false, response.IsAuthenticated);
        }
    }
}



