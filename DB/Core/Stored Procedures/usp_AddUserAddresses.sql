-----------------------------------------------------------------------------------------------------------------------
-- Procedure:	[usp_AddUserAddresses]
-- Author:		Justin Spalti
-- Date:		10/01/2015
--
-- Purpose:		Add user addresses
--
-- Notes:		N/A (or any additional notes)
--
-- Depends:		Core.Address, Core.UserAddress
--
-- REVISION HISTORY ---------------------------------------------------------------------------------------------------
-- 10/01/2015	Justin Spalti - Initial creation
-- 01/14/2016	Scott Martin		Added ModifiedOn parameter, Added CreatedOn and CreatedBy fields
-- 03/07/2016	Kyle Campbell	TFS #5793 Modifed field size for ComplexName and GateCode
-- 01/11/2017	Scott Martin	Updated the audit procs to save UserID
-----------------------------------------------------------------------------------------------------------------------

CREATE PROCEDURE [Core].[usp_AddUserAddresses]
	@UserID bigint,
	@AddressesXML xml,
	@ModifiedBy int,
	@ResultCode int OUTPUT,
	@ResultMessage nvarchar(500) OUTPUT
AS
BEGIN
DECLARE @AuditDetailID BIGINT,
		@AuditCursor CURSOR,
		@ModifiedOn DATETIME,
		@AdditionalResultXML XML,
		@IsPrimary BIT = 0,
		@UserAddressID BIGINT,
		@AddressId BIGINT,
		@ProcName VARCHAR(255) = OBJECT_NAME(@@PROCID);

