-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Reference].[usp_AddServicesModuleComponent]
-- Author:		Kyle Campbell
-- Date:		01/05/2017
--
-- Purpose:		Add ServicesModuleComponent
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/06/2017	Kyle Campbell	TFS #14007	Initial Creation
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Reference].[usp_AddServicesModuleComponent]
	@ServicesID INT,
	@ModuleComponentID INT,
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
		   @ResultMessage = 'Executed successfully'

	BEGIN TRY
		
		INSERT INTO Reference.ServicesModuleComponent
		(
			ModuleComponentID,
			ServicesID,
			IsActive,
			ModifiedBy,
			ModifiedOn,
			CreatedBy,
			CreatedOn			
		)
		VALUES
		(
			@ModuleComponentID,
			@ServicesID,
			1,
			@ModifiedBy,
			@ModifiedOn,
			@ModifiedBy,
			@ModifiedOn
		)

		SELECT @ID = SCOPE_IDENTITY();		

		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Reference', 'ServicesModuleComponent', @ID, NULL, NULL, NULL, NULL, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Reference', 'ServicesModuleComponent', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()			   
	END CATCH

END