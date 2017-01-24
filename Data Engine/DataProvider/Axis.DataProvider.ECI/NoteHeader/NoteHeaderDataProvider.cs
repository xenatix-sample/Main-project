using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using Axis.Model.ECI;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Axis.DataProvider.ECI
{
    public class NoteHeaderDataProvider : INoteHeaderDataProvider
    {
        #region Class Variables

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unitOfWork"></param>
        public NoteHeaderDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the note headers.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <param name="moduleID"></param>
        /// <returns></returns>
        public Response<NoteHeaderModel> GetNoteHeaders(long contactID, long moduleID, int noteTypeID)
        {
            var noteRepository = _unitOfWork.GetRepository<NoteHeaderModel>(SchemaName.Registration);
            List<SqlParameter> procParams = new List<SqlParameter> { new SqlParameter("ContactID", contactID),
                                            new SqlParameter("ModuleID", moduleID),
                                            new SqlParameter("NoteTypeID", noteTypeID) };
            return noteRepository.ExecuteStoredProc("usp_GetNoteHeaders", procParams);
        }

        /// <summary>
        /// Gets the note header.
        /// </summary>
        /// <param name="noteHeaderId"></param>
        /// <returns></returns>
        public Response<NoteHeaderModel> GetNoteHeader(long noteHeaderId)
        {
            var noteRepository = _unitOfWork.GetRepository<NoteHeaderModel>(SchemaName.Registration);
            List<SqlParameter> procParams = new List<SqlParameter> { new SqlParameter("NoteHeaderID", noteHeaderId) };
            return noteRepository.ExecuteStoredProc("usp_GetNoteHeader", procParams);
        }

        /// <summary>
        /// Adds the note header.
        /// </summary>
        /// <param name="noteHeader"></param>
        /// <returns></returns>
        public Response<NoteHeaderModel> AddNoteHeader(NoteHeaderModel noteHeader)
        {
            var noteRepository = _unitOfWork.GetRepository<NoteHeaderModel>(SchemaName.Registration);
            var procParams = BuildSpParams(noteHeader);
            return _unitOfWork.EnsureInTransaction(noteRepository.ExecuteNQStoredProc, "usp_AddNoteHeader", procParams, idResult: true,
                            forceRollback: noteHeader.ForceRollback.GetValueOrDefault(false));      
        }

        /// <summary>
        /// Updates the note header.
        /// </summary>
        /// <param name="noteHeader"></param>
        /// <returns></returns>
        public Response<NoteHeaderModel> UpdateNoteHeader(NoteHeaderModel noteHeader)
        {
            var noteRepository = _unitOfWork.GetRepository<NoteHeaderModel>(SchemaName.Registration);
            var procParams = BuildSpParams(noteHeader);
            return _unitOfWork.EnsureInTransaction(noteRepository.ExecuteNQStoredProc, "usp_UpdateNoteHeader", procParams,
                            forceRollback: noteHeader.ForceRollback.GetValueOrDefault(false));
        }

        /// <summary>
        /// Deletes the note header.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<NoteHeaderModel> DeleteNoteHeader(long Id, DateTime modifiedOn)
        {
            var noteRepository = _unitOfWork.GetRepository<NoteHeaderModel>(SchemaName.Registration);
            var param = new List<SqlParameter> { new SqlParameter("NoteHeaderID", Id), new SqlParameter("ModifiedOn", modifiedOn) };
            return noteRepository.ExecuteNQStoredProc("usp_DeleteNoteHeader", param);
        }

        #endregion

        #region Private Methods

        private List<SqlParameter> BuildSpParams(NoteHeaderModel noteHeader)
        {
            var spParameters = new List<SqlParameter>();

            if (noteHeader.NoteHeaderID > 0) // Only in case of Update
                spParameters.Add(new SqlParameter("NoteHeaderID", noteHeader.NoteHeaderID));

            spParameters.AddRange(new List<SqlParameter> {
                new SqlParameter("ContactID ", (object)noteHeader.ContactID ?? DBNull.Value),
                new SqlParameter("NoteTypeID", (object)noteHeader.NoteTypeID ?? DBNull.Value),
                new SqlParameter("TakenBy", (object)noteHeader.TakenBy ?? DBNull.Value),
                new SqlParameter("TakenTime", (object)noteHeader.TakenTime ?? DBNull.Value),
                new SqlParameter("ModifiedOn", noteHeader.ModifiedOn ?? DateTime.Now)
            });
            return spParameters;
        }

        #endregion
    }
}
