-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[usp_UpdateNoteHeaderVoid]
-- Author:		Scott Martin
-- Date:		04/06/2016
--
-- Purpose:		Update Note Header Void record
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/06/2016	Scott Martin		Initial creation
-- 12/15/2016	Scott Martin	Updated auditing
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_UpdateNoteHeaderVoid]
	@NoteHeaderVoidID bigint,
	@NoteHeaderID int,
	@NoteHeaderVoidReasonID smallint,
	@Comments nvarchar(1000),
	@ModifiedOn datetime,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@AuditDetailID BIGINT,
		@ContactID BIGINT;

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	BEGIN TRY
	SELECT @ContactID = ContactID FROM Registration.NoteHeader WHERE NoteHeaderID = @NoteHeaderID;

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'NoteHeaderVoid', @NoteHeaderVoidID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT

	UPDATE  Registration.NoteHeaderVoid
	SET NoteHeaderID = @NoteHeaderID,
		NoteHeaderVoidReasonID = @NoteHeaderVoidReasonID,
		Comments = @Comments,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		NoteHeaderVoidID = @NoteHeaderVoidID;
		

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'NoteHeaderVoid', @AuditDetailID, @NoteHeaderVoidID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE();
	END CATCH
END