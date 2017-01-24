using System;
using System.Threading.Tasks;
using System.Web.Http;
using Axis.Model.Common;
using Axis.Plugins.Registration.Model;
using Axis.Plugins.Registration.Repository;
using Axis.PresentationEngine.Helpers.Controllers;
using Axis.Plugins.Registration.Repository.Admission;
using Axis.Plugins.Registration.Models;

namespace Axis.Plugins.Registration.ApiControllers
{
    /// <summary>
    /// Admission Controller api
    /// </summary>
    public class AdmissionController : BaseApiController
    {
        /// <summary>
        /// The Admission repository
        /// </summary>
        private readonly IAdmissionRepository admissionRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdmissionController"/> class.
        /// </summary>
        /// <param name="admissionRepository">The admission repository.</param>
        public AdmissionController(IAdmissionRepository admissionRepository)
        {
            this.admissionRepository = admissionRepository;
        }

        /// <summary>
        /// Gets the Admission
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<AdmissionViewModal> GetAdmission(long contactId)
        {
            var result =  admissionRepository.GetAdmission(contactId);
            return result;
        }

        /// <summary>
        /// Adds the Admission
        /// </summary>       
        /// <returns></returns>
        [HttpPost]
        public Response<AdmissionViewModal> AddAdmission(AdmissionViewModal additional)
        {
            var response = admissionRepository.AddAdmission(additional);
            return response;
        }

        /// <summary>
        /// Updates the Admission
        /// </summary>        
        /// <returns></returns>
        [HttpPut]
        public Response<AdmissionViewModal> UpdateAdmission(AdmissionViewModal additional)
        {
            var response = admissionRepository.UpdateAdmission(additional);
            return response;
        }
       
    }
}