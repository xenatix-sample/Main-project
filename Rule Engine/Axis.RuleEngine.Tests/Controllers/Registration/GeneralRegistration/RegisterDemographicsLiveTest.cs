using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.RuleEngine.Registration;
using Moq;
using Axis.Model.Registration;
using Axis.Model.Address;
using Axis.Model.Common;
using System.Linq;
using Axis.RuleEngine.Plugins.Registration;
using Axis.RuleEngine.Service.Controllers;
using Axis.RuleEngine.Helpers.Results;
using System.Net.Http;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Helpers;
using Axis.Service;
using System.Collections.Specialized;

namespace Axis.RuleEngine.Tests.Controllers
{
    [TestClass]
    public class RegisterDemographicsLiveTest
    {
        #region Class Variables

        private CommunicationManager communicationManager;
        private const string baseRoute = "registration/";
        //private long contactId = 1;
        private string searchCriteria = "N";
        private string ContactType = "p";
        #endregion

        [TestInitialize]
        public void Initialize()
        {
            communicationManager = new CommunicationManager("X-Token", ConfigurationManager.AppSettings["UnitTestToken"]);
            communicationManager.UnitTestUrl = ConfigurationManager.AppSettings["UnitTestUrl"];
        }

     
        #region Client Search 

        [TestMethod]
        public void GetClientSummary_Success()
        {
            var url = baseRoute + "getClientSummary";

            var param = new NameValueCollection();
            param.Add("SearchCriteria", searchCriteria.ToString());
            param.Add("ContactType", ContactType.ToString());

            var response = communicationManager.Get<Response<ContactDemographicsModel>>(param, url);

            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Record not found");

        }

        #endregion
    }
}
