using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Configuration;

namespace Axis.DataEngine.Tests.Controllers
{
    [TestClass]
    public class BaseLiveTestController
    {
        #region Class Variables

        protected HttpClient _httpClient;

        #endregion

        #region Test Methods

        [TestInitialize]
        public void Initialize()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(ConfigurationManager.AppSettings["UnitTestUrl"]);
            _httpClient.DefaultRequestHeaders.Add("X-Token", ConfigurationManager.AppSettings["UnitTestToken"]);    
        }

        [TestCleanup]
        public void TearDown()
        {
            _httpClient.Dispose();
        }

        #endregion
    }
}