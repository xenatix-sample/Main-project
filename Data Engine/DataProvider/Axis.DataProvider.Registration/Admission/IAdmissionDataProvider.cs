using System;
using Axis.Model.Common;
using Axis.Model.Registration;

namespace Axis.DataProvider.Registration
{
    /// <summary>
    /// Admission data provider interface
    /// </summary>
    public interface IAdmissionDataProvider
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