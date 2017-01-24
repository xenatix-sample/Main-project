using System;
using System.Web.Http;
using Axis.Model.Common;
using Axis.Plugins.ECI.Models.EligibilityDetermination;
using Axis.Plugins.ECI.Repository.EligibilityDetermination;
using Axis.PresentationEngine.Helpers.Controllers;

namespace Axis.Plugins.ECI.ApiControllers
{
    public class EligibilityDeterminationController : BaseApiController
    {
        #region Class Variables

        private readonly IEligibilityDeterminationRepository _eligibilityDeterminationRepository;

        #endregion

        #region Constructors

        public EligibilityDeterminationController(IEligibilityDeterminationRepository eligibilityDeterminationRepository)
        {
            _eligibilityDeterminationRepository = eligibilityDeterminationRepository;
        }

        #endregion

        #region Json Results

        /// <summary>
        /// Load the saved data for the contact's eligibility determination
        /// </summary>
        /// <param name="contactID"></param>
        /// <returns></returns>
        [HttpGet]
        public Response<EligibilityDeterminationViewModel> GetEligibilityDetermination(long contactID)
        {
            return _eligibilityDeterminationRepository.GetEligibilityDetermination(contactID);
        }

        /// <summary>
        /// Load the saved data for the contact's eligibility determination
        /// </summary>
        /// <param name="contactID"></param>
        /// <returns></returns>
        [HttpGet]
        public Response<EligibilityDeterminationViewModel> GetEligibility(long eligibilityID)
        {
            return _eligibilityDeterminationRepository.GetEligibility(eligibilityID);
        }

        /// <summary>
        /// Load the tema member data for the contact's eligibility determination
        /// </summary>
        /// <param name="contactID"></param>
        /// <returns></returns>
        [HttpGet]
        public Response<Axis.Model.ECI.EligibilityDetermination.EligibilityTeamMemberModel> GetContactEligibilityMembers(long contactID)
        {
            return _eligibilityDeterminationRepository.GetContactEligibilityMembers(contactID);
        }

        [HttpGet]
        public Response<EligibilityDeterminationViewModel> GetEligibilityNote(long eligibilityID)
        {
            return _eligibilityDeterminationRepository.GetEligibilityNote(eligibilityID);
        }

        /// <summary>
        /// Save the contact's eligibility determination data
        /// </summary>
        /// <param name="eligibilityDetermination"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<EligibilityDeterminationViewModel> AddEligibility(EligibilityDeterminationViewModel eligibilityDetermination)
        {            
            return _eligibilityDeterminationRepository.AddEligibility(eligibilityDetermination);
        }

        /// <summary>
        /// Update the contact's eligibility determination record
        /// </summary>
        /// <param name="eligibilityDetermination"></param>
        /// <returns></returns>
        [HttpPut]
        public Response<EligibilityDeterminationViewModel> UpdateEligibility(EligibilityDeterminationViewModel eligibilityDetermination)
        {
            return _eligibilityDeterminationRepository.UpdateEligibility(eligibilityDetermination);
        }

        [HttpPut]
        public Response<EligibilityDeterminationViewModel> SaveEligibilityNote(EligibilityDeterminationViewModel eligibilityDetermination)
        {
            return _eligibilityDeterminationRepository.SaveEligibilityNote(eligibilityDetermination);
        }

        /// <summary>
        /// Deactivate a contact's eligibility record
        /// </summary>
        /// <param name="eligibilityID"></param>
        /// <returns></returns>
        [HttpDelete]
        public Response<EligibilityDeterminationViewModel> DeactivateEligibility(long eligibilityID, DateTime modifiedOn)
        {
            modifiedOn = modifiedOn.ToUniversalTime();
            return _eligibilityDeterminationRepository.DeactivateEligibility(eligibilityID, modifiedOn);
        }

        #endregion
    }
}
