-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[ECI].[usp_UpdateNoteAssessment]
-- Author:		Gurpreet Singh
-- Date:		01/12/2016
--
-- Purpose:		Update Note Assessment
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/12/2016	Gurpreet Singh		Initial Creation
-- 01/29/2016   Satish Singh		Added Location
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [ECI].[usp_GetNoteAssessments]
	@ContactID BIGINT,
	@NoteTypeID INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN

	BEGIN TRY
		SELECT @ResultCode = 0,
			   @ResultMessage = 'executed successfully'

		SELECT
			dis.[ScheduleNoteAssessmentID],
			dis.[ProgressNoteID],
			dis.[NoteAssessmentDate],
			dis.[NoteAssessmentTime],
			dis.[LocationID],
			dis.[Location],
			dis.[ProviderID],
			dis.[MembersInvited],
			dis.[IsActive],
			dis.[ModifiedBy],
			dis.[ModifiedOn]
		FROM [Registration].[NoteHeader] NH
		INNER JOIN  [ECI].[ProgressNote] PN ON NH.NoteHeaderID = PN.NoteHeaderID
		INNER JOIN [ECI].[ScheduleNoteAssessment] dis ON PN.ProgressNoteID = dis.ProgressNoteID
		WHERE
			(NH.[NoteTypeID] = @NoteTypeID OR ISNULL(@NoteTypeID, 0) = 0 )
			AND NH.[ContactID] = @ContactID
			AND NH.[IsActive] = 1 AND dis.[IsActive] = 1
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END

