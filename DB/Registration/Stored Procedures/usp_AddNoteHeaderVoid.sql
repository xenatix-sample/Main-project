-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Registration].[usp_AddNoteHeaderVoid]
-- Author:		Scott Martin
-- Date:		04/06/2016
--
-- Purpose:		Add Note Header Void record
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/06/2016	Scott Martin		Initial creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_AddNoteHeaderVoid]
	@NoteHeaderID int,
	@NoteHeaderVoidReasonID smallint,
	@Comments nvarchar(1000),
	@ModifiedOn datetime,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ID BIGINT OUTPUT
AS
BEGIN
DECLARE @ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	DECLARE @ContactID BIGINT;

	SELECT @ContactID = ContactID FROM Registration.NoteHeader WHERE NoteHeaderID = @NoteHeaderID;

	INSERT INTO  Registration.NoteHeaderVoid
	(
		NoteHeaderID,
		NoteHeaderVoidReasonID,
		Comments,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	VALUES
	(
		@NoteHeaderID,
		@NoteHeaderVoidReasonID,
		@Comments,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	);
		
	SELECT @ID = SCOPE_IDENTITY();

	DECLARE @AuditDetailID BIGINT;
			
	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'NoteHeaderVoid', @ID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'NoteHeaderVoid', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE();
	END CATCH
END