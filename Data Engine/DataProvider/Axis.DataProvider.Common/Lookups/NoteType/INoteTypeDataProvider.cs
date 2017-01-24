using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface INoteTypeDataProvider
    {
        /// <summary>
        /// Gets the NoteType.
        /// </summary>
        /// <returns></returns>
        Response<NoteTypeModel> GetNoteType();

        /// <summary>
        /// Gets the NoteType.
        /// </summary>
        /// <returns></returns>
        Response<ReferenceNoteTypeModel> GetNoteTypes();

        /// <summary>
        /// Gets the type of the progress note.
        /// </summary>
        /// <returns></returns>
        Response<PreferredNoteTypeModel> GetProgressNoteType();
    }
}
