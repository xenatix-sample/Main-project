using System;
using Axis.PresentationEngine.Helpers.Controllers;
using System.Threading.Tasks;
using System.Web.Http;
using Axis.Model.Common;

namespace Axis.Plugins.ECI.ApiControllers
{
    public class ECIAdditionalDemographicController : BaseApiController
    {
          /// <summary>
        /// The additional demographic repository
        /// </summary>
        private readonly IECIAdditionalDemographicRepository additionalDemographicRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdditionalDemographicController"/> class.
        /// </summary>
        /// <param name="additionalDemographicRepository">The additional demographic repository.</param>
        public ECIAdditionalDemographicController(IECIAdditionalDemographicRepository additionalDemographicRepository)
        {
            this.additionalDemographicRepository = additionalDemographicRepository;
        }

        /// <summary>
        /// Gets the additional demographic.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<ECIAdditionalDemographicsViewModel>> GetAdditionalDemographic(long contactId)
        {
            var result = await additionalDemographicRepository.GetAdditionalDemographicAsync(contactId);
            return result;
        }

        /// <summary>
        /// Adds the additional demographic.
        /// </summary>
        /// <param name="additional">The additional.</param>
        /// <returns></returns>
        [HttpPost]
        public Response<ECIAdditionalDemographicsViewModel> AddAdditionalDemographic(ECIAdditionalDemographicsViewModel additional)
        {
            return additionalDemographicRepository.AddAdditionalDemographic(additional);
        }

        /// <summary>
        /// Updates the additional demographic.
        /// </summary>
        /// <param name="additional">The additional.</param>
        /// <returns></returns>
        [HttpPost]
        public Response<ECIAdditionalDemographicsViewModel> UpdateAdditionalDemographic(ECIAdditionalDemographicsViewModel additional)
        {
            return additionalDemographicRepository.UpdateAdditionalDemographic(additional);
        }

        /// <summary>
        /// Deletes the additional demographic.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        public string DeleteAdditionalDemographic(long id, DateTime modifiedOn)
        {
            modifiedOn = modifiedOn.ToUniversalTime();
            additionalDemographicRepository.DeleteAdditionalDemographic(id, modifiedOn);
            return string.Empty;
        }
    }
}
