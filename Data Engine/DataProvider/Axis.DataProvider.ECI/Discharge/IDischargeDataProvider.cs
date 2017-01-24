
using System;
using Axis.Model.Common;
using Axis.Model.ECI;
namespace Axis.DataProvider.ECI
{
    public interface IDischargeDataProvider
    {
        /// <summary>
        /// Gets the dischages.
        /// </summary>
        /// <param name="progressNoteID">The progress note identifier.</param>
        /// <returns></returns>
        Response<DischargeModel> GetDischages(long contactID, int noteTypeID);

        /// <summary>
        /// Gets the dischage.
        /// </summary>
        /// <param name="noteHeaderId">The note header identifier.</param>
        /// <returns></returns>
        Response<DischargeModel> GetDischage(long dischargeID);

        /// <summary>
        /// Adds the dischage.
        /// </summary>
        /// <param name="discharge">The discharge.</param>
        /// <returns></returns>
        Response<DischargeModel> AddDischage(DischargeModel discharge);

        /// <summary>
        /// Updates the dischage.
        /// </summary>
        /// <param name="discharge">The discharge.</param>
        /// <returns></returns>
        Response<DischargeModel> UpdateDischage(DischargeModel discharge);

        /// <summary>
        /// Deletes the dischage.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        Response<DischargeModel> DeleteDischage(long Id, DateTime modifiedOn);
    }
}
