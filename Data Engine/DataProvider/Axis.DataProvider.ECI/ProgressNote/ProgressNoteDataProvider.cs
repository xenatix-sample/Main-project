using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Logging;
using Axis.Model.Common;
using Axis.Model.ECI;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Axis.DataProvider.ECI
{
    public class ProgressNoteDataProvider : IProgressNoteDataProvider
    {
        #region Class Variables

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// The _note header data provider
        /// </summary>
        private readonly INoteHeaderDataProvider _noteHeaderDataProvider;

        /// <summary>
        /// The _discharge data provider
        /// </summary>
        private readonly IDischargeDataProvider _dischargeDataProvider;

        /// <summary>
        /// The _progress note assessment data provider
        /// </summary>
        private readonly IProgressNoteAssessmentDataProvider _progressNoteAssessmentDataProvider;

        /// <summary>
        /// Logging
        /// </summary>
        private readonly ILogger _logger = null;
        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unitOfWork"></param>
        public ProgressNoteDataProvider(IUnitOfWork unitOfWork, INoteHeaderDataProvider noteHeaderDataProvider, IDischargeDataProvider dischargeDataProvider, IProgressNoteAssessmentDataProvider progressNoteAssessmentDataProvider, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _noteHeaderDataProvider = noteHeaderDataProvider;
            _dischargeDataProvider = dischargeDataProvider;
            _progressNoteAssessmentDataProvider = progressNoteAssessmentDataProvider;
            _logger = logger;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the progress notes.
        /// </summary>
        /// <param name="noteTypeID">The note type identifier.</param>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<NoteHeaderModel> GetProgressNotes(int noteTypeID, long contactID)
        {
            var noteResponse = _noteHeaderDataProvider.GetNoteHeaders(contactID, 1, noteTypeID);        //TODO: There is no module available in db, hardcoding to 1

            var noteRepository = _unitOfWork.GetRepository<ProgressNoteModel>(SchemaName.ECI);
            List<SqlParameter> procParams = new List<SqlParameter> { new SqlParameter("NoteTypeID", noteTypeID),
                                                                     new SqlParameter("ContactID", contactID) };
            var progressNoteResponse = noteRepository.ExecuteStoredProc("usp_GetProgressNotes", procParams);

            if (progressNoteResponse.ResultCode != 0)
            {
                noteResponse.ResultCode = progressNoteResponse.ResultCode;
                noteResponse.ResultMessage = progressNoteResponse.ResultMessage;
                return noteResponse;
            }
            else
            {
                noteResponse.DataItems.ForEach(m =>
                {
                    m.ProgressNote = (progressNoteResponse.DataItems.Where(a => a.NoteHeaderID == m.NoteHeaderID).FirstOrDefault());
                });
            }

            //Get ProgressNoteAssessment details only if note type id = 2
            if (noteTypeID == 2)
            {
                var noteAssessmentResponse = _progressNoteAssessmentDataProvider.GetProgressNotes(contactID, noteTypeID);
                if (noteAssessmentResponse.ResultCode != 0)
                {
                    noteResponse.ResultCode = noteAssessmentResponse.ResultCode;
                    noteResponse.ResultMessage = noteAssessmentResponse.ResultMessage;
                    return noteResponse;
                }
                else
                {
                    noteResponse.DataItems.ForEach(m =>
                    {
                        m.ProgressNoteAssessment = (noteAssessmentResponse.DataItems.Where(a => a.ProgressNoteID == m.ProgressNote.ProgressNoteID).FirstOrDefault());
                    });
                }
            }

            var dischargeResponse = _dischargeDataProvider.GetDischages(contactID, noteTypeID);
            if (dischargeResponse.ResultCode != 0)
            {
                noteResponse.ResultCode = dischargeResponse.ResultCode;
                noteResponse.ResultMessage = dischargeResponse.ResultMessage;
                return noteResponse;
            }
            else
            {
                noteResponse.DataItems.ForEach(m =>
                {
                    m.Discharge = (dischargeResponse.DataItems.Where(a => a.ProgressNoteID == m.ProgressNote.ProgressNoteID).FirstOrDefault());
                });
            }

            return noteResponse;
        }

        /// <summary>
        /// Gets the progress note.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<NoteHeaderModel> GetProgressNote(long progressNoteId)
        {
            var noteRepository = _unitOfWork.GetRepository<NoteHeaderModel>(SchemaName.ECI);
            List<SqlParameter> procParams = new List<SqlParameter> { new SqlParameter("ProgressNoteID", progressNoteId) };
            return noteRepository.ExecuteStoredProc("usp_GetProgressNote", procParams);
        }

        /// <summary>
        /// Adds the progress note.
        /// </summary>
        /// <param name="progressNote">The progress note.</param>
        /// <returns></returns>
        public Response<NoteHeaderModel> AddProgressNote(NoteHeaderModel noteHeader)
        {
            var response = new Response<NoteHeaderModel>() { ResultCode = -1, ResultMessage = "Error while saving Note details." };
            using (var transactionScope = _unitOfWork.BeginTransactionScope())
            {
                try
                {
                    var noteResp = _noteHeaderDataProvider.AddNoteHeader(noteHeader);
                    response.ResultCode = noteResp.ResultCode;
                    response.ResultMessage = noteResp.ResultMessage;
                    if (noteResp.ResultCode != 0)
                    {
                        return response;
                    }
                    else
                    {
                        //Assigns generated NoteHeaderID to model to save in ProgressNote 
                        noteHeader.ProgressNote.NoteHeaderID = noteResp.ID;
                    }

                    var noteRepository = _unitOfWork.GetRepository<NoteHeaderModel>(SchemaName.ECI);
                    var procParams = BuildSpParams(noteHeader);
                    var resp = noteRepository.ExecuteNQStoredProc("usp_AddProgressNote", procParams, idResult: true);
                    response.ResultCode = resp.ResultCode;
                    response.ResultMessage = resp.ResultMessage;
                    if (resp.ResultCode != 0)
                    {
                        return response;
                    }
                    else
                    {
                        //Assigns generated ProgressNoteID to model to save in Discharge 
                        noteHeader.Discharge.ProgressNoteID = noteHeader.ProgressNoteAssessment.ProgressNoteID = resp.ID;
                    }

                    //Add ProgressNoteAssessment details only if note type id = 2
                    if (noteHeader.NoteTypeID == 2)
                    {
                        var noteAssessmentResp = _progressNoteAssessmentDataProvider.AddNoteAssessment(noteHeader.ProgressNoteAssessment);
                        response.ResultCode = noteAssessmentResp.ResultCode;
                        response.ResultMessage = noteAssessmentResp.ResultMessage;
                        if (noteResp.ResultCode != 0)
                        {
                            return response;
                        }
                        else
                        {
                            //Assigns generated NoteHeaderID to model to save in ProgressNote 
                            noteHeader.ProgressNote.NoteHeaderID = noteResp.ID;
                        }
                    }

                    //Saves Discharge details only if Discharge selected
                    if (noteHeader.ProgressNote.IsDischarged)
                    {
                        var dischargeResp = _dischargeDataProvider.AddDischage(noteHeader.Discharge);
                        response.ResultCode = dischargeResp.ResultCode;
                        response.ResultMessage = dischargeResp.ResultMessage;
                        if (dischargeResp.ResultCode != 0)
                        {
                            return response;
                        }
                        else
                        {
                            //Assigns generated ProgressNoteID to model to save in Discharge 
                            noteHeader.Discharge.DischargeID = resp.ID;
                        }
                    }
                    response.ID = noteResp.ID;
                    if (!noteHeader.ForceRollback.GetValueOrDefault(false))
                        _unitOfWork.TransactionScopeComplete(transactionScope);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    response.ResultCode = -1;
                    response.ResultMessage = "Error while saving Note details.";
                }
                return response;
            }
        }

        /// <summary>
        /// Updates the progress note.
        /// </summary>
        /// <param name="progressNote">The progress note.</param>
        /// <returns></returns>
        public Response<NoteHeaderModel> UpdateProgressNote(NoteHeaderModel noteHeader)
        {
            var response = new Response<NoteHeaderModel>() { ResultCode = -1, ResultMessage = "Error while Updating Note details." };
            using (var transactionScope = _unitOfWork.BeginTransactionScope())
            {
                try
                {
                    //Update NoteHeader data
                    var noteResp = _noteHeaderDataProvider.UpdateNoteHeader(noteHeader);
                    response.ResultCode = noteResp.ResultCode;
                    response.ResultMessage = noteResp.ResultMessage;
                    if (noteResp.ResultCode != 0)
                    {
                        return response;
                    }

                    //Update ProgressNote data
                    var noteRepository = _unitOfWork.GetRepository<NoteHeaderModel>(SchemaName.ECI);
                    var procParams = BuildSpParams(noteHeader);
                    var resp = noteRepository.ExecuteNQStoredProc("usp_UpdateProgressNote", procParams);
                    response.ResultCode = resp.ResultCode;
                    response.ResultMessage = resp.ResultMessage;
                    if (resp.ResultCode != 0)
                    {
                        return response;
                    }

                    //Update ProgressNoteAssessment details only if note type id = 2
                    if (noteHeader.NoteTypeID == 2)
                    {
                        var noteAssessmentResp = noteHeader.ProgressNoteAssessment.ScheduleNoteAssessmentID > 0 ? _progressNoteAssessmentDataProvider.UpdateNoteAssessment(noteHeader.ProgressNoteAssessment) :
                                                                                    _progressNoteAssessmentDataProvider.AddNoteAssessment(noteHeader.ProgressNoteAssessment);
                        response.ResultCode = noteAssessmentResp.ResultCode;
                        response.ResultMessage = noteAssessmentResp.ResultMessage;
                        if (noteResp.ResultCode != 0)
                        {
                            return response;
                        }
                        else
                        {
                            //Assigns generated NoteHeaderID to model to save in ProgressNote 
                            noteHeader.ProgressNote.NoteHeaderID = noteResp.ID;
                        }
                    }

                    //Save/Update Discharge details only if Discharge selected
                    if (noteHeader.ProgressNote.IsDischarged)
                    {
                        noteHeader.Discharge.ProgressNoteID = noteHeader.ProgressNote.ProgressNoteID;
                        var dischargeResp = noteHeader.Discharge.DischargeID > 0 ? _dischargeDataProvider.UpdateDischage(noteHeader.Discharge) :
                                                                                    _dischargeDataProvider.AddDischage(noteHeader.Discharge);
                        response.ResultCode = dischargeResp.ResultCode;
                        response.ResultMessage = dischargeResp.ResultMessage;
                        if (dischargeResp.ResultCode != 0)
                        {
                            return response;
                        }
                    }
                    if (!noteHeader.ForceRollback.GetValueOrDefault(false))
                        _unitOfWork.TransactionScopeComplete(transactionScope);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    response.ResultCode = -1;
                    response.ResultMessage = "Error while Updating Note details.";
                }
                return response;
            }
        }

        /// <summary>
        /// Deletes the progress note.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<NoteHeaderModel> DeleteProgressNote(long Id, DateTime modifiedOn)
        {
            //Deleting NoteHeaderID
            var resp = _noteHeaderDataProvider.DeleteNoteHeader(Id, modifiedOn);
            var noteRepository = new Response<NoteHeaderModel>();
            noteRepository.ResultCode = resp.ResultCode;
            noteRepository.ResultMessage = resp.ResultMessage;
            return noteRepository;
            //var noteRepository = _unitOfWork.GetRepository<NoteHeaderModel>(SchemaName.ECI);
            //var param = new List<SqlParameter> { new SqlParameter("ProgressNoteID", Id) };
            //return noteRepository.ExecuteNQStoredProc("usp_DeleteProgressNote", param);
        }

        #endregion

        #region Private Methods

        private List<SqlParameter> BuildSpParams(NoteHeaderModel noteHeader)
        {
            var spParameters = new List<SqlParameter>();
            if (noteHeader.ProgressNote.ProgressNoteID > 0) // Only in case of Update
                spParameters.Add(new SqlParameter("ProgressNoteID", noteHeader.ProgressNote.ProgressNoteID));

            spParameters.AddRange(new List<SqlParameter> {
                new SqlParameter("NoteHeaderID", (object)noteHeader.ProgressNote.NoteHeaderID ?? DBNull.Value),
                new SqlParameter("StartTime", (object)noteHeader.ProgressNote.StartTime ?? DBNull.Value),
                new SqlParameter("EndTime", (object)noteHeader.ProgressNote.EndTime ?? DBNull.Value),
                new SqlParameter("ContactMethodID", (object)noteHeader.ProgressNote.ContactMethodID ?? DBNull.Value),
                new SqlParameter("ContactMethodOther", (object)noteHeader.ProgressNote.ContactMethodOther ?? DBNull.Value),
                new SqlParameter("FirstName", (object)noteHeader.ProgressNote.FirstName ?? DBNull.Value),
                new SqlParameter("LastName", (object)noteHeader.ProgressNote.LastName ?? DBNull.Value),
                new SqlParameter("RelationshipTypeID", (object)noteHeader.ProgressNote. RelationshipTypeID  ?? DBNull.Value),
                new SqlParameter("Summary", (object)noteHeader.ProgressNote.Summary ?? DBNull.Value),
                new SqlParameter("ReviewedSourceConcerns", (object)noteHeader.ProgressNote. ReviewedSourceConcerns?? DBNull.Value),
                new SqlParameter("ReviewedECIProcess", (object)noteHeader.ProgressNote. ReviewedECIProcess  ?? DBNull.Value),
                new SqlParameter("ReviewedECIServices", (object)noteHeader.ProgressNote.ReviewedECIServices ?? DBNull.Value),
                new SqlParameter("ReviewedECIRequirements", (object)noteHeader.ProgressNote.ReviewedECIRequirements ?? DBNull.Value),
                new SqlParameter("IsSurrogateParentNeeded", (object)noteHeader.ProgressNote.IsSurrogateParentNeeded ?? DBNull.Value),
                new SqlParameter("Comments", (object)noteHeader.ProgressNote.Comments ?? DBNull.Value),
                new SqlParameter("IsDischarged", (object)noteHeader.ProgressNote.IsDischarged ?? DBNull.Value),
                new SqlParameter("ModifiedOn", noteHeader.ModifiedOn ?? DateTime.Now)
            });

            return spParameters;
        }

        #endregion
    }
}
