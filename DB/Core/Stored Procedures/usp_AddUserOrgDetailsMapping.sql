-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddUserOrgDetailsMapping]
-- Author:		Sumana Sangapu
-- Date:		03/29/2016
--
-- Purpose:		Insert the user organization details 
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 03/29/2016	Sumana Sangapu    Initial creation. 
-- 01/11/2017	Scott Martin	Updated the audit procs to save UserID
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_AddUserOrgDetailsMapping]
	@UserOrgDetailsXML xml,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS

BEGIN
	DECLARE @AuditDetailID BIGINT,
			@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
			@ID BIGINT,
			@UserID INT

	SELECT	@ResultCode = 0,
			@ResultMessage = 'Executed Successfully'

	BEGIN TRY
	DECLARE @UO_ID TABLE (ID BIGINT, ModifiedOn DATETIME, UserID INT);
				
			INSERT INTO [Core].[UserOrganizationDetailsMapping]
			( 
			UserID, MappingID, IsActive, ModifiedBy, ModifiedOn, CreatedBy, CreatedOn 
			)
			OUTPUT inserted.UserOrganizationDetailMappingID, inserted.ModifiedOn, inserted.UserID
			INTO @UO_ID	
			SELECT
				T.C.value('UserID[1]','Bigint') as UserID,
				T.C.value('MappingID[1]','Int') as MappingID,
				T.C.value('IsActive[1]','BIT') as IsActive,
				@ModifiedBy,
				@ModifiedOn as ModifiedOn,
				@ModifiedBy as CreatedBy, 
				@ModifiedOn as CreatedOn
			FROM @UserOrgDetailsXML.nodes('UserOrgDetails/UserOrgDetails') AS T(C);

			DECLARE @AuditCursor CURSOR;
			BEGIN
				SET @AuditCursor = CURSOR FOR
				SELECT ID, ModifiedOn, UserID FROM @UO_ID;    

				OPEN @AuditCursor 
				FETCH NEXT FROM @AuditCursor 
				INTO @ID, @ModifiedOn, @UserID

				WHILE @@FETCH_STATUS = 0
				BEGIN
					EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Core', 'UserOrganizationDetailsMapping', @ID, NULL, NULL, NULL, @UserID, 1, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			
					EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Core', 'UserOrganizationDetailsMapping', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
				FETCH NEXT FROM @AuditCursor 
				INTO @ID, @ModifiedOn, @UserID
				END; 

				CLOSE @AuditCursor;
				DEALLOCATE @AuditCursor;
			END;

	END TRY
	BEGIN CATCH
	SELECT
		@ResultCode = ERROR_SEVERITY(),
		@ResultMessage = ERROR_MESSAGE()
	END CATCH
END