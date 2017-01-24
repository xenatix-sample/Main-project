using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model;
using Axis.Model.CallCenter;
using Axis.Model.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Axis.DataProvider.CallCenter.CallerInformation
{
    public class CallerInformationDataProvider : ICallerInformationDataProvider
    {
        #region Class Variables

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        readonly ICallCenterProgressNoteDataProvider _progressNoteDataProvider;

        #endregion Class Variables

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CallerInformationDataProvider" /> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public CallerInformationDataProvider(IUnitOfWork unitOfWork, ICallCenterProgressNoteDataProvider progressNoteDataProvider)
        {
            _unitOfWork = unitOfWork;
            _progressNoteDataProvider = progressNoteDataProvider;
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Gets the caller information.
        /// </summary>
        /// <param name="callCenterHeaderID">The call center header identifier.</param>
        /// <returns></returns>
        public Response<CallCenterProgressNoteModel> GetCallerInformation(long callCenterHeaderID)
        {
            var callCenterRepository = _unitOfWork.GetRepository<CallCenterProgressNoteModel>(SchemaName.CallCenter);
            var procParams = new List<SqlParameter> { new SqlParameter("CallCenterHeaderID", callCenterHeaderID) };
            return callCenterRepository.ExecuteStoredProc("usp_GetCrisisLine", procParams);
        }   

        /// <summary>
        /// Adds the caller information.
        /// </summary>
        /// <param name="callerHeader">The caller header.</param>
        /// <returns></returns>
        public Response<CallCenterProgressNoteModel> AddCallerInformation(CallCenterProgressNoteModel callerHeader)
        {
            var callCenterRepository = _unitOfWork.GetRepository<CallCenterProgressNoteModel>(SchemaName.CallCenter);
            var procParams = BuildParams(callerHeader);

            var response = _unitOfWork.EnsureInTransaction(callCenterRepository.ExecuteNQStoredProc, "usp_AddCrisisLine", procParams,
                                idResult: true, forceRollback: callerHeader.ForceRollback.GetValueOrDefault(false));

            if (callerHeader.NoteAdded && response.ID > 0)
            {
                callerHeader.CallCenterHeaderID = response.ID;
                var resp = _progressNoteDataProvider.AddCallCenterProgressNote(callerHeader);
            }

            return response;
        }

        /// <summary>
        /// Updates the caller information.
        /// </summary>
        /// <param name="infoModel">The caller header.</param>
        /// <returns></returns>
        public Response<CallCenterProgressNoteModel> UpdateCallerInformation(CallCenterProgressNoteModel infoModel)
        {
            var callCenterRepository = _unitOfWork.GetRepository<CallCenterProgressNoteModel>(SchemaName.CallCenter);
            var procParams = BuildParams(infoModel);

            return _unitOfWork.EnsureInTransaction(
                    callCenterRepository.ExecuteNQStoredProc,
                    "usp_UpdateCrisisLine",
                    procParams,
                    forceRollback: infoModel.ForceRollback.GetValueOrDefault(false)
                );
        }

        /// <summary>
        /// Updates the Modified On in caller information Table.
        /// </summary>
        /// <param name="callCenterHeaderID">The call center header identifier.</param>
        /// <returns></returns>
        public Response<CallCenterProgressNoteModel> UpdateModifiedOn(CallCenterProgressNoteModel model)
        {
            var callCenterRepository = _unitOfWork.GetRepository<CallCenterProgressNoteModel>(SchemaName.CallCenter);
            var procParams = new List<SqlParameter> { new SqlParameter("CallCenterHeaderID", model.CallCenterHeaderID), new SqlParameter("ModifiedOn", model.ModifiedOn) };
            return _unitOfWork.EnsureInTransaction(
                     callCenterRepository.ExecuteNQStoredProc,
                     "usp_UpdateModifiedOnHeader",
                     procParams
                 );
        }




        public Response<CallCenterAssessmentResponseModel> GetCallCenterAssessmentResponse(long callCenterHeaderID, long assessmentID)
        {
            var callCenterRepository = _unitOfWork.GetRepository<CallCenterAssessmentResponseModel>(SchemaName.CallCenter);
            var procParams = new List<SqlParameter> { new SqlParameter("CallCenterHeaderID", callCenterHeaderID), new SqlParameter("AssessmentID", assessmentID) };
            return callCenterRepository.ExecuteStoredProc("usp_GetCallCenterAssessmentResponse", procParams);
        }


        public Response<CallCenterAssessmentResponseModel> GetCallCenterAssessmentResponseByContactID(long contactID)
        {
            var callCenterRepository = _unitOfWork.GetRepository<CallCenterAssessmentResponseModel>(SchemaName.CallCenter);
            var procParams = new List<SqlParameter> { new SqlParameter("ContactID", contactID) };
            return callCenterRepository.ExecuteStoredProc("usp_GetCallCenterAssessmentResponseByContactID", procParams);
        }

        public Response<CallCenterAssessmentResponseModel> AddCallCenterAssessmentResponse(CallCenterAssessmentResponseModel callCenterAssessmentResponse)
        {
            var callCenterRepository = _unitOfWork.GetRepository<CallCenterAssessmentResponseModel>(SchemaName.CallCenter);
            var procParams = BuildCallCenterAssessmentParams(callCenterAssessmentResponse);

            var response = _unitOfWork.EnsureInTransaction(callCenterRepository.ExecuteNQStoredProc, "usp_AddCallCenterAssessmentResponse", procParams,
                                idResult: true, forceRollback: callCenterAssessmentResponse.ForceRollback.GetValueOrDefault(false));

            return response;
        }

        #endregion Public Methods

        #region Private Methods

        private List<SqlParameter> BuildParams(CallCenterProgressNoteModel infoModel)
        {
            var spParameters = new List<SqlParameter>();
            if (infoModel.CallCenterHeaderID > 0)
                spParameters.Add(new SqlParameter("CallCenterHeaderID", infoModel.CallCenterHeaderID));

            spParameters.AddRange(new List<SqlParameter>{
                new SqlParameter("ParentCallCenterHeaderID", (object)infoModel.ParentCallCenterHeaderID ?? DBNull.Value),
                new SqlParameter("CallCenterTypeID", infoModel.CallCenterTypeID),
                new SqlParameter("CallerID", (object)infoModel.CallerID ?? DBNull.Value),
                new SqlParameter("ClientID", (object)infoModel.ContactID ?? DBNull.Value),
                new SqlParameter("ContactTypeID", (object)infoModel.ContactTypeID ?? DBNull.Value),
                new SqlParameter("ProviderID", (object)infoModel.ProviderID ?? DBNull.Value),
                new SqlParameter("CallStartTime", (object)infoModel.CallStartTime ?? DBNull.Value),
                new SqlParameter("CallEndTime", (object)infoModel.CallEndTime ?? DBNull.Value),
                new SqlParameter("CallStatusID", (object)infoModel.CallStatusID ?? DBNull.Value),
                new SqlParameter("ProgramUnitID", (object)infoModel.ProgramUnitID ?? DBNull.Value),
                new SqlParameter("ReferralAgencyID", (object)infoModel.ReferralAgencyID ?? DBNull.Value),
                new SqlParameter("OtherReferralAgency", (object)infoModel.OtherReferralAgency ?? DBNull.Value),
                new SqlParameter("CountyID", (object)infoModel.CountyID ?? DBNull.Value),
                new SqlParameter("SuicideHomicideID", (object)infoModel.SuicideHomicideID ?? DBNull.Value),
                new SqlParameter("CallCenterPriorityID", (object)infoModel.CallCenterPriorityID ?? DBNull.Value),
                new SqlParameter("DateOfIncident", (object)infoModel.DateOfIncident ?? DBNull.Value),
                new SqlParameter("ReasonCalled", (object)infoModel.ReasonCalled ?? DBNull.Value),
                new SqlParameter("Disposition", (object)infoModel.Disposition ?? DBNull.Value),
                new SqlParameter("OtherInformation", (object)infoModel.OtherInformation ?? DBNull.Value),
                new SqlParameter("Comments", (object)infoModel.Comments ?? DBNull.Value),
                new SqlParameter("FollowUpRequired", (object)infoModel.FollowUpRequired ?? DBNull.Value),
                //new SqlParameter("ModifiedBy", (object)infoModel.ModifiedBy ?? DBNull.Value),
                new SqlParameter("ModifiedOn", (object)infoModel.ModifiedOn ?? DBNull.Value),
                new SqlParameter("IsLinkedToContact", (object)infoModel.IsLinkedToContact ?? DBNull.Value)
            });
            return spParameters;
        }

        private List<SqlParameter> BuildCallCenterAssessmentParams(CallCenterAssessmentResponseModel callCenterAssessmentResponse)
        {
            var spParameters = new List<SqlParameter>();
            if (callCenterAssessmentResponse.CallCenterAssessmentResponseID > 0)
                spParameters.Add(new SqlParameter("CallCenterAssessmentResponseID", callCenterAssessmentResponse.CallCenterAssessmentResponseID));

            spParameters.AddRange(new List<SqlParameter>{
                new SqlParameter("CallCenterHeaderID", (object)callCenterAssessmentResponse.CallCenterHeaderID ?? DBNull.Value),
                new SqlParameter("AssessmentID", callCenterAssessmentResponse.AssessmentID),
                new SqlParameter("ResponseID", (object)callCenterAssessmentResponse.ResponseID ?? DBNull.Value),
                new SqlParameter("ModifiedOn", (object)callCenterAssessmentResponse.ModifiedOn ?? DateTime.Now)
            });
            return spParameters;
        }
        #endregion Private Methods
    }
}
