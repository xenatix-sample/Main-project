using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using Axis.Model.ECI;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Axis.DataProvider.ECI
{
    public class ProgressNoteAssessmentDataProvider : IProgressNoteAssessmentDataProvider
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
        public ProgressNoteAssessmentDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the dischages.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <param name="noteTypeID">The note type identifier.</param>
        /// <returns></returns>
        public Response<ProgressNoteAssessmentModel> GetProgressNotes(long contactID, int noteTypeID)
        {
            var noteRepository = _unitOfWork.GetRepository<ProgressNoteAssessmentModel>(SchemaName.ECI);
            List<SqlParameter> procParams = new List<SqlParameter> { new SqlParameter("ContactID", contactID),
                                                                     new SqlParameter("NoteTypeID", noteTypeID)
                                                                    };
            return noteRepository.ExecuteStoredProc("usp_GetNoteAssessments", procParams);
        }

        /// <summary>
        /// Gets the progress note.
        /// </summary>
        /// <param name="progressNoteAssessmentID">The progress note assessment identifier.</param>
        /// <returns></returns>
        public Response<ProgressNoteAssessmentModel> GetProgressNote(long progressNoteAssessmentID)
        {
            var noteRepository = _unitOfWork.GetRepository<ProgressNoteAssessmentModel>(SchemaName.ECI);
            List<SqlParameter> procParams = new List<SqlParameter> { new SqlParameter("ScheduleNoteAssessmentID", progressNoteAssessmentID) };
            return noteRepository.ExecuteStoredProc("usp_GetNoteAssessment", procParams);
        }

        /// <summary>
        /// Adds the dischage.
        /// </summary>
        /// <param name="noteAssessment">The note assessment.</param>
        /// <returns></returns>
        public Response<ProgressNoteAssessmentModel> AddNoteAssessment(ProgressNoteAssessmentModel noteAssessment)
        {
            var noteRepository = _unitOfWork.GetRepository<ProgressNoteAssessmentModel>(SchemaName.ECI);
            var procParams = BuildSpParams(noteAssessment);
            return _unitOfWork.EnsureInTransaction(noteRepository.ExecuteNQStoredProc, "usp_AddNoteAssessment", procParams, idResult: true,
                            forceRollback: noteAssessment.ForceRollback.GetValueOrDefault(false));      
        }

        /// <summary>
        /// Updates the dischage.
        /// </summary>
        /// <param name="noteAssessment">The note assessment.</param>
        /// <returns></returns>
        public Response<ProgressNoteAssessmentModel> UpdateNoteAssessment(ProgressNoteAssessmentModel noteAssessment)
        {
            var noteRepository = _unitOfWork.GetRepository<ProgressNoteAssessmentModel>(SchemaName.ECI);
            var procParams = BuildSpParams(noteAssessment);
            return _unitOfWork.EnsureInTransaction(noteRepository.ExecuteNQStoredProc, "usp_UpdateNoteAssessment", procParams,
                            forceRollback: noteAssessment.ForceRollback.GetValueOrDefault(false));
        }

        /// <summary>
        /// Deletes the dischage.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<ProgressNoteAssessmentModel> DeleteNoteAssessment(long Id, DateTime modifiedOn)
        {
            var noteRepository = _unitOfWork.GetRepository<ProgressNoteAssessmentModel>(SchemaName.ECI);
            var param = new List<SqlParameter> { new SqlParameter("ScheduleNoteAssessmentID", Id), new SqlParameter("ModifiedOn", modifiedOn) };
            return noteRepository.ExecuteNQStoredProc("usp_DeleteNoteAssessment", param);
        }

        #endregion

        #region Private Methods

        private List<SqlParameter> BuildSpParams(ProgressNoteAssessmentModel noteAssessment)
        {
            var spParameters = new List<SqlParameter>();

            if (noteAssessment.ScheduleNoteAssessmentID > 0) // Only in case of Update
                spParameters.Add(new SqlParameter("ScheduleNoteAssessmentID", noteAssessment.ScheduleNoteAssessmentID));

            spParameters.AddRange(new List<SqlParameter> {
                new SqlParameter("ProgressNoteID", (object)noteAssessment.ProgressNoteID ?? DBNull.Value),
                new SqlParameter("NoteAssessmentDate", (object)noteAssessment.NoteAssessmentDate ?? DBNull.Value),
                new SqlParameter("NoteAssessmentTime", (object)noteAssessment.NoteAssessmentTime ?? DBNull.Value),
                new SqlParameter("LocationID", (object)noteAssessment.LocationID ?? DBNull.Value),
                new SqlParameter("Location", (object)noteAssessment.Location ?? DBNull.Value),
                new SqlParameter("ProviderID", (object)noteAssessment.ProviderID ?? DBNull.Value),
                new SqlParameter("MembersInvited", (object)noteAssessment.MembersInvited ?? DBNull.Value),
                new SqlParameter("ModifiedOn", noteAssessment.ModifiedOn ?? DateTime.Now)
            });
            return spParameters;
        }

        #endregion
    }
}
