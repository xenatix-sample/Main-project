using Axis.Model.Common;
using Axis.Model.ECI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.DataProvider.ECI
{
    public interface IProgressNoteAssessmentDataProvider
    {
        /// <summary>
        /// Gets the dischages.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <param name="noteTypeID">The note type identifier.</param>
        /// <returns></returns>
        Response<ProgressNoteAssessmentModel> GetProgressNotes(long contactID, int noteTypeID);

        /// <summary>
        /// Gets the dischage.
        /// </summary>
        /// <param name="ProgressNoteAssessmentID">The progress note assessment identifier.</param>
        /// <returns></returns>
        Response<ProgressNoteAssessmentModel> GetProgressNote(long ProgressNoteAssessmentID);

        /// <summary>
        /// Adds the dischage.
        /// </summary>
        /// <param name="noteAssessment">The note assessment.</param>
        /// <returns></returns>
        Response<ProgressNoteAssessmentModel> AddNoteAssessment(ProgressNoteAssessmentModel noteAssessment);

        /// <summary>
        /// Updates the dischage.
        /// </summary>
        /// <param name="noteAssessment">The note assessment.</param>
        /// <returns></returns>
        Response<ProgressNoteAssessmentModel> UpdateNoteAssessment(ProgressNoteAssessmentModel noteAssessment);

        /// <summary>
        /// Deletes the dischage.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        Response<ProgressNoteAssessmentModel> DeleteNoteAssessment(long Id, DateTime modifiedOn);
    }
}
