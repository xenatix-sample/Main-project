-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdateUserEmails]
-- Author:		Justin Spalti
-- Date:		09/30/2015
--
-- Purpose:		Update User Email Details
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		Core.Users, Core.UserEmail, Core.Email
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 09/30/2015	Justin Spalti - Initial creation
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added SystemModifiedOn fields
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_UpdateUserEmails]
	@EmailXML XML,
	@ModifiedBy INT,	
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @IsPrimary BIT = 0,
		@UserEmailID BIGINT = 0,
		@OtherUserEmailID BIGINT = 0,
		@UserID BIGINT,
		@EmailID BIGINT,
		@ModifiedOn DATETIME,
		@AuditDetailID BIGINT,
		@EmailCount INT,
		@AdditionalResultXML XML,
		@EmailsXML XML,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
	IF OBJECT_ID('tempdb..#tmpUserEmail') IS NOT NULL
		DROP TABLE #tmpUserEmail

	CREATE TABLE #tmpUserEmail
	(
		UserEmailID BIGINT,
		UserID BIGINT,
		EmailID BIGINT, 
		Email NVARCHAR(255), 
		EmailPermissionID int,
		IsPrimary bit,
		IsActive bit,
		ModifiedOn DATETIME
	);

	INSERT INTO #tmpUserEmail
	(
		UserEmailID,
		EmailID, 
		Email, 
		EmailPermissionID,
		IsPrimary,
		IsActive,
		ModifiedOn
	)
	SELECT 
		T.C.value('UserEmailID[1]', 'BIGINT'),
		T.C.value('EmailID[1]', 'BIGINT'), 
		T.C.value('Email[1]', 'NVARCHAR(255)'), 
		T.C.value('(./EmailPermissionID/text())[1]', 'INT'), 
		T.C.value('IsPrimary[1]', 'BIT'),
		T.C.value('IsActive[1]', 'BIT'),
		T.C.value('ModifiedOn[1]', 'DATETIME')
	FROM 
		@EmailXML.nodes('RequestXMLValue/Email') as T(C);

	UPDATE #tmpUserEmail
	SET UserID = CE.UserID
	FROM
		#tmpUserEmail
		INNER JOIN Core.UserEmail CE
			ON #tmpUserEmail.UserEmailID = CE.UserEmailID
	WHERE
		#tmpUserEmail.UserEmailID = CE.UserEmailID;
	
	DECLARE @UpdateCursor CURSOR;
	SET @UpdateCursor = CURSOR FOR
	SELECT UserEmailID, UserID, EmailID, IsPrimary, ModifiedOn FROM #tmpUserEmail WHERE UserEmailID IS NOT NULL;    

	OPEN @UpdateCursor 
	FETCH NEXT FROM @UpdateCursor 
	INTO @UserEmailID, @UserID, @EmailID, @IsPrimary, @ModifiedOn

	WHILE @@FETCH_STATUS = 0
	BEGIN
	SELECT @OtherUserEmailID = UserEmailID FROM Core.UserEmail WHERE UserID = @UserID AND IsPrimary = 1 AND UserEmailID <> @UserEmailID;

	IF ISNULL(@OtherUserEmailID, 0) > 0 AND @IsPrimary = 1
		BEGIN
		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'UserEmail', @OtherUserEmailID, NULL, NULL, NULL, @UserID, 1, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE CE
		SET IsPrimary = 0,
			ModifiedBy = @ModifiedBy,
			ModifiedOn = @ModifiedOn,
			SystemModifiedOn = GETUTCDATE()
		FROM
			Core.UserEmail CE 
		WHERE 
			CE.UserEmailID = @OtherUserEmailID;

		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'UserEmail', @AuditDetailID, @OtherUserEmailID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT; 
		END

	SET @EmailCount = (SELECT COUNT(*) FROM Core.UserEmail WHERE EmailID = @EmailID AND UserID <> @UserID);

	SET @EmailsXML = (SELECT * FROM #tmpUserEmail WHERE EmailID = @EmailID FOR XML PATH ('Email'), ROOT ('RequestXMLValue'));

	IF ISNULL(@EmailCount, 0) > 0
		BEGIN
		EXEC Core.usp_AddEmail @EmailsXML, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AdditionalResultXML OUTPUT;

		SET @EmailID = (SELECT T.C.value('Identifier[1]', 'BIGINT') FROM @AdditionalResultXML.nodes('OutParameters') AS T (C));
		END
	ELSE
		BEGIN
		EXEC Core.usp_UpdateEmail @EmailsXML, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT;
		END

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'UserEmail', @UserEmailID, NULL, NULL, NULL, @UserID, 1, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE ce
	SET ce.EmailID = @EmailID,
		ce.EmailPermissionID = te.EmailPermissionID,
		ce.IsActive = ISNULL(te.IsActive,1),
		IsPrimary = te.IsPrimary,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = @ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	FROM 
		Core.UserEmail ce 
		INNER JOIN #tmpUserEmail te
			ON te.UserEmailID = ce.UserEmailID
	WHERE
		ce.UserEmailID = @UserEmailID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'UserEmail', @AuditDetailID, @UserEmailID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT; 

	FETCH NEXT FROM @UpdateCursor 
	INTO @UserEmailID, @UserID, @EmailID, @IsPrimary, @ModifiedOn
	END; 

	CLOSE @UpdateCursor;
	DEALLOCATE @UpdateCursor;
			
	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END