
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[usp_UpdateRecordHeader]
-- Author:		Sumana Sangapu
-- Date:		01/12/2017
--
-- Purpose:		Add Record Header for print view snapshot details
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		Contact Table (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/12/2017	Sumana Sangapu	Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_UpdateRecordHeader]
	@RecordHeaderID BIGINT,
	@WorkflowID BIGINT,
	@RecordPrimaryKeyValue BIGINT,
	@IsActive	BIT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	DECLARE @AuditDetailID	BIGINT,
			@ProcName		VARCHAR(255) = OBJECT_NAME(@@PROCID),
			@ID				BIGINT

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY

			EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'RecordHeader', @ID, NULL, NULL, NULL, NULL, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
					
			UPDATE  rh
			SET		WorkflowID = @WorkflowID,
					RecordPrimaryKeyValue = @RecordPrimaryKeyValue,
					IsActive = @IsActive,
					ModifiedBy = @ModifiedBy,
					ModifiedOn = @ModifiedOn,
					SystemModifiedOn = GETUTCDATE()
			FROM	Core.RecordHeader rh 
			WHERE	rh.RecordHeaderID = @RecordHeaderID

			EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'RecordHeader', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END TRY

	BEGIN CATCH
		SELECT  @ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END