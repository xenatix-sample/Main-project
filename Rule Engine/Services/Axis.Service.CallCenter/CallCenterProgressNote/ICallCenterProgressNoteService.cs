using Axis.Model;
using Axis.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Service.CallCenter
{
    public interface ICallCenterProgressNoteService
    {
        /// <summary>
        /// Gets the call center progress note.
        /// </summary>
        /// <param name="callCenterHeaderID">The call center header identifier.</param>
        /// <returns></returns>
        Response<CallCenterProgressNoteModel> GetCallCenterProgressNote(long callCenterHeaderID);


        /// <summary>
        /// Adds the call center progress note.
        /// </summary>
        /// <param name="callCenterProgressNoteModel">The call center progress note model.</param>
        /// <returns></returns>
        Response<CallCenterProgressNoteModel> AddCallCenterProgressNote(CallCenterProgressNoteModel callCenterProgressNoteModel);



        /// <summary>
        /// Updates the call center progress note.
        /// </summary>
        /// <param name="callCenterProgressNoteModel">The call center progress note model.</param>
        /// <returns></returns>
        Response<CallCenterProgressNoteModel> UpdateCallCenterProgressNote(CallCenterProgressNoteModel callCenterProgressNoteModel);
    }
}