SELECT
    @ResultCode = 0,
    @ResultMessage = 'Executed Successfully';

	BEGIN TRY

	-- iterate through the addresses in the passed in xml, and each time an address is created, also associate the address w/ the user
	IF OBJECT_ID('tempdb..#tmpUserAddress') IS NOT NULL
		DROP TABLE #tmpUserAddress

	CREATE TABLE #tmpUserAddress 
	(
		UserAddressID BIGINT,
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
		ModifiedOn DATETIME,
		AuditDetailID BIGINT
	);

	INSERT INTO #tmpUserAddress 
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
		T.C.value('ModifiedOn[1]', 'DATETIME')
	FROM 
		@AddressesXML.nodes('RequestXMLValue/Address') AS T (C);
	
	EXEC Core.usp_AddAddress @AddressesXML, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AdditionalResultXML OUTPUT;

	UPDATE #tmpUserAddress
	SET AddressID = A.AddressID
	FROM
		#tmpUserAddress tCA
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

	UPDATE #tmpUserAddress
	SET UserAddressID = CA.UserAddressID
	FROM
		#tmpUserAddress tCA
		INNER JOIN Core.UserAddress CA
			ON tCA.AddressID = CA.AddressID
			AND CA.UserID = @UserID
	WHERE
		tCA.AddressID = CA.AddressID
		AND CA.UserID = @UserID;

	--Select statements purposely set this way so the procedure will fail if more than 1 primary address is found
	SET @IsPrimary = (SELECT IsPrimary FROM #tmpUserAddress WHERE IsPrimary = 1);

	SELECT @AddressId = AddressID, @ModifiedOn = ModifiedOn FROM #tmpUserAddress WHERE IsPrimary = 1;

	SET @UserAddressID = (SELECT UserAddressID FROM Core.UserAddress WHERE UserID = @UserID AND AddressID <> @AddressId AND IsPrimary = 1);

	--If the address to be inserted IsPrimary = 1 and there is an existing primary, set the existing primary to 0
	IF @UserAddressID IS NOT NULL AND @IsPrimary = 1
		BEGIN
		EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'UserAddress', @UserAddressID, NULL, NULL, NULL, @UserID, 1, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

		UPDATE Core.UserAddress
		SET IsPrimary = 0,
			ModifiedBy = @ModifiedBy,
			ModifiedOn = @ModifiedOn,
			SystemModifiedOn = GETUTCDATE()
		WHERE	
			UserAddressID = @UserAddressID;

		EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'UserAddress', @AuditDetailID, @UserAddressID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
		END

	IF EXISTS (SELECT TOP 1 * FROM #tmpUserAddress WHERE UserAddressID IS NOT NULL)
		BEGIN
			SET @AuditCursor = CURSOR FOR
			SELECT UserAddressID, ModifiedOn FROM #tmpUserAddress WHERE UserAddressID IS NOT NULL;    

			OPEN @AuditCursor 
			FETCH NEXT FROM @AuditCursor 
			INTO @UserAddressID, @ModifiedOn

			WHILE @@FETCH_STATUS = 0
			BEGIN
			EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Update', 'Core', 'UserAddress', @UserAddressID, NULL, NULL, NULL, @UserID, 1, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;
			
			UPDATE #tmpUserAddress
			SET AuditDetailID = @AuditDetailID
			WHERE
				UserAddressID = @UserAddressID;

			FETCH NEXT FROM @AuditCursor 
			INTO @UserAddressID, @ModifiedOn
			END; 

			CLOSE @AuditCursor;
			DEALLOCATE @AuditCursor;
		END;

	-- Merge statement for Core.UserAddress
	DECLARE	@tmpUserAddress TABLE
	(
		Operation varchar(10),
		UserAddressID INT,
		ModifiedOn DATETIME
	);

	MERGE INTO Core.UserAddress AS TARGET
	USING (SELECT * FROM #tmpUserAddress t)  AS SOURCE
		ON ISNULL(SOURCE.UserAddressID, 0) = TARGET.UserAddressID
	WHEN NOT MATCHED THEN
		INSERT
		(
			UserID,
			AddressID,
			MailPermissionID,
			IsPrimary,
			IsActive,
			ModifiedBy,
			ModifiedOn,
			CreatedBy,
			CreatedOn
		)
		VALUES
		(
			@UserID,
			SOURCE.AddressID,
			SOURCE.MailPermissionID,
			SOURCE.IsPrimary,
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
			TARGET.IsActive = 1,
			TARGET.ModifiedBy = @ModifiedBy,
			TARGET.ModifiedOn = SOURCE.ModifiedOn,
			TARGET.SystemModifiedOn = GETUTCDATE()
	OUTPUT
		$action,
		inserted.UserAddressID,
		inserted.ModifiedOn
	INTO
		@tmpUserAddress;	

	IF EXISTS (SELECT TOP 1 * FROM @tmpUserAddress WHERE Operation = 'Insert')
		BEGIN
		BEGIN
			SET @AuditCursor = CURSOR FOR
			SELECT UserAddressID, ModifiedOn FROM @tmpUserAddress WHERE Operation = 'Insert';    

			OPEN @AuditCursor 
			FETCH NEXT FROM @AuditCursor 
			INTO @UserAddressID, @ModifiedOn

			WHILE @@FETCH_STATUS = 0
			BEGIN
			EXEC Auditing.usp_AddPreAuditLog @ProcName, 'Insert', 'Core', 'UserAddress', @UserAddressID, NULL, NULL, NULL, @UserID, 1, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

			EXEC Auditing.usp_AddPostAuditLog 'Insert', 'Core', 'UserAddress', @AuditDetailID, @UserAddressID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

			FETCH NEXT FROM @AuditCursor 
			INTO @UserAddressID, @ModifiedOn
			END; 

			CLOSE @AuditCursor;
			DEALLOCATE @AuditCursor;
		END;
		END

	IF EXISTS (SELECT TOP 1 * FROM #tmpUserAddress WHERE UserAddressID IS NOT NULL)
		BEGIN
		BEGIN
			SET @AuditCursor = CURSOR FOR
			SELECT UserAddressID, ModifiedOn, AuditDetailID FROM #tmpUserAddress WHERE UserAddressID IS NOT NULL;   

			OPEN @AuditCursor 
			FETCH NEXT FROM @AuditCursor 
			INTO @UserAddressID, @ModifiedOn, @AuditDetailID

			WHILE @@FETCH_STATUS = 0
			BEGIN
			EXEC Auditing.usp_AddPostAuditLog 'Update', 'Core', 'UserAddress', @AuditDetailID, @UserAddressID, NULL, @ModifiedOn, @ModifiedBy, @ResultCode OUTPUT, @ResultMessage OUTPUT, @AuditDetailID OUTPUT;

			FETCH NEXT FROM @AuditCursor 
			INTO @UserAddressID, @ModifiedOn, @AuditDetailID
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