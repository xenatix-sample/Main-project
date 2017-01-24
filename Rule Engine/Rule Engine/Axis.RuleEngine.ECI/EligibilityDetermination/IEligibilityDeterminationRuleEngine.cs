using System;
using Axis.Model.Common;
using Axis.Model.ECI;
using Axis.Model.ECI.EligibilityDetermination;

namespace Axis.RuleEngine.ECI.EligibilityDetermination
{
    public interface IEligibilityDeterminationRuleEngine
    {
        Response<EligibilityDeterminationModel> GetEligibilityDetermination(long contactID);
        Response<EligibilityDeterminationModel> GetEligibility(long eligibilityID);
        Response<EligibilityTeamMemberModel> GetContactEligibilityMembers(long contactID);
        Response<EligibilityDeterminationModel> GetEligibilityNote(long eligibilityID);
        Response<EligibilityDeterminationModel> AddEligibility(EligibilityDeterminationModel eligibilityDetermination);
        Response<EligibilityDeterminationModel> UpdateEligibility(EligibilityDeterminationModel eligibilityDetermination);
        Response<EligibilityDeterminationModel> DeactivateEligibility(long eligibilityID, DateTime modifiedOn);
        Response<EligibilityDeterminationModel> SaveEligibilityNote(EligibilityDeterminationModel eligibilityDetermination);
    }
}
