-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_UpdateAddresses]
-- Author:		Demetrios C. Christopher
-- Date:		08/06/2015
--
-- Purpose:		Update modifiable Address Details
--
-- Notes:		n/a (or any additional notes)
--
-- Depends:		n/a
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 08/06/2015 - Demetrios Christopher - Initial draft
-- 09/22/2015   Gurpreet Singh      Changed AptComplexName to ComplexName
-- 01/12/2016	Scott Martin		Added audit logging
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added SystemModifiedOn field
-- 03/07/2016	Kyle Campbell	TFS #5793 Modifed field size for ComplexName and GateCode
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_UpdateAddress]
	@AddressXML XML,
	@ModifiedBy INT,
	@ResultCode INT OUTPUT,
	@ResultMessage NVARCHAR(500) OUTPUT
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
		AddressID BIGINT, 
		AddressTypeID INT, 
		Line1 NVARCHAR(200), 
		Line2 NVARCHAR(200), 
		City NVARCHAR(200), 
		StateProvince INT, 
		County INT, 
		Zip NVARCHAR(10), 
		ComplexName NVARCHAR(255), 
		GateCode NVARCHAR(50),
		IsActive BIT,
		ModifiedOn DATETIME,
		AuditDetailID BIGINT
	)

	INSERT INTO #tmpAddress
	(
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
		IsActive,
		ModifiedOn,
		AuditDetailID
	)
	SELECT 
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
		T.C.value('IsActive[1]', 'BIT'),
		T.C.value('ModifiedOn[1]', 'DATETIME'),
		NULL
	FROM 
		@AddressXML.nodes('RequestXMLValue/Address') AS T(C);

	DECLARE @AuditCursor CURSOR;
	DECLARE @AuditDetailID BIGINT;
	BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT AddressID, ModifiedOn FROM #tmpAddress;    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @AddressID, @ModifiedOn

		WHILE @@FETCH_STATUS = 0
		BEGIN
		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'Addresses', @AddressID, NULL, NULL, NULL, NULL, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE #tmpAddress
		SET AuditDetailID = @AuditDetailID
		WHERE
			AddressID = @AddressId;

		FETCH NEXT FROM @AuditCursor 
		INTO @AddressID, @ModifiedOn
		END; 

		CLOSE @AuditCursor;
		DEALLOCATE @AuditCursor;
	END;

	UPDATE a
	SET AddressTypeID = ta.AddressTypeID,
		Line1 = ta.Line1,
		Line2 = ta.Line2,
		City = ta.City,
		StateProvince = ta.StateProvince,
		County = ta.County,
		Zip = ta.Zip,
		ComplexName = ta.ComplexName,
		GateCode = ta.GateCode,
		IsActive = COALESCE(ta.IsActive, a.IsActive),
		ModifiedBy = @ModifiedBy,
		ModifiedOn = ta.ModifiedOn,
		SystemModifiedOn = GETUTCDATE()
	FROM 
		Core.Addresses a 
		INNER JOIN #tmpAddress ta 
			ON ta.AddressID = a.AddressID;

	BEGIN
		SET @AuditCursor = CURSOR FOR
		SELECT AddressID, ModifiedOn, AuditDetailID FROM #tmpAddress;    

		OPEN @AuditCursor 
		FETCH NEXT FROM @AuditCursor 
		INTO @AddressID, @ModifiedOn, @AuditDetailID

		WHILE @@FETCH_STATUS = 0
		BEGIN
		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'Addresses', @AuditDetailID, @AddressID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		FETCH NEXT FROM @AuditCursor 
		INTO @AddressID, @ModifiedOn, @AuditDetailID
		END; 

		CLOSE @AuditCursor;
		DEALLOCATE @AuditCursor;
	END;
		
	END TRY
	BEGIN CATCH
		SELECT 
				@ResultCode = ERROR_SEVERITY(),
				@ResultMessage =  ERROR_MESSAGE()
	END CATCH
END
