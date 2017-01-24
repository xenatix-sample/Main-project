-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Reference].[usp_SaveCredentialModuleComponent]
-- Author:		Kyle Campbell
-- Date:		01/05/2017
--
-- Purpose:		Saves mapping for Credential and ModuleComponent
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/05/2017	Kyle Campbell	TFS #14007	Initial Creation
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Reference].[usp_SaveCredentialModuleComponent]
	@ServicesID INT,
	@ModuleComponentID INT,
	@CredentialXMLValue XML,	
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

		DECLARE @CMC TABLE
		(
			CredentialModuleComponentID BIGINT,
			ModuleComponentID BIGINT,
			CredentialID SMALLINT,
			ServicesID INT,
			IsActive BIT
		);

		INSERT INTO @CMC (CredentialModuleComponentID, ModuleComponentID, CredentialID, ServicesID, IsActive)
		SELECT 
			CMC.CredentialModuleComponentID,
			@ModuleComponentID AS ModuleComponentID,
			T.C.value('CredentialID[1]', 'SMALLINT') AS CredentialID,
			@ServicesID AS ServicesID,
			CMC.IsActive
		FROM @CredentialXMLValue.nodes('CredentialXMLValue/Credential') AS T(C)
			FULL OUTER JOIN (SELECT * FROM Reference.CredentialModuleComponent WHERE ModuleComponentID = @ModuleComponentID AND ServicesID = @ServicesID) AS CMC
			ON T.C.value('CredentialID[1]','BIGINT') = CMC.CredentialID
		
		IF EXISTS (SELECT TOP 1 * FROM @CMC WHERE CredentialModuleComponentID IS NOT NULL)
		BEGIN
			DECLARE @C_ID INT,
				@IsActive BIT;

			SET @AuditCursor = CURSOR FOR
				SELECT CredentialModuleComponentID, CredentialID, IsActive FROM @CMC WHERE CredentialModuleComponentID IS NOT NULL
			
			OPEN @AuditCursor			
			FETCH NEXT FROM @AuditCursor INTO @ID, @C_ID, @IsActive

			WHILE @@FETCH_STATUS = 0				
			BEGIN
				
				--Prevent matching records from updating
				IF (@C_ID IS NOT NULL AND @IsActive = 0)
					BEGIN
						EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Reference', 'CredentialModuleComponent', @ID, NULL, NULL, NULL, NULL, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
								
						UPDATE Reference.CredentialModuleComponent SET IsActive = 1, ModifiedOn = @ModifiedOn, ModifiedBy = @ModifiedBy, SystemModifiedOn = GETUTCDATE() WHERE CredentialModuleComponentID = @ID

						EXEC Auditing.usp_AddPostAuditLog 'Update', 'Reference', 'CredentialModuleComponent', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
					END
				ELSE IF (@C_ID IS NULL AND @IsActive = 1)
					BEGIN
						EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Reference', 'CredentialModuleComponent', @ID, NULL, NULL, NULL, NULL, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
								
						UPDATE Reference.CredentialModuleComponent SET IsActive = 0, ModifiedOn = @ModifiedOn, ModifiedBy = @ModifiedBy, SystemModifiedOn = GETUTCDATE() WHERE CredentialModuleComponentID = @ID

						EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Reference', 'CredentialModuleComponent', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
					END									
				FETCH NEXT FROM @AuditCursor INTO @ID, @C_ID, @IsActive
			END;

			CLOSE @AuditCursor;
			DEALLOCATE @AuditCursor;			
		END

		IF EXISTS (SELECT TOP 1 * FROM @CMC WHERE CredentialModuleComponentID IS NULL AND CredentialID IS NOT NULL)
		BEGIN
			DECLARE @CredentialID INT;

			SET @AuditCursor = CURSOR FOR
				SELECT CredentialID FROM @CMC WHERE CredentialModuleComponentID IS NULL AND CredentialID IS NOT NULL

			OPEN @AuditCursor
			FETCH NEXT FROM @AuditCursor INTO @CredentialID

			WHILE @@FETCH_STATUS = 0
			BEGIN
				INSERT INTO Reference.CredentialModuleComponent (ModuleComponentID, CredentialID, ServicesID, IsActive, ModifiedOn, ModifiedBy, CreatedOn, CreatedBy)
				VALUES (@ModuleComponentID, @CredentialID, @ServicesID, 1, @ModifiedOn, @ModifiedBy, @ModifiedOn, @ModifiedBy);

				SELECT @ID = SCOPE_IDENTITY();

				EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Reference', 'CredentialModuleComponent', @ID, NULL, NULL, NULL, NULL, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

				EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Reference', 'CredentialModuleComponent', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
				FETCH NEXT FROM @AuditCursor INTO @CredentialID
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


