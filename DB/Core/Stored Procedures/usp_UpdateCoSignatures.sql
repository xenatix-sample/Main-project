
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[usp_UpdateUserCoSignatures]
-- Author:		Sumana Sangapu
-- Date:		04/09/2016
--
-- Purpose:		Updates User CoSignatures
--
-- Notes:		N/A
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/09/2016 - Sumana Sangapu	Initial Creation 
-- 04/13/2016	Sumana Sangapu	Interchanged the ModifiedBy and ModifiedOn fields
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_UpdateCoSignatures]
	@CoSignaturesXML xml,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @CoSignatureID  BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT @ResultCode = 0,
		   @ResultMessage = 'Executed Successfully';

	BEGIN TRY

			CREATE TABLE #tmpCS
			(	[CoSignatureID] bigint,
				[UserID] int ,
				[CoSigneeID] int ,
				[DocumentTypeGroupID] int ,
				[EffectiveDate] date,
				[ExpirationDate] date,
				[IsActive] bit ,
				[ModifiedOn] datetime,
				[ModifiedBy] int,
				AuditDetailID BIGINT
			);

			INSERT INTO #tmpCS
			(
				CoSignatureID, UserID, CoSigneeID, DocumentTypeGroupID, EffectiveDate, ExpirationDate, IsActive,ModifiedOn, ModifiedBy,  AuditDetailID 
			)
			SELECT
				T.C.value('CoSignatureID[1]','int') as CoSignatureID,
				T.C.value('UserID[1]','int') as UserID,
				T.C.value('CoSigneeID[1]','Int') as CoSigneeID,
				T.C.value('DocumentTypeGroupID[1]','Int') as DocumentTypeGroupID,
				T.C.value('EffectiveDate[1]','Date') as EffectiveDate,
				T.C.value('ExpirationDate[1]','Date') as ExpirationDate,
				T.C.value('IsActive[1]','BIT') as IsActive,
				@ModifiedOn as ModifiedOn,
				@ModifiedBy as ModifiedBy,
				NULL
			FROM @CoSignaturesXML.nodes('CoSignatures/CoSignatures') AS T(C);

			DECLARE @AuditCursor CURSOR;
			DECLARE @AuditDetailID BIGINT;
			BEGIN
				SET @AuditCursor = CURSOR FOR
				SELECT CoSignatureID, ModifiedOn FROM #tmpCS;    

				OPEN @AuditCursor 
				FETCH NEXT FROM @AuditCursor 
				INTO @CoSignatureID, @ModifiedOn

				WHILE @@FETCH_STATUS = 0
				BEGIN
				EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'CoSignatures', @CoSignatureID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

				UPDATE	#tmpCS
				SET		AuditDetailID = @AuditDetailID
				WHERE	CoSignatureID = @CoSignatureID;

				FETCH NEXT FROM @AuditCursor 
				INTO @CoSignatureID, @ModifiedOn
				END; 

				CLOSE @AuditCursor;
				DEALLOCATE @AuditCursor;
			END;

			UPDATE  c
			SET		CoSigneeID  = t.CoSigneeID,
					EffectiveDate = t.EffectiveDate,
					ExpirationDate = t.ExpirationDate,
					IsActive = t.IsActive,
					ModifiedBy = t.ModifiedBy,
					ModifiedOn = t.ModifiedOn,
					SystemModifiedOn = GETUTCDATE()
			FROM	[Core].[CoSignatures]  c
			INNER JOIN #tmpCS t
			ON		c.CoSignatureID = t.CoSignatureID 
			AND		c.UserID = t.UserID

			BEGIN
				SET @AuditCursor = CURSOR FOR
				SELECT CoSignatureID, ModifiedOn, AuditDetailID FROM #tmpCS;    

				OPEN @AuditCursor 
				FETCH NEXT FROM @AuditCursor 
				INTO @CoSignatureID, @ModifiedOn, @AuditDetailID

				WHILE @@FETCH_STATUS = 0
				BEGIN
				EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'CoSignatures', @AuditDetailID, @CoSignatureID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

				FETCH NEXT FROM @AuditCursor 
				INTO @CoSignatureID, @ModifiedOn, @AuditDetailID
				END; 

				CLOSE @AuditCursor;
				DEALLOCATE @AuditCursor;
			END;
		
			IF OBJECT_ID('tempdb..#tmpCS') IS NOT NULL
				DROP TABLE #tmpCS

	END TRY

	BEGIN CATCH
		SELECT 
				@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END