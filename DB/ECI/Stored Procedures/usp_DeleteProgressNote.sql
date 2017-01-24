-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_DeleteProgressNote]
-- Author:		Scott Martin
-- Date:		12/28/2015
--
-- Purpose:		Delete referral progress note
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/28/2015	Scott Martin	Initial creation.
-- 1/7/2016		Scott Martin	Moved to ECI schema and removed Referral prefix
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added SystemModifiedOn field
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [ECI].[usp_DeleteProgressNote]
	@ProgressNoteID BIGINT,
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

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Inactivate', 'ECI', 'ProgressNote', @ProgressNoteID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE ECI.ProgressNote
	SET IsActive = 0,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		ProgressNoteID = @ProgressNoteID;

	EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'ECI', 'ProgressNote', @AuditDetailID, @ProgressNoteID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO


