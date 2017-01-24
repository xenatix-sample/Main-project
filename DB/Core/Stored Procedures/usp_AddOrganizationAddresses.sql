-----------------------------------------------------------------------------------------------------------------------
-- Procedure:  [usp_AddOrganizationAddresses]
-- Author:		Scott Martin
-- Date:		12/21/2016
--
-- Purpose:		Add Organization Address Details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		Dependency on Organization Detail Table 
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/21/2016	Scott Martin	Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_AddOrganizationAddresses]
	@DetailID bigint,
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
		@OrganizationAddressID BIGINT,
		@AddressId BIGINT;

SELECT
    @ResultCode = 0,
    @ResultMessage = 'Executed Successfully';

	BEGIN TRY

	-- iterate through the addresses in the passed in xml, and each time an address is created, also associate the address w/ the contact
	IF OBJECT_ID('tempdb..#tmpOrganizationAddresses') IS NOT NULL
		DROP TABLE #tmpOrganizationAddresses

	CREATE TABLE #tmpOrganizationAddresses 
	(
		OrganizationAddressID BIGINT,
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

	INSERT INTO #tmpOrganizationAddresses 
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

	UPDATE #tmpOrganizationAddresses
	SET AddressID = A.AddressID
	FROM
		#tmpOrganizationAddresses tOA
		INNER JOIN Core.Addresses A
			ON ISNULL(tOA.AddressTypeID, 0) = ISNULL(A.AddressTypeID, 0)
			AND	ISNULL(tOA.Line1, '') = ISNULL(A.Line1, '')
			AND ISNULL(tOA.Line2, '') = ISNULL(A.Line2, '')
			AND ISNULL(tOA.City, '') = ISNULL(A.City, '')
			AND ISNULL(tOA.StateProvince, 0) = ISNULL(A.StateProvince, 0)
			AND ISNULL(tOA.County, 0) = ISNULL(A.County, 0)
			AND ISNULL(tOA.Zip, '') = ISNULL(A.Zip, '')
			AND ISNULL(tOA.ComplexName, '') = ISNULL(A.ComplexName, '')
			AND ISNULL(tOA.GateCode, '') = ISNULL(A.GateCode, '')
			AND A.AddressID IN (SELECT T.C.value('Identifier[1]', 'BIGINT') FROM @AdditionalResultXML.nodes('OutParameters') AS T (C))
	WHERE
		ISNULL(tOA.AddressTypeID, 0) = ISNULL(A.AddressTypeID, 0)
		AND	ISNULL(tOA.Line1, '') = ISNULL(A.Line1, '')
		AND ISNULL(tOA.Line2, '') = ISNULL(A.Line2, '')
		AND ISNULL(tOA.City, '') = ISNULL(A.City, '')
		AND ISNULL(tOA.StateProvince, 0) = ISNULL(A.StateProvince, 0)
		AND ISNULL(tOA.County, 0) = ISNULL(A.County, 0)
		AND ISNULL(tOA.Zip, '') = ISNULL(A.Zip, '')
		AND ISNULL(tOA.ComplexName, '') = ISNULL(A.ComplexName, '')
		AND ISNULL(tOA.GateCode, '') = ISNULL(A.GateCode, '');

	UPDATE #tmpOrganizationAddresses
	SET OrganizationAddressID = OA.OrganizationAddressID
	FROM
		#tmpOrganizationAddresses tOA
		INNER JOIN Core.OrganizationAddress OA
			ON tOA.AddressID = OA.AddressID
			AND OA.DetailID = @DetailID
	WHERE
		tOA.AddressID = OA.AddressID
		AND OA.DetailID = @DetailID;

	--Select statements purposely set this way so the procedure will fail if more than 1 primary address is found
	SET @IsPrimary = (SELECT IsPrimary FROM #tmpOrganizationAddresses WHERE IsPrimary = 1);

	SELECT @AddressId = AddressID, @ModifiedOn = ModifiedOn FROM #tmpOrganizationAddresses WHERE IsPrimary = 1;

	SET @OrganizationAddressID = (SELECT OrganizationAddressID FROM Core.OrganizationAddress WHERE DetailID = @DetailID AND AddressID <> @AddressId AND IsPrimary = 1);

	--If the address to be inserted IsPrimary = 1 and there is an existing primary, set the existing primary to 0
	IF @OrganizationAddressID IS NOT NULL AND @IsPrimary = 1
		BEGIN
		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'OrganizationAddress', @OrganizationAddressID, NULL, NULL, NULL, NULL, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE Core.OrganizationAddress
		SET IsPrimary = 0,
			ModifiedBy = @ModifiedBy,
			ModifiedOn = @ModifiedOn,
			SystemModifiedOn = GETUTCDATE()
		WHERE	
			OrganizationAddressID = @OrganizationAddressID;

		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'OrganizationAddress', @AuditDetailID, @OrganizationAddressID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		END

	IF EXISTS (SELECT TOP 1 * FROM #tmpOrganizationAddresses WHERE OrganizationAddressID IS NOT NULL)
		BEGIN
			SET @AuditCursor = CURSOR FOR
			SELECT OrganizationAddressID, ModifiedOn FROM #tmpOrganizationAddresses WHERE OrganizationAddressID IS NOT NULL;    

			OPEN @AuditCursor 
			FETCH NEXT FROM @AuditCursor 
			INTO @OrganizationAddressID, @ModifiedOn

			WHILE @@FETCH_STATUS = 0
			BEGIN
			EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'OrganizationAddress', @OrganizationAddressID, NULL, NULL, NULL, NULL, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			
			UPDATE #tmpOrganizationAddresses
			SET AuditDetailID = @AuditDetailID
			WHERE
				OrganizationAddressID = @OrganizationAddressID;

			FETCH NEXT FROM @AuditCursor 
			INTO @OrganizationAddressID, @ModifiedOn
			END; 

			CLOSE @AuditCursor;
			DEALLOCATE @AuditCursor;
		END;

	-- Merge statement for Registration.ContactAddress
	DECLARE	@tmpOrgAddresses TABLE
	(
		Operation varchar(10),
		ContactAddressID INT,
		ModifiedOn DATETIME
	);

	MERGE INTO Core.OrganizationAddress AS TARGET
	USING (SELECT * FROM #tmpOrganizationAddresses t)  AS SOURCE
		ON ISNULL(SOURCE.OrganizationAddressID, 0) = TARGET.OrganizationAddressID
	WHEN NOT MATCHED THEN
		INSERT
		(
			DetailID,
			AddressID,
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
			@DetailID,
			SOURCE.AddressID,
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
			TARGET.IsActive = 1,
			TARGET.ModifiedBy = @ModifiedBy,
			TARGET.ModifiedOn = SOURCE.ModifiedOn,
			TARGET.SystemModifiedOn = GETUTCDATE()
	OUTPUT
		$action,
		inserted.OrganizationAddressID,
		inserted.ModifiedOn
	INTO
		@tmpOrgAddresses;	

	IF EXISTS (SELECT TOP 1 * FROM @tmpOrgAddresses WHERE Operation = 'Insert')
		BEGIN
		BEGIN
			SET @AuditCursor = CURSOR FOR
			SELECT ContactAddressID, ModifiedOn FROM @tmpOrgAddresses WHERE Operation = 'Insert';    

			OPEN @AuditCursor 
			FETCH NEXT FROM @AuditCursor 
			INTO @OrganizationAddressID, @ModifiedOn

			WHILE @@FETCH_STATUS = 0
			BEGIN
			EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Core', 'OrganizationAddress', @OrganizationAddressID, NULL, NULL, NULL, NULL, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

			EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Core', 'OrganizationAddress', @AuditDetailID, @OrganizationAddressID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

			FETCH NEXT FROM @AuditCursor 
			INTO @OrganizationAddressID, @ModifiedOn
			END; 

			CLOSE @AuditCursor;
			DEALLOCATE @AuditCursor;
		END;
		END

	IF EXISTS (SELECT TOP 1 * FROM #tmpOrganizationAddresses WHERE OrganizationAddressID IS NOT NULL)
		BEGIN
		BEGIN
			SET @AuditCursor = CURSOR FOR
			SELECT OrganizationAddressID, ModifiedOn, AuditDetailID FROM #tmpOrganizationAddresses WHERE OrganizationAddressID IS NOT NULL;   

			OPEN @AuditCursor 
			FETCH NEXT FROM @AuditCursor 
			INTO @OrganizationAddressID, @ModifiedOn, @AuditDetailID

			WHILE @@FETCH_STATUS = 0
			BEGIN
			EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'OrganizationAddress', @AuditDetailID, @OrganizationAddressID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

			FETCH NEXT FROM @AuditCursor 
			INTO @OrganizationAddressID, @ModifiedOn, @AuditDetailID
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