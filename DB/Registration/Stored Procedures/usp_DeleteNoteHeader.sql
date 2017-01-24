-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_DeleteNoteHeader]
-- Author:		Scott Martin
-- Date:		1/7/2016
--
-- Purpose:		Delete a Note Header 
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 1/7/2016	Scott Martin		Initial Creation
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added SystemModifiedOn field
-- 12/15/2016	Scott Martin	Updated auditing
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_DeleteNoteHeader]
	@NoteHeaderID BIGINT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@ContactID BIGINT;

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	SELECT @ContactID = ContactID FROM Registration.NoteHeader WHERE NoteHeaderID = @NoteHeaderID;

	BEGIN TRY
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Registration', 'NoteHeader', @NoteHeaderID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;	

	UPDATE [Registration].[NoteHeader]
	SET	IsActive = 0,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		NoteHeaderID = @NoteHeaderID;

	EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Registration', 'NoteHeader', @AuditDetailID, @NoteHeaderID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT; 
  
	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END