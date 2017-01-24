-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[usp_SaveOrganizationDetailsModuleComponent]
-- Author:		Scott Martin
-- Date:		01/15/2017
--
-- Purpose:		Saves mapping for Organization Detail and Module Component
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/15/2017	Scott Martin	Initial Creation
-- 01/16/2017	Scott Martin	Added node if it doesn't exist in the xml
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_SaveOrganizationDetailsModuleComponent]	
	@ModuleComponentXMLValue XML,	
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
				@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
				@NodeExists BIT;

		SELECT @ResultCode = 0;
		SELECT @ResultMessage = 'Executed successfully';

		SET @NodeExists = @ModuleComponentXMLValue.exist('/ModuleComponentXMLValue/ModuleComponent')

		IF @NodeExists = 0
			BEGIN
			SET @ModuleComponentXMLValue.modify('insert <ModuleComponent /> into /ModuleComponentXMLValue[1]')
			END

		DECLARE @ODMC TABLE
		(
			OrganizationDetailsModuleComponentID BIGINT,
			ModuleComponentID BIGINT,
			DetailID BIGINT,
			IsActive BIT
		);

		DECLARE @DetailID BIGINT;

		SELECT 
			@DetailID = T.C.value('DetailID[1]', 'BIGINT')
		FROM @ModuleComponentXMLValue.nodes('ModuleComponentXMLValue/ModuleComponent') AS T(C);

		INSERT INTO @ODMC (OrganizationDetailsModuleComponentID, ModuleComponentID, DetailID, IsActive)
		SELECT 
			ODMC.OrganizationDetailsModuleComponentID,
			T.C.value('ModuleComponentID[1]', 'BIGINT') AS ModuleComponentID,
			T.C.value('DetailID[1]', 'BIGINT') AS DetailID,
			ODMC.IsActive
		FROM @ModuleComponentXMLValue.nodes('ModuleComponentXMLValue/ModuleComponent') AS T(C)
			FULL OUTER JOIN (SELECT * FROM Core.OrganizationDetailsModuleComponent WHERE DetailID = @DetailID) AS ODMC
			ON T.C.value('ModuleComponentID[1]','BIGINT') = ODMC.ModuleComponentID
		
		IF EXISTS (SELECT TOP 1 * FROM @ODMC WHERE OrganizationDetailsModuleComponentID IS NOT NULL)
		BEGIN
			DECLARE @MC_ID INT,
				@IsActive BIT;

			SET @AuditCursor = CURSOR FOR
				SELECT OrganizationDetailsModuleComponentID, ModuleComponentID, IsActive FROM @ODMC WHERE OrganizationDetailsModuleComponentID IS NOT NULL
			
			OPEN @AuditCursor			
			FETCH NEXT FROM @AuditCursor INTO @ID, @MC_ID, @IsActive

			WHILE @@FETCH_STATUS = 0				
			BEGIN
				
				--Prevent matching records from updating
				IF (@MC_ID IS NOT NULL AND @IsActive = 0)
					BEGIN
						EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'OrganizationDetailsModuleComponent', @ID, NULL, NULL, NULL, NULL, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
								
						UPDATE Core.OrganizationDetailsModuleComponent SET IsActive = 1, ModifiedOn = @ModifiedOn, ModifiedBy = @ModifiedBy, SystemModifiedOn = GETUTCDATE() WHERE OrganizationDetailsModuleComponentID = @ID

						EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'OrganizationDetailsModuleComponent', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
					END
				ELSE IF (@MC_ID IS NULL AND @IsActive = 1)
					BEGIN
						EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Core', 'OrganizationDetailsModuleComponent', @ID, NULL, NULL, NULL, NULL, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
								
						UPDATE Core.OrganizationDetailsModuleComponent SET IsActive = 0, ModifiedOn = @ModifiedOn, ModifiedBy = @ModifiedBy, SystemModifiedOn = GETUTCDATE() WHERE OrganizationDetailsModuleComponentID = @ID

						EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Core', 'OrganizationDetailsModuleComponent', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
					END									
				FETCH NEXT FROM @AuditCursor INTO @ID, @MC_ID, @IsActive
			END;

			CLOSE @AuditCursor;
			DEALLOCATE @AuditCursor;			
		END

		IF EXISTS (SELECT TOP 1 * FROM @ODMC WHERE OrganizationDetailsModuleComponentID IS NULL AND ModuleComponentID IS NOT NULL)
		BEGIN
			DECLARE @ModuleComponentID INT;
			
			SET @AuditCursor = CURSOR FOR
				SELECT ModuleComponentID FROM @ODMC WHERE OrganizationDetailsModuleComponentID IS NULL AND ModuleComponentID IS NOT NULL

			OPEN @AuditCursor
			FETCH NEXT FROM @AuditCursor INTO @ModuleComponentID

			WHILE @@FETCH_STATUS = 0
			BEGIN
				INSERT INTO Core.OrganizationDetailsModuleComponent (ModuleComponentID, DetailID, IsActive, ModifiedOn, ModifiedBy, CreatedOn, CreatedBy)
				VALUES (@ModuleComponentID, @DetailID, 1, @ModifiedOn, @ModifiedBy, @ModifiedOn, @ModifiedBy);

				SELECT @ID = SCOPE_IDENTITY();

				EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Core', 'OrganizationDetailsModuleComponent', @ID, NULL, NULL, NULL, NULL, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

				EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Core', 'OrganizationDetailsModuleComponent', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
				FETCH NEXT FROM @AuditCursor INTO @ModuleComponentID
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