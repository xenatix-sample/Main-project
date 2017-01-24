-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Reference].[usp_SaveTrackingFieldModuleComponent]
-- Author:		Kyle Campbell
-- Date:		01/05/2017
--
-- Purpose:		Saves mapping for TrackingField and ModuleComponent
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/05/2017	Kyle Campbell	TFS #14007	Initial Creation
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Reference].[usp_SaveTrackingFieldModuleComponent]
	@ServicesID INT,
	@ModuleComponentID INT,
	@TrackingFieldXMLValue XML,	
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

		DECLARE @TFMC TABLE
		(
			TrackingFieldModuleComponentID BIGINT,
			ModuleComponentID BIGINT,
			TrackingFieldID SMALLINT,
			ServicesID INT,
			IsActive BIT
		);

		INSERT INTO @TFMC (TrackingFieldModuleComponentID, ModuleComponentID, TrackingFieldID, ServicesID, IsActive)
		SELECT 
			TFMC.TrackingFieldModuleComponentID,
			@ModuleComponentID AS ModuleComponentID,
			T.C.value('TrackingFieldID[1]', 'SMALLINT') AS TrackingFieldID,
			@ServicesID AS ServicesID,
			TFMC.IsActive
		FROM @TrackingFieldXMLValue.nodes('TrackingFieldXMLValue/TrackingField') AS T(C)
			FULL OUTER JOIN (SELECT * FROM Reference.TrackingFieldModuleComponent WHERE ModuleComponentID = @ModuleComponentID AND ServicesID = @ServicesID) AS TFMC
			ON T.C.value('TrackingFieldID[1]','BIGINT') = TFMC.TrackingFieldID
		
		IF EXISTS (SELECT TOP 1 * FROM @TFMC WHERE TrackingFieldModuleComponentID IS NOT NULL)
		BEGIN
			DECLARE @TF_ID INT,
				@IsActive BIT;

			SET @AuditCursor = CURSOR FOR
				SELECT TrackingFieldModuleComponentID, TrackingFieldID, IsActive FROM @TFMC WHERE TrackingFieldModuleComponentID IS NOT NULL
			
			OPEN @AuditCursor			
			FETCH NEXT FROM @AuditCursor INTO @ID, @TF_ID, @IsActive

			WHILE @@FETCH_STATUS = 0				
			BEGIN
				
				--Prevent matching records from updating
				IF (@TF_ID IS NOT NULL AND @IsActive = 0)
					BEGIN
						EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Reference', 'TrackingFieldModuleComponent', @ID, NULL, NULL, NULL, NULL, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
								
						UPDATE Reference.TrackingFieldModuleComponent SET IsActive = 1, ModifiedOn = @ModifiedOn, ModifiedBy = @ModifiedBy, SystemModifiedOn = GETUTCDATE() WHERE TrackingFieldModuleComponentID = @ID

						EXEC Auditing.usp_AddPostAuditLog 'Update', 'Reference', 'TrackingFieldModuleComponent', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
					END
				ELSE IF (@TF_ID IS NULL AND @IsActive = 1)
					BEGIN
						EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Reference', 'TrackingFieldModuleComponent', @ID, NULL, NULL, NULL, NULL, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
								
						UPDATE Reference.TrackingFieldModuleComponent SET IsActive = 0, ModifiedOn = @ModifiedOn, ModifiedBy = @ModifiedBy, SystemModifiedOn = GETUTCDATE() WHERE TrackingFieldModuleComponentID = @ID

						EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Reference', 'TrackingFieldModuleComponent', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
					END									
				FETCH NEXT FROM @AuditCursor INTO @ID, @TF_ID, @IsActive
			END;

			CLOSE @AuditCursor;
			DEALLOCATE @AuditCursor;			
		END

		IF EXISTS (SELECT TOP 1 * FROM @TFMC WHERE TrackingFieldModuleComponentID IS NULL AND TrackingFieldID IS NOT NULL)
		BEGIN
			DECLARE @TrackingFieldID INT;

			SET @AuditCursor = CURSOR FOR
				SELECT TrackingFieldID FROM @TFMC WHERE TrackingFieldModuleComponentID IS NULL AND TrackingFieldID IS NOT NULL

			OPEN @AuditCursor
			FETCH NEXT FROM @AuditCursor INTO @TrackingFieldID

			WHILE @@FETCH_STATUS = 0
			BEGIN
				INSERT INTO Reference.TrackingFieldModuleComponent (ModuleComponentID, TrackingFieldID, ServicesID, IsActive, ModifiedOn, ModifiedBy, CreatedOn, CreatedBy)
				VALUES (@ModuleComponentID, @TrackingFieldID, @ServicesID, 1, @ModifiedOn, @ModifiedBy, @ModifiedOn, @ModifiedBy);

				SELECT @ID = SCOPE_IDENTITY();

				EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Reference', 'TrackingFieldModuleComponent', @ID, NULL, NULL, NULL, NULL, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

				EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Reference', 'TrackingFieldModuleComponent', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
				FETCH NEXT FROM @AuditCursor INTO @TrackingFieldID
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


