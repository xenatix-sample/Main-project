-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[ECI].[usp_GetNoteAssessment]
-- Author:		Gurpreet Singh
-- Date:		01/12/2016
--
-- Purpose:		Get Note Assessment
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/12/2016	Gurpreet Singh		Initial Creation
-- 01/29/2016   Satish Singh		Added Location
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [ECI].[usp_GetNoteAssessment]
	@ScheduleNoteAssessmentID BIGINT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	SELECT
		[ScheduleNoteAssessmentID], 
		[ProgressNoteID], 
		[NoteAssessmentDate], 
		[NoteAssessmentTime], 
		[LocationID], 
		[Location],
		[ProviderID], 
		[MembersInvited],
		[IsActive],
		[ModifiedBy],
		[ModifiedOn]
	FROM
		[ECI].[ScheduleNoteAssessment]
	WHERE
		ScheduleNoteAssessmentID = @ScheduleNoteAssessmentID
		AND [IsActive] = 1

 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
