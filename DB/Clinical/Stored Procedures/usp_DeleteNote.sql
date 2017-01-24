-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_DeleteNote]
-- Author:		Scott Martin
-- Date:		11/13/2015
--
-- Purpose:		Delete Note
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/13/2015	Scott Martin	Initial creation.
-- 11/18/2015	Scott Martin	TFS 3610	Add audit logging
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added CreatedBy and CreatedOn to Insert
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Clinical].[usp_DeleteNote]
	@NoteID BIGINT,
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

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Clinical', 'Notes', @NoteID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE  Clinical.Notes
	SET IsActive = 0, 
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		NoteID = @NoteID;

	EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Clinical', 'Notes', @AuditDetailID, @NoteID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	  
 	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO


