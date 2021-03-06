
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[usp_DeleteServiceRecording]
-- Author:		Sumana Sangapu
-- Date:		01/28/2016
--
-- Purpose:		Delete ServiceRecording
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/28/2016	Sumana Sangapu	  - Initial creation.
-- 02/17/2016	Scott Martin		Refactored for audit loggin
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_DeleteServiceRecording]
	@ServiceRecordingID BIGINT,
	@ModifiedBy INT,
	@ModifiedOn DATETIME,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Core', 'ServiceRecording', @ServiceRecordingID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE Core.[ServiceRecording]
	SET IsActive = 0,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()

	WHERE
		ServiceRecordingID = @ServiceRecordingID;

	EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Core', 'ServiceRecording', @AuditDetailID, @ServiceRecordingID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT; 

 	END TRY

	BEGIN CATCH
	SELECT @ResultCode = ERROR_SEVERITY(),
			@ResultMessage = ERROR_MESSAGE()
	END CATCH
END