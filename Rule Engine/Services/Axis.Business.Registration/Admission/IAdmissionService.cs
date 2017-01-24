using Axis.Model.Common;
using Axis.Model.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Service.Registration.Admission
{
    /// <summary>
    /// Admission data provider interface
    /// </summary>
    public interface IAdmissionService
    {
        /// <summary>
        /// Gets the Admission.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        Response<AdmissionModal> GetAdmission(long contactId);

        /// <summary>
        /// Add Admission.
        /// </summary>
        /// <param name="admission">The Admission.</param>
        /// <returns></returns>
        Response<AdmissionModal> AddAdmission(AdmissionModal admission);

        /// <summary>
        /// Updates admission.
        /// </summary>
        /// <param name="admission">The admission.</param>
        /// <returns></returns>
        Response<AdmissionModal> UpdateAdmission(AdmissionModal admission);
    }
}
