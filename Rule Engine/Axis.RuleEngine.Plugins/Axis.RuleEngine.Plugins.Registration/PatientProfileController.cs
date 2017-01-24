using System.Collections.Generic;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Filters;
using Axis.RuleEngine.Helpers.Results;
using Axis.RuleEngine.Registration;
using System.Web.Http;
using Axis.Helpers.Validation;
using Axis.Constant;

namespace Axis.RuleEngine.Plugins.Registration
{
    /// <summary>
    ///
    /// </summary>
    
    public class PatientProfileController : BaseApiController
    {
        /// <summary>
        /// The _patient Profile Rule Engine
        /// </summary>
        private readonly IPatientProfileRuleEngine _patientProfileRuleEngine;

        /// <summary>
        /// Initializes a new instance of the <see cref="PatientProfileController"/> class.
        /// </summary>
        /// <param name="patientProfileRuleEngine">The patient Profile Rule Engine.</param>
        public PatientProfileController(IPatientProfileRuleEngine patientProfileRuleEngine)
        {
            _patientProfileRuleEngine = patientProfileRuleEngine;
        }

        /// <summary>
        /// Gets the Patient Profile
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = RegistrationPermissionKey.Registration_Demography, Permission = Permission.Read)]
        public IHttpActionResult GetPatientProfile(long contactID)
        {
            return new HttpResult<Response<PatientProfileModel>>(_patientProfileRuleEngine.GetPatientProfile(contactID), Request);
        }
      
    }
}