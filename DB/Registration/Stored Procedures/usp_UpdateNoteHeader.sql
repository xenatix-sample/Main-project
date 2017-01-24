-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdateNoteHeader]
-- Author:		Scott Martin
-- Date:		1/7/2016
--
-- Purpose:		Update a Note Header 
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 1/7/2016	Scott Martin		Initial Creation
-- 12/15/2016	Scott Martin	Updated auditing
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_UpdateNoteHeader]
	@NoteHeaderID BIGINT,
	@ContactID BIGINT,
	@NoteTypeID INT,
	@TakenBy INT,
	@TakenTime DATETIME,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	BEGIN TRY
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'NoteHeader', @NoteHeaderID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE [Registration].[NoteHeader]
	SET	ContactID = @ContactID,
		NoteTypeID = @NoteTypeID,
		TakenBy = @TakenBy,
		TakenTime = @TakenTime,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		NoteHeaderID = @NoteHeaderID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'NoteHeader', @AuditDetailID, @NoteHeaderID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
  
	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END