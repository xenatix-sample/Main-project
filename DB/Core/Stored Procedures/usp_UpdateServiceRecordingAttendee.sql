-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[usp_UpdateServiceRecordingAttendee]
-- Author:		Scott Martin
-- Date:		03/23/2016
--
-- Purpose:		Update Service Recording Attendee
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/23/2016	Scott Martin		Initial creation
-- 12/15/2016	Scott Martin		Update auditing
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_UpdateServiceRecordingAttendee]
	@ServiceRecordingAttendeeID bigint,
	@ServiceRecordingID bigint,
	@Name nvarchar(255),
	@ModifiedOn datetime,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@AuditDetailID BIGINT,
		@ContactID BIGINT;

	BEGIN TRY
	SELECT @ContactID = ContactID FROM Core.vw_GetServiceRecordingDetails SRD WHERE SRD.ServiceRecordingID = @ServiceRecordingID;

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'ServiceRecordingAttendee', @ServiceRecordingAttendeeID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	UPDATE Core.ServiceRecordingAttendee
	SET	ServiceRecordingID = @ServiceRecordingID,
		Name = @Name,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		ServiceRecordingAttendeeID = @ServiceRecordingAttendeeID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'ServiceRecordingAttendee', @AuditDetailID, @ServiceRecordingAttendeeID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE();
	END CATCH
END