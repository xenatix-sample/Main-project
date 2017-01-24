using System.Threading.Tasks;
using System.Web.Http;
using Axis.Model.Common;
using Axis.Plugins.ECI.Models.ECIDemographics;
using Axis.Plugins.ECI.Repository.ECIDemographic;
using Axis.PresentationEngine.Helpers.Controllers;

namespace Axis.Plugins.ECI.ApiControllers
{
    public class ECIDemographicController : BaseApiController
    {
        #region Class Variables

        private readonly IECIDemographicRepository _eciDemographicRepository;

        #endregion

        #region Constructors

        public ECIDemographicController(IECIDemographicRepository eciDemographicRepository)
        {
            _eciDemographicRepository = eciDemographicRepository;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// GetContactDemographics to get data on basis of contactID
        /// </summary>
        /// <param name="contactID"></param>
        /// <returns>response in JsonResult</returns>
        [HttpGet]
        public async Task<Response<ECIContactDemographicsViewModel>> GetContactDemographics(long contactID)
        {
            var response = await _eciDemographicRepository.GetContactDemographics(contactID);
            return response;
        }

        /// <summary>
        /// AddContactDemographics method to save the contact data 
        /// </summary>
        /// <param name="contact"></param>
        /// <returns>response in JsonResult</returns>
        [HttpPost]
        public Response<ECIContactDemographicsViewModel> AddContactDemographics(ECIContactDemographicsViewModel contact)
        {
            return _eciDemographicRepository.AddContactDemographics(contact);
        }

        /// <summary>
        /// UpdateContactDemographics method to update the contact data
        /// </summary>
        /// <param name="contact"></param>
        /// <returns>JsonResult</returns>
        [HttpPut]
        public Response<ECIContactDemographicsViewModel> UpdateContactDemographics(ECIContactDemographicsViewModel contact)
        {
            return _eciDemographicRepository.UpdateContactDemographics(contact);
        }

        #endregion
    }
}
