using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Clinical;
using Axis.Model.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Axis.DataProvider.Clinical
{
    public class NoteDataProvider : INoteDataProvider
    {
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="NoteDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public NoteDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Get list of notes for contact
        /// </summary>
        /// <param name="contactID">The contact Id.</param>
        /// <returns></returns>
        public Response<NoteModel> GetNotes(long contactID)
        {
            var spParameters = new List<SqlParameter> { new SqlParameter("ContactID", contactID) };
            var repository = _unitOfWork.GetRepository<NoteModel>(SchemaName.Clinical);
            return repository.ExecuteStoredProc("usp_GetNotesByContact", spParameters);
        }

        /// <summary>
        /// Add note for contact
        /// </summary>
        /// <param name="note">The note</param>
        /// <returns></returns>
        public Response<NoteModel> AddNote(NoteModel note)
        {
            var spParameters = BuildSpParams(note);
            var repository = _unitOfWork.GetRepository<NoteModel>(SchemaName.Clinical);
            return repository.ExecuteNQStoredProc("usp_AddNote", spParameters, idResult: true);
        }

        /// <summary>
        /// Update note
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        public Response<NoteModel> UpdateNote(NoteModel note)
        {
            var spParameters = BuildSpParams(note);
            var repository = _unitOfWork.GetRepository<NoteModel>(SchemaName.Clinical);
            return repository.ExecuteNQStoredProc("usp_UpdateNote", spParameters);
        }

        /// <summary>
        /// Update note for NoteId
        /// </summary>
        /// <param name="note">The note.</param>
        /// <returns></returns>
        public Response<NoteDetailsModel> UpdateNoteDetails(NoteDetailsModel note)
        {
            var spParameters = new List<SqlParameter> { 
                                                        new SqlParameter("NoteID", note.NoteID),
                                                        new SqlParameter("Notes", note.Notes),
                                                        new SqlParameter("ModifiedOn", note.ModifiedOn ?? DateTime.Now)
                                                      };
            var repository = _unitOfWork.GetRepository<NoteDetailsModel>(SchemaName.Clinical);
            return repository.ExecuteNQStoredProc("usp_UpdateNoteDetails", spParameters);
        }

        /// <summary>
        /// Deactivate note for contact
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<NoteModel> DeleteNote(long Id, DateTime modifiedOn)
        {
            List<SqlParameter> procsParameters = new List<SqlParameter> { new SqlParameter("NoteId", Id), new SqlParameter("ModifiedOn", modifiedOn) };
            var noteRepository = _unitOfWork.GetRepository<NoteModel>(SchemaName.Clinical);
            return noteRepository.ExecuteNQStoredProc("usp_DeleteNote", procsParameters);
        }


        private List<SqlParameter> BuildSpParams(NoteModel note)
        {
            var spParameters = new List<SqlParameter>();
            if (note.NoteID > 0) // Only in case of Update
                spParameters.Add(new SqlParameter("NoteID", note.NoteID));  
          
            spParameters.AddRange(new List<SqlParameter> {
                new SqlParameter("ContactID", note.ContactID),
                new SqlParameter("NoteType", note.NoteTypeID),
                new SqlParameter("TakenBy", note.TakenBy),
                new SqlParameter("TakenTime", note.TakenTime),
                new SqlParameter("Notes",String.IsNullOrEmpty(note.Notes) ? " " : note.Notes),
                new SqlParameter("EncounterID", (object) note.EncounterID ?? DBNull.Value),
                new SqlParameter("ModifiedOn", note.ModifiedOn ?? DateTime.Now)
            });

            return spParameters;
        }
    }
}
