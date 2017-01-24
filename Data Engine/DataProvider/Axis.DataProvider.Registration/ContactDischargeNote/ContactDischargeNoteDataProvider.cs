using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using Axis.Model.Registration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.DataProvider.Registration
{
    public class ContactDischargeNoteDataProvider : IContactDischargeNoteDataProvider
    {
        #region Class Variables

        private readonly IUnitOfWork unitOfWork;

        #endregion

        #region Constructors

        public ContactDischargeNoteDataProvider(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets contact discharge note
        /// </summary>
        /// <param name="contactDischargeNoteID">contactDischargeNoteID</param>
        /// <returns></returns>
        public Response<ContactDischargeNote> GetContactDischargeNote(long contactDischargeNoteID)
        {
            var contactDischargeNoteRepository = unitOfWork.GetRepository<ContactDischargeNote>(SchemaName.Registration);
            var procParams = new List<SqlParameter> { new SqlParameter("ContactDischargeNoteID", contactDischargeNoteID) };

            return contactDischargeNoteRepository.ExecuteStoredProc("usp_GetContactDischargeNote", procParams);
        }


        /// <summary>
        /// Gets contact discharge note
        /// </summary>
        /// <param name="contactDischargeNoteID">contactDischargeNoteID</param>
        /// <param name="noteTypeID">noteTypeID</param>
        /// <returns></returns>
        public Response<ContactDischargeNote> GetContactDischargeNotes(long contactDischargeNoteID, int noteTypeID)
        {
            var contactDischargeNoteRepository = unitOfWork.GetRepository<ContactDischargeNote>(SchemaName.Registration);
            var procParams = new List<SqlParameter> { new SqlParameter("ContactDischargeNoteID", contactDischargeNoteID), new SqlParameter("noteTypeID", noteTypeID) };

            return contactDischargeNoteRepository.ExecuteStoredProc("usp_GetContactDischargeNotes", procParams);
        }

        /// <summary>
        /// Adds contact discharge note
        /// </summary>
        /// <param name="contactDischargeNote">contact discharge note</param>
        /// <returns></returns>
        public Response<ContactDischargeNote> AddContactDischargeNote(ContactDischargeNote contactDischargeNote)
        {
            var referralRepository = unitOfWork.GetRepository<ContactDischargeNote>(SchemaName.Registration);
            var procParams = BuildSpParams(contactDischargeNote);

            return unitOfWork.EnsureInTransaction(referralRepository.ExecuteNQStoredProc, "usp_AddContactDischargeNote", procParams, idResult: true,
                forceRollback: contactDischargeNote.ForceRollback.GetValueOrDefault(false));
        }

        /// <summary>
        /// Updates contact discharge note
        /// </summary>
        /// <param name="referral">contactDischargeNote model</param>
        /// <returns></returns>
        public Response<ContactDischargeNote> UpdateContactDischargeNote(ContactDischargeNote contactDischargeNote)
        {
            var referralRepository = unitOfWork.GetRepository<ContactDischargeNote>(SchemaName.Registration);
            var procParams = BuildSpParams(contactDischargeNote);

            return unitOfWork.EnsureInTransaction(referralRepository.ExecuteNQStoredProc, "usp_UpdateContactDischargeNote", procParams,
                forceRollback: contactDischargeNote.ForceRollback.GetValueOrDefault(false));
        }

        /// <summary>
        /// Deletes Contact Discharge Note
        /// </summary>
        /// <param name="id">Contact Discharge Note Id</param>
        /// <param name="modifiedOn">The modified on</param>
        /// <returns></returns>
        public Response<ContactDischargeNote> DeleteContactDischargeNote(long contactDischargeNoteId, DateTime modifiedOn)
        {
            var contactDischargeNoteRepository = unitOfWork.GetRepository<ContactDischargeNote>(SchemaName.Registration);
            var procParams = new List<SqlParameter> { new SqlParameter("ContactDischargeNoteID", contactDischargeNoteId), new SqlParameter("ModifiedOn", modifiedOn) };
            //TODO: Ensure transaction for delete
            return unitOfWork.EnsureInTransaction(contactDischargeNoteRepository.ExecuteNQStoredProc, "usp_DeleteContactDischargeNote", procParams);
        }

        #endregion

        #region Helpers

        private List<SqlParameter> BuildSpParams(ContactDischargeNote contactDischargeNote)
        {
            var spParameters = new List<SqlParameter>();

            if (contactDischargeNote.ContactDischargeNoteID > 0) // Only in case of Update
                spParameters.Add(new SqlParameter("ContactDischargeNoteID", contactDischargeNote.ContactDischargeNoteID));

            else
                spParameters.Add(new SqlParameter("ContactID", contactDischargeNote.ContactID));

            spParameters.AddRange(new List<SqlParameter> {
                new SqlParameter("ContactAdmissionID", (object)contactDischargeNote.ContactAdmissionID ?? DBNull.Value),
                new SqlParameter("DischargeReasonID", (object)contactDischargeNote.DischargeReasonID),
                new SqlParameter("DischargeDate", (object)contactDischargeNote.DischargeDate ?? DBNull.Value),
                new SqlParameter("NoteTypeID", (object)contactDischargeNote.NoteTypeID ?? DBNull.Value),
                new SqlParameter("SignatureStatusID", (object)contactDischargeNote.SignatureStatusID ?? DBNull.Value),
                new SqlParameter("NoteText", (object)contactDischargeNote.NoteText),
                new SqlParameter("IsDeceased", (object)contactDischargeNote.IsDeceased ?? DBNull.Value),
                new SqlParameter("DeceasedDate", (object)contactDischargeNote.DeceasedDate ?? DBNull.Value),
                new SqlParameter("ModifiedOn", DateTime.Now)
                });
            return spParameters;
        }

        #endregion
    }
}
