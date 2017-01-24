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
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added SystemModifiedOn field
-- 01/29/2016   Satish Singh		Added Location
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [ECI].[usp_UpdateNoteAssessment]
	@ScheduleNoteAssessmentID BIGINT,
	@ProgressNoteID BIGINT,
	@NoteAssessmentDate DATETIME,
	@NoteAssessmentTime TIME(7),
	@LocationID INT,
	@Location NVARCHAR(500),
	@ProviderID INT,
	@MembersInvited NVARCHAR(1000),
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode	INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT  @ResultCode = 0,
			@ResultMessage = 'executed successfully'

	BEGIN TRY
	EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'ECI', 'ScheduleNoteAssessment', @ScheduleNoteAssessmentID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE [ECI].[ScheduleNoteAssessment]
		SET ProgressNoteID    =  @ProgressNoteID, 
			NoteAssessmentDate=  @NoteAssessmentDate,
			NoteAssessmentTime=  @NoteAssessmentTime,
			LocationID		  =  @LocationID,
			Location		  =  @Location,
			ProviderID		  =  @ProviderID,
			MembersInvited	  =  @MembersInvited,
			ModifiedBy		  = @ModifiedBy,
			ModifiedOn		  = @ModifiedOn,
			SystemModifiedOn = GETUTCDATE()
		WHERE ScheduleNoteAssessmentID = @ScheduleNoteAssessmentID
		
	EXEC Auditing.usp_AddPostAuditLog 'Update', 'ECI', 'ScheduleNoteAssessment', @AuditDetailID, @ScheduleNoteAssessmentID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END TRY

	BEGIN CATCH
		SELECT 
				@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END