using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Service.Registration;

namespace Axis.RuleEngine.Registration
{
    /// <summary>
    ///
    /// </summary>
    public class PatientProfileRuleEngine : IPatientProfileRuleEngine
    {
        /// <summary>
        /// The patient Profile Service
        /// </summary>
        private readonly IPatientProfileService _patientProfileService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PatientProfileRuleEngine"/> class.
        /// </summary>
        /// <param name="patientProfileService">The patient Profile Service.</param>
        public PatientProfileRuleEngine(IPatientProfileService patientProfileService)
        {
            _patientProfileService = patientProfileService;
        }

        /// <summary>
        /// Gets the Patient Profile
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<PatientProfileModel> GetPatientProfile(long contactID)
        {
            return _patientProfileService.GetPatientProfile(contactID);
        }

       
    }
}