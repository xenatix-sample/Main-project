-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[Core].[usp_AddUserSecurityQuestions]
-- Author:		Justin Spalti
-- Date:		10/01/2015
--
-- Purpose:		Add User SEcurity Questions and Answers
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		Core.SecurityQuestion, Core.UserSecurityQuestion
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/01/2015	Justin Spalti - Initial creation
-- 10/02/2015 - Justin Spalti - Removed the UserSecurityQuestionID from the temp table to prevent PK errors
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added CreatedOn and CreatedBy fields
-- 02/17/2016	Scott Martin		Refactored for audit loggin
-- 03/04/2016   Satish Singh        Commented two proc Need to verify by Scott. 
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_AddUserSecurityQuestions]
	@UserID INT,
	@QuestionXml XML,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
	
AS
BEGIN
DECLARE @ID BIGINT,
		@ModifiedOn DATETIME,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT @ResultCode = 0,
		   @ResultMessage = 'Executed successfully';

	BEGIN TRY
	IF OBJECT_ID('tempdb..#tmpUserSecurityQuestions') IS NOT NULL
		DROP TABLE #tmpUserSecurityQuestions

	CREATE TABLE #tmpUserSecurityQuestions
	(
		SecurityQuestionID INT,				
		SecurityAnswer NVARCHAR(250),
		ModifiedOn DATETIME
	);

	INSERT INTO #tmpUserSecurityQuestions
	(
		SecurityQuestionID,
		SecurityAnswer,
		ModifiedOn
	)
	SELECT 				
		T.C.value('SecurityQuestionID[1]', 'int'),
		T.C.value('SecurityAnswer[1]', 'NVARCHAR(250)'),
		T.C.value('ModifiedOn[1]', 'DATETIME')

	FROM 
		@QuestionXml.nodes('RequestXMLValue/Question') AS T(C);

	DECLARE @USQ_ID TABLE (ID BIGINT, ModifiedOn DATETIME);

	INSERT INTO Core.UserSecurityQuestion
	(
		UserID,
		SecurityQuestionID,
		SecurityAnswer,
		IsActive,
		ModifiedBy,
		ModifiedOn,
		CreatedBy,
		CreatedOn
	)
	OUTPUT
		inserted.UserSecurityQuestionID,
		inserted.ModifiedOn
	INTO @USQ_ID
	SELECT
		@UserID,
		SecurityQuestionID,
		Core.fn_GenerateHash(SecurityAnswer),
		1,
		@ModifiedBy,
		ModifiedOn,
		@ModifiedBy,
		ModifiedOn
	FROM
		#tmpUserSecurityQuestions;

	DECLARE @AuditCursor CURSOR;
	DECLARE @AuditDetailID BIGINT;
	BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT ID, ModifiedOn FROM @USQ_ID;    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @ModifiedOn

		WHILE @@FETCH_STATUS = 0
		BEGIN
		--EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Core', 'UserSecurityQuestion', @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	--	EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Core', 'UserSecurityQuestion', @AuditDetailID, @ID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		FETCH NEXT FROM @AuditCursor 
		INTO @ID, @ModifiedOn
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
