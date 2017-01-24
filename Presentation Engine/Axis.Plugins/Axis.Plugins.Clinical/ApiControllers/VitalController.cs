using System;
using System.Threading.Tasks;
using System.Web.Http;
using Axis.Model.Common;
using Axis.Plugins.Clinical.Models.Vital;
using Axis.Plugins.Clinical.Repository.Vital;
using Axis.PresentationEngine.Helpers.Controllers;

namespace Axis.Plugins.Clinical.ApiControllers
{
    public class VitalController : BaseApiController
    {
        /// <summary>
        /// The contact Vital repository
        /// </summary>
        private readonly IVitalRepository _vitalRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="VitalController" /> class.
        /// </summary>
        /// <param name="vitalRepository">The contact Vital repository.</param>
        public VitalController(IVitalRepository vitalRepository)
        {
            this._vitalRepository = vitalRepository;
        }

        /// <summary>
        /// Gets the contact Vitals.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<VitalViewModel>> GetContactVitals(long contactId)
        {
            var response = await _vitalRepository.GetContactVitalsAsync(contactId);
            return response;
        }

        /// <summary>
        /// Adds the Vital.
        /// </summary>
        /// <param name="vital">The Vital.</param>
        /// <returns></returns>
        [HttpPost]
        public Response<VitalViewModel> AddVital(VitalViewModel vital)
        {
            vital.TakenTime = vital.TakenTime.ToUniversalTime();
            return _vitalRepository.AddVital(vital);
        }

        /// <summary>
        /// Updates the Vital.
        /// </summary>
        /// <param name="vital">The Vital.</param>
        /// <returns></returns>
        [HttpPut]
        public Response<VitalViewModel> UpdateVital(VitalViewModel vital)
        {
            vital.TakenTime = vital.TakenTime.ToUniversalTime();
            return _vitalRepository.UpdateVital(vital);
        }

        /// <summary>
        /// Deletes the Vital.
        /// </summary>
        /// <param name="vitalId"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        [HttpDelete]
        public Response<VitalViewModel> DeleteVital(long vitalId, DateTime modifiedOn)
        {
            modifiedOn = modifiedOn.ToUniversalTime();
            return _vitalRepository.DeleteVital(vitalId, modifiedOn);
        }
    }
}
