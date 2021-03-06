
-----------------------------------------------------------------------------------------------------------------------
-- Procedure:   [usp_AddPayorAddressDetails]
-- Author:		Sumana Sangapu
-- Date:		07/23/2015
--
-- Purpose:		Add Payor Address Details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		 
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 12/19/2015	Sumana Sangapu		Initial Creation
-- 12/20/2016   Atul Chauhan		Will return generated PayorAddressID
-- 12/23/2016	Atul Chauhan	    As per PBI 17995 modified ElectronicPayorID & ContactID size to 50
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Registration].[usp_AddPayorAddressDetails]
	@PayorPlanID bigint,
	@AddressesXML xml,
	@ElectronicPayorID NVARCHAR(50),
	@ContractID NVARCHAR(50),
	@ModifiedBy int,
	@ResultCode int OUTPUT,
	@ResultMessage nvarchar(500) OUTPUT,
	@ID BIGINT OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID),
		@AuditCursor CURSOR,
		@ModifiedOn DATETIME,
		@AdditionalResultXML XML,
		@IsPrimary BIT = 0,
		@PayorAddressID BIGINT,
		@AddressId BIGINT;

SELECT
    @ResultCode = 0,
    @ResultMessage = 'Executed Successfully';

	BEGIN TRY

	-- iterate through the addresses in the passed in xml, and each time an address is created, also associate the address w/ the contact
	IF OBJECT_ID('tempdb..#tmpPayorAddress') IS NOT NULL
		DROP TABLE #tmpPayorAddress

	CREATE TABLE #tmpPayorAddress 
	(
		PayorAddressID BIGINT,
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
		EffectiveDate date,
		ExpirationDate date,
		ModifiedOn DATETIME,
		AuditDetailID BIGINT
	);

	INSERT INTO #tmpPayorAddress 
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
		CASE T.C.value('EffectiveDate[1]', 'DATE') WHEN '1900-01-01' THEN NULL ELSE T.C.value('EffectiveDate[1]', 'DATE') END,
		CASE T.C.value('ExpirationDate[1]', 'DATE') WHEN '1900-01-01' THEN NULL ELSE T.C.value('ExpirationDate[1]', 'DATE') END,
		T.C.value('ModifiedOn[1]', 'DATETIME')
	FROM 
		@AddressesXML.nodes('RequestXMLValue/Address') AS T (C);
	
	EXEC Core.usp_AddAddress @AddressesXML, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AdditionalResultXML OUTPUT;

	UPDATE #tmpPayorAddress
	SET AddressID = A.AddressID
	FROM
		#tmpPayorAddress tPA
		INNER JOIN Core.Addresses A
			ON ISNULL(tPA.AddressTypeID, 0) = ISNULL(A.AddressTypeID, 0)
			AND	ISNULL(tPA.Line1, '') = ISNULL(A.Line1, '')
			AND ISNULL(tPA.Line2, '') = ISNULL(A.Line2, '')
			AND ISNULL(tPA.City, '') = ISNULL(A.City, '')
			AND ISNULL(tPA.StateProvince, 0) = ISNULL(A.StateProvince, 0)
			AND ISNULL(tPA.County, 0) = ISNULL(A.County, 0)
			AND ISNULL(tPA.Zip, '') = ISNULL(A.Zip, '')
			AND ISNULL(tPA.ComplexName, '') = ISNULL(A.ComplexName, '')
			AND ISNULL(tPA.GateCode, '') = ISNULL(A.GateCode, '')
			AND A.AddressID IN (SELECT T.C.value('Identifier[1]', 'BIGINT') FROM @AdditionalResultXML.nodes('OutParameters') AS T (C))
	WHERE
		ISNULL(tPA.AddressTypeID, 0) = ISNULL(A.AddressTypeID, 0)
		AND	ISNULL(tPA.Line1, '') = ISNULL(A.Line1, '')
		AND ISNULL(tPA.Line2, '') = ISNULL(A.Line2, '')
		AND ISNULL(tPA.City, '') = ISNULL(A.City, '')
		AND ISNULL(tPA.StateProvince, 0) = ISNULL(A.StateProvince, 0)
		AND ISNULL(tPA.County, 0) = ISNULL(A.County, 0)
		AND ISNULL(tPA.Zip, '') = ISNULL(A.Zip, '')
		AND ISNULL(tPA.ComplexName, '') = ISNULL(A.ComplexName, '')
		AND ISNULL(tPA.GateCode, '') = ISNULL(A.GateCode, '');

	UPDATE #tmpPayorAddress
	SET PayorAddressID = PA.PayorAddressID
	FROM
		#tmpPayorAddress tPA
		INNER JOIN Registration.PayorAddress PA
			ON tPA.AddressID = PA.AddressID
			AND PA.PayorAddressID = @PayorAddressID
	WHERE
		tPA.AddressID = PA.AddressID
		AND PA.PayorAddressID = @PayorAddressID;

	IF EXISTS (SELECT TOP 1 * FROM #tmpPayorAddress WHERE PayorAddressID IS NOT NULL)
		BEGIN
			SET @AuditCursor = CURSOR FOR
			SELECT PayorAddressID, ModifiedOn FROM #tmpPayorAddress WHERE PayorAddressID IS NOT NULL;    

			OPEN @AuditCursor 
			FETCH NEXT FROM @AuditCursor 
			INTO @PayorAddressID, @ModifiedOn

			WHILE @@FETCH_STATUS = 0
			BEGIN
			EXEC Core.usp_AddPreAuditLog @ProcName, 'Update', 'Registration', 'PayorAddress', @PayorAddressID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			
			UPDATE #tmpPayorAddress
			SET AuditDetailID = @AuditDetailID
			WHERE
				PayorAddressID = @PayorAddressID;

			FETCH NEXT FROM @AuditCursor 
			INTO @PayorAddressID, @ModifiedOn
			END; 

			CLOSE @AuditCursor;
			DEALLOCATE @AuditCursor;
		END;

	-- Merge statement for Registration.PayorAddress
	DECLARE	@tmpPayorAddress TABLE
	(
		Operation varchar(10),
		PayorAddressID INT,
		ModifiedOn DATETIME
	);

	MERGE INTO Registration.PayorAddress AS TARGET
	USING (SELECT * FROM #tmpPayorAddress t)  AS SOURCE
		ON ISNULL(SOURCE.PayorAddressID, 0) = TARGET.PayorAddressID
	WHEN NOT MATCHED THEN
		INSERT
		(
			PayorPlanID,
			AddressID,
			ContactID,
			ElectronicPayorID,
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
			@PayorPlanID,
			SOURCE.AddressID,
			@ContractID,
			@ElectronicPayorID,
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
		SET TARGET.AddressID = SOURCE.AddressID,
			ContactID = @ContractID,
			ElectronicPayorID = @ElectronicPayorID,
			TARGET.EffectiveDate = SOURCE.EffectiveDate,
			TARGET.ExpirationDate = SOURCE.ExpirationDate,
			TARGET.IsActive = 1,
			TARGET.ModifiedBy = @ModifiedBy,
			TARGET.ModifiedOn = SOURCE.ModifiedOn,
			TARGET.SystemModifiedOn = GETUTCDATE()
	OUTPUT
		$action,
		inserted.PayorAddressID,
		inserted.ModifiedOn
	INTO
		@tmpPayorAddress;	

		SELECT @ID=PayorAddressID FROM @tmpPayorAddress WHERE Operation = 'Insert'

	IF EXISTS (SELECT TOP 1 * FROM @tmpPayorAddress WHERE Operation = 'Insert')
		BEGIN
		BEGIN
			SET @AuditCursor = CURSOR FOR
			SELECT PayorAddressID, ModifiedOn FROM @tmpPayorAddress WHERE Operation = 'Insert';    

			OPEN @AuditCursor 
			FETCH NEXT FROM @AuditCursor 
			INTO @PayorAddressID, @ModifiedOn

			WHILE @@FETCH_STATUS = 0
			BEGIN
			EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Registration', 'PayorAddress', @PayorAddressID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

			EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Registration', 'PayorAddress', @AuditDetailID, @PayorAddressID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

			FETCH NEXT FROM @AuditCursor 
			INTO @PayorAddressID, @ModifiedOn
			END; 

			CLOSE @AuditCursor;
			DEALLOCATE @AuditCursor;
		END;
		END

	IF EXISTS (SELECT TOP 1 * FROM #tmpPayorAddress WHERE PayorAddressID IS NOT NULL)
		BEGIN
		BEGIN
			SET @AuditCursor = CURSOR FOR
			SELECT PayorAddressID, ModifiedOn, AuditDetailID FROM #tmpPayorAddress WHERE PayorAddressID IS NOT NULL;   

			OPEN @AuditCursor 
			FETCH NEXT FROM @AuditCursor 
			INTO @PayorAddressID, @ModifiedOn, @AuditDetailID

			WHILE @@FETCH_STATUS = 0
			BEGIN
			EXEC Auditing.usp_AddPostAuditLog 'Update', 'Registration', 'PayorAddress', @AuditDetailID, @PayorAddressID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

			FETCH NEXT FROM @AuditCursor 
			INTO @PayorAddressID, @ModifiedOn, @AuditDetailID
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