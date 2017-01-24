using System.Web.Http;
using Axis.DataProvider.Registration;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.DataEngine.Helpers.Results;
using Axis.DataEngine.Helpers.Controllers;

namespace Axis.DataEngine.Plugins.Registration
{
    public class PatientProfileController : BaseApiController
    {
        /// <summary>
        /// The patient Profile Data Provider
        /// </summary>
        readonly IPatientProfileDataProvider _patientProfileDataProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="PatientProfileController"/> class.
        /// </summary>
        /// <param name="patientProfileDataProvider">The patient Profile Data Provider.</param>
        public PatientProfileController(IPatientProfileDataProvider patientProfileDataProvider)
        {
            _patientProfileDataProvider = patientProfileDataProvider;
        }

        /// <summary>
        /// Gets the Patient Profile
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public IHttpActionResult GetPatientProfile(long contactID)
        {
            return new HttpResult<Response<PatientProfileModel>>(_patientProfileDataProvider.GetPatientProfile(contactID), Request);
        }

       
    }
}