-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_UpdateNote]
-- Author:		Scott Martin
-- Date:		11/13/2015
--
-- Purpose:		Update Note
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/13/2015	Scott Martin	Initial creation.
-- 11/18/2015	Scott Martin	TFS 3610	Add audit logging
-- 11/19/2015	Gurpreet Singh	Removed Notes parameter
-- 11/20/2015	Gurpreet Singh	Removed DocumentStatusID parameter
-- 12/04/2015   Satish Singh	Added Notes
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added SystemModifiedOn to Update statement
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Clinical].[usp_UpdateNote]
	@NoteID BIGINT,
	@ContactID BIGINT,
	@NoteTypeID SMALLINT,
	@TakenBy INT,
	@TakenTime DATETIME,
	@Notes NVARCHAR(MAX),
	@EncounterID BIGINT NULL,
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

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Clinical', 'Notes', @NoteID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE Clinical.Notes
	SET ContactID = @ContactID,
		TakenBy = @TakenBy,
		TakenTime = @TakenTime,
		NoteTypeID = @NoteTypeID,
		EncounterID = @EncounterID,
		ModifiedBy = @ModifiedBy,
		Notes = @Notes,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	WHERE
		NoteID = @NoteID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Clinical', 'Notes', @AuditDetailID, @NoteID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
	  
 	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END
GO