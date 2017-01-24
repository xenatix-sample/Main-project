-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Reference].[usp_SaveRecipientCodeModuleComponent]
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
CREATE PROCEDURE [Reference].[usp_SaveRecipientCodeModuleComponent]
	@ServicesID INT,
	@ModuleComponentID INT,
	@RecipientCodeXMLValue XML,	
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

		DECLARE @RCMC TABLE
		(
			RecipientCodeModuleComponentID BIGINT,
			ModuleComponentID BIGINT,
			RecipientCodeID SMALLINT,
			ServicesID INT,
			IsActive BIT
		);

		INSERT INTO @RCMC (RecipientCodeModuleComponentID, ModuleComponentID, RecipientCodeID, ServicesID, IsActive)
		SELECT 
			DMMC.RecipientCodeModuleComponentID,
			@ModuleComponentID AS ModuleComponentID,
			T.C.value('CodeID[1]', 'SMALLINT') AS RecipientCodeID,
			@ServicesID AS ServicesID,
			DMMC.IsActive
		FROM @RecipientCodeXMLValue.nodes('RecipientCodeXMLValue/RecipientCode') AS T(C)
			FULL OUTER JOIN (SELECT * FROM Reference.RecipientCodeModuleComponent WHERE ModuleComponentID = @ModuleComponentID AND ServicesID = @ServicesID) AS DMMC
			ON T.C.value('RecipientCodeID[1]','BIGINT') = DMMC.RecipientCodeID
		
		IF EXISTS (SELECT TOP 1 * FROM @RCMC WHERE RecipientCodeModuleComponentID IS NOT NULL)
		BEGIN
			DECLARE @RC_ID INT,
				@IsActive BIT;

			SET @AuditCursor = CURSOR FOR
				SELECT RecipientCodeModuleComponentID, RecipientCodeID, IsActive FROM @RCMC WHERE RecipientCodeModuleComponentID IS NOT NULL
			
			OPEN @AuditCursor			
			FETCH NEXT FROM @AuditCursor INTO @ID, @RC_ID, @IsActive

			WHILE @@FETCH_STATUS = 0				
			BEGIN
				
				--Prevent matching records from updating
				IF (@RC_ID IS NOT NULL AND @IsActive = 0)
					BEGIN
						EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Reference', 'RecipientCodeModuleComponent', @ID, NULL, NULL, NULL, NULL, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
								
						UPDATE Reference.RecipientCodeModuleComponent SET IsActive = 1, ModifiedOn = @ModifiedOn, ModifiedBy = @ModifiedBy, SystemModifiedOn = GETUTCDATE() WHERE RecipientCodeModuleComponentID = @ID

						EXEC Auditing.usp_AddPostAuditLog 'Update', 'Reference', 'RecipientCodeModuleComponent', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
					END
				ELSE IF (@RC_ID IS NULL AND @IsActive = 1)
					BEGIN
						EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Reference', 'RecipientCodeModuleComponent', @ID, NULL, NULL, NULL, NULL, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
								
						UPDATE Reference.RecipientCodeModuleComponent SET IsActive = 0, ModifiedOn = @ModifiedOn, ModifiedBy = @ModifiedBy, SystemModifiedOn = GETUTCDATE() WHERE RecipientCodeModuleComponentID = @ID

						EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Reference', 'RecipientCodeModuleComponent', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
					END	
				FETCH NEXT FROM @AuditCursor INTO @ID, @RC_ID, @IsActive
			END;

			CLOSE @AuditCursor;
			DEALLOCATE @AuditCursor;			
		END

		IF EXISTS (SELECT TOP 1 * FROM @RCMC WHERE RecipientCodeModuleComponentID IS NULL AND RecipientCodeID IS NOT NULL)
		BEGIN
			DECLARE @RecipientCodeID INT;

			SET @AuditCursor = CURSOR FOR
				SELECT RecipientCodeID FROM @RCMC WHERE RecipientCodeModuleComponentID IS NULL AND RecipientCodeID IS NOT NULL

			OPEN @AuditCursor
			FETCH NEXT FROM @AuditCursor INTO @RecipientCodeID

			WHILE @@FETCH_STATUS = 0
			BEGIN
				INSERT INTO Reference.RecipientCodeModuleComponent (ModuleComponentID, RecipientCodeID, ServicesID, IsActive, ModifiedOn, ModifiedBy, CreatedOn, CreatedBy)
				VALUES (@ModuleComponentID, @RecipientCodeID, @ServicesID, 1, @ModifiedOn, @ModifiedBy, @ModifiedOn, @ModifiedBy);

				SELECT @ID = SCOPE_IDENTITY();

				EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Reference', 'RecipientCodeModuleComponent', @ID, NULL, NULL, NULL, NULL, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

				EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Reference', 'RecipientCodeModuleComponent', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
				FETCH NEXT FROM @AuditCursor INTO @RecipientCodeID
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


