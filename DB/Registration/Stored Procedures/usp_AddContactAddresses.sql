-----------------------------------------------------------------------------------------------------------------------
-- Procedure:  [usp_AddContactAddresses]
-- Author:		Saurabh Sahu
-- Date:		07/23/2015
--
-- Purpose:		Add Contact Address Details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		Dependency on Contact Table (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 07/23/2015	Saurabh Sahu			Modification .
-- 07/30/2015	Demetrios Christopher	Updated to reflect new column name for StateProvince
-- 7/31/2015    John Crossen    Change schema from dbo to Registration
-- 08/03/2015   Sumana Sangapu	1016 - Changed proc name schema qualifier from dbo to Registration/Core/Reference
-- 08/14/2015	Sumana Sangapu  1227 - Refactor ContactMethods Schema
-- 08/15/2015	Rajiv Ranjan			Added IsPrimary into #tmpAddressesCreated
-- 08/25/2015	Rajiv Ranjan	Added /text() to read xml as a nullable
-- 09/23/2015	Sumana Sangapu	Added MERGE for the ContactAddress table
-- 09/29/2015   Arun Choudhary  Old Address marked as non primary.
-- 10/08/2015	Arun Choudhary  In case of non primary address, avoid to make other address to non primary
-- 10/20/2015   Vipul Singhal   In case of addition if record is matched in the Registration.ContactAddress(that is soft deleted) then make active that record
-- 01/14/2016	Scott Martin	Added ModifiedOn parameter, added SystemModifiedOn field, added CreatedBy and CreatedOn to Insert
-- 03/02/2016	Kyle Campbell	Added EffectiveDate and ExpirationDate fields
-- 03/07/2016	Kyle Campbell	TFS #5793 Modifed field size for ComplexName and GateCode
-- 03/09/2016	Scott Martin	Added ContactMethodPreferenceID
-- 03/10/2016	Scott Martin	Removed ContactMethodPreferenceID
-- 03/06/2016	Kyle Campbell	Refactored proc to prevent 1900-01-01 from being inserted by XML when value should be NULL
-- 12/15/2016	Scott Martin	Updated auditing
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_AddContactAddresses]
	@ContactID bigint,
	@AddressesXML xml,
	@ModifiedBy int,
	@ResultCode int OUTPUT,
	@ResultMessage nvarchar(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@AuditCursor CURSOR,
		@ModifiedOn DATETIME,
		@AdditionalResultXML XML,
		@IsPrimary BIT = 0,
		@ContactAddressID BIGINT,
		@AddressId BIGINT;

SELECT
    @ResultCode = 0,
    @ResultMessage = 'Executed Successfully';

	BEGIN TRY

	-- iterate through the addresses in the passed in xml, and each time an address is created, also associate the address w/ the contact
	IF OBJECT_ID('tempdb..#tmpContactAddresses') IS NOT NULL
		DROP TABLE #tmpContactAddresses

	CREATE TABLE #tmpContactAddresses 
	(
		ContactAddressID BIGINT,
		AddressID BIGINT,
		AddressTypeID int,
		Line1 nvarchar(200),
		Line2 nvarchar(200),
		City nvarchar(200),
		StateProvince int,
		County int,
		Zip nvarchar(10),
		ComplexName nvarchar(255),
		GateCode nvarchar(50),
		MailPermissionID int,
		IsPrimary BIT,
		EffectiveDate date,
		ExpirationDate date,
		ModifiedOn DATETIME,
		AuditDetailID BIGINT
	);

	INSERT INTO #tmpContactAddresses 
	(
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
		@AddressesXML.nodes('RequestXMLValue/Address') AS T (C);
	
	EXEC Core.usp_AddAddress @AddressesXML, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AdditionalResultXML OUTPUT;

	UPDATE #tmpContactAddresses
	SET AddressID = A.AddressID
	FROM
		#tmpContactAddresses tCA
		INNER JOIN Core.Addresses A
			ON ISNULL(tCA.AddressTypeID, 0) = ISNULL(A.AddressTypeID, 0)
			AND	ISNULL(tCA.Line1, '') = ISNULL(A.Line1, '')
			AND ISNULL(tCA.Line2, '') = ISNULL(A.Line2, '')
			AND ISNULL(tCA.City, '') = ISNULL(A.City, '')
			AND ISNULL(tCA.StateProvince, 0) = ISNULL(A.StateProvince, 0)
			AND ISNULL(tCA.County, 0) = ISNULL(A.County, 0)
			AND ISNULL(tCA.Zip, '') = ISNULL(A.Zip, '')
			AND ISNULL(tCA.ComplexName, '') = ISNULL(A.ComplexName, '')
			AND ISNULL(tCA.GateCode, '') = ISNULL(A.GateCode, '')
			AND A.AddressID IN (SELECT T.C.value('Identifier[1]', 'BIGINT') FROM @AdditionalResultXML.nodes('OutParameters') AS T (C))
	WHERE
		ISNULL(tCA.AddressTypeID, 0) = ISNULL(A.AddressTypeID, 0)
		AND	ISNULL(tCA.Line1, '') = ISNULL(A.Line1, '')
		AND ISNULL(tCA.Line2, '') = ISNULL(A.Line2, '')
		AND ISNULL(tCA.City, '') = ISNULL(A.City, '')
		AND ISNULL(tCA.StateProvince, 0) = ISNULL(A.StateProvince, 0)
		AND ISNULL(tCA.County, 0) = ISNULL(A.County, 0)
		AND ISNULL(tCA.Zip, '') = ISNULL(A.Zip, '')
		AND ISNULL(tCA.ComplexName, '') = ISNULL(A.ComplexName, '')
		AND ISNULL(tCA.GateCode, '') = ISNULL(A.GateCode, '');

	UPDATE #tmpContactAddresses
	SET ContactAddressID = CA.ContactAddressID
	FROM
		#tmpContactAddresses tCA
		INNER JOIN Registration.ContactAddress CA
			ON tCA.AddressID = CA.AddressID
			AND CA.ContactID = @ContactID
	WHERE
		tCA.AddressID = CA.AddressID
		AND CA.ContactID = @ContactID;

	--Select statements purposely set this way so the procedure will fail if more than 1 primary address is found
	SET @IsPrimary = (SELECT IsPrimary FROM #tmpContactAddresses WHERE IsPrimary = 1);

	SELECT @AddressId = AddressID, @ModifiedOn = ModifiedOn FROM #tmpContactAddresses WHERE IsPrimary = 1;

	SET @ContactAddressID = (SELECT ContactAddressID FROM Registration.ContactAddress WHERE ContactID = @ContactID AND AddressID <> @AddressId AND IsPrimary = 1);

	--If the address to be inserted IsPrimary = 1 and there is an existing primary, set the existing primary to 0
	IF @ContactAddressID IS NOT NULL AND @IsPrimary = 1
		BEGIN
		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'ContactAddress', @ContactAddressID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE Registration.ContactAddress
		SET IsPrimary = 0,
			ModifiedBy = @ModifiedBy,
			ModifiedOn = @ModifiedOn,
			SystemModifiedOn = GETUTCDATE()
		WHERE	
			ContactAddressID = @ContactAddressID;

		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'ContactAddress', @AuditDetailID, @ContactAddressID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		END

	IF EXISTS (SELECT TOP 1 * FROM #tmpContactAddresses WHERE ContactAddressID IS NOT NULL)
		BEGIN
			SET @AuditCursor = CURSOR FOR
			SELECT ContactAddressID, ModifiedOn FROM #tmpContactAddresses WHERE ContactAddressID IS NOT NULL;    

			OPEN @AuditCursor 
			FETCH NEXT FROM @AuditCursor 
			INTO @ContactAddressID, @ModifiedOn

			WHILE @@FETCH_STATUS = 0
			BEGIN
			EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'ContactAddress', @ContactAddressID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			
			UPDATE #tmpContactAddresses
			SET AuditDetailID = @AuditDetailID
			WHERE
				ContactAddressID = @ContactAddressID;

			FETCH NEXT FROM @AuditCursor 
			INTO @ContactAddressID, @ModifiedOn
			END; 

			CLOSE @AuditCursor;
			DEALLOCATE @AuditCursor;
		END;

	-- Merge statement for Registration.ContactAddress
	DECLARE	@tmpContactAddresses TABLE
	(
		Operation varchar(10),
		ContactAddressID INT,
		ModifiedOn DATETIME
	);

	MERGE INTO Registration.ContactAddress AS TARGET
	USING (SELECT * FROM #tmpContactAddresses t)  AS SOURCE
		ON ISNULL(SOURCE.ContactAddressID, 0) = TARGET.ContactAddressID
	WHEN NOT MATCHED THEN
		INSERT
		(
			ContactID,
			AddressID,
			MailPermissionID,
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
			SOURCE.AddressID,
			SOURCE.MailPermissionID,
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
			TARGET.MailPermissionID = SOURCE.MailPermissionID,
			TARGET.EffectiveDate = SOURCE.EffectiveDate,
			TARGET.ExpirationDate = SOURCE.ExpirationDate,
			TARGET.IsActive = 1,
			TARGET.ModifiedBy = @ModifiedBy,
			TARGET.ModifiedOn = SOURCE.ModifiedOn,
			TARGET.SystemModifiedOn = GETUTCDATE()
	OUTPUT
		$action,
		inserted.ContactAddressID,
		inserted.ModifiedOn
	INTO
		@tmpContactAddresses;	

	IF EXISTS (SELECT TOP 1 * FROM @tmpContactAddresses WHERE Operation = 'Insert')
		BEGIN
		BEGIN
			SET @AuditCursor = CURSOR FOR
			SELECT ContactAddressID, ModifiedOn FROM @tmpContactAddresses WHERE Operation = 'Insert';    

			OPEN @AuditCursor 
			FETCH NEXT FROM @AuditCursor 
			INTO @ContactAddressID, @ModifiedOn

			WHILE @@FETCH_STATUS = 0
			BEGIN
			EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'ContactAddress', @ContactAddressID, NULL, NULL, NULL, @ContactID, 2, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

			EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'ContactAddress', @AuditDetailID, @ContactAddressID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

			FETCH NEXT FROM @AuditCursor 
			INTO @ContactAddressID, @ModifiedOn
			END; 

			CLOSE @AuditCursor;
			DEALLOCATE @AuditCursor;
		END;
		END

	IF EXISTS (SELECT TOP 1 * FROM #tmpContactAddresses WHERE ContactAddressID IS NOT NULL)
		BEGIN
		BEGIN
			SET @AuditCursor = CURSOR FOR
			SELECT ContactAddressID, ModifiedOn, AuditDetailID FROM #tmpContactAddresses WHERE ContactAddressID IS NOT NULL;   

			OPEN @AuditCursor 
			FETCH NEXT FROM @AuditCursor 
			INTO @ContactAddressID, @ModifiedOn, @AuditDetailID

			WHILE @@FETCH_STATUS = 0
			BEGIN
			EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'ContactAddress', @AuditDetailID, @ContactAddressID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

			FETCH NEXT FROM @AuditCursor 
			INTO @ContactAddressID, @ModifiedOn, @AuditDetailID
			END; 

			CLOSE @AuditCursor;
			DEALLOCATE @AuditCursor;
		END;
		END

  END TRY
  BEGIN CATCH
    SELECT
      @ResultCode = ERROR_SEVERITY(),
      @ResultMessage = ERROR_MESSAGE()
  END CATCH
END