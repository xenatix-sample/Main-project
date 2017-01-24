-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Reference].[usp_SaveServiceStatusModuleComponent]
-- Author:		Kyle Campbell
-- Date:		01/05/2017
--
-- Purpose:		Saves mapping for ServiceStatus and ModuleComponent
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/05/2017	Kyle Campbell	TFS #14007	Initial Creation
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Reference].[usp_SaveServiceStatusModuleComponent]
	@ServicesID INT,
	@ModuleComponentID INT,
	@ServiceStatusXMLValue XML,	
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
SET NOCOUNT ON
	BEGIN TRY		
		DECLARE @ID BIGINT,						
				@AuditDetailID BIGINT,
				@AuditCursor CURSOR,				
				@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

		SELECT @ResultCode = 0;
		SELECT @ResultMessage = 'Executed successfully';

		DECLARE @SSMC TABLE
		(
			ServiceStatusModuleComponentID BIGINT,
			ModuleComponentID BIGINT,
			ServiceStatusID SMALLINT,
			ServicesID INT,
			IsActive BIT
		);

		INSERT INTO @SSMC (ServiceStatusModuleComponentID, ModuleComponentID, ServiceStatusID, ServicesID, IsActive)
		SELECT 
			SSMC.ServiceStatusModuleComponentID,
			@ModuleComponentID AS ModuleComponentID,
			T.C.value('ServiceStatusID[1]', 'SMALLINT') AS ServiceStatusID,
			@ServicesID AS ServicesID,
			SSMC.IsActive
		FROM @ServiceStatusXMLValue.nodes('ServiceStatusXMLValue/ServiceStatus') AS T(C)
			FULL OUTER JOIN (SELECT * FROM Reference.ServiceStatusModuleComponent WHERE ModuleComponentID = @ModuleComponentID AND ServicesID = @ServicesID) AS SSMC
			ON T.C.value('ServiceStatusID[1]','BIGINT') = SSMC.ServiceStatusID
		
		IF EXISTS (SELECT TOP 1 * FROM @SSMC WHERE ServiceStatusModuleComponentID IS NOT NULL)
		BEGIN
			DECLARE @SS_ID INT,
				@IsActive BIT;

			SET @AuditCursor = CURSOR FOR
				SELECT ServiceStatusModuleComponentID, ServiceStatusID, IsActive FROM @SSMC WHERE ServiceStatusModuleComponentID IS NOT NULL
			
			OPEN @AuditCursor			
			FETCH NEXT FROM @AuditCursor INTO @ID, @SS_ID, @IsActive

			WHILE @@FETCH_STATUS = 0				
			BEGIN
				
				--Prevent matching records from updating
				IF (@SS_ID IS NOT NULL AND @IsActive = 0)
					BEGIN
						EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Reference', 'ServiceStatusModuleComponent', @ID, NULL, NULL, NULL, NULL, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
								
						UPDATE Reference.ServiceStatusModuleComponent SET IsActive = 1, ModifiedOn = @ModifiedOn, ModifiedBy = @ModifiedBy, SystemModifiedOn = GETUTCDATE() WHERE ServiceStatusModuleComponentID = @ID

						EXEC Auditing.usp_AddPostAuditLog 'Update', 'Reference', 'ServiceStatusModuleComponent', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
					END
				ELSE IF (@SS_ID IS NULL AND @IsActive = 1)
					BEGIN
						EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Reference', 'ServiceStatusModuleComponent', @ID, NULL, NULL, NULL, NULL, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
								
						UPDATE Reference.ServiceStatusModuleComponent SET IsActive = 0, ModifiedOn = @ModifiedOn, ModifiedBy = @ModifiedBy, SystemModifiedOn = GETUTCDATE() WHERE ServiceStatusModuleComponentID = @ID

						EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Reference', 'ServiceStatusModuleComponent', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
					END									
				FETCH NEXT FROM @AuditCursor INTO @ID, @SS_ID, @IsActive
			END;

			CLOSE @AuditCursor;
			DEALLOCATE @AuditCursor;			
		END

		IF EXISTS (SELECT TOP 1 * FROM @SSMC WHERE ServiceStatusModuleComponentID IS NULL AND ServiceStatusID IS NOT NULL)
		BEGIN
			DECLARE @ServiceStatusID INT;

			SET @AuditCursor = CURSOR FOR
				SELECT ServiceStatusID FROM @SSMC WHERE ServiceStatusModuleComponentID IS NULL AND ServiceStatusID IS NOT NULL

			OPEN @AuditCursor
			FETCH NEXT FROM @AuditCursor INTO @ServiceStatusID

			WHILE @@FETCH_STATUS = 0
			BEGIN
				INSERT INTO Reference.ServiceStatusModuleComponent (ModuleComponentID, ServiceStatusID, ServicesID, IsActive, ModifiedOn, ModifiedBy, CreatedOn, CreatedBy)
				VALUES (@ModuleComponentID, @ServiceStatusID, @ServicesID, 1, @ModifiedOn, @ModifiedBy, @ModifiedOn, @ModifiedBy);

				SELECT @ID = SCOPE_IDENTITY();

				EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Reference', 'ServiceStatusModuleComponent', @ID, NULL, NULL, NULL, NULL, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

				EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Reference', 'ServiceStatusModuleComponent', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
				FETCH NEXT FROM @AuditCursor INTO @ServiceStatusID
			END
			
			CLOSE @AuditCursor;
			DEALLOCATE @AuditCursor;
		END
	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
	END CATCH

END


