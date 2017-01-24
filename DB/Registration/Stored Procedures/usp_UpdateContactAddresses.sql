-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdateContactAddresses]
-- Author:		Saurabh Sahu
-- Date:		07/23/2015
--
-- Purpose:		Update Contact Address Details
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/23/2015	Saurabh Sahu		Modification .
-- 7/31/2015    John Crossen    Change schema from dbo to Registration
-- 08/14/2015	Sumana Sangapu	1227 - Refactor Schema for ContactMethods
-- 08/16/2015	Rajiv Ranjan		Removed IsActiveAddress, IsActiveContactAddress, ModifiedOnAddress, ModifiedOnContactAddress
--									Added MailPermissions, IsPrimary into #tmpContactAddressesesCreated
-- 08/25/2015	Rajiv Ranjan	Added /text() to read xml as a nullable
-- 09/25/2015	Suresh Pandey	Change AptComplexName to ComplexName in #tmpContactAddresses and  @AddressesXML select
-- 10/09/2015   Arun Choudhary	If address is marked as Primary, set other addresses isprimary to 0  
-- 03/03/2016	Kyle Campbell	Added ExpirationDate and EffectiveDate to UPDATE Query
-- 03/07/2016	Kyle Campbell	TFS #5793 Modifed field size for ComplexName and GateCode
-- 03/09/2016	Scott Martin	Added ContactMethodPreferenceID
-- 03/10/2016	Scott Martin	Removed ContactMethodPreferenceID
-- 11/02/2016	Scott Martin	Removed code that would prevent updating if address information was the same but casing was different
-- 12/15/2016	Scott Martin	Updated auditing
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Registration.usp_UpdateContactAddresses
	@AddressesXML XML,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @IsPrimary BIT=0,
		@ContactAddressID BIGINT = 0,
		@OtherContactAddressID BIGINT = 0,
		@ContactID BIGINT,
		@AddressID BIGINT,
		@ExistingAddressID BIGINT,
		@ModifiedOn DATETIME,
		@AuditDetailID BIGINT,
		@AddressCount INT,
		@AdditionalResultXML XML,
		@AddressXML XML,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT @ResultCode = 0,
		   @ResultMessage = 'executed successfully'

	BEGIN TRY
	IF OBJECT_ID('tempdb..#tmpContactAddresses') IS NOT NULL
		DROP TABLE #tmpContactAddresses

	CREATE TABLE #tmpContactAddresses
	(
		ContactAddressID BIGINT,
		ContactID BIGINT,
		AddressID BIGINT,
		ExistingAddressID BIGINT,
		AddressTypeID INT, 
		Line1 NVARCHAR(200), 
		Line2 NVARCHAR(200), 
		City NVARCHAR(200), 
		StateProvince INT, 
		County INT, 
		Zip NVARCHAR(10), 
		ComplexName NVARCHAR(255), 
		GateCode NVARCHAR(50), 
		MailPermissionID INT, 		
		IsPrimary bit,
		EffectiveDate DATE,
		ExpirationDate DATE,
		ModifiedOn DATETIME
	)

	INSERT INTO #tmpContactAddresses
	(
		ContactAddressID,
		AddressID, 
		AddressTypeID, 
		Line1, 
		Line2, 
		City, 
		StateProvince, 
		County, 
		Zip, 
		ComplexName, 
		GateCode, 
		MailPermissionID,			
		IsPrimary,
		EffectiveDate,
		ExpirationDate,
		ModifiedOn
	)
	SELECT 
		T.C.value('ContactAddressID[1]', 'BIGINT'), 
		T.C.value('AddressID[1]', 'BIGINT'), 
		T.C.value('(./AddressTypeID/text())[1]', 'INT'), 
		T.C.value('Line1[1]', 'NVARCHAR(200)'),
		T.C.value('Line2[1]', 'NVARCHAR(200)'),
		T.C.value('City[1]', 'NVARCHAR(200)'),
		T.C.value('(./StateProvince/text())[1]', 'INT'),
		T.C.value('(./County/text())[1]', 'INT'),
		T.C.value('Zip[1]', 'NVARCHAR(10)'),
		T.C.value('ComplexName[1]', 'NVARCHAR(255)'),
		T.C.value('GateCode[1]', 'NVARCHAR(50)'),
		T.C.value('(./MailPermissionID/text())[1]', 'INT'),
		T.C.value('IsPrimary[1]', 'BIT'),
		CASE T.C.value('EffectiveDate[1]', 'DATE') WHEN '1900-01-01' THEN NULL ELSE T.C.value('EffectiveDate[1]', 'DATE') END,
		CASE T.C.value('ExpirationDate[1]', 'DATE') WHEN '1900-01-01' THEN NULL ELSE T.C.value('ExpirationDate[1]', 'DATE') END,
		T.C.value('ModifiedOn[1]', 'DATETIME')
	FROM 
		@AddressesXML.nodes('RequestXMLValue/Address') AS T(C);

	UPDATE #tmpContactAddresses
	SET ContactID = CE.ContactID
	FROM
		#tmpContactAddresses
		INNER JOIN Registration.ContactAddress CE
			ON #tmpContactAddresses.ContactAddressID = CE.ContactAddressID
	WHERE
		#tmpContactAddresses.ContactAddressID = CE.ContactAddressID;

	UPDATE #tmpContactAddresses
	SET ExistingAddressID = A.AddressID
	FROM
		#tmpContactAddresses tCA
		INNER JOIN Core.Addresses A
			ON ISNULL(tCA.AddressTypeID, 0) = ISNULL(A.AddressTypeID, 0)
			AND ISNULL(tCA.Line1, '') = ISNULL(A.Line1, '')
			AND ISNULL(tCA.Line2, '') = ISNULL(A.Line2, '')
			AND ISNULL(tCA.City, '') = ISNULL(A.City, '')
			AND ISNULL(tCA.StateProvince, 0) = ISNULL(A.StateProvince, 0)
			AND ISNULL(tCA.County, 0) = ISNULL(A.County, 0)
			AND ISNULL(tCA.Zip, '') = ISNULL(A.Zip, '')
			AND ISNULL(tCA.ComplexName, '') = ISNULL(A.ComplexName, '')
			AND ISNULL(tCA.GateCode, '') = ISNULL(A.GateCode, '')
	WHERE
		ISNULL(tCA.AddressTypeID, 0) = ISNULL(A.AddressTypeID, 0)
		AND ISNULL(tCA.Line1, '') = ISNULL(A.Line1, '')
		AND ISNULL(tCA.Line2, '') = ISNULL(A.Line2, '')
		AND ISNULL(tCA.City, '') = ISNULL(A.City, '')
		AND ISNULL(tCA.StateProvince, 0) = ISNULL(A.StateProvince, 0)
		AND ISNULL(tCA.County, 0) = ISNULL(A.County, 0)
		AND ISNULL(tCA.Zip, '') = ISNULL(A.Zip, '')
		AND ISNULL(tCA.ComplexName, '') = ISNULL(A.ComplexName, '')
		AND ISNULL(tCA.GateCode, '') = ISNULL(A.GateCode, '')
	
	DECLARE @UpdateCursor CURSOR;
	SET @UpdateCursor = CURSOR FOR
	SELECT ContactAddressID, ContactID, AddressID, ExistingAddressID, IsPrimary, ModifiedOn FROM #tmpContactAddresses WHERE ContactAddressID IS NOT NULL;    

	OPEN @UpdateCursor 
	FETCH NEXT FROM @UpdateCursor 
	INTO @ContactAddressID, @ContactID, @AddressID, @ExistingAddressID, @IsPrimary, @ModifiedOn

	WHILE @@FETCH_STATUS = 0
	BEGIN
	SELECT @OtherContactAddressID = ContactAddressID FROM Registration.ContactAddress WHERE ContactID = @ContactID AND IsPrimary = 1 AND ContactAddressID <> @ContactAddressID;

	IF ISNULL(@OtherContactAddressID, 0) > 0 AND @IsPrimary = 1
		BEGIN
		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'ContactAddress', @OtherContactAddressID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE CE
		SET IsPrimary = 0,
			ModifiedBy = @ModifiedBy,
			ModifiedOn = @ModifiedOn,
			SystemModifiedOn = GETUTCDATE()
		FROM
			Registration.ContactAddress CE 
		WHERE 
			CE.ContactAddressID = @OtherContactAddressID;

		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'ContactAddress', @AuditDetailID, @OtherContactAddressID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT; 
		END

	SET @AddressCount = (SELECT COUNT(*) FROM Registration.ContactAddress WHERE AddressID = @AddressID AND ContactID <> @ContactID);

	SET @AddressXML = (SELECT * FROM #tmpContactAddresses WHERE AddressID = @AddressID FOR XML PATH ('Address'), ROOT ('RequestXMLValue'));

	IF ISNULL(@AddressCount, 0) > 0
		BEGIN
		EXEC Core.usp_AddAddress @AddressXML, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AdditionalResultXML OUTPUT;

		SET @AddressID = (SELECT T.C.value('Identifier[1]', 'BIGINT') FROM @AdditionalResultXML.nodes('OutParameters') AS T (C));
		END

	IF ISNULL(@AddressCount, 0) = 0 AND @ExistingAddressID IS NOT NULL
		BEGIN
		SET @AddressID = @ExistingAddressID;
		END
	ELSE
		BEGIN
		EXEC Core.usp_UpdateAddress @AddressXML, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT;
		END

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'ContactAddress', @ContactAddressID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE ce
	SET ce.AddressID = @AddressID,
		ce.MailPermissionID = te.MailPermissionID,
		IsPrimary = te.IsPrimary,
		EffectiveDate = te.EffectiveDate,
		ExpirationDate = te.ExpirationDate,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = te.ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	FROM 
		Registration.ContactAddress ce 
		INNER JOIN #tmpContactAddresses te
			ON te.ContactAddressID = ce.ContactAddressID
	WHERE
		ce.ContactAddressID = @ContactAddressID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'ContactAddress', @AuditDetailID, @ContactAddressID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT; 

	FETCH NEXT FROM @UpdateCursor 
	INTO @ContactAddressID, @ContactID, @AddressID, @ExistingAddressID, @IsPrimary, @ModifiedOn
	END; 

	CLOSE @UpdateCursor;
	DEALLOCATE @UpdateCursor;
			
	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END
