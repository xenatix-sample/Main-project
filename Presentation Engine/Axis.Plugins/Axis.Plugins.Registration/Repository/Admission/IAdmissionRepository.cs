using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Plugins.Registration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Plugins.Registration.Repository.Admission
{
    /// <summary>
    /// Admission repository interface
    /// </summary>
    public interface IAdmissionRepository
    {
        /// <summary>
        /// Gets the Admission.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        Response<AdmissionViewModal> GetAdmission(long contactId);

        /// <summary>
        /// Add Admission.
        /// </summary>
        /// <param name="admission">The Admission.</param>
        /// <returns></returns>
        Response<AdmissionViewModal> AddAdmission(AdmissionViewModal admission);

        /// <summary>
        /// Updates admission.
        /// </summary>
        /// <param name="admission">The admission.</param>
        /// <returns></returns>
        Response<AdmissionViewModal> UpdateAdmission(AdmissionViewModal admission);
    }
}
