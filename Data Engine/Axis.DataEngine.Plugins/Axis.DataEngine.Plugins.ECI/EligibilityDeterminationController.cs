using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Http;
using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.ECI.EligibilityDetermination;
using Axis.Model.Common;
using Axis.Model.ECI;
using Axis.Model.ECI.EligibilityDetermination;

namespace Axis.DataEngine.Plugins.ECI
{
    public class EligibilityDeterminationController : BaseApiController
    {
        #region Class Variables

        readonly IEligibilityDeterminationDataProvider _eligibilityDeterminationDataProvider = null;

        #endregion

        #region Constructors

        public EligibilityDeterminationController(IEligibilityDeterminationDataProvider eligibilityDeterminationDataProvider)
        {
            _eligibilityDeterminationDataProvider = eligibilityDeterminationDataProvider;
        }

        #endregion

        #region Public Methods

        [HttpGet]
        public IHttpActionResult GetEligibilityDetermination(long contactID)
        {
            var eligibilityResponse = _eligibilityDeterminationDataProvider.GetEligibilityDetermination(contactID);
            var contactMembersResponse = GetContactEligibilityMembers(contactID);

            //update to a linq query after testing
            if (contactMembersResponse.DataItems.Count > 0)
            {
                foreach (var eligibility in eligibilityResponse.DataItems)
                {
                    var tmpEligibilityID = eligibility.EligibilityID;
                    foreach (
                        var member in contactMembersResponse.DataItems.Where(x => x.EligibilityID == tmpEligibilityID))
                    {
                        eligibility.Members.Add(member.UserID);
                    }
                }
            }

            return new HttpResult<Response<EligibilityDeterminationModel>>(eligibilityResponse, Request);
        }

        [HttpGet]
        public IHttpActionResult GetEligibility(long eligibilityID)
        {
            var eligibilityResponse = _eligibilityDeterminationDataProvider.GetEligibility(eligibilityID);
            var eligibilityMemberResponse = GetEligibilityMembers(eligibilityID);

            //update to a linq query after testing
            foreach (var member in eligibilityMemberResponse.DataItems)
            {
                eligibilityResponse.DataItems[0].Members.Add(member.UserID);
            }

            return new HttpResult<Response<EligibilityDeterminationModel>>(eligibilityResponse, Request);
        }

        [HttpGet]
        public IHttpActionResult GetEligibilityNote(long eligibilityID)
        {
            return new HttpResult<Response<EligibilityDeterminationModel>>(_eligibilityDeterminationDataProvider.GetEligibilityNote(eligibilityID), Request);
        }

        [HttpPost]
        public IHttpActionResult AddEligibility(EligibilityDeterminationModel eligibilityDetermination)
        {
            return new HttpResult<Response<EligibilityDeterminationModel>>(_eligibilityDeterminationDataProvider.AddEligibility(eligibilityDetermination), Request);
        }

        [HttpPut]
        public IHttpActionResult UpdateEligibility(EligibilityDeterminationModel eligibilityDetermination)
        {
            return new HttpResult<Response<EligibilityDeterminationModel>>(_eligibilityDeterminationDataProvider.UpdateEligibility(eligibilityDetermination), Request);
        }

        [HttpDelete]
        public IHttpActionResult DeactivateEligibility(long eligibilityID, DateTime modifiedOn)
        {
            return new HttpResult<Response<EligibilityDeterminationModel>>(_eligibilityDeterminationDataProvider.DeactivateEligibility(eligibilityID, modifiedOn), Request);
        }

        [HttpPut]
        public IHttpActionResult SaveEligibilityNote(EligibilityDeterminationModel eligibilityDetermination)
        {
            return new HttpResult<Response<EligibilityDeterminationModel>>(_eligibilityDeterminationDataProvider.SaveEligibilityNote(eligibilityDetermination), Request);
        }

        #endregion

        #region Private Methods

        [HttpGet]
        private Response<EligibilityTeamMemberModel> GetContactEligibilityMembers(long contactID)
        {
            return _eligibilityDeterminationDataProvider.GetContactEligibilityMembers(contactID);
        }

        [HttpGet]
        private Response<EligibilityTeamMemberModel> GetEligibilityMembers(long eligibilityID)
        {
            return _eligibilityDeterminationDataProvider.GetEligibilityMembers(eligibilityID);
        }

        #endregion
    }
}
