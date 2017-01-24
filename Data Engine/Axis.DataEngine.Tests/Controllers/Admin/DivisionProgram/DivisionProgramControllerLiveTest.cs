using Axis.Model.Admin;
using Axis.Model.Common;
using Axis.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Specialized;
using System.Configuration;
using System.Collections.Generic;

namespace Axis.DataEngine.Tests.Controllers.Admin.DivisionProgram
{
    [TestClass]
    public class DivisionProgramControllerLiveTest
    {
        #region Class Variables

        /// <summary>
        /// The communication manager
        /// </summary>
        private CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "DivisionProgram/";


        /// <summary>
        /// The request model
        /// </summary>
        //private DivisionProgramModel requestModel = null;
        private long userID = 1;

        #endregion Class Variables

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            communicationManager = new CommunicationManager("X-Token", ConfigurationManager.AppSettings["UnitTestToken"]);
            communicationManager.UnitTestUrl = ConfigurationManager.AppSettings["UnitTestUrl"];
        }

        [TestMethod]
        public void GetUserDivisionPrograms_Success()
        {
            var url = baseRoute + "GetDivisionPrograms";

            var param = new NameValueCollection();
            param.Add("userID", userID.ToString());

            var response = communicationManager.Get<Response<DivisionProgramModel>>(param, url);

            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.ResultCode == 0, "Result Code must be 0");
        }

        [TestMethod]
        public void SaveUserDivisionPrograms_Success()
        {
            var url = baseRoute + "SaveDivisionProgram";

            var Program1 = new ProgramsModel
            {
                MappingID = 9,
                IsActive = true,
                ProgramUnits = new List<DivisionProgramBaseModel>
                {
                    new DivisionProgramBaseModel {MappingID =  53, IsActive = false}
                }
            };

            var Program2 = new ProgramsModel
            {
                MappingID = 10,
                IsActive = true,
                ProgramUnits = new List<DivisionProgramBaseModel>
                {
                    new DivisionProgramBaseModel {MappingID =  55, IsActive = false}
                }
            };


            var userDivisionProgram = new DivisionProgramModel
            {
                MappingID = 3,
                IsActive = true,
                Programs = new List<ProgramsModel> { Program1, Program2 }
            };

            var response = communicationManager.Post<DivisionProgramModel, Response<DivisionProgramModel>>(userDivisionProgram, url);

            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.ResultCode == 0, "Result Code must be 0");
        }
    }
}
