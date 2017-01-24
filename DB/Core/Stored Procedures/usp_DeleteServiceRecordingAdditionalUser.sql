-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[usp_DeleteServiceRecordingAdditionalUser]
-- Author:		Scott Martin
-- Date:		03/23/2016
--
-- Purpose:		Delete Service Recording Additional User
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/23/2016	Scott Martin		Initial creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_DeleteServiceRecordingAdditionalUser]
	@ServiceRecordingAdditionalUserID bigint,
	@ModifiedOn datetime,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@AuditDetailID BIGINT;

	BEGIN TRY
	EXEC Core.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Core', 'ServiceRecordingAdditionalUser', @ServiceRecordingAdditionalUserID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	UPDATE Core.ServiceRecordingAdditionalUser
	SET	IsActive = 0,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		ServiceRecordingAdditionalUserID = @ServiceRecordingAdditionalUserID;

	EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Core', 'ServiceRecordingAdditionalUser', @AuditDetailID, @ServiceRecordingAdditionalUserID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE();
	END CATCH
END