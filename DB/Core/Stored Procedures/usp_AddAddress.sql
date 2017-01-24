-----------------------------------------------------------------------------------------------------------------------
-- Procedure:  [usp_AddAddress]
-- Author:		Demetrios C. Christopher
-- Date:		08/06/2015
--
-- Purpose:		Add Address Details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/06/2015 - Demetrios Christopher - Initial draft
-- 09/22/2015   John Crossen        Add Audit
-- 09/22/2015   Gurpreet Singh      Changed AptComplexName to ComplexName
-- 01/12/2016	Scott Martin		Changed Insert to a Merge statement to prevent exact duplicates
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added CreatedOn and CreatedBy fields
-- 11/04/2016	Scott Martin		Added a TOP 1 to the return xml query
-- 12/30/2016	Scott Martin		Removed TOP 1 and replaced with a group by in order to return IDs for multiple addresses
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_AddAddress]
	@AddressXML xml,
	@ModifiedBy INT,
	@ResultCode int OUTPUT,
	@ResultMessage nvarchar(500) OUTPUT,
	@AdditionalResult XML OUTPUT
AS
BEGIN
DECLARE @AddressId INT,
		@ModifiedOn DATETIME,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

	SELECT @ResultCode = 0,
			@ResultMessage = 'Executed Successfully'

	BEGIN TRY
	IF OBJECT_ID('tempdb..#tmpAddress') IS NOT NULL
		DROP TABLE #tmpAddress

	CREATE TABLE #tmpAddress 
	(
		AddressTypeID int,
		Line1 nvarchar(200),
		Line2 nvarchar(200),
		City nvarchar(200),
		StateProvince int,
		County int,
		Zip nvarchar(10),
		ComplexName nvarchar(255),
		GateCode nvarchar(50),
		ModifiedOn DATETIME
	);

	INSERT INTO #tmpAddress 
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
		T.C.value('ModifiedOn[1]', 'DATETIME')
	FROM 
		@AddressXML.nodes('RequestXMLValue/Address') AS T (C);

	DECLARE	@tmpAddressesCreated TABLE
	(
		Operation varchar(10),
		AddressID INT,
		ModifiedOn DATETIME
	);

	MERGE INTO Core.Addresses AS TARGET
	USING ( SELECT DISTINCT AddressTypeID, Line1, Line2, City, StateProvince, County, Zip, ComplexName, GateCode, ModifiedOn FROM #tmpAddress t ) AS SOURCE
		ON ISNULL(SOURCE.AddressTypeID, 0) = ISNULL(TARGET.AddressTypeID, 0)
		AND	ISNULL(SOURCE.Line1, '') = ISNULL(TARGET.Line1, '')
		AND ISNULL(SOURCE.Line2, '') = ISNULL(TARGET.Line2, '')
		AND ISNULL(SOURCE.City, '') = ISNULL(TARGET.City, '')
		AND ISNULL(SOURCE.StateProvince, 0) = ISNULL(TARGET.StateProvince, 0)
		AND ISNULL(SOURCE.County, 0) = ISNULL(TARGET.County, 0)
		AND ISNULL(SOURCE.Zip, '') = ISNULL(TARGET.Zip, '')
		AND ISNULL(SOURCE.ComplexName, '') = ISNULL(TARGET.ComplexName, '')
		AND ISNULL(SOURCE.GateCode, '') = ISNULL(TARGET.GateCode, '')
	WHEN NOT MATCHED THEN
		INSERT
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
			IsActive,
			ModifiedBy,
			ModifiedOn,
			CreatedBy,
			CreatedOn
		)
		VALUES
		(
			SOURCE.AddressTypeID,
			SOURCE.Line1,
			SOURCE.Line2,
			SOURCE.City,
			SOURCE.StateProvince,
			SOURCE.County,
			SOURCE.Zip,
			SOURCE.ComplexName,
			SOURCE.GateCode,
			1,
			@ModifiedBy,
			SOURCE.ModifiedOn,
			@ModifiedBy,
			SOURCE.ModifiedOn
		)
	WHEN MATCHED THEN
		UPDATE
		SET @AddressID = TARGET.AddressID
	OUTPUT
		$action,
		inserted.AddressID,
		inserted.ModifiedOn
	INTO
		@tmpAddressesCreated;

	SELECT @AdditionalResult = (
		SELECT
			MAX(tAC.AddressID) AS Identifier
		FROM
			@tmpAddressesCreated tAC
			INNER JOIN Core.Addresses A
				ON tAC.AddressID = A.AddressID
		GROUP BY
			ISNULL(AddressTypeID, 0),
			ISNULL(Line1, ''),
			ISNULL(Line2, ''),
			ISNULL(City, ''),
			ISNULL(StateProvince, 0),
			ISNULL(County, 0),
			ISNULL(Zip, ''),
			ISNULL(ComplexName, ''),
			ISNULL(GateCode, '')
		FOR XML PATH('OutParameters'), TYPE);
		
	IF EXISTS (SELECT TOP 1 * FROM @tmpAddressesCreated WHERE Operation = 'Insert')
		BEGIN
		DECLARE @AuditCursor CURSOR;
		DECLARE @AuditDetailID BIGINT;
		BEGIN
			SET @AuditCursor = CURSOR FOR
			SELECT AddressID, ModifiedOn FROM @tmpAddressesCreated WHERE Operation = 'Insert';    

			OPEN @AuditCursor 
			FETCH NEXT FROM @AuditCursor 
			INTO @AddressID, @ModifiedOn

			WHILE @@FETCH_STATUS = 0
			BEGIN
			EXEC Core.usp_AddPreAuditLog @ProcName, 'Insert', 'Core', 'Addresses', @AddressID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

			EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Core', 'Addresses', @AuditDetailID, @AddressID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

			FETCH NEXT FROM @AuditCursor 
			INTO @AddressID, @ModifiedOn
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