-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[usp_DeleteServiceRecordingAttendee]
-- Author:		Scott Martin
-- Date:		03/23/2016
--
-- Purpose:		Delete Service Recording Attendee
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/23/2016	Scott Martin		Initial creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_DeleteServiceRecordingAttendee]
	@ServiceRecordingAttendeeID bigint,
	@ModifiedOn datetime,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@AuditDetailID BIGINT;

	BEGIN TRY
	EXEC Core.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Core', 'ServiceRecordingAttendee', @ServiceRecordingAttendeeID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	UPDATE Core.ServiceRecordingAttendee
	SET	IsActive = 0,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		ServiceRecordingAttendeeID = @ServiceRecordingAttendeeID;

	EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Core', 'ServiceRecordingAttendee', @AuditDetailID, @ServiceRecordingAttendeeID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE();
	END CATCH
END