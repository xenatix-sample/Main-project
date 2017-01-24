using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using System.Collections.Generic;

namespace Axis.DataProvider.Common
{
    public class NoteStatusDataProvider : INoteStatusDataProvider
    {
        #region initializations

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="NoteStatusDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public NoteStatusDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        /// <summary>
        /// Get lookup data for NoteStatus
        /// </summary>
        /// <returns></returns>
        public Response<NoteStatusModel> GetNoteStatus()
        {
            var repository = _unitOfWork.GetRepository<NoteStatusModel>(SchemaName.Reference);
            return repository.ExecuteStoredProc("usp_GetDocumentStatus");
        }
    }
}
