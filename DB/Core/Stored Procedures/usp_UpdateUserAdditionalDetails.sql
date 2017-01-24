-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[usp_UpdateUserAdditionalDetails]
-- Author:		Sumana Sangapu
-- Date:		04/09/2016
--
-- Purpose:		Updates User AdditionalDetails
--
-- Notes:		N/A
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/09/2016 - Sumana Sangapu Initial Creation 
-- 01/11/2017	Scott Martin	Updated the audit procs to save UserID
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_UpdateUserAdditionalDetails]
	@UserAddtionalDetailsXML xml,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @UserAdditionalDetailID  BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@UserID INT;

	SELECT @ResultCode = 0,
		   @ResultMessage = 'Executed Successfully';

	BEGIN TRY

			CREATE TABLE #tmpAD
			(	[UserAdditionalDetailID] [bigint],
				[UserID] [int] ,
				[ContractingEntity] [nvarchar](100),
				[IDNumber] [nvarchar](100) ,
				[EffectiveDate] [datetime] ,
				[ExpirationDate] [datetime] ,
				[IsActive] [bit],
				[ModifiedOn] [datetime],
				[ModifiedBy] [int],
				AuditDetailID BIGINT
			);

			INSERT INTO #tmpAD
			(
				UserAdditionalDetailID, UserID, ContractingEntity, IDNumber, EffectiveDate, ExpirationDate, IsActive, ModifiedOn,ModifiedBy, AuditDetailID 
			)
			SELECT
				T.C.value('UserAdditionalDetailID[1]','int') as UserAdditionalDetailID,
				T.C.value('UserID[1]','int') as UserID,
				T.C.value('ContractingEntity[1]','nvarchar(100)') as ContractingEntity,
				T.C.value('IDNumber[1]','NVarchar(100)') as IDNumber,
				T.C.value('EffectiveDate[1]','Date') as EffectiveDate,
				T.C.value('ExpirationDate[1]','Date') as ExpirationDate,
				T.C.value('IsActive[1]','BIT') as IsActive,
				@ModifiedOn as ModifiedOn,
				@ModifiedBy as ModifiedBy,
				NULL
			FROM @UserAddtionalDetailsXML.nodes('UserAdditionalDetails/UserAdditionalDetails') AS T(C);

			DECLARE @AuditCursor CURSOR;
			DECLARE @AuditDetailID BIGINT;
			BEGIN
				SET @AuditCursor = CURSOR FOR
				SELECT UserAdditionalDetailID, ModifiedOn, UserID FROM #tmpAD;    

				OPEN @AuditCursor 
				FETCH NEXT FROM @AuditCursor 
				INTO @UserAdditionalDetailID, @ModifiedOn, @UserID

				WHILE @@FETCH_STATUS = 0
				BEGIN
				EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'UserAdditionalDetails', @UserAdditionalDetailID, NULL, NULL, NULL, @UserID, 1, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

				UPDATE	#tmpAD
				SET		AuditDetailID = @AuditDetailID
				WHERE	UserAdditionalDetailID = @UserAdditionalDetailID;

				FETCH NEXT FROM @AuditCursor 
				INTO @UserAdditionalDetailID, @ModifiedOn, @UserID
				END; 

				CLOSE @AuditCursor;
				DEALLOCATE @AuditCursor;
			END;

			UPDATE  a
			SET		
					ContractingEntity = t.ContractingEntity,
					IDNumber = t.IDNumber,
					EffectiveDate = t.EffectiveDate,
					ExpirationDate = t.ExpirationDate,
					IsActive = t.IsActive,
					ModifiedBy = t.ModifiedBy,
					ModifiedOn = t.ModifiedOn,
					SystemModifiedOn = GETUTCDATE()
			FROM	[Core].[UserAdditionalDetails]   a
			INNER JOIN #tmpAD t
			ON		a.UserAdditionalDetailID = t.UserAdditionalDetailID
			AND		a.UserID = t.UserID

			BEGIN
				SET @AuditCursor = CURSOR FOR
				SELECT UserAdditionalDetailID, ModifiedOn, AuditDetailID FROM #tmpAD;    

				OPEN @AuditCursor 
				FETCH NEXT FROM @AuditCursor 
				INTO @UserAdditionalDetailID, @ModifiedOn, @AuditDetailID

				WHILE @@FETCH_STATUS = 0
				BEGIN
				EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'UserAdditionalDetails', @AuditDetailID, @UserAdditionalDetailID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

				FETCH NEXT FROM @AuditCursor 
				INTO @UserAdditionalDetailID, @ModifiedOn, @AuditDetailID
				END; 

				CLOSE @AuditCursor;
				DEALLOCATE @AuditCursor;
			END;
		
			IF OBJECT_ID('tempdb..#tmpAD') IS NOT NULL
				DROP TABLE #tmpAD

	END TRY

	BEGIN CATCH
		SELECT 
				@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END