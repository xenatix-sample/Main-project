-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Clinical].[usp_AddNote]
-- Author:		Scott Martin
-- Date:		11/13/2015
--
-- Purpose:		Add a Note
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 11/13/2015	Scott Martin	 Initial creation.
-- 11/17/2015	Gurpreet Singh	 Updated NoteTypeID parameter name.
-- 11/18/2015	Scott Martin	TFS 3610	Add audit logging
-- 11/19/2015	Gurpreet Singh	Removed Notes parameter
-- 11/16/2015	Scott Martin	Added DocumentStatus
-- 12/04/2015   Satish Singh	Added Notes
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added CreatedBy and CreatedOn to Insert
-----------------------------------------------------------------------------------------------------------------------


CREATE PROCEDURE [Clinical].[usp_AddNote]
	@ContactID BIGINT,
	@NoteTypeID SMALLINT,
	@TakenBy INT,
	@TakenTime DATETIME,
	@Notes NVARCHAR(MAX),
	@EncounterID BIGINT NULL,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ID BIGINT OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	BEGIN TRY
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	INSERT INTO Clinical.Notes
	(
		ContactID,
		Notes,
		NoteTypeID,
		TakenBy,
		TakenTime,
		EncounterID,
		DocumentStatusID,
		ModifiedBy,
		ModifiedOn,
		[CreatedBy],
		[CreatedOn]
	)
	VALUES
	(
		@ContactID,
		@Notes,
		@NoteTypeID,
		@TakenBy,
		@TakenTime,
		@EncounterID,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	);
		
	SELECT @ID = SCOPE_IDENTITY();

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Clinical', 'Notes', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Clinical', 'Notes', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

 	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE();
	END CATCH
END
GO


