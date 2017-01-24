----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[usp_AddUserAdditionalDetails]
-- Author:		Sumana Sangapu
-- Date:		04/08/2016
--
-- Purpose:		Insert the User Identifier details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/08/2016	Sumana Sangapu    Initial creation. 
-- 09/06/2016	Rahul Vats		Reviewed the Proc 
-- 01/11/2017	Scott Martin	Updated the audit procs to save UserID
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_AddUserAdditionalDetails]
	@UserAdditionalDetailsXML xml,
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
	DECLARE @UI_ID TABLE (ID BIGINT, ModifiedOn DATETIME, UserID INT);
				
			INSERT INTO [Core].[UserAdditionalDetails] 
			( 
			 UserID, ContractingEntity, IDNumber, EffectiveDate, ExpirationDate, IsActive, ModifiedBy, ModifiedOn, CreatedBy, CreatedOn 
			)
			OUTPUT inserted.UserAdditionalDetailID, inserted.ModifiedOn, inserted.UserID
			INTO @UI_ID	
			SELECT
				T.C.value('UserID[1]','int') as UserID,
				T.C.value('ContractingEntity[1]','nvarchar(100)') as ContractingEntity,
				T.C.value('IDNumber[1]','NVarchar(100)') as IDNumber,
				T.C.value('EffectiveDate[1]','Date') as EffectiveDate,
				T.C.value('ExpirationDate[1]','Date') as ExpirationDate,
				T.C.value('IsActive[1]','BIT') as IsActive,
				@ModifiedBy,
				@ModifiedOn as ModifiedOn,
				@ModifiedBy as CreatedBy, 
				@ModifiedOn as CreatedOn
			FROM @UserAdditionalDetailsXML.nodes('UserAdditionalDetails/UserAdditionalDetails') AS T(C);

			DECLARE @AuditCursor CURSOR;
			BEGIN
				SET @AuditCursor = CURSOR FOR
				SELECT ID, ModifiedOn, UserID FROM @UI_ID;    

				OPEN @AuditCursor 
				FETCH NEXT FROM @AuditCursor 
				INTO @ID, @ModifiedOn, @UserID

				WHILE @@FETCH_STATUS = 0
				BEGIN
					EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Core', 'UserAdditionalDetails', @ID, NULL, NULL, NULL, @UserID, 1, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			
					EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Core', 'UserAdditionalDetails', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
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