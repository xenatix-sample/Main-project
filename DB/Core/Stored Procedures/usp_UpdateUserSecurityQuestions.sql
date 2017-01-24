-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[usp_UpdateUserSecurityQuestions]
-- Author:		Justin Spalti
-- Date:		10/01/2015
--
-- Purpose:		Update User Security Questions and Answers
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		Core.SecurityQuestion, Core.UserSecurityQuestion
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/01/2015	Justin Spalti - Initial creation
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added SystemModifiedOn fields
-- 02/17/2016	Scott Martin		Refactored for audit loggin
-- 03/04/2016   Satish Singh        Commented two proc Need to verify by Scott. 
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_UpdateUserSecurityQuestions]
	@QuestionXml XML,
	--@ModifiedOn DATETIME,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
DECLARE @UserSecurityQuestionID INT,
		@ModifiedOn DATETIME,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT @ResultCode = 0,
		   @ResultMessage = 'Executed successfully'

	BEGIN TRY

	IF OBJECT_ID('tempdb..#tmpUserSecurityQuestions') IS NOT NULL
		DROP TABLE #tmpUserSecurityQuestions

	CREATE TABLE #tmpUserSecurityQuestions
	(
		UserSecurityQuestionID BIGINT PRIMARY KEY NOT NULL,
		SecurityQuestionID INT,				
		SecurityAnswer NVARCHAR(250),
		ModifiedOn DATETIME,
		AuditDetailID BIGINT
	);

	INSERT INTO #tmpUserSecurityQuestions
	(
		UserSecurityQuestionID,
		SecurityQuestionID,
		SecurityAnswer,
		ModifiedOn,
		AuditDetailID
	)
	SELECT 
		T.C.value('UserSecurityQuestionID[1]', 'BIGINT'), 				
		T.C.value('SecurityQuestionID[1]', 'INT'),
		T.C.value('SecurityAnswer[1]', 'NVARCHAR(250)'),
		T.C.value('ModifiedOn[1]', 'DATETIME'),
		NULL
	FROM 
		@QuestionXml.nodes('RequestXMLValue/Question') AS T(C);

	DECLARE @AuditCursor CURSOR;
	DECLARE @AuditDetailID BIGINT;
	BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT UserSecurityQuestionID, ModifiedOn FROM #tmpUserSecurityQuestions;    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @UserSecurityQuestionID, @ModifiedOn

		WHILE @@FETCH_STATUS = 0
		BEGIN
	--	EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'UserSecurityQuestion', @UserSecurityQuestionID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE #tmpUserSecurityQuestions
		SET AuditDetailID = @AuditDetailID
		WHERE
			UserSecurityQuestionID = @UserSecurityQuestionID;

		FETCH NEXT FROM @AuditCursor 
		INTO @UserSecurityQuestionID, @ModifiedOn
		END; 

		CLOSE @AuditCursor;
		DEALLOCATE @AuditCursor;
	END;

	UPDATE usq
	SET SecurityQuestionID = tsq.SecurityQuestionID,
		SecurityAnswer = Core.fn_GenerateHash(tsq.SecurityAnswer),
		ModifiedOn = tsq.ModifiedOn,
		SystemModifiedOn = GETUTCDATE(),
		ModifiedBy = @ModifiedBy
	FROM
		#tmpUserSecurityQuestions tsq
		INNER JOIN Core.UserSecurityQuestion usq
			ON usq.UserSecurityQuestionID = tsq.UserSecurityQuestionID;

	BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT UserSecurityQuestionID, ModifiedOn, AuditDetailID FROM #tmpUserSecurityQuestions;    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @UserSecurityQuestionID, @ModifiedOn, @AuditDetailID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		--EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'UserSecurityQuestion', @AuditDetailID, @UserSecurityQuestionID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		FETCH NEXT FROM @AuditCursor 
		INTO @UserSecurityQuestionID, @ModifiedOn, @AuditDetailID
		END; 

		CLOSE @AuditCursor;
		DEALLOCATE @AuditCursor;
	END;
				
	END TRY
	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END