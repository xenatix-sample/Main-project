-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[ECI].[usp_DeleteNoteAssessment]
-- Author:		Gurpreet Singh
-- Date:		01/12/2016
--
-- Purpose:		Delete Note Assessment
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/12/2016	Gurpreet Singh	Initial creation.
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added SystemModifiedOn field
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [ECI].[usp_DeleteNoteAssessment]
	@ScheduleNoteAssessmentID BIGINT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Inactivate', 'ECI', 'ScheduleNoteAssessment', @ScheduleNoteAssessmentID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE [ECI].[ScheduleNoteAssessment]
	SET IsActive = 0,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		ScheduleNoteAssessmentID = @ScheduleNoteAssessmentID;

	EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'ECI', 'ScheduleNoteAssessment', @AuditDetailID, @ScheduleNoteAssessmentID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

 	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO
