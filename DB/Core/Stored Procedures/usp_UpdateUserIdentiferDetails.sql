-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[usp_UpdateUserIdentiferDetails]
-- Author:		Sumana Sangapu
-- Date:		04/09/2016
--
-- Purpose:		Updates User Identifier Details 
--
-- Notes:		N/A
--
-- Depends:		N/A
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 04/09/2016 - Sumana Sangapu Initial Creation 
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added SystemModifiedOn fields
-- 01/11/2017	Scott Martin	Added auditing
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_UpdateUserIdentiferDetails]
	@UserIdentifierXML xml,
	@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@Loop INT,
		@ID BIGINT,
		@UserID INT;

	SELECT @ResultCode = 0,
		   @ResultMessage = 'Executed Successfully';

	BEGIN TRY

		CREATE TABLE #tmpUI
		(	UserIdentifierDetailsID bigint, 
			UserID int , 
			UserIdentifierTypeID int, 
			IDNumber nvarchar(100), 
			EffectiveDate date, 
			ExpirationDate date, 
			IsActive bit, 
			ModifiedOn datetime, 
			ModifiedBy int,
			Completed BIT DEFAULT(0)
		);

		INSERT INTO #tmpUI
		(
			UserIdentifierDetailsID , 
			UserID  , 
			UserIdentifierTypeID , 
			IDNumber, 
			EffectiveDate , 
			ExpirationDate , 
			IsActive , 
			ModifiedOn , 
			ModifiedBy
		)
		SELECT 
			T.C.value('UserIdentifierDetailsID[1]', 'BIGINT'), 
			T.C.value('UserID[1]', 'INT'), 
			T.C.value('UserIdentifierTypeID[1]', 'INT'), 
			T.C.value('IDNumber[1]', 'NVARCHAR(100)'), 
			T.C.value('EffectiveDate[1]', 'DATE'), 
			T.C.value('ExpirationDate[1]', 'DATE'), 
			T.C.value('IsActive[1]', 'BIT'),
			@ModifiedOn as ModifiedOn,
			@ModifiedBy as ModifiedBy
		FROM 
			@UserIdentifierXML.nodes('UserIdentifiers/UserIdentifiers') AS T(C);

		SELECT @Loop = COUNT(*) FROM #tmpUI U INNER JOIN Core.UserIdentifierDetails CUID ON U.UserIdentifierDetailsID = CUID.UserIdentifierDetailsID AND U.UserID = CUID.UserID;

		WHILE @LOOP > 0

		BEGIN
		SET ROWCOUNT 1
		SELECT @ID = UserIdentifierDetailsID, @UserID = UserID FROM #tmpUI U INNER JOIN Core.UserIdentifierDetails CUID ON U.UserIdentifierDetailsID = CUID.UserIdentifierDetailsID AND U.UserID = CUID.UserID WHERE Completed = 0;
		SET ROWCOUNT 0

		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'UserIdentifierDetails', @ID, NULL, NULL, NULL, @UserID, 1, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE  u
		SET		IDNumber = t.IDNumber,
				EffectiveDate = t.EffectiveDate,
				ExpirationDate = t.ExpirationDate,
				IsActive = t.IsActive,
				ModifiedBy = t.ModifiedBy,
				ModifiedOn = t.ModifiedOn,
				SystemModifiedOn = GETUTCDATE()
		FROM	[Core].[UserIdentifierDetails] u
		INNER JOIN #tmpUI t
		ON		u.UserIdentifierDetailsID = t.UserIdentifierDetailsID 
		AND		u.UserID = t.UserID
		WHERE
			u.UserIdentifierDetailsID = @ID;

		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'UserIdentifierDetails', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		SELECT @Loop = @Loop - 1;
		UPDATE #tmpUI
		SET Completed = 1
		WHERE
			UserIdentifierDetailsID = @ID;
		END			
		
		IF OBJECT_ID('tempdb..#tmpUI') IS NOT NULL
			DROP TABLE #tmpUI

	END TRY

	BEGIN CATCH
		SELECT 
				@ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END