-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Reference].[usp_SaveServiceLocationModuleComponent]
-- Author:		Kyle Campbell
-- Date:		01/05/2017
--
-- Purpose:		Saves mapping for Service Location and ModuleComponent
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/05/2017	Kyle Campbell	TFS #14007	Initial Creation
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Reference].[usp_SaveServiceLocationModuleComponent]
	@ServicesID INT,
	@ModuleComponentID INT,
	@ServiceLocationXMLValue XML,	
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

		DECLARE @SLMC TABLE
		(
			ServiceLocationModuleComponentID BIGINT,
			ModuleComponentID BIGINT,
			ServiceLocationID SMALLINT,
			ServicesID INT,
			IsActive BIT
		);

		INSERT INTO @SLMC (ServiceLocationModuleComponentID, ModuleComponentID, ServiceLocationID, ServicesID, IsActive)
		SELECT 
			SLMC.ServiceLocationModuleComponentID,
			@ModuleComponentID AS ModuleComponentID,
			T.C.value('ServiceLocationID[1]', 'SMALLINT') AS ServiceLocationID,
			@ServicesID AS ServicesID,
			SLMC.IsActive
		FROM @ServiceLocationXMLValue.nodes('ServiceLocationXMLValue/ServiceLocation') AS T(C)
			FULL OUTER JOIN (SELECT * FROM Reference.ServiceLocationModuleComponent WHERE ModuleComponentID = @ModuleComponentID AND ServicesID = @ServicesID) AS SLMC
			ON T.C.value('ServiceLocationID[1]','BIGINT') = SLMC.ServiceLocationID
		
		IF EXISTS (SELECT TOP 1 * FROM @SLMC WHERE ServiceLocationModuleComponentID IS NOT NULL)
		BEGIN
			DECLARE @SL_ID INT,
				@IsActive BIT;

			SET @AuditCursor = CURSOR FOR
				SELECT ServiceLocationModuleComponentID, ServiceLocationID, IsActive FROM @SLMC WHERE ServiceLocationModuleComponentID IS NOT NULL
			
			OPEN @AuditCursor			
			FETCH NEXT FROM @AuditCursor INTO @ID, @SL_ID, @IsActive

			WHILE @@FETCH_STATUS = 0				
			BEGIN
				
				--Prevent matching records from updating
				IF (@SL_ID IS NOT NULL AND @IsActive = 0)
					BEGIN
						EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Reference', 'ServiceLocationModuleComponent', @ID, NULL, NULL, NULL, NULL, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
								
						UPDATE Reference.ServiceLocationModuleComponent SET IsActive = 1, ModifiedOn = @ModifiedOn, ModifiedBy = @ModifiedBy, SystemModifiedOn = GETUTCDATE() WHERE ServiceLocationModuleComponentID = @ID

						EXEC Auditing.usp_AddPostAuditLog 'Update', 'Reference', 'ServiceLocationModuleComponent', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
					END
				ELSE IF (@SL_ID IS NULL AND @IsActive = 1)
					BEGIN
						EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Reference', 'ServiceLocationModuleComponent', @ID, NULL, NULL, NULL, NULL, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
								
						UPDATE Reference.ServiceLocationModuleComponent SET IsActive = 0, ModifiedOn = @ModifiedOn, ModifiedBy = @ModifiedBy, SystemModifiedOn = GETUTCDATE() WHERE ServiceLocationModuleComponentID = @ID

						EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Reference', 'ServiceLocationModuleComponent', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
					END			
				FETCH NEXT FROM @AuditCursor INTO @ID, @SL_ID, @IsActive
			END;

			CLOSE @AuditCursor;
			DEALLOCATE @AuditCursor;			
		END

		IF EXISTS (SELECT TOP 1 * FROM @SLMC WHERE ServiceLocationModuleComponentID IS NULL AND ServiceLocationID IS NOT NULL)
		BEGIN
			DECLARE @ServiceLocationID INT;

			SET @AuditCursor = CURSOR FOR
				SELECT ServiceLocationID FROM @SLMC WHERE ServiceLocationModuleComponentID IS NULL AND ServiceLocationID IS NOT NULL

			OPEN @AuditCursor
			FETCH NEXT FROM @AuditCursor INTO @ServiceLocationID

			WHILE @@FETCH_STATUS = 0
			BEGIN
				INSERT INTO Reference.ServiceLocationModuleComponent (ModuleComponentID, ServiceLocationID, ServicesID, IsActive, ModifiedOn, ModifiedBy, CreatedOn, CreatedBy)
				VALUES (@ModuleComponentID, @ServiceLocationID, @ServicesID, 1, @ModifiedOn, @ModifiedBy, @ModifiedOn, @ModifiedBy);

				SELECT @ID = SCOPE_IDENTITY();

				EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Reference', 'ServiceLocationModuleComponent', @ID, NULL, NULL, NULL, NULL, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

				EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Reference', 'ServiceLocationModuleComponent', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
				FETCH NEXT FROM @AuditCursor INTO @ServiceLocationID
			END
			
			CLOSE @AuditCursor;
			DEALLOCATE @AuditCursor;
		END
	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
			   @ResultMessage = ERROR_MESSAGE()
		SELECT @ResultMessage
	END CATCH

END


