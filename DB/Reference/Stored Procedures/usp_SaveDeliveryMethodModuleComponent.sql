-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Reference].[usp_SaveDeliveryMethodModuleComponent]
-- Author:		Kyle Campbell
-- Date:		01/05/2017
--
-- Purpose:		Saves mapping for Delivery Method and ModuleComponent
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/05/2017	Kyle Campbell	TFS #14007	Initial Creation
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Reference].[usp_SaveDeliveryMethodModuleComponent]
	@ServicesID INT,
	@ModuleComponentID INT,
	@DeliveryMethodXMLValue XML,	
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

		DECLARE @DMMC TABLE
		(
			DeliveryMethodModuleComponentID BIGINT,
			ModuleComponentID BIGINT,
			DeliveryMethodID SMALLINT,
			ServicesID INT,
			IsActive BIT
		);

		INSERT INTO @DMMC (DeliveryMethodModuleComponentID, ModuleComponentID, DeliveryMethodID, ServicesID, IsActive)
		SELECT 
			DMMC.DeliveryMethodModuleComponentID,
			@ModuleComponentID AS ModuleComponentID,
			T.C.value('DeliveryMethodID[1]', 'SMALLINT') AS DeliveryMethodID,
			@ServicesID AS ServicesID,
			DMMC.IsActive
		FROM @DeliveryMethodXMLValue.nodes('DeliveryMethodXMLValue/DeliveryMethod') AS T(C)
			FULL OUTER JOIN (SELECT * FROM Reference.DeliveryMethodModuleComponent WHERE ModuleComponentID = @ModuleComponentID AND ServicesID = @ServicesID) AS DMMC
			ON T.C.value('DeliveryMethodID[1]','BIGINT') = DMMC.DeliveryMethodID
		
		IF EXISTS (SELECT TOP 1 * FROM @DMMC WHERE DeliveryMethodModuleComponentID IS NOT NULL)
		BEGIN
			DECLARE @DM_ID INT,
				@IsActive BIT;

			SET @AuditCursor = CURSOR FOR
				SELECT DeliveryMethodModuleComponentID, DeliveryMethodID, IsActive FROM @DMMC WHERE DeliveryMethodModuleComponentID IS NOT NULL
			
			OPEN @AuditCursor			
			FETCH NEXT FROM @AuditCursor INTO @ID, @DM_ID, @IsActive

			WHILE @@FETCH_STATUS = 0				
			BEGIN
				
				--Prevent matching records from updating
				IF (@DM_ID IS NOT NULL AND @IsActive = 0)
					BEGIN
						EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Reference', 'DeliveryMethodModuleComponent', @ID, NULL, NULL, NULL, NULL, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
								
						UPDATE Reference.DeliveryMethodModuleComponent SET IsActive = 1, ModifiedOn = @ModifiedOn, ModifiedBy = @ModifiedBy, SystemModifiedOn = GETUTCDATE() WHERE DeliveryMethodModuleComponentID = @ID

						EXEC Auditing.usp_AddPostAuditLog 'Update', 'Reference', 'DeliveryMethodModuleComponent', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
					END
				ELSE IF (@DM_ID IS NULL AND @IsActive = 1)
					BEGIN
						EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Reference', 'DeliveryMethodModuleComponent', @ID, NULL, NULL, NULL, NULL, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
								
						UPDATE Reference.DeliveryMethodModuleComponent SET IsActive = 0, ModifiedOn = @ModifiedOn, ModifiedBy = @ModifiedBy, SystemModifiedOn = GETUTCDATE() WHERE DeliveryMethodModuleComponentID = @ID

						EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Reference', 'DeliveryMethodModuleComponent', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
					END									
				FETCH NEXT FROM @AuditCursor INTO @ID, @DM_ID, @IsActive
			END;

			CLOSE @AuditCursor;
			DEALLOCATE @AuditCursor;			
		END

		IF EXISTS (SELECT TOP 1 * FROM @DMMC WHERE DeliveryMethodModuleComponentID IS NULL AND DeliveryMethodID IS NOT NULL)
		BEGIN
			DECLARE @DeliveryMethodID INT;

			SET @AuditCursor = CURSOR FOR
				SELECT DeliveryMethodID FROM @DMMC WHERE DeliveryMethodModuleComponentID IS NULL AND DeliveryMethodID IS NOT NULL

			OPEN @AuditCursor
			FETCH NEXT FROM @AuditCursor INTO @DeliveryMethodID

			WHILE @@FETCH_STATUS = 0
			BEGIN
				INSERT INTO Reference.DeliveryMethodModuleComponent (ModuleComponentID, DeliveryMethodID, ServicesID, IsActive, ModifiedOn, ModifiedBy, CreatedOn, CreatedBy)
				VALUES (@ModuleComponentID, @DeliveryMethodID, @ServicesID, 1, @ModifiedOn, @ModifiedBy, @ModifiedOn, @ModifiedBy);

				SELECT @ID = SCOPE_IDENTITY();

				EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Reference', 'DeliveryMethodModuleComponent', @ID, NULL, NULL, NULL, NULL, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

				EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Reference', 'DeliveryMethodModuleComponent', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
				FETCH NEXT FROM @AuditCursor INTO @DeliveryMethodID
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


