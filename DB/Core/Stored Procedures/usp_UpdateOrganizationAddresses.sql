-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdateOrganizationAddresses]
-- Author:		Scott Martin
-- Date:		12/21/2016
--
-- Purpose:		Update Organization Address Details
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/21/2016	Scott Martin	Initial Creation
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE Core.usp_UpdateOrganizationAddresses
	@AddressesXML XML,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE @IsPrimary BIT=0,
		@OrganizationAddressID BIGINT = 0,
		@OtherOrganizationAddressID BIGINT = 0,
		@DetailID BIGINT,
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
	IF OBJECT_ID('tempdb..#tmpOrganizationAddresses') IS NOT NULL
		DROP TABLE #tmpOrganizationAddresses

	CREATE TABLE #tmpOrganizationAddresses
	(
		OrganizationAddressID BIGINT,
		DetailID BIGINT,
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

	INSERT INTO #tmpOrganizationAddresses
	(
		OrganizationAddressID,
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
		T.C.value('OrganizationAddressID[1]', 'BIGINT'), 
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

	UPDATE #tmpOrganizationAddresses
	SET DetailID = OA.DetailID
	FROM
		#tmpOrganizationAddresses tOA
		INNER JOIN Core.OrganizationAddress OA
			ON tOA.OrganizationAddressID = OA.OrganizationAddressID
	WHERE
		tOA.OrganizationAddressID = OA.OrganizationAddressID;

	UPDATE #tmpOrganizationAddresses
	SET ExistingAddressID = A.AddressID
	FROM
		#tmpOrganizationAddresses tOA
		INNER JOIN Core.Addresses A
			ON ISNULL(tOA.AddressTypeID, 0) = ISNULL(A.AddressTypeID, 0)
			AND ISNULL(tOA.Line1, '') = ISNULL(A.Line1, '')
			AND ISNULL(tOA.Line2, '') = ISNULL(A.Line2, '')
			AND ISNULL(tOA.City, '') = ISNULL(A.City, '')
			AND ISNULL(tOA.StateProvince, 0) = ISNULL(A.StateProvince, 0)
			AND ISNULL(tOA.County, 0) = ISNULL(A.County, 0)
			AND ISNULL(tOA.Zip, '') = ISNULL(A.Zip, '')
			AND ISNULL(tOA.ComplexName, '') = ISNULL(A.ComplexName, '')
			AND ISNULL(tOA.GateCode, '') = ISNULL(A.GateCode, '')
	WHERE
		ISNULL(tOA.AddressTypeID, 0) = ISNULL(A.AddressTypeID, 0)
		AND ISNULL(tOA.Line1, '') = ISNULL(A.Line1, '')
		AND ISNULL(tOA.Line2, '') = ISNULL(A.Line2, '')
		AND ISNULL(tOA.City, '') = ISNULL(A.City, '')
		AND ISNULL(tOA.StateProvince, 0) = ISNULL(A.StateProvince, 0)
		AND ISNULL(tOA.County, 0) = ISNULL(A.County, 0)
		AND ISNULL(tOA.Zip, '') = ISNULL(A.Zip, '')
		AND ISNULL(tOA.ComplexName, '') = ISNULL(A.ComplexName, '')
		AND ISNULL(tOA.GateCode, '') = ISNULL(A.GateCode, '')
	
	DECLARE @UpdateCursor CURSOR;
	SET @UpdateCursor = CURSOR FOR
	SELECT OrganizationAddressID, DetailID, AddressID, ExistingAddressID, IsPrimary, ModifiedOn FROM #tmpOrganizationAddresses WHERE OrganizationAddressID IS NOT NULL;    

	OPEN @UpdateCursor 
	FETCH NEXT FROM @UpdateCursor 
	INTO @OrganizationAddressID, @DetailID, @AddressID, @ExistingAddressID, @IsPrimary, @ModifiedOn

	WHILE @@FETCH_STATUS = 0
	BEGIN
	SELECT @OtherOrganizationAddressID = OrganizationAddressID FROM Core.OrganizationAddress WHERE DetailID = @DetailID AND IsPrimary = 1 AND OrganizationAddressID <> @OrganizationAddressID;

	IF ISNULL(@OtherOrganizationAddressID, 0) > 0 AND @IsPrimary = 1
		BEGIN
		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'OrganizationAddress', @OtherOrganizationAddressID, NULL, NULL, NULL, NULL, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE OA
		SET IsPrimary = 0,
			ModifiedBy = @ModifiedBy,
			ModifiedOn = @ModifiedOn,
			SystemModifiedOn = GETUTCDATE()
		FROM
			Core.OrganizationAddress OA 
		WHERE 
			OA.OrganizationAddressID = @OtherOrganizationAddressID;

		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'OrganizationAddress', @AuditDetailID, @OtherOrganizationAddressID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT; 
		END

	SET @AddressCount = (SELECT Core.fn_GetAddressMappingCount(@AddressID, 3)) + (SELECT COUNT(*) FROM Core.OrganizationAddress WHERE AddressID = @AddressID AND DetailID <> @DetailID);

	SET @AddressXML = (SELECT * FROM #tmpOrganizationAddresses WHERE AddressID = @AddressID FOR XML PATH ('Address'), ROOT ('RequestXMLValue'));

	IF ISNULL(@AddressCount, 0) > 0
		BEGIN
		EXEC Core.usp_AddAddress @AddressXML, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AdditionalResultXML OUTPUT;

		SET @AddressID = (SELECT T.C.value('Identifier[1]', 'BIGINT') FROM @AdditionalResultXML.nodes('OutParameters') AS T (C));
		END
	ELSE
		BEGIN
		EXEC Core.usp_UpdateAddress @AddressXML, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT;
		END

	EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'OrganizationAddress', @OrganizationAddressID, NULL, NULL, NULL, NULL, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE OA
	SET OA.AddressID = @AddressID,
		IsPrimary = tOA.IsPrimary,
		EffectiveDate = tOA.EffectiveDate,
		ExpirationDate = tOA.ExpirationDate,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = tOA.ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	FROM 
		Core.OrganizationAddress OA 
		INNER JOIN #tmpOrganizationAddresses tOA
			ON OA.OrganizationAddressID = tOA.OrganizationAddressID
	WHERE
		OA.OrganizationAddressID = @OrganizationAddressID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'OrganizationAddress', @AuditDetailID, @OrganizationAddressID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT; 

	FETCH NEXT FROM @UpdateCursor 
	INTO @OrganizationAddressID, @DetailID, @AddressID, @ExistingAddressID, @IsPrimary, @ModifiedOn
	END; 

	CLOSE @UpdateCursor;
	DEALLOCATE @UpdateCursor;
			
	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END
