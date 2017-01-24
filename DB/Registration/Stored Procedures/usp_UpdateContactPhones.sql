-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[[usp_UpdateContactPhones]]
-- Author:		Saurabh Sahu
-- Date:		07/23/2015
--
-- Purpose:		Update Contact Phone Details
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		Contact Table (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/23/2015	Saurabh Sahu		Modification .
-- 7/31/2015    John Crossen    Change schema from dbo to Registration
-- 08/03/2015   Sumana Sangapu	1016 - Changed proc name schema qualifier from dbo to Registration/Core/Reference
-- 08/13/2015	Sumana Sangapu	1227 - Refactor schema
-- 08/16/2015	Rajiv Ranjan		Removed IsActivePhone, IsActiveContactPhone, ModifiedOnPhone & ModifiedOnContactPhone
-- 08/25/2015	Rajiv Ranjan	Added /text() to read xml as a nullable
-- 09/25/2015 - John Crossen    - Refactor Proc to use PK for update
-- 10/09/2105 - Avikal			- 2297 : Phone Screen : Modified Proc to update IsActive in contactPhone for delete and update feature
--										for	deactivation, contact Phone fields, setting isprimary for one field		
-- 01/20/2015	Arun Choudhary	Based on XML, update IsActive field to true/false.
-- 02/15/2016	Rajiv Ranjan		Move Core.usp_UpdatePhone to Registration.usp_UpdatePhone
-- 03/09/2016	Scott Martin	Added ContactMethodPreferenceID
-- 03/10/2016	Scott Martin	Removed ContactMethodPreferenceID
-- 06/02/2016	Gurmant Singh	Added EffectiveDate and ExpirationDate
-- 07/14/2016	Gurmant Singh	Increased length of Extension column from 5 to 10 characters
-- 12/15/2016	Scott Martin	Updated auditing
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Registration.usp_UpdateContactPhones
	@PhoneXML XML,
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
		@ContactPhoneID BIGINT = 0,
		@OtherContactPhoneID BIGINT = 0,
		@ContactID BIGINT,
		@PhoneID BIGINT,
		@ExistingPhoneID BIGINT,
		@ModifiedOn DATETIME,
		@PhoneCount INT,
		@AdditionalResultXML XML,
		@PhonesXML XML;

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
	IF OBJECT_ID('tempdb..#tmpContactPhones') IS NOT NULL
		DROP TABLE #tmpContactPhones

	CREATE TABLE #tmpContactPhones
	(
		ContactPhoneID BIGINT,
		ContactID BIGINT,
		PhoneID BIGINT,
		ExistingPhoneID BIGINT,
		PhoneTypeID INT, 
		Number NVARCHAR(50), 
		Extension NVARCHAR(10), 
		PhonePermissionID INT, 		
		IsPrimary BIT,
		EffectiveDate DATE,
		ExpirationDate DATE,
		IsActive BIT,
		ModifiedOn DATETIME
	);

	INSERT INTO #tmpContactPhones
	(
		ContactPhoneID,
		PhoneID, 
		PhoneTypeID, 
		Number, 
		Extension, 
		PhonePermissionID,
		IsPrimary,
		EffectiveDate,
		ExpirationDate,
		IsActive,
		ModifiedOn
	)
	SELECT 
		T.C.value('ContactPhoneID[1]', 'BIGINT'), 
		T.C.value('PhoneID[1]', 'BIGINT'), 
		T.C.value('(./PhoneTypeID/text())[1]', 'INT'), 
		T.C.value('Number[1]', 'NVARCHAR(50)'), 
		T.C.value('Extension[1]', 'NVARCHAR(10)'), 
		T.C.value('(./PhonePermissionID/text())[1]', 'INT'),
		T.C.value('IsPrimary[1]', 'BIT'),
		CASE T.C.value('EffectiveDate[1]', 'DATE') WHEN '1900-01-01' THEN NULL ELSE T.C.value('EffectiveDate[1]', 'DATE') END,
		CASE T.C.value('ExpirationDate[1]', 'DATE') WHEN '1900-01-01' THEN NULL ELSE T.C.value('ExpirationDate[1]', 'DATE') END,
		T.C.value('IsActive[1]', 'BIT'),
		T.C.value('ModifiedOn[1]', 'DATETIME')
	FROM 
		@PhoneXML.nodes('RequestXMLValue/Phone') AS T(C);

	UPDATE #tmpContactPhones
	SET ContactID = CE.ContactID
	FROM
		#tmpContactPhones
		INNER JOIN Registration.ContactPhone CE
			ON #tmpContactPhones.ContactPhoneID = CE.ContactPhoneID
	WHERE
		#tmpContactPhones.ContactPhoneID = CE.ContactPhoneID;

	UPDATE tCP
	SET ExistingPhoneID = P.PhoneID
	FROM
		#tmpContactPhones tCP
		INNER JOIN Core.Phone P
			ON ISNULL(tCP.Number, '') = ISNULL(P.Number, '')
			AND ISNULL(tCP.Extension, '') = ISNULL(P.Extension, '')
			AND ISNULL(tCP.PhoneTypeID, '') = ISNULL(P.PhoneTypeID, '')
	WHERE
		ISNULL(tCP.Number, '') = ISNULL(P.Number, '')
		AND ISNULL(tCP.Extension, '') = ISNULL(P.Extension, '')
		AND ISNULL(tCP.PhoneTypeID, '') = ISNULL(P.PhoneTypeID, '');
	
	DECLARE @UpdateCursor CURSOR;
	SET @UpdateCursor = CURSOR FOR
	SELECT ContactPhoneID, ContactID, PhoneID, ExistingPhoneID, IsPrimary, ModifiedOn FROM #tmpContactPhones WHERE ContactPhoneID IS NOT NULL;    

	OPEN @UpdateCursor 
	FETCH NEXT FROM @UpdateCursor 
	INTO @ContactPhoneID, @ContactID, @PhoneID, @ExistingPhoneID, @IsPrimary, @ModifiedOn

	WHILE @@FETCH_STATUS = 0
	BEGIN
	SELECT @OtherContactPhoneID = ContactPhoneID FROM Registration.ContactPhone WHERE ContactID = @ContactID AND IsPrimary = 1 AND ContactPhoneID <> @ContactPhoneID;

	IF ISNULL(@OtherContactPhoneID, 0) > 0 AND @IsPrimary = 1
		BEGIN
		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'ContactPhone', @OtherContactPhoneID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE CE
		SET IsPrimary = 0,
			ModifiedBy = @ModifiedBy,
			ModifiedOn = @ModifiedOn,
			SystemModifiedOn = GETUTCDATE()
		FROM
			Registration.ContactPhone CE 
		WHERE 
			CE.ContactPhoneID = @OtherContactPhoneID;

		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'ContactPhone', @AuditDetailID, @OtherContactPhoneID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT; 
		END

	SET @PhoneCount = (SELECT COUNT(*) FROM Registration.ContactPhone WHERE PhoneID = @PhoneID AND ContactID <> @ContactID);

	SET @PhonesXML = (SELECT * FROM #tmpContactPhones WHERE PhoneID = @PhoneID FOR XML PATH ('Phone'), ROOT ('RequestXMLValue'));

	IF ISNULL(@PhoneCount, 0) > 0
		BEGIN
		EXEC Core.usp_AddPhone @PhonesXML, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AdditionalResultXML OUTPUT;

		SET @PhoneID = (SELECT T.C.value('Identifier[1]', 'BIGINT') FROM @AdditionalResultXML.nodes('OutParameters') AS T (C));
		END

	IF ISNULL(@PhoneCount, 0) = 0 AND @ExistingPhoneID IS NOT NULL
		BEGIN
		SET @PhoneID = @ExistingPhoneID;
		END
	
	IF ISNULL(@PhoneCount, 0) = 0 AND @ExistingPhoneID IS NULL
		BEGIN
		EXEC Core.usp_UpdatePhone @PhonesXML, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT;
		END

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'ContactPhone', @ContactPhoneID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE ce
	SET ce.PhoneID = @PhoneID,
		ce.PhonePermissionID = te.PhonePermissionID,
		ce.IsActive = ISNULL(te.IsActive,1),
		ce.EffectiveDate = te.EffectiveDate,
		ce.ExpirationDate = te.ExpirationDate,
		IsPrimary = te.IsPrimary,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = te.ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	FROM 
		Registration.ContactPhone ce 
		INNER JOIN #tmpContactPhones te
			ON te.ContactPhoneID = ce.ContactPhoneID
	WHERE
		ce.ContactPhoneID = @ContactPhoneID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'ContactPhone', @AuditDetailID, @ContactPhoneID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT; 

	FETCH NEXT FROM @UpdateCursor 
	INTO @ContactPhoneID, @ContactID, @PhoneID, @ExistingPhoneID, @IsPrimary, @ModifiedOn
	END; 

	CLOSE @UpdateCursor;
	DEALLOCATE @UpdateCursor;
			
	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END