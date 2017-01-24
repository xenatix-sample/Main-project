
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Reference].[usp_UpdateServicesModuleComponent]
-- Author:		Kyle Campbell
-- Date:		01/05/2017
--
-- Purpose:		Update ServicesModuleComponent
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/06/2017	Kyle Campbell	TFS #14007	Initial Creation
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Reference].[usp_UpdateServicesModuleComponent]
	@ServicesModuleComponentID INT,
	@ServicesID INT,
	@ModuleComponentID INT,
	@IsActive BIT,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
	DECLARE @AuditDetailID BIGINT,
			@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT @ResultCode = 0,
		   @ResultMessage = 'Executed successfully'

	BEGIN TRY
		
		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Reference', 'ServicesModuleComponent', @ServicesModuleComponentID, NULL, NULL, NULL, NULL, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE Reference.ServicesModuleComponent 
		SET	
			ServicesID = @ServicesID,
			ModuleComponentID = @ModuleComponentID,
			IsActive = @IsActive,
			ModifiedBy = @ModifiedBy,
			ModifiedOn = @ModifiedOn,
			SystemModifiedOn = GETUTCDATE()
		WHERE ServicesModuleComponentID = @ServicesModuleComponentID;

		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Reference', 'ServicesModuleComponent', @AuditDetailID, @ServicesModuleComponentID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()			   
	END CATCH

END