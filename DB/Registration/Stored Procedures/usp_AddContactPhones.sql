-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddContactPhones]
-- Author:		Saurabh Sahu
-- Date:		07/23/2015
--
-- Purpose:		Add Contact Phone Details
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		Contact and Phone Table (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/23/2015	Saurabh Sahu		Modification .
-- 7/31/2015    John Crossen    Change schema from dbo to Registration
-- 08/03/2015   Sumana Sangapu	1016 - Changed proc name schema qualifier from dbo to Registration/Core/Reference
-- 08/13/2015   Sumana Sangapu  1227  Refactor Schema
-- 08/16/2015	Rajiv Ranjan	Added PhonePermissionID, IsPrimary into #tmpPhonesCreated
-- 08/25/2015	Rajiv Ranjan	Added /text() to read xml as a nullable
-- 10/12/2015	Avikal			2297 : modified the logic to set all the Phone isPrimary to false, if new Phone is Primary 
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added SystemModifiedOn field, added CreatedBy and CreatedOn to Insert
-- 03/09/2016	Scott Martin	Added ContactMethodPreferenceID
-- 03/10/2016	Scott Martin	Removed ContactMethodPreferenceID
-- 06/02/2016	Gurmant Singh	Added EffectiveDate and ExpirationDate
-- 07/12/2016	Scott Martin		Increased length of extension field
-- 07/14/2016	Gurmant Singh	Increased length of Extension column of temporary table #tmpContactPhones from 5 to 10 characters
-- 12/15/2016	Scott Martin	Updated auditing
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_AddContactPhones]
	@ContactID BIGINT,
	@PhonesXML XML,
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
		@ContactPhoneID BIGINT,
		@PhoneID BIGINT;
	 
	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully';

	BEGIN TRY

	-- iterate through the addresses in the passed in xml, and each time an address is created, also associate the address w/ the contact
	IF OBJECT_ID('tempdb..#tmpContactPhones') IS NOT NULL
		DROP TABLE #tmpContactPhones

	CREATE TABLE #tmpContactPhones
	(
		ContactPhoneID BIGINT,
		PhoneID BIGINT,
		PhoneTypeID INT, 
		Number NVARCHAR(50), 
		Extension NVARCHAR(10),
		PhonePermissionID int,
		IsPrimary BIT,
		EffectiveDate DATE,
		ExpirationDate DATE,
		ModifiedOn DATETIME,
		AuditDetailID BIGINT
	);

	INSERT INTO #tmpContactPhones
	(
		PhoneTypeID,
		Number,
		Extension,
		PhonePermissionID,
		IsPrimary,
		EffectiveDate,
		ExpirationDate,
		ModifiedOn
	)
	SELECT 
		T.C.value('(./PhoneTypeID/text())[1]', 'INT'), 
		T.C.value('Number[1]', 'NVARCHAR(50)'),
		T.C.value('Extension[1]', 'NVARCHAR(10)'),
		T.C.value('(./PhonePermissionID/text())[1]', 'int'),
		T.C.value('IsPrimary[1]', 'bit'),
		CASE T.C.value('EffectiveDate[1]', 'DATE') WHEN '1900-01-01' THEN NULL ELSE T.C.value('EffectiveDate[1]', 'DATE') END,
		CASE T.C.value('ExpirationDate[1]', 'DATE') WHEN '1900-01-01' THEN NULL ELSE T.C.value('ExpirationDate[1]', 'DATE') END,
		T.C.value('ModifiedOn[1]', 'DATETIME')
	FROM 
		@PhonesXML.nodes('RequestXMLValue/Phone') AS T(C);

	EXEC Core.usp_AddPhone @PhonesXML, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AdditionalResultXML OUTPUT;

	UPDATE #tmpContactPhones
	SET PhoneID = P.PhoneID
	FROM
		#tmpContactPhones tCP
		INNER JOIN Core.Phone P
			ON ISNULL(tCP.Number, '') = ISNULL(P.Number, '')
			AND ISNULL(tCP.PhoneTypeID, 0) = ISNULL(P.PhoneTypeID, 0)
			AND ISNULL(tCP.Extension, '') = ISNULL(P.Extension, '')
			AND P.PhoneID IN (SELECT T.C.value('Identifier[1]', 'BIGINT') FROM @AdditionalResultXML.nodes('OutParameters') AS T (C))
	WHERE
		ISNULL(tCP.Number, '') = ISNULL(P.Number, '')
		AND ISNULL(tCP.PhoneTypeID, 0) = ISNULL(P.PhoneTypeID, 0)
		AND ISNULL(tCP.Extension, '') = ISNULL(P.Extension, '');

	UPDATE #tmpContactPhones
	SET ContactPhoneID = CP.ContactPhoneID
	FROM
		#tmpContactPhones tCP
		INNER JOIN Registration.ContactPhone CP
			ON tCp.PhoneID = CP.PhoneID
			AND CP.ContactID = @ContactID
	WHERE
		tCp.PhoneID = CP.PhoneID
		AND CP.ContactID = @ContactID;

	--Select statements purposely set this way so the procedure will fail if more than 1 primary email is found
	SET @IsPrimary = (SELECT IsPrimary FROM #tmpContactPhones WHERE IsPrimary = 1);

	SELECT @PhoneID = PhoneID, @ModifiedOn = ModifiedOn FROM #tmpContactPhones WHERE IsPrimary = 1;

	SET @ContactPhoneID = (SELECT ContactPhoneID FROM Registration.ContactPhone WHERE ContactID = @ContactID AND PhoneID <> @PhoneID AND IsPrimary = 1);

	--If the email to be inserted IsPrimary = 1 and there is an existing primary, set the existing primary to 0
	IF @ContactPhoneID IS NOT NULL AND @IsPrimary = 1
		BEGIN
		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'ContactPhone', @ContactPhoneID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE Registration.ContactPhone
		SET IsPrimary = 0,
			ModifiedBy = @ModifiedBy,
			ModifiedOn = @ModifiedOn,
			SystemModifiedOn = GETUTCDATE()
		WHERE	
			ContactPhoneID = @ContactPhoneID;

		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'ContactPhone', @AuditDetailID, @ContactPhoneID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		END

	IF EXISTS (SELECT TOP 1 * FROM #tmpContactPhones WHERE ContactPhoneID IS NOT NULL)
		BEGIN
			SET @AuditCursor = CURSOR FOR
			SELECT ContactPhoneID, ModifiedOn FROM #tmpContactPhones WHERE ContactPhoneID IS NOT NULL;    

			OPEN @AuditCursor 
			FETCH NEXT FROM @AuditCursor 
			INTO @ContactPhoneID, @ModifiedOn

			WHILE @@FETCH_STATUS = 0
			BEGIN
			EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'ContactPhone', @ContactPhoneID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			
			UPDATE #tmpContactPhones
			SET AuditDetailID = @AuditDetailID
			WHERE
				ContactPhoneID = @ContactPhoneID;

			FETCH NEXT FROM @AuditCursor 
			INTO @ContactPhoneID, @ModifiedOn
			END; 

			CLOSE @AuditCursor;
			DEALLOCATE @AuditCursor;
		END;

		-- Merge statement for Registration.ContactPhone
		DECLARE	@tmpContactPhones TABLE
		(
			Operation varchar(10),
			ContactPhoneID INT,
			ModifiedOn DATETIME
		);

	MERGE INTO Registration.ContactPhone AS TARGET
	USING (SELECT * FROM #tmpContactPhones t)  AS SOURCE
		ON ISNULL(SOURCE.ContactPhoneID, 0) = TARGET.ContactPhoneID
	WHEN NOT MATCHED THEN
		INSERT
		(
			ContactID,
			PhoneID,
			PhonePermissionID,
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
			SOURCE.PhoneID,
			SOURCE.PhonePermissionID,
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
			TARGET.EffectiveDate = SOURCE.EffectiveDate,
			TARGET.ExpirationDate = SOURCE.ExpirationDate,
			TARGET.PhonePermissionID = SOURCE.PhonePermissionID,
			TARGET.IsActive = 1,
			TARGET.ModifiedBy = @ModifiedBy,
			TARGET.ModifiedOn = SOURCE.ModifiedOn,
			TARGET.SystemModifiedOn = GETUTCDATE()
	OUTPUT
		$action,
		inserted.ContactPhoneID,
		inserted.ModifiedOn
	INTO
		@tmpContactPhones;

	IF EXISTS (SELECT TOP 1 * FROM @tmpContactPhones WHERE Operation = 'Insert')
		BEGIN
		BEGIN
			SET @AuditCursor = CURSOR FOR
			SELECT ContactPhoneID, ModifiedOn FROM @tmpContactPhones WHERE Operation = 'Insert';    

			OPEN @AuditCursor 
			FETCH NEXT FROM @AuditCursor 
			INTO @ContactPhoneID, @ModifiedOn

			WHILE @@FETCH_STATUS = 0
			BEGIN
			EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'ContactPhone', @ContactPhoneID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

			EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'ContactPhone', @AuditDetailID, @ContactPhoneID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

			FETCH NEXT FROM @AuditCursor 
			INTO @ContactPhoneID, @ModifiedOn
			END; 

			CLOSE @AuditCursor;
			DEALLOCATE @AuditCursor;
		END;
		END

		IF EXISTS (SELECT TOP 1 * FROM #tmpContactPhones WHERE ContactPhoneID IS NOT NULL)
		BEGIN
		BEGIN
			SET @AuditCursor = CURSOR FOR
			SELECT ContactPhoneID, ModifiedOn, AuditDetailID FROM #tmpContactPhones WHERE ContactPhoneID IS NOT NULL;   

			OPEN @AuditCursor 
			FETCH NEXT FROM @AuditCursor 
			INTO @ContactPhoneID, @ModifiedOn, @AuditDetailID

			WHILE @@FETCH_STATUS = 0
			BEGIN
			EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'ContactPhone', @AuditDetailID, @ContactPhoneID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

			FETCH NEXT FROM @AuditCursor 
			INTO @ContactPhoneID, @ModifiedOn, @AuditDetailID
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