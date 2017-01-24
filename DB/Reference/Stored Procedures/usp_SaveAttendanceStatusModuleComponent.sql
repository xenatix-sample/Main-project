-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Reference].[usp_SaveAttendanceStatusModuleComponent]
-- Author:		Kyle Campbell
-- Date:		01/05/2017
--
-- Purpose:		Saves mapping for AttendanceStatus and ModuleComponent
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 01/05/2017	Kyle Campbell	TFS #14007	Initial Creation
-----------------------------------------------------------------------------------------------------------------------
CREATE PROCEDURE [Reference].[usp_SaveAttendanceStatusModuleComponent]
	@ServicesID INT,
	@ModuleComponentID INT,
	@AttendanceStatusXMLValue XML,	
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

		DECLARE @ASMC TABLE
		(
			AttendanceStatusModuleComponentID BIGINT,
			ModuleComponentID BIGINT,
			AttendanceStatusID SMALLINT,
			ServicesID INT,
			IsActive BIT
		);

		INSERT INTO @ASMC (AttendanceStatusModuleComponentID, ModuleComponentID, AttendanceStatusID, ServicesID, IsActive)
		SELECT 
			ASMC.AttendanceStatusModuleComponentID,
			@ModuleComponentID AS ModuleComponentID,
			T.C.value('AttendanceStatusID[1]', 'SMALLINT') AS AttendanceStatusID,
			@ServicesID AS ServicesID,
			ASMC.IsActive
		FROM @AttendanceStatusXMLValue.nodes('AttendanceStatusXMLValue/AttendanceStatus') AS T(C)
			FULL OUTER JOIN (SELECT * FROM Reference.AttendanceStatusModuleComponent WHERE ModuleComponentID = @ModuleComponentID AND ServicesID = @ServicesID) AS ASMC
			ON T.C.value('AttendanceStatusID[1]','BIGINT') = ASMC.AttendanceStatusID
		
		IF EXISTS (SELECT TOP 1 * FROM @ASMC WHERE AttendanceStatusModuleComponentID IS NOT NULL)
		BEGIN
			DECLARE @AS_ID INT,
				@IsActive BIT;

			SET @AuditCursor = CURSOR FOR
				SELECT AttendanceStatusModuleComponentID, AttendanceStatusID, IsActive FROM @ASMC WHERE AttendanceStatusModuleComponentID IS NOT NULL
			
			OPEN @AuditCursor			
			FETCH NEXT FROM @AuditCursor INTO @ID, @AS_ID, @IsActive

			WHILE @@FETCH_STATUS = 0				
			BEGIN
				
				--Prevent matching records from updating
				IF (@AS_ID IS NOT NULL AND @IsActive = 0)
					BEGIN
						EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Reference', 'AttendanceStatusModuleComponent', @ID, NULL, NULL, NULL, NULL, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
								
						UPDATE Reference.AttendanceStatusModuleComponent SET IsActive = 1, ModifiedOn = @ModifiedOn, ModifiedBy = @ModifiedBy, SystemModifiedOn = GETUTCDATE() WHERE AttendanceStatusModuleComponentID = @ID

						EXEC Auditing.usp_AddPostAuditLog 'Update', 'Reference', 'AttendanceStatusModuleComponent', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
					END
				ELSE IF (@AS_ID IS NULL AND @IsActive = 1)
					BEGIN
						EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Inactivate', 'Reference', 'AttendanceStatusModuleComponent', @ID, NULL, NULL, NULL, NULL, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
								
						UPDATE Reference.AttendanceStatusModuleComponent SET IsActive = 0, ModifiedOn = @ModifiedOn, ModifiedBy = @ModifiedBy, SystemModifiedOn = GETUTCDATE() WHERE AttendanceStatusModuleComponentID = @ID

						EXEC Auditing.usp_AddPostAuditLog 'Inactivate', 'Reference', 'AttendanceStatusModuleComponent', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
					END									
				FETCH NEXT FROM @AuditCursor INTO @ID, @AS_ID, @IsActive
			END;

			CLOSE @AuditCursor;
			DEALLOCATE @AuditCursor;			
		END

		IF EXISTS (SELECT TOP 1 * FROM @ASMC WHERE AttendanceStatusModuleComponentID IS NULL AND AttendanceStatusID IS NOT NULL)
		BEGIN
			DECLARE @AttendanceStatusID INT;

			SET @AuditCursor = CURSOR FOR
				SELECT AttendanceStatusID FROM @ASMC WHERE AttendanceStatusModuleComponentID IS NULL AND AttendanceStatusID IS NOT NULL

			OPEN @AuditCursor
			FETCH NEXT FROM @AuditCursor INTO @AttendanceStatusID

			WHILE @@FETCH_STATUS = 0
			BEGIN
				INSERT INTO Reference.AttendanceStatusModuleComponent (ModuleComponentID, AttendanceStatusID, ServicesID, IsActive, ModifiedOn, ModifiedBy, CreatedOn, CreatedBy)
				VALUES (@ModuleComponentID, @AttendanceStatusID, @ServicesID, 1, @ModifiedOn, @ModifiedBy, @ModifiedOn, @ModifiedBy);

				SELECT @ID = SCOPE_IDENTITY();

				EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Reference', 'AttendanceStatusModuleComponent', @ID, NULL, NULL, NULL, NULL, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

				EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Reference', 'AttendanceStatusModuleComponent', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
				FETCH NEXT FROM @AuditCursor INTO @AttendanceStatusID
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


