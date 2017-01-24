-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddContactEmails]
-- Author:		Saurabh Sahu
-- Date:		07/23/2015
--
-- Purpose:		Add Contact Email Details
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		Contact and Email Table (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/23/2015	Saurabh Sahu		Modification .
-- 7/31/2015    John Crossen    Change schema from dbo to Registration
-- 08/03/2015   Sumana Sangapu	1016 - Changed proc name schema qualifier from dbo to Registration/Core/Reference
-- 08/13/2015	Sumana Sangapu	1227 - Refactor Schema
-- 08/16/2015	Rajiv Ranjan	Added EmailPermissionID, IsPrimary into #tmpEmailsCreated
-- 08/25/2015	Rajiv Ranjan	Added /text() to read xml as a nullable
-- 10/08/2015	Satish Singh	Incase of non primary email avoid to make other email to non primary
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added SystemModifiedOn field, added CreatedBy and CreatedOn to Insert
-- 03/09/2016	Scott Martin	Added ContactMethodPreferenceID
-- 03/10/2016	Scott Martin	Removed ContactMethodPreferenceID
-- 06/02/2016	Gurmant Singh	Added EffectiveDate and ExpirationDate
-- 12/15/2016	Scott Martin	Updated auditing
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_AddContactEmails]
	@ContactID BIGINT,
	@EmailsXML XML,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT	
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@AuditCursor CURSOR,
		@ModifiedOn DATETIME,
		@AdditionalResultXML XML,
		@IsPrimary BIT = 0,
		@EffectiveDate DATE,
		@ExpirationDate DATE,
		@ContactEmailID BIGINT,
		@EmailID BIGINT;

