-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdateEmail]
-- Author:		Saurabh Sahu
-- Date:		08/10/2015
--
-- Purpose:		Update Email Details
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		(or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/10/2015	Saurabh Sahu		Initial Draft .
-- 01/12/2016	Scott Martin		Added audit logging
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added SystemModifiedOn field
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_UpdateEmail]
	@EmailXML XML,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @EmailID INT,
		@ModifiedOn DATETIME,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
	IF OBJECT_ID('tempdb..#tmpEmail') IS NOT NULL
		DROP TABLE #tmpEmail

	CREATE TABLE #tmpEmail
	(
		EmailID BIGINT, 
		Email NVARCHAR(255),
		IsActive BIT,
		ModifiedOn DATETIME,
		AuditDetailID BIGINT
	);

	INSERT INTO #tmpEmail
	(
		EmailID, 
		Email,
		IsActive,
		ModifiedOn,
		AuditDetailID
	)
	SELECT 
		T.C.value('EmailID[1]', 'BIGINT'), 
		T.C.value('Email[1]', 'NVARCHAR(255)'),
		T.C.value('IsActive[1]', 'BIT'),
		T.C.value('ModifiedOn[1]', 'DATETIME'),
		NULL
	FROM 
		@EmailXML.nodes('RequestXMLValue/Email') as T(C);

	DECLARE @AuditCursor CURSOR;
	DECLARE @AuditDetailID BIGINT;
	BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT EmailID, ModifiedOn FROM #tmpEmail;    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @EmailID, @ModifiedOn

		WHILE @@FETCH_STATUS = 0
		BEGIN
		EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'Email', @EmailID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE #tmpEmail
		SET AuditDetailID = @AuditDetailID
		WHERE
			EmailID = @EmailID;

		FETCH NEXT FROM @AuditCursor 
		INTO @EmailID, @ModifiedOn
		END; 

		CLOSE @AuditCursor;
		DEALLOCATE @AuditCursor;
	END;

	UPDATE e
	SET Email = te.Email,
		IsActive = COALESCE(te.IsActive, e.IsActive),
		ModifiedBy = @ModifiedBy,
		ModifiedOn = te.ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	FROM 
		Core.Email e
		INNER JOIN #tmpEmail te 
			ON te.EmailID = e.EmailID;

	BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT EmailID, ModifiedOn, AuditDetailID FROM #tmpEmail;    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @EmailID, @ModifiedOn, @AuditDetailID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'Email', @AuditDetailID, @EmailID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		FETCH NEXT FROM @AuditCursor 
		INTO @EmailID, @ModifiedOn, @AuditDetailID
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