using Axis.Model.Common;
using Axis.Model.Consents;
using Axis.PresentationEngine.Areas.Consents.Models;
using Axis.PresentationEngine.Areas.Consents.Repository;
using Axis.PresentationEngine.Helpers.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Axis.PresentationEngine.Areas.Consents.ApiControllers
{
    public class ConsentsController : BaseApiController
    {
         /// <summary>
        /// The consents repository
        /// </summary>
        private readonly IConsentsRepository _consentsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssessmentController" /> class.
        /// </summary>
        /// <param name="consentsRepository">The consents repository.</param>
        public ConsentsController(IConsentsRepository consentsRepository)
        {
            this._consentsRepository = consentsRepository;
        }

        /// <summary>
        /// Get assessment.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<ConsentsViewModel> GetConsents(long contactID)
        {
            return _consentsRepository.GetConsents(contactID);
        }
        
        /// <summary>
        /// Adds the consent.
        /// </summary>
        /// <param name="consent">The consent.</param>
        /// <returns></returns>
        [HttpPost]
        public Response<ConsentsViewModel> AddConsent(ConsentsViewModel consent)
        {
            return _consentsRepository.AddConsent(consent);
        }

        /// <summary>
        /// Updates the consents.
        /// </summary>
        /// <param name="consent">The consent.</param>
        /// <returns></returns>
        [HttpPut]
        public Response<ConsentsViewModel> UpdateConsent(ConsentsViewModel consent)
        {
            return _consentsRepository.UpdateConsent(consent);
        }
    }
}