SELECT @ResultCode = 0,
		@ResultMessage = 'executed successfully';

	BEGIN TRY
	IF OBJECT_ID('tempdb..#tmpContactEmails') IS NOT NULL
		DROP TABLE #tmpContactEmails

	CREATE TABLE #tmpContactEmails
	(
		ContactEmailID BIGINT,
		EmailID BIGINT,
		Email NVARCHAR(200), 
		EmailPermissionID INT,
		IsPrimary BIT,
		EffectiveDate DATE,
		ExpirationDate DATE,
		ModifiedOn DATETIME,
		AuditDetailID BIGINT
	);

	INSERT INTO #tmpContactEmails
	(
		Email,
		EmailPermissionID,
		IsPrimary,
		EffectiveDate,
		ExpirationDate,
		ModifiedOn
	)
	SELECT 
		T.C.value('Email[1]', 'NVARCHAR(255)'),
		T.C.value('(./EmailPermissionID/text())[1]', 'INT'),
		T.C.value('IsPrimary[1]', 'BIT'),
		CASE T.C.value('EffectiveDate[1]', 'DATE') WHEN '1900-01-01' THEN NULL ELSE T.C.value('EffectiveDate[1]', 'DATE') END,
		CASE T.C.value('ExpirationDate[1]', 'DATE') WHEN '1900-01-01' THEN NULL ELSE T.C.value('ExpirationDate[1]', 'DATE') END,
		T.C.value('ModifiedOn[1]', 'DATETIME')
	FROM 
		@EmailsXML.nodes('RequestXMLValue/Email') AS T(C);

	EXEC Core.usp_AddEmail @EmailsXML, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AdditionalResultXML OUTPUT;

	UPDATE #tmpContactEmails
	SET EmailID = E.EmailID
	FROM
		#tmpContactEmails tCE
		INNER JOIN Core.Email E
			ON ISNULL(tCE.Email, 0) = ISNULL(E.Email, 0)
			AND E.EmailID IN (SELECT T.C.value('Identifier[1]', 'BIGINT') FROM @AdditionalResultXML.nodes('OutParameters') AS T (C))
	WHERE
		ISNULL(tCE.Email, 0) = ISNULL(E.Email, 0);

	UPDATE #tmpContactEmails
	SET ContactEmailID = CE.ContactEmailID
	FROM
		#tmpContactEmails tCE
		INNER JOIN Registration.ContactEmail CE
			ON tCE.EmailID = CE.EmailID
			AND CE.ContactID = @ContactID
	WHERE
		tCE.EmailID = CE.EmailID
		AND CE.ContactID = @ContactID;

	--Select statements purposely set this way so the procedure will fail if more than 1 primary email is found
	SET @IsPrimary = (SELECT IsPrimary FROM #tmpContactEmails WHERE IsPrimary = 1);

	SELECT @EmailID = EmailID, @ModifiedOn = ModifiedOn FROM #tmpContactEmails WHERE IsPrimary = 1;

	SET @ContactEmailID = (SELECT ContactEmailID FROM Registration.ContactEmail WHERE ContactID = @ContactID AND EmailID <> @EmailID AND IsPrimary = 1);

	--If the email to be inserted IsPrimary = 1 and there is an existing primary, set the existing primary to 0
	IF @ContactEmailID IS NOT NULL AND @IsPrimary = 1
		BEGIN
		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'ContactEmail', @ContactEmailID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE Registration.ContactEmail
		SET IsPrimary = 0,
			ModifiedBy = @ModifiedBy,
			ModifiedOn = @ModifiedOn,
			SystemModifiedOn = GETUTCDATE()
		WHERE	
			ContactEmailID = @ContactEmailID;

		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'ContactEmail', @AuditDetailID, @ContactEmailID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		END

	IF EXISTS (SELECT TOP 1 * FROM #tmpContactEmails WHERE ContactEmailID IS NOT NULL)
		BEGIN
			SET @AuditCursor = CURSOR FOR
			SELECT ContactEmailID, ModifiedOn FROM #tmpContactEmails WHERE ContactEmailID IS NOT NULL;    

			OPEN @AuditCursor 
			FETCH NEXT FROM @AuditCursor 
			INTO @ContactEmailID, @ModifiedOn

			WHILE @@FETCH_STATUS = 0
			BEGIN
			EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'ContactEmail', @ContactEmailID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			
			UPDATE #tmpContactEmails
			SET AuditDetailID = @AuditDetailID
			WHERE
				ContactEmailID = @ContactEmailID;

			FETCH NEXT FROM @AuditCursor 
			INTO @ContactEmailID, @ModifiedOn
			END; 

			CLOSE @AuditCursor;
			DEALLOCATE @AuditCursor;
		END;

	-- Merge statement for Registration.ContactEmail
	DECLARE	@tmpContactEmails TABLE
	(
		Operation varchar(10),
		ContactEmailID INT,
		ModifiedOn DATETIME
	);

	MERGE INTO Registration.ContactEmail AS TARGET
	USING (SELECT * FROM #tmpContactEmails t)  AS SOURCE
		ON ISNULL(SOURCE.ContactEmailID, 0) = TARGET.ContactEmailID
	WHEN NOT MATCHED THEN
		INSERT
		(
			ContactID,
			EmailID,
			EmailPermissionID,
			IsPrimary,
			EffectiveDate,
			ExpirationDate,
			IsActive,
			ModifiedBy,
			ModifiedOn,
			CreatedBy,
			CreatedOn
		)
		VALUES
		(
			@ContactID,
			SOURCE.EmailID,
			SOURCE.EmailPermissionID,
			SOURCE.IsPrimary,
			SOURCE.EffectiveDate,
			SOURCE.ExpirationDate,
			1,
			@ModifiedBy,
			SOURCE.ModifiedOn,
			@ModifiedBy,
			SOURCE.ModifiedOn
		)
	WHEN MATCHED THEN
		UPDATE
		SET TARGET.IsPrimary = SOURCE.IsPrimary,
			TARGET.EmailPermissionID = SOURCE.EmailPermissionID,
			TARGET.IsActive = 1,
			TARGET.EffectiveDate = SOURCE.EffectiveDate,
			TARGET.ExpirationDate = SOURCE.ExpirationDate,
			TARGET.ModifiedBy = @ModifiedBy,
			TARGET.ModifiedOn = SOURCE.ModifiedOn,
			TARGET.SystemModifiedOn = GETUTCDATE()
	OUTPUT
		$action,
		inserted.ContactEmailID,
		inserted.ModifiedOn
	INTO
		@tmpContactEmails;

	IF EXISTS (SELECT TOP 1 * FROM @tmpContactEmails WHERE Operation = 'Insert')
		BEGIN
		BEGIN
			SET @AuditCursor = CURSOR FOR
			SELECT ContactEmailID, ModifiedOn FROM @tmpContactEmails WHERE Operation = 'Insert';    

			OPEN @AuditCursor 
			FETCH NEXT FROM @AuditCursor 
			INTO @ContactEmailID, @ModifiedOn

			WHILE @@FETCH_STATUS = 0
			BEGIN
			EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'ContactEmail', @ContactEmailID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

			EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'ContactEmail', @AuditDetailID, @ContactEmailID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

			FETCH NEXT FROM @AuditCursor 
			INTO @ContactEmailID, @ModifiedOn
			END; 

			CLOSE @AuditCursor;
			DEALLOCATE @AuditCursor;
		END;
		END

		IF EXISTS (SELECT TOP 1 * FROM #tmpContactEmails WHERE ContactEmailID IS NOT NULL)
		BEGIN
		BEGIN
			SET @AuditCursor = CURSOR FOR
			SELECT ContactEmailID, ModifiedOn, AuditDetailID FROM #tmpContactEmails WHERE ContactEmailID IS NOT NULL;   

			OPEN @AuditCursor 
			FETCH NEXT FROM @AuditCursor 
			INTO @ContactEmailID, @ModifiedOn, @AuditDetailID

			WHILE @@FETCH_STATUS = 0
			BEGIN
			EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'ContactEmail', @AuditDetailID, @ContactEmailID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

			FETCH NEXT FROM @AuditCursor 
			INTO @ContactEmailID, @ModifiedOn, @AuditDetailID
			END; 

			CLOSE @AuditCursor;
			DEALLOCATE @AuditCursor;
		END;
		END

	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END