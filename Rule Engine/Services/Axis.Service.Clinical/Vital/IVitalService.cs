using System;
using Axis.Model.Clinical;
using Axis.Model.Common;

namespace Axis.Service.Clinical.Vital
{
    public interface IVitalService
    {
        /// <summary>
        /// Gets the contact Vitals.
        /// </summary>
        /// <param name="ContactId">The contact identifier.</param>
        /// <returns></returns>
        Response<VitalModel> GetContactVitals(long ContactId);

        /// <summary>
        /// Adds the contact Vital.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns></returns>
        Response<VitalModel> AddVital(VitalModel vital);

        /// <summary>
        /// Updates the contact Vital.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns></returns>
        Response<VitalModel> UpdateVital(VitalModel vital);

        /// <summary>
        /// Deletes the contact Vital.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        Response<VitalModel> DeleteVital(long id, DateTime modifiedOn);
    }
}
