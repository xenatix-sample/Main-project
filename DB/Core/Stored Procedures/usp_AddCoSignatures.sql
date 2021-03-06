----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[usp_AddCoSignatures]
-- Author:		Sumana Sangapu
-- Date:		04/08/2016
--
-- Purpose:		Insert the CoSignatures details 
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/08/2016	Sumana Sangapu    Initial creation. 
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_AddCoSignatures]
	@CoSignaturesXML xml,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS

BEGIN
	DECLARE @AuditDetailID BIGINT,
			@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
			@ID BIGINT

	SELECT	@ResultCode = 0,
			@ResultMessage = 'Executed Successfully'

	BEGIN TRY
	DECLARE @CS_ID TABLE (ID INT, ModifiedOn DATETIME);
				
			INSERT INTO [Core].[CoSignatures]
			( 
			 UserID, CoSigneeID, DocumentTypeGroupID, EffectiveDate, ExpirationDate, IsActive, ModifiedBy, ModifiedOn, CreatedBy, CreatedOn
			)
			OUTPUT inserted.UserID, inserted.ModifiedOn
			INTO @CS_ID	
			SELECT
				T.C.value('UserID[1]','int') as UserID,
				T.C.value('CoSigneeID[1]','Int') as CoSigneeID,
				T.C.value('DocumentTypeGroupID[1]','Int') as DocumentTypeGroupID,
				T.C.value('EffectiveDate[1]','Date') as EffectiveDate,
				T.C.value('ExpirationDate[1]','Date') as ExpirationDate,
				T.C.value('IsActive[1]','BIT') as IsActive,
				@ModifiedBy,
				@ModifiedOn as ModifiedOn,
				@ModifiedBy as CreatedBy, 
				@ModifiedOn as CreatedOn
			FROM @CoSignaturesXML.nodes('CoSignatures/CoSignatures') AS T(C);

			DECLARE @AuditCursor CURSOR;
			BEGIN
				SET @AuditCursor = CURSOR FOR
				SELECT ID, ModifiedOn FROM @CS_ID;    

				OPEN @AuditCursor 
				FETCH NEXT FROM @AuditCursor 
				INTO @ID, @ModifiedOn

				WHILE @@FETCH_STATUS = 0
				BEGIN
					EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Core', 'CoSignatures', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			
					EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Core', 'CoSignatures', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		
				FETCH NEXT FROM @AuditCursor 
				INTO @ID, @ModifiedOn
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