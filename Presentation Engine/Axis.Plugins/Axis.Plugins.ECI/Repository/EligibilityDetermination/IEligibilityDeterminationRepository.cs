using System;
using Axis.Model.Common;
using Axis.Model.ECI.EligibilityDetermination;
using Axis.Plugins.ECI.Models.EligibilityDetermination;

namespace Axis.Plugins.ECI.Repository.EligibilityDetermination
{
    public interface IEligibilityDeterminationRepository
    {
        Response<EligibilityDeterminationViewModel> GetEligibilityDetermination(long contactID);
        Response<EligibilityDeterminationViewModel> GetEligibility(long eligibilityID);
        Response<EligibilityTeamMemberModel> GetContactEligibilityMembers(long contactID);
        Response<EligibilityDeterminationViewModel> GetEligibilityNote(long eligibilityID);
        Response<EligibilityDeterminationViewModel> AddEligibility(EligibilityDeterminationViewModel eligibilityDetermination);
        Response<EligibilityDeterminationViewModel> UpdateEligibility(EligibilityDeterminationViewModel eligibilityDetermination);
        Response<EligibilityDeterminationViewModel> DeactivateEligibility(long eligibilityID, DateTime modifiedOn);
        Response<EligibilityDeterminationViewModel> SaveEligibilityNote(EligibilityDeterminationViewModel eligibilityDetermination);
    }
}
