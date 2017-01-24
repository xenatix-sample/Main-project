using System;
using Axis.Model.Clinical;
using Axis.Model.Common;

namespace Axis.DataProvider.Clinical.Vital
{
    public interface IVitalDataProvider
    {
        /// <summary>
        /// Gets the Vitals.
        /// </summary>
        /// <param name="ContactId">The contact identifier.</param>
        /// <returns></returns>
        Response<VitalModel> GetContactVitals(long ContactId);

        Response<BPMethodModel> GetBPMethods();

        /// <summary>
        /// Adds the Vital.
        /// </summary>
        /// <param name="vital">The vital model.</param>
        /// <returns></returns>
        Response<VitalModel> AddVital(VitalModel vital);
        /// <summary>
        /// Updates the contact Vital.
        /// </summary>
        /// <param name="contact">The vital model.</param>
        /// <returns></returns>
        Response<VitalModel> UpdateVital(VitalModel vital);

        /// <summary>
        /// Deletes the Vital.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        Response<VitalModel> DeleteVital(long id, DateTime modifiedOn);
    }
}
