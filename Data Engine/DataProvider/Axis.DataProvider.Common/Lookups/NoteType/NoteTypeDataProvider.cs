using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    /// <summary>
    ///
    /// </summary>
    public class NoteTypeDataProvider : INoteTypeDataProvider
    {
        #region initializations

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="NoteTypeProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public NoteTypeDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        /// <summary>
        /// Get lookup data for NoteType
        /// </summary>
        /// <returns></returns>
        public Response<NoteTypeModel> GetNoteType()
        {
            var repository = _unitOfWork.GetRepository<NoteTypeModel>(SchemaName.Clinical);
            return repository.ExecuteStoredProc("usp_GetNoteType");
        }
        
        /// <summary>
        /// Get lookup data for NoteType
        /// </summary>
        /// <returns></returns>
        public Response<ReferenceNoteTypeModel> GetNoteTypes()
        {
            var repository = _unitOfWork.GetRepository<ReferenceNoteTypeModel>(SchemaName.Reference);
            return repository.ExecuteStoredProc("usp_GetNoteTypes");
        }

        /// <summary>
        /// Gets the type of the progress note.
        /// </summary>
        /// <returns></returns>
        public Response<PreferredNoteTypeModel> GetProgressNoteType()
        {
            var repository = _unitOfWork.GetRepository<PreferredNoteTypeModel>(SchemaName.Reference);
            return repository.ExecuteStoredProc("usp_GetProgressNoteType");
        }
    }
}
