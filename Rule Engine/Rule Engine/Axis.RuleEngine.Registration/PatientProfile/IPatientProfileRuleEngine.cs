using Axis.Model.Common;
using Axis.Model.Registration;

namespace Axis.RuleEngine.Registration
{
    /// <summary>
    ///
    /// </summary>
    public interface IPatientProfileRuleEngine
    {
        /// <summary>
        /// Gets the Patient Profile.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        Response<PatientProfileModel> GetPatientProfile(long contactID);
        
    }
}