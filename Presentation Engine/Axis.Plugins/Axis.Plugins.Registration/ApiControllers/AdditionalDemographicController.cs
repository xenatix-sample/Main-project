using System;
using System.Threading.Tasks;
using System.Web.Http;
using Axis.Model.Common;
using Axis.Plugins.Registration.Model;
using Axis.Plugins.Registration.Repository;
using Axis.PresentationEngine.Helpers.Controllers;

namespace Axis.Plugins.Registration.ApiControllers
{
    /// <summary>
    ///
    /// </summary>
    public class AdditionalDemographicController : BaseApiController
    {
        /// <summary>
        /// The additional demographic repository
        /// </summary>
        private readonly IAdditionalDemographicRepository additionalDemographicRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdditionalDemographicController"/> class.
        /// </summary>
        /// <param name="additionalDemographicRepository">The additional demographic repository.</param>
        public AdditionalDemographicController(IAdditionalDemographicRepository additionalDemographicRepository)
        {
            this.additionalDemographicRepository = additionalDemographicRepository;
        }

        /// <summary>
        /// Gets the additional demographic.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<AdditionalDemographicsViewModel>> GetAdditionalDemographic(long contactId)
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
        public Response<AdditionalDemographicsViewModel> AddAdditionalDemographic(AdditionalDemographicsViewModel additional)
        {
            var response = additionalDemographicRepository.AddAdditionalDemographic(additional);
            return response;
        }

        /// <summary>
        /// Updates the additional demographic.
        /// </summary>
        /// <param name="additional">The additional.</param>
        /// <returns></returns>
        [HttpPost]
        public Response<AdditionalDemographicsViewModel> UpdateAdditionalDemographic(AdditionalDemographicsViewModel additional)
        {
            var response = additionalDemographicRepository.UpdateAdditionalDemographic(additional);
            return response;
        }

        /// <summary>
        /// Deletes the additional demographic.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        [HttpDelete]
        public void DeleteAdditionalDemographic(long id, DateTime modifiedOn)
        {
            modifiedOn = modifiedOn.ToUniversalTime();
            additionalDemographicRepository.DeleteAdditionalDemographic(id, modifiedOn);
        }
    }
}