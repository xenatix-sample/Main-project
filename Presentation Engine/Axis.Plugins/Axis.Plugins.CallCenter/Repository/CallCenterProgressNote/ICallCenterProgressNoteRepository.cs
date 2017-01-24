using Axis.Model;
using Axis.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Plugins.CallCenter.Repository
{
    public interface ICallCenterProgressNoteRepository
    {

        /// <summary>
        /// Gets the call center progress note .
        /// </summary>
        /// <param name="callCenterHeaderID">The call center header identifier.</param>
        /// <returns></returns>
        Response<CallCenterProgressNoteViewModel> GetCallCenterProgressNote(long callCenterHeaderID);

        /// <summary>
        /// Adds the call center progress note .
        /// </summary>
        /// <param name="callCenterProgressNoteModel">The callCenterProgressNoteModel.</param>
        /// <returns></returns>
        Response<CallCenterProgressNoteViewModel> AddCallCenterProgressNote(CallCenterProgressNoteViewModel callCenterProgressNoteModel);

        /// <summary>
        /// Updates the call center progress note .
        /// </summary>
        /// <param name="callCenterProgressNoteModel">The callCenterProgressNoteModel.</param>
        /// <returns></returns>
        Response<CallCenterProgressNoteViewModel> UpdateCallCenterProgressNote(CallCenterProgressNoteViewModel callCenterProgressNoteModel);
    }
}
