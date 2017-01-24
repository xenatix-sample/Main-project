-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdateContactEmails]
-- Author:		Saurabh Sahu
-- Date:		07/23/2015
--
-- Purpose:		Update Contact Email Details
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		Contact Table (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/23/2015	Saurabh Sahu		Modification .
-- 7/31/2015    John Crossen    Change schema from dbo to Registration
-- 08/03/2015   Sumana Sangapu	1016 - Changed proc name schema qualifier from dbo to Registration/Core/Reference
-- 08/16/2015	Rajiv Ranjan	Removed @ModifiedOn parameter
--								Removed IsActiveEmail, IsActiveContactEmail, ModifiedOnEmail & ModifiedOnContactEmail
--								Corrected xml path from 'RequestXMLValue/Address' to 'RequestXMLValue/Email'
-- 08/25/2015	Rajiv Ranjan	Added /text() to read xml as a nullable
-- 09/25/2015 - John Crossen    - Refactor Proc to use PK for update
-- 10/08/2015	Satish Singh	Incase of non primary email avoid to make other email to non primary
-- 10/09/2015	Satish Singh	If primary then set others to non primary in respect of contactID
-- 01/20/2015	Arun Choudhary	Based on XML, update IsActive field to true/false.
-- 03/09/2016	Scott Martin	Added ContactMethodPreferenceID
-- 03/10/2016	Scott Martin	Removed ContactMethodPreferenceID
-- 06/02/2016	Gurmant Singh	Added EffectiveDate and ExpirationDate
-- 12/15/2016	Scott Martin	Updated auditing
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Registration.usp_UpdateContactEmails
	@EmailXML XML,
	@ModifiedBy INT,	
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@IsPrimary BIT=0,
		@EffectiveDate DATE,
		@ExpirationDate DATE,
		@ContactEmailID BIGINT = 0,
		@OtherContactEmailID BIGINT = 0,
		@ContactID BIGINT,
		@EmailID BIGINT,
		@ExistingEmailID BIGINT,
		@ModifiedOn DATETIME,
		@EmailCount INT,
		@AdditionalResultXML XML,
		@EmailsXML XML;

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
	IF OBJECT_ID('tempdb..#tmpContactEmails') IS NOT NULL
		DROP TABLE #tmpContactEmails

	CREATE TABLE #tmpContactEmails
	(
		ContactEmailID BIGINT,
		ContactID BIGINT,
		EmailID BIGINT,
		ExistingEmailID BIGINT, 
		Email NVARCHAR(255), 
		EmailPermissionID int,
		IsPrimary bit,
		EffectiveDate DATE,
		ExpirationDate DATE,
		IsActive bit,
		ModifiedOn DATETIME
	);

	INSERT INTO #tmpContactEmails
	(
		ContactEmailID,
		EmailID, 
		Email, 
		EmailPermissionID,
		IsPrimary,
		EffectiveDate,
		ExpirationDate,
		IsActive,
		ModifiedOn
	)
	SELECT 
		T.C.value('ContactEmailID[1]', 'BIGINT'),
		T.C.value('EmailID[1]', 'BIGINT'), 
		T.C.value('Email[1]', 'NVARCHAR(255)'), 
		T.C.value('(./EmailPermissionID/text())[1]', 'INT'), 
		T.C.value('IsPrimary[1]', 'BIT'),
		CASE T.C.value('EffectiveDate[1]', 'DATE') WHEN '1900-01-01' THEN NULL ELSE T.C.value('EffectiveDate[1]', 'DATE') END,
		CASE T.C.value('ExpirationDate[1]', 'DATE') WHEN '1900-01-01' THEN NULL ELSE T.C.value('ExpirationDate[1]', 'DATE') END,
		T.C.value('IsActive[1]', 'BIT'),
		T.C.value('ModifiedOn[1]', 'DATETIME')
	FROM 
		@EmailXML.nodes('RequestXMLValue/Email') as T(C);

	UPDATE #tmpContactEmails
	SET ContactID = CE.ContactID
	FROM
		#tmpContactEmails
		INNER JOIN Registration.ContactEmail CE
			ON #tmpContactEmails.ContactEmailID = CE.ContactEmailID
	WHERE
		#tmpContactEmails.ContactEmailID = CE.ContactEmailID;

	UPDATE tCE
	SET tCE.ExistingEmailID = E.EmailID
	FROM
		#tmpContactEmails tCE
		INNER JOIN Core.Email E
			ON tCE.Email = E.Email
	WHERE
		tCE.Email = E.Email;
	
	DECLARE @UpdateCursor CURSOR;
	SET @UpdateCursor = CURSOR FOR
	SELECT ContactEmailID, ContactID, EmailID, ExistingEmailID, IsPrimary, ModifiedOn FROM #tmpContactEmails WHERE ContactEmailID IS NOT NULL;    

	OPEN @UpdateCursor 
	FETCH NEXT FROM @UpdateCursor 
	INTO @ContactEmailID, @ContactID, @EmailID, @ExistingEmailID, @IsPrimary, @ModifiedOn

	WHILE @@FETCH_STATUS = 0
	BEGIN
	SELECT @OtherContactEmailID = ContactEmailID FROM Registration.ContactEmail WHERE ContactID = @ContactID AND IsPrimary = 1 AND ContactEmailID <> @ContactEmailID;

	IF ISNULL(@OtherContactEmailID, 0) > 0 AND @IsPrimary = 1
		BEGIN
		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'ContactEmail', @OtherContactEmailID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE CE
		SET IsPrimary = 0,
			ModifiedBy = @ModifiedBy,
			ModifiedOn = @ModifiedOn,
			SystemModifiedOn = GETUTCDATE()
		FROM
			Registration.ContactEmail CE 
		WHERE 
			CE.ContactEmailID = @OtherContactEmailID;

		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'ContactEmail', @AuditDetailID, @OtherContactEmailID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT; 
		END

	SET @EmailCount = (SELECT COUNT(*) FROM Registration.ContactEmail WHERE EmailID = @EmailID AND ContactID <> @ContactID);

	SET @EmailsXML = (SELECT * FROM #tmpContactEmails WHERE EmailID = @EmailID FOR XML PATH ('Email'), ROOT ('RequestXMLValue'));

	IF ISNULL(@EmailCount, 0) > 0
		BEGIN
		EXEC Core.usp_AddEmail @EmailsXML, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AdditionalResultXML OUTPUT;

		SET @EmailID = (SELECT T.C.value('Identifier[1]', 'BIGINT') FROM @AdditionalResultXML.nodes('OutParameters') AS T (C));
		END

	IF ISNULL(@EmailCount, 0) = 0 AND @ExistingEmailID IS NOT NULL
		BEGIN
		SET @EmailID = @ExistingEmailID;
		END
	
	IF ISNULL(@EmailCount, 0) = 0 AND @ExistingEmailID IS NULL
		BEGIN
		EXEC Core.usp_UpdateEmail @EmailsXML, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT;
		END

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'ContactEmail', @ContactEmailID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE ce
	SET ce.EmailID = @EmailID,
		ce.EmailPermissionID = te.EmailPermissionID,
		ce.IsActive = ISNULL(te.IsActive,1),
		IsPrimary = te.IsPrimary,
		ce.EffectiveDate = te.EffectiveDate,
		ce.ExpirationDate = te.ExpirationDate,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = te.ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	FROM 
		Registration.ContactEmail ce 
		INNER JOIN #tmpContactEmails te
			ON te.ContactEmailID = ce.ContactEmailID
	WHERE
		ce.ContactEmailID = @ContactEmailID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'ContactEmail', @AuditDetailID, @ContactEmailID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT; 

	FETCH NEXT FROM @UpdateCursor 
	INTO @ContactEmailID, @ContactID, @EmailID, @ExistingEmailID, @IsPrimary, @ModifiedOn
	END; 

	CLOSE @UpdateCursor;
	DEALLOCATE @UpdateCursor;
			
	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END