using System.Threading.Tasks;
using System.Web.Http;
using Axis.Model.Common;
using Axis.Plugins.Registration.Models;
using Axis.Plugins.Registration.Repository;
using Axis.PresentationEngine.Helpers.Controllers;

namespace Axis.Plugins.Registration.ApiControllers
{
    public class ConsentController : BaseApiController
    {
        #region Private Variable

        /// <summary>
        ///  Private variable for consent repository
        /// </summary>
        private readonly IConsentRepository consentRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="consentRepository"></param>
        public ConsentController(IConsentRepository consentRepository)
        {
            this.consentRepository = consentRepository;
        }

        #endregion

        #region Json Results

        /// <summary>
        /// Saves the signature to the database
        /// </summary>
        /// <param name="consentModel"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<ConsentViewModel> AddConsentSignature(ConsentViewModel consentModel)
        {
            return consentRepository.AddConsentSignature(consentModel);
        }

        /// <summary>
        /// Retrieves the signature from the database using the provided contactId
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<ConsentViewModel>> GetConsentSignature(long contactId)
        {
            var result = await consentRepository.GetConsentSignatureAsync(contactId);
            return result;
        }

        #endregion
    }
}
