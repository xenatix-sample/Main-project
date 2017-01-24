using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface INoteStatusDataProvider
    {
        /// <summary>
        /// Gets the NoteStatus.
        /// </summary>
        /// <returns></returns>
        Response<NoteStatusModel> GetNoteStatus();
    }
}
