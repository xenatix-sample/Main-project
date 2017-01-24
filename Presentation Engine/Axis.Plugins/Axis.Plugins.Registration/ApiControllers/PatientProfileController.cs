using Axis.Plugins.Registration.Repository.PatientProfile;
using Axis.PresentationEngine.Helpers.Controllers;
using System.Threading.Tasks;
using System.Web.Http;
using Axis.Model.Common;
using Axis.Plugins.Registration.Model;

namespace Axis.Plugins.Registration.ApiControllers
{
    public class PatientProfileController : BaseApiController
    {
        #region Class Variables

        private readonly IPatientProfileRepository _patientProfileRepository;

        #endregion

        #region Constructors

        public PatientProfileController(IPatientProfileRepository patientProfileRepository)
        {
            _patientProfileRepository = patientProfileRepository;
        }

        #endregion

        #region Data API

        /// <summary>
        /// To Get Patient Profile
        /// </summary>
        /// <param name="contactID"></param>
        /// <returns>response in JsonResult</returns>
        [HttpGet]
        public async Task<Response<PatientProfileViewModel>> GetPatientProfile(long contactID)
        {
            var model = await _patientProfileRepository.GetPatientProfile(contactID);
            return model;
        }


        #endregion
    }
}
