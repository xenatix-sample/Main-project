-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddNoteHeader]
-- Author:		Scott Martin
-- Date:		1/7/2016
--
-- Purpose:		Add a Note Header 
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 1/7/2016	Scott Martin		Initial Creation
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added CreatedBy and CreatedOn to Insert
-- 12/15/2016	Scott Martin	Updated auditing
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_AddNoteHeader]
	@ContactID BIGINT,
	@NoteTypeID INT,
	@TakenBy INT,
	@TakenTime DATETIME,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT,
	@ID INT OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	BEGIN TRY	
	INSERT INTO [Registration].[NoteHeader]
	(
		ContactID,
		NoteTypeID,
		TakenBy,
		TakenTime,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	VALUES
	(
		@ContactID,
		@NoteTypeID,
		@TakenBy,
		@TakenTime,
		1,
		@ModifiedBy,
		@ModifiedOn,
		@ModifiedBy,
		@ModifiedOn
	);
		
	SELECT @ID = SCOPE_IDENTITY();

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'NoteHeader', @ID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'NoteHeader', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
  
	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH
END