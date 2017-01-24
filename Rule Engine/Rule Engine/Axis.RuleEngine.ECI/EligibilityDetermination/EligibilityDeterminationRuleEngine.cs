using System;
using Axis.Model.Common;
using Axis.Model.ECI;
using Axis.Model.ECI.EligibilityDetermination;
using Axis.Service.ECI.EligibilityDetermination;

namespace Axis.RuleEngine.ECI.EligibilityDetermination
{
    public class EligibilityDeterminationRuleEngine : IEligibilityDeterminationRuleEngine
    {
        #region Class Variables

        private readonly IEligibilityDeterminationService _eligibilityDeterminationService;

        #endregion

        #region Constructors

        public EligibilityDeterminationRuleEngine(IEligibilityDeterminationService eligibilityDeterminationService)
        {
            _eligibilityDeterminationService = eligibilityDeterminationService;
        }

        #endregion

        #region Public Methods

        public Response<EligibilityDeterminationModel> GetEligibilityDetermination(long contactID)
        {
            return _eligibilityDeterminationService.GetEligibilityDetermination(contactID);
        }

        public Response<EligibilityDeterminationModel> GetEligibility(long eligibilityID)
        {
            return _eligibilityDeterminationService.GetEligibility(eligibilityID);
        }

        public Response<EligibilityTeamMemberModel> GetContactEligibilityMembers(long contactID)
        {
            return _eligibilityDeterminationService.GetContactEligibilityMembers(contactID);
        }

        public Response<EligibilityDeterminationModel> GetEligibilityNote(long eligibilityID)
        {
            return _eligibilityDeterminationService.GetEligibilityNote(eligibilityID);
        }

        public Response<EligibilityDeterminationModel> AddEligibility(EligibilityDeterminationModel eligibilityDetermination)
        {
            return _eligibilityDeterminationService.AddEligibility(eligibilityDetermination);
        }

        public Response<EligibilityDeterminationModel> UpdateEligibility(EligibilityDeterminationModel eligibilityDetermination)
        {
            return _eligibilityDeterminationService.UpdateEligibility(eligibilityDetermination);
        }

        public Response<EligibilityDeterminationModel> DeactivateEligibility(long eligibilityID, DateTime modifiedOn)
        {
            return _eligibilityDeterminationService.DeactivateEligibility(eligibilityID, modifiedOn);
        }

        public Response<EligibilityDeterminationModel> SaveEligibilityNote(EligibilityDeterminationModel eligibilityDetermination)
        {
            return _eligibilityDeterminationService.SaveEligibilityNote(eligibilityDetermination);
        }

        #endregion
    }
}
