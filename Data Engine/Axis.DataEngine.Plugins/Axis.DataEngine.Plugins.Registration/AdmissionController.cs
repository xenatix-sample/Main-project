using System;
using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Registration;
using Axis.Model.Common;
using Axis.Model.Registration;
using System.Web.Http;

namespace Axis.DataEngine.Plugins.Registration
{
    /// <summary>
    /// Admission Controller
    /// </summary>
    public class AdmissionController : BaseApiController
    {
        /// <summary>
        /// The admission data provider
        /// </summary>
        private readonly IAdmissionDataProvider _admissionDataProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdmissionController" /> class.
        /// </summary>
        /// <param name="admissionController">The admission data provider.</param>
        public AdmissionController(IAdmissionDataProvider admissionDataProvider)
        {
            _admissionDataProvider = admissionDataProvider;
        }

        /// <summary>
        /// Gets the admissions.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
       [HttpGet]
        public IHttpActionResult GetAdmission(long contactId)
        {
            return new HttpResult<Response<AdmissionModal>>(_admissionDataProvider.GetAdmission(contactId), Request);
        }

        /// <summary>
        /// Adds the admission.
        /// </summary>
        /// <param name="admission">The admission.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddAdmission(AdmissionModal admission)
        {
            return new HttpResult<Response<AdmissionModal>>(_admissionDataProvider.AddAdmission(admission), Request);
        }

        /// <summary>
        /// Updates the admission demographic.
        /// </summary>
        /// <param name="admission">The admission.</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateAdmission(AdmissionModal admission)
        {
            return new HttpResult<Response<AdmissionModal>>(_admissionDataProvider.UpdateAdmission(admission), Request);
        }        
    }
}