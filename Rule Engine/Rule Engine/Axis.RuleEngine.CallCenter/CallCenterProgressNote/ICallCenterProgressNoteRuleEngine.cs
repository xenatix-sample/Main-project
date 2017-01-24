using Axis.Model;
using Axis.Model.Common;

namespace Axis.RuleEngine.CallCenter
{
    public interface ICallCenterProgressNoteRuleEngine
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
        /// <param name="callCenterProgressNoteModel">The call center progress note.</param>
        /// <returns></returns>
        Response<CallCenterProgressNoteModel> AddCallCenterProgressNote(CallCenterProgressNoteModel callCenterProgressNoteModel);

        /// <summary>
        /// Updates the call center progress note.
        /// </summary>
        /// <param name="callCenterProgressNoteModel">The call center progress note.</param>
        /// <returns></returns>
        Response<CallCenterProgressNoteModel> UpdateCallCenterProgressNote(CallCenterProgressNoteModel callCenterProgressNoteModel);
    }
}
