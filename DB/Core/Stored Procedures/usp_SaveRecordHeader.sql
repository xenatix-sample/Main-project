-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[usp_SaveRecordHeader]
-- Author:		Sumana Sangapu
-- Date:		01/12/2017
--
-- Purpose:		Save Record Header and RecordHeader Details for print view snapshot details
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/12/2017	Sumana Sangapu	Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_SaveRecordHeader]
	@WorkflowDataKey nvarchar(250),
	@RecordPrimaryKeyValue BIGINT,
	@contactID BIGINT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	DECLARE @AuditDetailID	BIGINT,
			@ProcName		VARCHAR(255) = OBJECT_NAME(@@PROCID),
			@RecordHeaderID	BIGINT,
			@WorkflowID		BIGINT

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY

			SELECT		@WorkflowID = w.WorkflowID 
			FROM		Core.Workflow w 
			INNER JOIN	Core.WorkflowComponentMapping wcm
			ON			w.WorkflowID	= wcm.WorkflowID
			INNER JOIN	Core.ModuleComponent mc
			ON			wcm.ModuleComponentID = mc.ModuleComponentID
			WHERE		mc.DataKey = @WorkflowDataKey 

			IF NOT EXISTS (SELECT 'X' FROM Core.RecordHeader WHERE WorkflowID = @WorkflowID AND RecordPrimaryKeyValue = @RecordPrimaryKeyValue ) 
			BEGIN
						INSERT INTO Core.RecordHeader
						( WorkflowID, RecordPrimaryKeyValue, IsActive, ModifiedBy, ModifiedOn, CreatedBy, CreatedOn)
						SELECT	@WorkflowID, @RecordPrimaryKeyValue, '1' as IsActive, @ModifiedBy, @ModifiedOn, @ModifiedBy, @ModifiedOn 

						SET @RecordHeaderID = SCOPE_IDENTITY()

						EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Core', 'RecordHeader', @RecordHeaderID, NULL, NULL, NULL, NULL, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

						EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Core', 'RecordHeader', @AuditDetailID, @RecordHeaderID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			END
			ELSE 
			BEGIN
						SELECT @RecordHeaderID = RecordHeaderID FROM Core.RecordHeader WHERE WorkflowID = @WorkflowID AND RecordPrimaryKeyValue = @RecordPrimaryKeyValue
			END

			EXEC [Core].[usp_SaveRecordHeaderDetails] @RecordHeaderID, @WorkflowID, @RecordPrimaryKeyValue, @ContactID,@ModifiedOn,  @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT

	END TRY

	BEGIN CATCH
		SELECT  @ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END