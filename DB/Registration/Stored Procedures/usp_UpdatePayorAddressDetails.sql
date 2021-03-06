--------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdatePayorAddressDetails]
-- Author:		Sumana Sangapu
-- Date:		12/19/2016
--
-- Purpose:		Update Payor Address Details
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		N/A (or any dependencies such as other procs or functions)
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/19/2016	Sumana Sangapu	Initial Creation.
-- 12/23/2016	Vishal Yadav	Modified ElectronicPayorID & ContactID size to 50.
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_UpdatePayorAddressDetails]
	@AddressesXML XML,
	@ElectronicPayorID NVARCHAR(50),
	@ContractID NVARCHAR(50),
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
AS
BEGIN
DECLARE 
		@PayorAddressID BIGINT = 0,
		@OtherPayorAddressID BIGINT = 0,
		@PayorPlanID BIGINT,
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
	IF OBJECT_ID('tempdb..#tmpPayorAddress') IS NOT NULL
		DROP TABLE #tmpPayorAddress

	CREATE TABLE #tmpPayorAddress
	(
		PayorAddressID BIGINT,
		PayorPlanID BIGINT,
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

	INSERT INTO #tmpPayorAddress
	(
		PayorAddressID,
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
		T.C.value('PayorAddressID[1]', 'BIGINT'), 
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

	UPDATE #tmpPayorAddress
	SET PayorPlanID = CE.PayorPlanID
	FROM
		#tmpPayorAddress
		INNER JOIN Registration.PayorAddress CE
			ON #tmpPayorAddress.PayorAddressID = CE.PayorAddressID
	WHERE
		#tmpPayorAddress.PayorAddressID = CE.PayorAddressID;

	UPDATE #tmpPayorAddress
	SET ExistingAddressID = A.AddressID
	FROM
		#tmpPayorAddress tCA
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
	SELECT PayorAddressID, PayorPlanID, AddressID, ExistingAddressID, ModifiedOn FROM #tmpPayorAddress WHERE PayorAddressID IS NOT NULL;    

	OPEN @UpdateCursor 
	FETCH NEXT FROM @UpdateCursor 
	INTO @PayorAddressID, @PayorPlanID, @AddressID, @ExistingAddressID,  @ModifiedOn

	WHILE @@FETCH_STATUS = 0
	BEGIN
	SELECT @OtherPayorAddressID = PayorAddressID FROM Registration.PayorAddress WHERE PayorPlanID = @PayorPlanID AND PayorAddressID <> @PayorAddressID;

	SET @AddressCount = (SELECT COUNT(*) FROM Registration.PayorAddress WHERE AddressID = @AddressID AND PayorPlanID <> @PayorPlanID);

	SET @AddressXML = (SELECT * FROM #tmpPayorAddress WHERE AddressID = @AddressID FOR XML PATH ('Address'), ROOT ('RequestXMLValue'));

	IF ISNULL(@AddressCount, 0) > 0
		BEGIN
		EXEC Core.usp_AddAddress @AddressXML, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AdditionalResultXML OUTPUT;

		SET @AddressID = (SELECT T.C.value('Identifier[1]', 'BIGINT') FROM @AdditionalResultXML.nodes('OutParameters') AS T (C));
		END

	IF ISNULL(@AddressCount, 0) = 0 AND @ExistingAddressID IS NOT NULL
		BEGIN
		SET @AddressID = @ExistingAddressID;
		END
	
	IF ISNULL(@AddressCount, 0) = 0 AND @ExistingAddressID IS NULL
		BEGIN
		EXEC Core.usp_UpdateAddress @AddressXML, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT;
		END

	EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'PayorAddress', @PayorAddressID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

	UPDATE ce
	SET 
		PayorPlanID = te.PayorPlanID,
		ce.AddressID = @AddressID,
		ContactID = @ContractID,
		ElectronicPayorID = @ElectronicPayorID,
		EffectiveDate = te.EffectiveDate,
		ExpirationDate = te.ExpirationDate,
		ModifiedBy = @ModifiedBy,
		ModifiedOn = te.ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	FROM 
		Registration.PayorAddress ce 
		INNER JOIN #tmpPayorAddress te
			ON te.PayorAddressID = ce.PayorAddressID
	WHERE
		ce.PayorAddressID = @PayorAddressID;

	EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'PayorAddress', @AuditDetailID, @PayorAddressID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT; 

	FETCH NEXT FROM @UpdateCursor 
	INTO @PayorAddressID, @PayorPlanID, @AddressID, @ExistingAddressID, @ModifiedOn
	END; 

	CLOSE @UpdateCursor;
	DEALLOCATE @UpdateCursor;
			
	END TRY

	BEGIN CATCH
		SELECT @ResultCode = ERROR_SEVERITY(),
				@ResultMessage = ERROR_MESSAGE()
	END CATCH
END